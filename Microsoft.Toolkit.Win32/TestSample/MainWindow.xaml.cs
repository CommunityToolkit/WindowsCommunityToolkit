using Microsoft.Toolkit.Win32.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Media.Core;

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
    }

}
