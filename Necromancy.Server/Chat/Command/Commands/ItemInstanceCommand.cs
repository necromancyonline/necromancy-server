using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Quick item test commands.
    /// </summary>
    public class ItemInstanceCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(ItemInstanceCommand));

        public ItemInstanceCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "itemi";
        public override string HelpText => "usage: `/itemi [itemId]`";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res = BufferProvider.Provide();
            

            const int NUMBER = 10;
            for(int i = 0; i < NUMBER; i++) {
                res = BufferProvider.Provide();
                res.WriteInt32(2);
                Router.Send(client, (ushort)AreaPacketId.recv_situation_start, res, ServerType.Area);

                res = BufferProvider.Provide();
                ulong instanceId = (ulong)(10001 + i);
                res.WriteUInt64(instanceId);        //V|SPAWN ID
                res.WriteInt32(81101000 + i);               //V|BASE ID
                res.WriteByte(1);                           //V|QUANTITY
                res.WriteInt32(2);                   //V|STATUSES
                res.WriteFixedString("赤", 0x10);             //UNKNOWN - ITEM TYPE? decrypt key?
                res.WriteByte((byte)(0));                     //V|ZONE 66- treasure box
                res.WriteByte(0);                           //V|BAG
                res.WriteInt16((short)i);                   //V|SLOT
                res.WriteInt32(0);                     //unknown
                res.WriteInt32(0);                          //V|EQUIP SLOT
                res.WriteInt32(5);                          //V|CURRENT DURABILITY
                res.WriteByte((byte)7);                     //v|ENHANCEMENT LEVEL
                res.WriteByte(2);                           //SPECIAL FORGE LEVEL?
                res.WriteCString("");                       //V|TALK RING NAME
                res.WriteInt16(5);                          //V|PHYSICAL
                res.WriteInt16(5);                          //V|MAGICAL
                res.WriteInt32(5);                          //V|MAX DURABILITY
                res.WriteByte(5);                           //V|HARDNESS
                res.WriteInt32(2000);                       //V|WEIGHT IN THOUSANTHS, DISPLAYS AS HUNDREDTHS

                int numEntries = 2;
                res.WriteInt32(numEntries);                  //less than or equal to 2?
                for (int j = 0; j < numEntries; j++)
                {
                    res.WriteInt32((byte)14);                 //unknown
                }

                int numOfGemSlots = 1;
                res.WriteInt32(numOfGemSlots); //number of gem slots
                for (int j = 0; j < numOfGemSlots; j++)
                {
                    res.WriteByte(0);       //IS FILLED
                    res.WriteInt32(1);      //GEM TYPE
                    res.WriteInt32(0);      //GEM BASE ID
                    res.WriteInt32(0);      //unknown maybe gem item 2 id for diamon 2 gem combine 
                }

                res.WriteInt32(0);              //V|PROTECT UNTIL DATE IN SECONDS
                res.WriteInt64(Int64.MaxValue);         //unknown
                res.WriteInt16(0xFF);           //0 = green (in shop for sale)  0xFF = normal /*item.ShopStatus*/
                res.WriteInt32(i);         //unknown - 1 is guard
                res.WriteInt16(15);             //V|GP

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

                res.WriteInt64(Int64.MaxValue);              //unknown
                res.WriteInt16((short)0);       //V|+PHYSICAL
                res.WriteInt16((short)0);       //V|+MAGICAL
                res.WriteInt16((short)0);       //V|+WEIGHT IN THOUSANTHS, DISPLAYS AS HUNDREDTHS
                res.WriteInt16((short)0);       //V|+DURABILITY
                res.WriteInt16((short)0);       //V|+GP
                res.WriteInt16((short)0);       //V|+Ranged Efficiency
                res.WriteInt16((short)0);       //V|+Resevior Efficiency

                //UNIQUE EFFECTS
                res.WriteInt32(0);  //V|EFFECT1 TYPE - 0 IS NONE - PULLED FROM STR_TABLE?
                res.WriteInt32(0);  //V|EFFECT2 TYPE - 0 IS NONE
                res.WriteInt32(0);  //V|EFFECT3 TYPE - 0 IS NONE
                res.WriteInt32(0);  //V|EFFECT4 TYPE - 0 IS NONE
                res.WriteInt32(0);  //V|EFFECT5 TYPE - 0 IS NONE

                res.WriteInt32(0);  //V|EFFECT1 VALUE - IF ENABLED MUST BE GREATER THAN ZERO OR DISPLAY ERROR
                res.WriteInt32(0);  //V|EFFECT2 VALUE - IF ENABLED MUST BE GREATER THAN ZERO OR DISPLAY ERROR
                res.WriteInt32(0);  //V|EFFECT3 VALUE - IF ENABLED MUST BE GREATER THAN ZERO OR DISPLAY ERROR
                res.WriteInt32(0);  //V|EFFECT4 VALUE - IF ENABLED MUST BE GREATER THAN ZERO OR DISPLAY ERROR
                res.WriteInt32(0);  //V|EFFECT5 VALUE - IF ENABLED MUST BE GREATER THAN ZERO OR DISPLAY ERROR

                //UNKNOWN
                for (int j = 0; j < numEntries; j++)
                {
                    res.WriteInt32(1);//UNKNOWN
                    res.WriteByte((byte)1);//UNKNOWN
                    res.WriteByte((byte)1);//UNKNOWN
                    res.WriteInt16((short)1);//UNKNOWN
                    res.WriteInt16((short)1);//UNKNOWN
                }

                res.WriteInt16((short)0);      //V|Ranged Efficiency/distance - need better translation
                res.WriteInt16((short)0);      //V|Resevior/loading Efficiency/performance - need better translation
                res.WriteByte((byte)0);        //V|Number of loads - need better translation
                res.WriteByte((byte)0);        //Soul Partner card type color, pulled from str_table 100,1197,add 1 to sent value to find match

                res.WriteInt64(Int64.MaxValue);//UNKNOWN

                //base enchant display on bottom
                res.WriteInt16((short)0);  //Base Enchant Scroll ID

                res.WriteInt32(0);                  //misc field for displaying enchant removal / extraction I think: 0 - off, 1 - on, 5 percent sign, 6 remove, 7- extract
                res.WriteInt32(0);                  //enchantment effect statement, 100,1250,{stringID
                res.WriteInt16((short)0);           //enchantment effect value            
                res.WriteInt16((short)0);           //unknown

                res.WriteInt32(0);                  //misc field for displaying enchant removal / extraction I think: 0 - off, 1 - on, 5 percent sign, 6 remove, 7- extract
                res.WriteInt32(0);                  //enchantment effect statement, 100,1250,{stringID
                res.WriteInt16((short)0);           //enchantment effect value            
                res.WriteInt16((short)0);     //unknown            

                //sub enchantment, values hidden unless viewed at enchant shop maybe
                numEntries = 5;
                for (int j = 0; j < 5; j++)
                {
                    res.WriteInt16((short)(0));          //Sub Enchant Scroll ID
                    res.WriteInt32(1);                  //misc field for displaying enchant removal / extraction I think: 0 - off, 1 - on, 5 percent sign, 6 remove, 7- extract
                    res.WriteInt32(7);                  //enchantment effect statement, 100,1250,{stringID
                    res.WriteInt16((short)3);           //enchantment effect value            
                    res.WriteInt16((short)i);           //unknown
                }

                res.WriteInt16(2);                  //enchant max cost allowance

                Router.Send(client, (ushort)AreaPacketId.recv_item_instance, res, ServerType.Area);

                res = BufferProvider.Provide();
                Router.Send(client, (ushort)AreaPacketId.recv_situation_end, res, ServerType.Area);

                res = BufferProvider.Provide();
                res.WriteUInt64(instanceId);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt16((short)i);
                //Router.Send(client, (ushort)AreaPacketId.recv_item_update_place, res, ServerType.Area);
            }

            


        }
    }
}
