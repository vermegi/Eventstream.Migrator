using Eventstream.Migrator.Utilities;

namespace Eventstream.Migrator.Writing
{
    public class EventstreamWriter : IWriteAnEventstream
    {
        private readonly ISerialize _serializer;
        private readonly IWriteData _datawriter;

        public EventstreamWriter(ISerialize serializer, IWriteData datawriter)
        {
            _serializer = serializer;
            _datawriter = datawriter;
        }

        public void Save<TEvent>(TEvent anEvent)
        {
            var serializedEvent = _serializer.Serialize(anEvent);
            _datawriter.Write(serializedEvent);
        }
    }
}