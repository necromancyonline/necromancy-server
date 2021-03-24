using System;

namespace Necromancy.Server.Systems.Item
{
    [Flags]
    public enum ItemQualities
    {
        None        = 1 << 0,
        Poor        = 1 << 1,
        Normal      = 1 << 2,
        Good        = 1 << 3,
        Master      = 1 << 4,
        Legend      = 1 << 5,
        Artifact    = 1 << 6,
        All = Poor & Normal & Good & Master & Legend & Artifact
    }
}
