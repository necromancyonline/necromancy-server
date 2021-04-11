using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Custom
{
    public class SendUnknown1 : ClientHandler
    {
        public SendUnknown1(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) CustomPacketId.SendUnknown1;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint a = packet.data.ReadUInt32();
            uint b = packet.data.ReadUInt32();
            // Logger.Info(client, $"Unknown1 A:{a / 1000}");
            // Logger.Info(client, $"Unknown1 B:{b / 1000}");

            IBuffer buffer = BufferProvider.Provide();
            buffer.WriteInt32(0);
            buffer.WriteInt32(0);

            NecPacket response = new NecPacket(
                (ushort) CustomPacketId.RecvUnknown1,
                buffer,
                packet.serverType,
                PacketType.Unknown1
            );

            router.Send(client, response);
        }
    }
}
