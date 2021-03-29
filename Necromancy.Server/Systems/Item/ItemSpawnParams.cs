using Necromancy.Server.Common;

namespace Necromancy.Server.Systems.Item
{
    public class ItemSpawnParams
    {
        public ItemStatuses ItemStatuses { get; set; }
        public byte Quantity { get; set; } = 1;
        public GemSlot[] GemSlots { get; set; } = new GemSlot[0];
        public int plus_maximum_durability { get; set; } 
        public int plus_physical { get; set; } 
        public int plus_magical { get; set; }
        public int plus_gp { get; set; } 
        public int plus_weight { get; set; } 
        public int plus_ranged_eff { get; set; } 
        public int plus_reservoir_eff { get; set; } 
    }
}
