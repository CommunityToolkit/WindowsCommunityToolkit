// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class PowerBIEmbeddedPage
    {
        public PowerBIEmbeddedPage()
        {
            this.InitializeComponent();
        }

        private void PropertyDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PropertyValue.Text = string.Empty;

            switch (PropertyDropdown.SelectedIndex)
            {
                case 0:
                    if (PropertyTipsGroupId != null)
                    {
                        PropertyTipsGroupId.Visibility = Visibility.Visible;
                        PropertyTipsEmbedUrl.Visibility = Visibility.Collapsed;
                    }

                    break;
                case 1:
                    if (PropertyTipsGroupId != null)
                    {
                        PropertyTipsGroupId.Visibility = Visibility.Collapsed;
                        PropertyTipsEmbedUrl.Visibility = Visibility.Visible;
                    }

                    break;
            }
        }

        private void ClientIdExpandButton_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityStatusPanel(ClientIdBox, (Button)sender);
        }

        private void SetVisibilityStatusPanel(FrameworkElement box, Button switchButton)
        {
            if (box.Visibility == Visibility.Visible)
            {
                switchButton.Content = "\xE70D";
                box.Visibility = Visibility.Collapsed;
            }
            else
            {
                switchButton.Content = "\xE70E";
                box.Visibility = Visibility.Visible;
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ClientId.Text.Trim())
                && !string.IsNullOrEmpty(PropertyValue.Text.Trim()))
            {
                switch (PropertyDropdown.SelectedIndex)
                {
                    case 0:
                        PowerBIEmbeddedControl.ClientId = ClientId.Text.Trim();
                        PowerBIEmbeddedControl.EmbedUrl = string.Empty;
                        PowerBIEmbeddedControl.GroupId = PropertyValue.Text.Trim();
                        break;
                    case 1:
                        PowerBIEmbeddedControl.ClientId = ClientId.Text.Trim();
                        PowerBIEmbeddedControl.GroupId = string.Empty;
                        PowerBIEmbeddedControl.EmbedUrl = PropertyValue.Text.Trim();
                        break;
                }

                SetVisibilityStatusPanel(ClientIdBox, ClientIdExpandButton);
            }
        }
    }
}
