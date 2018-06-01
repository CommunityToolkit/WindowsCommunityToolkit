// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class ObjectStoragePage
    {
        private readonly IObjectStorageHelper localStorageHelper = new LocalObjectStorageHelper();
        private readonly IObjectStorageHelper roamingStorageHelper = new RoamingObjectStorageHelper();

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
            if (string.IsNullOrEmpty(KeyTextBox.Text))
            {
                return;
            }

            if (string.IsNullOrEmpty(ContentTextBox.Text))
            {
                return;
            }

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
