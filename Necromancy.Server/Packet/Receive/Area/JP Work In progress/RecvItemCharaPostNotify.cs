using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_item_chara_post_notify : PacketResponse
    {
        /// <summary>
        /// Notifies you that an item was mailed to you because your inventory was full.
        /// "Leather Armor was sent to the reward receiver due to inventory limit"
        /// </summary>
        private int _serialId;
        private byte _count;
        public recv_item_chara_post_notify(int serialId, byte count)
            : base((ushort) AreaPacketId.recv_item_chara_post_notify, ServerType.Area)
        {
            _serialId = serialId;
            _count = count;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_serialId);
            res.WriteByte(_count);
            return res;
        }
    }
}
