using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventChallengeScriptPlay : PacketResponse
    {
        public RecvEventChallengeScriptPlay()
            : base((ushort)AreaPacketId.recv_event_challenge_script_play, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("What");

            return res;
        }
    }
}
