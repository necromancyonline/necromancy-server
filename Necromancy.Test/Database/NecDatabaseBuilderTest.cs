using Necromancy.Server.Database;
using Necromancy.Server.Setting;
using Xunit;

namespace Necromancy.Test.Database
{
    public class NecDatabaseBuilderTest
    {
        [Fact]
        public void TestBuild()
        {
            /* Confirm successful build with currently configured database. */
            NecSetting settings = new NecSetting();
            new NecDatabaseBuilder(settings).Build();
        }
    }
}
