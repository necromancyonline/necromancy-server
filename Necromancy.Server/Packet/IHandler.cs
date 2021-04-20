namespace Necromancy.Server.Packet
{
    public interface IHandler
    {
        ushort id { get; }
        int expectedSize { get; }
    }
}
