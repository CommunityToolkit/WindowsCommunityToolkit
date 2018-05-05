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

using Microsoft.Toolkit.Uwp.UI.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ThemeListenerPage : Page
    {
        public ThemeListenerPage()
        {
            this.InitializeComponent();
            Listener = new ThemeListener();
            this.Loaded += ThemeListenerPage_Loaded;
            Listener.ThemeChanged += Listener_ThemeChanged;
            Shell.Current.ThemeChanged += Current_ThemeChanged;
        }

        private void Current_ThemeChanged(object sender, Models.ThemeChangedArgs e)
        {
            UpdateThemeState();
        }

        private void ThemeListenerPage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateThemeState();
        }

        private void Listener_ThemeChanged(ThemeListener sender)
        {
            UpdateThemeState();
        }

        private void UpdateThemeState()
        {
            SystemTheme.Text = Listener.CurrentThemeName;
            CurrentTheme.Text = Shell.Current.GetCurrentTheme().ToString();
        }

        public ThemeListener Listener { get; }
    }
}