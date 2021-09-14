// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace UnitTests.Extensions
{
    [TestClass]
    public class Test_FrameworkElementExtensions : VisualUITestBase
    {
        [Ignore]
        [TestCategory("FrameworkElementExtensions")]
        [TestMethod]
        public async Task Test_Ancestor_WeakReference()
        {
            // Need to sim loading the control, and not just load the XAML via XamlReader
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(
           @"<Page
                    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls""
                    xmlns:ui=""using:Microsoft.Toolkit.Uwp.UI"">

                    <Grid x:Name=""OuterGrid"" Width=""200"" Background=""Khaki"">
                        <Button x:Name=""InnerButton"" ui:FrameworkElementExtensions.AncestorType=""Grid"" />
                    </Grid>

                </Page>") as Page;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");
                await SetTestContentAsync(treeRoot);

                // Need to simulate loading the control (and not just rely upon XamlReader.Load)
                var frame = new Frame();
                frame.Navigate(treeRoot.GetType());
                Assert.IsInstanceOfType(frame.Content, typeof(Page));

                var btn = treeRoot.FindChild("InnerButton") as Button;

                Assert.IsNull(btn.Parent);

                btn.SetValue(FrameworkElementExtensions.AncestorProperty, new object());

                Assert.IsNull(btn.Parent);
            });
        }

        [Ignore]
        [TestCategory("FrameworkElementExtensions")]
        [TestMethod]
        public async Task Test_Ancestor_WeakRef_UnloadGrid()
        {
            // Need to sim loading the control, and not just load the XAML via XamlReader
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(
           @"<Page
                    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls""
                    xmlns:ui=""using:Microsoft.Toolkit.Uwp.UI"">

                    <Grid x:Name=""OuterGrid"" Width=""200"" Background=""Khaki"">
                        <Button x:Name=""InnerButton"" ui:FrameworkElementExtensions.AncestorType=""Grid"" />
                    </Grid>

                </Page>") as Page;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");
                await SetTestContentAsync(treeRoot);

                // Need to simulate loading the control (and not just rely upon XamlReader.Load)
                var frame = new Frame();
                frame.Navigate(treeRoot.GetType());
                Assert.IsInstanceOfType(frame.Content, typeof(Page));

                var grid = treeRoot.FindChild("OuterGrid") as Grid;
                var button = treeRoot.FindChild("InnerButton") as Button;

                button.SetValue(FrameworkElementExtensions.AncestorProperty, new object());

                treeRoot.Unloaded += (sender, e) =>
                {
                    Assert.AreEqual(grid, null);
                };

                // Allow treeRoot to Unload, before test ends
                await SetTestContentAsync(null);
            });
        }
    }
}
