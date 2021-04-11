using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Stats;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendCharaSelect : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendCharaSelect));

        public SendCharaSelect(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) MsgPacketId.send_chara_select;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int characterId = packet.data.ReadInt32();
            Character character = database.SelectCharacterById(characterId);

            if (character == null)
            {
                _Logger.Error(client, $"No character for CharacterId: {characterId}");
                client.Close();
                return;
            }
            server.instances.AssignInstance(character);

            client.character = character;
            client.character.criminalState = client.soul.criminalLevel;
            client.UpdateIdentity();
            client.character.CreateTask(server, client);

            _Logger.Debug(client, $"Selected Character: {character.name}");

            IBuffer res3 = BufferProvider.Provide();
            res3.WriteInt32(0); //ERR-CHARSELECT error check
            res3.WriteUInt32(client.character.instanceId);

            //sub_4E4210_2341
            res3.WriteInt32(client.character.mapId);
            res3.WriteInt32(client.character.mapId);
            res3.WriteInt32(client.character.mapId);
            res3.WriteByte(0);
            res3.WriteByte(0); //Bool
            res3.WriteFixedString(settings.dataAreaIpAddress, 0x41); //IP
            res3.WriteUInt16(settings.areaPort); //Port

            res3.WriteFloat(client.character.x);
            res3.WriteFloat(client.character.y);
            res3.WriteFloat(client.character.z);
            res3.WriteByte(client.character.heading);
            router.Send(client, (ushort) MsgPacketId.recv_chara_select_r, res3, ServerType.Msg);

            /*
             ERR_CHARSELECT	GENERIC	Failed to select a character (CODE:<errcode>)
             ERR_CHARSELECT	-8	Maintenance
             ERR_CHARSELECT	-13	You have selected an illegal character
            */


            //Logic to support your dead body //Make this static.  need a predictable deadbody instance ID to support disconnect/reconnet
            DeadBody deadBody = new DeadBody();
            deadBody.id = character.id;
            server.instances.AssignInstance(deadBody);
            character.deadBodyInstanceId = deadBody.instanceId;
            deadBody.characterInstanceId = character.instanceId;
            _Logger.Debug($"Dead Body Instance ID {deadBody.instanceId}   |  Character Instance ID {character.instanceId}");

        }
    }
}
