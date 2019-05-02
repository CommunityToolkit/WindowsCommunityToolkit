// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
    public class Offset : CompositionBehaviorBase<UIElement>
    {
        /// <summary>
        /// The Offset on the x axis of the associated object
        /// </summary>
        public static readonly DependencyProperty OffsetXProperty = DependencyProperty.Register(nameof(OffsetX), typeof(double), typeof(Offset), new PropertyMetadata(0d, PropertyChangedCallback));

        /// <summary>
        /// The Offset on the y axis of the associated object
        /// </summary>
        public static readonly DependencyProperty OffsetYProperty = DependencyProperty.Register(nameof(OffsetY), typeof(double), typeof(Offset), new PropertyMetadata(0d, PropertyChangedCallback));

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
        /// Starts the animation.
        /// </summary>
        public override void StartAnimation()
        {
            AssociatedObject.Offset(
                duration: Duration,
                delay: Delay,
                easingType: EasingType,
                easingMode: EasingMode,
                offsetX: (float)OffsetX,
                offsetY: (float)OffsetY)?.Start();
        }
    }
}