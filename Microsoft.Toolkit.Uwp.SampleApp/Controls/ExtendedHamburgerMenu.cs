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
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.SampleApp.Controls
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class ExtendedHamburgerMenu : HamburgerMenu
#pragma warning restore CS0618 // Type or member is obsolete
    {
        private static readonly bool _isCreatorsUpdateOrAbove = ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 4);

        private Button _hamburgerButton;
        private ListView _buttonsListView;
        private ListView _optionsListView;
        private Grid _samplePickerGrid;
        private GridView _samplePickerGridView;
        private Border _contentShadow;
        private Grid _searchGrid;
        private TextBlock _titleTextBlock;
        private AutoSuggestBox _searchBox;
        private Button _searchButton;

        private Canvas _moreInfoCanvas;
        private FrameworkElement _moreInfoContent;
        private Image _moreInfoImage;

        /// <summary>
        /// Event raised when an item is clicked
        /// </summary>
        public event ItemClickEventHandler SamplePickerItemClick;

        private Sample _currentSample;

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ExtendedHamburgerMenu), new PropertyMetadata(string.Empty));

        public Sample CurrentSample
        {
            get
            {
                return _currentSample;
            }

            set
            {
                _currentSample = value;
                var noop = SetHamburgerMenuSelection();
            }
        }

        public void HideSamplePicker()
        {
            if (SetupSamplePicker())
            {
                _samplePickerGrid.Visibility = Visibility.Collapsed;
            }

            var noop = SetHamburgerMenuSelection();
        }

        public async void ShowSamplePicker(Sample[] samples = null)
        {
            if (!SetupSamplePicker())
            {
                return;
            }

            if (samples == null && _currentSample != null)
            {
                var category = await Samples.GetCategoryBySample(_currentSample);
                if (category != null)
                {
                    samples = category.Samples.ToArray();
                }
            }

            if (samples == null)
            {
                samples = (await Samples.GetCategoriesAsync()).FirstOrDefault()?.Samples?.ToArray();
            }

            if (samples == null)
            {
                return;
            }

            if (_samplePickerGrid.Visibility == Visibility.Visible &&
                _samplePickerGridView.ItemsSource is Sample[] currentSamples &&
                currentSamples.Count() == samples.Count() &&
                currentSamples.Except(samples).Count() == 0)
            {
                return;
            }

            _samplePickerGridView.ItemsSource = samples;

            if (_currentSample != null && samples.Contains(_currentSample))
            {
                _samplePickerGridView.SelectedItem = _currentSample;
            }
            else
            {
                _samplePickerGridView.SelectedItem = null;
            }

            _samplePickerGrid.Visibility = Visibility.Visible;
        }

        public async Task StartSearch(string startingText = null)
        {
            if (_searchBox == null || _searchBox.Visibility == Visibility.Visible)
            {
                return;
            }

            var currentSearchText = _searchBox.Text;

            _searchBox.Text = string.Empty;

            if (!string.IsNullOrWhiteSpace(startingText))
            {
                _searchBox.Text = startingText;
            }
            else
            {
                _searchBox.Text = currentSearchText;
            }

            _searchButton.Visibility = Visibility.Collapsed;
            _searchBox.Visibility = Visibility.Visible;

            _searchBox.Focus(FocusState.Programmatic);

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
        }

        protected override void OnApplyTemplate()
        {
            Window.Current.CoreWindow.CharacterReceived -= CoreWindow_CharacterReceived;
            Window.Current.CoreWindow.CharacterReceived += CoreWindow_CharacterReceived;

            base.OnApplyTemplate();

            if (_hamburgerButton != null)
            {
                _hamburgerButton.Click -= HamburgerButton_Click;
            }

            SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequested;
            ItemClick -= ExtendedHamburgerMenu_ItemClick;
            OptionsItemClick -= ExtendedHamburgerMenu_OptionsItemClick;

            _hamburgerButton = GetTemplateChild("HamburgerButton") as Button;
            _optionsListView = GetTemplateChild("OptionsListView") as ListView;
            _searchGrid = GetTemplateChild("SearchGrid") as Grid;
            _titleTextBlock = GetTemplateChild("TitleTextBlock") as TextBlock;
            _buttonsListView = GetTemplateChild("ButtonsListView") as ListView;

            if (_hamburgerButton != null)
            {
                _hamburgerButton.Click += HamburgerButton_Click;
            }

            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            ItemClick += ExtendedHamburgerMenu_ItemClick;
            OptionsItemClick += ExtendedHamburgerMenu_OptionsItemClick;

            SetupMoreInfo();
            SetupSearch();
        }

        private void CoreWindow_CharacterReceived(CoreWindow sender, CharacterReceivedEventArgs args)
        {
            if (args.KeyCode == 27)
            {
                HideSamplePicker();
            }
        }

        private void SetupMoreInfo()
        {
            if (_moreInfoCanvas != null)
            {
                _moreInfoCanvas.Tapped -= MoreInfoCanvas_Tapped;
                SizeChanged += OnSizeChanged;
            }

            _moreInfoCanvas = GetTemplateChild("MoreInfoCanvas") as Canvas;
            _moreInfoContent = GetTemplateChild("MoreInfoContent") as FrameworkElement;
            _moreInfoImage = GetTemplateChild("MoreInfoImage") as Image;

            if (_moreInfoCanvas != null)
            {
                _moreInfoCanvas.Tapped += MoreInfoCanvas_Tapped;
                SizeChanged += OnSizeChanged;
            }
        }

        private bool SetupSamplePicker()
        {
            if (_samplePickerGrid != null)
            {
                return true;
            }

            _samplePickerGrid = GetTemplateChild("SamplePickerGrid") as Grid;
            _samplePickerGridView = GetTemplateChild("SamplePickerGridView") as GridView;
            _contentShadow = GetTemplateChild("ContentShadow") as Border;

            if (_samplePickerGridView != null)
            {
                _samplePickerGridView.ItemClick += SamplePickerGridView_ItemClick;
                _samplePickerGridView.ChoosingItemContainer += SamplePickerGridView_ChoosingItemContainer;
            }

            if (_contentShadow != null)
            {
                _contentShadow.Tapped += ContentShadow_Tapped;
            }

            return _samplePickerGrid != null;
        }

        private void SetupSearch()
        {
            if (_searchBox != null)
            {
                _searchBox.LostFocus -= SearchBox_LostFocus;
                _searchBox.TextChanged -= SearchBox_TextChanged;
                _searchBox.KeyDown -= SearchBox_KeyDown;
                _searchBox.QuerySubmitted -= SearchBox_QuerySubmitted;
            }

            if (_searchButton != null)
            {
                _searchButton.Click -= SearchButton_Click;
                _searchButton.GotFocus -= SearchButton_GotFocus;
            }

            _searchBox = GetTemplateChild("SearchBox") as AutoSuggestBox;
            _searchButton = GetTemplateChild("SearchButton") as Button;

            if (_searchBox == null || _searchButton == null)
            {
                return;
            }

            _searchBox.LostFocus += SearchBox_LostFocus;
            _searchBox.TextChanged += SearchBox_TextChanged;
            _searchBox.KeyDown += SearchBox_KeyDown;
            _searchBox.QuerySubmitted += SearchBox_QuerySubmitted;

            _searchButton.Click += SearchButton_Click;
            _searchButton.GotFocus += SearchButton_GotFocus;

            _searchBox.DisplayMemberPath = "Name";
            _searchBox.TextMemberPath = "Name";
        }

        private void SearchBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Down)
            {
                if (_samplePickerGrid.Visibility == Visibility.Visible)
                {
                    _samplePickerGridView.Focus(FocusState.Keyboard);
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var t = StartSearch();
        }

        private void SearchButton_GotFocus(object sender, RoutedEventArgs e)
        {
            var t = StartSearch();
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            UpdateSearchSuggestions();
        }

        private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            UpdateSearchSuggestions();
        }

        private async void UpdateSearchSuggestions()
        {
            var samples = (await Samples.FindSamplesByName(_searchBox.Text)).OrderBy(s => s.Name).ToArray();
            if (samples.Count() > 0)
            {
                ShowSamplePicker(samples);
            }
            else
            {
                HideSamplePicker();
            }
        }

        private async void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _searchButton.Visibility = Visibility.Visible;

            if (_isCreatorsUpdateOrAbove)
            {
                new ScaleAnimation() { To = "0, 1, 1", Duration = TimeSpan.FromMilliseconds(300) }.StartAnimation(_searchBox);

                await Task.Delay(300);
            }

            _searchBox.Visibility = Visibility.Collapsed;
        }

        private void ExtendedHamburgerMenu_OptionsItemClick(object sender, ItemClickEventArgs e)
        {
            HideSamplePicker();
        }

        private void ExtendedHamburgerMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!SetupSamplePicker())
            {
                return;
            }

            if (e.ClickedItem is SampleCategory category)
            {
                if (_samplePickerGrid.Visibility != Visibility.Collapsed && SelectedItem == e.ClickedItem)
                {
                    if (_hamburgerButton != null && _hamburgerButton.Visibility == Visibility.Visible)
                    {
                        HideItemsInNarrowView();
                    }
                    else
                    {
                        HideSamplePicker();
                    }
                }
                else
                {
                    ShowSamplePicker(category.Samples.ToArray());
                }
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (SetupSamplePicker() && _samplePickerGrid.Visibility == Visibility.Visible)
            {
                HideSamplePicker();
                e.Handled = true;
            }
        }

        private async Task SetHamburgerMenuSelection()
        {
            if (_currentSample != null)
            {
                var category = await Samples.GetCategoryBySample(_currentSample);

                if (Items.Contains(category))
                {
                    SelectedItem = category;
                    SelectedOptionsItem = null;
                }
            }
            else
            {
                SelectedItem = null;
                SelectedOptionsIndex = 0;
            }
        }

        private void ContentShadow_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            HideSamplePicker();

            if (_hamburgerButton != null && _hamburgerButton.Visibility == Visibility.Visible)
            {
                HideItemsInNarrowView();
            }
        }

        private void SamplePickerGridView_ChoosingItemContainer(Windows.UI.Xaml.Controls.ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            if (args.ItemContainer != null)
            {
                return;
            }

            GridViewItem container = (GridViewItem)args.ItemContainer ?? new GridViewItem();
            container.Loaded += ContainerItem_Loaded;
            container.PointerEntered += ItemContainer_PointerEntered;
            container.PointerExited += ItemContainer_PointerExited;

            args.ItemContainer = container;
        }

        private void ContainerItem_Loaded(object sender, RoutedEventArgs e)
        {
            var itemsPanel = (ItemsWrapGrid)_samplePickerGridView.ItemsPanelRoot;
            var itemContainer = (GridViewItem)sender;
            itemContainer.Loaded -= this.ContainerItem_Loaded;

            var button = itemContainer.FindDescendant<Button>();
            if (button != null)
            {
                button.Click -= MoreInfoClicked;
                button.Click += MoreInfoClicked;
            }

            if (!_isCreatorsUpdateOrAbove)
            {
                return;
            }

            var itemIndex = _samplePickerGridView.IndexFromContainer(itemContainer);

            var referenceIndex = itemsPanel.FirstVisibleIndex;

            if (_samplePickerGridView.SelectedIndex >= 0)
            {
                referenceIndex = _samplePickerGridView.SelectedIndex;
            }

            var relativeIndex = Math.Abs(itemIndex - referenceIndex);

            if (itemContainer.Content != CurrentSample && itemIndex >= 0 && itemIndex >= itemsPanel.FirstVisibleIndex && itemIndex <= itemsPanel.LastVisibleIndex)
            {
                var staggerDelay = TimeSpan.FromMilliseconds(relativeIndex * 30);

                var animationCollection = new AnimationCollection()
                {
                    new OpacityAnimation() { From = 0, To = 1, Duration = TimeSpan.FromMilliseconds(400), Delay = staggerDelay, SetInitialValueBeforeDelay = true },
                    new ScaleAnimation() { From = "0.9", To = "1", Duration = TimeSpan.FromMilliseconds(400), Delay = staggerDelay }
                };

                VisualExtensions.SetNormalizedCenterPoint(itemContainer, "0.5");

                animationCollection.StartAnimation(itemContainer);
            }
        }

        private void MoreInfoClicked(object sender, RoutedEventArgs e)
        {
            if (_moreInfoContent == null)
            {
                return;
            }

            var button = (Button)sender;
            var sample = button.DataContext as Sample;

            var container = button.FindAscendant<GridViewItem>();
            if (container == null)
            {
                return;
            }

            var point = container.TransformToVisual(this).TransformPoint(new Windows.Foundation.Point(0, 0));

            var x = point.X - ((_moreInfoContent.Width - container.ActualWidth) / 2);
            var y = point.Y - ((_moreInfoContent.Height - container.ActualHeight) / 2);

            x = Math.Max(x, 10);
            x = Math.Min(x, ActualWidth - _moreInfoContent.Width - 10);

            y = Math.Max(y, 10);
            y = Math.Min(y, ActualHeight - _moreInfoContent.Height - 10);

            Canvas.SetLeft(_moreInfoContent, x);
            Canvas.SetTop(_moreInfoContent, y);

            var centerX = (point.X + (container.ActualWidth / 2)) - x;
            var centerY = (point.Y + (container.ActualHeight / 2)) - y;

            VisualExtensions.SetCenterPoint(_moreInfoContent, new Vector3((float)centerX, (float)centerY, 0).ToString());

            // _samplePickerGridView.PrepareConnectedAnimation("sample_icon", sample, "SampleIcon");
            _moreInfoContent.DataContext = sample;
            _moreInfoCanvas.Visibility = Visibility.Visible;

            // var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("sample_icon");
            // var result = animation.TryStart(_moreInfoImage);
        }

        private void HideMoreInfo()
        {
            if (_isCreatorsUpdateOrAbove && _moreInfoImage != null && _moreInfoContent.DataContext != null)
            {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("sample_icon", _moreInfoImage);
            }

            _moreInfoCanvas.Visibility = Visibility.Collapsed;

            if (_isCreatorsUpdateOrAbove && _moreInfoImage != null && _moreInfoContent.DataContext != null)
            {
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("sample_icon");
                var t = _samplePickerGridView.TryStartConnectedAnimationAsync(animation, _moreInfoContent.DataContext, "SampleIcon");
            }

            _moreInfoContent.DataContext = null;
        }

        private void MoreInfoCanvas_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            HideMoreInfo();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            HideMoreInfo();
        }

        private void ItemContainer_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var panel = (sender as FrameworkElement).FindDescendant<DropShadowPanel>();
            if (panel != null)
            {
                var animation = new OpacityAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(1200) };
                animation.StartAnimation(panel);

                var parentAnimation = new ScaleAnimation() { To = "1", Duration = TimeSpan.FromMilliseconds(1200) };
                parentAnimation.StartAnimation(panel.Parent as UIElement);
            }
        }

        private void ItemContainer_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                var panel = (sender as FrameworkElement).FindDescendant<DropShadowPanel>();
                if (panel != null)
                {
                    panel.Visibility = Visibility.Visible;
                    var animation = new OpacityAnimation() { To = 1, Duration = TimeSpan.FromMilliseconds(600) };
                    animation.StartAnimation(panel);

                    var parentAnimation = new ScaleAnimation() { To = "1.1", Duration = TimeSpan.FromMilliseconds(600) };
                    parentAnimation.StartAnimation(panel.Parent as UIElement);
                }
            }
        }

        private void SamplePickerGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            HideSamplePicker();
            SamplePickerItemClick?.Invoke(this, e);

            if (_hamburgerButton != null && _hamburgerButton.Visibility == Visibility.Visible)
            {
                HideItemsInNarrowView();
            }
        }

        private void HamburgerButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_buttonsListView != null)
            {
                if (_buttonsListView.Visibility == Visibility.Collapsed)
                {
                    ExpandItemsInNarrowView();
                }
                else
                {
                    HideItemsInNarrowView();
                }
            }
        }

        private void ExpandItemsInNarrowView()
        {
            _buttonsListView.Visibility = Visibility.Visible;
            if (_optionsListView != null)
            {
                _optionsListView.Visibility = Visibility.Collapsed;
            }

            if (_searchGrid != null)
            {
                _searchGrid.Visibility = Visibility.Collapsed;
            }

            if (_titleTextBlock != null)
            {
                _titleTextBlock.Visibility = Visibility.Collapsed;
            }

            ShowSamplePicker();

            new RotationInDegreesAnimation() { To = 90, Duration = TimeSpan.FromMilliseconds(300) }.StartAnimation(_hamburgerButton.Content as UIElement);
        }

        private void HideItemsInNarrowView()
        {
            _buttonsListView.Visibility = Visibility.Collapsed;
            if (_optionsListView != null)
            {
                _optionsListView.Visibility = Visibility.Visible;
            }

            if (_searchGrid != null)
            {
                _searchGrid.Visibility = Visibility.Visible;
            }

            if (_titleTextBlock != null)
            {
                _titleTextBlock.Visibility = Visibility.Visible;
            }

            HideSamplePicker();
            new RotationInDegreesAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(300) }.StartAnimation(_hamburgerButton.Content as UIElement);
        }
    }
}
