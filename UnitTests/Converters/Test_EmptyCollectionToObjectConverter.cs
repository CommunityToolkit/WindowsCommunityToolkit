﻿// ******************************************************************
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

using System;
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
    public class Test_EmptyCollectionToObjectConverter
    {
        private static readonly object NullCollection = null;
        private static readonly object EmptyCollection = Array.Empty<object>();
        private static readonly object NotEmptyCollection = new[] { new object() };

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertNullCollectionToVisibility()
        {
            var converter = new EmptyCollectionToObjectConverter
            {
                NotEmptyValue = Visibility.Visible,
                EmptyValue = Visibility.Collapsed
            };
            var result = converter.Convert(NullCollection, typeof(Visibility), null, "en-us");
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertNotEmptyCollectionToVisibility()
        {
            var converter = new EmptyCollectionToObjectConverter
            {
                NotEmptyValue = Visibility.Visible,
                EmptyValue = Visibility.Collapsed
            };
            var result = converter.Convert(EmptyCollection, typeof(Visibility), null, "en-us");
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertEmptyCollectionToVisibility()
        {
            var converter = new EmptyCollectionToObjectConverter
            {
                NotEmptyValue = Visibility.Visible,
                EmptyValue = Visibility.Collapsed
            };
            var result = converter.Convert(EmptyCollection, typeof(Visibility), null, "en-us");
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertNotEmptyCollectionToVisibilityWithNegateTrue()
        {
            var converter = new EmptyCollectionToObjectConverter
            {
                NotEmptyValue = Visibility.Visible,
                EmptyValue = Visibility.Collapsed
            };
            var result = converter.Convert(NotEmptyCollection, typeof(Visibility), "true", "en-us");
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertEmptyCollectionToVisibilityWithNegateTrue()
        {
            var converter = new EmptyCollectionToObjectConverter
            {
                NotEmptyValue = Visibility.Visible,
                EmptyValue = Visibility.Collapsed
            };
            var result = converter.Convert(EmptyCollection, typeof(Visibility), "true", "en-us");
            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertNotEmptyCollectionToVisibilityWithNegateFalse()
        {
            var converter = new EmptyCollectionToObjectConverter
            {
                NotEmptyValue = Visibility.Visible,
                EmptyValue = Visibility.Collapsed
            };
            var result = converter.Convert(NotEmptyCollection, typeof(Visibility), "false", "en-us");
            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertEmptyCollectionToVisibilityWithNegateFalse()
        {
            var converter = new EmptyCollectionToObjectConverter
            {
                NotEmptyValue = Visibility.Visible,
                EmptyValue = Visibility.Collapsed
            };
            var result = converter.Convert(EmptyCollection, typeof(Visibility), "false", "en-us");
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertNotEmptyCollectionToVisibilityWithInvalidNegate()
        {
            var converter = new EmptyCollectionToObjectConverter
            {
                NotEmptyValue = Visibility.Visible,
                EmptyValue = Visibility.Collapsed
            };
            var result = converter.Convert(NotEmptyCollection, typeof(Visibility), 42, "en-us");
            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertEmptyCollectionToVisibilityWithInvalidNegate()
        {
            var converter = new EmptyCollectionToObjectConverter
            {
                NotEmptyValue = Visibility.Visible,
                EmptyValue = Visibility.Collapsed
            };
            var result = converter.Convert(EmptyCollection, typeof(Visibility), 42, "en-us");
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertNotEmptyCollectionToBrush()
        {
            var greenBrush = new SolidColorBrush(Colors.Green);
            var redBrush = new SolidColorBrush(Colors.Red);
            var converter = new EmptyCollectionToObjectConverter
            {
                NotEmptyValue = greenBrush,
                EmptyValue = redBrush
            };
            var result = converter.Convert(NotEmptyCollection, typeof(Brush), null, "en-us");
            Assert.AreEqual(greenBrush, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertEmptyCollectionToBrush()
        {
            var greenBrush = new SolidColorBrush(Colors.Green);
            var redBrush = new SolidColorBrush(Colors.Red);
            var converter = new EmptyCollectionToObjectConverter
            {
                NotEmptyValue = greenBrush,
                EmptyValue = redBrush
            };
            var result = converter.Convert(EmptyCollection, typeof(Brush), null, "en-us");
            Assert.AreEqual(redBrush, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertNotEmptyCollectionToBitmapImageWithTypeConversion()
        {
            var converter = new EmptyCollectionToObjectConverter
            {
                NotEmptyValue = "ms-appx:///Assets/NotEmptyCollection.png",
                EmptyValue = "ms-appx:///Assets/EmptyCollection.png"
            };

            var result = converter.Convert(NotEmptyCollection, typeof(ImageSource), null, "en-us");
            Assert.IsInstanceOfType(result, typeof(BitmapImage));
            Assert.AreEqual(converter.NotEmptyValue, ((BitmapImage)result).UriSource.ToString());
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_ConvertNotEmptyCollectionToBrushWithTypeConversion()
        {
            var converter = new EmptyCollectionToObjectConverter
            {
                NotEmptyValue = "Green",
                EmptyValue = "Red"
            };

            var result = converter.Convert(NotEmptyCollection, typeof(Brush), null, "en-us");
            Assert.IsInstanceOfType(result, typeof(SolidColorBrush));
            Assert.AreEqual(Colors.Green, ((SolidColorBrush)result).Color);
        }
    }
}