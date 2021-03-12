using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_select_channel_r : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_event_select_channel_r));

        public send_event_select_channel_r(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_event_select_channel_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int channelId = packet.Data.ReadInt32();

            if (channelId == -1)
            {
                Logger.Debug(
                    "Escape button was selected to close channel select. channelId code  == -0xFFFF => SendEventEnd");
                SendEventEnd(client);
                return;
            }

            if (!Server.Maps.TryGet(client.Character.MapId, out Map map))
            {
                Logger.Error($"MapId: {client.Character.MapId} does not exist");
                return;
            }

            client.Character.Channel = channelId;
            map.EnterForce(client);
            SendEventEnd(client);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(channelId); //Channel ID
            res2.WriteCString($"Channel {channelId}"); //Length to be Found
            Router.Send(Server.Clients.GetAll(), (ushort) AreaPacketId.recv_channel_notify, res2, ServerType.Area);
        }

        private void SendEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            Router.Send(client, (ushort) AreaPacketId.recv_event_end, res, ServerType.Area);
        }
    }
}
