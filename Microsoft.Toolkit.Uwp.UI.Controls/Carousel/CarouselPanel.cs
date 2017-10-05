// ****************************************************************** Copyright (c) Microsoft. All rights reserved. This code is licensed under the MIT License (MIT). THE CODE IS PROVIDED “AS IS”,
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE CODE OR THE USE OR OTHER
// DEALINGS IN THE CODE. ******************************************************************

using System;
using Microsoft.Toolkit.Uwp.UI.Extensions;

using System;

using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Structure used when an item moves
    /// </summary>
    public struct Proj
    {
        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>
        /// The depth.
        /// </value>
        public double Depth { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public double Position { get; set; }

        /// <summary>
        /// Gets or sets the rotation x.
        /// </summary>
        /// <value>
        /// The rotation x.
        /// </value>
        public double RotationX { get; set; }

        /// <summary>
        /// Gets or sets the rotation y.
        /// </summary>
        /// <value>
        /// The rotation y.
        /// </value>
        public double RotationY { get; set; }

        /// <summary>
        /// Gets or sets the rotation z.
        /// </summary>
        /// <value>
        /// The rotation z.
        /// </value>
        public double RotationZ { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Panel" />
    public class CarouselPanel : Panel
    {
        /// <summary>
        /// The carousel control
        /// </summary>
        private Carousel carouselControl;

        /// <summary>
        /// The desired height
        /// </summary>
        private double desiredHeight;

        // temp size
        /// <summary>
        /// The desired width
        /// </summary>
        private double desiredWidth;

        // Storyboard on gesture
        /// <summary>
        /// The storyboard
        /// </summary>
        private Storyboard storyboard = new Storyboard();

        /// <summary>
        /// Initializes a new instance of the <see cref="CarouselPanel"/> class.
        /// </summary>
        public CarouselPanel()
        {
            IsHitTestVisible = true;

            Background = new SolidColorBrush(Colors.Transparent);
            ManipulationMode = ManipulationModes.All;
            ManipulationCompleted += OnManipulationCompleted;
            ManipulationDelta += OnManipulationDelta;
            Tapped += OnTapped;
        }

        /// <summary>
        /// Gets the Current Carousel control
        /// </summary>
        /// <value>
        /// The carousel.
        /// </value>
        /// <exception cref="Exception">This CarouselPanel must be used as an ItemsPanel in a Carousel control</exception>
        public Carousel Carousel
        {
            get
            {
                if (carouselControl != null)
                {
                    return carouselControl;
                }

                carouselControl = this.FindAscendant<Carousel>();

                if (carouselControl == null)
                {
                    throw new Exception("This CarouselPanel must be used as an ItemsPanel in a Carousel control");
                }

                return carouselControl;
            }
        }

        /// <summary>
        /// Add an animation to the current storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard.</param>
        /// <param name="element">The element.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="toValue">To value.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <param name="easingFunction">The easing function.</param>
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

        /// <summary>
        /// End of a manipulation
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ManipulationCompletedRoutedEventArgs"/> instance containing the event data.</param>
        internal void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            // Need to know which direction we took for this manipulation.
            var translation = Carousel.Orientation == Orientation.Horizontal ? e.Cumulative.Translation.X : e.Cumulative.Translation.Y;

            // if manipulation is not enough to change index we will have to force refresh
            var lastIndex = Carousel.SelectedIndex;

            // potentially border effects
            bool hasBreak = false;

            for (int i = 0; i < Children.Count - 1; i++)
            {
                var child = Children[i];

                PlaneProjection projection = child.Projection as PlaneProjection;
                CompositeTransform compositeTransform = child.RenderTransform as CompositeTransform;

                if (projection == null || compositeTransform == null)
                {
                    continue;
                }

                var margin = Carousel.ItemMargin;
                var size = Carousel.Orientation == Orientation.Horizontal ? desiredWidth : desiredHeight;
                var left = Carousel.Orientation == Orientation.Horizontal ? compositeTransform.TranslateX : compositeTransform.TranslateY;
                var right = left + size + margin;
                var condition = translation < 0 ? (left > 0) : (right > 0);

                if (condition)
                {
                    Carousel.SelectedIndex = i;
                    hasBreak = true;
                    break;
                }
            }

            if (!hasBreak)
            {
                Carousel.SelectedIndex = translation > 0 ? 0 : Children.Count - 1;
            }

            e.Handled = true;
        }

        /// <summary>
        /// Set a manipulation
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ManipulationDeltaRoutedEventArgs"/> instance containing the event data.</param>
        internal void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var previousIndex = Carousel.SelectedIndex;

            for (int i = 0; i < Children.Count; i++)
            {
                var item = Children[i];

                var delta = Carousel.Orientation == Orientation.Horizontal ? e.Delta.Translation.X : e.Delta.Translation.Y;
                var itemLength = Carousel.Orientation == Orientation.Horizontal ? item.DesiredSize.Width : item.DesiredSize.Height;

                var proj = GetProjectionFromManipulation(item, delta);

                ApplyProjection(item, proj);

                // We have to take care of the first and last items when manipulating
                if ((i == 0 && proj.Position > itemLength / 2) || (i == Children.Count - 1 && proj.Position < -itemLength))
                {
                    e.Handled = true;
                    e.Complete();
                    Carousel.SelectedIndex = i;

                    // force refresh if we are already on the first / last item
                    if (previousIndex == i)
                    {
                        UpdatePosition();
                    }

                    return;
                }

                // Calculate the Z index to be sure selected item is over all others
                var zindexItemIndex = delta > 0 ? Carousel.SelectedIndex - 1 : Carousel.SelectedIndex + 1;
                var deltaFromSelectedIndex = Math.Abs(zindexItemIndex - i);

                int zindex = (Children.Count * 100) - deltaFromSelectedIndex;
                Canvas.SetZIndex(item, zindex);
            }

            e.Handled = true;
        }

        /// <summary>
        /// Update all positions. Launch every animations on all items with a unique StoryBoard
        /// </summary>
        internal void UpdatePosition()
        {
            this.storyboard = new Storyboard();
            this.ManipulationMode = ManipulationModes.None;

            // Start from the selected index which will be the center of the list and go both sides.
            if (this.Children.Count > 0)
            {
                int selectedIndex = this.Carousel.SelectedIndex;

                this.UpdatePositionForItem(this.Children[selectedIndex], 0);

                SingleLinkedList<UIElement> forwardList = new SingleLinkedList<UIElement>(this.Children, selectedIndex, this.Carousel.IsCircular);
                SingleLinkedList<UIElement> backwardList = new SingleLinkedList<UIElement>(this.Children, selectedIndex, this.Carousel.IsCircular);

                UIElement prevElement = backwardList.Previous();
                UIElement nextElement = forwardList.Next();
                List<UIElement> positionedElements = new List<UIElement>();

                int relativePosition = 1;
                while (prevElement != nextElement)
                {
                    // Ensure that the elements are not processed twice. This may happen when items count is odd.
                    if (prevElement != null && !positionedElements.Contains(prevElement))
                    {
                        this.UpdatePositionForItem(prevElement, relativePosition * -1);
                        positionedElements.Add(prevElement);
                    }

                    if (nextElement != null && !positionedElements.Contains(nextElement))
                    {
                        this.UpdatePositionForItem(nextElement, relativePosition);
                        positionedElements.Add(nextElement);
                    }

                    prevElement = backwardList.Previous();
                    nextElement = forwardList.Next();

                    relativePosition++;
                }
            }

            // When storyboard completed, Invalidate
            this.storyboard.Completed += (sender, o) =>
            {
                this.ManipulationMode = ManipulationModes.All;
            };

            this.storyboard.Begin();
        }

        /// <summary>
        /// Arrange all items
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this object should use to arrange itself and its children.</param>
        /// <returns>
        /// Return an item size
        /// </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            double centerLeft = 0;
            double centerTop = 0;

            Clip = new RectangleGeometry { Rect = new Rect(0, 0, finalSize.Width, finalSize.Height) };

            for (int i = 0; i < Children.Count; i++)
            {
                FrameworkElement container = Children[i] as FrameworkElement;

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

                // Get absolute position from SelectedIndex
                var deltaFromSelectedIndex = Math.Abs(Carousel.SelectedIndex - i);

                // Get rect position
                var rect = new Rect(centerLeft, centerTop, desiredWidth, desiredHeight);

                // Placing the rect in the center of screen
                container.Arrange(rect);

                // Get the initial projection (without move)
                var proj = GetProjectionFromSelectedIndex(i);

                // apply the projection to the current object
                ApplyProjection(container, proj);

                // calculate zindex and opacity
                int zindex = (Children.Count * 100) - deltaFromSelectedIndex;
                Canvas.SetZIndex(container, zindex);
            }

            return finalSize;
        }

        /// <summary>
        /// Measure items
        /// </summary>
        /// <param name="availableSize">The available size that this object can give to child objects. Infinity can be specified as a value to indicate that the object will size to whatever content is available.</param>
        /// <returns>
        /// Return carousel size
        /// </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            var containerWidth = 0d;
            var containerHeight = 0d;

            foreach (FrameworkElement container in Children)
            {
                container.Measure(availableSize);

                if (container.DesiredSize.Width > containerWidth)
                {
                    containerWidth = container.DesiredSize.Width;
                }

                if (container.DesiredSize.Height > containerHeight)
                {
                    containerHeight = container.DesiredSize.Height;
                }
            }

            var width = 0d;
            var height = 0d;

            // It's a Auto size, so we define the size should be 3 items
            if (double.IsInfinity(availableSize.Width))
            {
                width = Carousel.Orientation == Orientation.Horizontal ? containerWidth * (Children.Count > 3 ? 3 : Children.Count) : containerWidth;
            }
            else
            {
                width = availableSize.Width;
            }

            // It's a Auto size, so we define the size should be 3 items
            if (double.IsInfinity(availableSize.Height))
            {
                height = Carousel.Orientation == Orientation.Vertical ? containerHeight * (Children.Count > 3 ? 3 : Children.Count) : containerHeight;
            }
            else
            {
                height = availableSize.Height;
            }

            Clip = new RectangleGeometry { Rect = new Rect(0, 0, width, height) };

            return new Size(width, height);
        }

        /// <summary>
        /// Apply the projection, with or without a storyboard involved
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="proj">The proj.</param>
        /// <param name="storyboard">The storyboard.</param>
        private void ApplyProjection(UIElement element, Proj proj, Storyboard storyboard = null)
        {
            // then apply the plane projection transform
            PlaneProjection planeProjection = element.Projection as PlaneProjection;
            CompositeTransform compositeTransform = element.RenderTransform as CompositeTransform;

            if (planeProjection == null || compositeTransform == null)
            {
                return;
            }

            // Direct affectation (during first load) or with a storyboard
            if (storyboard == null)
            {
                compositeTransform.TranslateX = Carousel.Orientation == Orientation.Horizontal ? proj.Position : 0;
                compositeTransform.TranslateY = Carousel.Orientation == Orientation.Horizontal ? 0 : proj.Position;

                planeProjection.RotationX = proj.RotationX;
                planeProjection.RotationY = proj.RotationY;
                planeProjection.RotationZ = proj.RotationZ;

                planeProjection.GlobalOffsetZ = proj.Depth;
            }
            else
            {
                string localProjectionOrientation = Carousel.Orientation == Orientation.Horizontal ?
                                    "(UIElement.RenderTransform).(CompositeTransform.TranslateX)" : "(UIElement.RenderTransform).(CompositeTransform.TranslateY)";
                string localProjectionOrientationInvert = Carousel.Orientation == Orientation.Horizontal ?
                                    "(UIElement.RenderTransform).(CompositeTransform.TranslateY)" : "(UIElement.RenderTransform).(CompositeTransform.TranslateX)";

                string globalDepthProjection = "(UIElement.Projection).(PlaneProjection.GlobalOffsetZ)";
                string rotationXProjection = "(UIElement.Projection).(PlaneProjection.RotationX)";
                string rotationYProjection = "(UIElement.Projection).(PlaneProjection.RotationY)";
                string rotationZProjection = "(UIElement.Projection).(PlaneProjection.RotationZ)";

                AddAnimation(storyboard, element, Carousel.TransitionDuration, proj.Position, localProjectionOrientation, Carousel.EasingFunction);
                AddAnimation(storyboard, element, Carousel.TransitionDuration, 0, localProjectionOrientationInvert, Carousel.EasingFunction);
                AddAnimation(storyboard, element, Carousel.TransitionDuration, proj.Depth, globalDepthProjection, Carousel.EasingFunction);
                AddAnimation(storyboard, element, Carousel.TransitionDuration, proj.RotationX, rotationXProjection, Carousel.EasingFunction);
                AddAnimation(storyboard, element, Carousel.TransitionDuration, proj.RotationY, rotationYProjection, Carousel.EasingFunction);
                AddAnimation(storyboard, element, Carousel.TransitionDuration, proj.RotationZ, rotationZProjection, Carousel.EasingFunction);
            }
        }

        /// <summary>
        /// Calculate a new projection after a manipulation delta
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="delta">The delta.</param>
        /// <returns>
        /// Return the new projection
        /// </returns>
        private Proj GetProjectionFromManipulation(UIElement element, double delta)
        {
            PlaneProjection projection = element.Projection as PlaneProjection;
            CompositeTransform compositeTransform = element.RenderTransform as CompositeTransform;

            var bounds = Carousel.Orientation == Orientation.Horizontal ? desiredWidth : desiredHeight;
            var margin = Carousel.ItemMargin;

            // this maxsize from middle is the value when depth is max
            var maxBounds = bounds + margin;

            // Calculate the new position of the current item
            var newPosition = Carousel.Orientation == Orientation.Horizontal ? compositeTransform.TranslateX : compositeTransform.TranslateY;
            newPosition = newPosition + delta;

            // Calculate the relative position (to keep something positive)
            var relativePosition = Math.Abs(newPosition);

            // max Depth
            double depth = (double)-Carousel.ItemDepth;

            // rotations
            var rotationX = Carousel.ItemRotationX;
            var rotationY = Carousel.ItemRotationY;
            var rotationZ = Carousel.ItemRotationZ;

            // if the relativeposition is inside the bounds so calculate the proportionals
            if (relativePosition <= maxBounds)
            {
                depth = relativePosition * depth / maxBounds;
                rotationX = relativePosition * Carousel.ItemRotationX / maxBounds;
                rotationY = relativePosition * Carousel.ItemRotationY / maxBounds;
                rotationZ = relativePosition * Carousel.ItemRotationZ / maxBounds;
            }

            if (Carousel.InvertPositive && newPosition > 0)
            {
                rotationX = -rotationX;
                rotationY = -rotationY;
                rotationZ = -rotationZ;
            }

            return new Proj { Position = newPosition, Depth = depth, RotationX = rotationX, RotationY = rotationY, RotationZ = rotationZ };
        }

        /// <summary>
        /// Gets the projection from pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <returns></returns>
        private Proj GetProjectionFromPointer(int pointer)
        {
            // margin
            var margin = Carousel.ItemMargin;

            // size beetween each element
            var widthOrHeight = Carousel.Orientation == Orientation.Horizontal ? desiredWidth : desiredHeight;

            // calculate the position with the margin applied
            double position = pointer * (widthOrHeight + margin);

            // Depth orientation
            var depth = pointer == 0 ? 0 : -Carousel.ItemDepth;

            // Rotation on each axes
            var rotationX = pointer == 0 ? 0 : Carousel.ItemRotationX;
            var rotationY = pointer == 0 ? 0 : Carousel.ItemRotationY;
            var rotationZ = pointer == 0 ? 0 : Carousel.ItemRotationZ;

            if (Carousel.InvertPositive && pointer > 0)
            {
                rotationX = -rotationX;
                rotationY = -rotationY;
                rotationZ = -rotationZ;
            }

            return new Proj { Position = position, Depth = depth, RotationX = rotationX, RotationY = rotationY, RotationZ = rotationZ };
        }

        /// <summary>
        /// get the projection from a current index. Used On ArrangeOverride step
        /// </summary>
        /// <param name="childIndex">Index of the child.</param>
        /// <returns>
        /// Return the new projection
        /// </returns>
        private Proj GetProjectionFromSelectedIndex(int childIndex)
        {
            // margin
            var margin = Carousel.ItemMargin;

            // we want the middle image to be indice 0, to be sure the centered image is with no rotation
            var relativeIndex = childIndex - Carousel.SelectedIndex;

            // size beetween each element
            var widthOrHeight = Carousel.Orientation == Orientation.Horizontal ? desiredWidth : desiredHeight;

            // calculate the position with the margin applied
            double position = relativeIndex * (widthOrHeight + margin);

            // Depth orientation
            var depth = relativeIndex == 0 ? 0 : -Carousel.ItemDepth;

            // Rotation on each axes
            var rotationX = relativeIndex == 0 ? 0 : Carousel.ItemRotationX;
            var rotationY = relativeIndex == 0 ? 0 : Carousel.ItemRotationY;
            var rotationZ = relativeIndex == 0 ? 0 : Carousel.ItemRotationZ;

            if (Carousel.InvertPositive && relativeIndex > 0)
            {
                rotationX = -rotationX;
                rotationY = -rotationY;
                rotationZ = -rotationZ;
            }

            return new Proj { Position = position, Depth = depth, RotationX = rotationX, RotationY = rotationY, RotationZ = rotationZ };
        }

        /// <summary>
        /// Tapp an item
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TappedRoutedEventArgs"/> instance containing the event data.</param>
        private void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var position = e.GetPosition(this);

            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];
                var rect = child.TransformToVisual(this).TransformBounds(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));

                if (!(position.X >= rect.Left && position.X <= rect.Right && position.Y >= rect.Top && position.Y <= rect.Bottom))
                {
                    continue;
                }

                Carousel.SelectedIndex = i;

                return;
            }
        }

        /// <summary>
        /// Updates the position for item.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="pointer">The pointer.</param>
        private void UpdatePositionForItem(UIElement element, int pointer)
        {
            PlaneProjection planeProjection = element.Projection as PlaneProjection;

            if (planeProjection == null)
            {
                return;
            }

            // Get target projection
            var props = GetProjectionFromPointer(pointer);

            // Apply projection
            ApplyProjection(element, props, storyboard);

            // Zindex and Opacity
            int zindex = (Carousel.Items.Count * 100) - Math.Abs(pointer);
            Canvas.SetZIndex(element, zindex);
        }

        /// <summary>
        /// Linked List capable of moving forward, backward and circular for a collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        private class SingleLinkedList<T>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SingleLinkedList{T}" /> class.
            /// </summary>
            /// <param name="items">The items.</param>
            /// <param name="pointerPos">The pointer position.</param>
            /// <param name="isCircular">if set to <c>true</c> [is circular].</param>
            public SingleLinkedList(ICollection<T> items, int pointerPos = 0, bool isCircular = false)
            {
                this.Items = items;
                this.PointerPosition = pointerPos;
                this.IsCircular = isCircular;
            }

            /// <summary>
            /// Gets a value indicating whether this instance is circular.
            /// </summary>
            /// <value>
            ///   <c>true</c> if this instance is circular; otherwise, <c>false</c>.
            /// </value>
            public bool IsCircular { get; private set; }

            /// <summary>
            /// Gets the items.
            /// </summary>
            /// <value>
            /// The items.
            /// </value>
            public ICollection<T> Items { get; private set; }

            /// <summary>
            /// Gets the pointer position.
            /// </summary>
            /// <value>
            /// The pointer position. Returns -1 if the end or beginnning of list is reach in a non-circular list.
            /// </value>
            public int PointerPosition { get; private set; }

            /// <summary>
            /// Gets the current item.
            /// </summary>
            /// <returns>Returns item of type <see cref="SingleLinkedList{T}"/>. Returns a default value of type if no element is found.</returns>
            public T GetCurrent()
            {
                return (this.PointerPosition == -1) ? default(T) : this.Items.ElementAt(this.PointerPosition);
            }

            /// <summary>
            /// Moves the pointer to the next item and returns it.
            /// </summary>
            /// <returns>Returns item of type <see cref="SingleLinkedList{T}"/></returns>
            public T Next()
            {
                this.PointerPosition = (this.PointerPosition == this.Items.Count - 1 || this.PointerPosition == -1) ? (this.IsCircular ? 0 : -1) : this.PointerPosition + 1;

                return this.GetCurrent();
            }

            /// <summary>
            /// Moves the pointer to the previous item and returns it.
            /// </summary>
            /// <returns>Returns item of type <see cref="SingleLinkedList{T}"/></returns>
            public T Previous()
            {
                this.PointerPosition = (this.PointerPosition <= 0) ? (this.IsCircular ? this.Items.Count - 1 : -1) : this.PointerPosition - 1;

                return this.GetCurrent();
            }
        }
    }
}