// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Behaviors
{
    /// <summary>
    /// Performs a rotation animation using composition.
    /// </summary>
    public class Rotate : CompositionBehaviorBase<UIElement>
    {
        /// <summary>
        /// The rotation of the associated object in degrees
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(Rotate), new PropertyMetadata(0d, PropertyChangedCallback));

        /// <summary>
        /// The center (x axis) of rotation for associated object
        /// </summary>
        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register(nameof(CenterX), typeof(double), typeof(Rotate), new PropertyMetadata(0d, PropertyChangedCallback));

        /// <summary>
        /// The center (y axis) of rotation for associated object
        /// </summary>
        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register(nameof(CenterY), typeof(double), typeof(Rotate), new PropertyMetadata(0d, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the center point (x axis) of the associated object.
        /// </summary>
        /// <value>
        /// The center point (x axis) of the associated object.
        /// </value>
        public double CenterX
        {
            get { return (double)GetValue(CenterXProperty); }
            set { SetValue(CenterXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the center point (y axis) of the associated object.
        /// </summary>
        /// <value>
        /// The center point (y axis) of the associated object.
        /// </value>
        public double CenterY
        {
            get { return (double)GetValue(CenterYProperty); }
            set { SetValue(CenterYProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Rotation in degrees.
        /// </summary>
        /// <value>
        /// The Rotation of the associated object in degrees.
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
            AssociatedObject.Rotate(
                duration: Duration,
                delay: Delay,
                easingType: EasingType,
                easingMode: EasingMode,
                value: (float)Value,
                centerX: (float)CenterX,
                centerY: (float)CenterY)?
                .Start();
        }
    }
}