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
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public sealed partial class Shell
    {
        public static Shell Current { get; private set; }

        private bool _isPaneOpen;
        private Sample _currentSample;
        private AutoSuggestBox _searchBox;

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
        }

        public void ShowOnlyHeader(string title)
        {
            Title.Text = title;
            HideInfoArea();
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
            var pageType = Type.GetType("Microsoft.Toolkit.Uwp.SampleApp.SamplePages." + sample.Type);

            if (pageType != null)
            {
                InfoAreaPivot.Items.Clear();

                NavigationFrame.Navigate(pageType, sample.Name);
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
            var sampleCategories = (await Samples.GetCategoriesAsync()).ToList();
            var moreResources = sampleCategories.Last(); // Remove the last one because it is a specific case
            sampleCategories.Remove(moreResources);

            HamburgerMenu.ItemsSource = sampleCategories;

            // Options
            HamburgerMenu.OptionsItemsSource = new[]
            {
                new Option { Glyph = "", Name = "More resources", PageType = typeof(About), Tag = moreResources },
                new Option { Glyph = "", Name = "About", PageType = typeof(About) }
            };

            HideInfoArea();

            NavigationFrame.Navigating += NavigationFrame_Navigating;
            NavigationFrame.Navigated += NavigationFrameOnNavigated;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

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

        private async void NavigationFrame_Navigating(object sender, NavigatingCancelEventArgs navigationEventArgs)
        {
            SampleCategory category;
            if (navigationEventArgs.SourcePageType == typeof(SamplePicker) || navigationEventArgs.Parameter == null)
            {
                DataContext = null;
                category = navigationEventArgs.Parameter as SampleCategory;

                if (category != null)
                {
                    TrackingManager.TrackPage($"{navigationEventArgs.SourcePageType.Name} - {category.Name}");
                }

                HideInfoArea();
            }
            else
            {
                TrackingManager.TrackPage(navigationEventArgs.SourcePageType.Name);
                ShowInfoArea();

                var sampleName = navigationEventArgs.Parameter.ToString();
                var sample = await Samples.GetSampleByName(sampleName);

                if (sample == null)
                {
                    HideInfoArea();
                    return;
                }

                category = await Samples.GetCategoryBySample(sample);

                var propertyDesc = sample.PropertyDescriptor;

                DataContext = sample;

                InfoAreaPivot.Items.Clear();

                if (propertyDesc != null)
                {
                    NavigationFrame.DataContext = propertyDesc.Expando;
                }

                Title.Text = sample.Name;

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

                if (!string.IsNullOrEmpty(sample.CodeUrl))
                {
                    GitHub.NavigateUri = new Uri(sample.CodeUrl);
                    GitHub.Visibility = Visibility.Visible;
                }
                else
                {
                    GitHub.Visibility = Visibility.Collapsed;
                }

                if (sample.HasDocumentation)
                {
                    InfoAreaPivot.Items.Add(DocumentationPivotItem);
                    DocumentationTextblock.Text = await _currentSample.GetDocumentationAsync();
                }

                if (InfoAreaPivot.Items.Count == 0)
                {
                    HideInfoArea();
                }
            }

            if (category == null && navigationEventArgs.SourcePageType == typeof(SamplePicker))
            {
                // This is a search
                HamburgerMenu.SelectedItem = null;
                HamburgerMenu.SelectedOptionsItem = null;
            }
            else
            {
                if (HamburgerMenu.Items.Contains(category))
                {
                    HamburgerMenu.SelectedItem = category;
                    HamburgerMenu.SelectedOptionsItem = null;
                }
                else
                {
                    HamburgerMenu.SelectedItem = null;
                    HamburgerMenu.SelectedOptionsIndex = category != null ? 0 : 1;
                }
            }
        }

        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            ExpandOrCloseProperties();
        }

        private void ExpandOrCloseProperties()
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
                        // pane is closed, so let's open it
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
            if (backRequestedEventArgs.Handled)
            {
                return;
            }

            if (NavigationFrame.CanGoBack)
            {
                backRequestedEventArgs.Handled = true;

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

            if (_isPaneOpen)
            {
                ExpandOrCloseProperties();
            }
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
            if (option == null)
            {
                return;
            }

            if (option.Tag != null)
            {
                NavigationFrame.Navigate(typeof(SamplePicker), option.Tag);
                return;
            }

            if (NavigationFrame.CurrentSourcePageType != option.PageType)
            {
                NavigationFrame.Navigate(option.PageType);
            }
        }

        private async void InfoAreaPivot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InfoAreaPivot.SelectedItem != null)
            {
                var sample = DataContext as Sample;

                if (sample != null)
                {
                    TrackingManager.TrackEvent("PropertyGrid", (InfoAreaPivot.SelectedItem as FrameworkElement)?.Name, sample.Name);
                }
            }

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

        private async void DocumentationTextblock_OnLinkClicked(object sender, LinkClickedEventArgs e)
        {
            TrackingManager.TrackEvent("Link", e.Link);
            await Launcher.LaunchUriAsync(new Uri(e.Link));
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DataContext == null)
            {
                return;
            }
        }

        private void GitHub_OnClick(object sender, RoutedEventArgs e)
        {
            TrackingManager.TrackEvent("Link", GitHub.NavigateUri.ToString());
        }

        private async void UpdateSearchSuggestions()
        {
            _searchBox.ItemsSource = (await Samples.FindSamplesByName(_searchBox.Text)).OrderBy(s => s.Name);
        }

        private void ConnectToSearch()
        {
            var searchButton = HamburgerMenu.FindDescendantByName("SearchButton") as Button;
            _searchBox = HamburgerMenu.FindDescendantByName("SearchBox") as AutoSuggestBox;

            if (_searchBox == null || searchButton == null)
            {
                return;
            }

            searchButton.Click += async (sender, args) =>
            {
                HamburgerMenu.IsPaneOpen = true;
                _searchBox.Text = string.Empty;

                // We need to wait for the textbox to be created to focus it (only first time).
                TextBox innerTextbox = null;

                do
                {
                    innerTextbox = _searchBox.FindDescendant<TextBox>();
                    innerTextbox?.Focus(FocusState.Programmatic);

                    if (innerTextbox == null)
                    {
                        await Task.Delay(150);
                    }
                }
                while (innerTextbox == null);
            };

            _searchBox.DisplayMemberPath = "Name";
            _searchBox.TextMemberPath = "Name";

            _searchBox.QuerySubmitted += (sender, args) =>
            {
                NavigationFrame.Navigate(typeof(SamplePicker), _searchBox.Text);
            };

            _searchBox.TextChanged += (sender, args) =>
            {
                if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput)
                {
                    return;
                }

                UpdateSearchSuggestions();
            };
        }

        private void Shell_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Connect to search UI
            ConnectToSearch();
        }
    }
}
