using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    ///     Quick item test commands.
    /// </summary>
    public class ItemCommand : ServerChatCommand
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(ItemCommand));

        public ItemCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "itm";
        public override string helpText => "usage: `/itm [itemId] (optional)u`";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command.Length < 1)
            {
                responses.Add(ChatResponse.CommandError(client, "To few arguments"));
                return;
            }

            if (!int.TryParse(command[0], out int itemId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }

            if (client.character == null)
            {
                responses.Add(ChatResponse.CommandError(client, "Character is null"));
                return;
            }

            if (!server.settingRepository.itemInfo.ContainsKey(itemId))
            {
                responses.Add(ChatResponse.CommandError(client, $"ItemId: '{itemId}' does not exist"));
                return;
            }


            ItemSpawnParams spawmParam = new ItemSpawnParams();
            spawmParam.itemStatuses = ItemStatuses.Identified;
            if (command.Length > 1 && command[1] == "u") spawmParam.itemStatuses = ItemStatuses.Unidentified;

            ItemService itemService = new ItemService(client.character);
            ItemInstance itemInstance = itemService.SpawnItemInstance(ItemZoneType.AdventureBag, itemId, spawmParam);
            byte itemZoneOverride = 0;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(2);
            router.Send(client, (ushort)AreaPacketId.recv_situation_start, res, ServerType.Area);

            if (command.Length > 1 && command[1] == "u")
            {
                if (command.Length > 2 && command[2] != "")
                    itemZoneOverride = byte.Parse(command[2]);
                else
                    itemZoneOverride = (byte)itemInstance.location.zoneType;
                _Logger.Debug(itemInstance.type.ToString());
                RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(client, itemInstance);
                router.Send(client, recvItemInstanceUnidentified.ToPacket());
            }
            else
            {
                _Logger.Debug(itemInstance.type.ToString());
                RecvItemInstance recvItemInstance = new RecvItemInstance(client, itemInstance);
                router.Send(client, recvItemInstance.ToPacket());
            }

            res = BufferProvider.Provide();
            router.Send(client, (ushort)AreaPacketId.recv_situation_end, res, ServerType.Area);
        }
    }
}
