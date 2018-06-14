// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
