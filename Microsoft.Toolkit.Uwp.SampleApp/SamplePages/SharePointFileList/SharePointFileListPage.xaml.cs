// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Controls.Graph;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the opacity behavior.
    /// </summary>
    public sealed partial class SharePointFileListPage : Page, IXamlRenderListener
    {
        private SharePointFileList _sharePointFilesControl;
        private StackPanel _loadPanel;
        private TextBox _docLibOrDriveURL;
        private Button _loadButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointFileListPage"/> class.
        /// </summary>
        public SharePointFileListPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            _sharePointFilesControl = control.FindDescendantByName("SharePointFileListControl") as SharePointFileList;
            _loadPanel = control.FindDescendantByName("LoadPanel") as StackPanel;
            _docLibOrDriveURL = control.FindDescendantByName("DocLibOrDriveURL") as TextBox;
            _loadButton = control.FindDescendantByName("LoadButton") as Button;

            if (_sharePointFilesControl != null && _loadPanel != null && _docLibOrDriveURL != null && _loadButton != null)
            {
                _loadButton.Click += LoadtButton_Click;

                if (!string.IsNullOrEmpty(_sharePointFilesControl.DriveUrl))
                {
                    _sharePointFilesControl.Visibility = Visibility.Visible;
                    _loadPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    _sharePointFilesControl.Visibility = Visibility.Collapsed;
                    _loadPanel.Visibility = Visibility.Visible;
                }
            }
        }

        private void LoadtButton_Click(object sender, RoutedEventArgs e)
        {
            string driveURL = _docLibOrDriveURL.Text.Trim();
            if (string.IsNullOrEmpty(driveURL))
            {
                return;
            }

            _sharePointFilesControl.DriveUrl = driveURL;

            _sharePointFilesControl.Visibility = Visibility.Visible;
            _loadPanel.Visibility = Visibility.Collapsed;
        }
    }
}
