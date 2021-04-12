using System;

namespace Necromancy.Server.Model
{
    public class Soul
    {
        public Soul()
        {
            id = -1;
            accountId = -1;
            level = 0;
            password = null;
            name = null;
            created = DateTime.Now;
            experienceCurrent = 2001002;
            warehouseGold = 10203040;
            name = "MissingSoul";
            pointsLawful = 0;
            pointsNeutral = 0;
            pointsChaos = 0;
            criminalLevel = 0;
            pointsCurrent = 0;
            materialLife = 0;
            materialReincarnation = 0;
            materialLawful = 0;
            materialChaos = 0;
            alignmentId = 0;
        }

        public int id { get; set; }
        public int accountId { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public byte level { get; set; }
        public DateTime created { get; set; }
        public ulong experienceCurrent { get; set; }
        public ulong warehouseGold { get; set; }
        public int pointsLawful { get; set; }
        public int pointsNeutral { get; set; }
        public int pointsChaos { get; set; }
        public byte criminalLevel { get; set; }
        public int pointsCurrent { get; set; }
        public int materialLife { get; set; }
        public int materialReincarnation { get; set; }
        public int materialLawful { get; set; }
        public int materialChaos { get; set; }
        public uint alignmentId { get; set; }

        public void SetSoulAlignment()
        {
            uint alignmentId = 0;
            int maxAlignment = Math.Max(pointsLawful, Math.Max(pointsNeutral, pointsChaos));
            if (maxAlignment == pointsLawful) alignmentId = 1; // (uint)Alignments.Lawful;
            else if (maxAlignment == pointsNeutral) alignmentId = 2; // (uint)Alignments.Neutral;
            else if (maxAlignment == pointsChaos) alignmentId = 3; // (uint)Alignments.Chaotic;
            this.alignmentId = alignmentId;
        }
    }
}
