using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_guard_start : ClientHandler
    {
        public send_battle_guard_start(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_battle_guard_start;


        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            //Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_guard_start_self, res, ServerType.Area);

            res.WriteUInt32(1); //If sending a 1, guard fails. Need to come up with logic to make it so people can't block when not using shields.
            Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_guard_start_r, res, ServerType.Area);
            
            client.Character.AddStateBit(CharacterState.BlockPose);
        }
    }
}
