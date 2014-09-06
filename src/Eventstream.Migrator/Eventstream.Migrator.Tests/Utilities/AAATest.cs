using Xunit;

namespace Eventstream.Migrator.Tests.Utilities
{
    public abstract class AAATest : IUseFixture<SetItUp>
    {
        public abstract void Arrange();
        public abstract void Act();

        public void SetFixture(SetItUp data)
        {
            data.Setup(Arrange);
            ExecuteTest();
        }

        protected virtual void ExecuteTest()
        {
            Act();
        }
    }
}