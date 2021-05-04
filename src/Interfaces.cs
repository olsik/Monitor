namespace Monitor
{
    public interface IDataSource
    {
        string Name { get; }
        int[] Data { get; }
        //const int _DataLength = 60;
        void SetParams(int dataLength, int timePeriod);
    }
}