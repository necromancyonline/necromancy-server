using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_refusallist_remove_user : ClientHandler
    {
        public send_refusallist_remove_user(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_refusallist_remove_user;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            int targetSoulId = packet.Data.ReadInt32();

            /*
            REFUSAL_LIST	0	%s is added to your Block List
            REFUSAL_LIST	1	%s is removed from your Block List
            REFUSAL_LIST	2	%s has been deleted from your Block List as its Soul has been deleted
            REFUSAL_LIST	-1	Invalid action
            REFUSAL_LIST	-20	The Soul Name does not exist
            REFUSAL_LIST	-2201	The target is on your Block List
            REFUSAL_LIST	-2202	You have been added to the target's Block List
            REFUSAL_LIST	-2203	The Soul Name does not exist
            REFUSAL_LIST	-2204	Block List is full
            REFUSAL_LIST	-2205	Soul Name has already been added
            REFUSAL_LIST	-2206	You may not add yourself to the Block List
            REFUSAL_LIST	-2207	The Soul Name does not exist
            REFUSAL_LIST	-2208	You may not add party members to the Block List
            REFUSAL_LIST	GENERIC	Unable to add to Block List
            */

            res.WriteInt32(1); //Result 
            res.WriteInt32(targetSoulId); //ref)soulId

            Router.Send(client, (ushort) AreaPacketId.recv_refusallist_remove_user_r, res, ServerType.Area);
        }
    }
}
