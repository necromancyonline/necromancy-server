namespace Necromancy.Server.Systems.Item
{
    public class ItemInstance : ItemBase
    {
        public const int MaxGemSlots = 3;

        public string talkRingName = "";

        /// <summary>
        ///     An instance of a base item. Holds mostly changable values.
        /// </summary>
        /// <param name="instanceId">Item's generated ID from the database.</param>
        public ItemInstance(ulong instanceId)
        {
            this.instanceId = instanceId;
        }

        /// <summary>
        ///     ID Generated when item is created from a base item template.
        /// </summary>
        public ulong instanceId { get; }

        /// <summary>
        ///     Owner's character ID.
        /// </summary>
        public int ownerId { get; internal set; }

        /// <summary>
        ///     Item's displayed name when unidentified. Always "? <c>ItemType</c>".
        /// </summary>
        public string unidentifiedName => "? " + type;

        public byte quantity { get; set; } = 1;

        public ItemStatuses statuses { get; internal set; }

        public ItemLocation location { get; set; } = ItemLocation.InvalidLocation;

        public ItemEquipSlots currentEquipSlot { get; internal set; }

        /// <summary>
        ///     Current durability remaining of the item.
        /// </summary>
        public int currentDurability { get; internal set; }

        public byte enhancementLevel { get; internal set; }

        public byte specialForgeLevel { get; internal set; }

        public short physical { get; internal set; }

        public short magical { get; internal set; }

        public int maximumDurability { get; internal set; }

        public byte hardness { get; internal set; }

        /// <summary>
        ///     Weight in thousandths.
        /// </summary>
        public int weight { get; internal set; }

        public GemSlot[] gemSlots { get; internal set; } = new GemSlot[0];

        public int enchantId { get; internal set; }

        public short gp { get; internal set; }

        /// <summary>
        ///     Item is provided with 'Protect' status until this date in seconds.
        ///     Maximum year is 2038 because it is an integer.
        /// </summary>
        public int protectUntil { get; internal set; }

        public short plusPhysical { get; internal set; }
        public short plusMagical { get; internal set; }
        public short plusWeight { get; internal set; }
        public short plusDurability { get; internal set; }
        public short plusGp { get; internal set; }
        public short plusRangedEff { get; internal set; }
        public short plusReservoirEff { get; internal set; }

        //Update once better translation available
        public short rangedEffDist { get; internal set; }
        public short reservoirLoadPerf { get; internal set; }
        public byte numOfLoads { get; internal set; }

        public byte spCardColor { get; internal set; }

        //Auction properties
        public string consignerSoulName { get; set; }
        public int secondsUntilExpiryTime { get; set; }
        public ulong minimumBid { get; set; }
        public ulong buyoutPrice { get; set; }
        public int bidderSoulId { get; set; }
        public string bidderName { get; set; }
        public int currentBid { get; set; }
        public bool isBidCancelled { get; internal set; }
        public int maxBid { get; set; }
        public string comment { get; set; }


        /// <summary>
        ///     Helper function to check if the item is identified or not.
        /// </summary>
        public bool isIdentified => (ItemStatuses.Identified & statuses) != 0 && (ItemStatuses.Unidentified & statuses) == 0;
    }
}
