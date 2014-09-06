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

        public override void Arrange()
        {
            _eventstreamReader = new Mock<IReadAnEventstream>();
            _migrationgetter = new Mock<IGetEventstreamMigrations>();

            _sut = new EventStreamMigrator(_eventstreamReader.Object, _migrationgetter.Object);
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

        [Fact]
        public void It_asks_the_migrationgetter_for_the_migrations()
        {
            _migrationgetter.Verify(getter => getter.GetMigrations());
        }
    }
}