using Arrowgene.Services.Buffers;
using Arrowgene.Services.Networking.Tcp;

namespace Necromancy.Server
{
    public class AuthenticationServer : NecromancyServer
    {
        public override void OnReceivedData(ITcpSocket socket, byte[] data)
        {
            IBuffer buffer = new StreamBuffer(data);
            PacketLogIn(buffer);
            buffer.SetPositionStart();

            ushort size = buffer.ReadUInt16(Endianness.Big);
            ushort opCode = buffer.ReadUInt16(Endianness.Big);

            switch (opCode)
            {
                case 0x0557: //network::proto_auth_implement_client::send_base_check_version
                {
                    uint major = buffer.ReadUInt32();
                    uint minor = buffer.ReadUInt32();
                    _logger.Info($"{major} - {minor}");
                    IBuffer res = new StreamBuffer();
                    res.WriteInt32(0);
                    res.WriteInt32(major);
                    res.WriteInt32(minor);
                    Send(socket, 0xDDEF, res); //network::proto_auth_implement_client::recv_base_check_version_r
                    break;
                }
                case 0x93AD: //network::proto_auth_implement_client::send_base_authenticate
                {
                    string accountName = buffer.ReadCString();
                    string password = buffer.ReadCString();
                    string macAddress = buffer.ReadCString();
                    int unknown = buffer.ReadInt16();
                    _logger.Info($"[Login]Account:{accountName} Password:{password} Unknown:{unknown}");

                    // TODO find network::proto_auth_implement_client::recv_base_authenticate_r op code

                     IBuffer res = new StreamBuffer();
                     res.WriteInt32(0);
                     res.WriteInt32(1);
                     //res.WriteInt32(1);
                     //res.WriteInt32(0);
                     //res.WriteCString("1");
                     //res.WriteCString("n");
                     //res.WriteCString("127.0.0.1");
                     //res.WriteCString("60001");

                     //Send(socket, 0x4A17, res); //proto_auth_implement_client::recv_base_authenticate_soe_r() structure is writeint32(0),writeint32(1)
                     //Send(socket, 0x530A, res); //proto_auth_implement_client::recv_base_authenticate_hangame_r structure is writeint32(0),writeint32(1)
                     Send(socket, 0xC715, res); //proto_auth_implement_client::recv_base_authenticate_r structure is writeint32(0),writeint32(1)
                     //Send(socket, 0xD6D2, res); //proto_auth_implement_client::recv_base_ping_r  has no structure
                     //Send(socket, 0x17B7, res); //proto_auth_implement_client::recv_base_get_worldlist_r structure is writeint32(0)+writrecstring("name")(or equvalent in bytes)
                     //Send(socket, 0x73BA, res); //proto_auth_implement_client::recv_cpf_authenticate structure is writeint(0)
                     //Send(socket, 0x0AEC, res); //proto_auth_implement_client::recv_base_check_version2_r structure seems to be 18 bytes?
                     //Send(socket, 0xD773, res); //proto_auth_implement_client::recv_cpf_notify_error  has no structure
                     //Send(socket, 0xEA7E, res); //proto_auth_implement_client::recv_base_authenticate_soe_sessionid structure seems to be 1 byte


                        // Authentication Server Switch - 0x004DE650
                        // OP Codes:
                        //848c proto_auth_implement_client::recv_base_select_world_r()
                        //174a proto_auth_implement_client::recv_base_authenticate_soe_r()
                        //0a53 proto_auth_implement_client::recv_base_authenticate_hangame_r
                        //15c7 proto_auth_implement_client::recv_base_authenticate_r
                        //d2d6 proto_auth_implement_client::recv_base_ping_r
                        //b717 proto_auth_implement_client::recv_base_get_worldlist_r
                        //ba73 proto_auth_implement_client::recv_cpf_authenticate
                        //ec0a proto_auth_implement_client::recv_base_check_version2_r
                        //efdd proto_auth_implement_client::recv_base_check_version_r
                        //73d7 proto_auth_implement_client::recv_cpf_notify_error
                        //7eea proto_auth_implement_client::recv_base_authenticate_soe_sessionid

                        // Send Move to world for testing currently. 
                        // Replace if we know correct flow
                        /*IBuffer res = new StreamBuffer();
                        res.WriteInt32(0);
                        res.WriteCString("127.0.0.1"); // Message Server IP
                        res.WriteInt32(60001); // Message Server Port
                        Send(socket, 0x8C84, res); //"network::proto_auth_implement_client::recv_base_select_world_r()"*/
                        break;
                }
                case 0xCF53: //network::proto_auth_implement_client::send_base_select_world
                {
                        byte lastByte = 1;
                        byte otherBytes = 255;

                        IBuffer res = new StreamBuffer();
                        res.WriteInt32(0);
                        res.WriteByte(otherBytes);
                        res.WriteByte(otherBytes);
                        res.WriteByte(otherBytes);
                        res.WriteByte(otherBytes);
                        res.WriteByte(lastByte);

                        //res.WriteCString("nam");
                        //Data sent
                        //00 00 00 00 
                        //00 00 00 00 01 01 01 01 01 produces world merge explaination dualogue.
                        //00 00 00 00 0A 0A 0A 0A 0A produces world selection dialogue. 
                        //00 00 00 00 00 00 00 00 01 is it the last byte being 01 that produces world merge dialgue?
                        //does nothing special: 02, 03, 04, 05, 06, 07, 08, 09, 0A, 0B, 0C, 0D, 0E, 0F, 0x10,
                        //00 00 00 00 00 00 00 01 00 does nothing special
                        //

                        Send(socket, 0x17B7, res); //proto_auth_implement_client::recv_base_get_worldlist_r structure is writeint32(0)+writrecstring("name")(or equvalent in bytes)
                        break;
                    }
                default:
                {
                    _logger.Error($"[Login]OPCode: {opCode} not handled");
                    break;
                }
            }
        }
    }
}
 