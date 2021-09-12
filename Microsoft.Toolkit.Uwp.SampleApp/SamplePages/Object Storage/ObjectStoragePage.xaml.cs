// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Helpers;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class ObjectStoragePage
    {
        private readonly ApplicationDataStorageHelper _settingsStorage = ApplicationDataStorageHelper.GetCurrent();

        public ObjectStoragePage()
        {
            InitializeComponent();
        }

        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(KeyTextBox.Text))
            {
                return;
            }

            // Read from local storage
            if (_settingsStorage.KeyExists(KeyTextBox.Text))
            {
                ContentTextBox.Text = _settingsStorage.Read<string>(KeyTextBox.Text);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(KeyTextBox.Text))
            {
                return;
            }

            if (string.IsNullOrEmpty(ContentTextBox.Text))
            {
                return;
            }

            // Save into local storage
            _settingsStorage.Save(KeyTextBox.Text, ContentTextBox.Text);
        }
    }
}