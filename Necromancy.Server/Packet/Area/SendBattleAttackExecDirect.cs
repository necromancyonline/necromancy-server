using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendBattleAttackExecDirect : ClientHandler
    {
        public SendBattleAttackExecDirect(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_battle_attack_exec_direct;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int unknown1 = packet.data.ReadInt32();
            int targetId = packet.data.ReadInt32();
            int unknown2 = packet.data.ReadInt32();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(targetId);

            router.Send(client.map, (ushort) AreaPacketId.recv_battle_attack_exec_direct_r, res, ServerType.Area,
                client);
        }
    }
}
