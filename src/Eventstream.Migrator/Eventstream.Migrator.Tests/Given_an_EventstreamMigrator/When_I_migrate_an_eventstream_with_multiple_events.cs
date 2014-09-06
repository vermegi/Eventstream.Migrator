using System.Collections.Generic;
using Eventstream.Migrator.Tests.Utilities;
using Moq;
using Xunit;

namespace Eventstream.Migrator.Tests.Given_an_EventstreamMigrator
{
    public class When_I_migrate_an_eventstream_with_multiple_events : AAATest
    {
        private Mock<IReadAnEventstream> _eventstreamreader;
        private Mock<IGetEventstreamMigrations> _migrationgetter;
        private Mock<IWriteAnEventstream> _eventstreamwriter;
        private EventStreamMigrator _sut;
        private Mock<IMigrate> _mockMigration;
        private SomeEventBase _someEvent;
        private SomeEventBase _migratedEvent;

        public override void Arrange()
        {
            var readcounter = 0;
            _eventstreamreader = new Mock<IReadAnEventstream>();
            _eventstreamreader.Setup(reader => reader.Read())
                .Returns(() =>
                {
                    readcounter++;
                    return readcounter <= 4;

                });

            _mockMigration = new Mock<IMigrate>();
            var migratedEvents = new List<SomeEventBase>
            {
                _migratedEvent
            };
            _mockMigration.Setup(migration => migration.Migrate(_someEvent))
                .Returns(migratedEvents);
            var migrates = new List<IMigrate>
            {
                _mockMigration.Object
            };
            _migrationgetter = new Mock<IGetEventstreamMigrations>();
            _migrationgetter.Setup(getter => getter.GetMigrations())
                .Returns(migrates);
            _eventstreamwriter = new Mock<IWriteAnEventstream>();

            _sut = new EventStreamMigrator(_eventstreamreader.Object, _migrationgetter.Object, _eventstreamwriter.Object);
        }

        public override void Act()
        {
            _sut.RunMigrations<SomeEventBase>();
        }

        [Fact]
        public void It_tells_the_eventstreamreader_to_read_while_there_are_events()
        {
            _eventstreamreader.Verify(reader => reader.Read(), Times.Exactly(5));
        }

        [Fact]
        public void It_tells_the_eventstreamreader_to_get_the_next_event_while_there_are_events()
        {
            _eventstreamreader.Verify(reader => reader.Get<SomeEventBase>(), Times.Exactly(4));
        }

        [Fact]
        public void It_asks_the_migrationgetter_for_the_migration()
        {
            _migrationgetter.Verify(getter => getter.GetMigrations(), Times.Once);
        }

        [Fact]
        public void It_applies_each_migration_to_the_eventstream()
        {
            _mockMigration.Verify(migration => migration.Migrate(_someEvent), Times.Exactly(4));
        }

        [Fact]
        public void It_saves_4_events_to_the_eventstreamwriter()
        {
            _eventstreamwriter.Verify(writer => writer.Save(_migratedEvent), Times.Exactly(4));
        }
    }
}