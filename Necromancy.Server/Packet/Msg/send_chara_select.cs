using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_select : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_chara_select));

        public send_chara_select(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_chara_select;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int characterId = packet.Data.ReadInt32();
            Character character = Database.SelectCharacterById(characterId);
            if (character == null)
            {
                Logger.Error(client, $"No character for CharacterId: {characterId}");
                client.Close();
                return;
            }
            Server.Instances.AssignInstance(character); 

            client.Character = character;
            client.UpdateIdentity();
            client.Character.CreateTask(Server, client);

            Logger.Debug(client, $"Selected Character: {character.Name}");

            IBuffer res3 = BufferProvider.Provide();
            res3.WriteInt32(0); //ERR-CHARSELECT error check
            res3.WriteUInt32(client.Character.InstanceId);

            //sub_4E4210_2341  // 
            //res3.WriteInt32(0);//client.Character.MapId); //MapSerialID //passeed to Send_Map_Entry
            //res3.WriteInt32(0);//client.Character.MapId); //MapID

            res3.WriteInt32(client.Character.MapId);
            res3.WriteInt32(client.Character.MapId);
            res3.WriteInt32(client.Character.MapId);
            res3.WriteByte(0);
            res3.WriteByte(0); //Bool
            res3.WriteFixedString(Settings.DataAreaIpAddress, 0x41); //IP
            res3.WriteUInt16(Settings.AreaPort); //Port

            res3.WriteFloat(client.Character.X);
            res3.WriteFloat(client.Character.Y);
            res3.WriteFloat(client.Character.Z);
            res3.WriteByte(client.Character.Heading);
            Router.Send(client, (ushort) MsgPacketId.recv_chara_select_r, res3, ServerType.Msg);

            /*
             ERR_CHARSELECT	GENERIC	Failed to select a character (CODE:<errcode>)
             ERR_CHARSELECT	-8	Maintenance
             ERR_CHARSELECT	-13	You have selected an illegal character
            */


            //Logic to support your dead body //Do Dead Body IDs need to be persistant, or can they change at each login?  TODO...
            DeadBody deadBody = new DeadBody();
            Server.Instances.AssignInstance(deadBody);
            character.DeadBodyInstanceId = deadBody.InstanceId;
            deadBody.CharacterInstanceId = character.InstanceId;
            character.movementId = character.InstanceId;
            Logger.Debug($"Dead Body Instance ID {deadBody.InstanceId}   |  Character Instance ID {character.InstanceId}");

        }
    }
}
