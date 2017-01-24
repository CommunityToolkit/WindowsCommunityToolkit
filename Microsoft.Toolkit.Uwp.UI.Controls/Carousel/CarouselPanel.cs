using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class CarouselPanel : Panel
    {
        public CarouselPanel()
        {
            Background = new SolidColorBrush(Colors.Transparent);

            ManipulationMode = Orientation == Orientation.Vertical ?
                Windows.UI.Xaml.Input.ManipulationModes.TranslateY : Windows.UI.Xaml.Input.ManipulationModes.TranslateX;
        }

        public int ItemIndex
        {
            get
            {
                return (int)GetValue(ItemIndexProperty);
            }

            set
            {
                if (value < 0 || value >= Children.Count)
                {
                    return;
                }

                if (value != ItemIndex)
                {
                    SetValue(ItemIndexProperty, value);
                }
            }
        }

        public static readonly DependencyProperty ItemIndexProperty =
            DependencyProperty.Register("ItemIndex", typeof(int), typeof(CarouselPanel), new PropertyMetadata(0, OnPropertyChanged));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(CarouselPanel), new PropertyMetadata(Orientation.Vertical, OnOrientationChanged));

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CarouselPanel).InvalidateArrange();
            var carousel = d as CarouselPanel;
            carousel.ManipulationMode = (Orientation)e.NewValue == Orientation.Vertical ?
                Windows.UI.Xaml.Input.ManipulationModes.TranslateY : Windows.UI.Xaml.Input.ManipulationModes.TranslateX;
        }

        public double ItemSpacing
        {
            get { return (double)GetValue(ItemSpacingProperty); }
            set { SetValue(ItemSpacingProperty, value); }
        }

        public static readonly DependencyProperty ItemSpacingProperty =
            DependencyProperty.Register("ItemSpacing", typeof(double), typeof(CarouselPanel), new PropertyMetadata(80d, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CarouselPanel).InvalidateArrange();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double centerLeft = finalSize.Width / 2;
            double centerTop = finalSize.Height / 2;

            this.Clip = new RectangleGeometry { Rect = new Rect(0, 0, finalSize.Width, finalSize.Height) };

            for (int i = 0; i < this.Children.Count; ++i)
            {
                var child = this.Children[i];
                double deltaFromSelectedItem = 0;
                if (i < ItemIndex)
                {
                    // item is above
                    for (int j = i; j < this.ItemIndex; ++j)
                    {
                        var value = Orientation == Orientation.Vertical ? Children[j].DesiredSize.Height : Children[j].DesiredSize.Width;
                        deltaFromSelectedItem -= value + ItemSpacing;
                    }
                }
                else if (i > ItemIndex)
                {
                    for (int j = this.ItemIndex; j < i; ++j)
                    {
                        var value = Orientation == Orientation.Vertical ? Children[j].DesiredSize.Height : Children[j].DesiredSize.Width;
                        deltaFromSelectedItem += value + ItemSpacing;
                    }
                }

                double childLeft, childTop;
                if (Orientation == Orientation.Vertical)
                {
                    childLeft = centerLeft - (child.DesiredSize.Width / 2);
                    childTop = centerTop - (Children[ItemIndex].DesiredSize.Height / 2) + deltaFromSelectedItem;
                }
                else
                {
                    childLeft = centerLeft - (Children[ItemIndex].DesiredSize.Width / 2) + deltaFromSelectedItem;
                    childTop = centerTop - (child.DesiredSize.Height / 2);
                }

                child.Arrange(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));

                var visual = ElementCompositionPreview.GetElementVisual(child);
                var offsetAnimation = visual.Compositor.CreateVector3KeyFrameAnimation();
                offsetAnimation.Duration = TimeSpan.FromMilliseconds(200);
                offsetAnimation.InsertKeyFrame(1, new System.Numerics.Vector3((float)childLeft, (float)childTop, 0));
                visual.StartAnimation("Offset", offsetAnimation);

                (child as CarouselItem).CarouselItemLocation = i - ItemIndex;
                (child as CarouselItem).IsCentered = i == ItemIndex;
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            this.Clip = new RectangleGeometry { Rect = new Rect(0, 0, availableSize.Width, availableSize.Height) };

            foreach (var child in this.Children)
            {
                child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }

            return availableSize;
        }

        public UIElement AddElementToPanel(UIElement element)
        {
            if (!(element is CarouselItem))
            {
                var item = new CarouselItem();
                item.IsActionable = false;
                item.Content = element;
                element = item;
            }

            Children.Add(element);

            return element;
        }

        public CarouselItem GetTopItem()
        {
            if (Children.Count == 0)
            {
                return null;
            }

            return Children.ElementAt(ItemIndex) as CarouselItem;
        }
    }
}
