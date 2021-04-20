namespace Necromancy.Server.Systems.Item
{
    public enum ItemType
    {
        //#Classification,display,Remarks,Type,.  per  itemType.Csv
        //Necromancy Database Table and below Enum by Classification to get in game 'display' for each item.
        NONE = 0,
        TRUMP = 1,
        DAGGER = 2,
        SWORD_1H = 3,
        RAPIER = 4,
        SWORD_2H = 5,
        KATANA_1H = 6,
        KATANA_2H = 7,
        AXE_1H = 8,
        AXE_2H = 9,
        CLUB_1H = 10,
        CLUB_2H = 11,
        WAND_1H = 12,
        WAND_2H = 13,
        SPEAR = 14,
        BOW = 15,
        CROSSBOW = 16,
        GUN = 17,
        INSTRUMENT = 18,
        DOUBLE_EDGED = 19,
        SHIELD_SMALL = 30,
        SHIELD_MEDIUM = 31,
        SHIELD_LARGE = 32,
        ARROW = 33,
        BOLT = 34,
        BULLET = 35,
        PROTECTOR = 36, //PROTCT  Old ARMOR
        ACCESSORY = 37, //ACCRY,  old accessory
        HELMET = 38, //helmetm, armor
        HAT = 39,
        HOOD = 40,
        ARMOR_TOPS = 41, //ARMOR
        ARMOR_BOTTOMS = 42, //leg armor
        CLOTHES_TOPS = 43, //TOP
        CLOTHES_BOTTOMS = 44, //undergarment
        COAT = 45, //outerwear
        GLOVES = 46, //gloves
        GAUNTLETS = 47, //back of the hand
        BRACLET = 48, // bracelet
        SABATONS = 49, // iron shoes
        SHOES = 50, // SHOES
        CLOAK = 51, // mantle 
        MANTLE = CLOAK,
        RING = 52, //ring
        EARRING = 53, //earrings
        NECKLACE = 54, //NCKLCE necklace
        BELT = 55, //belt
        DRUG = 56, //Chem
        SKILLCHIP = 57, //Skill
        BAG = 58, //bag
        LOSTSOUL = 59, //LSTSOL "Blank Display Type"

        FORGESTONE = 60, //FG_STN   Forgi
        TALKRING_MAKEKIT = 61, //TOKRNG  Talk ring
        MASTER_TALKRING = 62, //TOKRING Talk ring
        TALKRING = 63, //TOKRNG Talk
        FORGEGUARDSTONE = 64, //FG_GRD "blank"

        //FORGE_PARAMETER_SUPPORT = 56, //gem
        GEM = 65, //GEM
        GEM_PIECE = 66, //GEM debris
        GEM_SPALL = 67, //gem extract   ** no way to distinguish gem spall vs gem synthesis
        GEM_SYNTHESIS = 94, //GEM synthetic material ** why is this 94.. ..
        MAP_FRAGMENT = 68, //MAP PC  divided map
        OTHER = 69,
        OTHERS = 69, //OTHER
        AVATAR = 70, //avatar
        ALLCHT = 71, // "blank
        BUFF = 72, //buff
        BUFF_FOOD = 73, //food
        LEATHER = 74, //leather shoes
        FORGETICKET = 75, //training ticket
        FORGE_SP_SUPPORT = 76, // special training
        FORGE_GEM_SUPPORT = 77, //Gem translpant material

        // =78, //arca ??
        ARRANGE_TICKET = 79, //arrange ticket
        PARTNER_CARD = 80, //partner card
        PARTNER_NURTURE_FOOD = 81, //partner experience points
        PARTNER_NURTURE_PLAY = 82, //partner intimact
        EVENT = 83, //seasonal event
        SEASON_EVENT = 83, //seasonal event
        TREASURE_KYE = 84, //key
        DEPUS = 85, //confidential 
        SKILL_BOOK = 86, //ancient documents
        JOB_CHANGE = 87, //job change certificate
        CAMP = 88, //camp
        TOOL = 89, //tool
        COLLECTION = 90, //collectibles
        MATERIAL = 91, //material
        SPHERE = 92, //sphere
        CAMPAIGN = 93, //campaign

        SCROLL = 95, //scroll
        SCROLL_SUPPORT = 96, //enchantment support
        WHOLECHAT = ALLCHT
    }
}
