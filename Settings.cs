using ModLib.Definitions;
using ModLib.Definitions.Attributes;
using System.Xml.Serialization;

namespace RoyalArmouryFix
{
    public class Settings : SettingsBase
    {
        public const string InstanceID = "RoyalArmouryFixSettings";
        public override string ModName => "Royal Armoury Fix";
        public override string ModuleFolderName => "RoyalArmouryFix";
        [XmlElement]
        public override string ID { get; set; } = InstanceID;

        public static Settings Instance
        {
            get
            {
                return (Settings)SettingsDatabase.GetSettings<Settings>();
            }
        }
        /* Localized settings. However, localization is not supported by ModLib yet (only MCM), so for maximum compatibility menu localization is not yet supported. Once it is supported, uncomment and use this section
        #region Localized settings section
        #region Filtering The Armory's Item Selection
        [XmlElement]
        [SettingProperty("{=kRA_op_FSv}Filtering The Armory's Item Selection", "{=kRA_op_FSvI}Enables the filtering of the items shown in the Armory")]
        [SettingPropertyGroup("{=kRA_op_FS}Filtering The Armory's Item Selection", true)]
        public bool bItemQualityTweaksEnabled { get; set; } = false;

        [XmlElement]
        [SettingProperty("{=kRA_op_FSg}Lowest COST of items to show in Armory (in GOLD)", 0, 99999, "{=kRA_op_FSgI}This is the lowest cost of an item to be still shown")]
        [SettingPropertyGroup("{=kRA_op_FS}Filtering The Armory's Item Selection")]
        public int iLowestValueItemInArmory { get; set; } = 10001;

        [XmlElement]
        [SettingProperty("{=kRA_op_FSs}Show item if either COST OR RARITY condition is met", "{=kRA_op_FSsI}If false, then both Cost AND Rarity condition must be met to show.")]
        [SettingPropertyGroup("{=kRA_op_FS}Filtering The Armory's Item Selection")]
        public bool bCostOrValueForItemEnough { get; set; } = true;

        [XmlElement]
        [SettingProperty("{=kRA_op_FSr}Lowest RARITY items to show (APPEARANCE)", 0f, 3f, "{=kRA_op_FSrI}This is the rarity(called appearance) of an item to be still shown")]
        [SettingPropertyGroup("{=kRA_op_FS}Filtering The Armory's Item Selection")]
        public float fLowestAppearanceItemInArmory { get; set; } = 1.01f;

        [XmlElement]
        [SettingProperty("{=kRA_op_FSc}Filter items by Town's culture", "{=kRA_op_FScI}Do not show items from different cultures (than current Town's culture).")]
        [SettingPropertyGroup("{=kRA_op_FS}Filtering The Armory's Item Selection")]
        public bool bFilterByCulture { get; set; } = true;

        [XmlElement]
        [SettingProperty("{=kRA_op_FSn}Show neutral culture items (in all Towns)", "{=kRA_op_FSnI}Also show items that have no culture tag defined.")]
        [SettingPropertyGroup("{=kRA_op_FS}Filtering The Armory's Item Selection")]
        public bool bShowCultureMissingItems { get; set; } = true;

        [XmlElement]
        [SettingProperty("{=kRA_op_FSu}Unlimited item selection and quantity if entry was free", "{=kRA_op_FSuI}Should free entry automatically mean unlimited item selection?")]
        [SettingPropertyGroup("{=kRA_op_FS}Filtering The Armory's Item Selection")]
        public bool bUnlimitedSelectionIfFree { get; set; } = false;

        [XmlElement]
        [SettingProperty("{=kRA_op_FSi}Include Crafted Weapons (DO NOT UNCHECK)", "{=kRA_op_FSiI}FAULTY FUNCTION BY TW, IT WILL FILTER OUT ALL WEAPONS IF UNCHECKED. Ideally it should include/not include Player crafted items into Armory selection, but does not work currently.")]
        [SettingPropertyGroup("{=kRA_op_FS}Filtering The Armory's Item Selection")]
        public bool bShowCraftedItems { get; set; } = true;

        [XmlElement]
        [SettingProperty("{=kRA_op_FSTp}Town prosperity affects stock variety", "{=kRA_op_FSTpI}The more prosperous the Town is, the more varied items will spawn and vice-versa.")]
        [SettingPropertyGroup("{=kRA_op_FS}Filtering The Armory's Item Selection/{=kRA_op_FST}Town prosperity affects selection variety", true)]
        public bool bProsperityAffectsItemSelection { get; set; } = true;

        [XmlElement]
        [SettingProperty("{=kRA_op_FSTm}MINIMUM PROSPERITY needed for any item to spawn", 0, 15000, "{=kRA_op_FSTmI}Prosperity for any item to be available, ie. no item will shown in Town's below this prosperity level")]
        [SettingPropertyGroup("{=kRA_op_FS}Filtering The Armory's Item Selection/{=kRA_op_FST}Town prosperity affects selection variety")]
        public int iMinProsperityForAnyItem { get; set; } = 1000;

        [XmlElement]
        [SettingProperty("{=kRA_op_FSTw}PROSPERITY needed for the WHOLE SELECTION to show", 0, 15000, "{=kRA_op_FSTwI}Town prosperity needed for all items to be guaranteed")]
        [SettingPropertyGroup("{=kRA_op_FS}Filtering The Armory's Item Selection/{=kRA_op_FST}Town prosperity affects selection variety")]
        public int iProsperityForGuaranteedItem { get; set; } = 10000;

        [XmlElement]
        [SettingProperty("{=kRA_op_FSa}Item's value and rarity affects appearance chances", "{=kRA_op_FSaI}The more expensive or rare the item is, the less chance it has to appear in stock")]
        [SettingPropertyGroup("{=kRA_op_FS}Filtering The Armory's Item Selection")]
        public bool bItemValueAndRarityAffectsChanceToShow { get; set; } = true;
        #endregion


        #region Changing The Quantity Of Items In The Armory
        [XmlElement]
        [SettingProperty("{=kRA_op_CQv}Enable changing the QUANTITY of items shown in the Armory", "{=kRA_op_CQvI}How many items should show up in the Armory")]
        [SettingPropertyGroup("{=kRA_op_CQ}Changing The Quantity Of Items In The Armory", true)]
        public bool bItemQuantityTweaksEnabled { get; set; } = false;

        [XmlElement]
        [SettingProperty("{=kRA_op_CQRv}Randomize quantity", "{=kRA_op_CQRvI}Randomize the quantity of available items")]
        [SettingPropertyGroup("{=kRA_op_CQ}Changing The Quantity Of Items In The Armory/{=kRA_op_CQR}Randomize quantity", true)]
        public bool bRandomizeItemAvailableQuantity { get; set; } = true;

        [XmlElement]
        [SettingProperty("{=kRA_op_CQRp}Randomize based on prosperity", "{=kRA_op_CQRpI}More prosperus Towns will have more items. If not set, quantity is truly random (within set parameters).")]
        [SettingPropertyGroup("{=kRA_op_CQ}Changing The Quantity Of Items In The Armory/{=kRA_op_CQR}Randomize quantity")]
        public bool bRandomizeQuantityBasedOnProsperity { get; set; } = true;

        [XmlElement]
        [SettingProperty("{=kRA_op_CQq}Quantity of available items", 1, 99, "{=kRA_op_CQqI}If quantity is randomized, this is the max possible quantity")]
        [SettingPropertyGroup("{=kRA_op_CQ}Changing The Quantity Of Items In The Armory")]
        public int iItemAvailableQuantity { get; set; } = 3;
        #endregion


        #region Admission Costs
        [XmlElement]
        [SettingProperty("{=kRA_op_ACc}Enable changing the COST OF ADMISSION", "{=kRA_op_ACcI}Change the entry fee costs into the Royal Armory")]
        [SettingPropertyGroup("{=kRA_op_AC}Admission Costs", true)]
        public bool bAdmissionCosts { get; set; } = false;

        [XmlElement]
        [SettingProperty("{=kRA_op_ACRv}Even Rulers should pay", "{=kRA_op_ACRvI}You have to pay even if you own the town or own the kingdom as king or queen.")]
        [SettingPropertyGroup("{=kRA_op_AC}Admission Costs/{=kRA_op_ACR}Even Rulers should pay", true)]
        public bool bEvenRulersShouldPay { get; set; } = false;

        [XmlElement]
        [SettingProperty("{=kRA_op_ACRg}Rulers pay with GOLD instead of influence", "{=kRA_op_ACRgI}By default influence paymant would be required")]
        [SettingPropertyGroup("{=kRA_op_AC}Admission Costs/{=kRA_op_ACR}Even Rulers should pay")]
        public bool bRulersPayGold { get; set; } = false;

        [XmlElement]
        [SettingProperty("{=kRA_op_ACRv}Value multiplier for Rulers (1=100%)", 0, 2, "{=kRA_op_ACRvI}0.1 means Rulers pay only 10% gold")]
        [SettingPropertyGroup("{=kRA_op_AC}Admission Costs/{=kRA_op_ACR}Even Rulers should pay")]
        public float fPercentageOfCostForRuler { get; set; } = 0.1f;

        [XmlElement]
        [SettingProperty("{=kRA_op_ACr}Relationship with Town's owner matters", "{=kRA_op_ACrI}Relationship with the Town's owner modifies the admission costs, for better or worse.")]
        [SettingPropertyGroup("{=kRA_op_AC}Admission Costs")]
        public bool bRelationshipModifiesAdmissionCost { get; set; } = true;

        [XmlElement]
        [SettingProperty("{=kRA_op_ACTv}Ignore Town's prosperity for admission cost", "{=kRA_op_ACTvI}Use flat value instead of Town's prosperity to calculate the admission costs,")]
        [SettingPropertyGroup("{=kRA_op_AC}Admission Costs/{=kRA_op_ACT}Ignore Town Prosperity and use flat costs", true)]
        public bool bIgnoreTownProsperityForAdmission { get; set; } = false;

        [XmlElement]
        [SettingProperty("{=kRA_op_ACTg}Flat admission cost in GOLD", 0, 99999, "{=kRA_op_ACTgI}Will use this flat cost istead of prosperity")]
        [SettingPropertyGroup("{=kRA_op_AC}Admission Costs/{=kRA_op_ACT}Ignore Town Prosperity and use flat costs")]
        public int iFlatAdmissionCostGold { get; set; } = 5000;

        [XmlElement]
        [SettingProperty("{=kRA_op_ACTi}Flat admission cost in INFLUENCE (Vassals)", 0, 999, "{=kRA_op_ACTiI}Will use this flat cost istead of prosperity")]
        [SettingPropertyGroup("{=kRA_op_AC}Admission Costs/{=kRA_op_ACT}Ignore Town Prosperity and use flat costs")]
        public int iFlatAdmissionCostInfluence { get; set; } = 50;

        [XmlElement]
        [SettingProperty("{=kRA_op_ACmg}Minimum cost of entry in GOLD", 0, 99999, "{=kRA_op_ACmgI}Player needs to pay at least this amount")]
        [SettingPropertyGroup("{=kRA_op_AC}Admission Costs")]
        public int iMinCostOfAdmissionGold { get; set; } = 0;

        [XmlElement]
        [SettingProperty("{=kRA_op_ACmxg}Maximum cost of entry in GOLD", 0, 99999, "{=kRA_op_ACmxgI}Player should not pay more than this")]
        [SettingPropertyGroup("{=kRA_op_AC}Admission Costs")]
        public int iMaxCostOfAdmissionGold { get; set; } = 20000;

        [XmlElement]
        [SettingProperty("{=kRA_op_ACmi}Minimum cost of entry in INFLUENCE", 0, 999, "{=kRA_op_ACmiI}Vassals (or even Rulers if selected) pay in influence, at least this amount.")]
        [SettingPropertyGroup("{=kRA_op_AC}Admission Costs")]
        public int iMinCostOfAdmissionInfluence { get; set; } = 0;

        [XmlElement]
        [SettingProperty("{=kRA_op_ACmxi}Maximum cost of entry in INFLUENCE", 0, 999, "{=kRA_op_ACmxiI}Vassals (or even Rulers if selected) pay in influence, max this amount")]
        [SettingPropertyGroup("{=kRA_op_AC}Admission Costs")]
        public int iMaxCostOfAdmissionInfluence { get; set; } = 200;

        [XmlElement]
        [SettingProperty("{=kRA_op_ACm}Gold value multiplier for Mercenaries (1=100%)", 0, 2, "{=kRA_op_ACmI}0.25 means mercenaries pay only 25% gold")]
        [SettingPropertyGroup("{=kRA_op_AC}Admission Costs")]
        public float fPercentageOfGoldForMercenary { get; set; } = 0.25f;

        [XmlElement]
        [SettingProperty("{=kRA_op_ACVv}Use GOLD for Vassals instead of influence", "{=kRA_op_ACVvI}Pay with gold (instead of influence) even as a Vassal")]
        [SettingPropertyGroup("{=kRA_op_AC}Admission Costs/{=kRA_op_ACV}Use GOLD for Vassals instead of influence", true)]
        public bool bVassalPaysWithGoldToo { get; set; } = false;

        [XmlElement]
        [SettingProperty("{=kRA_op_ACVg}Gold value multiplier for Vassals (1=100%)", 0, 2, "{=kRA_op_ACVgI}0.5 means Vassals pay only 50% gold")]
        [SettingPropertyGroup("{=kRA_op_AC}Admission Costs/{=kRA_op_ACV}Use GOLD for Vassals instead of influence")]
        public float fPercentageOfGoldForVassal { get; set; } = 0.5f;

        #endregion


        [XmlElement]
        [SettingProperty("{=kRA_op_PSv}Preserve Town's stock selection between visits", "{=kRA_op_PSvI}There is semi-constant stock on multiple entries by utilizing random seed based on prosperity level and Town's name. If unchecked, fully randomize stock on each entry. ")]
        [SettingPropertyGroup("{=kRA_op_PS}Preserve Town's Stock Selection Between Visits", true)]
        public bool bPreserveRandomSeedForStock { get; set; } = true;

        [XmlElement]
        [SettingProperty("{=kRA_op_PSp}Prosperity change needed for stock change", 0, 9999, "{=kRA_op_PSpI}Town needs to change this amount of prosperity to change its selection of items")]
        [SettingPropertyGroup("{=kRA_op_PS}Preserve Town's Stock Selection Between Visits")]
        public int iProsperityChangeNeededForNewStock { get; set; } = 100;

        #endregion
        */


