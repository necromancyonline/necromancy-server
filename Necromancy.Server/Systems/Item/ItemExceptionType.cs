using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    public enum ItemExceptionType
    {
        OK = 0,
        Generic = 1,
        BagLocation = -201,         //Store location is incorrect
        Amount = -204,              //Item amount is incorrect
        InvalidTarget = -205,       //The target to use this item is incorrect
        DelayNotUp = -206,          //Unable to use due to delay time
        InventoryFull = -207,       //No space available in inventory
        Cursed = -208,              //Unable to use this item since it is cursed
        Broken = -209,              //Unable to use this item since it is broken
        Requirement = -210,         //You do not meet the requirements to equip this item
        UnableToUse = -211,         //Unable to use this item
        Status = -212,              //You are not in the right status to use this item
        OnCooldown = -230,          //Unable to use since it is on cool down.
        AlreadyReceived = -2601,    //You've already received this scrap
        OutsideTown = -2708,        //Cannot be used outside of town
        ShopOpen = -3001            //Unable to move items when you have a shop open
    }
}
