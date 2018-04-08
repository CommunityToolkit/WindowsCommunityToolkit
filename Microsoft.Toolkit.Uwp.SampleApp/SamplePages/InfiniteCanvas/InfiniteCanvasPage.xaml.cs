using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// InfinteCanvas sample page.
    /// </summary>
    public sealed partial class InfiniteCanvasPage : Page, IXamlRenderListener
    {
        private const string InfiniteCanvasFileName = "infiniteCanvasFile.txt";
        private InfiniteCanvas _infiniteCanvas;

        public InfiniteCanvasPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            _infiniteCanvas = control.FindChildByName("canvas") as InfiniteCanvas;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Export & Save", async (sender, args) =>
            {
                var json = _infiniteCanvas.ExportAsJson();
                await StorageFileHelper.WriteTextToLocalFileAsync(json, InfiniteCanvasFileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);
            });

            Shell.Current.RegisterNewCommand("Import and Load", async (sender, args) =>
            {
                var json = await StorageFileHelper.ReadTextFromLocalFileAsync(InfiniteCanvasFileName);
                _infiniteCanvas.ImportFromJson(json);
            });
        }
    }
}
