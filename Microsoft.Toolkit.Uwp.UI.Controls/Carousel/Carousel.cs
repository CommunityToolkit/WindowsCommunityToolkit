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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class Carousel : Canvas
    {
        private double initialOffset;

        // internalList keep reference on all UIElement
        private List<UIElement> internalList = new List<UIElement>();

        // ItemTemplate
        private DataTemplate itemTemplate;

        // prevent ArrangeOverride on items adding
        private bool isUpdatingPosition;

        // Storyboard on gesture
        private Storyboard storyboard = new Storyboard();

        // temp size
        private double desiredWidth;
        private double desiredHeight;

        // Gesture movement
        private bool isIncrementing;

        /// <summary>
        /// Gets or sets duration of the easing function animation (ms)
        /// </summary>
        public int TransitionDuration
        {
            get { return (int)GetValue(TransitionDurationProperty); }
            set { SetValue(TransitionDurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TransitionDuration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TransitionDurationProperty =
            DependencyProperty.Register("TransitionDuration", typeof(int), typeof(Carousel), new PropertyMetadata(500));

        /// <summary>
        /// Gets or sets selected Index
        /// </summary>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Carousel), new PropertyMetadata(0, OnLightStonePropertyChanged));

        private static void OnLightStonePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                return;
            }

            Carousel lightStone = (Carousel)d;
            lightStone.UpdatePosition();
        }

        /// <summary>
        /// Gets or sets depth of non Selected Index Items
        /// </summary>
        public double Depth
        {
            get { return (double)GetValue(DepthProperty); }
            set { SetValue(DepthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Depth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DepthProperty =
            DependencyProperty.Register("Depth", typeof(double), typeof(Carousel), new PropertyMetadata(0d, OnLightStonePropertyChanged));

        /// <summary>
        /// Gets or sets easing function to apply for each Transition
        /// </summary>
        public EasingFunctionBase EasingFunction
        {
            get { return (EasingFunctionBase)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EasingFunction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(EasingFunctionBase), typeof(Carousel), new PropertyMetadata(new ExponentialEase { EasingMode = EasingMode.EaseOut }));

        /// <summary>
        /// Gets or sets the X translation
        /// </summary>
        public int TranslateX
        {
            get { return (int)GetValue(TranslateXProperty); }
            set { SetValue(TranslateXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TranslateX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TranslateXProperty =
            DependencyProperty.Register("TranslateX", typeof(int), typeof(Carousel), new PropertyMetadata(0, OnLightStonePropertyChanged));

        /// <summary>
        /// Gets or sets Y translation
        /// </summary>
        public int TranslateY
        {
            get { return (int)GetValue(TranslateYProperty); }
            set { SetValue(TranslateYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Translation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TranslateYProperty =
            DependencyProperty.Register("TranslateY", typeof(int), typeof(Carousel), new PropertyMetadata(0, OnLightStonePropertyChanged));

        /// <summary>
        /// Gets or sets rotation angle
        /// </summary>
        public double Rotation
        {
            get { return (double)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Rotation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RotationProperty =
            DependencyProperty.Register("Rotation", typeof(double), typeof(Carousel), new PropertyMetadata(0d, OnLightStonePropertyChanged));

        public int MaxVisibleItems
        {
            get { return (int)GetValue(MaxVisibleItemsProperty); }
            set { SetValue(MaxVisibleItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxViewableItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxVisibleItemsProperty =
            DependencyProperty.Register("MaxVisibleItems", typeof(int), typeof(Carousel), new PropertyMetadata(15, OnLightStonePropertyChanged));

        /// <summary>
        /// Gets or sets items source
        /// </summary>
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                        "ItemsSource",
                        typeof(object),
                        typeof(Carousel),
                        new PropertyMetadata(0, ItemsSourceChangedCallback));

        private static void ItemsSourceChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue == null)
            {
                return;
            }

            if (args.NewValue == args.OldValue)
            {
                return;
            }

            Carousel lightStone = dependencyObject as Carousel;

            if (lightStone == null)
            {
                return;
            }

            var obsList = args.NewValue as INotifyCollectionChanged;

            if (obsList != null)
            {
                obsList.CollectionChanged += (sender, eventArgs) =>
                {
                    switch (eventArgs.Action)
                    {
                        case NotifyCollectionChangedAction.Remove:
                            foreach (var oldItem in eventArgs.OldItems)
                            {
                                for (int i = 0; i < lightStone.internalList.Count; i++)
                                {
                                    var fxElement = lightStone.internalList[i] as FrameworkElement;
                                    if (fxElement == null || fxElement.DataContext != oldItem)
                                    {
                                        continue;
                                    }

                                    lightStone.RemoveAt(i);
                                }
                            }

                            break;
                        case NotifyCollectionChangedAction.Add:
                            foreach (var newItem in eventArgs.NewItems)
                            {
                                lightStone.CreateItem(newItem, 0);
                            }

                            break;
                    }
                };
            }

            lightStone.Bind();
        }

        /// <summary>
        /// Gets or sets item Template
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get { return itemTemplate; }
            set { itemTemplate = value; }
        }

        public Carousel()
        {
            this.PointerPressed += OnPointerPressed;
            this.ManipulationMode = ManipulationModes.TranslateX;
            this.ManipulationCompleted += OnManipulationCompleted;
            this.ManipulationDelta += OnManipulationDelta;

            this.Tapped += OnTapped;
        }

        /// <summary>
        /// Bind all Items
        /// </summary>
        private void Bind()
        {
            if (ItemsSource == null || (ItemsSource as IEnumerable) == null)
            {
                return;
            }

            this.Children.Clear();
            this.internalList.Clear();

            foreach (object item in (IEnumerable)ItemsSource)
            {
                this.CreateItem(item);
            }
        }

        /// <summary>
        /// Remove an item at position. Reaffect SelectedIndex
        /// </summary>
        public void RemoveAt(int index)
        {
            var uiElement = this.internalList[index];

            // this.isUpdatingPosition = true;
            // Remove from internal list
            this.internalList.RemoveAt(index);

            // Remove from visual tree
            this.Children.Remove(uiElement);

            if (this.SelectedIndex == index)
            {
                this.SelectedIndex = this.SelectedIndex == 0 ? 0 : this.SelectedIndex - 1;
            }
            else if (this.SelectedIndex > index)
            {
                this.SelectedIndex--;
            }

            this.UpdatePosition();
        }

        /// <summary>
        /// Create an item (Load data template and bind)
        /// </summary>
        /// <returns>Return a new carousel item</returns>
        private FrameworkElement CreateItem(object item, double opacity = 1)
        {
            FrameworkElement element = ItemTemplate.LoadContent() as FrameworkElement;
            if (element == null)
            {
                return null;
            }

            element.DataContext = item;
            element.Opacity = opacity;
            element.RenderTransformOrigin = new Point(0.5, 0.5);

            PlaneProjection planeProjection = new PlaneProjection();
            planeProjection.CenterOfRotationX = 0.5;
            planeProjection.CenterOfRotationY = 0.5;

            element.Projection = planeProjection;

            this.internalList.Add(element);
            this.Children.Add(element);

            return element;
        }

        /// <summary>
        /// Arrange all items
        /// </summary>
        /// <returns>Return an item size</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (isUpdatingPosition)
            {
                return finalSize;
            }

            double centerLeft = 0;
            double centerTop = 0;

            this.Clip = new RectangleGeometry { Rect = new Rect(0, 0, finalSize.Width, finalSize.Height) };

            // Storyboard for all Items to appear
            var localStoryboard = new Storyboard();

            for (int i = 0; i < this.internalList.Count; i++)
            {
                UIElement container = internalList[i];

                Size desiredSize = container.DesiredSize;
                if (double.IsNaN(desiredSize.Width) || double.IsNaN(desiredSize.Height))
                {
                    continue;
                }

                // get the good center and top position
                if (centerLeft == 0 && centerTop == 0 && desiredSize.Width > 0 && desiredSize.Height > 0)
                {
                    desiredWidth = desiredSize.Width;
                    desiredHeight = desiredSize.Height;

                    centerLeft = (finalSize.Width / 2) - (desiredWidth / 2);
                    centerTop = (finalSize.Height - desiredHeight) / 2;
                }

                // Get position from SelectedIndex
                var deltaFromSelectedIndex = Math.Abs(this.SelectedIndex - i);

                // Get rect position
                var rect = new Rect(centerLeft, centerTop, desiredWidth, desiredHeight);

                container.Arrange(rect);
                Canvas.SetLeft(container, centerLeft);
                Canvas.SetTop(container, centerTop);

                // Apply Transform
                PlaneProjection planeProjection = container.Projection as PlaneProjection;

                if (planeProjection == null)
                {
                    continue;
                }

                // Get an initial projection (without move)
                var props = GetProjection(i, 0d);
                if (!double.IsNaN(props.Item1) && !double.IsNaN(props.Item2) && !double.IsNaN(props.Item3) && !double.IsNaN(props.Item4))
                {
                    planeProjection.LocalOffsetX = props.Item1;
                    planeProjection.GlobalOffsetY = props.Item2;
                    planeProjection.GlobalOffsetZ = props.Item3;
                    planeProjection.RotationY = props.Item4;
                }

                // calculate zindex and opacity
                int zindex = (this.internalList.Count * 100) - deltaFromSelectedIndex;
                double opacity = 1d - Math.Abs((double)(i - this.SelectedIndex) / (MaxVisibleItems + 1));

                container.Opacity = opacity;

                // Item appears
                if (container.Visibility == Visibility.Visible && container.Opacity == 0d)
                {
                    AddAnimation(localStoryboard, container, TransitionDuration, 0d, "Opacity", this.EasingFunction);
                }
                else
                {
                    container.Opacity = opacity;
                }

                Canvas.SetZIndex(container, zindex);
            }

            if (localStoryboard.Children.Count > 0)
            {
                localStoryboard.Begin();
            }

            return finalSize;
        }

        /// <summary>
        /// Measure items
        /// </summary>
        /// <returns>Return item size</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            this.Clip = new RectangleGeometry { Rect = new Rect(0, 0, availableSize.Width, availableSize.Height) };

            foreach (FrameworkElement container in this.Children)
            {
                container.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }

            return availableSize;
        }

        /// <summary>
        /// Tap an element
        /// </summary>
        private void OnTapped(object sender, TappedRoutedEventArgs args)
        {
            var positionX = args.GetPosition(this).X;
            for (int i = 0; i < this.internalList.Count; i++)
            {
                var child = internalList[i];
                var rect = child.TransformToVisual(this).TransformBounds(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));

                if (!(positionX >= rect.Left) || !(positionX <= (rect.Left + rect.Width)))
                {
                    continue;
                }

                isIncrementing = i > this.SelectedIndex;

                this.SelectedIndex = i;
                return;
            }
        }

        /// <summary>
        /// Initial pressed position
        /// </summary>
        private void OnPointerPressed(object sender, PointerRoutedEventArgs args)
        {
            initialOffset = args.GetCurrentPoint(this).Position.X;
        }

        /// <summary>
        /// Update all positions. Launch every animations on all items with a unique StoryBoard
        /// </summary>
        private void UpdatePosition()
        {
            storyboard = new Storyboard();

            isUpdatingPosition = true;

            for (int i = 0; i < this.internalList.Count; i++)
            {
                // Do not animate all items
                var inf = this.SelectedIndex - (MaxVisibleItems * 2);
                var sup = this.SelectedIndex + (MaxVisibleItems * 2);

                if (i < inf || i > sup)
                {
                    continue;
                }

                var item = internalList[i];

                PlaneProjection planeProjection = item.Projection as PlaneProjection;

                if (planeProjection == null)
                {
                    continue;
                }

                // Get target projection
                var props = GetProjection(i, 0d);

                // Zindex and Opacity
                var deltaFromSelectedIndex = Math.Abs(this.SelectedIndex - i);
                int zindex = (this.internalList.Count * 100) - deltaFromSelectedIndex;
                Canvas.SetZIndex(item, zindex);

                double opacity = 1d - Math.Abs((double)(i - this.SelectedIndex) / (MaxVisibleItems + 1));

                var newVisibility = deltaFromSelectedIndex > MaxVisibleItems
                               ? Visibility.Collapsed
                               : Visibility.Visible;

                // Item already present
                if (item.Visibility == newVisibility && item.Visibility == Visibility.Visible)
                {
                    if (!double.IsNaN(props.Item1))
                    {
                        AddAnimation(storyboard, item, TransitionDuration, props.Item1, "(UIElement.Projection).(PlaneProjection.LocalOffsetX)", this.EasingFunction);
                    }

                    AddAnimation(storyboard, item, TransitionDuration, props.Item2, "(UIElement.Projection).(PlaneProjection.GlobalOffsetY)", this.EasingFunction);
                    AddAnimation(storyboard, item, TransitionDuration, props.Item3, "(UIElement.Projection).(PlaneProjection.GlobalOffsetZ)", this.EasingFunction);

                    if (!double.IsNaN(props.Item4))
                    {
                        AddAnimation(storyboard, item, TransitionDuration, props.Item4, "(UIElement.Projection).(PlaneProjection.RotationY)", this.EasingFunction);
                    }

                    AddAnimation(storyboard, item, TransitionDuration, opacity, "Opacity", this.EasingFunction);
                }
                else if (newVisibility == Visibility.Visible)
                {
                    // This animation will occur in the ArrangeOverride() method
                    item.Visibility = newVisibility;
                    item.Opacity = 0d;
                }
                else if (newVisibility == Visibility.Collapsed)
                {
                    AddAnimation(storyboard, item, TransitionDuration, 0d, "Opacity", this.EasingFunction);
                    storyboard.Completed += (sender, o) =>
                        item.Visibility = Visibility.Collapsed;
                }
            }

            // When storyboard completed, Invalidate
            storyboard.Completed += (sender, o) =>
            {
                this.isUpdatingPosition = false;
                this.InvalidateArrange();
            };

            storyboard.Begin();
        }

        /// <summary>
        /// Get a new projection position for each items
        /// </summary>
        /// <returns> return a tuple containing newOffsetX, translateY, newDepth, newRotation</returns>
        private Tuple<double, double, double, double> GetProjection(int i, double deltaX)
        {
            var isLeftToRight = deltaX > 0;
            var isRightToLeft = !isLeftToRight;

            double newDepth = -this.Depth;

            double initialRotation = (i == this.SelectedIndex) ? 0d : ((i < this.SelectedIndex) ? Rotation : -Rotation);
            double newRotation = 0d;

            double offsetX = (i == this.SelectedIndex) ? 0 : (i - this.SelectedIndex) * desiredWidth;
            double translateX = (i == this.SelectedIndex) ? 0 : ((i < this.SelectedIndex) ? -TranslateX : TranslateX);
            double initialOffsetX = offsetX + translateX;
            double newOffsetX = 0d;

            var translateY = TranslateY;

            if (i == this.SelectedIndex)
            {
                // rotation is from -Rotation to Rotation
                // We get the proportional of deltaX by desiredWidth
                newRotation = initialRotation - (Rotation * deltaX / desiredWidth);

                // the offset max is the Sum(TranslateX + desiredWidth)
                // We get the proportional too
                newOffsetX = deltaX * (TranslateX + desiredWidth) / desiredWidth;
            }

            // only the first item on the left or right is moving on x, z, and d
            else if ((i == this.SelectedIndex - 1 && isLeftToRight) || (i == this.SelectedIndex + 1 && isRightToLeft))
            {
                // We get the rotation (proportional from delta to desiredwidth, always)
                // by far the initial position is Rotation, so we made a subsraction
                newRotation = initialRotation - (Rotation * deltaX / desiredWidth);

                // The Translation is decreasing to 0
                newOffsetX = initialOffsetX - (initialOffsetX * Math.Abs(deltaX) / desiredWidth);
            }

            // Other items just moved on x
            else
            {
                newOffsetX = initialOffsetX + deltaX;

                newRotation = initialRotation;
            }

            return new Tuple<double, double, double, double>(newOffsetX, translateY, newDepth, newRotation);
        }

        /// <summary>
        /// Set a manipulation
        /// </summary>
        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var deltaX = e.Cumulative.Translation.X;

            if (deltaX > desiredWidth)
            {
                deltaX = desiredWidth;
            }

            if (deltaX < -desiredWidth)
            {
                deltaX = -desiredWidth;
            }

            // Dont animate all items
            var inf = this.SelectedIndex - (MaxVisibleItems * 2);
            var sup = this.SelectedIndex + (MaxVisibleItems * 2);

            for (int i = 0; i < this.internalList.Count; i++)
            {
                // Dont animate all items
                if (i < inf || i > sup)
                {
                    continue;
                }

                var item = internalList[i];

                PlaneProjection planeProjection = item.Projection as PlaneProjection;

                if (planeProjection == null)
                {
                    continue;
                }

                // Get the new projection for the current item
                var props = GetProjection(i, deltaX);
                planeProjection.LocalOffsetX = props.Item1;
                planeProjection.GlobalOffsetY = props.Item2;
                planeProjection.GlobalOffsetZ = props.Item3;
                planeProjection.RotationY = props.Item4;

                // Zindex and Opacity

                // Minimum amount of movement to declare as a manipulation
                int moveThreshold = (int)desiredWidth / 4;

                // last position
                var clientX = e.Position.X;

                var zindexItemIndex = deltaX > 0 ? this.SelectedIndex - 1 : this.SelectedIndex + 1;
                var deltaFromSelectedIndex = Math.Abs(zindexItemIndex - i);

                // if we are under minimum amount, no Zindex applied
                if (Math.Abs(clientX - initialOffset) > moveThreshold)
                {
                    int zindex = (this.internalList.Count * 100) - deltaFromSelectedIndex;
                    Canvas.SetZIndex(item, zindex);
                }
            }
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            // Minimum amount of movement to declare as a manipulation
            int moveThreshold = (int)desiredWidth / 4;

            // last position
            var clientX = e.Position.X;

            // If not enough movement, re go in last placement
            if (!(Math.Abs(clientX - initialOffset) > moveThreshold))
            {
                // Recenter to selectedIndex;
                this.UpdatePosition();

                return;
            }

            isIncrementing = clientX < initialOffset;

            // Here is a manipulation
            if (isIncrementing)
            {
                // we are trying to move the last item.. to the left Just raised an UpdatePosition to
                // repositionne it on the center of the screen.
                if (this.SelectedIndex == (this.internalList.Count - 1))
                {
                    this.UpdatePosition();
                }
                else
                {
                    this.SelectedIndex = (this.SelectedIndex < (this.internalList.Count - 1)) ? this.SelectedIndex + 1 : this.SelectedIndex;
                }
            }
            else if (this.SelectedIndex > 0)
            {
                this.SelectedIndex--;
            }
            else
            {
                // we are trying to move the first item.. to the right. Just raised an UpdatePosition to
                // repositionne it on the center of the screen.
                if (this.SelectedIndex == 0)
                {
                    this.UpdatePosition();
                }
                else
                {
                    this.SelectedIndex = 0;
                }
            }

            initialOffset = clientX;
        }

        public static void AddAnimation(Storyboard storyboard, DependencyObject element, Timeline timeline, string propertyPath)
        {
            storyboard.Children.Add(timeline);
            Storyboard.SetTarget(timeline, element);
            Storyboard.SetTargetProperty(timeline, propertyPath);
        }

        public static void AddAnimation(Storyboard storyboard, DependencyObject element, int duration, double toValue, string propertyPath, EasingFunctionBase easingFunction = null)
        {
            DoubleAnimation timeline = new DoubleAnimation();
            timeline.To = toValue;
            timeline.Duration = TimeSpan.FromMilliseconds(duration);
            if (easingFunction != null)
            {
                timeline.EasingFunction = easingFunction;
            }

            storyboard.Children.Add(timeline);

            Storyboard.SetTarget(timeline, element);
            Storyboard.SetTargetProperty(timeline, propertyPath);
        }

        private static void AddAnimation(Storyboard storyboard, DependencyObject element, int duration, double fromValue, double toValue, string propertyPath, EasingFunctionBase easingFunction = null)
        {
            DoubleAnimation timeline = new DoubleAnimation();
            timeline.From = fromValue;
            timeline.To = toValue;
            timeline.Duration = TimeSpan.FromMilliseconds(duration);
            if (easingFunction != null)
            {
                timeline.EasingFunction = easingFunction;
            }

            storyboard.Children.Add(timeline);

            Storyboard.SetTarget(timeline, element);
            Storyboard.SetTargetProperty(timeline, propertyPath);
        }

        private static void Animate(DependencyObject element, string propertyPath, int duration, double toValue, EasingFunctionBase easingFunction = null, EventHandler<object> completed = null)
        {
            DoubleAnimation timeline = new DoubleAnimation();
            timeline.To = toValue;
            timeline.Duration = TimeSpan.FromMilliseconds(duration);
            if (easingFunction != null)
            {
                timeline.EasingFunction = easingFunction;
            }

            Storyboard sb = new Storyboard();
            if (completed != null)
            {
                sb.Completed += completed;
            }

            sb.Children.Add(timeline);
            Storyboard.SetTarget(sb, element);
            Storyboard.SetTargetProperty(sb, propertyPath);
            sb.Begin();
        }

        private static void Animate(DependencyObject element, string propertyPath, int duration, int startingDuration, double toValue, EasingFunctionBase easingFunction = null, EventHandler<object> completed = null)
        {
            DoubleAnimation timeline = new DoubleAnimation();
            timeline.To = toValue;
            timeline.Duration = TimeSpan.FromMilliseconds(duration);
            if (easingFunction != null)
            {
                timeline.EasingFunction = easingFunction;
            }

            Storyboard sb = new Storyboard();
            if (completed != null)
            {
                sb.Completed += completed;
            }

            sb.Children.Add(timeline);
            Storyboard.SetTarget(sb, element);
            Storyboard.SetTargetProperty(sb, propertyPath);
            sb.BeginTime = TimeSpan.FromMilliseconds(startingDuration);
            sb.Begin();
        }
    }
}
