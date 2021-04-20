using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvMapChangeForce : PacketResponse
    {
        private readonly NecClient _client;
        private readonly Map _map;
        private readonly MapPosition _mapPosition;
        private readonly NecSetting _setting;

        public RecvMapChangeForce(Map map, MapPosition mapPosition, NecSetting setting, NecClient client)
            : base((ushort)AreaPacketId.recv_map_change_force, ServerType.Area)
        {
            _map = map;
            _mapPosition = mapPosition;
            _setting = setting;
            _client = client;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //sub_484B00 map ip and connection
            res.WriteUInt32(_client.character.instanceId); //MapSerialID
            res.WriteInt32(_map.id); //MapID
            res.WriteInt32(_map.id); //MapID
            res.WriteByte(0); //new
            res.WriteByte(0); //new bool
            res.WriteFixedString(_setting.dataAreaIpAddress, 65); //IP
            res.WriteUInt16(_setting.areaPort); //Port
            if (_mapPosition == null)
            {
                res.WriteFloat(_map.x); //X Pos
                res.WriteFloat(_map.y); //Y Pos
                res.WriteFloat(_map.z); //Z Pos
                res.WriteByte(_map.orientation); //View offset
            }
            else
            {
                res.WriteFloat(_mapPosition.x); //X Pos
                res.WriteFloat(_mapPosition.y); //Y Pos
                res.WriteFloat(_mapPosition.z); //Z Pos
                res.WriteByte(_mapPosition.heading); //View offset
            }

            return res;
        }
    }
}
