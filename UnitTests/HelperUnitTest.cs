// *********************************************************
//  Copyright (c) Microsoft. All rights reserved.
//  This code is licensed under the MIT License (MIT).
//  THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//  DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
//  TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
//  THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// *********************************************************

using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.Windows.Toolkit;

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

        [TestMethod]
        public void TestStringToColor()
        {
            Assert.IsTrue("Red".ToColor().ToString() == "#FFFF0000");
        }
    }
}
