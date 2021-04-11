using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendSoulSetPasswd : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendSoulSetPasswd));

        public SendSoulSetPasswd(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) MsgPacketId.send_soul_set_passwd;

        public override void Handle(NecClient client, NecPacket packet)
        {
            string soulPassword = packet.data.ReadCString();
            Soul soul = client.soul;
            soul.password = soulPassword;
            if (!database.UpdateSoul(soul))
            {
                _Logger.Error(client, $"Failed to save password for SoulId: {soul.id}");
                client.Close();
                return;
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0); // bool in JP client TODO what is it in US???
            res.WriteCString("");
            router.Send(client, (ushort) MsgPacketId.recv_soul_set_passwd_r, res, ServerType.Msg);
        }
    }
}
