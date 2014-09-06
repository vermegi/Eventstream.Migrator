using System;

namespace Eventstream.Migrator.Tests.Utilities
{
    public class SetItUp
    {
        public void Setup(Action task)
        {
            task();
        }
    }
}