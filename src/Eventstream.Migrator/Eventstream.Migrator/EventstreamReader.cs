namespace Eventstream.Migrator
{
    public class EventstreamReader : IReadAnEventstream
    {
        private readonly IReadData _datareader;
        private readonly ISerialize _serializer;
        private bool _hasreadData;

        public EventstreamReader(IReadData datareader, ISerialize serializer)
        {
            _datareader = datareader;
            _serializer = serializer;
        }

        public bool Read()
        {
            _hasreadData = _datareader.Read();
            return _hasreadData;
        }

        public TEvent Get<TEvent>()
        {
            if (!_hasreadData)
                return default(TEvent);

            var data = _datareader.GetData();
            _serializer.Deserialize<TEvent>(data);
            return default(TEvent);
        }
    }
}