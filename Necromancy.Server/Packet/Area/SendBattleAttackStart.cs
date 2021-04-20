using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendBattleAttackStart : ClientHandler
    {
        public SendBattleAttackStart(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_battle_attack_start;


        public override void Handle(NecClient client, NecPacket packet)
        {
            client.character.battleAnim =
                231; //at the start of every attack, set the battle anim to 231.  231 is the 1st anim for all weapon types
            client.character.battleNext = 0;

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.character.instanceId);
            router.Send(client.map, (ushort)AreaPacketId.recv_battle_attack_start_r, res, ServerType.Area);
        }
    }
}
