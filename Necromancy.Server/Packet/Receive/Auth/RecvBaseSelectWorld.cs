using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Packet.Receive.Auth
{
    public class RecvBaseSelectWorld : PacketResponse
    {
        private readonly NecSetting _necSetting;

        public RecvBaseSelectWorld(NecSetting necSetting)
            : base((ushort)AuthPacketId.recv_base_select_world_r, ServerType.Auth)
        {
            _necSetting = necSetting;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString(_necSetting.dataMsgIpAddress);
            res.WriteInt32(_necSetting.msgPort);
            return res;
        }
    }
}
