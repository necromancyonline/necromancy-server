using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendCharaDelete : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendCharaDelete));

        private readonly NecServer _server;

        public SendCharaDelete(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort id => (ushort) MsgPacketId.send_chara_delete;


        public override void Handle(NecClient client, NecPacket packet)
        {
            int characterId = packet.data.ReadInt32();
            _Logger.Debug($"CharacterId [{characterId}] deleted from Soul [{client.soul.name}]");
            _server.database.DeleteCharacter(characterId);
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);


            router.Send(client, (ushort) MsgPacketId.recv_chara_delete_r, res, ServerType.Msg);
        }
    }
}
