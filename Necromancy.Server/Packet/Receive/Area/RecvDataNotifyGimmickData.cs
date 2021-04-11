using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyGimmickData : PacketResponse
    {
        private Gimmick _gimmickSpawn;

        public RecvDataNotifyGimmickData(Gimmick gimmickSpawn)
            : base((ushort) AreaPacketId.recv_data_notify_gimmick_data, ServerType.Area)
        {
            _gimmickSpawn = gimmickSpawn;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer resI = BufferProvider.Provide();
            resI.WriteUInt32(_gimmickSpawn.instanceId);
            resI.WriteFloat(_gimmickSpawn.x);
            resI.WriteFloat(_gimmickSpawn.y);
            resI.WriteFloat(_gimmickSpawn.z);
            resI.WriteByte(_gimmickSpawn.heading);
            resI.WriteInt32(_gimmickSpawn.modelId); //Gimmick number (from gimmick.csv)
            resI.WriteInt32(_gimmickSpawn.state); //Gimmick State
            resI.WriteInt32(0); //new
            return resI;
        }
    }
}
