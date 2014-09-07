namespace Eventstream.Migrator
{
    public interface IReadData
    {
        bool Read();
        object GetData();
    }
}