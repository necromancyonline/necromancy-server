namespace Necromancy.Server.Packet
{
    public enum PacketType : byte
    {
        Byte = 0,
        UInt16 = 1,
        UInt24 = 2,
        UInt32 = 3,
        HeartBeat = 16,
        HeartBeat2 = 32,
        Disconnect = 48
    }
}
