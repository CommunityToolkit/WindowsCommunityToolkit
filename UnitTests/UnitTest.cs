using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.Windows.Toolkit;

namespace UnitTests
{
    [TestClass]
    public class CoreUnitTests
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
    }
}
