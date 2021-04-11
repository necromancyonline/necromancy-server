using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_attack_next : ClientHandler
    {
        public send_battle_attack_next(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_battle_attack_next;


        public override void Handle(NecClient client, NecPacket packet)
        {
            if (client.Character.battleNext == 0)
            {
                client.Character.battleAnim = 232; // 232 is the '2nd' attack animation for all weapons.  
                client.Character.battleNext = 1;
            }
            else
            {
                client.Character.battleAnim = (byte) (232 + client.Character.battleNext); // 233,234,235,236...
                client.Character.battleNext += 1;
            }


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //0 means success
            Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_attack_next_r, res, ServerType.Area, client);
        }
    }
}
