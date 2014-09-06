namespace Eventstream.Migrator
{
    public class EventStreamMigrator
    {
        private readonly IReadAnEventstream _reader;
        private readonly IGetEventstreamMigrations _migrationgetter;

        public EventStreamMigrator(IReadAnEventstream reader, IGetEventstreamMigrations migrationgetter)
        {
            _reader = reader;
            _migrationgetter = migrationgetter;
        }

        public void RunMigrations()
        {
            _reader.Read();
            _migrationgetter.GetMigrations();
        }
    }
}