        /* Non-localized settings for ModLib compatiblity. Hopefully ModLib will support Localization in the future, so the section above could be used, and this one deleted. */
        #region Non-Localized settings section
        #region Filtering The Armory's Item Selection
        [XmlElement]
        [SettingProperty("Filtering The Armory's Item Selection", "Enables the filtering of the items shown in the Armory")]
        [SettingPropertyGroup("Filtering The Armory's Item Selection", true)]
        public bool bItemQualityTweaksEnabled { get; set; } = false;

        [XmlElement]
        [SettingProperty("Lowest COST of items to show in Armory (in GOLD)", 0, 99999, "This is the lowest cost of an item to be still shown")]
        [SettingPropertyGroup("Filtering The Armory's Item Selection")]
        public int iLowestValueItemInArmory { get; set; } = 10001;

        [XmlElement]
        [SettingProperty("Show item if either COST OR RARITY condition is met", "If false, then both Cost AND Rarity condition must be met to show.")]
        [SettingPropertyGroup("Filtering The Armory's Item Selection")]
        public bool bCostOrValueForItemEnough { get; set; } = true;

        [XmlElement]
        [SettingProperty("Lowest RARITY items to show (APPEARANCE)", 0f, 3f, "This is the rarity(called appearance) of an item to be still shown")]
        [SettingPropertyGroup("Filtering The Armory's Item Selection")]
        public float fLowestAppearanceItemInArmory { get; set; } = 1.01f;

