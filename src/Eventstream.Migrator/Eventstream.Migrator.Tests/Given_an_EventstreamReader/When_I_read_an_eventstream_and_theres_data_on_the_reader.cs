using Eventstream.Migrator.Tests.Given_an_EventstreamMigrator;
using Eventstream.Migrator.Tests.Utilities;
using Moq;
using Xunit;

namespace Eventstream.Migrator.Tests.Given_an_EventstreamReader
{
    public class When_I_read_an_eventstream_and_theres_data_on_the_reader : AAATest
    {
        private EventstreamReader _sut;
        private Mock<IReadData> _datareader;
        private bool _readresult;
        private SomeEventBase _getresult;
        private Mock<ISerialize> _serializer;
        private object _readData;
        private SomeEventBase _deserializedObject;

        public override void Arrange()
        {
            _readData = new object();
            _datareader = new Mock<IReadData>();
            _datareader.Setup(rdr => rdr.Read())
                .Returns(true);
            _datareader.Setup(rdr => rdr.GetData())
                .Returns(_readData);

            _deserializedObject = new SomeEventBase();
            _serializer = new Mock<ISerialize>();
            _serializer.Setup(serializer => serializer.Deserialize<SomeEventBase>(_readData))
                .Returns(_deserializedObject);

            _sut = new EventstreamReader(_datareader.Object, _serializer.Object);
        }

        public override void Act()
        {
            _readresult = _sut.Read();
            _getresult = _sut.Get<SomeEventBase>();
        }

        [Fact]
        public void It_returns_the_result_from_the_reader()
        {
            Assert.True(_readresult);
        }

        [Fact]
        public void It_gets_the_read_data_from_the_reader()
        {
            _datareader.Verify(rdr => rdr.GetData());
        }

        [Fact]
        public void It_asks_the_serializer_to_deserialize_the_get_result()
        {
            _serializer.Verify(serializer => serializer.Deserialize<SomeEventBase>(_readData));
        }

        [Fact]
        public void It_returns_the_deserialized_result()
        {
            Assert.Equal(_deserializedObject, _getresult);
        }
    }
}