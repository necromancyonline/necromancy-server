using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendEventSelectMapAndChannelR : ClientHandler
    {
        private static readonly NecLogger _Logger =
            LogProvider.Logger<NecLogger>(typeof(SendEventSelectMapAndChannelR));

        public SendEventSelectMapAndChannelR(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort) AreaPacketId.send_event_select_map_and_channel_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int mapId = packet.data.ReadInt32();
            int channelId = packet.data.ReadInt32();

            if (mapId == -2147483648)
            {
                _Logger.Debug(
                    "Escape button was selected to close dungeun select. MapID code  == -2147483648 => SendEventEnd");
                SendEventEnd(client);
                return;
            }

            if (!server.maps.TryGet(mapId, out Map map))
            {
                _Logger.Error($"MapId: {mapId} does not exist");
                return;
            }

            client.character.channel = channelId;
            map.EnterForce(client);
            SendEventEnd(client);
        }

        private void SendEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            router.Send(client, (ushort) AreaPacketId.recv_event_end, res, ServerType.Area);
        }
    }
}
