namespace Necromancy.Server.Model.Stats
{
    public class BaseStat
    {
        protected readonly object CurrentLock = new object();
        protected readonly object MaxLock = new object();
        protected uint InstanceId;
        protected bool IsDepleted;
        protected int StatCurrent;
        protected int StatMax;

        public BaseStat(int currVal, int statMaxVal)
        {
            StatCurrent = currVal;
            StatMax = statMaxVal;
            IsDepleted = false;
        }

        public bool depleted
        {
            get => IsDepleted;
            set => IsDepleted = value;
        }

        public int current
        {
            get => StatCurrent;
            protected set
            {
                lock (CurrentLock)
                {
                    StatCurrent = value;
                }
            }
        }

        public int max
        {
            get => StatMax;
            private set
            {
                lock (MaxLock)
                {
                    StatCurrent = value;
                }
            }
        }

        public void SetCurrent(int value)
        {
            lock (CurrentLock)
            {
                StatCurrent = value;
            }
        }

        // Set current to +/- value % of _current/_max
        public int SetCurrent(sbyte value, bool useMax = false)
        {
            lock (CurrentLock)
            {
                if (useMax)
                    StatCurrent += StatMax * (value / 100);
                else
                    StatCurrent += StatCurrent * (value / 100);
            }

            return StatCurrent;
        }

        public int SetMax(int value)
        {
            lock (MaxLock)
            {
                StatMax = value;
            }

            return StatMax;
        }

        public void ToMax()
        {
            StatCurrent = StatMax;
            IsDepleted = false;
        }

        public int Modify(int amount, uint instanceId = 0)
        {
            lock (CurrentLock)
            {
                if (IsDepleted)
                    return StatCurrent;
                StatCurrent += amount;
                if (StatCurrent <= 0)
                {
                    IsDepleted = true;
                    this.InstanceId = instanceId;
                }
            }

            return StatCurrent;
        }
    }
}
