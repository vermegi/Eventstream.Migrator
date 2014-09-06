using System.Collections.Generic;
using Eventstream.Migrator.Tests.Utilities;
using Moq;
using Xunit;

namespace Eventstream.Migrator.Tests.Given_an_EventstreamMigrator
{
    public class When_I_migrate_an_eventstream_with_multiple_migrations : AAATest
    {
        private EventStreamMigrator _sut;
        private Mock<IReadAnEventstream> _eventstreamreader;
        private Mock<IGetEventstreamMigrations> _migrationgetter;
        private Mock<IWriteAnEventstream> _eventstreamwriter;
        private Mock<IMigrate> _firstMigration;
        private Mock<IMigrate> _secondMigration;
        private SomeEventBase _someEvent;
        private readonly SomeEventBase _migratedEvent1 = new SomeEventBase();
        private readonly SomeEventBase _migratedEvent2 = new SomeEventBase();
        private readonly SomeEventBase _migratedEvent3 = new SomeEventBase();
        private readonly SomeEventBase _migratedEvent4 = new SomeEventBase();
        private readonly SomeEventBase _migratedEvent5 = new SomeEventBase();
        private readonly SomeEventBase _migratedEvent6 = new SomeEventBase();

        public override void Arrange()
        {
            var readcounter = 0;
            _someEvent = new SomeEventBase();
            _eventstreamreader = new Mock<IReadAnEventstream>();
            _eventstreamreader.Setup(reader => reader.Read())
                .Returns(() =>
                {
                    readcounter++;
                    return readcounter <= 1;
                });
            _eventstreamreader.Setup(reader => reader.Get<SomeEventBase>())
                .Returns(_someEvent);

            var migratedEventsOfSomeEvent = new List<SomeEventBase>
            {
                _migratedEvent1,
                _migratedEvent2
            };
            var migratedEventsOfMigratedEvent1 = new List<SomeEventBase>
            {
                _migratedEvent3,
                _migratedEvent4
            };
            var migratedEventsOfMigratedEvent2 = new List<SomeEventBase>
            {
                _migratedEvent5,
                _migratedEvent6
            };
            _firstMigration = new Mock<IMigrate>();
            _firstMigration.Setup(migration => migration.Migrate(_someEvent))
                .Returns(migratedEventsOfSomeEvent);
            _secondMigration = new Mock<IMigrate>();
            _secondMigration.Setup(migration => migration.Migrate(_migratedEvent1))
                .Returns(migratedEventsOfMigratedEvent1);
            _secondMigration.Setup(migration => migration.Migrate(_migratedEvent2))
                .Returns(migratedEventsOfMigratedEvent2);
            var migrations = new List<IMigrate>
            {
                _firstMigration.Object,
                _secondMigration.Object
            };
            _migrationgetter = new Mock<IGetEventstreamMigrations>();
            _migrationgetter.Setup(getter => getter.GetMigrations())
                .Returns(migrations);

            _eventstreamwriter = new Mock<IWriteAnEventstream>();

            _sut = new EventStreamMigrator(_eventstreamreader.Object, _migrationgetter.Object, _eventstreamwriter.Object);
        }

        public override void Act()
        {
            _sut.RunMigrations<SomeEventBase>();
        }

        [Fact]
        public void It_runs_the_first_migration_on_the_original_event()
        {
            _firstMigration.Verify(migration => migration.Migrate(_someEvent));
        }

        [Fact]
        public void It_runs_the_second_migration_on_the_migrated_events_of_the_first_migration()
        {
            _secondMigration.Verify(migration => migration.Migrate(_migratedEvent1));
            _secondMigration.Verify(migration => migration.Migrate(_migratedEvent2));
        }
    }
}