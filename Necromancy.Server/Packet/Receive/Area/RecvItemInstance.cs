using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;
using System;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemInstance : PacketResponse
    {
        private readonly ItemInstance _itemInstance;
        private readonly NecClient _client;

        public RecvItemInstance(NecClient client, ItemInstance itemInstance)
            : base((ushort)AreaPacketId.recv_item_instance, ServerType.Area)
        {
            _itemInstance = itemInstance;
            _client = client;
            Clients.Add(_client);
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_itemInstance.InstanceID);                  //INSTANCE ID
            res.WriteInt32(_itemInstance.BaseID);                       //BASE ID
            res.WriteByte(_itemInstance.Quantity);                      //QUANTITY
            res.WriteInt32((int)_itemInstance.Statuses);                //STATUSES
            res.WriteFixedString("", 0x10);                             //Carmellia 128 bit encryption key for the item row.
            res.WriteByte((byte)_itemInstance.Location.ZoneType);       //STORAGE ZONE
            res.WriteByte(_itemInstance.Location.Container);            //BAG
            res.WriteInt16(_itemInstance.Location.Slot);                //SLOT
            res.WriteInt32(0b11111111);                                          //UNKNOWN
            res.WriteInt32((int)_itemInstance.CurrentEquipSlot);        //CURRENT EQUIP SLOT
            res.WriteInt32(_itemInstance.CurrentDurability);            //CURRENT DURABILITY
            res.WriteByte(_itemInstance.EnhancementLevel);              //ENHANCEMENT LEVEL?
            res.WriteByte(_itemInstance.SpecialForgeLevel);             //?SPECIAL FORGE LEVEL?
            res.WriteCString(_itemInstance.TalkRingName);               //TALK RING NAME
            res.WriteInt16(_itemInstance.Physical);                     //PHYSICAL
            res.WriteInt16(_itemInstance.Magical);                      //MAGICAL
            res.WriteInt32(_itemInstance.MaximumDurability);            //MAX DURABILITY
            res.WriteByte(_itemInstance.Hardness);                      //HARDNESS
            res.WriteInt32(_itemInstance.Weight);                       //WEIGHT IN THOUSANDTHS

            const int MAX_WHATEVER_SLOTS = 2;
            int numEntries = 2;
            res.WriteInt32(numEntries);                                 //less than or equal to 2?
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32((byte)0);                                //UNKNOWN
            }

            int numOfGemSlots = _itemInstance.GemSlots.Length;
            res.WriteInt32(numOfGemSlots);                              //NUMBER OF GEM SLOTS
            for (int j = 0; j < numOfGemSlots; j++)
            {
                byte isFilled = _itemInstance.GemSlots[j].IsFilled ? (byte) 1 : (byte) 0;
                res.WriteByte(isFilled);                                //IS FILLED
                res.WriteInt32((int)_itemInstance.GemSlots[j].Type);    //GEM TYPE
                res.WriteInt32(_itemInstance.GemSlots[j].Gem.BaseID);   //GEM BASE ID
                res.WriteInt32(0);                                      //UNKNOWN maybe gem item 2 id for diamon 2 gem combine 
            }
            
            res.WriteInt32(_itemInstance.ProtectUntil);                 //PROTECT UNTIL DATE IN SECONDS
            res.WriteInt64(0);                                          //UNKNOWN
            res.WriteInt16(0xff);                                       //0 = green (in shop for sale)  0xFF = normal /*item.ShopStatus*/
            res.WriteInt32(_itemInstance.EnchantId);                    //UNKNOWN - ENCHANT ID? 1 IS GUARD
            res.WriteInt16(_itemInstance.GP);                           //GP

            numEntries = 5; //new
            //equip skill related
            res.WriteInt32(numEntries); // less than or equal to 5 - must be 5 or crashes as of now
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32(801011);  //unknown
                res.WriteByte((byte)10);   //unknown
                res.WriteByte((byte)10);   //unknown
                res.WriteInt16((short)10);  //unknown
                res.WriteInt16((short)10);  //unknown
            }
            //Item_Update_Parameter  section
            res.WriteInt64(Int64.MaxValue);              //unknown
            res.WriteInt16(_itemInstance.PlusPhysical);                 //+PHYSICAL
            res.WriteInt16(_itemInstance.PlusMagical);                  //+MAGICAL
            res.WriteInt16(_itemInstance.PlusWeight);                   //+WEIGHT IN THOUSANTHS, DISPLAYS AS HUNDREDTHS
            res.WriteInt16(_itemInstance.PlusDurability);               //+DURABILITY
            res.WriteInt16(_itemInstance.PlusGP);                       //+GP
            res.WriteInt16(_itemInstance.PlusRangedEff);                //+Ranged Efficiency
            res.WriteInt16(_itemInstance.PlusReservoirEff);             //+Resevior Efficiency

            //UNIQUE EFFECTS
            res.WriteInt32(Util.GetRandomNumber(1,10));  //V|EFFECT1 TYPE - 0 IS NONE - PULLED FROM STR_TABLE?
            res.WriteInt32(Util.GetRandomNumber(1, 10));  //V|EFFECT2 TYPE - 0 IS NONE
            res.WriteInt32(Util.GetRandomNumber(1, 10));  //V|EFFECT3 TYPE - 0 IS NONE
            res.WriteInt32(Util.GetRandomNumber(1, 10));  //V|EFFECT4 TYPE - 0 IS NONE
            res.WriteInt32(Util.GetRandomNumber(1, 10));  //V|EFFECT5 TYPE - 0 IS NONE

            res.WriteInt32(Util.GetRandomNumber(10, 100));  //V|EFFECT1 VALUE - IF ENABLED MUST BE GREATER THAN ZERO OR DISPLAY ERROR
            res.WriteInt32(Util.GetRandomNumber(10, 100));  //V|EFFECT2 VALUE - IF ENABLED MUST BE GREATER THAN ZERO OR DISPLAY ERROR
            res.WriteInt32(Util.GetRandomNumber(10, 100));  //V|EFFECT3 VALUE - IF ENABLED MUST BE GREATER THAN ZERO OR DISPLAY ERROR
            res.WriteInt32(Util.GetRandomNumber(10, 100));  //V|EFFECT4 VALUE - IF ENABLED MUST BE GREATER THAN ZERO OR DISPLAY ERROR
            res.WriteInt32(Util.GetRandomNumber(10, 100));  //V|EFFECT5 VALUE - IF ENABLED MUST BE GREATER THAN ZERO OR DISPLAY ERROR

            //UNKNOWN
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32(j);//UNKNOWN
                res.WriteFixedString("Ok", 0x2);
                res.WriteInt16((short)1);//UNKNOWN
                res.WriteInt16((short)1);//UNKNOWN
            }
            //End Update Parameter.
            res.WriteInt16(_itemInstance.RangedEffDist);            //Ranged Efficiency/distance - need better translation
            res.WriteInt16(_itemInstance.ReservoirLoadPerf);        //Resevior/loading Efficiency/performance - need better translation
            res.WriteByte(_itemInstance.NumOfLoads);                //Number of loads - need better translation
            res.WriteByte(_itemInstance.SPCardColor);               //Soul Partner card type color, pulled from str_table 100,1197,add 1 to sent value to find match

            res.WriteInt64(Int64.MaxValue);//UNKNOWN

            //base enchant display on bottom
            res.WriteInt16((short)0);  //Base Enchant Scroll ID

            res.WriteInt32(0);                  //misc field for displaying enchant removal / extraction I think: 0 - off, 1 - on, 5 percent sign, 6 remove, 7- extract
            res.WriteInt32(0);                  //enchantment effect statement, 100,1250,{stringID
            res.WriteInt16(0);           //enchantment effect value            
            res.WriteInt16(0);           //unknown

            res.WriteInt32(0);                  //misc field for displaying enchant removal / extraction I think: 0 - off, 1 - on, 5 percent sign, 6 remove, 7- extract
            res.WriteInt32(0);                  //enchantment effect statement, 100,1250,{stringID
            res.WriteInt16(0);           //enchantment effect value            
            res.WriteInt16(0);     //unknown            

            //sub enchantment, values hidden unless viewed at enchant shop maybe
            numEntries = 5;
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt16(0);          //Sub Enchant Scroll ID
                res.WriteInt32(0);                  //misc field for displaying enchant removal / extraction I think: 0 - off, 1 - on, 5 percent sign, 6 remove, 7- extract
                res.WriteInt32(0);                  //enchantment effect statement, 100,1250,{stringID
                res.WriteInt16(0);           //enchantment effect value            
                res.WriteInt16(0);           //unknown
            }

            res.WriteInt16(0);                  //enchant max cost allowance

            return res;            
        }
    }
}
