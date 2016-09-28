using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class SettingStoragePage
    {
        private ISettingStorageHelper localStorageHelper = new LocalSettingStorageHelper();
        private ISettingStorageHelper roamingStorageHelper = new RoamingSettingStorageHelper();

        public SettingStoragePage()
        {
            InitializeComponent();
        }

        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            if (StorageModeToggle.IsOn)
            {
                // Read from roaming storage
                ContentTextBox.Text = roamingStorageHelper.Read<string>(KeyTextBox.Text);
            }
            else
            {
                // Read from local storage
                ContentTextBox.Text = localStorageHelper.Read<string>(KeyTextBox.Text);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (StorageModeToggle.IsOn)
            {
                // Save into roaming storage
                roamingStorageHelper.Save(KeyTextBox.Text, ContentTextBox.Text);
            }
            else
            {
                // Save into local storage
                localStorageHelper.Save(KeyTextBox.Text, ContentTextBox.Text);
            }
        }
    }
}
