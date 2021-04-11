using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Area
{    public class SendSoulDispitemRequestData : ClientHandler
    {
        private NecServer _server;
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendSoulDispitemRequestData));
        public SendSoulDispitemRequestData(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort id => (ushort)AreaPacketId.send_soul_dispitem_request_data;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort)AreaPacketId.recv_soul_dispitem_request_data_r, res, ServerType.Area);



            //ToDo  find a better home for these functionalities . This send is the last stop before initial map entry.
            LoadInventory(client, _server);
            //LoadCloakRoom(client);
            //LoadBattleStats(client);
            LoadHonor(client);
            //LoadSoulDispItem(client);
            LoadSoulState(client);
        }

        public void LoadSoulDispItem(NecClient client)
        {
            //notify you of the soul item you got based on something above.
            IBuffer res19 = BufferProvider.Provide();
            res19.WriteInt32(Util.GetRandomNumber(62000001, 62000015)); //soul_dispitem.csv
            router.Send(client, (ushort)AreaPacketId.recv_soul_dispitem_notify_data, res19, ServerType.Area);
        }

        public void LoadSoulState (NecClient client)
        {
            if (client.character.hp.current <=0)
            {
                client.character.state |= Model.CharacterModel.CharacterState.SoulForm;
                client.character.hasDied = true;
            }
            if (client.character.hp.current < -1) client.character.state |= Model.CharacterModel.CharacterState.LostState;
            if ((int)client.account.state == 100) client.character.state |= Model.CharacterModel.CharacterState.GameMaster;

        }

        public void LoadHonor(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1); // must be under 0x3E8 // DBLoad your honor titles, and dump them here. 1000 at a time
            for (int i = 0; i < 1; i++)
            {
                res.WriteInt32(10010101);/*novice monster hunter*/
                res.WriteUInt32(client.character.instanceId);
                res.WriteByte(1); // bool	New Title 0:Yes  1:No
            }
            router.Send(client, (ushort)AreaPacketId.recv_get_honor_notify, res, ServerType.Area);
        }
        //Moved to Map entry to fix null character issue
        public void LoadBattleStats(NecClient client)
        {
        }

        public void LoadInventory(NecClient client, NecServer server)
        {
            ItemService itemService = new ItemService(client.character);
            List<ItemInstance> ownedItems = itemService.LoadOwneditemInstances(server);
            foreach (ItemInstance itemInstance in ownedItems)
            {
                if (itemInstance.statuses.HasFlag(ItemStatuses.Unidentified))
                {
                    RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(client, itemInstance);
                    router.Send(client, recvItemInstanceUnidentified.ToPacket());
                    _Logger.Debug($" Unidentified item : {itemInstance.location.zoneType}");
                }
                else
                {
                    RecvItemInstance recvItemInstance = new RecvItemInstance(client, itemInstance);
                    router.Send(client, recvItemInstance.ToPacket());
                }

            }
        }
        public void LoadCloakRoom(NecClient client)
        {
            //populate soul inventory from database.

        }
    }
}
