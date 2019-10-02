using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_chat_post_message : Handler
    {

        public send_chat_post_message(NecServer server) : base(server)
        {

        }

        public override ushort Id => (ushort)AreaPacketId.send_chat_post_message;

        int x = 0;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int ChatType = packet.Data.ReadInt32();     // 0 - Area, 1 - Shout, other todo...
            string To = packet.Data.ReadCString();
            string Message = packet.Data.ReadCString();
          



            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // errcode : 0 for success
            /*
            CHAT_MSG	GENERIC	Unknown statement error
            CHAT_MSG	2	You are unable to whisper to yourself
            CHAT_MSG	-802	You are not able to speak since you are not in a party
            CHAT_MSG	-1400	Unable to find Soul.  Reason: <errmsg>
            CHAT_MSG	-1401	You do not have an all-chat item
            CHAT_MSG	-1402	Action failed since it is on cool down.
            CHAT_MSG	-1403	You may not repeat phrases over and over in chat
            CHAT_MSG	-1404	You may not shout at Soul Rank 1
            CHAT_MSG	-1405	Action failed since it is on cool down.
            CHAT_MSG	-2201	Target refused to accept your whisper
            CHAT_MSG	-2202	Unable to whisper as you are on the recipient's Block List.
            CHAT_MSG	-3000	You may not chat in all-chat during trades.
            CHAT_MSG	-3001	You may not chat in all-chat while you have a shop open.
             */

            Router.Send(client, (ushort)AreaPacketId.recv_chat_post_message_r, res);
                

            
            //Parse "!" at the begging of a chat message to access the console commands below
            if (Message[0] == '!')
                Message = commandParse(client, Message);

            SendChatNotifyMessage(client, Message, ChatType);

        }

        /// <summary>
        /// Begin Console commands and Test funtions below.   To be or restricted to GM use at a later time.
        /// </summary>

        private void SendChatNotifyMessage(NecClient client, string Message, int ChatType)
        {

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(ChatType);
            res.WriteInt32(client.Character.Id);
            res.WriteFixedString($"{client.Soul.Name}", 49);
            res.WriteFixedString($"{client.Character.Name}", 37);
            res.WriteFixedString($"{Message}", 769);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_chat_notify_message, res);
        }

        private string commandParse(NecClient client, string Message)
        {
            string command = null;
            string[] SplitMessage = Message.Split('!', ':');
            int i = 1;
            long x = 0;
            bool cont = true;
            while (cont)
            {
                if (i == 4)
                {
                    cont = false;
                }
                command += Message[i];
                i++;
            }
            if (Message.Length >= 6)
            {
                string newString = null;

                for (int k = 0; k < Message.Length - 5; k++)
                {
                    newString += Message[k + 5];
                }

                Int64.TryParse(newString, out long newInt);

                x = newInt;
            }
            switch (command)
            {
                case "rbox":
                    SendRandomBoxNotifyOpen(client);
                    break;
                case "trap":
                    SendTrapEvent(client);
                    break;
                case "mess":
                    SendMessageEvent(client);
                    break;
                case "stor":
                    SendSoulStorageEvent(client);
                    break;
                case "salv":
                    SendSalvageNotifyBody(client);
                    break;
                case "ques":
                    QuestStarted(client); 
                    break;
                case "tbox":
                    SendEventTreasureboxBegin(client); 
                    break;
                case "gems":
                    GemNotifyOpen(client);
                    break;
                case "mant":
                    SendUnionMantleOpen(client);
                    break;
                case "unio":
                    SendUnionOpenWindow(client);
                    break;
                case "list":
                    SendWantedListOpen(client);
                    break;
                case "jail":
                    SendWantedJailOpen(client);
                    break;
                case "auct":
                    SendAuctionNotifyOpen(client);
                    break;
                case "shop":
                    SendShopNotifyOpen(client);
                    break;
                case "item":
                    SendDataNotifyItemObjectData(client);
                    break;
                case "mail":
                    SendMailOpenR(client);
                    break;
                case "chid":
                    SendCharacterId(client);
                    break;
                case "accs":
                    SendLootAccessObject(client);
                    break;
                /*case "move":
                    SendItemMove(client);
                    break;*/
                case "itis":
                    SendItemInstance(client, x);
                    break;
                case "itus":
                    SendItemInstanceUnidentified(client, x);
                    break;
                case "upit":
                    SendItemUpdateState(client);
                    break;
                case "stuf":
                    SendStallUpdateFeatureItem(client);
                    break;
                case "sssi":
                    SendStallSellItem(client);
                    break;               
                default:
                    command = "unrecognized";
                    break;
            }
  

            switch (SplitMessage[1])
            {
                case "NPC":
                    if (SplitMessage[2] == "") { SplitMessage[2] = "0"; }
                    AdminConsoleNPC(client, Convert.ToInt32(SplitMessage[2]));
                    break;
                case "Monster":
                    AdminConsoleRecvDataNotifyMonsterData(client);
                    break;
                case "Item":
                    SendDataNotifyItemObjectData(client);
                    break;
                case "Died":
                    IBuffer res4 = BufferProvider.Provide();
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_self_lost_notify, res4);
                    break;
                case "GetUItem":
                    AdminConsoleRecvItemInstanceUnidentified(client);
                    break;
                case "GetItem":
                    AdminConsoleRecvItemInstance(client);
                    break;
                case "SendMail":
                    SendMailOpenR(client);
                    break;
                case "GetMail":
                    AdminConsoleSelectPackageUpdate(client);
                    break;
                case "logout":
                    LogOut(client);
                    break;
                case "ReadFile":
                    FileReader.GameFileReader(client);
                    break;
                case "MapChange":
                    if (SplitMessage[2] == "") { SplitMessage[2] = "0"; }
                    SendMapChangeForce(client, Convert.ToInt32(SplitMessage[2]));
                    break;
                case "ChannelChange":
                    IBuffer res = BufferProvider.Provide();

                    res.WriteInt32(0);//Error
                    client.Character.MapId = 1001001;
                    //sub_4E4210_2341  // impacts map spawn ID
                    res.WriteInt32(client.Character.MapId);//MapSerialID
                    res.WriteInt32(client.Character.MapId);//MapID
                    res.WriteFixedString("127.0.0.1", 65);//IP
                    res.WriteInt16(60002);//Port

                    //sub_484420   //  does not impact map spawn coord
                    res.WriteFloat(0);//X Pos
                    res.WriteFloat(-8600);//Y Pos
                    res.WriteFloat(15000);//Z Pos
                    res.WriteByte(1);//View offset
                                     //

                    Router.Send(client, (ushort)MsgPacketId.recv_channel_select_r, res);
                    break;
                default:
                    SplitMessage[1] = "unrecognized";
                    //Message = $"Unrecognized command '{SplitMessage[1]}' ";
                    break;
            }
            if (command == "unrecognized" && SplitMessage[1] == "unrecognized")
            {
                Message = $"Unrecognized command - {Message}";
            }
            else
            {
                Message = $"Sent command - {Message}";
            }
            return Message;
        }



        private void AdminConsoleNPC(NecClient client, int ModelID)
        {
            if (ModelID <= 1) { ModelID = NPCModelID[Util.GetRandomNumber(1, 10)]; }

            IBuffer res3 = BufferProvider.Provide();
            res3.WriteInt32(NPCModelID[Util.GetRandomNumber(1, 10)]);   // NPC ID (object id)

            res3.WriteInt32((NPCSerialID[Util.GetRandomNumber(1, 10)]));      // NPC Serial ID from "npc.csv"

            res3.WriteByte(0);              // 0 - Clickable NPC (Active NPC, player can select and start dialog), 1 - Not active NPC (Player can't start dialog)

            res3.WriteCString($"Belong Here");//Name

            res3.WriteCString($"I Don't");//Title

            res3.WriteFloat(client.Character.X + Util.GetRandomNumber(25, 150));//X Pos
            res3.WriteFloat(client.Character.Y + Util.GetRandomNumber(25, 150));//Y Pos
            res3.WriteFloat(client.Character.Z);//Z Pos
            res3.WriteByte(client.Character.viewOffset);//view offset

            res3.WriteInt32(19);

            //this is an x19 loop but i broke it up
            res3.WriteInt32(24);
            res3.WriteInt32(1);
            res3.WriteInt32(1);
            res3.WriteInt32(1);
            res3.WriteInt32(1);
            res3.WriteInt32(1);
            res3.WriteInt32(1);
            res3.WriteInt32(1);
            res3.WriteInt32(1);
            res3.WriteInt32(1);
            res3.WriteInt32(1);
            res3.WriteInt32(1);
            res3.WriteInt32(1);
            res3.WriteInt32(1);
            res3.WriteInt32(1);
            res3.WriteInt32(1);
            res3.WriteInt32(1);
            res3.WriteInt32(1);
            res3.WriteInt32(1);

            res3.WriteInt32(19);


            int numEntries = 19;


            for (int i = 0; i < numEntries; i++)

            {
                // loop start
                res3.WriteInt32(210901); // this is a loop within a loop i went ahead and broke it up
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(3);

                res3.WriteInt32(10310503);
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(3);

                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(1); // bool
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(0);

            }

            res3.WriteInt32(19);

            //this is an x19 loop but i broke it up
            res3.WriteInt32(3);
            res3.WriteInt32(3);
            res3.WriteInt32(3);
            res3.WriteInt32(3);
            res3.WriteInt32(3);
            res3.WriteInt32(3);
            res3.WriteInt32(3);
            res3.WriteInt32(3);
            res3.WriteInt32(3);
            res3.WriteInt32(3);
            res3.WriteInt32(3);
            res3.WriteInt32(3);
            res3.WriteInt32(3);
            res3.WriteInt32(3);
            res3.WriteInt32(3);
            res3.WriteInt32(3);
            res3.WriteInt32(3);
            res3.WriteInt32(3);
            res3.WriteInt32(3);

            res3.WriteInt32(ModelID);   //NPC Model from file "model_common.csv"

            res3.WriteInt16(100);       //NPC Model Size

            res3.WriteByte(2);

            res3.WriteByte(5);

            res3.WriteByte(6);
            
            res3.WriteInt32(0); //Hp Related Bitmask?  This setting makes the NPC "alive"    11111110 = npc flickering, 0 = npc alive

            res3.WriteInt32(Util.GetRandomNumber(1, 9)); //npc Emoticon above head 1 for skull

            res3.WriteInt32(8);// add strange light on certain npc
            res3.WriteFloat(0); //x for icons
            res3.WriteFloat(0); //y for icons
            res3.WriteFloat(50); //z for icons

            res3.WriteInt32(128);

            int numEntries2 = 128;


            for (int i = 0; i < numEntries2; i++)

            {
                res3.WriteInt32(0);
                res3.WriteInt32(0);
                res3.WriteInt32(0);

            }

            Router.Send(client, (ushort)AreaPacketId.recv_data_notify_npc_data, res3);
        }
        private void SendItemInstanceUnidentified(NecClient client, long x)
        {
            //recv_item_instance_unidentified = 0xD57A,

            IBuffer res = BufferProvider.Provide();

            res.WriteInt64(50100102);//Item Object ID 

            res.WriteCString("HP Potion");//Name

            res.WriteInt32(45);//Wep type

            res.WriteInt32(1);

            res.WriteByte(8);//Number of items

            res.WriteInt32(0);//Item status 0 = identified  

            res.WriteInt32(50100102);//Item icon
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt32(1);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0); // bool
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(0);// 0 = adventure bag. 1 = character equipment
            res.WriteByte(0);// 0~2
            res.WriteInt16(3);// bag index

            res.WriteInt32(0);//bit mask. This indicates where to put items.   e.g. 01 head 010 arm 0100 feet etc (0 for not equipped)

            res.WriteInt64(x);

            res.WriteInt32(1);

            Router.Send(client, (ushort)AreaPacketId.recv_item_instance_unidentified, res);
        }

        private void SendStallSellItem(NecClient client)
        {
            //recv_stall_sell_item = 0x919C,
            IBuffer res = BufferProvider.Provide();

            res.WriteCString("C1Str"); // find max size Character name/Soul name
            res.WriteCString("CStr2"); // find max size Character name/Soul name
            res.WriteInt64(25);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt16(16);
            res.WriteInt32(client.Character.Id); //Item id

            Router.Send(client, (ushort)AreaPacketId.recv_stall_sell_item, res);
        }

        private void SendStallUpdateFeatureItem(NecClient client)
        {
            //recv_stall_update_feature_item = 0xB195,
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(client.Character.Id);

            res.WriteInt32(10200101);
            res.WriteByte(2);
            res.WriteByte(2);
            res.WriteByte(2);

            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt32(9);

            res.WriteByte(0); // bool

            res.WriteInt32(0);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_stall_update_feature_item, res);
        }

        private void SendItemUpdateState(NecClient client)
        {
            //recv_item_update_state = 0x3247, 
            IBuffer res = BufferProvider.Provide();

            res.WriteInt64(300000);//ItemID
            res.WriteInt32(200000);//Icon type, [x]00000 = certain armors, 1 = orb? 2 = helmet, up to 6

            Router.Send(client, (ushort)AreaPacketId.recv_item_update_state, res);
        }

        private void SendItemInstance(NecClient client, long x)
        {
            //recv_item_instance = 0x86EA,
            IBuffer res = BufferProvider.Provide();

            res.WriteInt64(69);//ItemID
            res.WriteInt32((int)x);//Icon type, [x]00000 = certain armors, 1 = orb? 2 = helmet, up to 6
            res.WriteByte(0);//Number of "items"
            res.WriteInt32(0);//Item status, in multiples of numbers, 8 = blessed/cursed/both 
            res.WriteFixedString("fixed", 0x10);
            res.WriteByte(0); // 0 = adventure bag. 1 = character equipment
            res.WriteByte(0); // 0~2 // maybe.. more bag index?
            res.WriteInt16(1);// bag index
            res.WriteInt32(0);//Slot spots? 10200101 here caused certain spots to have an item, -1 for all slots(avatar included)
            res.WriteInt32(1);//Percentage stat, 9 max i think
            res.WriteByte(1);
            res.WriteByte(3);
            res.WriteCString("cstring"); // find max size 
            res.WriteInt16(2);
            res.WriteInt16(1);
            res.WriteInt32(1);//Divides max % by this number
            res.WriteByte(1);
            res.WriteInt32(0);
            int numEntries = 2;
            res.WriteInt32(numEntries); // less than or equal to 2

            //for (int i = 0; i < numEntries; i++)
            res.WriteInt32(0);
            res.WriteInt32(0);

            numEntries = 3;
            res.WriteInt32(numEntries); // less than or equal to 3
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte(0); //bool
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
            }
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt32(0);//Guard protection toggle, 1 = on, everything else is off
            res.WriteInt16(0);

            Router.Send(client, (ushort)AreaPacketId.recv_item_instance, res);
        }

        private void SendLootAccessObject(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(-10);
            /*
                LOOT	-1	It is carrying nothing
                LOOT	-10	No one can be looted nearby
                LOOT	-207	No space available in inventory
                LOOT	-1500	No permission to loot
            */

            Router.Send(client, (ushort)AreaPacketId.recv_loot_access_object_r, res);
        }


        private void SendCharacterId(NecClient client)
        {
            string chid = client.Character.Id.ToString();
            SendChatNotifyMessage(client, chid, 0);
        }

        private void SendMailOpenR(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            //recv_mail_open_r = 0xCE7,
 
            res.WriteInt32(client.Character.Id);

            Router.Send(client, (ushort)AreaPacketId.recv_mail_open_r, res);
        }

        private void SendRandomBoxNotifyOpen(NecClient client)
        {
            //recv_random_box_notify_open = 0xC374,
            IBuffer res = BufferProvider.Provide();

            int numEntries = 10; // Slots
            res.WriteInt32(numEntries);//less than or equal to 10

            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt64(10001000100010002 + i); // ?

            }

            res.WriteInt32(itemIDs[x]); // Show item name                                                   


            Router.Send(client, (ushort)AreaPacketId.recv_random_box_notify_open, res);                                                 // Trying to spawn item in this boxe, maybe i need the item instance ?

            IBuffer res1 = BufferProvider.Provide();
            res1.WriteInt64(0); //
            res1.WriteInt32(itemIDs[x]);
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_state, res1);


        }

        private void SendTrapEvent(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0);

            Router.Send(client, (ushort)AreaPacketId.recv_event_start, res);

            IBuffer res2 = BufferProvider.Provide();

            res2.WriteInt32(10); // Percent

            res2.WriteInt32(0);

            res2.WriteByte(1); // bool  change chest image  1 = gold
            Router.Send(client, (ushort)AreaPacketId.recv_event_removetrap_begin, res2);

        }

        private void SendMessageEvent(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0);

            Router.Send(client, (ushort)AreaPacketId.recv_event_start, res);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(0);
            res2.WriteCString("Wake up Samurai we have a city to burn"); // find max size   show the text of the message
            Router.Send(client, (ushort)AreaPacketId.recv_event_message, res2);

        }
        private void SendSoulStorageEvent(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0);

            Router.Send(client, (ushort)AreaPacketId.recv_event_start, res);

            IBuffer res0 = BufferProvider.Provide();
            res0.WriteInt64(3); // Gold in the storage
            Router.Send(client, (ushort)AreaPacketId.recv_event_soul_storage_open, res0);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt64(0);
            Router.Send(client, (ushort)AreaPacketId.recv_self_money_notify, res2);

          /*  IBuffer res1 = BufferProvider.Provide();
            res1.WriteByte(1);
            Router.Send(client, (ushort)AreaPacketId.recv_event_end, res1); */

        }
        private void SendSalvageNotifyBody(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();  // it appear in the collected body
            res.WriteInt32(1); //  slots
            res.WriteCString($"{client.Soul.Name}"); // Soul Name
            res.WriteCString($"{client.Character.Name}"); // Character Name
            Router.Send(client, (ushort)AreaPacketId.recv_charabody_salvage_notify_body, res);
        }
        
        private void QuestStarted(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);	
            res.WriteByte(1); // Icon 1 = icon 0= white square

            res.WriteFixedString("The Adventurer's trial",0x61); // Quest Name
	
            res.WriteInt32(2); // Quest level
            res.WriteInt32(0);

            res.WriteFixedString("Aleache ",0x61); // NPC NAME
	
            res.WriteByte(1); // bool
            res.WriteByte(1); // bool

            res.WriteInt32(1);	// area name, side panel 1 = Illfalo port or maybe it get the map name ?
            res.WriteInt32(50); // Reward experience points
            res.WriteInt32(6); // Reward Gold
            res.WriteInt32(8); // Reward Skill points

            int numEntries = 0xA;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0); // need to find
                res.WriteFixedString("test",0x10); // ?
                res.WriteInt16(0);
                res.WriteInt32(0); // need to find too 
            }
	
            res.WriteByte(0);
	
            int numEntries2 = 0xC;
            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteInt32(100202); // put item in selected prize, maybe item id ?
                res.WriteFixedString("talk",0x10); // ?
                res.WriteInt16(0);
                res.WriteInt32(8); // Cursed, blessed, ect... 8 = cursed
            }
	
            res.WriteByte(4); // Item slots for Selected Prize   0 = no Selected prize.
	
            res.WriteFixedString("To be considered an adventurer, you'll need to pass a trial. talk to " + $"{client.Character.Name}" + " To take the test.",0x181); // Quest comment
	
            res.WriteInt64(0); // ?
	
            res.WriteByte(0);
		
            res.WriteFixedString("Complete the trial and find the (Item Name, Mob Name ect...) ",0x181);  // Completion Requirements
	
            int numEntries3 = 0x5;
            for (int i = 0; i < numEntries3; i++)
            {
                res.WriteByte(0); // Type of quest. 0 = Defeat, 1 = Collect, 2 = head toward the designated area. ECT....
                res.WriteInt32(2); // Monster Name for the defeat type. (And name of other find, refere to.....)
                res.WriteInt32(5); // Monster Defeat/ items Collect/ ect.. first number
                res.WriteInt32(10); // Monster Defeat/ items Collect/ ect..  second number
                res.WriteInt32(0);
                res.WriteInt32(0);
            }
	
            res.WriteByte(1); //  0 = Desactivate.  1 = (The goal of the quest, to get the reward after finish it.) (it need to be activate, for monster defeat, collect, ect...)
	
            res.WriteInt32(0);
	
            res.WriteFloat(0);
            Router.Send(client, (ushort)AreaPacketId.recv_quest_started, res);
        }
        
        private void GemNotifyOpen(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_gem_notify_open, res);
        }
        private void SendUnionMantleOpen(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            for (int i = 0; i < 0x10; i++)
            {
                res.WriteByte(0);
            }
            Router.Send(client, (ushort)AreaPacketId.recv_union_mantle_open, res);
        }
        private void SendUnionOpenWindow(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_union_open_window, res);
        }
        private void SendWantedListOpen(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(9999); // Bounty points.
            res.WriteInt32(0);
            res.WriteInt32(0);  // When i change list doesn't open anymore, don't know what is it
            Router.Send(client, (ushort)AreaPacketId.recv_wanted_list_open, res);
        }
        private void SendWantedJailOpen(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1); // 1 make all 3 option availabe ?  and 0 unvailable ?

            res.WriteInt64(70); // Bail
            Router.Send(client, (ushort)AreaPacketId.recv_wanted_jail_open, res);
        }

        private void SendShopNotifyOpen(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(20);
            /* Shop ID, 0 it's forge, 1 it's cursed, 2 Purchase shop, 3 purchase and curse, 4 it's sell, 
                        5 sell and curse. 6 purchase and sell. 7 Purchase, Sell, Curse.
                        
                        8 Identify. 9 identify & curse. 10 Purchase & Identify.
                        
                        11 Purchase, Identify & Curse. 12 Sell And Identify
                        
                        13 Sell, Identify & Curse, 14 Purchase, Sell & Identify
                        
                        15 All of what i say before except the forge.
                        
                        16 Repair ! 17 repair and curse. 18 Repair and purchase;
                        
                        19 repair, purchase, cursed. 20 Repair and sell
           */
            res.WriteInt32(0); // don't know
            res.WriteInt32(0); // don't know too
            res.WriteByte(0); // Don't know too
            Router.Send(client, (ushort)AreaPacketId.recv_shop_notify_open, res);
        }

        private void SendAuctionNotifyOpen(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(5); // must be <= 5

            int numEntries = 0x5;
            for (int i = 0; i < numEntries; i++)
            {

                res.WriteByte(0); // Slots 0 to 2
                // res.WriteInt64(0xAAAAAAAA); // this is shown in the structure but for some reason isnt read

                res.WriteInt32(0);
                res.WriteInt64(0);
                res.WriteInt32(0); // Lowest
                res.WriteInt32(0); // Buy Now
                res.WriteFixedString($"{client.Soul.Name}", 49);
                res.WriteByte(1); // 1 permit to show item in the search section ??
                res.WriteFixedString("Comment", 385); // Comment in the item information
                res.WriteInt16(0); // Bid
                res.WriteInt32(0);

                res.WriteInt32(0);
                res.WriteInt32(0);

            }

            res.WriteInt32(8); // must be< = 8

            int numEntries2 = 0x8;
            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteByte(0); // Slots 0 to 3
                // res.WriteInt64(0xAAAAAAAA); // this is shown in the structure but for some reason isnt read

                res.WriteInt32(0);
                res.WriteInt64(0);
                res.WriteInt32(0); // Lowest bid info
                res.WriteInt32(0); // Buy now bid info
                res.WriteFixedString($"{client.Soul.Name}", 49);
                res.WriteByte(1); // Change nothing ??
                res.WriteFixedString("Zgeg", 385); // Comment in the bid info
                res.WriteInt16(0);
                res.WriteInt32(0);

                res.WriteInt32(0); // Bid Amount (bid info seciton)
                res.WriteInt32(0);

            }

            res.WriteByte(0); // bool
            Router.Send(client, (ushort)AreaPacketId.recv_auction_notify_open, res);
        }

        private void SendEventTreasureboxBegin(NecClient client)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(0);
            res2.WriteByte(0);

            Router.Send(client, (ushort)AreaPacketId.recv_event_start, res2);


            IBuffer res3 = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_event_sync, res3);


            //recv_event_tresurebox_begin = 0xBD7E,
            IBuffer res = BufferProvider.Provide();

            int numEntries = 0x10;
            res.WriteInt32(numEntries);

            //for loop of 0x10
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            res.WriteInt32(100101);

            Router.Send(client, (ushort)AreaPacketId.recv_event_tresurebox_begin, res);


         /*   IBuffer res4 = BufferProvider.Provide();
            res4.WriteByte(3);
            Router.Send(client, (ushort)AreaPacketId.recv_event_end, res4); */
        }

        private void SendDataNotifyItemObjectData(NecClient client)
        {
            //SendDataNotifyItemObjectData
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(251001);//Object ID
            res.WriteFloat(client.Character.X);//Initial X
            res.WriteFloat(client.Character.Y);//Initial Y
            res.WriteFloat(client.Character.Z);//Initial Z

            res.WriteFloat(client.Character.X);//Final X
            res.WriteFloat(client.Character.Y);//Final Y
            res.WriteFloat(client.Character.Z);//Final Z
            res.WriteByte(client.Character.viewOffset);//View offset

            res.WriteInt32(0);// 0 here gives an indication (blue pillar thing) and makes it pickup-able
            res.WriteInt32(0);
            res.WriteInt32(0);

            res.WriteInt32(255);//Anim: 1 = fly from the source. (I think it has to do with odd numbers, some cause it to not be pickup-able)

            res.WriteInt32(0);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_itemobject_data, res);
        }

        private void LogOut(NecClient client)
        {
            byte[] byteArr = new byte[8] { 0x00, 0x06, 0xEE, 0x91, 0, 0, 0, 0 };

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            res.SetPositionStart();

            for (int i = 4; i < 8; i++)
            {
                byteArr[i] += res.ReadByte();
            }

            client.Session.msgSocket.Send(byteArr);

            System.Threading.Thread.Sleep(4000);

            byte[] byteArrr = new byte[9] { 0x00, 0x07, 0x52, 0x56, 0, 0, 0, 0, 0 };

            IBuffer res2 = BufferProvider.Provide();

            res2.WriteInt32(0);
            res2.WriteByte(0);

            res2.SetPositionStart();

            for (int i = 4; i < 9; i++)
            {
                byteArrr[i] += res2.ReadByte();
            }

            client.Session.msgSocket.Send(byteArrr);


        }
        private void AdminConsoleSelectPackageUpdate(NecClient client)
        {
            int errcode = 0;
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(errcode);//Error message Call. 0 for success. see additional options in Sys_msg.csv
                                    /*
                                    1	You have unopened mails	SYSTEM_WARNING
                                    2	No mails to delete	SYSTEM_WARNING
                                    3	You have %d unreceived mails. Please check your inbox.	SYSTEM_WARNING
                                    -2414	Mail title includes banned words	SYSTEM_WARNING

                                    */

            Router.Send(client, (ushort)AreaPacketId.recv_select_package_update_r, res);

        }

        private void AdminConsoleRecvDataNotifyMonsterData(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(Util.GetRandomNumber(55566, 55888));

            res.WriteCString($"Demon Bardock");//Name while spawning

            res.WriteCString($"Titan");//Title

            res.WriteFloat(client.Character.X + Util.GetRandomNumber(25, 150));//X Pos
            res.WriteFloat(client.Character.Y + Util.GetRandomNumber(25, 150));//Y Pos
            res.WriteFloat(client.Character.Z);//Z Pos
            res.WriteByte(client.Character.viewOffset);//view offset

            res.WriteInt32(70101); // Monster serial ID.  70101 for Lesser Demon.  If this is invalid, you can't "loot" the monster or see it's first CString

            res.WriteInt32(2070001); // Model from model_common.csv  2070001 for Lesser Demon

            res.WriteInt16(100); //model size

            res.WriteInt32(0x10); // cmp to 0x10 = 16

            int numEntries = 0x10;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(1);
            }

            res.WriteInt32(0x10); // cmp to 0x10 = 16

            int numEntries2 = 0x10;
            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteInt32(70101); // this was an x2 loop (i broke it down)
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteInt32(70101);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);

                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1); // bool
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);

                res.WriteByte(1);
            }

            res.WriteInt32(0x10); // cmp to 0x10 = 16

            int numEntries3 = 0x10; //equipment slots to display?
            for (int i = 0; i < numEntries3; i++)

            {
                res.WriteInt64(10001000100010002 + i);
            }

            res.WriteInt32(0); //1000 0000 here makes it stand up and not be dead.

            res.WriteInt64(1);

            res.WriteInt64(1);

            res.WriteInt64(1);

            res.WriteByte(0);

            res.WriteByte(0);

            res.WriteInt32(1);

            res.WriteInt32(1);

            res.WriteInt32(0x80); // cmp to 0x80 = 128

            int numEntries4 = 0x80;  //Statuses?
            for (int i = 0; i < numEntries4; i++)

            {
                res.WriteInt32(1);
                res.WriteInt32(1);
                res.WriteInt32(1);
            }



            Router.Send(client, (ushort)AreaPacketId.recv_data_notify_monster_data, res);

            IBuffer res3 = BufferProvider.Provide();
            res3.WriteInt32(client.Character.Id);
            Router.Send(client, (ushort)AreaPacketId.recv_charabody_access_start_r, res3);

            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(1);
            res4.WriteInt32(1);
            Router.Send(client, (ushort)AreaPacketId.recv_charabody_loot_start2_r, res4);

            IBuffer res5 = BufferProvider.Provide();
            res5.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_charabody_notify_loot_start2, res5);

            IBuffer res1 = BufferProvider.Provide();
            res1.WriteInt32(70101);

            res1.WriteInt32(1);
            Router.Send(client, (ushort)AreaPacketId.recv_monster_hate_on, res1);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(70101);

            res2.WriteInt32(1);
            Router.Send(client, (ushort)AreaPacketId.recv_monster_state_update_notify, res2);


        }

        private void AdminConsoleRecvItemInstanceUnidentified(NecClient client)
        {
            x = 0;
            for (int i = 0; i < 19; i++)
            {
                System.Threading.Thread.Sleep(100);
                //recv_item_instance_unidentified = 0xD57A,
                IBuffer res = BufferProvider.Provide();

                res.WriteInt64(10001000100010002 + i);

                res.WriteCString($"ID:{itemIDs[x]} MSK:{EquipBitMask[x]} Type:{EquipItemType[x]}"); // Item Name

                res.WriteInt32(EquipItemType[x] - 1); // Item Type. Refer To ItemType.csv   
                res.WriteInt32(EquipBitMask[x]); //Slot Limiting Bitmask.  Limits  Slot Item can be Equiped.

                res.WriteByte(1); // Numbers of items

                res.WriteInt32(EquipBitMask[Util.GetRandomNumber(1, 8)]); /* 10001003 Put The Item Unidentified. 0 put the item Identified 1-2-4-8-16 follow this patterns (8 cursed, 16 blessed)*/


                res.WriteInt32(itemIDs[x]);  //Item ID for Icon
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt32(1);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(1); // bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(2);                       // 0 = adventure bag. 1 = character equipment, 2 = royal bag
                res.WriteByte(0);                       // 0~2
                res.WriteInt16((short)x);               // bag index 0 to 24

                res.WriteInt32(EquipBitMask[x]); //bit mask. This indicates where to put items.  

                res.WriteInt64(1111111111111110);

                res.WriteInt32(1);
                x++;


                Router.Send(client, (ushort)AreaPacketId.recv_item_instance_unidentified, res);


                IBuffer res0 = BufferProvider.Provide();
                res0.WriteInt64(10001000100010002 + i);
                res0.WriteInt32(Util.GetRandomNumber(100,200)); // MaxDura points
                Router.Send(client, (ushort)AreaPacketId.recv_item_update_maxdur, res0);


                IBuffer res2 = BufferProvider.Provide();  // Maybe not the good one ?
                res2.WriteInt64(10001000100010002 + i);
                res2.WriteInt32(Util.GetRandomNumber(1, 200)); // Durability points
                Router.Send(client, (ushort)AreaPacketId.recv_item_update_durability, res2);


                IBuffer res4 = BufferProvider.Provide();
                res4.WriteInt64(10001000100010002 + i);
                res4.WriteInt32(Util.GetRandomNumber(800, 10000)); // Weight points
                Router.Send(client, (ushort)AreaPacketId.recv_item_update_weight, res4);

 
                IBuffer res5 = BufferProvider.Provide();
                res5.WriteInt64(10001000100010002 + i);
                res5.WriteInt16((short)Util.GetRandomNumber(5, 500)); // Defense and attack points
                Router.Send(client, (ushort)AreaPacketId.recv_item_update_physics, res5);


                IBuffer res6 = BufferProvider.Provide();
                res6.WriteInt64(10001000100010002 + i);
                res6.WriteInt16((short)Util.GetRandomNumber(5, 500)); // Magic def and attack Points
                Router.Send(client, (ushort)AreaPacketId.recv_item_update_magic, res6);


                IBuffer res7 = BufferProvider.Provide();
                res7.WriteInt64(10001000100010002 + i);
                res7.WriteInt32(Util.GetRandomNumber(1, 10)); // for the moment i don't know what it change
                Router.Send(client, (ushort)AreaPacketId.recv_item_update_enchantid, res7);


                IBuffer res8 = BufferProvider.Provide();
                res8.WriteInt64(10001000100010002 + i);
                res8.WriteInt16((short)Util.GetRandomNumber(0, 100000)); // Shwo GP on certain items
                Router.Send(client, (ushort)AreaPacketId.recv_item_update_ac, res8);

                IBuffer res9 = BufferProvider.Provide();
                res9.WriteInt64(10001000100010002 + i);
                res9.WriteInt32(Util.GetRandomNumber(1, 50)); // for the moment i don't know what it change
                Router.Send(client, (ushort)AreaPacketId.recv_item_update_date_end_protect, res9);

                IBuffer res1 = BufferProvider.Provide();
                res1.WriteInt64(0); //10001000100010002 + i   put stuff unidentified and get the status equipped  , 0 put stuff identified
                res1.WriteInt32(itemIDs[x]);
                Router.Send(client, (ushort)AreaPacketId.recv_item_update_state, res1);


            }
        }


        private void AdminConsoleRecvItemInstance(NecClient client)
        {
            x = 0;
            for (int j = 0; j < 19; j++)
            {
                IBuffer res = BufferProvider.Provide();
                //recv_item_instance = 0x86EA,
                x++;
                res.WriteInt64(1000200030004001 + x);  //  Assume Unique ID instance identifier. 1 here makes item green icon
                res.WriteInt32(EquipItemType[x] - 1);
                res.WriteByte(1);               //number of items in stack
                res.WriteInt32(itemIDs[x]);       //
                res.WriteFixedString("WhatIsThis", 0x10);
                res.WriteByte(0);                       // 0 = adventure bag. 1 = character equipment
                res.WriteByte(0);                       // 0~2
                res.WriteInt16((short)x);               // bag index
                res.WriteInt32(EquipBitMask[x]);        //bit mask. This indicates where to put items.   ??
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteCString("ThisIsThis"); // find max size 
                res.WriteInt16(0);
                res.WriteInt16(0);
                res.WriteInt32(itemIDs[x]);
                res.WriteByte(0);
                res.WriteInt32(itemIDs[x]);
                int numEntries = 2;
                res.WriteInt32(numEntries); // less than or equal to 2
                for (int i = 0; i < numEntries; i++)
                {
                    res.WriteInt32(itemIDs[x]);
                }
                numEntries = 3;
                res.WriteInt32(numEntries); // less than or equal to 3
                for (int i = 0; i < numEntries; i++)
                {
                    res.WriteByte(0); //bool
                    res.WriteInt32(itemIDs[x]);
                    res.WriteInt32(itemIDs[x]);
                    res.WriteInt32(itemIDs[x]);
                }
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt16(0);
                res.WriteInt32(1);  //1 here lables the item "Gaurd".   no effect from higher numbers
                res.WriteInt16(0);

                Router.Send(client, (ushort)AreaPacketId.recv_item_instance, res);
            }

        }

        private void SendMapChangeForce(NecClient client, int MapID)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);
            res.WriteInt32(MapID);
            res.WriteFixedString("127.0.0.1", 65);//IP
            res.WriteInt16(60002);//Port

            res.WriteFloat(100);//x coord
            res.WriteFloat(100);//y coord
            res.WriteFloat(100);//z coord
            res.WriteByte(1);//view offset maybe?

            Router.Send(client, (ushort)AreaPacketId.recv_map_change_force, res);

            SendMapChangeSyncOk(client);
            client.Character.MapId = MapID;
        }

        private void SendMapChangeSyncOk(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            Router.Send(client, (ushort)AreaPacketId.recv_map_change_sync_ok, res);

            //Add a wait statement here
        }
    

        /////////Int array for testing Item ID's. 
        int[] itemIDs = new int[] {10800405/*Weapon*/,15100901/*Shield* */,20000101/*Arrow*/,110301/*head*/,210701/*Torso*/,360103/*Pants*/,401201/*Hands*/,560103/*Feet*/,690101/*Cape*/
                    ,30300101/*Necklace*/,30200107/*Earring*/,30400105/*Belt*/,30100106/*Ring*/,70000101/*Talk Ring*/,160801/*Avatar Head */,260801/*Avatar Torso*/,360801/*Avatar Pants*/,460801/*Avatar Hands*/,560801/*Avatar Feet*/,1,2,3 };
        int[] NPCModelID = new int[] { 1911105, 1112101, 1122401, 1122101, 1311102, 1111301, 1121401, 1131401, 2073002, 1421101 };
        int[] NPCSerialID = new int[] { 10000101, 10000102, 10000103, 10000104, 10000105, 10000106, 10000107, 10000108, 80000009, 10000101 };
        //int[] EquipBitMask = new int[] { 0b1, 0b10, 0b100, 0b1000, 0b10000, 0b100000, 0b1000000, 0b10000000, 0b100000000, 0b1000000000, 0b10000000000, 0b100000000000, 0b1000000000000, 0b10000000000000, 0b100000000000000, 0b10000000000000000, 0b10000000000000000, 0b1000000000000000000, 0b10000000000000000000 };
        int[] EquipBitMask = new int[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288, 1048576, 2097152 };
        int[] EquipItemType = new int[] { 9, 21, 23, 28, 31, 32, 36, 40, 41, 44, 43, 45, 42, 54, 62, 62, 62, 62, 62, 62, 62, 62 };
        int[] EquipStatus = new int[] { 0, 1, 2, 4, 8, 16 };

    }
}
