using Arrowgene.Services.Buffers;
using Arrowgene.Services.Networking.Tcp;

namespace Necromancy.Server
{
    public class AreaServer : NecromancyServer
    {
        public override void OnReceivedData(ITcpSocket socket, byte[] data)
        {
            IBuffer buffer = new StreamBuffer(data);
            PacketLogIn(buffer);
            buffer.SetPositionStart();

            int size = buffer.ReadInt16(Endianness.Big);
            int opCode = buffer.ReadInt16(Endianness.Big);

            switch (opCode)
            {
                default:
                {
                    _logger.Error($"[World]OPCode: {opCode} not handled");
                    break;
                }
            }
        }
    }
}