using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_raisescale_request_revive : ClientHandler
    {
        public send_raisescale_request_revive(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_raisescale_request_revive;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res1 = BufferProvider.Provide();
            res1.WriteInt32(0); //Has to be 0 or else you DC
            res1.WriteUInt32(client.Character.DeadBodyInstanceId);
            res1.WriteUInt32(client.Character.InstanceId);
            Router.Send(client, (ushort)AreaPacketId.recv_revive_init_r, res1, ServerType.Area);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_raisescale_request_revive_r, res, ServerType.Area);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(0); // Error code, 0 = success
            Router.Send(client, (ushort)AreaPacketId.recv_revive_execute_r, res2, ServerType.Area);

            client.Character.soulFormState -= 1;
            client.Character.Hp.toMax();
            client.Character.movementId = client.Character.InstanceId;
            client.Character.State = CharacterState.NormalForm;

            RecvCharaUpdateHp cHpUpdate = new RecvCharaUpdateHp(client.Character.Hp.current);
            Router.Send(client, cHpUpdate.ToPacket());

            IBuffer res4 = BufferProvider.Provide();
            res4.WriteUInt32(client.Character.InstanceId);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_start_notify, res4, ServerType.Area);

            IBuffer res5 = BufferProvider.Provide();
            res5.WriteUInt32(client.Character.InstanceId);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_notify_raise, res5, ServerType.Area);

            IBuffer res6 = BufferProvider.Provide();
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_end_notify, res6, ServerType.Area);

            IBuffer res3 = BufferProvider.Provide();
            res3.WriteUInt32(client.Character.DeadBodyInstanceId);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_object_disappear_notify, res3, ServerType.Area);

            //client.Character.hadDied = false;
            client.Character.Hp.depleted = false;
            RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(client.Character, client.Soul.Name);
            Router.Send(client, cData.ToPacket());

            IBuffer res7 = BufferProvider.Provide();
            res7.WriteByte(0);
            Router.Send(client, (ushort)AreaPacketId.recv_event_end, res7, ServerType.Area);
        }
    }
}
