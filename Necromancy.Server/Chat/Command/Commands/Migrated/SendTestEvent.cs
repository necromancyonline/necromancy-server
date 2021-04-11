using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command.Commands
{
    // many different things. To-Do.  break up things
    public class SendTestEvent : ServerChatCommand
    {
        public SendTestEvent(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            // Check the list to know what recv do
            // recv_data_notify_eventlink           spawn the aura for changing area for event
            // recv_data_notify_maplink             spawn the aura for changing map
            // recv_data_notify_goldobject_data     permit to get item or gold?
            // recv_data_notify_eo_data             permit to get the effect, of spell, and other things
            // recv_data_notify_ggate_stone_data    permit to acess object  or display name of object (when you acess object, it's like npc, you can have discussion and take choice),
            // recv_talkring_create_masterring_r    send a message in the shop that say you create a Master ring
            // recv_sixthsense_trap_notify          icon that avertise if a trap is around you
            // recv_event_system_message            Show system message on the middle of the screen
            // Recv event_message                   Permit to get dialogue message without name
            // Recv_event_message_no_object         permit to get the dialogue, with name, comment, and 1 other things that i don't know
            // recv_event_select_exec_winpos        open some windows with text, need recv_event_select_push to permit to get the choice like the other beelow ?
            // recv_event_select_exec               put it before the recv_event_select_push!! The recv_event_select_push, put the choice, the recv_event_select_exec take the choice in the window, and put a title
            // recv_event_request_int               open a pin code ?

            //recv_0xE8B9 = 0xE8B9,
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.character.instanceId);
            res.WriteFloat(4);
            router.Send(client, 0xE8B9, res, ServerType.Area);

            //--------------------------------------------------------------
            //recv_self_dragon_pos_notify = 0x6FB2;
            /*IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.InstanceId); //Works with character instance ID, might be able to use its own ID.

            res.WriteFloat(client.Character.X); // X
            res.WriteFloat(client.Character.Y); // Y
            res.WriteFloat(client.Character.Z); // Z
            //Location on minimap where the icon for a guardian statue is.

            res.WriteByte(1);//Bool? Shows a minimap icon if 1, doesn't if any other value.
            Router.Send(client, 0x6FB2, res, ServerType.Area);

            //--------------------------------------------------------------
            //recv_quest_hint = 0x505E,
            /*IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.InstanceId); // Quester's instance id

            res.WriteInt32(0); //
            //I think the above int32 is for quest text as our pop-up is missing text.
            res.WriteFloat(client.Character.X); // X
            res.WriteFloat(client.Character.Y); // Y
            res.WriteFloat(client.Character.Z); // Z
            //Location on minimap where the icon should show for a hint.

            res.WriteInt32(j++); //Hint "instance" ID, increasing this as it goes on will make it more show up instead of updating one.
            Router.Send(client, (ushort)AreaPacketId.recv_quest_hint, res, ServerType.Area);
            //--------------------------------------------------------------

            //recv_charabody_self_notify_abyss_stead_pos = 0x679B,
            /*IBuffer res = BufferProvider.Provide();
            res.WriteFloat(client.Character.X);
            res.WriteFloat(client.Character.Y);
            res.WriteFloat(client.Character.Z);
            Router.Send(client, (ushort)AreaPacketId.recv_charabody_self_notify_abyss_stead_pos, res, ServerType.Area);*
            //--------------------------------------------------------------

            //recv_monster_hate_on = 0x5C47
            /*IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0x38);//Monster instance ID, has to be a monster or causes a crash(the game knows it isn't if it isn't)
            res.WriteInt32(client.Character.InstanceId);//Player/Character instance ID
            Router.Send(client, (ushort)AreaPacketId.recv_monster_hate_on, res, ServerType.Area);
            //Causes the monster fight music to go off
            //--------------------------------------------------------------

            /*IBuffer res = BufferProvider.Provide(); // Show a panel "The scale will be available in"
            res.WriteInt32(90); // Time before the "scale" be available
            Router.Send(client, (ushort) AreaPacketId.recv_charabody_self_warpdragon_penalty, res, ServerType.Area);

            //--------------------------------------------------------------

            /*IBuffer res1 = BufferProvider.Provide(); // Message that signal a player stole an another player body
            res1.WriteByte(1);
            res1.WriteByte(0); // ??
            res1.WriteInt16(0); // ??

            res1.WriteInt16(80); // Pieces that the player stoles
            res1.WriteCString("PoorSoulPlayer"); // Length 0x31 // Name of stolen player ?
            res1.WriteCString($"{client.Character.Name}"); // Length 0x5B // Name of player
            Router.Send(client, (ushort)AreaPacketId.recv_charabody_notify_loot_item, res1, ServerType.Area);

            --------------------------------------------------------------

           /* IBuffer res2 = BufferProvider.Provide(); // "A new help item has added, and open the help menu"
            res2.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_charabody_notify_loot_start2, res2, ServerType.Area);

            /*IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(client.Character.InstanceId);
            res2.WriteInt32(4); //4 = chara dissapear, is dead ???
            res2.WriteInt32(10);
            Router.Send(client, (ushort)AreaPacketId.recv_charabody_notify_deadstate, res2, ServerType.Area);

            ------------------------------------------------------------

            /* IBuffer res = BufferProvider.Provide(); // It's the aura portal for event
             res.WriteInt32(0); // ID

             res.WriteFloat(client.Character.X); //x
             res.WriteFloat(client.Character.Y + 50); //y
             res.WriteFloat(client.Character.Z + 2); //z

             res.WriteByte(180);

             res.WriteFloat(client.Character.Y + 50); // Size
             res.WriteFloat(5); // distance

             res.WriteInt32(4); // Color
             Router.Send(client, (ushort) AreaPacketId.recv_data_notify_eventlink, res, ServerType.Area);

            ----------------------------------------------------------

             IBuffer res1 = BufferProvider.Provide(); // it's the aura portal for map
             res1.WriteInt32(2); // ID


             res1.WriteFloat(client.Character.X); //x
             res1.WriteFloat(client.Character.Y); //y
             res1.WriteFloat(client.Character.Z + 2); //z
             res1.WriteByte(180); // offset

             res1.WriteFloat(1000); // Size
             res1.WriteFloat(100); // distance

             res1.WriteInt32(0); // Type of aura   0 to 5, crash above 5
             Router.Send(client, (ushort) AreaPacketId.recv_data_notify_maplink, res1, ServerType.Area);

            ----------------------------------------------------------

             /* IBuffer res = BufferProvider.Provide();
              res.WriteInt32(1); // ID

              res.WriteFloat(client.Character.X);//X of the float text
              res.WriteFloat(client.Character.Y + 50);//Y of the float text
              res.WriteFloat(client.Character.Z + 120);//Z of the float text

              res.WriteFloat(client.Character.X);//X of the float text
              res.WriteFloat(client.Character.Y + 50);//Y of the float text
              res.WriteFloat(client.Character.Z + 120);//Z of the float text
              res.WriteByte(180); // view offest ?

              res.WriteInt32(0);
              res.WriteInt32(0);
              res.WriteInt32(0);

              res.WriteInt32(0); // Jump item animation
              Router.Send(client, (ushort)AreaPacketId.recv_data_notify_goldobject_data, res, ServerType.Area);

            ------------------------------------------------------------

              /*IBuffer res = BufferProvider.Provide();
              res.WriteInt32(0);// 0 or 1, other = crash
              res.WriteInt32(1);// ??
              res.WriteByte(1);// 0 = Text, 1 = F to examine  , other = dissapear the both, text and examine, but not the effect ?
              res.WriteCString("a");//"0x5B" first sentence of the text
              res.WriteCString("b");//"0x5B" second sentence
              res.WriteFloat(client.Character.X);//X of the float text
              res.WriteFloat(client.Character.Y +50);//Y of the float text
              res.WriteFloat(client.Character.Z + 120);//Z of the float text
              res.WriteByte(180);// view offset
              res.WriteInt32(2016001);// 0 = permit to see the examine and text but no models, to see models refer to the model_common.csv

              res.WriteInt16(100);//  size of the object


              res.WriteInt32(0);// 0 = collision, 1 = no collision ?(maybe), when you appear the things lool like object

              res.WriteInt32(2);//0= no effect color appear, blue = cleared, yellow = puzzle, red = ready for fight
              Router.Send(client, (ushort)AreaPacketId.recv_data_notify_ggate_stone_data, res, ServerType.Area);

            -----------------------------------------------------------


              /* IBuffer res = BufferProvider.Provide();
           res.WriteInt32(client.Character.InstanceId);
           res.WriteInt32(2);
           res.WriteCString("ToBeFound"); // find max size
           res.WriteCString("ToBeFound"); // find max size
           res.WriteFloat(client.Character.X);
           res.WriteFloat(client.Character.Y);
           res.WriteFloat(client.Character.Z);
           res.WriteByte(180);
           res.WriteInt32(0);

           int numEntries = 19;
           res.WriteInt32(numEntries);//less than or equal to 19
           for (int i = 0; i < numEntries; i++)
               res.WriteInt32(0);

           numEntries = 19;
           res.WriteInt32(0);
           for (int i = 0; i < numEntries; i++)
           {
               res.WriteInt32(0);
               res.WriteByte(0);
               res.WriteByte(0);
               res.WriteByte(0);
               res.WriteInt32(0);
               res.WriteByte(0);
               res.WriteByte(0);
               res.WriteByte(0);

               res.WriteByte(0);
               res.WriteByte(0);
               res.WriteByte(0);//bool
               res.WriteByte(0);
               res.WriteByte(0);
               res.WriteByte(0);
               res.WriteByte(0);
               res.WriteByte(0);
           }

           numEntries = 19;
           res.WriteInt32(0);
           for (int i = 0; i < numEntries; i++)
           {
               res.WriteInt32(0);
           }

           res.WriteInt32(0);
           res.WriteInt32(0);
           res.WriteByte(0);
           res.WriteByte(0);
           res.WriteByte(0);
           res.WriteInt32(0);
           res.WriteInt32(0);
           res.WriteInt32(0);
           res.WriteInt32(0);
           res.WriteByte(0);
           res.WriteByte(0);//bool
           res.WriteInt32(0);
           Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_charabody_data, res, ServerType.Area);


            /*
            IBuffer res0 = BufferProvider.Provide();
            res0.WriteInt32(0); //1 = cinematic, 0 Just start the event without cinematic
            res0.WriteByte(0);

            Router.Send(client, (ushort)AreaPacketId.recv_event_start, res0);  */


            /* IBuffer res = BufferProvider.Provide();
             res.WriteInt16(2);
             res.WriteByte(1);
             res.WriteInt32(1);
             int numEntries = 0xA;
             res.WriteInt32(numEntries);// less than or equal to 0xA
             for (int i = 0; i < numEntries; i++)
             {
                 res.WriteByte(1);
                 res.WriteInt32(1);
                 res.WriteFixedString("./interface/premiumservice/icon_%06d.dds", 0x19);
             }
             numEntries = 0x64;
             res.WriteInt32(numEntries);//less than or equal to 0x64
             for (int i = 0; i < numEntries; i++)
                 {
                 res.WriteByte(1);
                 res.WriteFixedString("./interface/premiumservice/icon_%06d.dds", 0x1F);
                 }
             Router.Send(client.Map, (ushort)AreaPacketId.recv_cash_shop_notify_open, res, ServerType.Area); /*



             /*  IBuffer res = BufferProvider.Provide();
               res.WriteCString($"{client.Soul.Name}");
               res.WriteCString($"{client.Character.Name}");
               Router.Send(client.Map, (ushort)AreaPacketId.recv_charabody_self_salvage_notify, res, ServerType.Area); */

            //recv_data_notify_maplink

            /*    IBuffer res = BufferProvider.Provide();
                res.WriteCString("ababab"); // Length 0xC01
                Router.Send(client, (ushort)AreaPacketId.recv_event_system_message, res, ServerType.Area);  show system message on middle of the screen.

                IBuffer res0 = BufferProvider.Provide();
                res0.WriteInt32(client.Character.InstanceId);
                Router.Send(client, (ushort)AreaPacketId.recv_gimmick_access_object_r, res0);

                IBuffer res1 = BufferProvider.Provide();
                res1.WriteInt32(105005);

                res1.WriteInt32(105005);

                res1.WriteInt32(105005);
                Router.Send(client, (ushort)AreaPacketId.recv_gimmick_access_object_notify, res1);              Maybe permit to spawn door and chair on the map ?


                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(105005);
                res.WriteFloat(-1175);
                res.WriteFloat(422);
                res.WriteFloat(-0);
                res.WriteByte(1);
                res.WriteInt32(105005);
                res.WriteInt32(105005);

                Router.Send(client, (ushort)AreaPacketId.recv_data_notify_gimmick_data, res, ServerType.Area);


                IBuffer res2 = BufferProvider.Provide();
                res2.WriteInt32(105005);
                res2.WriteInt32(105005);
                Router.Send(client, (ushort)AreaPacketId.recv_gimmick_state_update, res2);

                /* IBuffer res = BufferProvider.Provide();
                 res.WriteByte(1);
                 res.WriteByte(0);
                 res.WriteByte(0);

                 res.WriteCString("Hello My name is patrick");

                 Router.Send(client, (ushort)AreaPacketId.recv_dbg_message, res, ServerType.Area); * / Display message in the chat (only this function ?) Maybe message for equiped and unequiped item ? and use potion ?




               /*  IBuffer res2 = BufferProvider.Provide();

                 res2.WriteInt32(100006);

                 res2.WriteByte(0); // bool
                 Router.Send(client, (ushort)AreaPacketId.recv_event_select_ready, res2);

                 IBuffer res0 = BufferProvider.Provide();
                 res0.WriteCString("Cinematic test !"); // find max size  Text display at the top of the screen
                 res0.WriteInt32(100006);
                 Router.Send(client, (ushort)AreaPacketId.recv_event_show_board_start, res0); */


            /*  IBuffer res3 = BufferProvider.Provide();
              res3.WriteInt32(0);
              Router.Send(client, (ushort)AreaPacketId.recv_event_change_type, res3); */
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "test";
    }
}
