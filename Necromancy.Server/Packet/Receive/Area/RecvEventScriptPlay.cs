using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventScriptPlay : PacketResponse
    {
        private readonly uint _objectId;
        private readonly string _scriptPath;

        public RecvEventScriptPlay(string scriptPath, uint objectId)
            : base((ushort)AreaPacketId.recv_event_script_play, ServerType.Area)
        {
            _scriptPath = scriptPath;
            _objectId = objectId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString(_scriptPath); // find max size
            res.WriteUInt32(_objectId); //ObjectID
            return res;
        }
    }
}
