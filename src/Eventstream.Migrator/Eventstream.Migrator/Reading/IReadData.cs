namespace Eventstream.Migrator.Reading
{
    public interface IReadData
    {
        bool Read();
        object GetData();
    }
}