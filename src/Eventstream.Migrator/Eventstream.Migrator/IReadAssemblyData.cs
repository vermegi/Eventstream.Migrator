using System;

namespace Eventstream.Migrator
{
    public interface IReadAssemblyData
    {
        Type[] GetMigrationTypes();
    }
}