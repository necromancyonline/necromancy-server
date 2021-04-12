using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Auth
{
    public class SendBaseSelectWorld : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendBaseSelectWorld));

        public SendBaseSelectWorld(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AuthPacketId.send_base_select_world;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint selectedWorld = packet.data.ReadUInt32();

            _Logger.Info($"Selected World: {selectedWorld}");


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString(settings.dataMsgIpAddress);
            res.WriteUInt16(settings.msgPort);
            res.WriteFixedString("", 0x14);
            router.Send(client, (ushort)AuthPacketId.recv_base_select_world_r, res, ServerType.Auth);
        }
    }
}
