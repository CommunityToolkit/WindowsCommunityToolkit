using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the Eyedropper control.
    /// </summary>
    public sealed partial class EyedropperPage : Page, IXamlRenderListener
    {
        public EyedropperPage()
        {
            this.InitializeComponent();
            Load();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
        }

        private void Load()
        {
            SampleController.Current.RegisterNewCommand("Global Eyedropper", async (sender, args) =>
            {
                var eyedropper = new Eyedropper();
                var color = await eyedropper.Open();
                InAppNotification.Show($"You get {color}.", 3000);
            });
        }
    }
}
