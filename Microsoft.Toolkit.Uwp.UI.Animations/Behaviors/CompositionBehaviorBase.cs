// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Behaviors
{
    /// <summary>
    /// A base class for all behaviors using composition.It contains some of the common properties to set on a visual.
    /// </summary>
    /// <typeparam name="T">The type of the associated object.</typeparam>
    /// <seealso cref="Microsoft.Toolkit.Uwp.UI.Animations.Behaviors.BehaviorBase{T}" />
    public abstract class CompositionBehaviorBase<T> : BehaviorBase<T>
        where T : UIElement
    {
        /// <summary>
        /// Called when the associated object has been loaded.
        /// </summary>
        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();

            if (AutomaticallyStart)
            {
                StartAnimation();
            }
        }

        /// <summary>
        /// The duration of the animation.
        /// </summary>
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(nameof(Duration), typeof(double), typeof(CompositionBehaviorBase<T>), new PropertyMetadata(1d, PropertyChangedCallback));

        /// <summary>
        /// The delay of the animation.
        /// </summary>
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(nameof(Delay), typeof(double), typeof(CompositionBehaviorBase<T>), new PropertyMetadata(0d, PropertyChangedCallback));

        /// <summary>
        /// The property sets if the animation should automatically start.
        /// </summary>
        public static readonly DependencyProperty AutomaticallyStartProperty = DependencyProperty.Register(nameof(AutomaticallyStart), typeof(bool), typeof(CompositionBehaviorBase<T>), new PropertyMetadata(true, PropertyChangedCallback));

        /// <summary>
        /// The <see cref="EasingType"/> used to generate the easing function of the animation.
        /// </summary>
        public static readonly DependencyProperty EasingTypeProperty = DependencyProperty.Register(nameof(EasingType), typeof(EasingType), typeof(CompositionBehaviorBase<T>), new PropertyMetadata(EasingType.Default, PropertyChangedCallback));

        /// <summary>
        /// The <see cref="EasingMode"/> used to generate the easing function of the animation.
        /// </summary>
        public static readonly DependencyProperty EasingModeProperty = DependencyProperty.Register(nameof(EasingMode), typeof(EasingMode), typeof(CompositionBehaviorBase<T>), new PropertyMetadata(EasingMode.EaseOut, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets a value indicating whether [automatically start] on the animation is set.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatically start]; otherwise, <c>false</c>.
        /// </value>
        public bool AutomaticallyStart
        {
            get { return (bool)GetValue(AutomaticallyStartProperty); }
            set { SetValue(AutomaticallyStartProperty, value); }
        }

        /// <summary>
        /// Gets or sets the delay.
        /// </summary>
        /// <value>
        /// The delay.
        /// </value>
        public double Delay
        {
            get { return (double)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public double Duration
        {
            get { return (double)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="EasingType"/> used to generate the easing function of the animation.
        /// </summary>
        /// <value>
        /// The easing function
        /// </value>
        public EasingType EasingType
        {
            get { return (EasingType)GetValue(EasingTypeProperty); }
            set { SetValue(EasingTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="EasingMode"/> used to generate the easing function of the animation.
        /// </summary>
        /// <value>
        /// The easing mode
        /// </value>
        public EasingMode EasingMode
        {
            get { return (EasingMode)GetValue(EasingModeProperty); }
            set { SetValue(EasingModeProperty, value); }
        }

        /// <summary>
        /// Starts the animation.
        /// </summary>
        public abstract void StartAnimation();

        /// <summary>
        /// If any of the properties are changed then the animation is automatically started depending on the AutomaticallyStart property.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="dependencyPropertyChangedEventArgs">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var behavior = dependencyObject as CompositionBehaviorBase<T>;
            if (behavior == null)
            {
                return;
            }

            if (behavior.AutomaticallyStart)
            {
                behavior.StartAnimation();
            }
        }
    }
}
