using System;

namespace Necromancy.Server.Systems.Item
{
    [Flags]
    public enum ItemQualities
    {
        None = 0 << 0,
        Poor = 1 << 0,
        Normal = 1 << 1,
        Good = 1 << 2,
        Master = 1 << 3,
        Legend = 1 << 4,
        Artifact = 1 << 5,
        Epic = 1 << 6,
        Other = 1 << 7,
        All = Poor + Normal + Good + Epic + Master + Legend + Artifact + Other
    }
}
