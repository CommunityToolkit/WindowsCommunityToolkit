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

namespace Microsoft.Windows.Toolkit.UI.Animations.Behaviors
{
    /// <summary>
    /// Performs a rotation animation using composition.
    /// </summary>
    public class Rotation : CompositionBehaviorBase
    {
        /// <summary>
        /// The Opacity x of the associated object
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(Opacity), new PropertyMetadata(1d, PropertyChangedCallback));

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
        public override void StartAnimation()
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