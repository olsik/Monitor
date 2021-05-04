using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace Monitor
{
    public abstract class NetworkDataSource : DataSource
    {
        private long PrevValue = -1;
        private long CurValue;
        private int curInterval = 1000;
        public override int[] Data { get { return data; } }

        protected abstract long GetCurValue(NetworkInterface ni);
        protected override void CollectData()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return;

            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            CurValue = 0;
            foreach (NetworkInterface ni in interfaces)
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;
                CurValue += GetCurValue(ni);
            }

            for (int i = dataLength - 1; i > 0; i--)
                data[i] = data[i - 1];
            data[0] = PrevValue == -1 ? 0 : (int)((CurValue - PrevValue)*(8.0/1024) / (curInterval / 1000.0));
            PrevValue = CurValue;
        }
        protected override void UpdateParams()
        {
            if (dataLength != prevDataLength)
            {
                int[] newData = new int[dataLength];
                for (int i = 0; i < dataLength && i < prevDataLength; i++)
                    newData[i] = data[i];
                data = newData;
            }

            curInterval = 1000 * timePeriod_sec / dataLength;
            int prevInterval = 1000 * prevTimePeriod_sec / prevDataLength;
            if (curInterval != prevInterval)
                aTimer.Interval = curInterval;

            prevDataLength = dataLength;
            prevTimePeriod_sec = timePeriod_sec;
        }
    }
    public class NetworkDownDataSource : NetworkDataSource
    {
        public override string Name { get { return "NetworkDown"; } }
        protected override long GetCurValue(NetworkInterface ni)
        {
            return ni != null ? ni.GetIPv4Statistics().BytesReceived : 0;
        }
    }
    public class NetworkUpDataSource : NetworkDataSource
    {
        public override string Name { get { return "NetworkUp"; } }
        protected override long GetCurValue(NetworkInterface ni)
        {
            return ni != null ? ni.GetIPv4Statistics().BytesSent : 0;
        }
    }
}