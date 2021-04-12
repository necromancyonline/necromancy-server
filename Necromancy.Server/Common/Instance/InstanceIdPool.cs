using System.Collections.Concurrent;
using Arrowgene.Logging;

namespace Necromancy.Server.Common.Instance
{
    public class InstanceIdPool : IInstanceIdPool
    {
        private static readonly ILogger _Logger = LogProvider.Logger(typeof(InstanceIdPool));

        private readonly ConcurrentStack<uint> _idPool;

        public InstanceIdPool(string name, uint lowerBound, uint size)
        {
            _idPool = new ConcurrentStack<uint>();
            this.lowerBound = lowerBound;
            this.name = name;
            this.size = size;
            upperBound = this.lowerBound + this.size;
            _Logger.Debug($"Pool:{this.name} Loading:{this.size}");
            for (uint i = this.lowerBound; i < upperBound; i++) _idPool.Push(i);

            _Logger.Debug($"Pool:{this.name} - Finished");
        }

        public uint used => size - (uint)_idPool.Count;
        public uint lowerBound { get; }

        public uint upperBound { get; }
        public uint size { get; }
        public string name { get; }

        public void Push(uint id)
        {
            if (id < lowerBound || id > upperBound)
            {
                _Logger.Error($"Id: {id} does not belong to pool {name} ({lowerBound}-{upperBound})");
                return;
            }

            _idPool.Push(id);
        }

        public bool TryPop(out uint id)
        {
            return _idPool.TryPop(out id);
        }
    }
}
