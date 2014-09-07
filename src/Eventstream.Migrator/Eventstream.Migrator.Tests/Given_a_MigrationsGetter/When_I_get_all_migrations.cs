using System;
using System.Collections.Generic;
using System.Linq;
using Eventstream.Migrator.Tests.Utilities;
using Moq;
using Xunit;

namespace Eventstream.Migrator.Tests.Given_a_MigrationsGetter
{
    public class When_I_get_all_migrations : AAATest
    {
        private MigrationGetter _sut;
        private Mock<IReadAssemblyData> _assemblyReader;
        private IEnumerable<IMigrate> _result;

        public override void Arrange()
        {
            _assemblyReader = new Mock<IReadAssemblyData>();
            var migrationTypes = new Type[]
            {
                typeof(FirstMigration),
                typeof(SecondMigration)
            };
            _assemblyReader.Setup(reader => reader.GetMigrationTypes())
                .Returns(migrationTypes);

            _sut = new MigrationGetter(_assemblyReader.Object);
        }

        public override void Act()
        {
            _result = _sut.GetMigrations();
        }

        [Fact]
        public void It_returns_an_instantiated_version_of_each_IMigrate()
        {
            Assert.NotNull(_result);
            Assert.Equal(2, _result.Count());
            Assert.True(_result.Any(mig => mig.GetType() == typeof (FirstMigration)));
            Assert.True(_result.Any(mig => mig.GetType() == typeof (SecondMigration)));
        }

        [Fact]
        public void It_returns_these_migrations_in_order()
        {
            Assert.Equal(0, _result.First().Order);
            Assert.Equal(1, _result.Last().Order);
        }
    }

    public class SecondMigration : IMigrate
    {
        public IEnumerable<TEvent> Migrate<TEvent>(TEvent someEvent)
        {
            return null;
        }

        public int Order { get { return 0; } }
    }

    public class FirstMigration : IMigrate
    {
        public IEnumerable<TEvent> Migrate<TEvent>(TEvent someEvent)
        {
            return null;
        }

        public int Order { get { return 1; } }
    }
}