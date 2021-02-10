using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_forge_execute : ClientHandler
    {
        public send_forge_execute(NecServer server) : base(server)
        {
        }

        int i = 0;
        int j = 0;
        public override ushort Id => (ushort) AreaPacketId.send_forge_execute;

        public override void Handle(NecClient client, NecPacket packet)
        {
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadInt16();
            uint forgeStoneCount = packet.Data.ReadUInt32();
            for (int i = 0; i < forgeStoneCount; i++)
            {
                packet.Data.ReadByte();
                packet.Data.ReadByte();
                packet.Data.ReadInt16();
            }
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            short forgePrice = packet.Data.ReadInt16();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadInt16();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1);// 0 is a pass, anything but 0 is a fail; seems like a check for if you can still upgrade the weapon
            res.WriteInt32(1);// anything but a 1 here is a fail condition, 1 here is a pass condition.
            Router.Send(client, (ushort) AreaPacketId.recv_forge_execute_r, res, ServerType.Area);
        }
    }
}
