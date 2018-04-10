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

using System.Linq;
using Microsoft.Toolkit.Extensions;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

using Assert = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert;

namespace UnitTests.UI.Controls
{
    [TestClass]
    #pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
    public class Test_UniformGrid_Dimensions
    {
        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_GetDimensions_AllVisible()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <controls:UniformGrid x:Name=""UniformGrid"">
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
    </controls:UniformGrid>
</Page>") as FrameworkElement;

            Assert.IsNotNull(treeroot, "Could not load XAML tree.");

            var grid = treeroot.FindChildByName("UniformGrid") as UniformGrid;

            Assert.IsNotNull(grid, "Could not find UniformGrid in tree.");

            var children = grid.Children.Select(item => item as FrameworkElement);

            Assert.AreEqual(8, grid.Children.Count());

            var dimensions = UniformGrid.GetDimensions(ref children, 0, 0, 0);
            
            Assert.AreEqual(3, dimensions.rows);
            Assert.AreEqual(3, dimensions.columns);
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_GetDimensions_FirstColumn()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <controls:UniformGrid x:Name=""UniformGrid"">
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
    </controls:UniformGrid>
</Page>") as FrameworkElement;

            Assert.IsNotNull(treeroot, "Could not load XAML tree.");

            var grid = treeroot.FindChildByName("UniformGrid") as UniformGrid;

            Assert.IsNotNull(grid, "Could not find UniformGrid in tree.");

            var children = grid.Children.Select(item => item as FrameworkElement);

            Assert.AreEqual(8, grid.Children.Count());

            var dimensions = UniformGrid.GetDimensions(ref children, 0, 0, 2);

            Assert.AreEqual(4, dimensions.rows);
            Assert.AreEqual(4, dimensions.columns);
        }
    }
    #pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
}