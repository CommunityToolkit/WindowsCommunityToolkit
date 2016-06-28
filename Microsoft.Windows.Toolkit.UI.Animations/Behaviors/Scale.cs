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
    public class Scale : CompositionBehaviorBase
    {
        /// <summary>
        /// The scale x of the associated object
        /// </summary>
        public static readonly DependencyProperty ScaleXProperty = DependencyProperty.Register("ScaleX", typeof(double), typeof(Scale), new PropertyMetadata(1d, PropertyChangedCallback));

        /// <summary>
        /// The scale y of the associated object
        /// </summary>
        public static readonly DependencyProperty ScaleYProperty = DependencyProperty.Register("ScaleY", typeof(double), typeof(Scale), new PropertyMetadata(1d, PropertyChangedCallback));

        /// <summary>
        /// The scale z of the associated object
        /// </summary>
        public static readonly DependencyProperty ScaleZProperty = DependencyProperty.Register("ScaleZ", typeof(double), typeof(Scale), new PropertyMetadata(1d, PropertyChangedCallback));

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
        public override void StartAnimation()
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