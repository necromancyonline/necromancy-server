using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_loot_complete3 : ClientHandler
    {
        public send_charabody_loot_complete3(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_charabody_loot_complete3;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte fromZone = packet.Data.ReadByte();
            byte fromContainer = packet.Data.ReadByte();
            short fromSlot = packet.Data.ReadInt16();

            IBuffer res = BufferProvider.Provide();
            //Move item here.
        }
    }
}
