using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Arrowgene.Logging;
using Necromancy.Server.Logging;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Moves character location to another instance ID's location.
    /// </summary>
    public class TeleportToCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(TeleportToCommand));
        public TeleportToCommand(NecServer server) : base(server)
        {
        }

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
                IInstance instance = Server.Instances.GetInstance(x);
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
                        $"Please provide a character/npc/gimmick/map transition instance id"));
                    return;
                }
            }

            IBuffer res = BufferProvider.Provide();
            if (character2 != null)
            {
                res.WriteUInt32(client.Character.InstanceId);
                res.WriteFloat(character2.X);
                res.WriteFloat(character2.Y);
                res.WriteFloat(character2.Z);
                res.WriteByte(client.Character.Heading);
                res.WriteByte(client.Character.movementAnim);
            }
            else if (npc2 != null)
            {
                res.WriteUInt32(client.Character.InstanceId);
                res.WriteFloat(npc2.X);
                res.WriteFloat(npc2.Y);
                res.WriteFloat(npc2.Z);
                res.WriteByte(client.Character.Heading);
                res.WriteByte(client.Character.movementAnim);
            }
            else if (gimmick2 != null)
            {
                res.WriteUInt32(client.Character.InstanceId);
                res.WriteFloat(gimmick2.X);
                res.WriteFloat(gimmick2.Y);
                res.WriteFloat(gimmick2.Z);
                res.WriteByte(client.Character.Heading);
                res.WriteByte(client.Character.movementAnim);
            }
            else if (monsterSpawn2 != null)
            {
                res.WriteUInt32(client.Character.InstanceId);
                res.WriteFloat(monsterSpawn2.X);
                res.WriteFloat(monsterSpawn2.Y);
                res.WriteFloat(monsterSpawn2.Z);
                res.WriteByte(client.Character.Heading);
                res.WriteByte(client.Character.movementAnim);
            }
            else if (mapTran2 != null)
            {
                res.WriteUInt32(client.Character.InstanceId);
                res.WriteFloat(mapTran2.ReferencePos.X);
                res.WriteFloat(mapTran2.ReferencePos.Y);
                res.WriteFloat(mapTran2.ReferencePos.Z);
                res.WriteByte(client.Character.Heading);
                res.WriteByte(client.Character.movementAnim);
            }

            Router.Send(client.Map, (ushort) AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "tpto";
        public override string HelpText => "usage: `/tpto [instance id]` - Moves character to [instance id]'s location";
    }
}
