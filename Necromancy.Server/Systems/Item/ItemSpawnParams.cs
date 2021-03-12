using Necromancy.Server.Common;

namespace Necromancy.Server.Systems.Item
{
    public class ItemSpawnParams
    {
        public ItemStatuses ItemStatuses { get; set; }
        public byte Quantity { get; set; } = 1;
        public GemSlot[] GemSlots { get; set; } = new GemSlot[0];
        public int plus_maximum_durability { get; set; } = Util.GetRandomNumber(-10,10);
        public int plus_physical { get; set; } = Util.GetRandomNumber(-20, 20);
        public int plus_magical { get; set; } = Util.GetRandomNumber(-20,20);
        public int plus_gp { get; set; } = Util.GetRandomNumber(-15, 25);
        public int plus_weight { get; set; } = Util.GetRandomNumber(-50, 25);
        public int plus_ranged_eff { get; set; } = Util.GetRandomNumber(-10, 10);
        public int plus_reservoir_eff { get; set; } = Util.GetRandomNumber(-10, 10);
    }
}
