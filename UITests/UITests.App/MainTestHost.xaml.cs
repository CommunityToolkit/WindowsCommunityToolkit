// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UITests.App.Pages;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace UITests.App
{
    /// <summary>
    /// MainPage hosting all other test pages.
    /// </summary>
    public sealed partial class MainTestHost
    {
        private DispatcherQueue _queue;

        private Assembly _executingAssembly = Assembly.GetExecutingAssembly();

        public MainTestHost()
        {
            InitializeComponent();
            ((App)Application.Current).host = this;
            _queue = DispatcherQueue.GetForCurrentThread();
        }

        internal bool OpenPage(string pageName)
        {
            try
            {
                Log.Comment("Trying to Load Page: " + pageName);

                // Ensure we're on the UI thread as we'll be called from the AppService now.
                _queue.EnqueueAsync(() =>
                {
                    navigationFrame.Navigate(FindPageType(pageName));
                });
            }
            catch (Exception e)
            {
                Log.Error("Exception Finding Page {0}: {1} ", pageName, e.Message);
                return false;
            }

            return true;
        }

        private Type FindPageType(string pageName)
        {
            try
            {
                return _executingAssembly.GetType("UITests.App.Pages." + pageName);
            }
            catch (Exception e)
            {
                Log.Error("Exception Finding Page {0}: {1} ", pageName, e.Message);
            }

            return null;
        }

        private void NavigationFrame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            Log.Comment("Navigated to Page {0}", e.SourcePageType.FullName);
        }

        private void NavigationFrame_NavigationFailed(object sender, Windows.UI.Xaml.Navigation.NavigationFailedEventArgs e)
        {
            Log.Error("Failed to navigate to page {0}", e.SourcePageType.FullName);
        }
    }
}
