using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendBaseEnter : ConnectionHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(ConnectionHandler));

        public SendBaseEnter(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_base_enter;

        public override void Handle(NecConnection connection, NecPacket packet)
        {
            int accountId = packet.data.ReadInt32();
            int unknown = packet.data.ReadInt32();
            byte[] unknown1 = packet.data.ReadBytes(20); // Suspect SessionId

            // TODO replace with sessionId
            NecClient client = server.clients.GetByAccountId(accountId);
            if (client == null)
            {
                _Logger.Error(connection, $"AccountId: {accountId} has no active session");
                connection.socket.Close();
                return;
            }

            client.areaConnection = connection;
            connection.client = client;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //  Error
            router.Send(connection, (ushort) AreaPacketId.recv_base_enter_r, res);
        }
    }
}
