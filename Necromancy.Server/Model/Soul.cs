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
        public ulong ExperienceCurrent { get; set; }
        public ulong WarehouseGold { get; set; }
        public int PointsLawful { get; set; }
        public int PointsNeutral { get; set; }
        public int PointsChaos { get; set; }
        public byte CriminalLevel { get; set; }
        public int PointsCurrent { get; set; }
        public int MaterialLife { get; set; }
        public int MaterialReincarnation { get; set; }
        public int MaterialLawful { get; set; }
        public int MaterialChaos { get; set; }
        public uint AlignmentId { get; set; }


        public Soul()
        {
            Id = -1;
            AccountId = -1;
            Level = 0;
            Password = null;
            Name = null;
            Created = DateTime.Now;
            ExperienceCurrent = 2001002;
            WarehouseGold = 10203040;
            Name = "MissingSoul";
            PointsLawful = 0;
            PointsNeutral = 0;
            PointsChaos = 0;
            CriminalLevel = 0;
            PointsCurrent = 0;
            MaterialLife = 0;
            MaterialReincarnation = 0;
            MaterialLawful = 0;
            MaterialChaos = 0;
            AlignmentId = 0;
        }

    }
}
