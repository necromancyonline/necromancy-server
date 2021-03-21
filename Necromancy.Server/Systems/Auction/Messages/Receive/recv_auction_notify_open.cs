using Necromancy.Server.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Auction.Messages.Receive
{
    class recv_auction_notify_open
    {

        public void Execute()
        {

            PacketRouter router = new PacketRouter();
            //router.Send(client, (ushort) AreaPacketId.recv_auction_notify_open, res, ServerType.Area);
        }
    }
}
