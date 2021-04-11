using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendGimmickAccessObject : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendGimmickAccessObject));

        public SendGimmickAccessObject(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_gimmick_access_object;


        public override void Handle(NecClient client, NecPacket packet)
        {
            int targetInstanceId = packet.data.ReadInt32();
            int unknown = packet.data.ReadInt32();

            _Logger.Debug($"accessing gimick with instance ID {targetInstanceId}");

            IBuffer res = BufferProvider.Provide(); // this is the buffer we create
            res.WriteInt32(0); //Error Check?
            router.Send(client, (ushort) AreaPacketId.recv_gimmick_access_object_r, res,
                ServerType.Area); //this sends out our packet to the first operand

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteUInt32(client.character
                .instanceId); //this is probably for letting others know who accessed it (Instance Id)
            res2.WriteInt32(targetInstanceId);
            res2.WriteInt32(unknown);
            router.Send(client.map, (ushort) AreaPacketId.recv_gimmick_access_object_notify, res2, ServerType.Area);

            IBuffer res3 = BufferProvider.Provide();
            res3.WriteInt32(targetInstanceId); //Gimmick Object ID.
            res3.WriteInt32(unknown); //Gimmick State
            router.Send(client.map, (ushort) AreaPacketId.recv_gimmick_state_update, res3, ServerType.Area);
        }
    }
}
