using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_base_check_version_msg : ConnectionHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_base_check_version_msg));

        public send_base_check_version_msg(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_base_check_version;

        public override void Handle(NecConnection connection, NecPacket packet)
        {
            uint unknown = packet.Data.ReadUInt32();
            uint major = packet.Data.ReadUInt32();
            uint minor = packet.Data.ReadUInt32();
            Logger.Info($"{major} - {minor}");

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteUInt32(unknown);
            res.WriteUInt32(major);
            res.WriteUInt32(minor);

            Router.Send(connection, (ushort) MsgPacketId.recv_base_check_version_r, res);
        }
    }
}
