using System.Collections.Generic;
using System.Linq;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// gives you a title from honor.csv
    /// </summary>
    public class HonorCommand : ServerChatCommand
    {
        public HonorCommand(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            int x = 10010101;
            if (command[0] == "all")
            {
                //foreach (HonorSetting honorSetting in Server.SettingRepository.Honor.Values)
                int[] honorSettings = Server.SettingRepository.Honor.Keys.ToArray();
                int numEntries = Server.SettingRepository.Honor.Count; //its over 1000!

                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(1000); // must be under 0x3E8 // wtf?
                for (int i = 0; i < 1000; i++)
                {
                    res.WriteInt32(honorSettings[i]);
                    res.WriteUInt32(client.Character.InstanceId);
                    res.WriteByte(1); // bool	New Title 0:Yes  1:No	
                }
                Router.Send(client, (ushort)AreaPacketId.recv_get_honor_notify, res, ServerType.Area);
                res = BufferProvider.Provide();
                res.WriteInt32(numEntries-1000); // must be under 0x3E8 // wtf?
                for (int i = 1000; i < numEntries; i++)
                {
                    res.WriteInt32(honorSettings[i]);
                    res.WriteUInt32(client.Character.InstanceId);
                    res.WriteByte(0); // bool	New Title 0:Yes  1:No	
                }
                Router.Send(client, (ushort)AreaPacketId.recv_get_honor_notify, res, ServerType.Area);
            }
            else if (!int.TryParse(command[0], out x))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}.  try 10010101"));
                return;
            }

            IBuffer  res2 = BufferProvider.Provide();

                res2 = BufferProvider.Provide();
                res2.WriteInt32(x);
                res2.WriteUInt32(client.Character.InstanceId);
                res2.WriteByte(1); // bool		New Title 0:Yes  1:No	
                Router.Send(client, (ushort)AreaPacketId.recv_get_honor, res2, ServerType.Area);

        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "honor";
        public override string HelpText => "usage: `/honor 10010101` - gives you the novice monster hunter title.";
    }
}
/*
 * ToDo,  write a DB table and queries to run this at world select.
 *             int[] titles = new int[] { 10010101, 10010102, 10010103, 10010104 };

            IBuffer res = BufferProvider.Provide(), res2 = BufferProvider.Provide();
            int numEntries = x;
            res.WriteInt32(numEntries); // must be under 0x3E8 // wtf?
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(titles[i]);
                res.WriteUInt32(client.Character.InstanceId);
                res.WriteByte(1); // bool		
            }
            Router.Send(client, (ushort)AreaPacketId.recv_get_honor_notify, res, ServerType.Area);

    */
