using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.Windows.Toolkit;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class HelperUnitTests
    {
        [TestMethod]
        public void TestCompress()
        {
            var compressedVersion = "UWPToolkit".Compress();
            Assert.IsTrue(compressedVersion == "H4sIAAAAAAAEAAsNDwjJz8/JziwBAAVj3RwKAAAA");
        }

        [TestMethod]
        public void TestDecompress()
        {
            var compressedVersion = "H4sIAAAAAAAEAAsNDwjJz8/JziwBAAVj3RwKAAAA".Decompress();
            Assert.IsTrue(compressedVersion == "UWPToolkit");
        }

        [TestMethod]
        public async Task TestGetPackagedFileAsync()
        {
            using (var stream = await Helpers.GetPackagedFileStreamAsync("Assets/Sub/test.txt"))
            {

            }
        }

        [TestMethod]
        public async Task TestReadTextAsString()
        {
            using (var stream = await Helpers.GetPackagedFileStreamAsync("Assets/Sub/test.txt"))
            {
                var readText = await stream.ReadTextAsync();
                Assert.IsTrue(readText == "This is my content text");
            }
        }

        [TestMethod]
        public void TestIsInternetAvailable()
        {
            Assert.IsTrue(Helpers.IsInternetAvailable());
        }

        [TestMethod]
        public void TestIsInternetOnMeteredConnection()
        {
            Assert.IsFalse(Helpers.IsInternetOnMeteredConnection);
        }
    }
}
