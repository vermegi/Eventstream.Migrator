namespace Eventstream.Migrator
{
    public class EventStreamMigrator
    {
        private readonly IReadAnEventstream _reader;

        public EventStreamMigrator(IReadAnEventstream reader)
        {
            _reader = reader;
        }

        public void RunMigrations()
        {
            _reader.Read();
        }
    }
}