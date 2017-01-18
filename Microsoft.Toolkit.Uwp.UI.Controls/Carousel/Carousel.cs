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
    [TemplatePart(Name = PartCarouselPanel, Type = typeof(CarouselPanel))]
    public sealed class Carousel : ContentControl
    {
        public CarouselPanel CarouselPanel { get; private set; }

        private const string PartCarouselPanel = "TPanel";
        private const double _scrollRate = 100;
        private int _originalIndex;

        public Carousel()
        {
            this.DefaultStyleKey = typeof(Carousel);
        }

        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(Carousel), new PropertyMetadata(null, OnCarouselItemSourceChanged));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Carousel), new PropertyMetadata(Orientation.Vertical, OnOrientationChanged));

        public DataTemplate ItemTemplate
        {
            get { return _itemTemplate; }
            set { _itemTemplate = value; }
        }

        public int CurrentItemIndex
        {
            get
            {
                return (int)GetValue(CurrentItemIndexProperty);
            }

            set
            {
                if (CarouselPanel != null)
                {
                    var newValue = Clamp(value, 0, CarouselPanel.Children.Count - 1);
                    CarouselPanel.ItemIndex = newValue;
                    SetValue(CurrentItemIndexProperty, newValue);
                }
            }
        }

        public static readonly DependencyProperty CurrentItemIndexProperty =
            DependencyProperty.Register("CurrentItemIndex", typeof(int), typeof(Carousel), new PropertyMetadata(0));

        public event EventHandler<CarouselItemInvokedEventArgs> ItemInvoked;

        protected override void OnApplyTemplate()
        {
            if (CarouselPanel != null)
            {
                // clean up
                CarouselPanel.Children.Clear();
            }

            CarouselPanel = this.GetTemplateChild(PartCarouselPanel) as CarouselPanel;

            if (CarouselPanel != null)
            {
                CarouselPanel.Orientation = Orientation;
                UpdateItems();
                CarouselPanel.ItemIndex = CurrentItemIndex;

                CarouselPanel.PointerWheelChanged += CarouselPanel_PointerWheelChanged;
                CarouselPanel.ManipulationStarted += CarouselPanel_ManipulationStarted;
                CarouselPanel.ManipulationCompleted += CarouselPanel_ManipulationCompleted;
                CarouselPanel.ManipulationDelta += CarouselPanel_ManipulationDelta;
                CarouselPanel.Tapped += CarouselPanel_Tapped;
            }

            KeyDown += Carousel_KeyDown;
            KeyUp += Carousel_KeyUp;

            base.OnApplyTemplate();
        }

        private void Carousel_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (CarouselPanel == null)
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
                    var item = CarouselPanel.GetTopItem();
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
                    var item = CarouselPanel.GetTopItem();
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
                CurrentItemIndex = CarouselPanel.Children.IndexOf(item);
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
            CarouselPanel.InvalidateArrange();
        }

        private void CarouselPanel_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            CurrentItemIndex += e.GetCurrentPoint(null).Properties.MouseWheelDelta > 0 ? -1 : 1;
            e.Handled = true;
        }

        private DataTemplate _itemTemplate;

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Carousel carousel = d as Carousel;

            if (carousel.CarouselPanel == null)
            {
                return;
            }

            carousel.CarouselPanel.Orientation = (Orientation)e.NewValue;
        }

        private static void OnCarouselItemSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Carousel carousel = d as Carousel;
            carousel.HandleNewItemsSource(e.NewValue, e.OldValue);
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

            if (CarouselPanel == null)
            {
                return;
            }

            CarouselPanel.Children.Clear();

            foreach (var item in items)
            {
                CarouselPanel.AddElementToPanel(CreateItem(item));
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
