using Necromancy.Server.Database;
using Necromancy.Server.Model;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Packet
{
    public abstract class Handler : IHandler
    {
        protected Handler(NecServer server)
        {
            this.server = server;
            router = server.router;
            database = server.database;
            settings = server.setting;
            maps = server.maps;
            clients = server.clients;
        }

        protected NecServer server { get; }
        protected NecSetting settings { get; }
        protected PacketRouter router { get; }
        protected MapLookup maps { get; }
        protected ClientLookup clients { get; }
        protected IDatabase database { get; }

        public abstract ushort id { get; }
        public virtual int expectedSize => NecQueueConsumer.NoExpectedSize;
    }
}
