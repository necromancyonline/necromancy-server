using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Auction_House.Logic;
using System;
using System.Text;

namespace Necromancy.Server.Systems.Auction_House
{
    public class send_auction_search : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_auction_search));

        public send_auction_search(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_auction_search;

        public override void Handle(NecClient client, NecPacket packet)
        {
            SearchCriteria searchCriteria = new SearchCriteria();
            searchCriteria.SoulRankMin = packet.Data.ReadByte();
            searchCriteria.SoulRankMax = packet.Data.ReadByte();
            searchCriteria.ForgePriceMin = packet.Data.ReadByte();
            searchCriteria.ForgePriceMax = packet.Data.ReadByte();
            searchCriteria.Quality = (SearchCriteria.Qualities) packet.Data.ReadInt16();
            searchCriteria.Class = (SearchCriteria.Classes) packet.Data.ReadInt16();

            Logger.Info("YEFAS2F");
            int NUMBER_OF_ITEMS_DEBUG = 20;

            for (int i = 0; i < NUMBER_OF_ITEMS_DEBUG; i++)
            {

                IBuffer r1 = BufferProvider.Provide();
                r1.WriteInt64(i + 300);                     //spawned item iD
                r1.WriteInt32((int)00100101 + i);          //item id
                r1.WriteByte((byte) 1);                     //quantity
                r1.WriteInt32((int) ItemStatuses.Normal);   //Item status
                r1.WriteFixedString("Test Bruh", 16);                //unknown
                r1.WriteByte((byte) (ItemZone.AdventureBag));      //item zone
                r1.WriteByte((byte)( i  / 50));             //bag number                
                r1.WriteInt16((short)( i % 50));            //bag slot or slot for bags to go in
                r1.WriteInt32((int)ItemEquipSlot.None);     //slot equipped to
                r1.WriteInt32((int)20);                     //current durability
                r1.WriteByte((byte)i);                      //enhancement level +1, +2 etc
                r1.WriteByte(0);                            //special forge level, must be less than or equal the the req special forge level in table
                r1.WriteCString("Nani?!");                        //unknown
                r1.WriteInt16((short)5);                    //phys attr (attack, def)
                r1.WriteInt16((short)5);                    //mag attr (mattack, mdef)
                r1.WriteInt32((int)21);                     //maximum durability
                r1.WriteByte((byte)8);                      //hardness
                r1.WriteInt32((int)501);                      //unknown

                const int MAX_WHATEVER_SLOTS = 2;
                const int numEntries = 2;
                r1.WriteInt32(numEntries);                  //less than or equal to 2
                for (int j = 0; j < numEntries; j++)
                {
                    r1.WriteInt32((byte)8);                 //unknown
                }

                const int MAX_GEM_SLOTS = 3;
                const int numGemSlots = 1;
                r1.WriteInt32(numGemSlots);                 //number of gem slots
                for (int j = 0; j < numGemSlots; j++)
                {
                    r1.WriteByte(1);                        //is gem slot filled
                    r1.WriteInt32(3);                       //slot type 1 round, 2 triangle, 3 diamond
                    r1.WriteInt32(100001);                       //gem item id
                    r1.WriteInt32(100003);                       //maybe gem item 2 id for diamon 2 gem combine
                }

                r1.WriteInt32(3);                          //unknown
                r1.WriteInt32(200);                         //unknown
                r1.WriteInt16((short)50);                 //unknown
                r1.WriteInt32(2);                           //enchant id 
                r1.WriteInt16((short)2000);                 //GP

                IBuffer r0 = BufferProvider.Provide();
                r0.WriteInt64(i + 300); //V | SPAWN ID
                r0.WriteCString("? Helmet"); // V | DISPLAY NAME
                r0.WriteInt32((int)ItemType.HELMET); // V | ITEM TYPE
                r0.WriteInt32((int)ItemEquipSlot.Head); // equip slot display on icon AND WHERE CLIENT SAYS YOU CAN EQUIP
                r0.WriteByte((byte) 1); //V | QUANTITY
                r0.WriteInt32((int) ItemStatuses.IsUnidentified); //V | STATUS, CURSED / BESSED / ETC
                r0.WriteInt32(00100101); //Item icon 50100301 = camp | base item id | leadher guard 100101 | 50100502 bag medium | 200901 soldier cuirass | 90012001 bag?  always 8

                r0.WriteByte((byte)byte.MaxValue); //unknown
                r0.WriteByte((byte)5); //unknown
                r0.WriteByte((byte)5); //unknown

                r0.WriteInt32(00100101); // base item id? tested a bit
                r0.WriteByte((byte)5); //unknown 
                r0.WriteByte((byte)5); //unknown
                r0.WriteByte((byte)5); //unknown

                r0.WriteByte(5); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                r0.WriteByte(5); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                r0.WriteByte(5); // bool
                r0.WriteByte((byte)5); //unknown
                r0.WriteByte((byte)5); //unknown
                r0.WriteByte((byte)5); //unknown
                r0.WriteByte((byte)5); //texture related

                r0.WriteByte((byte)5); //unknown
                    
                r0.WriteByte((byte) (ItemZone.AdventureBag + i)); // 0 = adventure bag. 1 = character equipment 2 = Royal bag. _inventoryItem.StorageType
                r0.WriteByte((byte)(i / 50)); // V | bag number
                r0.WriteInt16((short)(i % 50)); // V |  slot in bag
                r0.WriteInt32((int) ItemEquipSlot.None); // V | equips item to this slot ItemEquipSlot items not in zone adventure bag, character equipment, and royal bag, avatar bag (maybe more) cannot be equipped.
                r0.WriteInt64(5000); //unknown tested a bit maybe sale price?
                r0.WriteInt32(1597618552); // V | protect expiry datetime in seconds

                //Router.Send(client.Map, (ushort)AreaPacketId.recv_item_instance, r1, ServerType.Area);
                Router.Send(client.Map, (ushort)AreaPacketId.recv_item_instance_unidentified, r0, ServerType.Area);

               

                IBuffer r4 = BufferProvider.Provide();
                r4 = BufferProvider.Provide();
                r4.WriteInt64(i + 300);
                r4.WriteInt32(i); // guard
                //Router.Send(client, (ushort)AreaPacketId.recv_item_update_enchantid, r4, ServerType.Area);

                IBuffer r5 = BufferProvider.Provide();
                r5 = BufferProvider.Provide();
                r5.WriteInt64(i + 300);
                r5.WriteInt16((short) i); // Shwo GP on certain items guard points
                //Router.Send(client, (ushort)AreaPacketId.recv_item_update_ac, r5, ServerType.Area);

                IBuffer r6 = BufferProvider.Provide();
                r6 = BufferProvider.Provide();
                r6.WriteInt64(i + 300); //client.Character.EquipId[x]  put stuff unidentified and get the status equipped  , 0 put stuff identified
                r6.WriteInt32((int)ItemStatuses.Normal);
                //Router.Send(client, (ushort)AreaPacketId.recv_item_update_state, r6, ServerType.Area);

                IBuffer r7 = BufferProvider.Provide();
                r7.WriteInt64(i + 300);
                r7.WriteInt32(i); // for the moment i don't know what it change
                //Router.Send(client, (ushort)AreaPacketId.recv_item_update_date_end_protect, r7, ServerType.Area);         
            }

            Logger.Info("YEFASF");
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error

            res.WriteInt32(NUMBER_OF_ITEMS_DEBUG); // number of loops
            
            for (int i = 0; i < NUMBER_OF_ITEMS_DEBUG; i++)
            {
                string hellothere = i.ToString() + " " + Convert.ToString(i, 2).PadLeft(8, '0'); ;
                res.WriteInt32(i); //row identifier 
                res.WriteInt64(i + 300); //spawned item id
                res.WriteInt32(17); // Lowest
                res.WriteInt32(500); // Buy Now
                res.WriteFixedString(hellothere, 49); // Soul Name Of Sellers
                res.WriteByte(8); // 0 = nothing.    Other = Logo appear. maybe it's effect or rank, or somethiung else ?
                res.WriteFixedString("", 385); // Item Comment
                res.WriteInt16(0); // Bid
                res.WriteInt32(1000); // Item remaining time
            }

            Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_search_r, res, ServerType.Area);

            IBuffer res19 = BufferProvider.Provide();
            res19.WriteInt32(00100101); //soul_dispitem.csv
            Router.Send(client, (ushort)AreaPacketId.recv_soul_dispitem_notify_data, res19, ServerType.Area);

        }

        
             
    }
}
