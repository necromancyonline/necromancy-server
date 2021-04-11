using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_friend_reply_to_link2 : ClientHandler
    {
        public send_friend_reply_to_link2(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_friend_reply_to_link2;


        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0); // 0  = msg friend added

            Router.Send(client, (ushort) MsgPacketId.recv_friend_result_reply_link2, res, ServerType.Msg);
            /* Logic for all the possibilities below goes above here.
            FRIEND_REGIST	10	Sent Friend Request to %s
            FRIEND_REGIST	20	Denied the Friend Request
            FRIEND_REGIST	2	Your Friend Request was cancelled
            FRIEND_REGIST	1	Your Friend Request was not approved
            FRIEND_REGIST	0	Added %s on your Friend List
            FRIEND_REGIST	200	%s approved your Friend Request and was added to your Friend List.
            FRIEND_REGIST	201	%s has denied your Friend Request
            FRIEND_REGIST	202	Unable to add to Friend List
            FRIEND_REGIST	-2101	%s is already on your Friend List
            FRIEND_REGIST	-2102	You have already asked someone else to join your Friend List. Try again once that player has replied.
            FRIEND_REGIST	-2103	Friend List full
            FRIEND_REGIST	-2104	The other party's Friend List is full
            FRIEND_REGIST	-2105	The targeted player is processing another Friend Request. Please try again later.
            FRIEND_REGIST	-2106	%s has denied your Friend Request
            FRIEND_REGIST	-2108	You have sent a Friend Request to %s
            FRIEND_REGIST	GENERIC	Unable to add to Friend List
            */
        }
    }
}
