using System;

namespace Necromancy.Server.Systems.Item
{
    public class ItemInstance : ItemBase
    {
        public const int MAX_GEM_SLOTS = 3;

        /// <summary>
        /// An instance of a base item. Holds mostly changable values.
        /// </summary>
        /// <param name="instanceId">Item's generated ID from the database.</param>
        public ItemInstance(ulong instanceId)
        {
            InstanceID = instanceId;
        }
        /// <summary>
        /// ID Generated when item is created from a base item template.
        /// </summary>
        public ulong InstanceID { get; private set; }
        /// <summary>
        /// Owner's character ID.
        /// </summary>
        public int OwnerID { get; internal set; }
        /// <summary>
        /// Item's displayed name when unidentified. Always "? <c>ItemType</c>".
        /// </summary>
        public string UnidentifiedName { 
            get {
                return "? " + Type.ToString();
            }
        }

        public byte Quantity { get; set; } = 1;

        public ItemStatuses Statuses { get; internal set; }

        public ItemLocation Location { get; set; } = ItemLocation.InvalidLocation;

        public ItemEquipSlots CurrentEquipSlot { get; internal set; }
        /// <summary>
        /// Current durability remaining of the item.
        /// </summary>
        public int CurrentDurability { get; internal set; }

        public byte EnhancementLevel { get; internal set; }

        public byte SpecialForgeLevel { get; internal set; }

        public string TalkRingName = "";

        public short Physical { get; internal set; }

        public short Magical { get; internal set; }

        public int MaximumDurability { get; internal set; }

        public byte Hardness { get; internal set; }

        /// <summary>
        /// Weight in thousandths.
        /// </summary>
        public int Weight { get; internal set; }

        public GemSlot[] GemSlots { get; internal set; } = new GemSlot[0];

        public int EnchantId { get; internal set; }

        public short GP { get; internal set; }
        /// <summary>
        /// Item is provided with 'Protect' status until this date in seconds. 
        /// Maximum year is 2038 because it is an integer.
        /// </summary>
        public int ProtectUntil { get; internal set; }

        public short PlusPhysical { get; internal set; }
        public short PlusMagical { get; internal set; }
        public short PlusWeight { get; internal set; }
        public short PlusDurability { get; internal set; }
        public short PlusGP { get; internal set; }
        public short PlusRangedEff { get; internal set; }
        public short PlusReservoirEff { get; internal set; }

        //Update once better translation available
        public short RangedEffDist { get; internal set; }
        public short ReservoirLoadPerf { get; internal set; }
        public byte NumOfLoads { get; internal set; }

        public byte SPCardColor { get; internal set; }

        //Auction properties
        public byte ConsignmentSlot { get; set; } 
        public string ConsignerName { get; set; }
        public int SecondsUntilExpiryTime { get; set; }
        public ulong MinimumBid { get; set; }
        public ulong BuyoutPrice { get; set; }
        public int BidderId { get; set; }
        public string BidderName { get; set; }
        public int CurrentBid { get; set; }
        public string Comment { get; set; }
        public bool IsCancellable { get; set; } = true;


        /// <summary>
        /// Helper function to check if the item is identified or not.
        /// </summary>
        public bool IsIdentified { 
            get
            {
                return ((ItemStatuses.Identified & Statuses) != 0) && ((ItemStatuses.Unidentified & Statuses) == 0);
            } 
        }
        
    }
}
