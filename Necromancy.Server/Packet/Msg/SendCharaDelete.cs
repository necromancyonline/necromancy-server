using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_delete : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_chara_delete));

        private readonly NecServer _server;

        public send_chara_delete(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort Id => (ushort) MsgPacketId.send_chara_delete;


        public override void Handle(NecClient client, NecPacket packet)
        {
            int characterId = packet.Data.ReadInt32();
            Logger.Debug($"CharacterId [{characterId}] deleted from Soul [{client.Soul.Name}]");
            _server.Database.DeleteCharacter(characterId);
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);


            Router.Send(client, (ushort) MsgPacketId.recv_chara_delete_r, res, ServerType.Msg);
        }
    }
}