        [XmlElement]
        [SettingProperty("Filter items by Town's culture", "Do not show items from different cultures (than current Town's culture).")]
        [SettingPropertyGroup("Filtering The Armory's Item Selection")]
        public bool bFilterByCulture { get; set; } = true;

        [XmlElement]
        [SettingProperty("Show neutral culture items (in all Towns)", "Also show items that have no culture tag defined.")]
        [SettingPropertyGroup("Filtering The Armory's Item Selection")]
        public bool bShowCultureMissingItems { get; set; } = true;

        [XmlElement]
        [SettingProperty("Unlimited item selection and quantity if entry was free", "Should free entry automatically mean unlimited item selection?")]
        [SettingPropertyGroup("Filtering The Armory's Item Selection")]
        public bool bUnlimitedSelectionIfFree { get; set; } = false;

        [XmlElement]
        [SettingProperty("Include Player Crafted Weapons.")]
        [SettingPropertyGroup("Filtering The Armory's Item Selection")]
        public bool bShowCraftedItems { get; set; } = false;

        [XmlElement]
        [SettingProperty("Town prosperity affects stock variety", "The more prosperous the Town is, the more varied items will spawn and vice-versa.")]
        [SettingPropertyGroup("Filtering The Armory's Item Selection/Town prosperity affects selection variety", true)]
        public bool bProsperityAffectsItemSelection { get; set; } = true;

