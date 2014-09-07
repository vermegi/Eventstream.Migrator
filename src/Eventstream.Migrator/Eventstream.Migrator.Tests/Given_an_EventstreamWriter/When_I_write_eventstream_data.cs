using Eventstream.Migrator.Tests.Given_an_EventstreamMigrator;
using Eventstream.Migrator.Tests.Utilities;
using Eventstream.Migrator.Utilities;
using Eventstream.Migrator.Writing;
using Moq;
using Xunit;

namespace Eventstream.Migrator.Tests.Given_an_EventstreamWriter
{
    public class When_I_write_eventstream_data : AAATest
    {
        private EventstreamWriter _sut;
        private SomeEventBase _anEvent;
        private Mock<ISerialize> _serializer;
        private Mock<IWriteData> _datawriter;
        private object _serializedEvent;

        public override void Arrange()
        {
            _anEvent = new SomeEventBase();

            _serializedEvent = new object();
            _serializer = new Mock<ISerialize>();
            _serializer.Setup(serializer => serializer.Serialize(_anEvent))
                .Returns(_serializedEvent);

            _datawriter = new Mock<IWriteData>();

            _sut = new EventstreamWriter(_serializer.Object, _datawriter.Object);
        }

        public override void Act()
        {
            _sut.Save(_anEvent);
        }

        [Fact]
        public void It_asks_the_serializer_to_serialize_the_event()
        {
            _serializer.Verify(serializer => serializer.Serialize(_anEvent));
        }

        [Fact]
        public void It_tells_the_datawriter_to_save_the_serialized_event()
        {
            _datawriter.Verify(writer => writer.Write(_serializedEvent));
        }
    }
}