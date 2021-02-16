using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportNoactDead : PacketResponse
    {
        private readonly uint _instancedId;
        private readonly int _deadType;
        private readonly int _lossRegion;
        private readonly int _deathType;


        public RecvBattleReportNoactDead(uint instancedId, int deadType)
            : base((ushort) AreaPacketId.recv_battle_report_noact_notify_dead, ServerType.Area)
        {
            _instancedId = instancedId;
            _deadType = deadType;
            _lossRegion = 1;
            _deathType = 1;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instancedId);
            res.WriteInt32(_deadType); //Dead int "Dead_type"
            res.WriteInt32(_lossRegion); //Loss_region
            res.WriteInt32(_deathType); //Death_Type
            return res;
        }
    }
}
