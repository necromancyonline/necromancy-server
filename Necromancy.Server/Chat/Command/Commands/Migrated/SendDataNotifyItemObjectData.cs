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

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            //SendDataNotifyItemObjectData
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(251001); //sys_msg.csv call
            res.WriteFloat(client.Character.X); //Initial X
            res.WriteFloat(client.Character.Y); //Initial Y
            res.WriteFloat(client.Character.Z); //Initial Z

            res.WriteFloat(client.Character.X); //Final X
            res.WriteFloat(client.Character.Y); //Final Y
            res.WriteFloat(client.Character.Z); //Final Z
            res.WriteByte(client.Character.Heading); //View offset

            res.WriteInt32(0); // 0 here gives an indication (blue pillar thing) and makes it pickup-able
            res.WriteInt32(0);
            res.WriteInt32(0);

            res.WriteInt32(
                255); //Anim: 1 = fly from the source. (I think it has to do with odd numbers, some cause it to not be pickup-able)

            res.WriteInt32(0);

            Router.Send(client.Map, (ushort) AreaPacketId.recv_data_notify_itemobject_data, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "itemobject";
    }
}
