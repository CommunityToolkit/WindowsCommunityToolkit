using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace UnitTests.Extensions
{
    public class Test_FrameworkElementExtensions : VisualUITestBase
    {
        [TestCategory("FrameworkElementExtensions")]
        [TestMethod]
        public async Task Test_Ancestor_WeakReference()
        {
            // Need to sim loading the control, and not just load the XAML via XamlReader
            var view = App.DispatcherQueue.EnqueueAsync(async () =>
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
    }
}
