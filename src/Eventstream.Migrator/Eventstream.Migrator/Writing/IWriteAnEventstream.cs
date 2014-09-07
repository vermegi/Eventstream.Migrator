namespace Eventstream.Migrator.Writing
{
    public interface IWriteAnEventstream
    {
        void Save<TEvent>(TEvent migratedEvent);
    }
}