// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the viewport behavior.
    /// </summary>
    public sealed partial class ViewportBehaviorPage
    {
        private readonly ObservableCollection<string> _logs = new ObservableCollection<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewportBehaviorPage"/> class.
        /// </summary>
        public ViewportBehaviorPage()
        {
            InitializeComponent();

            LogsItemsControl.ItemsSource = _logs;
            EffectElement.Fade(value: 0, duration: 0).Start();
        }

        private async void AddLog(string log)
        {
            _logs.Add(log);
            await Task.Yield(); // wait for layout updated.
            LogsScrollViewer.ChangeView(null, LogsScrollViewer.ScrollableHeight, null);
        }

        private void ClearLogsButton_Click(object sender, RoutedEventArgs e)
        {
            _logs.Clear();
        }

        private async void EffectHost_EnteredViewport(object sender, EventArgs e)
        {
            AddLog("Entered viewport");

            await EffectElement.Fade(value: 1, duration: 2000).StartAsync();
        }

        private async void EffectHost_ExitedViewport(object sender, EventArgs e)
        {
            AddLog("Exited viewport");

            EffectElement.Source = null;
            await EffectElement.Fade(value: 0, duration: 0).StartAsync();
        }

        private void EffectHost_EnteringViewport(object sender, EventArgs e)
        {
            AddLog("Entering viewport");

            EffectElement.Source = new BitmapImage(new Uri("ms-appx:///Assets/ToolkitLogo.png"));
        }

        private void EffectHost_ExitingViewport(object sender, EventArgs e)
        {
            AddLog("Exiting viewport");
        }
    }
}