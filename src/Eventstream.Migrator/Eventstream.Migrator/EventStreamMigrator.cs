namespace Eventstream.Migrator
{
    public class EventStreamMigrator
    {
        private readonly IReadAnEventstream _eventReader;
        private readonly IGetEventstreamMigrations _migrationgetter;
        private readonly IWriteAnEventstream _eventWriter;

        public EventStreamMigrator(IReadAnEventstream eventReader, IGetEventstreamMigrations migrationgetter, IWriteAnEventstream eventWriter)
        {
            _eventReader = eventReader;
            _migrationgetter = migrationgetter;
            _eventWriter = eventWriter;
        }

        public void RunMigrations<TEvent>()
        {
            var events = _eventReader.Read<TEvent>();
            var migrations = _migrationgetter.GetMigrations();

            foreach (var anEvent in events)
            {
                foreach (var migration in migrations)
                {
                    var migratedEvents = migration.Migrate(anEvent);

                    foreach (var migratedEvent in migratedEvents)
                    {
                        _eventWriter.Save(migratedEvent);
                    }
                }
            }
        }
    }
}