using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendBattleAttackNext : ClientHandler
    {
        public SendBattleAttackNext(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_battle_attack_next;


        public override void Handle(NecClient client, NecPacket packet)
        {
            if (client.character.battleNext == 0)
            {
                client.character.battleAnim = 232; // 232 is the '2nd' attack animation for all weapons.
                client.character.battleNext = 1;
            }
            else
            {
                client.character.battleAnim = (byte) (232 + client.character.battleNext); // 233,234,235,236...
                client.character.battleNext += 1;
            }


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //0 means success
            router.Send(client.map, (ushort) AreaPacketId.recv_battle_attack_next_r, res, ServerType.Area, client);
        }
    }
}
