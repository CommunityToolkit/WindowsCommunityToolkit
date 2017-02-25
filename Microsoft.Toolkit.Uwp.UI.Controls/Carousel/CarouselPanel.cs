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
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class CarouselPanel : Panel
    {
        // prevent ArrangeOverride on items adding
        private bool isUpdatingPosition;

        // Storyboard on gesture
        private Storyboard storyboard = new Storyboard();

        // temp size
        private double desiredWidth;
        private double desiredHeight;

        private Carousel carouselControl;

        public CarouselPanel()
        {
            this.IsHitTestVisible = true;

            this.ManipulationMode = ManipulationModes.All;
            this.ManipulationCompleted += OnManipulationCompleted;
            this.ManipulationDelta += OnManipulationDelta;
            this.Tapped += OnTapped;
        }

        /// <summary>
        /// Gets the Current Carousel control
        /// </summary>
        public Carousel Carousel
        {
            get { return carouselControl ?? (carouselControl = this.FindVisualAscendant<Carousel>()); }
        }

        /// <summary>
        /// Tapp an item
        /// </summary>
        private void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var position = e.GetPosition(this);

            for (int i = 0; i < this.Children.Count; i++)
            {
                var child = this.Children[i];
                var rect = child.TransformToVisual(this).TransformBounds(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));

                if (!(position.X >= rect.Left && position.X <= rect.Right && position.Y >= rect.Top && position.Y <= rect.Bottom))
                {
                    continue;
                }

                this.Carousel.SelectedIndex = i;

                return;
            }
        }

        /// <summary>
        /// Set a manipulation
        /// </summary>
        internal void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var previousIndex = this.Carousel.SelectedIndex;

            for (int i = 0; i < this.Children.Count; i++)
            {
                var item = this.Children[i];

                var delta = this.Carousel.Orientation == Orientation.Horizontal ? e.Delta.Translation.X : e.Delta.Translation.Y;
                var itemLength = this.Carousel.Orientation == Orientation.Horizontal ? item.DesiredSize.Width : item.DesiredSize.Height;

                var proj = GetProjectionFromManipulation(item, delta);

                this.ApplyProjection(item, proj);

                // We have to take care of the first and last items when manipulating
                if ((i == 0 && proj.Position > itemLength / 2) || (i == this.Children.Count - 1 && proj.Position < -itemLength))
                {
                    e.Handled = true;
                    e.Complete();
                    this.Carousel.SelectedIndex = i;

                    // force refresh if we are already on the first / last item
                    if (previousIndex == i)
                    {
                        this.UpdatePosition();
                    }

                    return;
                }

                // Calculate the Z index to be sure selected item is over all others
                var zindexItemIndex = delta > 0 ? this.Carousel.SelectedIndex - 1 : this.Carousel.SelectedIndex + 1;
                var deltaFromSelectedIndex = Math.Abs(zindexItemIndex - i);

                int zindex = (this.Children.Count * 100) - deltaFromSelectedIndex;
                Canvas.SetZIndex(item, zindex);
            }

            e.Handled = true;
        }

        /// <summary>
        /// End of a manipulation
        /// </summary>
        internal void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            // Need to know which direction we took for this manipulation.
            var translation = this.Carousel.Orientation == Orientation.Horizontal ? e.Cumulative.Translation.X : e.Cumulative.Translation.Y;

            // if manipulation is not enough to change index we will have to force refresh
            var lastIndex = this.Carousel.SelectedIndex;

            // potentially border effects
            bool hasBreak = false;

            for (int i = 0; i < this.Children.Count - 1; i++)
            {
                var child = this.Children[i];

                PlaneProjection projection = child.Projection as PlaneProjection;
                CompositeTransform compositeTransform = child.RenderTransform as CompositeTransform;

                if (projection == null || compositeTransform == null)
                {
                    continue;
                }

                var margin = this.Carousel.ItemMargin;
                var size = this.Carousel.Orientation == Orientation.Horizontal ? desiredWidth : desiredHeight;
                var left = this.Carousel.Orientation == Orientation.Horizontal ? compositeTransform.TranslateX : compositeTransform.TranslateY;
                var right = left + size + margin;
                var condition = translation < 0 ? (left > 0) : (right > 0);

                if (condition)
                {
                    this.Carousel.SelectedIndex = i;
                    hasBreak = true;
                    break;
                }
            }

            if (!hasBreak)
            {
                this.Carousel.SelectedIndex = translation > 0 ? 0 : this.Children.Count - 1;
            }

            e.Handled = true;
        }

        /// <summary>
        /// Update all positions. Launch every animations on all items with a unique StoryBoard
        /// </summary>
        internal void UpdatePosition()
        {
            storyboard = new Storyboard();
            isUpdatingPosition = true;
            this.ManipulationMode = ManipulationModes.None;

            for (int i = 0; i < this.Children.Count; i++)
            {
                var item = this.Children[i];

                PlaneProjection planeProjection = item.Projection as PlaneProjection;

                if (planeProjection == null)
                {
                    continue;
                }

                // Get target projection
                var props = GetProjectionFromSelectedIndex(i);

                // Apply projection
                this.ApplyProjection(item, props, storyboard);

                // Zindex and Opacity
                var deltaFromSelectedIndex = Math.Abs(this.Carousel.SelectedIndex - i);
                int zindex = (this.Carousel.Items.Count * 100) - deltaFromSelectedIndex;
                Canvas.SetZIndex(item, zindex);
            }

            // When storyboard completed, Invalidate
            storyboard.Completed += (sender, o) =>
            {
                this.isUpdatingPosition = false;
                this.ManipulationMode = ManipulationModes.All;
            };

            storyboard.Begin();
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

            for (int i = 0; i < this.Children.Count; i++)
            {
                FrameworkElement container = this.Children[i] as FrameworkElement;

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
                var deltaFromSelectedIndex = Math.Abs(this.Carousel.SelectedIndex - i);

                // Get rect position
                var rect = new Rect(centerLeft, centerTop, desiredWidth, desiredHeight);

                // Placing the rect in the center of screen
                container.Arrange(rect);

                // Get the initial projection (without move)
                var proj = GetProjectionFromSelectedIndex(i);

                // apply the projection to the current object
                this.ApplyProjection(container, proj);

                // calculate zindex and opacity
                int zindex = (this.Children.Count * 100) - deltaFromSelectedIndex;
                Canvas.SetZIndex(container, zindex);
            }

            return finalSize;
        }

        /// <summary>
        /// Apply the projection, with or without a storyboard involved
        /// </summary>
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
                compositeTransform.TranslateX = this.Carousel.Orientation == Orientation.Horizontal ? proj.Position : 0;
                compositeTransform.TranslateY = this.Carousel.Orientation == Orientation.Horizontal ? 0 : proj.Position;

                planeProjection.RotationX = proj.RotationX;
                planeProjection.RotationY = proj.RotationY;
                planeProjection.RotationZ = proj.RotationZ;

                planeProjection.GlobalOffsetZ = proj.Depth;
            }
            else
            {
                string localProjectionOrientation = this.Carousel.Orientation == Orientation.Horizontal ?
                                    "(UIElement.RenderTransform).(CompositeTransform.TranslateX)" : "(UIElement.RenderTransform).(CompositeTransform.TranslateY)";
                string localProjectionOrientationInvert = this.Carousel.Orientation == Orientation.Horizontal ?
                                    "(UIElement.RenderTransform).(CompositeTransform.TranslateY)" : "(UIElement.RenderTransform).(CompositeTransform.TranslateX)";

                string globalDepthProjection = "(UIElement.Projection).(PlaneProjection.GlobalOffsetZ)";
                string rotationXProjection = "(UIElement.Projection).(PlaneProjection.RotationX)";
                string rotationYProjection = "(UIElement.Projection).(PlaneProjection.RotationY)";
                string rotationZProjection = "(UIElement.Projection).(PlaneProjection.RotationZ)";

                AddAnimation(storyboard, element, this.Carousel.TransitionDuration, proj.Position, localProjectionOrientation, this.Carousel.EasingFunction);
                AddAnimation(storyboard, element, this.Carousel.TransitionDuration, 0, localProjectionOrientationInvert, this.Carousel.EasingFunction);
                AddAnimation(storyboard, element, this.Carousel.TransitionDuration, proj.Depth, globalDepthProjection, this.Carousel.EasingFunction);
                AddAnimation(storyboard, element, this.Carousel.TransitionDuration, proj.RotationX, rotationXProjection, this.Carousel.EasingFunction);
                AddAnimation(storyboard, element, this.Carousel.TransitionDuration, proj.RotationY, rotationYProjection, this.Carousel.EasingFunction);
                AddAnimation(storyboard, element, this.Carousel.TransitionDuration, proj.RotationZ, rotationZProjection, this.Carousel.EasingFunction);
            }
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
        /// Calculate a new projection after a manipulation delta
        /// </summary>
        /// <returns>Return the new projection</returns>
        private Proj GetProjectionFromManipulation(UIElement element, double delta)
        {
            PlaneProjection projection = element.Projection as PlaneProjection;
            CompositeTransform compositeTransform = element.RenderTransform as CompositeTransform;

            var bounds = this.Carousel.Orientation == Orientation.Horizontal ? desiredWidth : desiredHeight;
            var margin = this.Carousel.ItemMargin;

            // this maxsize from middle is the value when depth is max
            var maxBounds = bounds + margin;

            // Calculate the new position of the current item
            var newPosition = this.Carousel.Orientation == Orientation.Horizontal ? compositeTransform.TranslateX : compositeTransform.TranslateY;
            newPosition = newPosition + delta;

            // Calculate the relative position (to keep something positive)
            var relativePosition = Math.Abs(newPosition);

            // max Depth
            var depth = -this.Carousel.ItemDepth;

            // rotations
            var rotationX = this.Carousel.ItemRotationX;
            var rotationY = this.Carousel.ItemRotationY;
            var rotationZ = this.Carousel.ItemRotationZ;

            // if the relativeposition is inside the bounds so calculate the proportionals
            if (relativePosition <= maxBounds)
            {
                depth = relativePosition * depth / maxBounds;
                rotationX = relativePosition * this.Carousel.ItemRotationX / maxBounds;
                rotationY = relativePosition * this.Carousel.ItemRotationY / maxBounds;
                rotationZ = relativePosition * this.Carousel.ItemRotationZ / maxBounds;
            }

            if (this.Carousel.InvertPositive && newPosition > 0)
            {
                rotationX = -rotationX;
                rotationY = -rotationY;
                rotationZ = -rotationZ;
            }

            return new Proj { Position = newPosition, Depth = depth, RotationX = rotationX, RotationY = rotationY, RotationZ = rotationZ };
        }

        /// <summary>
        /// get the projection from a current index. Used On ArrangeOverride step
        /// </summary>
        /// <returns>Return the new projection</returns>
        private Proj GetProjectionFromSelectedIndex(int childIndex)
        {
            // margin
            var margin = this.Carousel.ItemMargin;

            // we want the middle image to be indice 0, to be sure the centered image is with no rotation
            var relativeIndex = childIndex - this.Carousel.SelectedIndex;

            // size beetween each element
            var widthOrHeight = this.Carousel.Orientation == Orientation.Horizontal ? desiredWidth : desiredHeight;

            // calculate the position with the margin applied
            double position = relativeIndex * (widthOrHeight + margin);

            // Depth orientation
            var depth = relativeIndex == 0 ? 0 : -Carousel.ItemDepth;

            // Rotation on each axes
            var rotationX = relativeIndex == 0 ? 0 : Carousel.ItemRotationX;
            var rotationY = relativeIndex == 0 ? 0 : Carousel.ItemRotationY;
            var rotationZ = relativeIndex == 0 ? 0 : Carousel.ItemRotationZ;

            if (this.Carousel.InvertPositive && relativeIndex > 0)
            {
                rotationX = -rotationX;
                rotationY = -rotationY;
                rotationZ = -rotationZ;
            }

            return new Proj { Position = position, Depth = depth, RotationX = rotationX, RotationY = rotationY, RotationZ = rotationZ };
        }

        /// <summary>
        /// Add an animation to the current storyboard
        /// </summary>
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
    }

    /// <summary>
    /// Structure used when an item moves
    /// </summary>
    public struct Proj
    {
        public double Position { get; set; }

        public double Depth { get; set; }

        public double RotationX { get; set; }

        public double RotationY { get; set; }

        public double RotationZ { get; set; }
    }
}