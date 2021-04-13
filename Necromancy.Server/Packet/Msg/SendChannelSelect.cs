using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendChannelSelect : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendChannelSelect));

        public SendChannelSelect(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)MsgPacketId.send_channel_select;

        public override void Handle(NecClient client, NecPacket packet)
        {
            if (!server.maps.TryGet(client.character.mapId, out Map map))
            {
                _Logger.Error(client, $"No map found for MapId: {client.character.mapId}");
                client.Close();
                return;
            }

            int channelId = packet.data.ReadInt32();
            client.character.channel = channelId;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //Error Check

            //sub_4E4210_2341  // impacts map spawn ID (old Comment)
            res.WriteInt32(map.id); //MapSerialID
            res.WriteInt32(channelId); //channel??????
            res.WriteFixedString(settings.dataAreaIpAddress, 0x41); //IP?
            res.WriteUInt16(settings.areaPort); //Port

            //sub_484420   //  does not impact map spawn coord (old Comment)
            res.WriteFloat(client.character.x); //X Pos
            res.WriteFloat(client.character.y); //Y Pos
            res.WriteFloat(client.character.z); //Z Pos
            res.WriteByte(client.character.heading); //View offset
            //

            router.Send(client, (ushort)MsgPacketId.recv_channel_select_r, res, ServerType.Msg);

            map.EnterForce(client);
            SendEventEnd(client);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteUInt32(client.character.instanceId);
            res2.WriteCString("IsThisMyChannel?????"); //Length to be Found
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
