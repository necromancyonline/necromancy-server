using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class SendDoorOpen : ClientHandler
    {
        public SendDoorOpen(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_door_open;



        public override void Handle(NecClient client, NecPacket packet)
        {
            int doorInstanceId = packet.data.ReadInt32();
            int doorState = packet.data.ReadInt32();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //Door instance Id?
            router.Send(client.map, (ushort)AreaPacketId.recv_door_open_r, res, ServerType.Area);
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
