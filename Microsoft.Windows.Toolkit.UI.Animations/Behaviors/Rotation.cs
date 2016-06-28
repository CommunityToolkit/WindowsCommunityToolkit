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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Microsoft.Xaml.Interactivity;

namespace Microsoft.Windows.Toolkit.UI.Animations.Behaviors
{
    /// <summary>
    /// Performs a rotation animation using composition.
    /// </summary>
    /// <seealso>
    ///     <cref>Microsoft.Xaml.Interactivity.Behavior{Windows.UI.Xaml.UIElement}</cref>
    /// </seealso>
    public class Rotation : Behavior<UIElement>
    {
        /// <summary>
        /// Called after the behavior is attached to the <see cref="P:Microsoft.Xaml.Interactivity.Behavior.AssociatedObject" />.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the <see cref="P:Microsoft.Xaml.Interactivity.Behavior.AssociatedObject" />
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();

            if (AutomaticallyStart)
            {
                StartAnimation();
            }
        }

        /// <summary>
        /// The duration of the animation.
        /// </summary>
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(double), typeof(Opacity), new PropertyMetadata(1d));

        /// <summary>
        /// The delay of the animation.
        /// </summary>
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(double), typeof(Opacity), new PropertyMetadata(0d));

        /// <summary>
        /// The Opacity x of the associated object
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(Opacity), new PropertyMetadata(1d));

        /// <summary>
        /// The property sets if the animation should automatically start.
        /// </summary>
        public static readonly DependencyProperty AutomaticallyStartProperty = DependencyProperty.Register("AutomaticallyStart", typeof(bool), typeof(Opacity), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether [automatically start] on the animation is set.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatically start]; otherwise, <c>false</c>.
        /// </value>
        public bool AutomaticallyStart
        {
            get { return (bool)GetValue(AutomaticallyStartProperty); }
            set { SetValue(AutomaticallyStartProperty, value); }
        }

        /// <summary>
        /// Gets or sets the delay.
        /// </summary>
        /// <value>
        /// The delay.
        /// </value>
        public double Delay
        {
            get { return (double)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public double Duration
        {
            get { return (double)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Opacity x.
        /// </summary>
        /// <value>
        /// The Opacity x.
        /// </value>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        public void StartAnimation()
        {
            var visual = ElementCompositionPreview.GetElementVisual(AssociatedObject);
            var compositor = visual?.Compositor;

            if (compositor == null)
            {
                return;
            }

            var rotationAnimation = compositor.CreateScalarKeyFrameAnimation();
            rotationAnimation.Duration = TimeSpan.FromSeconds(Duration);
            rotationAnimation.DelayTime = TimeSpan.FromSeconds(Delay);
            rotationAnimation.InsertKeyFrame(1f, (float)Value);

            visual.StartAnimation("RotationAngleInDegrees", rotationAnimation);
        }
    }
}