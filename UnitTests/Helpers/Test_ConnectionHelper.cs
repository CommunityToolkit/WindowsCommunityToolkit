using Microsoft.Toolkit.Uwp;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace UnitTests.Helpers
{
    [TestClass]
    public class Test_ConnectionHelper
    {
        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ConnectionHelper_IsInternetOnMeteredConnection()
        {
            Assert.IsFalse(ConnectionHelper.IsInternetOnMeteredConnection);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ConnectionHelper_IsInternetAvailable()
        {
            Assert.IsTrue(ConnectionHelper.IsInternetAvailable);
        }
    }
}
