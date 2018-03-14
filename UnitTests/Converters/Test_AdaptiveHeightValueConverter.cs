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

using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Converters;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using UITestMethod = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethodAttribute;

namespace UnitTests.Converters
{
    [TestClass]
    public class Test_AdaptiveHeightValueConverter
    {
        [TestCategory("Converters")]
        [TestMethod]
        public void Convert_ValueIsNull_ReturnsNaN()
        {
            var converter = new AdaptiveHeightValueConverter();
            var result = converter.Convert(null, null, null, null);
            Assert.AreEqual(result, double.NaN);
        }

        [TestCategory("Converters")]
        [TestMethod]
        public void Convert_ParameterIsNull_ReturnsValue()
        {
            var converter = new AdaptiveHeightValueConverter();
            double value = 100;
            var result = converter.Convert(value, null, null, null);
            Assert.AreEqual(result, value);
        }


        [TestCategory("Converters")]
        [TestMethod]
        public void Convert_GridViewIsNull_ReturnsValue()
        {
            var converter = new AdaptiveHeightValueConverter();
            double value = 100;
            var result = converter.Convert(value, null, null, null);
            Assert.AreEqual(result, value);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Convert_GridViewWithItemContainerStyle_ReturnsHeightPlusItemMargin()
        {
            var converter = new AdaptiveHeightValueConverter();
            double value = 100;
            var gridView = new AdaptiveGridView();

            var margin = new Thickness(10);
            var style = new Style(typeof(GridViewItem));
            style.Setters.Add(new Setter(GridViewItem.MarginProperty, margin));
            gridView.ItemContainerStyle = style;

            var result = converter.Convert(value, null, gridView, null);
            Assert.AreEqual(result, value + margin.Bottom + margin.Top);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Convert_GridViewWithNoItemContainerStyleAndNoItems_ReturnsHeightPlusDefaultItemMargin()
        {
            var converter = new AdaptiveHeightValueConverter();
            double value = 100;
            var gridView = new AdaptiveGridView { ItemContainerStyle = null };
            var result = converter.Convert(value, null, gridView, null);
            Assert.AreEqual(result, value + converter.DefaultItemMargin.Bottom + converter.DefaultItemMargin.Top);
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Convert_GridViewWithPadding_ReturnsHeightPlusPadding()
        {
            var converter = new AdaptiveHeightValueConverter { DefaultItemMargin = new Thickness(0) };
            double value = 100;
            var gridView = new AdaptiveGridView { ItemContainerStyle = null, Padding = new Thickness(10) };
            var result = converter.Convert(value, null, gridView, null);
            Assert.AreEqual(result, value + gridView.Padding.Bottom + gridView.Padding.Top);
        }
    }
}
