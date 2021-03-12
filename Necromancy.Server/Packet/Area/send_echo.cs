using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_echo : ClientHandler
    {
        public send_echo(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_echo;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int unknown = packet.Data.ReadInt32(); //Chat type?
            int unknown2 = packet.Data.ReadInt32(); //Chat type also maybe?
            int size = packet.Data.ReadInt32();
            byte[] message = packet.Data.ReadBytes(size);
            
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(unknown);
            res2.WriteInt32(unknown2);
            int numEntries = size;
            res2.WriteInt32(numEntries);//Less than or equal to 0x4E20
            for(int i = 0; i < numEntries; i++)
            {
                res2.WriteByte(message[i]);
            }
            Router.Send(client.Map, (ushort)AreaPacketId.recv_echo_notify, res2, ServerType.Area);

            IBuffer res = BufferProvider.Provide();
            Router.Send(client, (ushort) AreaPacketId.recv_echo_r, res, ServerType.Area);
        }
    }
}
