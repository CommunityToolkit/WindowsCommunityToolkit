using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Behaviors
{
    public class Saturation : CompositionBehaviorBase
    {
        /// <summary>
        /// The _framework element
        /// </summary>
        private FrameworkElement _frameworkElement;

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

        /// <summary>
        /// The Blur value of the associated object
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(Saturation), new PropertyMetadata(0d, PropertyChangedCallback));

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
            if (AnimationExtensions.SaturationEffect.IsSupported)
            {
                _frameworkElement?.Saturation(duration: Duration, delay: Delay, value: (float)Value)?.StartAsync();
            }
        }
    }
}
