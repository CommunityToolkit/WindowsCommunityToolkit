// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using UITests.App.Commands;
using UITests.App.Pages;

namespace UITests.App
{
    /// <summary>
    /// MainPage hosting all other test pages.
    /// </summary>
    public sealed partial class MainTestHost : IRecipient<RequestPageMessage>
    {
        private Assembly _executingAssembly = Assembly.GetExecutingAssembly();

        private TaskCompletionSource<bool> _loadingStateTask;

        public MainTestHost()
        {
            InitializeComponent();

            WeakReferenceMessenger.Default.Register<RequestPageMessage>(this);

            // Initialize Custom Commands for AppService
            VisualTreeHelperCommands.Initialize(DispatcherQueue, async () =>
            {
                var dpi = 0.0;
                await DispatcherQueue.EnqueueAsync(() =>
                {
                    dpi = XamlRoot.RasterizationScale;
                });
                return dpi;
            });
        }

        public void Receive(RequestPageMessage message)
        {
            // Reply with task back to so it can be properly awaited link:App.AppService.xaml.cs#L56
            message.Reply(OpenPage(message.PageName));
        }

        private async Task<bool> OpenPage(string pageName)
        {
            try
            {
                Log.Comment("Trying to Load Page: " + pageName);

                _loadingStateTask = new TaskCompletionSource<bool>();

                // Ensure we're on the UI thread as we'll be called from the AppService now.
                _ = DispatcherQueue.EnqueueAsync(() =>
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

        private void NavigationFrame_Navigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
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

        private void NavigationFrame_NavigationFailed(object sender, Microsoft.UI.Xaml.Navigation.NavigationFailedEventArgs e)
        {
            Log.Error("Failed to navigate to page {0}", e.SourcePageType.FullName);
            _loadingStateTask.SetResult(false);
        }

        private void GoBackInvokerButton_Click(object sender, RoutedEventArgs e)
        {
            Log.Comment("Go Back Clicked. Navigating to Page...");
            navigationFrame.Navigate(typeof(HomePage));
            Log.Comment("Navigated to Page.");
        }

        private void CloseAppInvokerButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Exit();
        }
    }
}