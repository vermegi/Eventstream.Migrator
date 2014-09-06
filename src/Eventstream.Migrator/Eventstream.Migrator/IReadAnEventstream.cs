using System.Collections.Generic;

namespace Eventstream.Migrator
{
    public interface IReadAnEventstream
    {
        IEnumerable<TEvent> Read<TEvent>();
    }
}