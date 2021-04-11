using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_soul_create : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_soul_create));

        public send_soul_create(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_soul_create;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte unknown = packet.Data.ReadByte();
            string soulName = packet.Data.ReadCString();

            Soul soul = new Soul();
            soul.Name = soulName;
            soul.AccountId = client.Account.Id;
            if (!Database.InsertSoul(soul))
            {
                Logger.Error(client, $"Failed to create SoulName: {soulName}");
                client.Close();
                return;
            }

            client.Soul = soul;
            Logger.Info($"Created SoulName: {soulName}");

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) MsgPacketId.recv_soul_create_r, res, ServerType.Msg);
        }
    }
}
