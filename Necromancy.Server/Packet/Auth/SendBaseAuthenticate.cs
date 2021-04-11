using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Packet.Auth
{
    public class send_base_authenticate : ConnectionHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_base_authenticate));

        public send_base_authenticate(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AuthPacketId.send_base_authenticate;

        public override void Handle(NecConnection connection, NecPacket packet)
        {
            string accountName = packet.Data.ReadCString();
            string password = packet.Data.ReadCString();
            string macAddress = packet.Data.ReadCString();
            int unknown = packet.Data.ReadInt16();
            Logger.Info($"Account:{accountName} Password:{password} Unknown:{unknown}");

            Account account = Database.SelectAccountByName(accountName);
            if (account == null)
            {
                if (Settings.RequireRegistration)
                {
                    Logger.Error(connection, $"AccountName: {accountName} doesn't exist");
                    SendResponse(connection, null);
                    connection.Socket.Close();
                    return;
                }

                string bCryptHash = BCrypt.Net.BCrypt.HashPassword(password, NecSetting.BCryptWorkFactor);
                account = Database.CreateAccount(accountName, accountName, bCryptHash);
            }

            if (!BCrypt.Net.BCrypt.Verify(password, account.Hash))
            {
                Logger.Error(connection, $"Invalid password for AccountName: {accountName}");
                SendResponse(connection, null);
                connection.Socket.Close();
                return;
            }

            NecClient client = new NecClient();
            client.Account = account;
            client.AuthConnection = connection;
            connection.Client = client;
            client.UpdateIdentity();
            Server.Clients.Add(client);

            SendResponse(connection, account);
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
                res.WriteInt32(account.Id);
            }

            Router.Send(connection, (ushort) AuthPacketId.recv_base_authenticate_r, res);
        }
    }
}
