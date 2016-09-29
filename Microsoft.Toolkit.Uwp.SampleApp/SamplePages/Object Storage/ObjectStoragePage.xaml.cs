using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class ObjectStoragePage
    {
        private IObjectStorageHelper localStorageHelper = new LocalObjectStorageHelper();
        private IObjectStorageHelper roamingStorageHelper = new RoamingObjectStorageHelper();

        public ObjectStoragePage()
        {
            InitializeComponent();
        }

        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            if (StorageModeToggle.IsOn)
            {
                // Read from roaming storage
                if (roamingStorageHelper.KeyExists(KeyTextBox.Text))
                {
                    ContentTextBox.Text = roamingStorageHelper.Read<string>(KeyTextBox.Text);
                }
            }
            else
            {
                // Read from local storage
                if (localStorageHelper.KeyExists(KeyTextBox.Text))
                {
                    ContentTextBox.Text = localStorageHelper.Read<string>(KeyTextBox.Text);
                }
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
