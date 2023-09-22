// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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
            this.Loaded += ThemeListenerPage_Loaded;
            SampleController.Current.ThemeChanged += Current_ThemeChanged;
        }

        private void Current_ThemeChanged(object sender, Models.ThemeChangedArgs e)
        {
            UpdateThemeState();
        }

        private void ThemeListenerPage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateThemeState();
        }

        private void UpdateThemeState()
        {
            SystemTheme.Text = SampleController.Current.ThemeListener.CurrentThemeName;
            CurrentTheme.Text = SampleController.Current.GetCurrentTheme().ToString();
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Loaded -= ThemeListenerPage_Loaded;
            SampleController.Current.ThemeChanged -= Current_ThemeChanged;
        }
    }
}