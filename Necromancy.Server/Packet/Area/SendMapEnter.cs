using System;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendMapEnter : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendMapEnter));

        public SendMapEnter(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_map_enter;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //error check. must be 0
            res.WriteByte(0); //Bool - play cutscene. 1 yes, 0 no?  //to-do,  play a cutscene on first time map entry
            router.Send(client, (ushort)AreaPacketId.recv_map_enter_r, res, ServerType.Area);

            if (client.map.deadBodies.ContainsKey(client.character.deadBodyInstanceId))
            {
                _Logger.Debug($"found dead body of {client.character.name} already on map.  Hope you didn't lose your loot!");
                DeadBody deadBody = server.instances.GetInstance(client.character.deadBodyInstanceId) as DeadBody;
                deadBody.connectionState = 1;

                RecvCharaBodyNotifySpirit recvCharaBodyNotifySpirit = new RecvCharaBodyNotifySpirit(client.character.deadBodyInstanceId, (byte)RecvCharaBodyNotifySpirit.ValidSpirit.ConnectedClient);
                router.Send(client.map, recvCharaBodyNotifySpirit.ToPacket());
            }
            else if (client.character.state.HasFlag(CharacterState.SoulForm))
            {
                DeadBody deadBody = server.instances.GetInstance(client.character.deadBodyInstanceId) as DeadBody;
                deadBody.x = client.character.x;
                deadBody.y = client.character.y;
                deadBody.z = client.character.z;
                deadBody.heading = client.character.heading;
                deadBody.beginnerProtection = (byte)client.character.beginnerProtection;
                deadBody.charaName = client.character.name;
                deadBody.soulName = client.soul.name;
                deadBody.equippedItems = client.character.equippedItems;
                deadBody.raceId = client.character.raceId;
                deadBody.sexId = client.character.sexId;
                deadBody.hairId = client.character.hairId;
                deadBody.hairColorId = client.character.hairColorId;
                deadBody.faceId = client.character.faceId;
                deadBody.faceArrangeId = client.character.faceArrangeId;
                deadBody.voiceId = client.character.voiceId;
                deadBody.level = client.character.level;
                deadBody.classId = client.character.classId;
                deadBody.equippedItems = client.character.equippedItems;
                deadBody.connectionState = 1;
                client.map.deadBodies.Add(deadBody.instanceId, deadBody);
                RecvDataNotifyCharaBodyData cBodyData = new RecvDataNotifyCharaBodyData(deadBody);
                if (client.map.id.ToString()[0] != "1"[0]) //Don't Render dead bodies in town.  Town map ids all start with 1
                    router.Send(client.map, cBodyData.ToPacket(), client);
                router.Send(client, cBodyData.ToPacket());
            }

            //Re-do all your stats
            ItemService itemService = new ItemService(client.character);
            router.Send(client, itemService.CalculateBattleStats(client));


            //added delay to prevent crash on map entry.
            Task.Delay(TimeSpan.FromSeconds(10)).ContinueWith
            (t1 =>
                {
                    client.character.ClearStateBit(CharacterState.InvulnerableForm);

                    RecvCharaNotifyStateflag recvCharaNotifyStateflag = new RecvCharaNotifyStateflag(client.character.instanceId, (ulong)client.character.state);
                    router.Send(client.map, recvCharaNotifyStateflag.ToPacket());
                }
            );
        }
    }
}
