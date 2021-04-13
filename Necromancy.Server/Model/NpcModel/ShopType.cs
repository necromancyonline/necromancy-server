namespace Necromancy.Server.Model
{
    internal enum ShopType
    {
        None = 1 << 0,
        Curse = 1 << 0,
        Purchase = 1 << 1,
        Sell = 1 << 2,
        Identify = 1 << 3,
        Repair = 1 << 4,
        Special = 1 << 5,
        Meal = 1 << 6,
        Forge = 1 << 7,
        Trade = 1 << 8,
        Restore = 0b010000000000000,
        Fuse = 0b100000000000000,
        Donkey = Purchase + Sell + Identify,
        Blacksmith = Purchase + Identify + Repair + Forge,
        Gem = Restore + Fuse + Purchase,
        Bar = Meal
    }
}
