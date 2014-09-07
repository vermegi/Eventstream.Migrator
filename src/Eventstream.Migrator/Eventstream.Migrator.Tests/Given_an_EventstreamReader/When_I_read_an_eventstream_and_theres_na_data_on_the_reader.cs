using Eventstream.Migrator.Tests.Given_an_EventstreamMigrator;
using Eventstream.Migrator.Tests.Utilities;
using Moq;
using Xunit;

namespace Eventstream.Migrator.Tests.Given_an_EventstreamReader
{
    public class When_I_read_an_eventstream_and_theres_na_data_on_the_reader : AAATest
    {
        private EventstreamReader _sut;
        private Mock<IReadData> _datareader;
        private bool _readresult;
        private SomeEventBase _getresult;

        public override void Arrange()
        {
            _datareader = new Mock<IReadData>();

            _sut = new EventstreamReader(_datareader.Object);
        }

        public override void Act()
        {
            _readresult = _sut.Read();
            _getresult = _sut.Get<SomeEventBase>();
        }

        [Fact]
        public void It_returns_the_result_from_the_reader()
        {
            Assert.False(_readresult);
        }

        [Fact]
        public void A_get_operation_results_in_a_null_value()
        {
            Assert.Null(_getresult);
        }
    }
}