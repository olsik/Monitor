using System;

namespace Monitor
{
    public class DataSource : IDataSource
    {
        private int[] data = new int[IDataSource.DataLength];
        private Random rnd = new Random();
        // public  DataSource()
        // {

        // }
        public string Name { get { return "test"; } }
        public int[] Data
        {
            get
            {
                for (int i = IDataSource.DataLength - 1; i > 0; i--)
                    data[i] = data[i - 1];
                data[0] = rnd.Next(0, 100);
                return data;
            }
        }
    }
}
