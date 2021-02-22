using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_raisescale_request_revive_event : ClientHandler
    {
        public send_raisescale_request_revive_event(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_raisescale_request_revive_event;

        public override void Handle(NecClient client, NecPacket packet)
        {

            IBuffer res22 = BufferProvider.Provide();
            res22.WriteInt32(1); // 0 = normal 1 = cinematic
            res22.WriteByte(0);
            Router.Send(client, (ushort)AreaPacketId.recv_event_start, res22, ServerType.Area);
            //if success
            res22 = BufferProvider.Provide();
            res22.WriteCString("scale\revive_success"); // animates your body floating up and standing
            res22.WriteUInt32(client.Character.InstanceId); //ObjectID
            Router.Send(client, (ushort)AreaPacketId.recv_event_script_play, res22, ServerType.Area);
            //if fail
            res22 = BufferProvider.Provide();
            res22.WriteCString("scale\revive_fail"); // animates your body floating up and standing
            res22.WriteUInt32(client.Character.InstanceId); //ObjectID
            //Router.Send(client, (ushort)AreaPacketId.recv_event_script_play, res22, ServerType.Area);
            //if fail again. you're lost
            res22 = BufferProvider.Provide();
            res22.WriteCString("scale\revive_lost"); // animates your body floating up and standing
            res22.WriteUInt32(client.Character.InstanceId); //ObjectID
            //Router.Send(client, (ushort)AreaPacketId.recv_event_script_play, res22, ServerType.Area);
        }
    }
}
