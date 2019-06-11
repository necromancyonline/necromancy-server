using Arrowgene.Services.Buffers;
using Arrowgene.Services.Networking.Tcp;

namespace Necromancy.Server
{
    public class MessageServer : NecromancyServer
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
                case 0x0557: // network::proto_msg_implement_client::send_base_check_version
                {
                    uint unknown = buffer.ReadUInt32();
                    uint major = buffer.ReadUInt32();
                    uint minor = buffer.ReadUInt32();
                    _logger.Info($"{major} - {minor}");
                    IBuffer res = new StreamBuffer();

                    res.WriteInt32(0);
                    res.WriteInt32(unknown);
                    res.WriteInt32(major);
                    res.WriteInt32(minor);

                    Send(socket, 0xDDEF, res); // network::proto_msg_implement_client::recv_base_check_version_r
                    break;
                }
                case 0x3DA5: // network::proto_msg_implement_client::send_base_login
                {

                    buffer.Position = 7;
                    string macAddress = buffer.ReadFixedString(17);
                    IBuffer res = new StreamBuffer();
                    res.WriteInt32(0);
                    
                    // TODO find proto_msg_implement_client::send_base_login

                   // Send(socket, 0xDDEF, res); // network::proto_msg_implement_client::recv_base_check_version_r
                    break;
                }
                default:
                {
                    _logger.Error($"[World]OPCode: {opCode} not handled");
                    break;
                }
                
                // MessageServer Switch - 0x004E4210
                // OP Codes: (all Hex 0X)
                // 831C
                // 4090
                // 1AF3
                // 087D
                // 0482
                
                // 082 - 019C
                // 082 - 019C - 002A
                // 082 - 019C - 002A - 0003
                
                // BA73
                // 9AD1
                // 8E0A
                // 8D24
                // 8D24 - 853F
                // 8D24 - 853F - 0637
                // 8D24 - 853F - 0637 - 003B
                
            }
        }
    }
}