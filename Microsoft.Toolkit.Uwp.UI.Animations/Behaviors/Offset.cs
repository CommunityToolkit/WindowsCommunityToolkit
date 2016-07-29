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

using Microsoft.Toolkit.Uwp.UI.Animations.Extensions;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Behaviors
{
    /// <summary>
    /// Performs an offset animation using composition.
    /// </summary>
    /// <seealso cref="Microsoft.Toolkit.Uwp.UI.Animations.Behaviors.CompositionBehaviorBase" />
    /// <seealso>
    ///   <cref>Microsoft.Xaml.Interactivity.Behavior{Windows.UI.Xaml.UIElement}</cref>
    /// </seealso>
    public class Offset : CompositionBehaviorBase
    {
        /// <summary>
        /// The Offset on the x axis of the associated object
        /// </summary>
        public static readonly DependencyProperty OffsetXProperty = DependencyProperty.Register("OffsetX", typeof(double), typeof(Offset), new PropertyMetadata(1d, PropertyChangedCallback));

        /// <summary>
        /// The Offset on the y axis of the associated object
        /// </summary>
        public static readonly DependencyProperty OffsetYProperty = DependencyProperty.Register("OffsetY", typeof(double), typeof(Offset), new PropertyMetadata(1d, PropertyChangedCallback));

        /// <summary>
        /// The Offset on the z axis of the associated object
        /// </summary>
        public static readonly DependencyProperty OffsetZProperty = DependencyProperty.Register("OffsetZ", typeof(double), typeof(Offset), new PropertyMetadata(1d, PropertyChangedCallback));

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
        /// Starts the animation.
        /// </summary>
        public override void StartAnimation()
        {
            AssociatedObject.Offset(
                duration: Duration,
                delay: Delay,
                offsetX: (float)OffsetX,
                offsetY: (float)OffsetY,
                offsetZ: (float)OffsetZ)?
                .StartAsync();
        }
    }
}