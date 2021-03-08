using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyGGateStoneData : PacketResponse
    {
        private GGateSpawn _gGateSpawn;

        public RecvDataNotifyGGateStoneData(GGateSpawn gGateSpawn)
            : base((ushort) AreaPacketId.recv_data_notify_ggate_stone_data, ServerType.Area)
        {
            _gGateSpawn = gGateSpawn;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_gGateSpawn.InstanceId); // Unique Object ID.
            res.WriteInt32(_gGateSpawn.Id); // Serial ID for Interaction? from npc.csv????
            res.WriteByte(_gGateSpawn.Interaction); // 0 = Text, 1 = F to examine  , 2 or above nothing
            res.WriteCString(_gGateSpawn.Name); //"0x5B" //Name
            res.WriteCString(_gGateSpawn.Title); //"0x5B" //Title
            res.WriteFloat(_gGateSpawn.X); //X
            res.WriteFloat(_gGateSpawn.Y); //Y
            res.WriteFloat(_gGateSpawn.Z); //Z
            res.WriteByte(_gGateSpawn.Heading); //
            res.WriteInt32(_gGateSpawn.ModelId); // Optional Model ID. Warp Statues. Gaurds, Pedistals, Etc., to see models refer to the model_common.csv
            res.WriteInt16(_gGateSpawn.Size); //  size of the object
            res.WriteInt32(_gGateSpawn.Active); // 0 = collision, 1 = no collision  (active/Inactive?)
            res.WriteInt32(_gGateSpawn.Glow); //0= no effect color appear, //Red = 0bxx1x   | Gold = obxxx1   |blue = 0bx1xx

            return res;
        }
    }
}
