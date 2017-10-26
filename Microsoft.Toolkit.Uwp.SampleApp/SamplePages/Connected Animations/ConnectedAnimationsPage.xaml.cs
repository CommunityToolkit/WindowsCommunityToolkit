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

using Microsoft.Toolkit.Uwp.SampleApp.SamplePages.ConnectedAnimations.Pages;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConnectedAnimationsPage : Page
    {
        public ConnectedAnimationsPage()
        {
            this.InitializeComponent();
            RootFrame.Navigate(typeof(FirstPage));
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            BackButton.Visibility = RootFrame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
        }

        private void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.SourcePageType == RootFrame.SourcePageType)
            {
                e.Cancel = true;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            RootFrame.GoBack();
        }
    }
}
