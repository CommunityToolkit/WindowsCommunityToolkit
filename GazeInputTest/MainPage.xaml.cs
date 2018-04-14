//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

using Microsoft.Toolkit.Uwp.Input.Gaze;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GazeInputTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            ShowCursor.IsChecked = GazeApi.GetIsGazeCursorVisible(this);
        }

        private void OnGazePointerEvent(GazePointer sender, GazePointerEventArgs ea)
        {
            Dwell.Content = ea.PointerState.ToString();
        }

        private void Dwell_Click(object sender, RoutedEventArgs e)
        {
            Dwell.Content = "Clicked";
        }

        private void ShowCursor_Toggle(object sender, RoutedEventArgs e)
        {
            if (ShowCursor.IsChecked.HasValue)
            {
                GazeApi.SetIsGazeCursorVisible(this, ShowCursor.IsChecked.Value);
            }
        }

        int clickCount;

        private void OnLegacyInvoked(object sender, RoutedEventArgs e)
        {
            clickCount++;
            HowButton.Content = string.Format("{0}: Legacy click", clickCount);
        }

        private void OnGazeInvoked(object sender, GazeInvokedRoutedEventArgs e)
        {
            clickCount++;
            HowButton.Content = string.Format("{0}: Accessible click", clickCount);
            e.Handled = true;
        }
    }
}
