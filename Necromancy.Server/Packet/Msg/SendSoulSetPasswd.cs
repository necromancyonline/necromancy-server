using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_soul_set_passwd : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_soul_set_passwd));

        public send_soul_set_passwd(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_soul_set_passwd;

        public override void Handle(NecClient client, NecPacket packet)
        {
            string soulPassword = packet.Data.ReadCString();
            Soul soul = client.Soul;
            soul.Password = soulPassword;
            if (!Database.UpdateSoul(soul))
            {
                Logger.Error(client, $"Failed to save password for SoulId: {soul.Id}");
                client.Close();
                return;
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0); // bool in JP client TODO what is it in US???
            res.WriteCString("");
            Router.Send(client, (ushort) MsgPacketId.recv_soul_set_passwd_r, res, ServerType.Msg);
        }
    }
}
