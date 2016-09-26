using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StoragePage
    {
        private IStorageService localStorageService = new LocalStorageService();
        private IStorageService roamingStorageService = new RoamingStorageService();

        public StoragePage()
        {
            this.InitializeComponent();
        }

        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            if (StorageModeToggle.IsOn)
            {
                // Read from roaming storage
                ContentTextBox.Text = roamingStorageService.Read<string>(KeyTextBox.Text);
            }
            else
            {
                // Read from local storage
                ContentTextBox.Text = localStorageService.Read<string>(KeyTextBox.Text);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (StorageModeToggle.IsOn)
            {
                // Save into roaming storage
                roamingStorageService.Save(KeyTextBox.Text, ContentTextBox.Text);
            }
            else
            {
                // Save into local storage
                localStorageService.Save(KeyTextBox.Text, ContentTextBox.Text);
            }
        }
    }
}
