using System.Collections.Generic;
using System.Linq;
using Eventstream.Migrator.Reading;
using Eventstream.Migrator.Writing;

namespace Eventstream.Migrator.Migrating
{
    public class EventStreamMigrator
    {
        private readonly IReadAnEventstream _eventReader;
        private readonly IGetEventstreamMigrations _migrationgetter;
        private readonly IWriteAnEventstream _eventWriter;

        public EventStreamMigrator(
            IReadAnEventstream eventReader, 
            IGetEventstreamMigrations migrationgetter, 
            IWriteAnEventstream eventWriter)
        {
            _eventReader = eventReader;
            _migrationgetter = migrationgetter;
            _eventWriter = eventWriter;
        }

        public void RunMigrations<TEvent>()
        {
            var migrations = _migrationgetter.GetMigrations();

            var migrationlist = migrations as IList<IMigrate> ?? migrations.ToList();
            if (migrations == null || !migrationlist.Any())
                return;

            while (_eventReader.Read())
            {
                var anEvent = _eventReader.Get<TEvent>();

                var inputQueueForMigration = new Queue<TEvent>();
                inputQueueForMigration.Enqueue(anEvent);

                inputQueueForMigration = migrationlist.Aggregate(inputQueueForMigration, ProcessMigration);

                foreach (var migratedEvent in inputQueueForMigration)
                {
                    _eventWriter.Save(migratedEvent);
                }
            }
        }

        private static Queue<TEvent> ProcessMigration<TEvent>(Queue<TEvent> inputQueueForMigration, IMigrate migration)
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

            return outputQueueOfMigration;
        }
    }
}