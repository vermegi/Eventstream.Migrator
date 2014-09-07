using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventstream.Migrator.Migrating
{
    public class MigrationGetter : IGetEventstreamMigrations
    {
        private readonly IReadAssemblyData _assemblyreader;

        public MigrationGetter(IReadAssemblyData assemblyreader)
        {
            _assemblyreader = assemblyreader;
        }

        public IEnumerable<IMigrate> GetMigrations()
        {
            var migrationTypes = _assemblyreader.GetMigrationTypes();

            return migrationTypes
                .Select(migrationType => (IMigrate) Activator.CreateInstance((Type) migrationType))
                .OrderBy(mig => mig.Order);
        }
    }
}