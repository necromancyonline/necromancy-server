using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemInstance : PacketResponse
    {
        private readonly NecClient _client;
        private readonly ItemInstance _itemInstance;

        public RecvItemInstance(NecClient client, ItemInstance itemInstance)
            : base((ushort)AreaPacketId.recv_item_instance, ServerType.Area)
        {
            _itemInstance = itemInstance;
            _client = client;
            clients.Add(_client);
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_itemInstance.instanceId); //INSTANCE ID
            res.WriteInt32(_itemInstance.baseId); //BASE ID
            res.WriteByte(_itemInstance.quantity); //QUANTITY
            res.WriteInt32((int)_itemInstance.statuses); //STATUSES
            res.WriteFixedString("", 0x10); //Carmellia 128 bit encryption key for the item row.
            res.WriteByte((byte)_itemInstance.location.zoneType); //STORAGE ZONE
            res.WriteByte(_itemInstance.location.container); //BAG
            res.WriteInt16(_itemInstance.location.slot); //SLOT
            res.WriteInt32(0); //UNKNOWN
            res.WriteInt32((int)_itemInstance.currentEquipSlot); //CURRENT EQUIP SLOT
            res.WriteInt32(_itemInstance.currentDurability); //CURRENT DURABILITY
            res.WriteByte(_itemInstance.enhancementLevel); //ENHANCEMENT LEVEL?
            res.WriteByte(_itemInstance.specialForgeLevel); //?SPECIAL FORGE LEVEL?
            res.WriteCString(_itemInstance.talkRingName); //TALK RING NAME
            res.WriteInt16(_itemInstance.physical); //PHYSICAL
            res.WriteInt16(_itemInstance.magical); //MAGICAL
            res.WriteInt32(_itemInstance.maximumDurability); //MAX DURABILITY
            res.WriteByte(_itemInstance.hardness); //HARDNESS
            res.WriteInt32(_itemInstance.weight / 10); //WEIGHT IN THOUSANDTHS

            int numEntries = 2;
            res.WriteInt32(numEntries); //less than or equal to 2?
            for (int j = 0; j < numEntries; j++) res.WriteInt32(0); //UNKNOWN

            int numOfGemSlots = _itemInstance.gemSlots.Length;
            res.WriteInt32(numOfGemSlots); //NUMBER OF GEM SLOTS
            for (int j = 0; j < numOfGemSlots; j++)
            {
                byte isFilled = _itemInstance.gemSlots[j].isFilled ? 1 : 0;
                res.WriteByte(isFilled); //IS FILLED
                res.WriteInt32((int)_itemInstance.gemSlots[j].type); //GEM TYPE
                res.WriteInt32(_itemInstance.gemSlots[j].gem.baseId); //GEM BASE ID
                res.WriteInt32(0); //UNKNOWN maybe gem item 2 id for diamon 2 gem combine
            }

            res.WriteInt32(_itemInstance.protectUntil); //PROTECT UNTIL DATE IN SECONDS
            res.WriteInt64(0); //UNKNOWN
            res.WriteInt16(0xff); //0 = green (in shop for sale)  0xFF = normal /*item.ShopStatus*/
            res.WriteInt32(_itemInstance.enchantId); //UNKNOWN - ENCHANT ID? 1 IS GUARD
            res.WriteInt16(_itemInstance.gp); //GP

            numEntries = 5; //new
            //equip skill related
            res.WriteInt32(numEntries); // less than or equal to 5 - must be 5 or crashes as of now
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32(801011); //unknown
                res.WriteByte(10); //unknown
                res.WriteByte(10); //unknown
                res.WriteInt16(10); //unknown
                res.WriteInt16(10); //unknown
            }

            //Item_Update_Parameter  section
            res.WriteInt64(Util.GetRandomNumber(1, 100)); //Plus sale value??
            res.WriteInt16(_itemInstance.plusPhysical); //+PHYSICAL
            res.WriteInt16(_itemInstance.plusMagical); //+MAGICAL
            res.WriteInt16((short)(_itemInstance.plusWeight / 10)); //+WEIGHT IN THOUSANTHS, DISPLAYS AS HUNDREDTHS //TODO REMOVE THIS DIVISION AND FIX TRHOUGHOUT CODE
            res.WriteInt16(_itemInstance.plusDurability); //+DURABILITY
            res.WriteInt16(_itemInstance.plusGp); //+GP
            res.WriteInt16(_itemInstance.plusRangedEff); //+Ranged Efficiency
            res.WriteInt16(_itemInstance.plusReservoirEff); //+Resevior Efficiency

            //UNIQUE EFFECTS
            res.WriteInt32(0); //V|EFFECT1 TYPE - 0 IS NONE - PULLED FROM STR_TABLE?
            res.WriteInt32(0); //V|EFFECT2 TYPE - 0 IS NONE
            res.WriteInt32(0); //V|EFFECT3 TYPE - 0 IS NONE
            res.WriteInt32(0); //V|EFFECT4 TYPE - 0 IS NONE
            res.WriteInt32(0); //V|EFFECT5 TYPE - 0 IS NONE

            res.WriteInt32(0); //V|EFFECT1 VALUE - IF ENABLED MUST BE GREATER THAN ZERO OR DISPLAY ERROR
            res.WriteInt32(0); //V|EFFECT2 VALUE - IF ENABLED MUST BE GREATER THAN ZERO OR DISPLAY ERROR
            res.WriteInt32(0); //V|EFFECT3 VALUE - IF ENABLED MUST BE GREATER THAN ZERO OR DISPLAY ERROR
            res.WriteInt32(0); //V|EFFECT4 VALUE - IF ENABLED MUST BE GREATER THAN ZERO OR DISPLAY ERROR
            res.WriteInt32(0); //V|EFFECT5 VALUE - IF ENABLED MUST BE GREATER THAN ZERO OR DISPLAY ERROR

            //UNKNOWN
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32(j); //UNKNOWN
                res.WriteFixedString("Ok", 0x2); //could be two bytes, not a fixed string.
                res.WriteInt16(1); //UNKNOWN
                res.WriteInt16(1); //UNKNOWN
            }

            //End Update Parameter.
            res.WriteInt16(_itemInstance.rangedEffDist); //Ranged Efficiency/distance - need better translation
            res.WriteInt16(_itemInstance.reservoirLoadPerf); //Resevior/loading Efficiency/performance - need better translation
            res.WriteByte(_itemInstance.numOfLoads); //Number of loads - need better translation
            res.WriteByte(_itemInstance.spCardColor); //Soul Partner card type color, pulled from str_table 100,1197,add 1 to sent value to find match

            res.WriteInt64(0); //UNKNOWN

            //base enchant display on bottom
            res.WriteInt16(0); //Base Enchant Scroll ID

            res.WriteInt32(0); //misc field for displaying enchant removal / extraction I think: 0 - off, 1 - on, 5 percent sign, 6 remove, 7- extract
            res.WriteInt32(0); //enchantment effect statement, 100,1250,{stringID
            res.WriteInt16(0); //enchantment effect value
            res.WriteInt16(0); //unknown

            res.WriteInt32(0); //misc field for displaying enchant removal / extraction I think: 0 - off, 1 - on, 5 percent sign, 6 remove, 7- extract
            res.WriteInt32(0); //enchantment effect statement, 100,1250,{stringID
            res.WriteInt16(0); //enchantment effect value
            res.WriteInt16(0); //unknown

            //sub enchantment, values hidden unless viewed at enchant shop maybe
            numEntries = 5;
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt16(0); //Sub Enchant Scroll ID
                res.WriteInt32(0); //misc field for displaying enchant removal / extraction I think: 0 - off, 1 - on, 5 percent sign, 6 remove, 7- extract
                res.WriteInt32(0); //enchantment effect statement, 100,1250,{stringID
                res.WriteInt16(0); //enchantment effect value
                res.WriteInt16(0); //unknown
            }

            res.WriteInt16(0); //enchant max cost allowance

            return res;
        }
    }
}
