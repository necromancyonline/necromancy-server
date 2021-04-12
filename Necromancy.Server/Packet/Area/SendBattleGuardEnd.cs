using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendBattleGuardEnd : ClientHandler
    {
        public SendBattleGuardEnd(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_battle_guard_end;


        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            router.Send(client, (ushort)AreaPacketId.recv_battle_guard_end_self, res, ServerType.Area);

            res.WriteInt32(0);
            router.Send(client, (ushort)AreaPacketId.recv_battle_guard_end_r, res, ServerType.Area);

            client.character.ClearStateBit(CharacterState.BlockPose);


            res = BufferProvider.Provide();
            res.WriteUInt32(client.character.instanceId);
            router.Send(client.map, (ushort)AreaPacketId.recv_dbg_battle_guard_end_notify, res, ServerType.Area);
        }
    }
}
