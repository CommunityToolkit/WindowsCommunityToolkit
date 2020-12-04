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
using Windows.UI.Xaml.Media.Animation;

namespace UITests.App
{
    /// <summary>
    /// MainPage hosting all other test pages.
    /// </summary>
    public sealed partial class MainTestHost
    {
        private DispatcherQueue _queue;

        private Assembly _executingAssembly = Assembly.GetExecutingAssembly();

        private TaskCompletionSource<bool> _loadingStateTask;

        public MainTestHost()
        {
            InitializeComponent();
            ((App)Application.Current).host = this;
            _queue = DispatcherQueue.GetForCurrentThread();
        }

        // TODO: we should better expose how to control the MainTestHost vs. making this internal.
        internal async Task<bool> OpenPage(string pageName)
        {
            try
            {
                Log.Comment("Trying to Load Page: " + pageName);

                _loadingStateTask = new TaskCompletionSource<bool>();

                // Ensure we're on the UI thread as we'll be called from the AppService now.
                _ = _queue.EnqueueAsync(() =>
                {
                    // Navigate without extra animations
                    navigationFrame.Navigate(FindPageType(pageName), new SuppressNavigationTransitionInfo());
                });

                // Wait for load to complete
                await _loadingStateTask.Task;
            }
            catch (Exception e)
            {
                Log.Error("Exception Loading Page {0}: {1} ", pageName, e.Message);
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
            if (e.Content is Page page)
            {
                if (page.IsLoaded)
                {
                    Log.Comment("Loaded Page {0}", e.SourcePageType.FullName);
                    _loadingStateTask.SetResult(true);
                }
                else
                {
                    page.Loaded += this.Page_Loaded;
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var page = sender as Page;

            page.Loaded -= Page_Loaded;

            Log.Comment("Loaded Page (E) {0}", page.GetType().FullName);
            _loadingStateTask.SetResult(true);
        }

        private void NavigationFrame_NavigationFailed(object sender, Windows.UI.Xaml.Navigation.NavigationFailedEventArgs e)
        {
            Log.Error("Failed to navigate to page {0}", e.SourcePageType.FullName);
            _loadingStateTask.SetResult(false);
        }
    }
}
