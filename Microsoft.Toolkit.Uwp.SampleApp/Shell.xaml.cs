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

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.SampleApp.Pages;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public sealed partial class Shell
    {
        public static Shell Current { get; private set; }

        public Shell()
        {
            InitializeComponent();

            Current = this;
        }

        /// <summary>
        /// Navigates to a Sample via a deep link.
        /// </summary>
        /// <param name="deepLink">The deep link. Specified as protocol://[collectionName]?sample=[sampleName]</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task NavigateToSampleAsync(string deepLink)
        {
            var parser = DeepLinkParser.Create(deepLink);
            var targetSample = await Samples.GetSampleByName(parser["sample"]);
            if (targetSample != null)
            {
                NavigateToSample(targetSample);
            }
        }

        public void NavigateToSample(Sample sample)
        {
            NavigationFrame.Navigate(typeof(SampleController), sample);
        }

        public Task StartSearch(string startingText = "")
        {
            return HamburgerMenu.StartSearch(startingText);
        }

        public void SetTitles(string title)
        {
            HamburgerMenu.Title = title;
            ApplicationView.SetTitle(this, title);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationFrame.Navigating += NavigationFrame_Navigating;
            NavigationFrame.Navigated += NavigationFrameOnNavigated;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            // Get list of samples
            var sampleCategories = (await Samples.GetCategoriesAsync()).ToList();

            HamburgerMenu.ItemsSource = sampleCategories;

            // Options
            HamburgerMenu.OptionsItemsSource = new[]
            {
                new Option { Glyph = "\xE10F", Name = "About", PageType = typeof(About) }
            };

            ClearTitle();
            NavigationFrame.Navigate(typeof(About));

            if (!string.IsNullOrWhiteSpace(e?.Parameter?.ToString()))
            {
                var parser = DeepLinkParser.Create(e.Parameter.ToString());
                var targetSample = await Sample.FindAsync(parser.Root, parser["sample"]);
                if (targetSample != null)
                {
                    NavigateToSample(targetSample);
                }
            }
        }

        private void NavigationFrame_Navigating(object sender, NavigatingCancelEventArgs navigationEventArgs)
        {
            var name = navigationEventArgs.SourcePageType.Name;
            TrackingManager.TrackPage(name);
        }

        private void ClearTitle()
        {
            SetTitles(string.Empty);
        }

        /// <summary>
        /// Called when [back requested] event is fired.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="backRequestedEventArgs">The <see cref="BackRequestedEventArgs"/> instance containing the event data.</param>
        private void OnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs)
        {
            if (backRequestedEventArgs.Handled)
            {
                return;
            }

            if (NavigationFrame.CanGoBack)
            {
                NavigationFrame.GoBack();
                backRequestedEventArgs.Handled = true;
            }
        }

        /// <summary>
        /// When the frame has navigated this method is called.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="navigationEventArgs">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        private void NavigationFrameOnNavigated(object sender, NavigationEventArgs navigationEventArgs)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = NavigationFrame.CanGoBack
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
        }

        private void HamburgerMenu_OnOptionsItemClick(object sender, ItemClickEventArgs e)
        {
            var option = e.ClickedItem as Option;
            if (option == null)
            {
                return;
            }

            if (NavigationFrame.CurrentSourcePageType != option.PageType)
            {
                NavigationFrame.Navigate(option.PageType);
            }

            HamburgerMenu.IsPaneOpen = false;

            var expanders = HamburgerMenu.FindDescendants<Expander>();
            foreach (var expander in expanders)
            {
                expander.IsExpanded = false;
            }
        }

        private void HamburgerMenu_SamplePickerItemClick(object sender, ItemClickEventArgs e)
        {
            NavigateToSample(e.ClickedItem as Sample);
        }
    }
}