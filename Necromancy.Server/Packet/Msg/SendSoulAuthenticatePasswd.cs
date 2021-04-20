using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendSoulAuthenticatePasswd : ClientHandler
    {
        public SendSoulAuthenticatePasswd(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)MsgPacketId.send_soul_authenticate_passwd;

        public override void Handle(NecClient client, NecPacket packet)
        {
            string soulPassword = packet.data.ReadCString();

            IBuffer res = BufferProvider.Provide();
            if (settings.requirePin && client.soul.password != soulPassword)
            {
                res.WriteInt32(1); //  Error: 0 - Success, other vales (maybe) error code
                res.WriteByte(0); // 0 = OK | 1 = need to change soul name (bool type) true = other values, false - 0
                res.WriteCString("");
                router.Send(client, (ushort)MsgPacketId.recv_soul_authenticate_passwd_r, res, ServerType.Msg);
                client.Close();
                return;
            }

            res.WriteInt32(0); //  Error: 0 - Success, other vales (maybe) error code
            res.WriteByte(0); // 0 = OK | 1 = need to change soul name (bool type) true = other values, false - 0
            res.WriteCString("");
            router.Send(client, (ushort)MsgPacketId.recv_soul_authenticate_passwd_r, res, ServerType.Msg);
        }
    }
}
