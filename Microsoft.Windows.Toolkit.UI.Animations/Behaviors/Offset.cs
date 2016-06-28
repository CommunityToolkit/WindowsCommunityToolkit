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
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Microsoft.Xaml.Interactivity;

namespace Microsoft.Windows.Toolkit.UI.Animations.Behaviors
{
    /// <summary>
    /// Performs an offset animation using composition.
    /// </summary>
    /// <seealso>
    ///     <cref>Microsoft.Xaml.Interactivity.Behavior{Windows.UI.Xaml.UIElement}</cref>
    /// </seealso>
    public class Offset : Behavior<UIElement>
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
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(double), typeof(Offset), new PropertyMetadata(1d));

        /// <summary>
        /// The delay of the animation.
        /// </summary>
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(double), typeof(Offset), new PropertyMetadata(0d));

        /// <summary>
        /// The Offset x of the associated object
        /// </summary>
        public static readonly DependencyProperty OffsetXProperty = DependencyProperty.Register("OffsetX", typeof(double), typeof(Offset), new PropertyMetadata(1d));

        /// <summary>
        /// The Offset y of the associated object
        /// </summary>
        public static readonly DependencyProperty OffsetYProperty = DependencyProperty.Register("OffsetY", typeof(double), typeof(Offset), new PropertyMetadata(1d));

        /// <summary>
        /// The Offset z of the associated object
        /// </summary>
        public static readonly DependencyProperty OffsetZProperty = DependencyProperty.Register("OffsetZ", typeof(double), typeof(Offset), new PropertyMetadata(1d));

        /// <summary>
        /// The property sets if the animation should automatically start.
        /// </summary>
        public static readonly DependencyProperty AutomaticallyStartProperty = DependencyProperty.Register("AutomaticallyStart", typeof(bool), typeof(Offset), new PropertyMetadata(true));

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
        /// Gets or sets the Offset x.
        /// </summary>
        /// <value>
        /// The Offset x.
        /// </value>
        public double OffsetX
        {
            get { return (double)GetValue(OffsetXProperty); }
            set { SetValue(OffsetXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Offset y.
        /// </summary>
        /// <value>
        /// The Offset y.
        /// </value>
        public double OffsetY
        {
            get { return (double)GetValue(OffsetYProperty); }
            set { SetValue(OffsetYProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Offset z.
        /// </summary>
        /// <value>
        /// The Offset z.
        /// </value>
        public double OffsetZ
        {
            get { return (double)GetValue(OffsetZProperty); }
            set { SetValue(OffsetZProperty, value); }
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

            var offsetAnimation = compositor.CreateVector3KeyFrameAnimation();
            offsetAnimation.Duration = TimeSpan.FromSeconds(Duration);
            offsetAnimation.DelayTime = TimeSpan.FromSeconds(Delay);
            offsetAnimation.InsertKeyFrame(1f, new Vector3((float)OffsetX, (float)OffsetY, (float)OffsetZ));
            visual.StartAnimation("Offset", offsetAnimation);
        }
    }
}