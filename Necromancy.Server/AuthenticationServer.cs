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
                case 0xCF53: //network::proto_auth_implement_client::send_base_get_worldlist

                {
                        IBuffer res = new StreamBuffer();
                        res.WriteInt32(1);    //Number of List Entry
                        res.WriteByte((byte)0);//u8(0); ?
                        res.WriteByte((byte)0);//u8(0); ?
                        res.WriteByte((byte)0);//u8(0); ?

                        for (byte i = 0; i < 1; i++)
                        {
                            res.WriteByte((byte)(i+1));    //u8(i) Maybe Server ID? (I'm assuming this comes in with send_base_select_world, but the client doesn't go down that route when selecting a server right now.)

                            res.WriteFixedString("Necromancy", 42); // utf8("Server Name", 42);    //Server Name, 42 length (Probably not 42, but nothing after it seems obvious. Needs checking.)

                            res.WriteInt16(15);//u16(15); Max Player Count
                            res.WriteInt16(10);// u16(10);    Current Player Count

                            //Something about these sets the Icon "L" next to the server.
                            res.WriteByte((byte)0);// u8(0);
                            res.WriteByte((byte)0);// u8(0);
                        }
                        res.WriteInt16(0);//u16(0);    Necessary. Not sure if there's anything here?

                        //res.WriteCString("nam");
                        //Data sent
                        //00 0B 17 B7 01 01 01 01 00 00 00 00 00 doesnt hit the string(incorrect packet)
                        //00 0B 17 B7 01 00 00 00 00 00 00 00 00 doesnt hit the string either
                        //00 00 00 00 
                        //00 00 00 00 01 01 01 01 01 produces world merge explaination dialogue.
                        //00 00 00 00 0A 0A 0A 0A 0A produces world selection dialogue. 
                        //00 00 00 00 00 00 00 00 01 is it the last byte being 01 that produces world merge dialgue?
                        //does nothing special: 02, 03, 04, 05, 06, 07, 08, 09, 0A, 0B, 0C, 0D, 0E, 0F, 0x10,
                        //00 00 00 00 00 00 00 01 00 does nothing special
                        //00 00 00 00 FF FF FF FF 01 produces world merge explanation dialogue.

                        Send(socket, 0x17B7, res); //proto_auth_implement_client::recv_base_get_worldlist_r 

                        /*IBuffer res2 = new StreamBuffer();
                        res2.WriteByte(1);//jmp > 0x0a
                        res2.WriteByte(0);
                        res2.WriteByte(0);
                        res2.WriteByte(0);

                        // 4bytes
                        res2.WriteByte(0);
                        res2.WriteByte(0);
                        res2.WriteByte(0);
                        res2.WriteByte(0);

                        //37 byte
                        res2.WriteFixedString("Necromancy", 37); // 16 bytes

                        //15byte
                        res2.WriteFixedString("127.0.0.1", 15);

                        Send(socket, 0x17B7, res2);//proto_auth_implement_client::recv_base_get_worldlist_r*/

                        break;
                    }

                    case 0x3F20: //network::proto_auth_implement_client::send_base_select_world
                    {
                        IBuffer res = new StreamBuffer();
                        res.WriteInt32(0);
                        res.WriteCString("127.0.0.1"); // Message Server IP
                        res.WriteInt32(60001); // Message Server Port
                        Send(socket, 0x8C84, res); //"network::proto_auth_implement_client::recv_base_select_world_r()"

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
 