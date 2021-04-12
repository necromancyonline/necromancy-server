using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendQuestAbort : ClientHandler
    {
        public SendQuestAbort(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_quest_abort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int abortQuestNumber = packet.data.ReadInt32();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(abortQuestNumber);
            router.Send(client, (ushort)AreaPacketId.recv_quest_abort_r, res, ServerType.Area);
        }
    }
}
