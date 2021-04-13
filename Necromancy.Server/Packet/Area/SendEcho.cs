using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendEcho : ClientHandler
    {
        public SendEcho(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_echo;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int unknown = packet.data.ReadInt32(); //Chat type?
            int unknown2 = packet.data.ReadInt32(); //Chat type also maybe?
            int size = packet.data.ReadInt32();
            byte[] message = packet.data.ReadBytes(size);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(unknown);
            res2.WriteInt32(unknown2);
            int numEntries = size;
            res2.WriteInt32(numEntries); //Less than or equal to 0x4E20
            for (int i = 0; i < numEntries; i++) res2.WriteByte(message[i]);
            router.Send(client.map, (ushort)AreaPacketId.recv_echo_notify, res2, ServerType.Area);

            IBuffer res = BufferProvider.Provide();
            router.Send(client, (ushort)AreaPacketId.recv_echo_r, res, ServerType.Area);
        }
    }
}