        [XmlElement]
        [SettingProperty("MINIMUM PROSPERITY needed for any item to spawn", 0, 15000, "Prosperity for any item to be available, ie. no item will shown in Town's below this prosperity level")]
        [SettingPropertyGroup("Filtering The Armory's Item Selection/Town prosperity affects selection variety")]
        public int iMinProsperityForAnyItem { get; set; } = 1000;

        [XmlElement]
        [SettingProperty("PROSPERITY needed for the WHOLE SELECTION to show", 0, 15000, "Town prosperity needed for all items to be guaranteed")]
        [SettingPropertyGroup("Filtering The Armory's Item Selection/Town prosperity affects selection variety")]
        public int iProsperityForGuaranteedItem { get; set; } = 10000;

        [XmlElement]
        [SettingProperty("Item's value and rarity affects appearance chances", "The more expensive or rare the item is, the less chance it has to appear in stock")]
        [SettingPropertyGroup("Filtering The Armory's Item Selection")]
        public bool bItemValueAndRarityAffectsChanceToShow { get; set; } = true;
        #endregion


        #region Changing The Quantity Of Items In The Armory
        [XmlElement]
        [SettingProperty("Enable changing the QUANTITY of items shown in the Armory", "How many items should show up in the Armory")]
        [SettingPropertyGroup("Changing The Quantity Of Items In The Armory", true)]
        public bool bItemQuantityTweaksEnabled { get; set; } = false;

