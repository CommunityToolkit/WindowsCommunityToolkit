using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages.Loading
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoadingPage : INotifyPropertyChanged
    {
        private bool _isBusy;

        public LoadingPage()
        {
            this.InitializeComponent();
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private async void ShowLoadingDialogDelegateAsync(object sender, TappedRoutedEventArgs e)
        {
            IsBusy = true;
            await Task.Delay(3000);
            IsBusy = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
