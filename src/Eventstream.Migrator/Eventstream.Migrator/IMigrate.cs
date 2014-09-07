using System.Collections.Generic;

namespace Eventstream.Migrator
{
    public interface IMigrate
    {
        IEnumerable<TEvent> Migrate<TEvent>(TEvent someEvent);
        int Order { get; }
    }
}