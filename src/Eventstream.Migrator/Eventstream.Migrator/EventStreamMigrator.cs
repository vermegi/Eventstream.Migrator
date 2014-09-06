using System.Collections.Generic;

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
            var migrations = _migrationgetter.GetMigrations();
            while (_eventReader.Read())
            {
                var anEvent = _eventReader.Get<TEvent>();

                IEnumerable<TEvent> migratedEvents = new List<TEvent>{anEvent};

                foreach (var migration in migrations)
                {
                    foreach (var migratedEvent in migratedEvents)
                    {
                        migratedEvents = migration.Migrate(anEvent);                        
                    }

                    foreach (var migratedEvent in migratedEvents)
                    {
                        _eventWriter.Save(migratedEvent);
                    }
                }
            }
        }
    }
}