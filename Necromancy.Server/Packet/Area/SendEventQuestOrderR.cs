using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class SendEventQuestOrderR : ClientHandler
    {
        public SendEventQuestOrderR(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_event_quest_order_r;

        public override void Handle(NecClient client, NecPacket packet)
        {



            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);
            res.WriteByte(1); // 0 Show the soul level mission, 1 Deactive the Show soul level mission
            res.WriteFixedString("The Sexy Dwarf", 0x61); // Quest Name
            res.WriteInt32(70); // Quest Level
            res.WriteInt32(80000); // Time limit
            res.WriteFixedString("Marina", 0x61); // The NPC that send you the quest
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteInt32(40); // Reward EXP
            res.WriteInt32(50); // Reward GOLD
            int numEntries4 = 0xA;
            res.WriteInt32(numEntries4);
            for (int i = 0; i < numEntries4; i++)
            {
                res.WriteInt32(0x10); //size of string
                res.WriteFixedString("ItemName", 0x10);
                res.WriteInt16(1); // Numbers of items
                res.WriteInt32(8); // ItemInfo type (Cursed, Blessed, Ect...)
            }
            res.WriteByte(1);
            int numEntries5 = 0xC;
            for (int k = 0; k < numEntries5; k++)
            {
                res.WriteInt32(0x10); //size of string
                res.WriteFixedString("Please Save the dandy sexy Dwarf !", 0x10);
                res.WriteInt16(1); // Selected Prize Numbers of ItemInfo
                res.WriteInt32(8); // Selected Prize Item Type (Cursed, Blessed, Ect...)
            }
            res.WriteByte(1);
            //??res.WriteByte(1);
            res.WriteFixedString("Please Help me, The sexy dwarf dissapear !, I love him plz, search him, and bring it back to me", 0x181); // Story of the quest
            res.WriteFixedString("okay", 0x181); // completion Requirement
            for (int m = 0; m < 0x5; m++)
            {
                res.WriteByte(0); // Type of more completion requirmeent
                res.WriteInt32(4); // Mobs Name, ItemInfo Names, for the more requirement
                res.WriteInt32(50); // Max of mob to kill, or itemc collect, or ect....
                res.WriteInt32(0);
                res.WriteInt32(0);
            }
            res.WriteByte(1); // Add more completion requirement, Collect, Mob kill, ect.., 2 = 2 requirement !! 1 = 1requirement !! 0 = 0requirement.
            //no_JP Router.Send(client.Map, (ushort)AreaPacketId.recv_event_quest_order, res, ServerType.Area);


            SendEventEnd(client);
        }
        private void SendEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);

        }

    }
}
