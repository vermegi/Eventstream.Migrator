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
            _migrationgetter = new Mock<IGetEventstreamMigrations>();
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
    }
}