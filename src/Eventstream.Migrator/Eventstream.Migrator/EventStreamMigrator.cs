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

        public void RunMigrations<TEvent>()
        {
            var events = _reader.Read<TEvent>();
            var migrations = _migrationgetter.GetMigrations();

            foreach (var anEvent in events)
            {
                foreach (var migration in migrations)
                {
                    migration.Migrate(anEvent);
                }
            }
        }
    }
}