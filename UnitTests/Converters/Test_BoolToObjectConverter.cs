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

using Microsoft.Toolkit.Uwp.UI.Converters;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using UITestMethod = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethodAttribute;

namespace UnitTests.Converters
{
    [TestClass]
    public class Test_BoolToObjectConverter
    {
        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertTrueToVisibility()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed
            };
            var result = converter.Convert(true, typeof(Visibility), null, "en-us");
            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertFalseToVisibility()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed
            };
            var result = converter.Convert(false, typeof(Visibility), null, "en-us");
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertTrueToVisibilityWithNegateTrue()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed
            };
            var result = converter.Convert(true, typeof(Visibility), "true", "en-us");
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertFalseToVisibilityWithNegateTrue()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed
            };
            var result = converter.Convert(false, typeof(Visibility), "true", "en-us");
            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertTrueToVisibilityWithNegateFalse()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed
            };
            var result = converter.Convert(true, typeof(Visibility), "false", "en-us");
            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertFalseToVisibilityWithNegateFalse()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed
            };
            var result = converter.Convert(false, typeof(Visibility), "false", "en-us");
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertTrueToVisibilityWithInvalidNegate()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed
            };
            var result = converter.Convert(true, typeof(Visibility), 42, "en-us");
            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertFalseToVisibilityWithInvalidNegate()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed
            };
            var result = converter.Convert(false, typeof(Visibility), 42, "en-us");
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertTrueToBrush()
        {
            var greenBrush = new SolidColorBrush(Colors.Green);
            var redBrush = new SolidColorBrush(Colors.Red);
            var converter = new BoolToObjectConverter
            {
                TrueValue = greenBrush,
                FalseValue = redBrush
            };
            var result = converter.Convert(true, typeof(Brush), null, "en-us");
            Assert.AreEqual(greenBrush, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertFalseToBrush()
        {
            var greenBrush = new SolidColorBrush(Colors.Green);
            var redBrush = new SolidColorBrush(Colors.Red);
            var converter = new BoolToObjectConverter
            {
                TrueValue = greenBrush,
                FalseValue = redBrush
            };
            var result = converter.Convert(false, typeof(Brush), null, "en-us");
            Assert.AreEqual(redBrush, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertTrueToBitmapImageWithTypeConversion()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = "ms-appx:///Assets/True.png",
                FalseValue = "ms-appx:///Assets/False.png"
            };

            var result = converter.Convert(true, typeof(ImageSource), null, "en-us");
            Assert.IsInstanceOfType(result, typeof(BitmapImage));
            Assert.AreEqual(converter.TrueValue, ((BitmapImage)result).UriSource.ToString());
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertTrueToBrushWithTypeConversion()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = "Green",
                FalseValue = "Red"
            };

            var result = converter.Convert(true, typeof(Brush), null, "en-us");
            Assert.IsInstanceOfType(result, typeof(SolidColorBrush));
            Assert.AreEqual(Colors.Green, ((SolidColorBrush)result).Color);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertBackVisible()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed
            };
            var result = converter.ConvertBack(Visibility.Visible, typeof(Visibility), null, "en-us");
            Assert.AreEqual(true, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertBackCollapsed()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed
            };
            var result = converter.ConvertBack(Visibility.Collapsed, typeof(Visibility), null, "en-us");
            Assert.AreEqual(false, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertBackVisibleWithNegateTrue()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed
            };
            var result = converter.ConvertBack(Visibility.Visible, typeof(Visibility), "true", "en-us");
            Assert.AreEqual(false, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertBackCollapseWithNegateTrue()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed
            };
            var result = converter.ConvertBack(Visibility.Collapsed, typeof(Visibility), "true", "en-us");
            Assert.AreEqual(true, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertBackVisibleWithNegateFalse()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed
            };
            var result = converter.ConvertBack(Visibility.Visible, typeof(Visibility), "false", "en-us");
            Assert.AreEqual(true, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertBackCollaspedWithNegateFalse()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed
            };
            var result = converter.ConvertBack(Visibility.Collapsed, typeof(Visibility), "false", "en-us");
            Assert.AreEqual(false, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertBackBrush()
        {
            var greenBrush = new SolidColorBrush(Colors.Green);
            var redBrush = new SolidColorBrush(Colors.Red);
            var converter = new BoolToObjectConverter
            {
                TrueValue = greenBrush,
                FalseValue = redBrush
            };
            var result = converter.ConvertBack(greenBrush, typeof(Brush), null, "en-us");
            Assert.AreEqual(true, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertBackBrush2()
        {
            var greenBrush = new SolidColorBrush(Colors.Green);
            var redBrush = new SolidColorBrush(Colors.Red);
            var converter = new BoolToObjectConverter
            {
                TrueValue = greenBrush,
                FalseValue = redBrush
            };
            var result = converter.ConvertBack(redBrush, typeof(Brush), null, "en-us");
            Assert.AreEqual(false, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertBackRefTypeWithTypeConversion()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = "ms-appx:///Assets/True.png",
                FalseValue = "ms-appx:///Assets/False.png"
            };

            var result = converter.ConvertBack(converter.TrueValue, typeof(ImageSource), null, "en-us");
            Assert.AreEqual(false, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertBackRefTypeWithTypeConversion2()
        {
            var converter = new BoolToObjectConverter
            {
                TrueValue = "ms-appx:///Assets/True.png",
                FalseValue = "ms-appx:///Assets/False.png"
            };

            var result = converter.ConvertBack(converter.FalseValue, typeof(ImageSource), null, "en-us");
            Assert.AreEqual(false, result);
        }
    }
}
