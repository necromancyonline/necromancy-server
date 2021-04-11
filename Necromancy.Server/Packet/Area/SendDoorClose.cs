using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_door_close : ClientHandler
    {
        public send_door_close(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_door_close;

        

        public override void Handle(NecClient client, NecPacket packet)
        {
            int doorInstanceId = packet.Data.ReadInt32();
            int doorState = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //Door instance id?
            Router.Send(client.Map, (ushort)AreaPacketId.recv_door_close_r, res, ServerType.Area);
            SendDoorUpdateNotify(client, doorInstanceId, doorState);
        }

        private void SendDoorUpdateNotify(NecClient client, int doorInstanceId, int doorState)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(doorInstanceId); //Door instance id?
            res.WriteInt32(doorState); //Door state
            Router.Send(client.Map, (ushort)AreaPacketId.recv_door_update_notify, res, ServerType.Area);
        }
    }
}
