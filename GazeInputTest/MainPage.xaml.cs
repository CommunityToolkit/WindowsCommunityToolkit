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

using Microsoft.Toolkit.Uwp.Input.GazeInteraction;
using System;
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

            ShowCursor.IsChecked = GazeInput.GetIsCursorVisible(this);
        }

        private void OnStateChanged(object sender, StateChangedEventArgs ea)
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
                GazeInput.SetIsCursorVisible(this, ShowCursor.IsChecked.Value);
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

        private void OnInvokeProgress(object sender, GazeProgressEventArgs e)
        {
            if (e.State == GazeProgressState.Progressing)
            {
                ProgressShow.Value = (100.0 * e.ElapsedTicks) / e.TriggerTicks;
            }
            ProgressShow.IsIndeterminate = e.State == GazeProgressState.Complete;
            e.Handled = true;
        }
    }
}
