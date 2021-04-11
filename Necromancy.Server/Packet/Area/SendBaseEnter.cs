using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_base_enter : ConnectionHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(ConnectionHandler));

        public send_base_enter(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_base_enter;

        public override void Handle(NecConnection connection, NecPacket packet)
        {
            int accountId = packet.Data.ReadInt32();
            int unknown = packet.Data.ReadInt32();
            byte[] unknown1 = packet.Data.ReadBytes(20); // Suspect SessionId

            // TODO replace with sessionId
            NecClient client = Server.Clients.GetByAccountId(accountId);
            if (client == null)
            {
                Logger.Error(connection, $"AccountId: {accountId} has no active session");
                connection.Socket.Close();
                return;
            }

            client.AreaConnection = connection;
            connection.Client = client;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //  Error
            Router.Send(connection, (ushort) AreaPacketId.recv_base_enter_r, res);
        }
    }
}
