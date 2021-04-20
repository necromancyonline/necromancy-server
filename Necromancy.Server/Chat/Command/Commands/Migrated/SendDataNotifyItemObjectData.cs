using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //spawns a green soul material item
    public class SendDataNotifyItemObjectData : ServerChatCommand
    {
        public SendDataNotifyItemObjectData(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "itemobject";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            //SendDataNotifyItemObjectData
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(251001); //sys_msg.csv call
            res.WriteFloat(client.character.x); //Initial X
            res.WriteFloat(client.character.y); //Initial Y
            res.WriteFloat(client.character.z); //Initial Z

            res.WriteFloat(client.character.x); //Final X
            res.WriteFloat(client.character.y); //Final Y
            res.WriteFloat(client.character.z); //Final Z
            res.WriteByte(client.character.heading); //View offset

            res.WriteInt32(0); // 0 here gives an indication (blue pillar thing) and makes it pickup-able
            res.WriteInt32(0);
            res.WriteInt32(0);

            res.WriteInt32(
                255); //Anim: 1 = fly from the source. (I think it has to do with odd numbers, some cause it to not be pickup-able)

            res.WriteInt32(0);

            router.Send(client.map, (ushort)AreaPacketId.recv_data_notify_itemobject_data, res, ServerType.Area);
        }
    }
}
