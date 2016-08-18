using Windows.UI;
using Microsoft.Toolkit.Uwp.UI.Converters;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
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
    }
}