        [XmlElement]
        [SettingProperty("Randomize quantity", "Randomize the quantity of available items")]
        [SettingPropertyGroup("Changing The Quantity Of Items In The Armory/Randomize quantity", true)]
        public bool bRandomizeItemAvailableQuantity { get; set; } = true;

        [XmlElement]
        [SettingProperty("Randomize based on prosperity", "More prosperus Towns will have more items. If not set, quantity is truly random (within set parameters).")]
        [SettingPropertyGroup("Changing The Quantity Of Items In The Armory/Randomize quantity")]
        public bool bRandomizeQuantityBasedOnProsperity { get; set; } = true;

        [XmlElement]
        [SettingProperty("Quantity of available items", 1, 99, "If quantity is randomized, this is the max possible quantity")]
        [SettingPropertyGroup("Changing The Quantity Of Items In The Armory")]
        public int iItemAvailableQuantity { get; set; } = 3;
        #endregion


        #region Admission Costs
        [XmlElement]
        [SettingProperty("Enable changing the COST OF ADMISSION", "Change the entry fee costs into the Royal Armory")]
        [SettingPropertyGroup("Admission Costs", true)]
        public bool bAdmissionCosts { get; set; } = false;

        [XmlElement]
        [SettingProperty("Even Rulers should pay", "You have to pay even if you own the town or own the kingdom as king or queen.")]
        [SettingPropertyGroup("Admission Costs/Even Rulers should pay", true)]
        public bool bEvenRulersShouldPay { get; set; } = false;

        [XmlElement]
        [SettingProperty("Rulers pay with GOLD instead of influence", "By default influence paymant would be required")]
        [SettingPropertyGroup("Admission Costs/Even Rulers should pay")]
        public bool bRulersPayGold { get; set; } = false;

        [XmlElement]
        [SettingProperty("Value multiplier for Rulers (1=100%)", 0, 2, "0.1 means Rulers pay only 10% gold")]
        [SettingPropertyGroup("Admission Costs/Even Rulers should pay")]
        public float fPercentageOfCostForRuler { get; set; } = 0.1f;

