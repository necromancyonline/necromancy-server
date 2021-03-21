using System;

namespace Necromancy.Server.Systems.Item
{
    [Flags]
    public enum ItemZoneType : byte
    {
        InvalidZone     = 255,
        AdventureBag    = 0,
        EquippedBags    = 1, 
        PremiumBag      = 2,
        Warehouse       = 3,
        UNKNOWN4        = 4,
        BagSlot         = 8,
        //probably warehouse expansions?
        UNKNOWN9        = 9, //shows item 
        WarehouseSp     = 10,
        AvatarInventory = 12,
        TreasureBox     = 66,
        LootingBodyAdventureBag  = 71,
        LootingBodyEquippedBags = 72,
        LootingBodyPremiumBag = 74,

        TempAuctionZone = 99


    }
}
