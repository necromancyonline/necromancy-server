using System;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Packet.Auth
{
    public class SendBaseAuthenticate : ConnectionHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendBaseAuthenticate));

        public SendBaseAuthenticate(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AuthPacketId.send_base_authenticate;

        public override void Handle(NecConnection connection, NecPacket packet)
        {
            string accountName = packet.data.ReadCString();
            string password = packet.data.ReadCString();
            string macAddress = packet.data.ReadCString();
            int unknown = packet.data.ReadInt16();
            _Logger.Info($"Account:{accountName} Password:{password} Unknown:{unknown}");

            Account account = database.SelectAccountByName(accountName);
            if (account == null)
            {
                if (settings.requireRegistration)
                {
                    _Logger.Error(connection, $"AccountName: {accountName} doesn't exist");
                    SendResponse(connection, null);
                    connection.socket.Close();
                    return;
                }

                string bCryptHash = BCrypt.Net.BCrypt.HashPassword(password, NecSetting.B_CRYPT_WORK_FACTOR);
                account = database.CreateAccount(accountName, accountName, bCryptHash);
            }

            if (!BCrypt.Net.BCrypt.Verify(password, account.hash))
            {
                _Logger.Error(connection, $"Invalid password for AccountName: {accountName}");
                SendResponse(connection, null);
                connection.socket.Close();
                return;
            }

            NecClient client = new NecClient();
            client.account = account;
            client.authConnection = connection;
            connection.client = client;
            client.UpdateIdentity();
            server.clients.Add(client);

            SendResponse(connection, account);

            //if client did not send a hardbeat within 75 seconds, something went wrong. remove the client. Thats enough time for 4 heartbeats.
            Task.Delay(TimeSpan.FromSeconds(75)).ContinueWith
            (t1 =>
                {
                    if (client != null)
                        if (client.heartBeat == 0)
                        {
                            server.clients.Remove(client);
                            _Logger.Error($"Initial heartbeat missed. disconnecting client. Server.clientCount is now {server.clients.GetCount()}");
                        }
                }
            );
        }

        private void SendResponse(NecConnection connection, Account account)
        {
            IBuffer res = BufferProvider.Provide();
            if (account == null)
            {
                res.WriteInt32(1); // Error (0 = OK,  1 = ID or Pw to long)
                res.WriteInt32(0); // Account Id
            }
            else
            {
                res.WriteInt32(0); // Error (0 = OK,  1 = ID or Pw to long)
                res.WriteInt32(account.id);
            }

            router.Send(connection, (ushort)AuthPacketId.recv_base_authenticate_r, res);
        }
    }
}
