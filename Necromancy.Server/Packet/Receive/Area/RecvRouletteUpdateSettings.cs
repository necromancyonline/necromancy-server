using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvRouletteUpdateSettings : PacketResponse
    {
        public RecvRouletteUpdateSettings()
            : base((ushort)AreaPacketId.recv_roulette_update_settings, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(0);
            res.WriteInt16(0);
            return res;
        }
    }
}
