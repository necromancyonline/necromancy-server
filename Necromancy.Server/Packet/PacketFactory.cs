using System;
using System.Collections.Generic;
using System.Threading;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Packet
{
    public class PacketFactory
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(PacketFactory));

        public const int PacketIdSize = 2;
        public const int PacketLengthTypeSize = 1;
        public const int HeartbeatPacketBodySize = 8;
        public const int Unknown1PacketBodySize = 8;
        public const int DisconnectPacketBodySize = 4;

        private bool _readHeader;
        private bool _readPacketLengthType;
        private uint _dataSize;
        private ushort _id;
        private int _position;
        private IBuffer _buffer;
        private readonly NecSetting _setting;
        private byte[] _header;
        private PacketType _packetType;
        private byte _headerSize;

        public PacketFactory(NecSetting setting)
        {
            _setting = setting;
            Reset();
        }

        public byte[] Write(NecPacket packet)
        {
            byte[] data = packet.data.GetAllBytes();
            IBuffer buffer = BufferProvider.Provide();

            PacketType packetType;
            switch (packet.packetType)
            {
                case PacketType.HeartBeat:
                    packetType = PacketType.HeartBeat;
                    buffer.WriteByte((byte) packetType);
                    buffer.WriteBytes(data);
                    break;
                case PacketType.Unknown1:
                    packetType = PacketType.Unknown1;
                    buffer.WriteByte((byte) packetType);
                    buffer.WriteBytes(data);
                    break;
                case PacketType.Disconnect:
                    packetType = PacketType.Disconnect;
                    buffer.WriteByte((byte) packetType);
                    buffer.WriteBytes(data);
                    break;
                default:
                    ulong dataSize = (ulong) (data.Length + PacketIdSize);
                    if (dataSize < byte.MaxValue)
                    {
                        packetType = PacketType.Byte;
                        buffer.WriteByte((byte) packetType);
                        buffer.WriteByte((byte) dataSize);
                    }
                    else if (dataSize < ushort.MaxValue)
                    {
                        packetType = PacketType.UInt16;
                        buffer.WriteByte((byte) packetType);
                        buffer.WriteUInt16((ushort) dataSize);
                    }
                    else if (dataSize < uint.MaxValue)
                    {
                        packetType = PacketType.UInt32;
                        buffer.WriteByte((byte) packetType);
                        buffer.WriteUInt32((uint) dataSize);
                    }
                    else
                    {
                        _Logger.Error($"{dataSize} to big");
                        return null;
                    }

                    buffer.WriteUInt16(packet.id);
                    buffer.WriteBytes(data);
                    break;
            }

            byte headerSize = CalculateHeaderSize(packetType);
            packet.header = buffer.GetBytes(0, headerSize);

            return buffer.GetAllBytes();
        }

        public List<NecPacket> Read(byte[] data, ServerType serverType)
        {
            List<NecPacket> packets = new List<NecPacket>();
            if (_buffer == null)
            {
                _buffer = BufferProvider.Provide(data);
            }
            else
            {
                _buffer.SetPositionEnd();
                _buffer.WriteBytes(data);
            }

            _buffer.Position = _position;

            bool read = true;
            while (read)
            {
                read = false;

                if (!_readPacketLengthType
                    && _buffer.Size - _buffer.Position > PacketLengthTypeSize)
                {
                    byte lengthType = _buffer.ReadByte();
                    if (!Enum.IsDefined(typeof(PacketType), lengthType))
                    {
                        _Logger.Error($"PacketType: '{lengthType}' not found");
                        Reset();
                        return packets;
                    }

                    _readPacketLengthType = true;
                    _packetType = (PacketType) lengthType;
                    _headerSize = CalculateHeaderSize(_packetType);
                }

                if (_readPacketLengthType
                    && !_readHeader
                    && _buffer.Size - _buffer.Position >= _headerSize - PacketLengthTypeSize)
                {
                    // TODO acquire 1st byte differently in case -1 doesnt work
                    _header = _buffer.GetBytes(_buffer.Position - PacketLengthTypeSize, _headerSize);

                    switch (_packetType)
                    {
                        case PacketType.Byte:
                        {
                            _dataSize = _buffer.ReadByte();
                            _dataSize -= PacketIdSize;
                            _id = _buffer.ReadUInt16();
                            _readHeader = true;
                            break;
                        }
                        case PacketType.UInt16:
                        {
                            _dataSize = _buffer.ReadUInt16();
                            _dataSize -= PacketIdSize;
                            _id = _buffer.ReadUInt16();
                            _readHeader = true;
                            break;
                        }
                        case PacketType.UInt32:
                        {
                            _dataSize = _buffer.ReadUInt32();
                            _dataSize -= PacketIdSize;
                            _id = _buffer.ReadUInt16();
                            _readHeader = true;
                            break;
                        }
                        case PacketType.HeartBeat:
                        {
                            _dataSize = HeartbeatPacketBodySize;
                            _id = (ushort) CustomPacketId.SendHeartbeat;
                            _readHeader = true;
                            break;
                        }
                        case PacketType.Unknown1:
                        {
                            _dataSize = Unknown1PacketBodySize;
                            _id = (ushort) CustomPacketId.SendUnknown1;
                            _readHeader = true;
                            break;
                        }
                        case PacketType.Disconnect:
                        {
                            _dataSize = DisconnectPacketBodySize;
                            _id = (ushort) CustomPacketId.SendDisconnect;
                            _readHeader = true;
                            break;
                        }
                        default:
                        {
                            // TODO update arrowgene buffer to read uint24 && int24
                            _Logger.Error($"PacketType: '{_packetType}' not supported");
                            Reset();
                            return packets;
                        }
                    }
                }

                if (_readPacketLengthType
                    && _readHeader
                    && _buffer.Size - _buffer.Position >= _dataSize)
                {
                    // TODO update arrowgene service to read uint
                    byte[] packetData = _buffer.ReadBytes((int) _dataSize);
                    IBuffer buffer = BufferProvider.Provide(packetData);
                    NecPacket packet = new NecPacket(_id, buffer, serverType);
                    packet.header = _header;
                    packets.Add(packet);
                    _readPacketLengthType = false;
                    _readHeader = false;
                    read = _buffer.Position != _buffer.Size;
                }
            }

            if (_buffer.Position == _buffer.Size)
            {
                Reset();
            }
            else
            {
                _position = _buffer.Position;
            }

            return packets;
        }

        private void Reset()
        {
            _readPacketLengthType = false;
            _readHeader = false;
            _dataSize = 0;
            _id = 0;
            _position = 0;
            _buffer = null;
            _header = null;
        }

        private byte CalculateHeaderSize(PacketType packetType)
        {
            switch (packetType)
            {
                case PacketType.HeartBeat:
                {
                    return PacketLengthTypeSize;
                }
                case PacketType.Unknown1:
                {
                    return PacketLengthTypeSize;
                }
                case PacketType.Disconnect:
                {
                    return PacketLengthTypeSize;
                }
                default:
                {
                    return (byte) (PacketLengthTypeSize + (packetType + 1) + PacketIdSize);
                }
            }
        }

        private int CalculatePadding(int size)
        {
            return (4 - (size & 3)) & 3;
        }
    }
}
