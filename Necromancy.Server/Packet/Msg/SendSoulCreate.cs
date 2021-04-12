using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendSoulCreate : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendSoulCreate));

        public SendSoulCreate(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)MsgPacketId.send_soul_create;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte unknown = packet.data.ReadByte();
            string soulName = packet.data.ReadCString();

            Soul soul = new Soul();
            soul.name = soulName;
            soul.accountId = client.account.id;
            if (!database.InsertSoul(soul))
            {
                _Logger.Error(client, $"Failed to create SoulName: {soulName}");
                client.Close();
                return;
            }

            client.soul = soul;
            _Logger.Info($"Created SoulName: {soulName}");

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort)MsgPacketId.recv_soul_create_r, res, ServerType.Msg);
        }
    }
}
