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

namespace Microsoft.Windows.Toolkit.UI.Animations.Behaviors
{
    /// <summary>
    /// Performs a resize animation using composition.
    /// </summary>
    /// <seealso cref="Microsoft.Windows.Toolkit.UI.Animations.Behaviors.CompositionBehaviorBase" />
    public class Resize : CompositionBehaviorBase
    {
        /// <summary>
        /// The Resize x of the associated object
        /// </summary>
        public static readonly DependencyProperty ResizeXProperty = DependencyProperty.Register("ResizeX", typeof(double), typeof(Resize), new PropertyMetadata(1d, PropertyChangedCallback));

        /// <summary>
        /// The Resize y of the associated object
        /// </summary>
        public static readonly DependencyProperty ResizeYProperty = DependencyProperty.Register("ResizeY", typeof(double), typeof(Resize), new PropertyMetadata(1d, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the Resize x.
        /// </summary>
        /// <value>
        /// The Resize x.
        /// </value>
        public double ResizeX
        {
            get { return (double)GetValue(ResizeXProperty); }
            set { SetValue(ResizeXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Resize y.
        /// </summary>
        /// <value>
        /// The Resize y.
        /// </value>
        public double ResizeY
        {
            get { return (double)GetValue(ResizeYProperty); }
            set { SetValue(ResizeYProperty, value); }
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

            var resizeAnimation = compositor.CreateVector2KeyFrameAnimation();
            resizeAnimation.Duration = TimeSpan.FromSeconds(Duration);
            resizeAnimation.DelayTime = TimeSpan.FromSeconds(Delay);
            resizeAnimation.InsertKeyFrame(1f, new Vector2((float)ResizeX, (float)ResizeY));
            visual.StartAnimation("Size", resizeAnimation);
        }
    }
}