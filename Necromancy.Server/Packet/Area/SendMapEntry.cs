using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendMapEntry : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendMapEntry));

        public SendMapEntry(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_map_entry;

        public override void Handle(NecClient client, NecPacket packet)
        {
            client.character.mapChange = false;
            int mapId = packet.data.ReadInt32();
            Map map = server.maps.Get(mapId);
            if (map == null)
            {
                _Logger.Error(client, $"MapId: {mapId} not found in map lookup");
                client.Close();
                return;
            }

            map.Enter(client);
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort)AreaPacketId.recv_map_entry_r, res, ServerType.Area);
        }
    }
}
