namespace Eventstream.Migrator.Reading
{
    public interface IReadAnEventstream
    {
        bool Read();
        TEvent Get<TEvent>();
    }
}