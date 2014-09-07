using System;

namespace Eventstream.Migrator.Migrating
{
    public interface IReadAssemblyData
    {
        Type[] GetMigrationTypes();
    }
}