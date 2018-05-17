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

using System;
using Microsoft.Toolkit.Uwp.Helpers;
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

        private async void LoadtButton_Click(object sender, RoutedEventArgs e)
        {
            string driveURL = _docLibOrDriveURL.Text.Trim();
            if (string.IsNullOrEmpty(driveURL))
            {
                return;
            }

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                _sharePointFilesControl.DriveUrl = driveURL;

                _sharePointFilesControl.Visibility = Visibility.Visible;
                _loadPanel.Visibility = Visibility.Collapsed;
            });
        }
    }
}
