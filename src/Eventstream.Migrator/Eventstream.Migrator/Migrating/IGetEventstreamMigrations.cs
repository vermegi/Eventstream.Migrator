using System.Collections.Generic;

namespace Eventstream.Migrator.Migrating
{
    public interface IGetEventstreamMigrations
    {
        IEnumerable<IMigrate> GetMigrations();
    }
}