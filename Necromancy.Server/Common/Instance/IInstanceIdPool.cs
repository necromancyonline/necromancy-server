namespace Necromancy.Server.Common.Instance
{
    public interface IInstanceIdPool
    {
        uint used { get; }
        uint lowerBound { get; }
        uint upperBound { get; }
        uint size { get; }
        string name { get; }
    }
}
