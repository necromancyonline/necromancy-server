using System;

namespace Necromancy.Server.Model
{
    public class Soul
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public byte Level { get; set; }
        public DateTime Created { get; set; }
        public long WarehouseGold { get; set; }

        public Soul()
        {
            Id = -1;
            AccountId = -1;
            Level = 0;
            Password = null;
            Name = null;
            Created = DateTime.Now;
            WarehouseGold = 10203040;
            Name = "MissingSoul";
        }
    }
}
