using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Custom
{
    public class SendDisconnect : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendDisconnect));

        public SendDisconnect(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) CustomPacketId.SendDisconnect;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int data = packet.data.ReadInt32();
            _Logger.Error(client,
                $"{client.soul.name} {client.character.name} has sent a disconnect packet to the server.  Wave GoodBye! ");

            IBuffer buffer = BufferProvider.Provide();
            buffer.WriteInt32(data);

            NecPacket response = new NecPacket(
                (ushort) CustomPacketId.RecvDisconnect,
                buffer,
                packet.serverType,
                PacketType.Disconnect
            );

            router.Send(client, response);
        }
    }
}