        [XmlElement]
        [SettingProperty("Relationship with Town's owner matters", "Relationship with the Town's owner modifies the admission costs, for better or worse.")]
        [SettingPropertyGroup("Admission Costs")]
        public bool bRelationshipModifiesAdmissionCost { get; set; } = true;

        [XmlElement]
        [SettingProperty("Ignore Town's prosperity for admission cost", "Use flat value instead of Town's prosperity to calculate the admission costs,")]
        [SettingPropertyGroup("Admission Costs/Ignore Town Prosperity and use flat costs", true)]
        public bool bIgnoreTownProsperityForAdmission { get; set; } = false;

        [XmlElement]
        [SettingProperty("Flat admission cost in GOLD", 0, 99999, "Will use this flat cost istead of prosperity")]
        [SettingPropertyGroup("Admission Costs/Ignore Town Prosperity and use flat costs")]
        public int iFlatAdmissionCostGold { get; set; } = 5000;

        [XmlElement]
        [SettingProperty("Flat admission cost in INFLUENCE (Vassals)", 0, 999, "Will use this flat cost istead of prosperity")]
        [SettingPropertyGroup("Admission Costs/Ignore Town Prosperity and use flat costs")]
        public int iFlatAdmissionCostInfluence { get; set; } = 50;

        [XmlElement]
        [SettingProperty("Minimum cost of entry in GOLD", 0, 99999, "Player needs to pay at least this amount")]
        [SettingPropertyGroup("Admission Costs")]
        public int iMinCostOfAdmissionGold { get; set; } = 0;

        [XmlElement]
        [SettingProperty("Maximum cost of entry in GOLD", 0, 99999, "Player should not pay more than this")]
        [SettingPropertyGroup("Admission Costs")]
        public int iMaxCostOfAdmissionGold { get; set; } = 20000;

        [XmlElement]
        [SettingProperty("Minimum cost of entry in INFLUENCE", 0, 999, "Vassals (or even Rulers if selected) pay in influence, at least this amount.")]
        [SettingPropertyGroup("Admission Costs")]
        public int iMinCostOfAdmissionInfluence { get; set; } = 0;

        [XmlElement]
        [SettingProperty("Maximum cost of entry in INFLUENCE", 0, 999, "Vassals (or even Rulers if selected) pay in influence, max this amount")]
        [SettingPropertyGroup("Admission Costs")]
        public int iMaxCostOfAdmissionInfluence { get; set; } = 200;

        [XmlElement]
        [SettingProperty("Gold value multiplier for Mercenaries (1=100%)", 0, 2, "0.25 means mercenaries pay only 25% gold")]
        [SettingPropertyGroup("Admission Costs")]
        public float fPercentageOfGoldForMercenary { get; set; } = 0.25f;

        [XmlElement]
        [SettingProperty("Use GOLD for Vassals instead of influence", "Pay with gold (instead of influence) even as a Vassal")]
        [SettingPropertyGroup("Admission Costs/Use GOLD for Vassals instead of influence", true)]
        public bool bVassalPaysWithGoldToo { get; set; } = false;

        [XmlElement]
        [SettingProperty("Gold value multiplier for Vassals (1=100%)", 0, 2, "0.5 means Vassals pay only 50% gold")]
        [SettingPropertyGroup("Admission Costs/Use GOLD for Vassals instead of influence")]
        public float fPercentageOfGoldForVassal { get; set; } = 0.5f;
        #endregion

        [XmlElement]
        [SettingProperty("Preserve Town's stock selection between visits", "There is semi-constant stock on multiple entries by utilizing random seed based on prosperity level and Town's name. If unchecked, fully randomize stock on each entry. ")]
        [SettingPropertyGroup("Preserve Town's Stock Selection Between Visits", true)]
        public bool bPreserveRandomSeedForStock { get; set; } = true;

        [XmlElement]
        [SettingProperty("Prosperity change needed for stock change", 0, 9999, "Town needs to change this amount of prosperity to change its selection of items")]
        [SettingPropertyGroup("Preserve Town's Stock Selection Between Visits")]
        public int iProsperityChangeNeededForNewStock { get; set; } = 100;

        #endregion

    }
}