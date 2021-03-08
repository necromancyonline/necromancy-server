using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifySoulMaterialObjectData : PacketResponse
    {
        public RecvDataNotifySoulMaterialObjectData()
            : base((ushort) AreaPacketId.recv_data_notify_soulmaterialobject_data, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //objectID

            res.WriteFloat(0);//X
            res.WriteFloat(0);//Y
            res.WriteFloat(0);//Z

            res.WriteFloat(0);//X
            res.WriteFloat(0);//Y
            res.WriteFloat(0);//Z
            res.WriteByte(0);

            res.WriteInt32(0); //ap??

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);

            res.WriteInt32(0); //Stateflag??
            return res;
        }
    }
}
