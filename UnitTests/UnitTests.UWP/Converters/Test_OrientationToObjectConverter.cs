// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UnitTests.Converters
{
    [TestClass]
    public class Test_OrientationToObjectConverter
    {
        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_OrientationConvertNullToString()
        {
            var converter = new OrientationToObjectConverter
            {
                HorizontalValue = "Horizontal",
                VerticalValue = "Vertical",
            };

            var result = converter.Convert(null, typeof(string), null, "en-us");
            Assert.AreEqual("Vertical", result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_OrientationConvertVerticalToVisibility()
        {
            var converter = new OrientationToObjectConverter
            {
                HorizontalValue = Visibility.Visible,
                VerticalValue = Visibility.Collapsed,
            };

            var result = converter.Convert(Orientation.Vertical, typeof(Visibility), null, "en-us");
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_OrientationConvertHorizontalToVisibility()
        {
            var converter = new OrientationToObjectConverter
            {
                HorizontalValue = Visibility.Visible,
                VerticalValue = Visibility.Collapsed,
            };

            var result = converter.Convert(Orientation.Horizontal, typeof(Visibility), null, "en-us");
            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_OrientationConvertStringToVisibilityWithNegateTrue()
        {
            var converter = new OrientationToObjectConverter
            {
                HorizontalValue = Visibility.Visible,
                VerticalValue = Visibility.Collapsed,
            };

            var result = converter.Convert("anything", typeof(Visibility), "true", "en-us");
            Assert.AreEqual(Visibility.Visible, result);
        }
    }
}