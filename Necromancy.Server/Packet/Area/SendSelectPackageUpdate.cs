using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendSelectPackageUpdate : ClientHandler
    {
        public SendSelectPackageUpdate(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_select_package_update;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int objectId = packet.data.ReadInt32();
            int operation = packet.data.ReadInt32();
            int errcode = 0;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(errcode); //Error message Call. 0 for success. see additional options in Sys_msg.csv
            router.Send(client, (ushort)AreaPacketId.recv_select_package_update_r, res, ServerType.Area);


            if (operation == 0x01 || operation == 0x0) //receive message
                ;


            /*
            if (operation == 0x2)//reply to message
            if (operation == 0x03)//delete mail
            if (operation == 0x04)//unprotect message
            */
        }

        private void SendLootAccessObject(NecClient client, int objectId)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(objectId); //ObjectID or error check

            router.Send(client, (ushort)AreaPacketId.recv_loot_access_object_r, res, ServerType.Area);
        }


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
