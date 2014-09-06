using Eventstream.Migrator.Tests.Utilities;
using Moq;
using Xunit;

namespace Eventstream.Migrator.Tests.Given_an_EventstreamMigrator
{
    public class When_I_migrate_an_eventstream_and_there_are_no_migrations : AAATest
    {
        private Mock<IWriteAnEventstream> _eventstreamwriter;
        private Mock<IGetEventstreamMigrations> _migrationgetter;
        private Mock<IReadAnEventstream> _eventstreamreader;
        private EventStreamMigrator _sut;

        public override void Arrange()
        {
            _eventstreamwriter = new Mock<IWriteAnEventstream>();
            _migrationgetter = new Mock<IGetEventstreamMigrations>();
            _eventstreamreader = new Mock<IReadAnEventstream>();

            _sut = new EventStreamMigrator(_eventstreamreader.Object,
                _migrationgetter.Object, _eventstreamwriter.Object);
        }

        public override void Act()
        {
            _sut.RunMigrations<SomeEventBase>();
        }

        [Fact]
        public void It_gets_the_migrations()
        {
            _migrationgetter.Verify(getter => getter.GetMigrations());
        }

        [Fact]
        public void It_never_reads_any_events_from_the_eventstream()
        {
            _eventstreamreader.Verify(rdr => rdr.Read(), Times.Never);
            _eventstreamreader.Verify(rdr => rdr.Get<SomeEventBase>(), Times.Never);
        }

        [Fact]
        public void It_never_saves_any_migrations_to_the_eventstream()
        {
            _eventstreamwriter.Verify(writer => writer.Save(It.IsAny<SomeEventBase>()), Times.Never);
        }
    }
}