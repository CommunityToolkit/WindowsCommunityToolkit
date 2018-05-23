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

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Behaviors
{
    /// <summary>
    /// Applies a basic point light to a UIElement. You control the intensity by setting the distance of the light.
    /// </summary>
    /// <seealso cref="Microsoft.Toolkit.Uwp.UI.Animations.Behaviors.CompositionBehaviorBase" />
    /// <seealso cref="AnimationExtensions.IsLightingSupported"/>
    public class Light : CompositionBehaviorBase<FrameworkElement>
    {
        /// <summary>
        /// The Blur value of the associated object
        /// </summary>
        public static readonly DependencyProperty DistanceProperty = DependencyProperty.Register(nameof(Distance), typeof(double), typeof(Light), new PropertyMetadata(0d, PropertyChangedCallback));

        /// <summary>
        /// The Color of the spotlight no the associated object.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(Light), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        /// <summary>
        /// Gets or sets the Blur.
        /// </summary>
        /// <value>
        /// The Blur.
        /// </value>
        public double Distance
        {
            get { return (double)GetValue(DistanceProperty); }
            set { SetValue(DistanceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the spotlight.
        /// </summary>
        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// Starts the animation.
        /// </summary>
        public override void StartAnimation()
        {
            if (AnimationExtensions.IsLightingSupported)
            {
                AssociatedObject?.Light(
                    duration: Duration,
                    delay: Delay,
                    easingType: EasingType,
                    easingMode: EasingMode,
                    distance: (float)Distance,
                    color: ((SolidColorBrush)Color).Color)?.Start();
            }
        }
    }
}
