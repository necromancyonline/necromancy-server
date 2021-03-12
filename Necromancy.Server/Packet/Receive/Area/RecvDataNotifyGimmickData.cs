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
            resI.WriteUInt32(_gimmickSpawn.InstanceId);
            resI.WriteFloat(_gimmickSpawn.X);
            resI.WriteFloat(_gimmickSpawn.Y);
            resI.WriteFloat(_gimmickSpawn.Z);
            resI.WriteByte(_gimmickSpawn.Heading);
            resI.WriteInt32(_gimmickSpawn.ModelId); //Gimmick number (from gimmick.csv)
            resI.WriteInt32(_gimmickSpawn.State); //Gimmick State
            resI.WriteInt32(0); //new
            return resI;
        }
    }
}
