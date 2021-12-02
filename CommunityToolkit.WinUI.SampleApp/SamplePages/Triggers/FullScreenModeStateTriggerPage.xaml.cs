// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace CommunityToolkit.WinUI.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FullScreenModeStateTriggerPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FullScreenModeStateTriggerPage"/> class.
        /// </summary>
        public FullScreenModeStateTriggerPage()
        {
            InitializeComponent();
            SampleController.Current.RegisterNewCommand("Toggle Full Screen", ToggleFullScreenMode);
        }

        private void ToggleFullScreenMode(object sender, RoutedEventArgs e)
        {
            if (CoreWindow.GetForCurrentThread() == null)
            {
                return;
            }

            var view = ApplicationView.GetForCurrentView();
            if (view == null)
            {
                return;
            }

            var isFullScreenMode = view.IsFullScreenMode;

            if (isFullScreenMode)
            {
                view.ExitFullScreenMode();
            }
            else
            {
                view.TryEnterFullScreenMode();
            }
        }
    }
}