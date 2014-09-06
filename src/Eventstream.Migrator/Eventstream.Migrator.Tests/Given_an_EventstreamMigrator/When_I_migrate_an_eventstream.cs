using System.Collections.Generic;
using Eventstream.Migrator.Tests.Utilities;
using Moq;
using Xunit;

namespace Eventstream.Migrator.Tests.Given_an_EventstreamMigrator
{
    public class When_I_migrate_an_eventstream : AAATest
    {
        private EventStreamMigrator _sut;
        private Mock<IReadAnEventstream> _eventstreamReader;
        private Mock<IGetEventstreamMigrations> _migrationgetter;
        private Mock<IMigrate> _mockMigration;
        private SomeEventBase _someEvent;
        private Mock<IWriteAnEventstream> _eventstreamWriter;
        private SomeEventBase _migratedEvent;

        public override void Arrange()
        {
            _someEvent = new SomeEventBase();
            var events = new List<SomeEventBase>
            {
                _someEvent
            };
            _eventstreamReader = new Mock<IReadAnEventstream>();
            _eventstreamReader.Setup(reader => reader.Read<SomeEventBase>())
                .Returns(events);

            _mockMigration = new Mock<IMigrate>();
            _migratedEvent = new SomeEventBase();
            var migratedEvents = new List<SomeEventBase>
            {
                _migratedEvent
            };
            _mockMigration.Setup(migration => migration.Migrate(_someEvent))
                .Returns(migratedEvents);
            var migrations = new List<IMigrate>
            {
                _mockMigration.Object
            };
            _migrationgetter = new Mock<IGetEventstreamMigrations>();
            _migrationgetter.Setup(getter => getter.GetMigrations())
                .Returns(migrations);

            _eventstreamWriter = new Mock<IWriteAnEventstream>();

            _sut = new EventStreamMigrator(_eventstreamReader.Object, _migrationgetter.Object, _eventstreamWriter.Object);
        }

        public override void Act()
        {
            _sut.RunMigrations<SomeEventBase>();
        }

        [Fact]
        public void It_tells_the_eventstreamreader_to_read_the_eventstream()
        {
            _eventstreamReader.Verify(rdr => rdr.Read<SomeEventBase>());
        }

        [Fact]
        public void It_asks_the_migrationgetter_for_the_migrations()
        {
            _migrationgetter.Verify(getter => getter.GetMigrations());
        }

        [Fact]
        public void It_applies_each_migration_to_each_event_in_the_eventstream()
        {
            _mockMigration.Verify(migration => migration.Migrate(_someEvent));
        }

        [Fact]
        public void It_tells_the_eventstreamwriter_to_save_the_migrated_event()
        {
            _eventstreamWriter.Verify(writer => writer.Save(_migratedEvent));
        }
    }

    public class SomeEventBase
    {
    }
}