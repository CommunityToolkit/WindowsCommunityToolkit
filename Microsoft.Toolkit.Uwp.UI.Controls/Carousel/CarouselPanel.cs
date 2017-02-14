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
        internal CarouselPanel()
        {
            Background = new SolidColorBrush(Colors.Transparent);

            ManipulationMode = Orientation == Orientation.Vertical ?
                Windows.UI.Xaml.Input.ManipulationModes.TranslateY : Windows.UI.Xaml.Input.ManipulationModes.TranslateX;
        }

        internal int ItemIndex
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

        internal double ItemSpacing
        {
            get { return (double)GetValue(ItemSpacingProperty); }
            set { SetValue(ItemSpacingProperty, value); }
        }

        internal Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        internal static readonly DependencyProperty ItemIndexProperty =
            DependencyProperty.Register("ItemIndex", typeof(int), typeof(CarouselPanel), new PropertyMetadata(0, OnPropertyChanged));

        internal static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(CarouselPanel), new PropertyMetadata(Orientation.Vertical, OnOrientationChanged));

        internal static readonly DependencyProperty ItemSpacingProperty =
            DependencyProperty.Register("ItemSpacing", typeof(double), typeof(CarouselPanel), new PropertyMetadata(80d, OnPropertyChanged));

        internal UIElement AddElementToPanel(UIElement element)
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

        internal CarouselItem GetTopItem()
        {
            if (Children.Count == 0)
            {
                return null;
            }

            return Children.ElementAt(ItemIndex) as CarouselItem;
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CarouselPanel).InvalidateArrange();
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CarouselPanel).InvalidateArrange();
            var carousel = d as CarouselPanel;
            carousel.ManipulationMode = (Orientation)e.NewValue == Orientation.Vertical ?
                Windows.UI.Xaml.Input.ManipulationModes.TranslateY : Windows.UI.Xaml.Input.ManipulationModes.TranslateX;
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
    }
}
