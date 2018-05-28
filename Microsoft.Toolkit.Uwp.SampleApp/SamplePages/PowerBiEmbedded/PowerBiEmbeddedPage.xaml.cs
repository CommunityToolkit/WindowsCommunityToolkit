using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class PowerBiEmbeddedPage : IXamlRenderListener
    {
        public PowerBiEmbeddedPage()
        {
            this.InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
        }
    }
}
