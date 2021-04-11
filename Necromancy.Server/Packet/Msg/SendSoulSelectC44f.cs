using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_soul_select_C44F : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_soul_select_C44F));

        public send_soul_select_C44F(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_soul_select_C44F;

        public override void Handle(NecClient client, NecPacket packet)
        {
            string soulName = packet.Data.ReadCString();
            List<Soul> souls = Database.SelectSoulsByAccountId(client.Account.Id);
            foreach (Soul soul in souls)
            {
                if (soul.Name == soulName)
                {
                    client.Soul = soul;
                    break;
                }
            }

            IBuffer res = BufferProvider.Provide();
            if (client.Soul == null)
            {
                Logger.Error(client, $"Soul with name: '{soulName}' not found");
                res.WriteInt32(1); // 0 = OK | 1 = Failed to return to soul selection
                Router.Send(client, (ushort) MsgPacketId.recv_soul_select_r, res, ServerType.Msg);
                client.Close();
                return;
            }

            res.WriteInt32(0); // 0 = OK | 1 = Failed to return to soul selection
            if (client.Soul.Password == null)
            {
                Logger.Info(client, "Password not set, initiating set password");
                res.WriteByte(0); // bool - 0 = Set New Password | 1 = Enter Password
            }
            else
            {
                res.WriteByte(1); // bool - 0 = Set New Password | 1 = Enter Password
            }


            Router.Send(client, (ushort) MsgPacketId.recv_soul_select_r, res, ServerType.Msg);
        }
    }
}
