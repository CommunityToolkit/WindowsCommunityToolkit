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
        private Grid _samplePickerGrid;
        private GridView _samplePickerGridView;
        private Border _contentShadow;

        /// <summary>
        /// Event raised when an item is clicked
        /// </summary>
        public event ItemClickEventHandler SamplePickerItemClick;

        private Sample _currentSample;

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

        public void ShowSamplePicker(Sample[] samples)
        {
            if (!SetupSamplePicker())
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

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequested;
            ItemClick -= ExtendedHamburgerMenu_ItemClick;
            OptionsItemClick -= ExtendedHamburgerMenu_OptionsItemClick;

            _hamburgerButton = GetTemplateChild("HamburgerButton") as Button;

            if (_hamburgerButton != null)
            {
                _hamburgerButton.Click += HamburgerButton_Click;
            }

            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            ItemClick += ExtendedHamburgerMenu_ItemClick;
            OptionsItemClick += ExtendedHamburgerMenu_OptionsItemClick;
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
        }

        private void HamburgerButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_buttonsListView == null)
            {
                _buttonsListView = GetTemplateChild("ButtonsListView") as ListView;
            }

            if (_buttonsListView != null)
            {
                _buttonsListView.Visibility = _buttonsListView.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
