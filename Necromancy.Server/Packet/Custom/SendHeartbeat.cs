using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Custom
{
    public class SendHeartbeat : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendHeartbeat));
        public SendHeartbeat(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)CustomPacketId.SendHeartbeat;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint time = packet.data.ReadUInt32();
            client.heartBeat = time;
            //_Logger.Debug($"Time since client executable start in milliseconds {time}");

            //TODO :  if (client.heartBeat != client.heartBeat2) { "SomeBody is glitching the matrix. stop them!"}

            IBuffer buffer = BufferProvider.Provide();
            buffer.WriteInt32(0);
            buffer.WriteUInt32(time);

            NecPacket response = new NecPacket(
                (ushort)CustomPacketId.RecvHeartbeat,
                buffer,
                packet.serverType,
                PacketType.HeartBeat
            );

            router.Send(client, response);
        }
    }
}
