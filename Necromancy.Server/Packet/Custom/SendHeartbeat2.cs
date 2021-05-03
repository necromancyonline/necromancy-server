using System;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Custom
{
    public class SendHeartbeat2 : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendHeartbeat));
        public SendHeartbeat2(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)CustomPacketId.SendHeartbeat2;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint a = packet.data.ReadUInt32();
            uint time = packet.data.ReadUInt32();
            client.heartBeat = time;
            // Logger.Info(client, $"Unknown1 A:{a / 1000}");
            // Logger.Info(client, $"Unknown1 B:{b / 1000}");
            _Logger.Debug($"Time since client executable start in seconds {time/1000}");
            _Logger.Debug($" if another heartbeat is not received in 600 seconds the client will be disconnected at {(time / 1000) + 600} : {DateTime.Now.AddSeconds(600)}");


            IBuffer buffer = BufferProvider.Provide();
            buffer.WriteInt32(0);
            buffer.WriteUInt32(time);

            NecPacket response = new NecPacket(
                (ushort)CustomPacketId.RecvHeartbeat2,
                buffer,
                packet.serverType,
                PacketType.HeartBeat2
            );

            router.Send(client, response);

            Task.Delay(TimeSpan.FromSeconds(600)).ContinueWith
            (t1 =>
                {
                    if (client != null)
                        if (client.heartBeat == time)
                        {
                            server.clients.Remove(client);
                            _Logger.Error($"Heartbeat missed. disconnecting client. Server.clientCount is now {server.clients.GetCount()}");
                        }
                }
            );
        }
    }
}
