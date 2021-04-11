using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendBattleAttackPose : ClientHandler
    {
        public SendBattleAttackPose(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_battle_attack_pose;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            router.Send(client, (ushort) AreaPacketId.recv_battle_attack_pose_self, res, ServerType.Area);

            res.WriteUInt32(client.character.instanceId);

            router.Send(client.map, (ushort) AreaPacketId.recv_battle_attack_pose_r, res, ServerType.Area);
            router.Send(client.map, (ushort) AreaPacketId.recv_battle_attack_pose_start_notify, res, ServerType.Area, client);
        }

    }
}
