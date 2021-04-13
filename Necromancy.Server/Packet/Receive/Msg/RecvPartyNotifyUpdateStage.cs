using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyNotifyUpdateStage : PacketResponse
    {
        private readonly int _chapterId;
        private readonly uint _charaInstanceId;
        private readonly int _dungeonId;
        private readonly byte _floor;

        public RecvPartyNotifyUpdateStage(uint charaInstanceId, int dungeonId, byte floor, int chapterId)
            : base((ushort)MsgPacketId.recv_party_notify_update_stage, ServerType.Msg)
        {
            _charaInstanceId = charaInstanceId;
            _dungeonId = dungeonId;
            _floor = floor;
            _chapterId = chapterId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_charaInstanceId); //chara instance id
            res.WriteInt32(_dungeonId); //dungeon id (almost the same as map id, but multi level/map dungeons have 1 dungeon id, but 8 map IDs)
            res.WriteByte(_floor); //dungeon lvl (pew pew pew... no wait.  probably the floor you're on)
            res.WriteInt32(_chapterId); //chapter id
            return res;
        }
    }
}
