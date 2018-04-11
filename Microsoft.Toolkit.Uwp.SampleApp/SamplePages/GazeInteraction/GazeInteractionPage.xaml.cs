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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.Input.Gaze;
using Windows.UI.Popups;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GazeInteractionPage : Page
    {
        private uint _button1ClickCount = 0;
        private uint _togglebutton1ClickCount = 0;

        public GazeInteractionPage()
        {
            this.InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            TextBlock_Button1.Text = $"Clicks = {++_button1ClickCount}";
        }

        private void ToggleButton1_Checked(object sender, RoutedEventArgs e)
        {
            TextBlock_ToggleButton1.Text = $"Checks = {++_togglebutton1ClickCount}";
        }

        private void MessageDialog_Click(object sender, RoutedEventArgs e)
        {
            ShowMessageDialog();
        }

        private void ContentDialog_Click(object sender, RoutedEventArgs e)
        {
            ShowContentDialog();
        }

        private async void ShowMessageDialog()
        {
            string message = $"Congratulations!! You have a MessageDialog";
            MessageDialog dlg = new MessageDialog(message);
            await dlg.ShowAsync();
        }

        private async void ShowContentDialog()
        {
            ContentDialog dlg = new ContentDialog()
            {
                Title = "I am a content Dialog",
                Content = "There is content here.",
                CloseButtonText = "Ok"
            };
            await dlg.ShowAsync();
        }
    }
}
