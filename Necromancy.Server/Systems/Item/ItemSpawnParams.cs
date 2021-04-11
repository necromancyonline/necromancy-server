using Necromancy.Server.Common;

namespace Necromancy.Server.Systems.Item
{
    public class ItemSpawnParams
    {
        public ItemStatuses itemStatuses { get; set; }
        public byte quantity { get; set; } = 1;
        public GemSlot[] gemSlots { get; set; } = new GemSlot[0];
        public int plusMaximumDurability { get; set; }
        public int plusPhysical { get; set; }
        public int plusMagical { get; set; }
        public int plusGp { get; set; }
        public int plusWeight { get; set; }
        public int plusRangedEff { get; set; }
        public int plusReservoirEff { get; set; }
    }
}
