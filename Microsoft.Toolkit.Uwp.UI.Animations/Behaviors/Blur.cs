// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Behaviors
{
    /// <summary>
    /// Performs an blur animation using composition.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Blurs are only supported on Build 1607 and up. Assigning the blur behavior on an older
    /// version of Windows will not add any effect. You can use the <see cref="AnimationExtensions.IsBlurSupported"/>
    /// property to check for whether blurs are supported on the device at runtime.
    /// </para>
    /// </remarks>
    /// <seealso>
    ///     <cref>Microsoft.Xaml.Interactivity.Behavior{Windows.UI.Xaml.UIElement}</cref>
    /// </seealso>
    /// <seealso cref="AnimationExtensions.IsBlurSupported"/>
    public class Blur : CompositionBehaviorBase<FrameworkElement>
    {
        /// <summary>
        /// The Blur value of the associated object
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(Blur), new PropertyMetadata(0d, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the Blur.
        /// </summary>
        /// <value>
        /// The Blur.
        /// </value>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Starts the animation.
        /// </summary>
        public override void StartAnimation()
        {
            if (AnimationExtensions.BlurEffect.IsSupported)
            {
                AssociatedObject?.Blur(duration: Duration, delay: Delay, value: (float)Value, easingType: EasingType, easingMode: EasingMode)?.Start();
            }
        }
    }
}
