using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Custom
{
    public class SendHeartbeat : ClientHandler
    {
        public SendHeartbeat(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) CustomPacketId.SendHeartbeat;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint time = packet.data.ReadUInt32();

            IBuffer buffer = BufferProvider.Provide();
            buffer.WriteInt32(0);
            buffer.WriteInt32(0);

            NecPacket response = new NecPacket(
                (ushort) CustomPacketId.RecvHeartbeat,
                buffer,
                packet.serverType,
                PacketType.HeartBeat
            );

            router.Send(client, response);
        }
    }
}
