using System;

namespace Necromancy.Server.Systems.Item
{
    [Flags]
    public enum ItemEquipSlots
    {
        None = 0,
        RightHand = 1 << 0,
        LeftHand = 1 << 1,
        Quiver = 1 << 2,
        Head = 1 << 3,
        Torso = 1 << 4,
        Legs = 1 << 5,
        Arms = 1 << 6,
        Feet = 1 << 7,
        Cloak = 1 << 8,
        Mantle = 1 << 8,
        Ring = 1 << 9,
        Earring = 1 << 10,
        Necklace = 1 << 11,
        Belt = 1 << 12,
        Talkring = 1 << 13,
        AvatarHead = 1 << 14,
        AvatarTorso = 1 << 15,
        AvatarLegs = 1 << 16,
        AvatarArms = 1 << 17,
        AvatarFeet = 1 << 18,
        AvatarCloak = 1 << 19,
        TwoHanded = RightHand | LeftHand
    }
}
