using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendDoorClose : ClientHandler
    {
        public SendDoorClose(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_door_close;


        public override void Handle(NecClient client, NecPacket packet)
        {
            int doorInstanceId = packet.data.ReadInt32();
            int doorState = packet.data.ReadInt32();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //Door instance id?
            router.Send(client.map, (ushort)AreaPacketId.recv_door_close_r, res, ServerType.Area);
            SendDoorUpdateNotify(client, doorInstanceId, doorState);
        }

        private void SendDoorUpdateNotify(NecClient client, int doorInstanceId, int doorState)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(doorInstanceId); //Door instance id?
            res.WriteInt32(doorState); //Door state
            router.Send(client.map, (ushort)AreaPacketId.recv_door_update_notify, res, ServerType.Area);
        }
    }
}
