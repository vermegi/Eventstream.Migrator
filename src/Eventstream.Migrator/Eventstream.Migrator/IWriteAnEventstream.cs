namespace Eventstream.Migrator
{
    public interface IWriteAnEventstream
    {
        void Save<TEvent>(TEvent migratedEvent);
    }
}