using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendStallOpen : ClientHandler
    {
        public SendStallOpen(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_stall_open;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            router.Send(client, (ushort)AreaPacketId.recv_stall_open_r, res, ServerType.Area);

            SendStallNotifyOpend(client);
        }

        private void SendStallNotifyOpend(NecClient client)
        {
            //recv_stall_notify_opend = 0x7FC5,
            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(client.character.instanceId);
            res.WriteCString("Unky's Shop"); // find max size, shop name
            int numEntries = 5;
            res.WriteInt32(numEntries); //less than or equal to 5

            //for (int i = 0; i < numEntries; i++)
            //1
            res.WriteInt32(10200101); //Icon/Wep ID i think
            res.WriteByte(2);
            res.WriteByte(2);
            res.WriteByte(2);

            res.WriteInt32(0); //Broken Icon when -1,
            res.WriteInt16(0); //Item count I think
            res.WriteInt32(9);

            res.WriteByte(0); //Bool //Changed Icon background and made it brownish?

            //2
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt32(0);

            res.WriteByte(0); //Bool

            //3
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt32(0);

            res.WriteByte(0); //Bool

            //4
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt32(0);

            res.WriteByte(0); //Bool

            //5
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt32(0);

            res.WriteByte(0); //Bool

            router.Send(client.map, (ushort)AreaPacketId.recv_stall_notify_opend, res, ServerType.Area, client);
        }
    }
}
