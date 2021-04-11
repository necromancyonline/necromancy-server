using System;
using System.Collections.Generic;
using System.Text;


namespace Necromancy.Server.Model
{
    public class Auction
    {
        public int typeId { get; set; } // Bid or Rebid
        public int slotsId { get; set; }
        public Int64 unknown { get; set; }
        public int lowest { get; set; }
        public int buyNow { get; set; }
        public string name { get; set; }
        public byte unknown1 { get; set; }
        public string comment { get; set; }
        public Int16 bid { get; set; }
        public int time { get; set; }
        public int bidAmount { get; set; } // Bid Price Or Bid Amount
        public int statuses { get; set; }

        public Auction()
        {
            slotsId = -1;
        }
    }
}
