using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_entry : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_map_entry));

        public send_map_entry(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_map_entry;

        public override void Handle(NecClient client, NecPacket packet)
        {
            client.Character.mapChange = false;
            int mapId = packet.Data.ReadInt32();
            Map map = Server.Maps.Get(mapId);
            if (map == null)
            {
                Logger.Error(client, $"MapId: {mapId} not found in map lookup");
                client.Close();
                return;
            }
            map.Enter(client);
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_map_entry_r, res, ServerType.Area);
        }
    }
}
