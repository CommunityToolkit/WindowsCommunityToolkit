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
using Microsoft.Toolkit.Uwp.UI.Controls.Graph;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the opacity behavior.
    /// </summary>
    public sealed partial class SharePointFilesPage : Page, IXamlRenderListener
    {
        private SharePointFiles _sharePointFilesControl;
        private StackPanel _convertPanel;
        private TextBox _tbDocLibURL;
        private Button _btnConvert;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointFilesPage"/> class.
        /// </summary>
        public SharePointFilesPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            _sharePointFilesControl = control.FindDescendantByName("SharePointFilesControl") as SharePointFiles;
            _convertPanel = control.FindDescendantByName("ConvertPanel") as StackPanel;
            _tbDocLibURL = control.FindDescendantByName("tbDocLibURL") as TextBox;
            _btnConvert = control.FindDescendantByName("btnConvert") as Button;

            if (_sharePointFilesControl != null)
            {
                _btnConvert.Click += ConvertButton_Click;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Convert Drive URL", (sender, args) =>
            {
                if (_sharePointFilesControl != null && _convertPanel != null)
                {
                    if (_sharePointFilesControl.Visibility == Visibility.Collapsed)
                    {
                        _sharePointFilesControl.Visibility = Visibility.Visible;
                        _convertPanel.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        _sharePointFilesControl.Visibility = Visibility.Collapsed;
                        _convertPanel.Visibility = Visibility.Visible;
                    }
                }
            });
        }

        private async void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            if (_sharePointFilesControl != null && _tbDocLibURL != null && _convertPanel != null)
            {
                if (string.IsNullOrEmpty(_sharePointFilesControl.GraphAccessToken))
                {
                    await new MessageDialog("Please set the GraphAccessToken in the properties panel first.").ShowAsync();
                    return;
                }

                string driveURL = await _sharePointFilesControl.GetDriveUrlFromSharePointUrl(_tbDocLibURL.Text);
                DataPackage copyData = new DataPackage();
                copyData.SetText(driveURL);
                Clipboard.SetContent(copyData);

                _sharePointFilesControl.Visibility = Visibility.Visible;
                _convertPanel.Visibility = Visibility.Collapsed;
            }
        }
    }
}
