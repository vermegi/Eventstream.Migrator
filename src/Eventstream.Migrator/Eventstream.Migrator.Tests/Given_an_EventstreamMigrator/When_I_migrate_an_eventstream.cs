using Eventstream.Migrator.Tests.Utilities;
using Moq;
using Xunit;

namespace Eventstream.Migrator.Tests.Given_an_EventstreamMigrator
{
    public class When_I_migrate_an_eventstream : AAATest
    {
        private EventStreamMigrator _sut;
        private Mock<IReadAnEventstream> _eventstreamReader;

        public override void Arrange()
        {
            _eventstreamReader = new Mock<IReadAnEventstream>();

            _sut = new EventStreamMigrator(_eventstreamReader.Object);
        }

        public override void Act()
        {
            _sut.RunMigrations();
        }

        [Fact]
        public void It_tells_the_eventstreamreader_to_read_the_eventstream()
        {
            _eventstreamReader.Verify(rdr => rdr.Read());
        }
    }

    public interface IReadAnEventstream
    {
        void Read();
    }

    public class EventStreamMigrator
    {
        private readonly IReadAnEventstream _reader;

        public EventStreamMigrator(IReadAnEventstream reader)
        {
            _reader = reader;
        }

        public void RunMigrations()
        {
            _reader.Read();
        }
    }
}