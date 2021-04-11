using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendCharabodyLootStart3 : ClientHandler
    {
        public SendCharabodyLootStart3(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort) AreaPacketId.send_charabody_loot_start3;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte fromZone = packet.data.ReadByte();
            byte fromContainer = packet.data.ReadByte();
            short fromSlot = packet.data.ReadInt16();
            packet.data.ReadInt32();

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);
            res.WriteInt32(0);

            router.Send(client, (ushort)AreaPacketId.recv_charabody_loot_start2_r, res, ServerType.Area);
        }
    }
}
