// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Behaviors
{
    /// <summary>
    /// A behavior to allow Saturation changes to a UI Element.
    /// </summary>
    public class Saturation : CompositionBehaviorBase
    {
        /// <summary>
        /// The Saturation value of the associated object
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(Saturation), new PropertyMetadata(0d, PropertyChangedCallback));

        /// <summary>
        /// The _framework element
        /// </summary>
        private FrameworkElement _frameworkElement;

        /// <summary>
        /// Gets or sets the Saturation.
        /// </summary>
        /// <value>
        /// The Saturation.
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
            if (AnimationExtensions.SaturationEffect.IsSupported)
            {
                _frameworkElement?.Saturation(
                    duration: Duration,
                    delay: Delay,
                    easingType: EasingType,
                    easingMode: EasingMode,
                    value: (float)Value)?.StartAsync();
            }
        }

        /// <summary>
        /// Called after the behavior is attached to the <see cref="P:Microsoft.Xaml.Interactivity.Behavior.AssociatedObject" />.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the <see cref="P:Microsoft.Xaml.Interactivity.Behavior.AssociatedObject" />
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            _frameworkElement = AssociatedObject as FrameworkElement;
        }
    }
}
