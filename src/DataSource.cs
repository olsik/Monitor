using System;
using System.Timers;

namespace Monitor
{
    public abstract class DataSource : IDataSource
    {
        protected Timer aTimer = new Timer();
        protected int dataLength = 100;
        protected int timePeriod_sec = 100;
        protected int prevDataLength = 100;
        protected int prevTimePeriod_sec = 100;
        protected int[] data = new int[100];

        public abstract string Name { get; }
        public abstract int[] Data { get; }
        protected abstract void CollectData();
        protected abstract void UpdateParams();

        public DataSource()
        {
            CreateTimer();
        }
        public void SetParams(int dataLength, int timePeriod)
        {
            this.dataLength = dataLength;
            this.timePeriod_sec = timePeriod;
            UpdateParams();
        }
        protected void CreateTimer()
        {
            aTimer.Interval = 1000;
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        protected void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            aTimer.Enabled = false;
            CollectData();
            aTimer.Enabled = true;
        }
    }
}