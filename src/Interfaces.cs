namespace Monitor
{
    public interface IDataSource
    {
        string Name { get; }
        int[] Data { get; }
        const int DataLength = 100;
    }
}