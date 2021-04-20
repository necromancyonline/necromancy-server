using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendEventSelectChannelR : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendEventSelectChannelR));

        public SendEventSelectChannelR(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_event_select_channel_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int channelId = packet.data.ReadInt32();

            if (channelId == -1)
            {
                _Logger.Debug(
                    "Escape button was selected to close channel select. channelId code  == -0xFFFF => SendEventEnd");
                SendEventEnd(client);
                return;
            }

            if (!server.maps.TryGet(client.character.mapId, out Map map))
            {
                _Logger.Error($"MapId: {client.character.mapId} does not exist");
                return;
            }

            client.character.channel = channelId;
            map.EnterForce(client);
            SendEventEnd(client);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(channelId); //Channel ID
            res2.WriteCString($"Channel {channelId}"); //Length to be Found
            router.Send(server.clients.GetAll(), (ushort)AreaPacketId.recv_channel_notify, res2, ServerType.Area);
        }

        private void SendEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);
        }
    }
}
