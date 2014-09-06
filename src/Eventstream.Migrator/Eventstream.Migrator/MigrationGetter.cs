using System;
using System.Collections.Generic;

namespace Eventstream.Migrator
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

            foreach (var migrationType in migrationTypes)
            {
                yield return (IMigrate) Activator.CreateInstance(migrationType);
            }
        }
    }
}