using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_attack_pose : ClientHandler
    {
        public send_battle_attack_pose(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_battle_attack_pose;

        public override void Handle(NecClient client, NecPacket packet)
        {
            SendBattleAttackPoseStartNotify(client);

            IBuffer res2 = BufferProvider.Provide();
            Router.Send(client, (ushort) AreaPacketId.recv_battle_attack_pose_self, res2, ServerType.Area);


            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.Character.InstanceId);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_attack_pose_r, res, ServerType.Area, client);
        }

        private void SendBattleAttackPoseStartNotify(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(client.Character.InstanceId);

            Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_attack_pose_start_notify, res, ServerType.Area,
                client);

            client.Character.weaponEquipped = true;
            //client.Character.AddStateBit(CharacterState.BattlePose);
        }
    }
}
