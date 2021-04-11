using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendSoulSelectC44F : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendSoulSelectC44F));

        public SendSoulSelectC44F(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) MsgPacketId.send_soul_select_C44F;

        public override void Handle(NecClient client, NecPacket packet)
        {
            string soulName = packet.data.ReadCString();
            List<Soul> souls = database.SelectSoulsByAccountId(client.account.id);
            foreach (Soul soul in souls)
            {
                if (soul.name == soulName)
                {
                    client.soul = soul;
                    break;
                }
            }

            IBuffer res = BufferProvider.Provide();
            if (client.soul == null)
            {
                _Logger.Error(client, $"Soul with name: '{soulName}' not found");
                res.WriteInt32(1); // 0 = OK | 1 = Failed to return to soul selection
                router.Send(client, (ushort) MsgPacketId.recv_soul_select_r, res, ServerType.Msg);
                client.Close();
                return;
            }

            res.WriteInt32(0); // 0 = OK | 1 = Failed to return to soul selection
            if (client.soul.password == null)
            {
                _Logger.Info(client, "Password not set, initiating set password");
                res.WriteByte(0); // bool - 0 = Set New Password | 1 = Enter Password
            }
            else
            {
                res.WriteByte(1); // bool - 0 = Set New Password | 1 = Enter Password
            }


            router.Send(client, (ushort) MsgPacketId.recv_soul_select_r, res, ServerType.Msg);
        }
    }
}
