// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace UnitTests.UI.Controls
{
    [TestClass]
    public class Test_WrapPanel : VisualUITestBase
    {
        [TestCategory("WrapPanel")]
        [TestMethod]
        public async Task Test_WrapPanel_Normal_Horizontal()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <controls:WrapPanel x:Name=""WrapPanel"">
        <Border Width=""100"" Height=""50""/>
        <Border Width=""100"" Height=""50""/>
        <Border Width=""100"" Height=""50""/>
    </controls:WrapPanel>
</Page>") as FrameworkElement;

                var expected = new (int u, int v)[]
                {
                    (0, 0),
                    (100, 0),
                    (200, 0),
                };

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                // Initialize Visual Tree
                await SetTestContentAsync(treeRoot);

                var panel = treeRoot.FindChild("WrapPanel") as WrapPanel;

                Assert.IsNotNull(panel, "Could not find WrapPanel in tree.");

                // Force Layout calculations
                panel.UpdateLayout();

                var children = panel.Children.Select(item => item as FrameworkElement).ToArray();

                Assert.AreEqual(3, panel.Children.Count);

                // Check all children are in expected places.
                for (int i = 0; i < children.Count(); i++)
                {
                    var transform = treeRoot.CoordinatesTo(children[i]);
                    Assert.AreEqual(expected[i].u, transform.X);
                    Assert.AreEqual(expected[i].v, transform.Y);
                }
            });
        }

        [TestCategory("WrapPanel")]
        [TestMethod]
        public async Task Test_WrapPanel_Normal_Vertical()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <controls:WrapPanel x:Name=""WrapPanel"" Orientation=""Vertical"">
        <Border Width=""100"" Height=""50""/>
        <Border Width=""100"" Height=""50""/>
        <Border Width=""100"" Height=""50""/>
    </controls:WrapPanel>
</Page>") as FrameworkElement;

                var expected = new (int u, int v)[]
                {
                    (0, 0),
                    (0, 50),
                    (0, 100),
                };

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                // Initialize Visual Tree
                await SetTestContentAsync(treeRoot);

                var panel = treeRoot.FindChild("WrapPanel") as WrapPanel;

                Assert.IsNotNull(panel, "Could not find WrapPanel in tree.");

                // Force Layout calculations
                panel.UpdateLayout();

                var children = panel.Children.Select(item => item as FrameworkElement).ToArray();

                Assert.AreEqual(3, panel.Children.Count);

                // Check all children are in expected places.
                for (int i = 0; i < children.Count(); i++)
                {
                    var transform = treeRoot.CoordinatesTo(children[i]);
                    Assert.AreEqual(expected[i].u, transform.X);
                    Assert.AreEqual(expected[i].v, transform.Y);
                }
            });
        }
    }
}