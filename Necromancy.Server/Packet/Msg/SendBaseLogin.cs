using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendBaseLogin : ConnectionHandler
    {
        public const int SoulCount = 8;
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendBaseLogin));

        public SendBaseLogin(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)MsgPacketId.send_base_login;

        public override void Handle(NecConnection connection, NecPacket packet)
        {
            int accountId = packet.data.ReadInt32();
            byte[] unknown = packet.data.ReadBytes(20); // Suspect SessionId
            // TODO replace with sessionId
            NecClient client = server.clients.GetByAccountId(accountId);
            if (client == null)
            {
                _Logger.Error(connection, $"AccountId: {accountId} has no active session");
                // TODO refactor null check
                SendResponse(connection, client);
                connection.socket.Close();
                return;
            }

            client.msgConnection = connection;
            connection.client = client;
            SendResponse(connection, client);
        }

        private void SendResponse(NecConnection connection, NecClient client)
        {
            List<Soul> souls = database.SelectSoulsByAccountId(client.account.id);
            if (souls.Count <= 0)
            {
                IBuffer resq = BufferProvider.Provide();
                resq.WriteInt32(0); //  Error
                for (int i = 0; i < 8; i++)
                {
                    resq.WriteByte(1);
                    resq.WriteFixedString(string.Empty, 49); // Soul Name
                    resq.WriteByte(client.soul.level); // Soul Level
                    resq.WriteByte(0); // bool - if use value 1, can't join in msg server character list
                }

                resq.WriteInt32(0);
                resq.WriteByte(0); //bool
                resq.WriteByte(0);
                router.Send(client, (ushort)MsgPacketId.recv_base_login_r, resq, ServerType.Msg);
                return;
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //  Error
            for (int i = 0; i < SoulCount; i++)
                if (souls.Count > i)
                {
                    Soul soul = souls[0];
                    res.WriteByte(1);
                    res.WriteFixedString(soul.name, 49);
                    res.WriteByte(soul.level);
                    res.WriteByte(1); // bool - if use value 1, can't join in msg server character list
                }
                else
                {
                    res.WriteByte(0);
                    res.WriteFixedString(string.Empty, 49); // Soul Name
                    res.WriteByte(0); // Soul Level
                    res.WriteByte(0); // bool - if use value 1, can't join in msg server character list
                }

            res.WriteInt32(1);
            res.WriteByte(1); // bool
            res.WriteByte(1);

            router.Send(client, (ushort)MsgPacketId.recv_base_login_r, res, ServerType.Msg);
        }
    }
}
