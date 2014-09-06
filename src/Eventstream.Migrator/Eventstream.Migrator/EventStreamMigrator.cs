using System.Collections.Generic;
using System.Linq;

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

                var inputQueueForMigration = new Queue<TEvent>();
                inputQueueForMigration.Enqueue(anEvent);

                foreach (var migration in migrations)
                {
                    var outputQueueOfMigration = new Queue<TEvent>();
                    while (inputQueueForMigration.Count > 0)
                    {
                        var eventToProcess = inputQueueForMigration.Dequeue();

                        var migratedEvents = migration.Migrate(eventToProcess);

                        foreach (var migratedEvent in migratedEvents)
                        {
                            outputQueueOfMigration.Enqueue(migratedEvent);
                        }
                    }
                    inputQueueForMigration = outputQueueOfMigration;
                }

                foreach (var migratedEvent in inputQueueForMigration)
                {
                    _eventWriter.Save(migratedEvent);
                }
            }
        }
    }
}