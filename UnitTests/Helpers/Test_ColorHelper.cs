// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Microsoft.Toolkit.Uwp;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace UnitTests.Helpers
{
    [TestClass]
    public class Test_ColorHelper
    {
        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ColorHelper_ToColor_Predifined()
        {
            Assert.AreEqual("Red".ToColor(), Windows.UI.Colors.Red);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ColorHelper_ToColor_Hex8Digits()
        {
            Assert.AreEqual("#FFFF0000".ToColor(), Windows.UI.Colors.Red);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ColorHelper_ToColor_Hex6Digits()
        {
            Assert.AreEqual("#FF0000".ToColor(), Windows.UI.Colors.Red);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ColorHelper_ToColor_Hex4Digits()
        {
            Assert.AreEqual("#FF00".ToColor(), Windows.UI.Colors.Red);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ColorHelper_ToColor_Hex3Digits()
        {
            Assert.AreEqual("#F00".ToColor(), Windows.UI.Colors.Red);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ColorHelper_ToColor_ScreenColor()
        {
            Assert.AreEqual("sc#1.0,1.0,0,0".ToColor(), Windows.UI.Colors.Red);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ColorHelper_ToHex()
        {
            Assert.AreEqual(Windows.UI.Colors.Red.ToHex(), "#FFFF0000");
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ColorHelper_ToInt()
        {
            Assert.AreEqual(Windows.UI.Colors.Red.ToInt(), -65536);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ColorHelper_ToHsl()
        {
            HslColor hslRed;
            hslRed.A = 1.0;  // Alpha
            hslRed.H = 0.0;  // Hue
            hslRed.S = 1.0;  // Saturation
            hslRed.L = 0.5;  // Lightness

            Assert.AreEqual(Windows.UI.Colors.Red.ToHsl(), hslRed);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ColorHelper_ToHsv()
        {
            HsvColor hsvRed;
            hsvRed.A = 1.0;  // Alpha
            hsvRed.H = 0.0;  // Hue
            hsvRed.S = 1.0;  // Saturation
            hsvRed.V = 1.0;  // Value

            Assert.AreEqual(Windows.UI.Colors.Red.ToHsv(), hsvRed);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ColorHelper_FromHsl()
        {
            Assert.AreEqual(ColorHelper.FromHsl(0.0, 1.0, 0.5), Windows.UI.Colors.Red);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ColorHelper_FromHsv()
        {
            Assert.AreEqual(ColorHelper.FromHsv(0.0, 1.0, 1.0), Windows.UI.Colors.Red);
        }
    }
}
