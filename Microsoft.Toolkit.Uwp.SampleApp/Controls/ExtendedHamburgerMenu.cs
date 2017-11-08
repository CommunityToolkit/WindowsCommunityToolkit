using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.SampleApp.Controls
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class ExtendedHamburgerMenu : HamburgerMenu
#pragma warning restore CS0618 // Type or member is obsolete
    {
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
                    samples = category.Samples;
                }
            }

            if (samples == null)
            {
                samples = (await Samples.GetCategoriesAsync()).FirstOrDefault()?.Samples;
            }

            if (samples == null)
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

        public async Task StartSearch(string startingText = "")
        {
            if (_searchBox == null || _searchBox.Visibility == Visibility.Visible)
            {
                return;
            }

            HideSamplePicker();
            _searchBox.Text = startingText;

            _searchButton.Visibility = Visibility.Collapsed;
            _searchBox.Visibility = Visibility.Visible;

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

            SetupSearch();
        }

        private void SetupSearch()
        {
            if (_searchBox != null)
            {
                _searchBox.LostFocus -= SearchBox_LostFocus;
                _searchBox.TextChanged -= SearchBox_TextChanged;
                _searchBox.KeyDown -= SearchBox_KeyDown;
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
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput)
            {
                return;
            }

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

            new ScaleAnimation() { To = "0, 1, 1", Duration = TimeSpan.FromMilliseconds(300) }.StartAnimation(_searchBox);

            await Task.Delay(300);

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
                    HideSamplePicker();
                }
                else
                {
                    ShowSamplePicker(category.Samples);
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

        private bool SetupSamplePicker()
        {
            if (_samplePickerGrid != null)
            {
                return true;
            }

            _samplePickerGrid = GetTemplateChild("SamplePickerGrid") as Grid;
            _samplePickerGridView = GetTemplateChild("SamplePickerGridView") as GridView;
            _contentShadow = GetTemplateChild("ContentShadow") as Border;

            if (_samplePickerGrid != null)
            {
                if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.XamlCompositionBrushBase"))
                {
                    AcrylicBrush myBrush = new AcrylicBrush
                    {
                        BackgroundSource = AcrylicBackgroundSource.Backdrop,
                        TintColor = Color.FromArgb(255, 0xF3, 0xF3, 0xF3),
                        FallbackColor = Color.FromArgb(216, 0xF3, 0xF3, 0xF3),
                        TintOpacity = 0.9
                    };

                    _samplePickerGrid.Background = myBrush;
                }
                else
                {
                    GetTemplateChild("SamplePickerGridBackground");
                }
            }

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
            var noop = SetHamburgerMenuSelection();

            if (_hamburgerButton != null && _hamburgerButton.Visibility == Visibility.Visible)
            {
                HideItemsInNarrowView();
            }
        }

        private void SamplePickerGridView_ChoosingItemContainer(Windows.UI.Xaml.Controls.ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            if (!AnimationHelper.IsImplicitHideShowSupported || args.ItemContainer != null)
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

                VisualEx.SetNormalizedCenterPoint(itemContainer, "0.5");

                animationCollection.StartAnimation(itemContainer);
            }

            itemContainer.Loaded -= this.ContainerItem_Loaded;
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
