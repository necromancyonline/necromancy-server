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
                    byte otherBytes = 0;
                    //buffer.Position = 7;
                    //string macAddress = buffer.ReadFixedString(17);
                    IBuffer res = new StreamBuffer();
                    res.WriteByte(otherBytes);
                    res.WriteByte(otherBytes);
                    res.WriteByte(otherBytes);
                    res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);
                    //res.WriteByte(otherBytes);

                    Send(socket, 0x1C83, res);
                    
                   // TODO find proto_msg_implement_client::send_base_login
                    break;
                }
                default:
                {
                    _logger.Error($"[World]OPCode: {opCode} not handled");
                    break;
                }

                    // MessageServer Switch - 0x004E4210
                    // OP Codes: (all Hex 0X)
                    // 831C //unable to figure this one out 24+opcode max
                    // 4090 proto_msg_implement_client::recv_party_notify_update_map 00 13 90 40 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
                    // 1AF3
                    // 087D
                    // 0482

                    // 082 - 019C
                    // 082 - 019C - 002A
                    // 082 - 019C - 002A - 0003

                    // BA73 proto_msg_implement_client::recv_cpf_authentication 00 06 73 BA 00 00 00 00
                    // 9AD1 proto_msg_implement_client::recv_chara_delete_r 00 06 D1 9A 00 00 00 00
                    // 8E0A proto_msg_implement_client::recv_party_notify_kick (no structure)
                    // 8D24
                    // 8D24 - 853F
                    // 8D24 - 853F - 0637
                    // 8D24 - 853F - 0637 - 003B

            }
        }
    }
}