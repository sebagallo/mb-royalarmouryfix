using System;
using System.Security.Cryptography;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace RoyalArmouryFix
{
    public class RoyalArmoury : CampaignBehaviorBase
    {
        private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
        {
            string menuText = new TextObject("{=kRA_menu_access}Access The Royal Armoury").ToString();
            campaignGameStarter.AddGameMenuOption("town_keep", "armoury", menuText, generic_access_on_condition,
                armory_on_consequence);
        }

        private bool generic_access_on_condition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Trade;
            return true;
        }


        private void armory_on_consequence(MenuCallbackArgs args)
        {
            Clan OwnerClan = Settlement.CurrentSettlement.OwnerClan;

            string sDisplayMessageTitle = "",
                sDisplayMessageBody = "";

            float fCostOfAdmissionGold = 0f,
                fCostOfAdmissionInfluence = 0f;

            bool bPlayerRulesTown = false,
                bPlayerIsVassal = false,
                bPlayerIsMercenary = false,
                bShowEntryOption = true;

            if (OwnerClan.Leader == Hero.MainHero)
            {
                bPlayerRulesTown = true;
            }
            else if (OwnerClan.Kingdom == Clan.PlayerClan.Kingdom && Clan.PlayerClan.Kingdom != null)
            {
                if (Hero.MainHero == OwnerClan.Kingdom.Leader) //Player is the ruler of the kingdom
                {
                    bPlayerRulesTown = true;
                }
                else
                {
                    if (Clan.PlayerClan.IsUnderMercenaryService)
                    {
                        bPlayerIsMercenary = true;
                    }
                    else
                    {
                        bPlayerIsVassal = true;
                    }
                }
            }

            // Influence fee calculation
            if ((bPlayerIsVassal && !Settings.Instance.bVassalPaysWithGoldToo) || (bPlayerRulesTown &&
                Settings.Instance.bEvenRulersShouldPay && !Settings.Instance.bRulersPayGold))
            {
                fCostOfAdmissionInfluence = Settings.Instance.bIgnoreTownProsperityForAdmission
                    ? Settings.Instance.iFlatAdmissionCostInfluence
                    : Settlement.CurrentSettlement.Prosperity / 100;

                if (bPlayerRulesTown)
                {
                    fCostOfAdmissionInfluence *= Settings.Instance.fPercentageOfCostForRuler;
                }
            }

            // Gold fee calculation            
            if ((!bPlayerRulesTown && !bPlayerIsVassal) ||
                (bPlayerIsVassal && Settings.Instance.bVassalPaysWithGoldToo) || (bPlayerRulesTown &&
                    Settings.Instance.bEvenRulersShouldPay && Settings.Instance.bRulersPayGold))
            {
                fCostOfAdmissionGold = Settings.Instance.bIgnoreTownProsperityForAdmission
                    ? Settings.Instance.iFlatAdmissionCostGold
                    : Settlement.CurrentSettlement.Prosperity;

                if (bPlayerIsMercenary)
                {
                    fCostOfAdmissionGold *= Settings.Instance.fPercentageOfGoldForMercenary;
                }

                if (bPlayerIsVassal)
                {
                    fCostOfAdmissionGold *= Settings.Instance.fPercentageOfGoldForVassal;
                }

                if (bPlayerRulesTown)
                {
                    fCostOfAdmissionGold *= Settings.Instance.fPercentageOfCostForRuler;
                }
            }


            if (Settings.Instance.bRelationshipModifiesAdmissionCost && OwnerClan.Leader != Hero.MainHero)
            {
                fCostOfAdmissionGold -= fCostOfAdmissionGold * (int) OwnerClan.Leader.GetRelationWithPlayer() / 100;
                fCostOfAdmissionInfluence -=
                    fCostOfAdmissionInfluence * (int) OwnerClan.Leader.GetRelationWithPlayer() / 100;
            }

            fCostOfAdmissionGold -= fCostOfAdmissionGold % 100; // Clean up the entry fee in gold cost to be in hundreds

            int iCostOfAdmissionGold = fCostOfAdmissionGold > 0
                ? (int) Math.Min(Settings.Instance.iMaxCostOfAdmissionGold,
                    Math.Max(fCostOfAdmissionGold, Settings.Instance.iMinCostOfAdmissionGold))
                : 0;
            int iCostOfAdmissionInfluence = fCostOfAdmissionInfluence > 0
                ? (int) Math.Min(Settings.Instance.iMaxCostOfAdmissionInfluence,
                    Math.Max(fCostOfAdmissionInfluence, Settings.Instance.iMinCostOfAdmissionInfluence))
                : 0;


            if (bPlayerRulesTown)
            {
                sDisplayMessageTitle += new TextObject("{=kRA_welcome_ruler}Welcome to The Royal Armoury, Mylord!\n")
                    .ToString();

                if (!Settings.Instance.bEvenRulersShouldPay)
                {
                    iCostOfAdmissionGold = 0;
                    iCostOfAdmissionInfluence = 0;
                }
            }

            if (Settings.Instance.bProsperityAffectsItemSelection && (!Settings.Instance.bUnlimitedSelectionIfFree ||
                                                                      iCostOfAdmissionGold > 0 ||
                                                                      iCostOfAdmissionInfluence > 0))
            {
                var selection =
                    new TextObject(
                        "{=kRA_selection}The item selection variety is expected to be {SEL} ({PERCENT}).\n \n");

                int iSelectionPercentage =
                    (int) ((Settlement.CurrentSettlement.Prosperity - Settings.Instance.iMinProsperityForAnyItem) /
                           (Settings.Instance.iProsperityForGuaranteedItem -
                            Settings.Instance.iMinProsperityForAnyItem) *
                           100);
                iSelectionPercentage = Math.Min(100, Math.Max(0, iSelectionPercentage));

                var sel = new TextObject(
                    "{=kRA_sel_none}non existent, as besides some scrawny mice, the Armory is mostly empty");
                switch (iSelectionPercentage)
                {
                    case int i when i >= 100:
                        sel = new TextObject("{=kRA_sel_100}Impeccable");
                        break;
                    case int i when (i >= 80 && i < 100):
                        sel = new TextObject("{=kRA_sel_80}Excellent");
                        break;
                    case int i when (i >= 60 && i < 80):
                        sel = new TextObject("{=kRA_sel_60}Above Average");
                        break;
                    case int i when (i >= 40 && i < 60):
                        sel = new TextObject("{=kRA_sel_40}Average");
                        break;
                    case int i when (i >= 20 && i < 40):
                        sel = new TextObject("{=kRA_sel_20}Below Average");
                        break;
                    case int i when (i >= 10 && i < 20):
                        sel = new TextObject("{=kRA_sel_10}Poor");
                        break;
                    case int i when (i > 0 && i < 10):
                        sel = new TextObject("{=kRA_sel_0}Abysmal");
                        break;
                    case int i when (i <= 0):
                        break;
                }

                selection.SetTextVariable("SEL", sel);
                selection.SetTextVariable("PERCENT", iSelectionPercentage);
                sDisplayMessageBody += selection.ToString();
            }

            if (iCostOfAdmissionGold == 0 && iCostOfAdmissionInfluence == 0)
            {
                if (!bPlayerRulesTown)
                {
                    sDisplayMessageTitle += new TextObject("{=kRA_access_free}You can freely enter, sir.");
                }
            }
            else
            {
                if (bPlayerIsMercenary)
                {
                    sDisplayMessageTitle += new TextObject("{=kRA_access_merc}You are a mercenary for the kingdom.\n");
                }

                if (bPlayerIsVassal)
                {
                    sDisplayMessageTitle += new TextObject("{=kRA_access_vassal}You are a vassal of the kingdom.\n");
                }

                sDisplayMessageTitle += new TextObject("{=kRA_access_cost}Accessing The Royal Armoury costs ");

                if (iCostOfAdmissionGold > 0)
                {
                    sDisplayMessageTitle +=
                        iCostOfAdmissionGold.ToString() + new TextObject("{=kRA_access_cost_g} gold");
                }

                if (iCostOfAdmissionInfluence > 0)
                {
                    sDisplayMessageTitle += iCostOfAdmissionInfluence.ToString() +
                                            new TextObject("{=kRA_access_cost_i} influence");
                }

                if ((Hero.MainHero.Gold < iCostOfAdmissionGold && iCostOfAdmissionGold > 0) ||
                    (Hero.MainHero.Clan.Influence < iCostOfAdmissionInfluence && iCostOfAdmissionInfluence > 0))
                {
                    if (bPlayerRulesTown)
                    {
                        sDisplayMessageBody +=
                            new TextObject(
                                "{=kRA_cannot_enter_ruler}This is really embarassing, Sire, alas I cannot let You in.\n \n");
                    }
                    else
                    {
                        sDisplayMessageBody +=
                            new TextObject("{=kRA_cannot_enter}You cannot afford to enter now.\n \n");
                    }

                    bShowEntryOption = false;
                }

                if (Settings.Instance.bRelationshipModifiesAdmissionCost && OwnerClan.Leader != Hero.MainHero)
                {
                    var text = new TextObject(
                        "{=kRA_cost_info}To lower the costs (gold or influence) improve your relations with {CLAN_LEADER_NAME} of clan {CLAN_NAME} (currently at {CLAN_LEADER_RELATION}). \n");
                    text.SetTextVariable("CLAN_NAME", OwnerClan.Name);
                    text.SetTextVariable("CLAN_LEADER_NAME", OwnerClan.Leader.Name);
                    text.SetTextVariable("CLAN_LEADER_GENDER", OwnerClan.Leader.IsFemale ? 1 : 0);
                    text.SetTextVariable("CLAN_LEADER_RELATION", OwnerClan.Leader.GetRelationWithPlayer().ToString());
                    text.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"Icons\\Influence@2x\">");
                    sDisplayMessageBody += text;
                }

                TextObject discount;
                if (Settings.Instance.fPercentageOfGoldForMercenary < 1)
                {
                    discount = new TextObject(
                        "{=kRA_disc_merc_yes}Discounts are available to mercenaries (pay only {COST} %). ");
                }
                else
                {
                    discount = new TextObject("{=kRA_disc_merc_no}Mercenaries pay {COST} %. ");
                }

                discount.SetTextVariable("COST", Settings.Instance.fPercentageOfGoldForMercenary * 100);
                sDisplayMessageBody += discount;


                if (!Settings.Instance.bVassalPaysWithGoldToo)
                {
                    sDisplayMessageBody +=
                        new TextObject("{=kRA_vassal_pay_i}As a vassal, you would pay with influence. ");
                }
                else
                {
                    sDisplayMessageBody += new TextObject("{=kRA_vassal_pay_g}As a vassal, you would pay with gold. ");

                    if (Settings.Instance.fPercentageOfGoldForVassal < 1)
                    {
                        discount = new TextObject(
                            "{=kRA_disc_vassal_yes}Discounts are available to vassals (pay only {COST} %). ");
                    }
                    else
                    {
                        discount = new TextObject("{=kRA_disc_vassal_no}Vassals pay {COST} %. ");
                    }

                    discount.SetTextVariable("COST", Settings.Instance.fPercentageOfGoldForVassal * 100);
                    sDisplayMessageBody += discount;
                }


                int prosperity = 0;
                if (Settings.Instance.bIgnoreTownProsperityForAdmission)
                {
                    sDisplayMessageBody +=
                        new TextObject("{=kRA_pros_fee_no}Town prosperity has NO effect on the entry fee, ");
                }
                else
                {
                    sDisplayMessageBody += new TextObject("{=kRA_pros_fee_yes}Town prosperity affects the entry fee, ");
                    prosperity = 1;
                }

                var variety =
                    new TextObject("{=kRA_pros_sel}and does {EFFECTS_VAR_TEXT}affect item selection variety. ");
                variety.SetTextVariable("EFFECTS_ENTRY", prosperity);

                if (Settings.Instance.bProsperityAffectsItemSelection)
                {
                    variety.SetTextVariable("EFFECTS_VAR", 1);
                    variety.SetTextVariable("EFFECTS_VAR_TEXT", "");
                }
                else
                {
                    variety.SetTextVariable("EFFECTS_VAR", 0);
                    variety.SetTextVariable("EFFECTS_VAR_TEXT", new TextObject("{=kRA_pros_sel_not}NOT ").ToString());
                }

                sDisplayMessageBody += variety;

                if (!Settings.Instance.bEvenRulersShouldPay)
                {
                    sDisplayMessageBody +=
                        new TextObject("{=kRA_fee_ruler}\nUltimately, ruling the town grants free entry. ");
                }
            }

            InformationManager.ShowInquiry(
                new InquiryData(
                    sDisplayMessageTitle,
                    sDisplayMessageBody,
                    bShowEntryOption, true,
                    new TextObject("{=kRA_enter}Enter").ToString(),
                    new TextObject("{=kRA_leave}Leave").ToString(),
                    () =>
                    {
                        Hero.MainHero.ChangeHeroGold(-iCostOfAdmissionGold);
                        Hero.MainHero.Clan.Influence -= iCostOfAdmissionInfluence;

                        ItemRoster armoury = new ItemRoster();

                        MD5 md5Hasher = MD5.Create();
                        var hashed =
                            md5Hasher.ComputeHash(
                                System.Text.Encoding.UTF8.GetBytes(Settlement.CurrentSettlement.Name.ToString()));
                        var iRandSeed = BitConverter.ToInt32(hashed, 0);
                        iRandSeed += (int) (Settlement.CurrentSettlement.Prosperity /
                                            Settings.Instance.iProsperityChangeNeededForNewStock);

                        Random random = new Random(Settings.Instance.bPreserveRandomSeedForStock
                            ? iRandSeed
                            : (int) DateTime.Now.Ticks);

                        foreach (ItemObject item in ItemObject.All)
                        {
                            if (!item.IsCraftedByPlayer || Settings.Instance.bShowCraftedItems)
                            {
                                if ((item.Culture == Settlement.CurrentSettlement.Culture ||
                                     !Settings.Instance.bFilterByCulture ||
                                     (item.Culture == null &&
                                      Settings.Instance.bShowCultureMissingItems)) // Culture filters
                                    &&
                                    (
                                        (Settings.Instance.bCostOrValueForItemEnough &&
                                         (item.Value >= Settings.Instance.iLowestValueItemInArmory || item.Appearance >=
                                             Settings.Instance
                                                 .fLowestAppearanceItemInArmory)) // Cost OR Value is enough?
                                        ||
                                        (!Settings.Instance.bCostOrValueForItemEnough &&
                                         (item.Value >= Settings.Instance.iLowestValueItemInArmory && item.Appearance >=
                                             Settings.Instance
                                                 .fLowestAppearanceItemInArmory)) // Cost AND value is needed?
                                    )
                                )
                                {
                                    if ((Settings.Instance.bUnlimitedSelectionIfFree && iCostOfAdmissionGold == 0 &&
                                         iCostOfAdmissionInfluence == 0) ||
                                        !Settings.Instance.bProsperityAffectsItemSelection)
                                    {
                                        armoury.AddToCounts(item, 99);
                                    }
                                    else
                                    {
                                        float fItemChanceToAppearBasedOnValueAndRarity = Math.Max(1,
                                            Math.Min(
                                                (item.Appearance - Settings.Instance.fLowestAppearanceItemInArmory) *
                                                (item.Value - Settings.Instance.iLowestValueItemInArmory), 500000) /
                                            1000); // normalize to a number between 1 to 500
                                        int iHalfOfProsperityDifference =
                                            (Settings.Instance.iProsperityForGuaranteedItem -
                                             Settings.Instance.iMinProsperityForAnyItem) / 2;
                                        fItemChanceToAppearBasedOnValueAndRarity *= iHalfOfProsperityDifference / 500;

                                        int iRandMax = Settings.Instance.iProsperityForGuaranteedItem -
                                                       (Settings.Instance.bItemValueAndRarityAffectsChanceToShow
                                                           ? iHalfOfProsperityDifference
                                                           : 0);

                                        int iItemChanceToAppear =
                                            random.Next(Settings.Instance.iMinProsperityForAnyItem, iRandMax);

                                        if (Settings.Instance.bItemValueAndRarityAffectsChanceToShow)
                                        {
                                            iItemChanceToAppear += (int) fItemChanceToAppearBasedOnValueAndRarity;
                                        }

                                        if (Settlement.CurrentSettlement.Prosperity >= iItemChanceToAppear)
                                        {
                                            float fMaxItemsPossible =
                                                Settings.Instance.bRandomizeQuantityBasedOnProsperity
                                                    ? (Settlement.CurrentSettlement.Prosperity / 1000)
                                                    : Settings.Instance.iItemAvailableQuantity;
                                            float fItemsToAdd = Settings.Instance.bRandomizeItemAvailableQuantity
                                                ? random.Next(1, (int) fMaxItemsPossible + 1)
                                                : Settings.Instance.iItemAvailableQuantity;
                                            fItemsToAdd += Settings.Instance.bRandomizeQuantityBasedOnProsperity
                                                ? (Settlement.CurrentSettlement.Prosperity / 4000)
                                                : 0;

                                            if (Settings.Instance.bItemValueAndRarityAffectsChanceToShow)
                                            {
                                                fItemsToAdd = Math.Max(1,
                                                    fItemsToAdd / Math.Max(1, item.Appearance * item.Value / 100000));
                                            }

                                            armoury.AddToCounts(item, (int) fItemsToAdd);
                                        }
                                    }
                                }
                            }
                        }

                        InventoryManager.OpenScreenAsTrade(armoury, Settlement.CurrentSettlement.Town);
                    },
                    null,
                    ""),
                true);
        }

        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this,
                new Action<CampaignGameStarter>(this.OnSessionLaunched));
        }

        public override void SyncData(IDataStore dataStore)
        {
        }
    }
}