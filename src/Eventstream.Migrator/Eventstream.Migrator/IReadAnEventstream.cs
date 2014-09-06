namespace Eventstream.Migrator
{
    public interface IReadAnEventstream
    {
        bool Read();
        TEvent Get<TEvent>();
    }
}