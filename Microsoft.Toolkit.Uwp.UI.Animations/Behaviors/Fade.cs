// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Behaviors
{
    /// <summary>
    /// Performs an fade animation using composition.
    /// </summary>
    /// <seealso>
    ///     <cref>Microsoft.Xaml.Interactivity.Behavior{Windows.UI.Xaml.UIElement}</cref>
    /// </seealso>
    public class Fade : CompositionBehaviorBase<UIElement>
    {
        /// <summary>
        /// The Opacity value of the associated object
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(Fade), new PropertyMetadata(1d, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the Opacity.
        /// </summary>
        /// <value>
        /// The Opacity.
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
            AssociatedObject.Fade((float)Value, Duration, Delay, EasingType, EasingMode)?.Start();
        }
    }
}