using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSelfDragonPosNotify : PacketResponse
    {
        private NecClient _client;
        private byte _activate;
        public RecvSelfDragonPosNotify(NecClient client, byte activate)
            : base((ushort) AreaPacketId.recv_self_dragon_pos_notify, ServerType.Area)
        {
            _client = client;
            _activate = activate;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_client.character.instanceId + 100); //dragon statue objectId

            res.WriteFloat(_client.character.x); // x
            res.WriteFloat(_client.character.y); //y
            res.WriteFloat(_client.character.z); //z

            res.WriteByte(_activate); //on / off
            return res;
        }
    }
}
