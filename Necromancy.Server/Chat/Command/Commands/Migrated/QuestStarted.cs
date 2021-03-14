using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //accepts a quest to defeat 10 hydras
    public class QuestStarted : ServerChatCommand
    {
        public QuestStarted(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(1); // Icon 1 = icon 0= white square

            res.WriteFixedString("The Adventurer's trial", 0x61); // Quest Name

            res.WriteInt32(2); // Quest level
            res.WriteInt32(0);

            res.WriteFixedString("Aleache ", 0x61); // NPC NAME

            res.WriteByte(1); // bool
            res.WriteByte(1); // bool

            res.WriteInt32(1); // area name, side panel 1 = Illfalo port or maybe it get the map name ?
            res.WriteInt32(50); // Reward experience points
            res.WriteInt32(6); // Reward Gold
            res.WriteInt32(8); // Reward Skill points

            int numEntries = 0xA;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0); // need to find
                res.WriteFixedString("test", 0x10); // ?
                res.WriteInt16(0);
                res.WriteInt32(0); // need to find too 
            }

            res.WriteByte(0);

            int numEntries2 = 0xC;
            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteInt32(100202); // put item in selected prize, maybe item id ?
                res.WriteFixedString("talk", 0x10); // ?
                res.WriteInt16(0);
                res.WriteInt32(8); // Cursed, blessed, ect... 8 = cursed
            }

            res.WriteByte(4); // Item slots for Selected Prize   0 = no Selected prize.

            res.WriteFixedString(
                "To be considered an adventurer, you'll need to pass a trial. talk to " + $"{client.Character.Name}" +
                " To take the test.", 0x181); // Quest comment

            res.WriteInt64(0); // ?

            res.WriteByte(0);

            res.WriteFixedString("Complete the trial and find the (Item Name, Mob Name ect...) ",
                0x181); // Completion Requirements

            int numEntries3 = 0x5;
            for (int i = 0; i < numEntries3; i++)
            {
                res.WriteByte(
                    0); // Type of quest. 0 = Defeat, 1 = Collect, 2 = head toward the designated area. ECT....
                res.WriteInt32(2); // Monster Name for the defeat type. (And name of other find, refere to.....)
                res.WriteInt32(5); // Monster Defeat/ items Collect/ ect.. first number
                res.WriteInt32(10); // Monster Defeat/ items Collect/ ect..  second number
                res.WriteInt32(0);
                res.WriteInt32(0);
            }

            res.WriteByte(1); //  0 = Desactivate.  1 = (The goal of the quest, to get the reward after finish it.) (it need to be activate, for monster defeat, collect, ect...)

            res.WriteInt32(0);

            res.WriteFloat(0);
            Router.Send(client, (ushort) AreaPacketId.recv_quest_started, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "ques";
    }
}
