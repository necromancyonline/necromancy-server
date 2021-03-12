using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Auth
{
    public class send_base_select_world : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_base_select_world));

        public send_base_select_world(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AuthPacketId.send_base_select_world;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint selectedWorld = packet.Data.ReadUInt32();

            Logger.Info($"Selected World: {selectedWorld}");


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString(Settings.DataMsgIpAddress);
            res.WriteUInt16(Settings.MsgPort);
            res.WriteFixedString("", 0x14);
            Router.Send(client, (ushort) AuthPacketId.recv_base_select_world_r, res, ServerType.Auth);
        }
    }
}
