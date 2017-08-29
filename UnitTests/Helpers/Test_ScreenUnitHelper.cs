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

using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace UnitTests.Helpers
{
    [TestClass]
    public class Test_ScreenUnitHelper
    {
        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ScreenUnitHelper_Convert_SameUnits()
        {
            // Act
            float result = ScreenUnitHelper.Convert(ScreenUnit.Pixel, ScreenUnit.Pixel, 1);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ScreenUnitHelper_Convert_PixelToCentimeter()
        {
            // Act
            float result = ScreenUnitHelper.Convert(ScreenUnit.Pixel, ScreenUnit.Centimeter, 1247.244094488f);

            // Assert
            Assert.AreEqual(33, result);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ScreenUnitHelper_Convert_CentimeterToPixel()
        {
            // Act
            float result = ScreenUnitHelper.Convert(ScreenUnit.Centimeter, ScreenUnit.Pixel, 33);

            // Assert
            Assert.AreEqual(1247.244094488f, result);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ScreenUnitHelper_Convert_PixelToInch()
        {
            // Act
            float result = ScreenUnitHelper.Convert(ScreenUnit.Pixel, ScreenUnit.Inch, 2688);

            // Assert
            Assert.AreEqual(28, result);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ScreenUnitHelper_Convert_InchToPixel()
        {
            // Act
            float result = ScreenUnitHelper.Convert(ScreenUnit.Inch, ScreenUnit.Pixel, 28);

            // Assert
            Assert.AreEqual(2688, result);
        }

        [TestMethod]
        public void Test_ScreenUnitHelper_Convert_InchToCentimeter()
        {
            // Act
            float result = ScreenUnitHelper.Convert(ScreenUnit.Inch, ScreenUnit.Centimeter, 60);

            // Assert
            Assert.AreEqual(152.4f, result);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ScreenUnitHelper_Convert_CentimeterToInch()
        {
            // Act
            float result = ScreenUnitHelper.Convert(ScreenUnit.Centimeter, ScreenUnit.Inch, 152.4f);

            // Assert
            Assert.AreEqual(60, result);
        }
    }
}
