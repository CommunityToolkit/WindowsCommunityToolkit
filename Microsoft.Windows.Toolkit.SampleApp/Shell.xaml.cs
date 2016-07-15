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
using Microsoft.Windows.Toolkit.SampleApp.Pages;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Windows.Toolkit.SampleApp
{
    public sealed partial class Shell
    {
        public static Shell Current { get; private set; }

        private bool _isPaneOpen;
        private Sample _currentSample;

        private bool _sampleHasPropertyDescriptor;

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
            RootGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
            RootGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
        }

        public void HideInfoArea()
        {
            InfoAreaGrid.Visibility = Visibility.Collapsed;
            RootGrid.ColumnDefinitions[1].Width = GridLength.Auto;
            RootGrid.RowDefinitions[1].Height = GridLength.Auto;
            _currentSample = null;
        }

        public void ShowOnlyHeader(string title)
        {
            Title.Text = title;
            HideInfoArea();
        }

        public async Task NavigateToSampleAsync(Sample sample)
        {
            var pageType = Type.GetType("Microsoft.Windows.Toolkit.SampleApp.SamplePages." + sample.Type);

            if (pageType != null)
            {
                ShowInfoArea();

                var propertyDesc = await sample.GetPropertyDescriptorAsync();
                DataContext = sample;
                Title.Text = sample.Name;

                Properties.Visibility = (propertyDesc != null && propertyDesc.Options.Count > 0) ? Visibility.Visible : Visibility.Collapsed;

                NavigationFrame.Navigate(pageType, propertyDesc);

                _currentSample = sample;
                _sampleHasPropertyDescriptor = propertyDesc != null;

                if (_sampleHasPropertyDescriptor)
                {
                    CodeRenderer.XamlSource = _currentSample.UpdatedXamlCode;

                    if (!InfoAreaPivot.Items.Contains(PropertiesPivotItem))
                    {
                        InfoAreaPivot.Items.Insert(0, PropertiesPivotItem);
                    }

                    InfoAreaPivot.SelectedIndex = 0;
                }
                else
                {
                    CodeRenderer.CSharpSource = await _currentSample.GetCSharpSource();
                    if (InfoAreaPivot.Items.Contains(PropertiesPivotItem))
                    {
                        InfoAreaPivot.Items.Remove(PropertiesPivotItem);
                    }
                }
            }
        }

        public void RegisterNewCommand(string name, RoutedEventHandler action)
        {
            var commandButton = new Button
            {
                Content = name,
                Margin = new Thickness(10, 5, 10, 5),
                Foreground = Title.Foreground
            };

            commandButton.Click += action;
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
                HideInfoArea();
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
            if (option != null)
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

            if (_sampleHasPropertyDescriptor)
            {
                CodeRenderer.XamlSource = _currentSample.UpdatedXamlCode;
            }
            else
            {
                CodeRenderer.CSharpSource = await _currentSample.GetCSharpSource();
            }
        }
    }
}
