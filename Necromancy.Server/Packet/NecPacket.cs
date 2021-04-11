using System;
using Arrowgene.Buffers;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet
{
    public class NecPacket
    {
        public static string GetPacketIdName(ushort id, ServerType serverType)
        {
            switch (serverType)
            {
                case ServerType.Auth:
                    if (Enum.IsDefined(typeof(AuthPacketId), id))
                    {
                        AuthPacketId authPacketId = (AuthPacketId) id;
                        return authPacketId.ToString();
                    }

                    break;
                case ServerType.Msg:
                    if (Enum.IsDefined(typeof(MsgPacketId), id))
                    {
                        MsgPacketId msgPacketId = (MsgPacketId) id;
                        return msgPacketId.ToString();
                    }

                    break;
                case ServerType.Area:
                    if (Enum.IsDefined(typeof(AreaPacketId), id))
                    {
                        AreaPacketId areaPacketId = (AreaPacketId) id;
                        return areaPacketId.ToString();
                    }

                    break;
            }

            if (Enum.IsDefined(typeof(CustomPacketId), id))
            {
                CustomPacketId customPacketId = (CustomPacketId) id;
                return customPacketId.ToString();
            }

            return null;
        }

        private string _packetIdName;

        public NecPacket(ushort id, IBuffer buffer, ServerType serverType)
        {
            data = buffer;
            this.id = id;
            this.serverType = serverType;
            packetType = null;
        }

        public NecPacket(ushort id, IBuffer buffer, ServerType serverType, PacketType packetType)
        {
            data = buffer;
            this.id = id;
            this.serverType = serverType;
            this.packetType = null;
            this.packetType = packetType;
        }

        public IBuffer data { get; }
        public ushort id { get; }
        public byte[] header { get; set; }
        public ServerType serverType { get; }
        public PacketType? packetType { get; }

        public string packetIdName
        {
            get
            {
                if (_packetIdName != null)
                {
                    return _packetIdName;
                }

                _packetIdName = GetPacketIdName(id, serverType);
                if (_packetIdName == null)
                {
                    _packetIdName = $"ID_NOT_DEFINED_{serverType}_{id}";
                }

                return _packetIdName;
            }
        }
    }
}
