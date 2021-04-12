using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    ///     Moves character location to another instance ID's location.
    /// </summary>
    public class TeleportToCommand : ServerChatCommand
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(TeleportToCommand));

        public TeleportToCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "tpto";
        public override string helpText => "usage: `/tpto [instance id]` - Moves character to [instance id]'s location";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            Character character2 = null;
            NpcSpawn npc2 = null;
            Gimmick gimmick2 = null;
            MapTransition mapTran2 = null;
            MonsterSpawn monsterSpawn2 = null;
            if (uint.TryParse(command[0], out uint x))
            {
                IInstance instance = server.instances.GetInstance(x);
                if (instance is Character character)
                {
                    character2 = character;
                }
                else if (instance is NpcSpawn npc)
                {
                    npc2 = npc;
                }
                else if (instance is Gimmick gimmick)
                {
                    gimmick2 = gimmick;
                }
                else if (instance is MonsterSpawn monsterSpawn)
                {
                    monsterSpawn2 = monsterSpawn;
                }
                else if (instance is MapTransition mapTran)
                {
                    mapTran2 = mapTran;
                }
                else
                {
                    responses.Add(ChatResponse.CommandError(client,
                        "Please provide a character/npc/gimmick/map transition instance id"));
                    return;
                }
            }

            IBuffer res = BufferProvider.Provide();
            if (character2 != null)
            {
                res.WriteUInt32(client.character.instanceId);
                res.WriteFloat(character2.x);
                res.WriteFloat(character2.y);
                res.WriteFloat(character2.z);
                res.WriteByte(client.character.heading);
                res.WriteByte(client.character.movementAnim);
            }
            else if (npc2 != null)
            {
                res.WriteUInt32(client.character.instanceId);
                res.WriteFloat(npc2.x);
                res.WriteFloat(npc2.y);
                res.WriteFloat(npc2.z);
                res.WriteByte(client.character.heading);
                res.WriteByte(client.character.movementAnim);
            }
            else if (gimmick2 != null)
            {
                res.WriteUInt32(client.character.instanceId);
                res.WriteFloat(gimmick2.x);
                res.WriteFloat(gimmick2.y);
                res.WriteFloat(gimmick2.z);
                res.WriteByte(client.character.heading);
                res.WriteByte(client.character.movementAnim);
            }
            else if (monsterSpawn2 != null)
            {
                res.WriteUInt32(client.character.instanceId);
                res.WriteFloat(monsterSpawn2.x);
                res.WriteFloat(monsterSpawn2.y);
                res.WriteFloat(monsterSpawn2.z);
                res.WriteByte(client.character.heading);
                res.WriteByte(client.character.movementAnim);
            }
            else if (mapTran2 != null)
            {
                res.WriteUInt32(client.character.instanceId);
                res.WriteFloat(mapTran2.referencePos.X);
                res.WriteFloat(mapTran2.referencePos.Y);
                res.WriteFloat(mapTran2.referencePos.Z);
                res.WriteByte(client.character.heading);
                res.WriteByte(client.character.movementAnim);
            }

            router.Send(client.map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
        }
    }
}
