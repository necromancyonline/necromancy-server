using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_gimmick_access_object : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_gimmick_access_object));

        public send_gimmick_access_object(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_gimmick_access_object;


        public override void Handle(NecClient client, NecPacket packet)
        {
            int targetInstanceId = packet.Data.ReadInt32();
            int unknown = packet.Data.ReadInt32();

            Logger.Debug($"accessing gimick with instance ID {targetInstanceId}");

            IBuffer res = BufferProvider.Provide(); // this is the buffer we create 
            res.WriteInt32(0); //Error Check?
            Router.Send(client, (ushort) AreaPacketId.recv_gimmick_access_object_r, res,
                ServerType.Area); //this sends out our packet to the first operand

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteUInt32(client.Character
                .InstanceId); //this is probably for letting others know who accessed it (Instance Id)
            res2.WriteInt32(targetInstanceId);
            res2.WriteInt32(unknown);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_gimmick_access_object_notify, res2, ServerType.Area);

            IBuffer res3 = BufferProvider.Provide();
            res3.WriteInt32(targetInstanceId); //Gimmick Object ID.
            res3.WriteInt32(unknown); //Gimmick State
            Router.Send(client.Map, (ushort) AreaPacketId.recv_gimmick_state_update, res3, ServerType.Area);
        }
    }
}
