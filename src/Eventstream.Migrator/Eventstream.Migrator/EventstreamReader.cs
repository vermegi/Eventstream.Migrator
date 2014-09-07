namespace Eventstream.Migrator
{
    public class EventstreamReader : IReadAnEventstream
    {
        public EventstreamReader(IReadData datareader)
        {
            
        }

        public bool Read()
        {
            return false;
        }

        public TEvent Get<TEvent>()
        {
            return default(TEvent);
        }
    }
}