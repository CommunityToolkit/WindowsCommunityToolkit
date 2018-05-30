using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class PowerBIEmbeddedPage : IXamlRenderListener
    {
        public PowerBIEmbeddedPage()
        {
            this.InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
        }
    }
}
