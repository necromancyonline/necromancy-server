using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Model.Stats
{
    public class BaseStat
    {
        protected readonly object currentLock = new object();
        protected readonly object maxLock = new object();
        protected int statMax;
        protected int statCurrent;
        protected uint instanceId;
        protected bool isDepleted;
        public BaseStat(int currVal, int statMaxVal)
        {
            statCurrent = currVal;
            statMax = statMaxVal;
            isDepleted = false;
        }

        public bool depleted
        {
            get => isDepleted;
            set
            {
                isDepleted = value;
            }
        }

        public void SetCurrent(int value)
        {
            lock (currentLock)
            {
                statCurrent = value;
            }
        }

        // Set current to +/- value % of _current/_max
        public int SetCurrent(sbyte value, bool useMax = false)
        {
            lock (currentLock)
            {
                if (useMax)
                    statCurrent += (statMax * (value / 100));
                else
                    statCurrent += (statCurrent * (value / 100));
            }
            return statCurrent;
        }
        public int SetMax(int value)
        {
            lock (maxLock)
            {
                statMax = value;
            }
            return statMax;
        }
        public void ToMax()
        {
            statCurrent = statMax;
            isDepleted = false;
        }
        public int current
        {
            get => statCurrent;
            protected set
            {
                lock (currentLock)
                {
                    statCurrent = value;
                }
            }
        }
        public int max
        {
            get => statMax;
            private set
            {
                lock (maxLock)
                {
                    statCurrent = value;
                }
            }
        }
        public int Modify(int amount, uint instanceId = 0)
        {
            lock (currentLock)
            {
                if (isDepleted)
                    return statCurrent;
                statCurrent += amount;
                if (statCurrent <= 0)
                {
                    isDepleted = true;
                    this.instanceId = instanceId;
                }
            }
            return statCurrent;
        }
    }
}
