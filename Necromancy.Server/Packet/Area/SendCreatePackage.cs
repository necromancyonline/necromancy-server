using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_create_package : ClientHandler
    {
        public send_create_package(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_create_package;

        public override void Handle(NecClient client, NecPacket packet)
        {
            string recipient = packet.Data.ReadCString();
            string title = packet.Data.ReadCString();
            string content = packet.Data.ReadCString();
            int unknownInt = packet.Data.ReadInt32();
            byte itemCount = packet.Data.ReadByte();
            long money = packet.Data.ReadInt64();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//Failed to send message error if not 0
            Router.Send(client, (ushort) AreaPacketId.recv_create_package_r, res, ServerType.Area);

            SendPackageNotifyAdd(client, recipient, title, content, unknownInt, itemCount,  money);
        }
        private void SendPackageNotifyAdd(NecClient client, string recipient, string title, string content,
                                         int unknownInt, byte itemCount,  long money)
        {
            NecClient RecipientClient = Server.Clients.GetBySoulName(recipient);
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);//Failed to send message error if not 0
            res.WriteInt32(Util.GetRandomNumber(900,921));//Object ID of mail package item
            res.WriteFixedString(client.Soul.Name, 0x31);//Soul name of sender
            res.WriteFixedString(client.Character.Name, 0x5B);//Character name but of what?
            res.WriteFixedString($"{title}", 0x5B);//Title
            res.WriteFixedString($"{content}", 0x259);//Content
            res.WriteInt32(Util.GetRandomNumber(0,10));
            res.WriteInt16(1);//Mail package state. 1=un-read 3 = received //This number needs to be odd otherwise it causes a "colored" mail and causes inf loop of send_select_package_update
            res.WriteInt64(10200101); //item instance id
            res.WriteInt32(10200101);//Responsible for icon
            res.WriteFixedString("help", 0x49);//
            res.WriteFixedString($"me ", 0x49);//Item Title
            res.WriteInt32(1);//Odd numbers here make the item have the title and correct icon, 1/3 = not broken, -1/5 = broken 
            res.WriteInt32(Util.GetRandomNumber(0, 10));
            res.WriteInt32(Util.GetRandomNumber(0, 10));
            res.WriteInt32(Util.GetRandomNumber(0, 10));
            res.WriteFixedString("pls", 0x10);
            res.WriteByte(itemCount);//Number of items
            res.WriteInt32(Util.GetRandomNumber(0, 10));
            res.WriteInt32(Util.GetRandomNumber(0, 10));

            res.WriteByte(1);//bool
            res.WriteInt32(1);
            res.WriteInt32(Util.GetRandomNumber(0, 10));
            res.WriteInt32(Util.GetRandomNumber(0, 10));

            res.WriteByte(1);//bool
            res.WriteInt32(2);
            res.WriteInt32(Util.GetRandomNumber(0, 10));
            res.WriteInt32(Util.GetRandomNumber(0, 10));

            res.WriteByte(1);//bool
            res.WriteInt32(3);
            res.WriteInt32(Util.GetRandomNumber(0, 10));
            res.WriteInt32(Util.GetRandomNumber(0, 10));

            res.WriteInt64(money);//Transfered money   
            

            Router.Send(RecipientClient, (ushort)AreaPacketId.recv_package_notify_add, res, ServerType.Area);
            /*
            MAIL	-2400	Recipient inbox is full
            MAIL	-2401	You may not send parcels outside of a town
            MAIL	-2402	You may not send items in Soul form
            MAIL	-2403	You may not delete unopened mails or items
            MAIL	-2404	You may not delete unopened mails or items
            MAIL	-2405	You may not delete protected mails
            MAIL	-2406	You may not receive items outside of a town
            MAIL	-1	You may not receive items outside of a town
            MAIL	-2	You may not send blank mails
            MAIL	-20	The Soul Name does not exist
            MAIL	-1606	You do not have enough gold
            MAIL	-3	No recipient specified
            MAIL	-4	You may not send a mail to yourself
            MAIL	-2410	You may not send a mail to yourself
            MAIL	-900	No space available in inventory
            MAIL	-207	No space available in inventory
            MAIL	-553	You have reached the gold cap
            MAIL	-2411	You may not receive anything in Soul form
            MAIL	-2412	You may not send money in Soul form
            MAIL	1	You have unopened mails
            MAIL	2	No mails to delete
            MAIL	3	You have %d unreceived mails. Please check your inbox.
            MAIL	-2414	Mail title includes banned words
            MAIL	-2415	Mail includes banned words
            MAIL	-2417	You received a mail, but your inbox is full. Please clean your inbox.
            MAIL	-3002	Unable to create new parcels during an event
            MAIL	GENERIC	Failed to send mail

            */
        }
    }
}
