using System.Collections.Generic;

namespace Eventstream.Migrator
{
    public interface IGetEventstreamMigrations
    {
        IEnumerable<IMigrate> GetMigrations();
    }
}