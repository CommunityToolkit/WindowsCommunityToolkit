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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Control the user to cycle through a collection of items
    /// </summary>
    [TemplatePart(Name = PartCarouselPanel, Type = typeof(CarouselPanel))]
    public sealed class Carousel : ContentControl
    {
        private const string PartCarouselPanel = "TPanel";
        private const double _scrollRate = 100;
        private int _originalIndex;
        private CarouselPanel _carouselPanel;
        private DataTemplate _itemTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Carousel"/> class.
        /// </summary>
        public Carousel()
        {
            this.DefaultStyleKey = typeof(Carousel);
        }

        /// <summary>
        /// Gets or sets the DataTemplate to be applied on each <see cref="CarouselItem"/>
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get { return _itemTemplate; }
            set { _itemTemplate = value; }
        }

        /// <summary>
        /// Gets or sets the collection of items
        /// </summary>
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the orientation of the control
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of the current centered item
        /// </summary>
        public int CurrentItemIndex
        {
            get
            {
                return (int)GetValue(CurrentItemIndexProperty);
            }

            set
            {
                if (_carouselPanel != null)
                {
                    var newValue = Clamp(value, 0, _carouselPanel.Children.Count - 1);
                    _carouselPanel.ItemIndex = newValue;
                    SetValue(CurrentItemIndexProperty, newValue);
                }
            }
        }

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> property
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(Carousel), new PropertyMetadata(null, OnCarouselItemSourceChanged));

        /// <summary>
        /// Identifies the <see cref="Orientation"/> property
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Carousel), new PropertyMetadata(Orientation.Vertical, OnOrientationChanged));

        /// <summary>
        /// Identifies the <see cref="CurrentItemIndex"/> property
        /// </summary>
        public static readonly DependencyProperty CurrentItemIndexProperty =
            DependencyProperty.Register("CurrentItemIndex", typeof(int), typeof(Carousel), new PropertyMetadata(0));

        /// <summary>
        /// Occurs when item has been invoked
        /// </summary>
        public event EventHandler<CarouselItemInvokedEventArgs> ItemInvoked;

        protected override void OnApplyTemplate()
        {
            if (_carouselPanel != null)
            {
                // clean up
                _carouselPanel.Children.Clear();
            }

            _carouselPanel = this.GetTemplateChild(PartCarouselPanel) as CarouselPanel;

            if (_carouselPanel != null)
            {
                _carouselPanel.Orientation = Orientation;
                UpdateItems();
                _carouselPanel.ItemIndex = CurrentItemIndex;

                _carouselPanel.PointerWheelChanged += CarouselPanel_PointerWheelChanged;
                _carouselPanel.ManipulationStarted += CarouselPanel_ManipulationStarted;
                _carouselPanel.ManipulationCompleted += CarouselPanel_ManipulationCompleted;
                _carouselPanel.ManipulationDelta += CarouselPanel_ManipulationDelta;
                _carouselPanel.Tapped += CarouselPanel_Tapped;
            }

            KeyDown += Carousel_KeyDown;
            KeyUp += Carousel_KeyUp;

            base.OnApplyTemplate();
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Carousel carousel = d as Carousel;

            if (carousel._carouselPanel == null)
            {
                return;
            }

            carousel._carouselPanel.Orientation = (Orientation)e.NewValue;
        }

        private static void OnCarouselItemSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Carousel carousel = d as Carousel;
            carousel.HandleNewItemsSource(e.NewValue, e.OldValue);
        }

        private void Carousel_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (_carouselPanel == null)
            {
                return;
            }

            switch (e.Key)
            {
                case Windows.System.VirtualKey.Down:
                case Windows.System.VirtualKey.GamepadDPadDown:
                case Windows.System.VirtualKey.GamepadLeftThumbstickDown:
                    if (Orientation == Orientation.Vertical)
                    {
                        CurrentItemIndex++;
                        e.Handled = true;
                    }

                    break;
                case Windows.System.VirtualKey.Up:
                case Windows.System.VirtualKey.GamepadDPadUp:
                case Windows.System.VirtualKey.GamepadLeftThumbstickUp:
                    if (Orientation == Orientation.Vertical)
                    {
                        CurrentItemIndex--;
                        e.Handled = true;
                    }

                    break;
                case Windows.System.VirtualKey.Left:
                case Windows.System.VirtualKey.GamepadDPadLeft:
                case Windows.System.VirtualKey.GamepadLeftThumbstickLeft:
                    if (Orientation == Orientation.Horizontal)
                    {
                        CurrentItemIndex--;
                        e.Handled = true;
                    }

                    break;
                case Windows.System.VirtualKey.Right:
                case Windows.System.VirtualKey.GamepadDPadRight:
                case Windows.System.VirtualKey.GamepadLeftThumbstickRight:
                    if (Orientation == Orientation.Horizontal)
                    {
                        CurrentItemIndex++;
                        e.Handled = true;
                    }

                    break;
                case Windows.System.VirtualKey.Space:
                case Windows.System.VirtualKey.Enter:
                case Windows.System.VirtualKey.GamepadA:
                    var item = _carouselPanel.GetTopItem();
                    if (item == null || !item.IsActionable)
                    {
                        break;
                    }

                    item.Scale(duration: 200, centerX: 0.5f, centerY: 0.5f, scaleX: 1f, scaleY: 1f).StartAsync();
                    ElementSoundPlayer.Play(ElementSoundKind.Invoke);
                    ItemInvoked?.Invoke(this, new Controls.CarouselItemInvokedEventArgs() { Container = item });
                    break;
            }
        }

        private void Carousel_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Space:
                case Windows.System.VirtualKey.Enter:
                case Windows.System.VirtualKey.GamepadA:
                    var item = _carouselPanel.GetTopItem();
                    if (item == null || !item.IsActionable)
                    {
                        break;
                    }

                    item.Scale(duration: 200, centerX: 0.5f, centerY: 0.5f, scaleX: 0.9f, scaleY: 0.9f).StartAsync();
                    break;
            }
        }

        private void CarouselPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            List<UIElement> elements = new List<UIElement>(
                VisualTreeHelper.FindElementsInHostCoordinates(
                    e.GetPosition(Window.Current.Content), this));

            CarouselItem item = elements.Where(el => el is CarouselItem).FirstOrDefault() as CarouselItem;

            if (item != null)
            {
                CurrentItemIndex = _carouselPanel.Children.IndexOf(item);
            }

            Focus(FocusState.Pointer);
        }

        private void CarouselPanel_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            _originalIndex = CurrentItemIndex;
        }

        private void CarouselPanel_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var value = Orientation == Orientation.Horizontal ? e.Cumulative.Translation.X : e.Cumulative.Translation.Y;
            var scalledValue = (int)(value / _scrollRate) * -1;
            CurrentItemIndex = _originalIndex + scalledValue;
        }

        private void CarouselPanel_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            _carouselPanel.InvalidateArrange();
        }

        private void CarouselPanel_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            CurrentItemIndex += e.GetCurrentPoint(null).Properties.MouseWheelDelta > 0 ? -1 : 1;
            e.Handled = true;
        }

        private void HandleNewItemsSource(object newValue, object oldValue)
        {
            if (newValue == null)
            {
                return;
            }

            if (newValue == oldValue)
            {
                return;
            }

            if (oldValue != null)
            {
                var oldObservableList = oldValue as INotifyCollectionChanged;
                if (oldObservableList != null)
                {
                    oldObservableList.CollectionChanged -= ObservableList_CollectionChanged;
                }
            }

            var items = newValue as IEnumerable<object>;
            if (items == null)
            {
                return;
            }

            var observableList = newValue as INotifyCollectionChanged;
            if (observableList != null)
            {
                observableList.CollectionChanged += ObservableList_CollectionChanged;
            }

            UpdateItems();
        }

        private void ObservableList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (CoreApplication.MainView.CoreWindow != null)
            {
                var t = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    UpdateItems();
                });
            }
        }

        private void UpdateItems()
        {
            IEnumerable<object> items = ItemsSource as IEnumerable<object>;
            if (items == null)
            {
                return;
            }

            if (_carouselPanel == null)
            {
                return;
            }

            _carouselPanel.Children.Clear();

            foreach (var item in items)
            {
                _carouselPanel.AddElementToPanel(CreateItem(item));
            }
        }

        private FrameworkElement CreateItem(object item)
        {
            FrameworkElement element = ItemTemplate.LoadContent() as FrameworkElement;
            if (element == null)
            {
                return null;
            }

            element.DataContext = item;
            return element;
        }

        private T Clamp<T>(T val, T min, T max)
            where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0)
            {
                return min;
            }
            else if (val.CompareTo(max) > 0)
            {
                return max;
            }
            else
            {
                return val;
            }
        }
    }
}
