using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_channel_select : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_channel_select));

        public send_channel_select(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_channel_select;

        public override void Handle(NecClient client, NecPacket packet)
        {
            if (!Server.Maps.TryGet(client.Character.MapId, out Map map))
            {
                Logger.Error(client, $"No map found for MapId: {client.Character.MapId}");
                client.Close();
                return;
            }

            int channelId = packet.Data.ReadInt32();
            client.Character.Channel = channelId;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //Error Check

            //sub_4E4210_2341  // impacts map spawn ID (old Comment)
            res.WriteInt32(map.Id); //MapSerialID
            res.WriteInt32(channelId); //channel??????
            res.WriteFixedString(Settings.DataAreaIpAddress, 0x41); //IP?
            res.WriteUInt16(Settings.AreaPort); //Port

            //sub_484420   //  does not impact map spawn coord (old Comment)
            res.WriteFloat(client.Character.X); //X Pos
            res.WriteFloat(client.Character.Y); //Y Pos
            res.WriteFloat(client.Character.Z); //Z Pos
            res.WriteByte(client.Character.Heading); //View offset
            //

            Router.Send(client, (ushort) MsgPacketId.recv_channel_select_r, res, ServerType.Msg);

            map.EnterForce(client);
            SendEventEnd(client);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteUInt32(client.Character.InstanceId);
            res2.WriteCString("IsThisMyChannel?????"); //Length to be Found
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
