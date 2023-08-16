// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Windows.UI.Xaml;

namespace UnitTests.Converters
{
    [TestClass]
    public class Test_DoubleToObjectConverter
    {
        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_DoubleToVisibilityConverter_LessThan_Visible()
        {
            var converter = new DoubleToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed,
                LessThan = 1.0,
            };
            var result = converter.Convert(0.5, typeof(Visibility), null, "en-us");
            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_DoubleToVisibilityConverter_LessThan_Collapsed()
        {
            var converter = new DoubleToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed,
                LessThan = 1.0,
            };
            var result = converter.Convert(1.5, typeof(Visibility), null, "en-us");
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_DoubleToVisibilityConverter_GreaterThan_Visible()
        {
            var converter = new DoubleToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed,
                GreaterThan = 1.0,
            };
            var result = converter.Convert(1.5, typeof(Visibility), null, "en-us");
            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_DoubleToVisibilityConverter_GreaterThan_Collapsed()
        {
            var converter = new DoubleToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed,
                GreaterThan = 1.0,
            };
            var result = converter.Convert(0.5, typeof(Visibility), null, "en-us");
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_DoubleToVisibilityConverter_GreaterLessThan_Visible()
        {
            var converter = new DoubleToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed,
                GreaterThan = 1.0,
                LessThan = 2.0,
            };
            var result = converter.Convert(1.5, typeof(Visibility), null, "en-us");
            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_DoubleToVisibilityConverter_GreaterLessThan_ValueGreater_Collapsed()
        {
            var converter = new DoubleToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed,
                GreaterThan = 1.0,
                LessThan = 2.0,
            };
            var result = converter.Convert(2.5, typeof(Visibility), null, "en-us");
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_DoubleToVisibilityConverter_GreaterLessThan_ValueLess_Collapsed()
        {
            var converter = new DoubleToObjectConverter
            {
                TrueValue = Visibility.Visible,
                FalseValue = Visibility.Collapsed,
                GreaterThan = 1.0,
                LessThan = 2.0,
            };
            var result = converter.Convert(0.5, typeof(Visibility), null, "en-us");
            Assert.AreEqual(Visibility.Collapsed, result);
        }
    }
}
