using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Chat.Command.Commands
{
    class ItemGeneratorCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(ItemCommand));

        public ItemGeneratorCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "genitems";
        public override string HelpText => "usage: `/genitems [package]`";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command.Length < 1)
            {
                responses.Add(ChatResponse.CommandError(client, "To few arguments"));
                return;
            }

            switch (command[0])
            {
                case "bags":
                    spawnBags(client);
                    break;
                case "a_darkmerchant":
                    spawnAvatarDarkMerchant(client);
                    break;
                case "charmed":
                    SpawnCharmedGear(client);
                    break;
                default:
                    responses.Add(ChatResponse.CommandError(client, $"Invalid Package: {command[0]}"));
                    return;
            }
            
        }

        private void SpawnCharmedGear(NecClient client)
        {
            int[] itemIds = new int[5];
            itemIds[0] = 120308;
            itemIds[1] = 220308;
            itemIds[2] = 320308;
            itemIds[3] = 420308;
            itemIds[4] = 520308;
            SendItems(client, itemIds);
        }

        private void spawnAvatarDarkMerchant(NecClient client)
        {
            int[] itemIds = new int[5];
            itemIds[0] = 162502;
            itemIds[1] = 262502;
            itemIds[2] = 362502;
            itemIds[3] = 462502;
            itemIds[4] = 562502;
            SendItems(client, itemIds);
        }

        private void spawnBags(NecClient client)
        {
            int[] itemIds = new int[18];
            itemIds[0] = 50100501;
            itemIds[1] = 50100502;
            itemIds[2] = 50100503;
            itemIds[3] = 50100504;
            itemIds[4] = 50100505;
            itemIds[5] = 50100506;
            itemIds[6] = 50100507;
            itemIds[7] = 50100511;
            itemIds[8] = 50100512;
            itemIds[9] = 50100513;
            itemIds[10] = 50100521;
            itemIds[11] = 50100599;
            itemIds[12] = 90012001; //royal bag 16 slots
            itemIds[13] = 90012002; //deluxe royal bag - 24 slots??
            itemIds[14] = 90012003;
            itemIds[15] = 90012051;
            itemIds[16] = 90012052;
            itemIds[17] = 90012053;
            SendItems(client, itemIds);
        }

        private void SendItems(NecClient client, int[] itemIds)
        {
            ItemSpawnParams[] spawmParams = new ItemSpawnParams[itemIds.Length];
            for (int i = 0; i < itemIds.Length; i++)
            {
                spawmParams[i] = new ItemSpawnParams();
                spawmParams[i].ItemStatuses = ItemStatuses.Identified;
            }
            ItemService itemService = new ItemService(client.Character);
            List<ItemInstance> items = itemService.SpawnItemInstances(ItemZoneType.AdventureBag, itemIds, spawmParams);

            RecvSituationStart recvSituationStart = new RecvSituationStart(2);
            Router.Send(client, recvSituationStart.ToPacket());

            foreach (ItemInstance itemInstance in items)
            {
                Logger.Debug(itemInstance.Type.ToString());
                RecvItemInstance recvItemInstance = new RecvItemInstance(client, itemInstance);
                Router.Send(client, recvItemInstance.ToPacket());
            }

            RecvSituationEnd recvSituationEnd = new RecvSituationEnd();
            Router.Send(client, recvSituationEnd.ToPacket());
        }
    }
}

