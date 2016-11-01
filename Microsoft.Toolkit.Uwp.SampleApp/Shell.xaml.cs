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
using Microsoft.Toolkit.Uwp.SampleApp.Pages;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public sealed partial class Shell
    {
        private const int RootGridColumnsMinWidth = 300;
        private const int RootGridColumnsDefaultMinWidth = 0;

        public static Shell Current { get; private set; }

        private bool _isPaneOpen;
        private Sample _currentSample;

        public bool DisplayWaitRing
        {
            set { waitRing.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Shell()
        {
            InitializeComponent();

            Current = this;
        }

        public void ShowInfoArea()
        {
            InfoAreaGrid.Visibility = Visibility.Visible;
            UpdateRootGridMinWidth();
            RootGrid.ColumnDefinitions[0].Width = new GridLength(2, GridUnitType.Star);
            RootGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
            RootGrid.RowDefinitions[1].Height = new GridLength(32);
            Splitter.Visibility = Visibility.Visible;
        }

        public void HideInfoArea()
        {
            InfoAreaGrid.Visibility = Visibility.Collapsed;
            RootGrid.ColumnDefinitions[1].Width = GridLength.Auto;
            RootGrid.RowDefinitions[1].Height = GridLength.Auto;
            _currentSample = null;
            CommandArea.Children.Clear();
            Splitter.Visibility = Visibility.Collapsed;
            RootGrid.ColumnDefinitions[0].MinWidth = RootGridColumnsDefaultMinWidth;
            RootGrid.ColumnDefinitions[1].MinWidth = RootGridColumnsDefaultMinWidth;
        }

        public void ShowOnlyHeader(string title)
        {
            Title.Text = title;
            HideInfoArea();
        }

        public async Task NavigateToSampleAsync(Sample sample)
        {
            var pageType = Type.GetType("Microsoft.Toolkit.Uwp.SampleApp.SamplePages." + sample.Type);

            if (pageType != null)
            {
                InfoAreaPivot.Items.Clear();
                ShowInfoArea();

                var propertyDesc = await sample.GetPropertyDescriptorAsync();
                DataContext = sample;
                Title.Text = sample.Name;

                NavigationFrame.Navigate(pageType, propertyDesc);

                _currentSample = sample;

                if (propertyDesc != null && propertyDesc.Options.Count > 0)
                {
                    InfoAreaPivot.Items.Add(PropertiesPivotItem);
                }

                if (sample.HasXAMLCode)
                {
                    XamlCodeRenderer.XamlSource = _currentSample.UpdatedXamlCode;

                    InfoAreaPivot.Items.Add(XamlPivotItem);

                    InfoAreaPivot.SelectedIndex = 0;
                }

                if (sample.HasCSharpCode)
                {
                    CSharpCodeRenderer.CSharpSource = await _currentSample.GetCSharpSourceAsync();
                    InfoAreaPivot.Items.Add(CSharpPivotItem);
                }

                if (sample.HasJavaScriptCode)
                {
                    JavaScriptCodeRenderer.CSharpSource = await _currentSample.GetJavaScriptSourceAsync();
                    InfoAreaPivot.Items.Add(JavaScriptPivotItem);
                }

                UpdateRootGridMinWidth();
            }
        }

        public void RegisterNewCommand(string name, RoutedEventHandler action)
        {
            var commandButton = new Button
            {
                Content = name,
                Margin = new Thickness(10),
                Foreground = Title.Foreground,
                MinWidth = 150
            };

            commandButton.Click += action;

            CommandArea.Children.Add(commandButton);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Get list of samples
            HamburgerMenu.ItemsSource = await Samples.GetCategoriesAsync();

            // Options
            HamburgerMenu.OptionsItemsSource = new[] { new Option { Glyph = "", Name = "About", PageType = typeof(About) } };

            HideInfoArea();

            NavigationFrame.Navigated += NavigationFrameOnNavigated;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        private void UpdateRootGridMinWidth()
        {
            if (ActualWidth > 2 * RootGridColumnsMinWidth)
            {
                RootGrid.ColumnDefinitions[0].MinWidth = RootGridColumnsMinWidth;
                RootGrid.ColumnDefinitions[1].MinWidth = RootGridColumnsMinWidth;
            }
            else
            {
                RootGrid.ColumnDefinitions[0].MinWidth = 0;
                RootGrid.ColumnDefinitions[1].MinWidth = 0;
            }
        }

        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            var states = VisualStateManager.GetVisualStateGroups(HamburgerMenu).FirstOrDefault();
            string currentState = states.CurrentState.Name;

            switch (currentState)
            {
                case "NarrowState":
                case "MediumState":
                    // If pane is open, close it
                    if (_isPaneOpen)
                    {
                        Grid.SetRowSpan(InfoAreaGrid, 1);
                        Grid.SetRow(InfoAreaGrid, 1);
                        _isPaneOpen = false;
                        ExpandButton.Content = "";
                    }
                    else
                    {
                        // ane is closed, so let's open it
                        Grid.SetRowSpan(InfoAreaGrid, 2);
                        Grid.SetRow(InfoAreaGrid, 0);
                        _isPaneOpen = true;
                        ExpandButton.Content = "";
                    }

                    break;

                case "WideState":
                    // If pane is open, close it
                    if (_isPaneOpen)
                    {
                        Grid.SetColumnSpan(InfoAreaGrid, 1);
                        Grid.SetColumn(InfoAreaGrid, 1);
                        _isPaneOpen = false;
                        ExpandButton.Content = "";
                    }
                    else
                    {
                        // Pane is closed, so let's open it
                        Grid.SetColumnSpan(InfoAreaGrid, 2);
                        Grid.SetColumn(InfoAreaGrid, 0);
                        _isPaneOpen = true;
                        ExpandButton.Content = "";
                    }

                    break;
            }
        }

        /// <summary>
        /// Called when [back requested] event is fired.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="backRequestedEventArgs">The <see cref="BackRequestedEventArgs"/> instance containing the event data.</param>
        private void OnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs)
        {
            if (NavigationFrame.CanGoBack)
            {
                backRequestedEventArgs.Handled = true;

                var previousPage = NavigationFrame.BackStack.Last();

                if (previousPage.SourcePageType == typeof(SamplePicker))
                {
                    HideInfoArea();
                }
                else
                {
                    ShowInfoArea();
                }

                NavigationFrame.GoBack();
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

        private void HamburgerMenu_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var category = e.ClickedItem as SampleCategory;

            if (category != null)
            {
                HideInfoArea();
                NavigationFrame.Navigate(typeof(SamplePicker), category);
            }
        }

        private void HamburgerMenu_OnOptionsItemClick(object sender, ItemClickEventArgs e)
        {
            var option = e.ClickedItem as Option;
            if (option != null && NavigationFrame.CurrentSourcePageType != option.PageType)
            {
                NavigationFrame.Navigate(option.PageType);
            }
        }

        private async void InfoAreaPivot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InfoAreaPivot.SelectedItem == PropertiesPivotItem)
            {
                return;
            }

            if (_currentSample == null)
            {
                return;
            }

            if (_currentSample.HasXAMLCode)
            {
                XamlCodeRenderer.XamlSource = _currentSample.UpdatedXamlCode;
            }

            if (_currentSample.HasCSharpCode)
            {
                CSharpCodeRenderer.CSharpSource = await _currentSample.GetCSharpSourceAsync();
            }

            if (_currentSample.HasJavaScriptCode)
            {
                JavaScriptCodeRenderer.JavaScriptSource = await _currentSample.GetJavaScriptSourceAsync();
            }
        }
    }
}
