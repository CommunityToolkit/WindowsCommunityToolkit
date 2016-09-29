using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
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
