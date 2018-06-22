using System;
using System.Windows;
using System.Windows.Input;
using Windows.Media.Core;
using Windows.UI.Xaml.Controls;

namespace TestSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Uri CaptionedMediaUri { get; } =
                new Uri("https://mediaplatstorage1.blob.core.windows.net/windows-universal-samples-media/elephantsdream-clip-h264_sd-aac_eng-aac_spa-aac_eng_commentary-srt_eng-srt_por-srt_swe.mkv");

        public MediaSource MediaSource { get; } = MediaSource.CreateFromUri(
            new Uri("https://mediaplatstorage1.blob.core.windows.net/windows-universal-samples-media/elephantsdream-clip-h264_sd-aac_eng-aac_spa-aac_eng_commentary-srt_eng-srt_por-srt_swe.mkv"));

        public MainWindow()
        {
            InitializeComponent();

        }

        private void inkCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            inkCanvas.InkPresenter.InputDeviceTypes =
    Windows.UI.Core.CoreInputDeviceTypes.Mouse |
    Windows.UI.Core.CoreInputDeviceTypes.Pen | Windows.UI.Core.CoreInputDeviceTypes.Touch;
        }




        private void inkToolbar_Initialized(object sender, EventArgs e)
        {
            toolButtonLasso.Content = new SymbolIcon(Symbol.SelectAll);
            //var selectButton = new InkToolbarCustomToolButton();// { Command = command, IsEnabled =true };
            //ToolTipService.SetToolTip(selectButton, "testing");
            //selectButton.Content = new SymbolIcon(Symbol.SelectAll);
            //selectButton.Click += SelectButton_Click;
            //inkToolbar.Children.Add(selectButton);
        }

        private void SelectButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

    }

}
