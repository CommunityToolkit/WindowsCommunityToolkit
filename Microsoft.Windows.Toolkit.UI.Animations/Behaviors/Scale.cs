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
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Windows.Toolkit.UI.Animations.Behaviors
{
    /// <summary>
    /// Performs a scale animation using composition.
    /// </summary>
    /// <seealso>
    ///     <cref>Microsoft.Xaml.Interactivity.Behavior{Windows.UI.Xaml.UIElement}</cref>
    /// </seealso>
    public class Scale : Behavior<UIElement>
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
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(double), typeof(Scale), new PropertyMetadata(1d));

        /// <summary>
        /// The delay of the animation.
        /// </summary>
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(double), typeof(Scale), new PropertyMetadata(0d));

        /// <summary>
        /// The scale x of the associated object
        /// </summary>
        public static readonly DependencyProperty ScaleXProperty = DependencyProperty.Register("ScaleX", typeof(double), typeof(Scale), new PropertyMetadata(1d));

        /// <summary>
        /// The scale y of the associated object
        /// </summary>
        public static readonly DependencyProperty ScaleYProperty = DependencyProperty.Register("ScaleY", typeof(double), typeof(Scale), new PropertyMetadata(1d));

        /// <summary>
        /// The scale z of the associated object
        /// </summary>
        public static readonly DependencyProperty ScaleZProperty = DependencyProperty.Register("ScaleZ", typeof(double), typeof(Scale), new PropertyMetadata(1d));

        /// <summary>
        /// The property sets if the animation should automatically start.
        /// </summary>
        public static readonly DependencyProperty AutomaticallyStartProperty = DependencyProperty.Register("AutomaticallyStart", typeof(bool), typeof(Scale), new PropertyMetadata(true));

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
        /// Gets or sets the scale x.
        /// </summary>
        /// <value>
        /// The scale x.
        /// </value>
        public double ScaleX
        {
            get { return (double)GetValue(ScaleXProperty); }
            set { SetValue(ScaleXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the scale y.
        /// </summary>
        /// <value>
        /// The scale y.
        /// </value>
        public double ScaleY
        {
            get { return (double)GetValue(ScaleYProperty); }
            set { SetValue(ScaleYProperty, value); }
        }

        /// <summary>
        /// Gets or sets the scale z.
        /// </summary>
        /// <value>
        /// The scale z.
        /// </value>
        public double ScaleZ
        {
            get { return (double)GetValue(ScaleZProperty); }
            set { SetValue(ScaleZProperty, value); }
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

            var scaleAnimation = compositor.CreateVector3KeyFrameAnimation();
            scaleAnimation.Duration = TimeSpan.FromSeconds(Duration);
            scaleAnimation.DelayTime = TimeSpan.FromSeconds(Delay);
            scaleAnimation.InsertKeyFrame(1f, new Vector3((float)ScaleX, (float)ScaleY, (float)ScaleZ));
            visual.StartAnimation("Scale", scaleAnimation);
        }
    }
}