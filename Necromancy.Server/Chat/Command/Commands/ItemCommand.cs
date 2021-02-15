using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Quick item test commands.
    /// </summary>
    public class ItemCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(ItemCommand));

        public ItemCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "itm";
        public override string HelpText => "usage: `/itm [itemId] (optional)u`";

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
            
            if (client.Character == null)
            {
                responses.Add(ChatResponse.CommandError(client, "Character is null"));
                return;
            }

            if (!Server.SettingRepository.ItemInfo.ContainsKey(itemId))
            {
                responses.Add(ChatResponse.CommandError(client, $"ItemId: '{itemId}' does not exist"));
                return;
            }

            //bool IsIdentified = true;
            //if (command.Length > 1 && command[1] == "u")
            //{
            //    IsIdentified = false;
            //}

            int[] itemIds = new int[] { itemId };
            ItemSpawnParams[] spawmParams = new ItemSpawnParams[itemIds.Length];
            for (int i = 0; i < itemIds.Length; i++)
            {
                spawmParams[i] = new ItemSpawnParams();
                spawmParams[i].ItemStatuses = ItemStatuses.Identified;
            }
            ItemService itemService = new ItemService(client.Character);
            List<ItemInstance> items = itemService.SpawnItemInstances(ItemZoneType.AdventureBag, itemIds, spawmParams);
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(2);
            Router.Send(client, (ushort)AreaPacketId.recv_situation_start, res, ServerType.Area);
            foreach (ItemInstance itemInstance in items)
            {
                Logger.Debug(itemInstance.Type.ToString());
                RecvItemInstance recvItemInstance = new RecvItemInstance(client, itemInstance);
                Router.Send(client, recvItemInstance.ToPacket());
            }
            res = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_situation_end, res, ServerType.Area);
        }        
    }
}
