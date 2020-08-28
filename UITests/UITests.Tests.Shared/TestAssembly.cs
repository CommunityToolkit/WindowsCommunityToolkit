#if USING_TAEF
using WEX.TestExecution.Markup;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Windows.UI.Xaml.Tests.MUXControls.InteractionTests.Infra;

namespace UITests.Tests
{
    // This is marked as a test class to make sure our AssemblyInitialize and AssemblyCleanup
    // fixtures get executed.  It won't actually host any tests.
    [TestClass]
    public class TestAssembly
    {
        [AssemblyInitialize]
        [TestProperty("CoreClrProfile", ".NETCoreApp2.1")]
        [TestProperty("RunFixtureAs:Assembly", "ElevatedUserOrSystem")]
        public static void AssemblyInitialize(TestContext testContext)
        {
            TestEnvironment.AssemblyInitialize(testContext, "UITests.App.pfx");
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            TestEnvironment.AssemblyCleanup();
        }
    }
}