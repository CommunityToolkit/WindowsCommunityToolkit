using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Windows.Toolkit.SampleApp.SamplePages
{
    public sealed partial class VariableSizedGridPage : Page
    {
        public VariableSizedGridPage()
        {
            this.InitializeComponent();
            this.DataContext = new Data.PhotosDataSource().GetItems();
        }
    }
}
