using Microsoft.Toolkit.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Animations.Xaml;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors.Animations
{
    /// <summary>
    /// An <see cref="IAction"/> implementation that can trigger a target <see cref="AnimationCollection2"/> instance.
    /// </summary>
    public sealed class StartAnimationAction : DependencyObject, IAction
    {
        /// <summary>
        /// Gets or sets the linked <see cref="AnimationCollection2"/> instance to invoke.
        /// </summary>
        public AnimationCollection2 Animation
        {
            get => (AnimationCollection2)GetValue(AnimationProperty);
            set => SetValue(AnimationProperty, value);
        }

        /// <summary>
        /// Identifies the <seealso cref="Animation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AnimationProperty = DependencyProperty.Register(
            "Animation",
            typeof(AnimationCollection2),
            typeof(StartAnimationAction),
            new PropertyMetadata(null));

        /// <inheritdoc/>
        public object Execute(object sender, object parameter)
        {
            Guard.IsNotNull(Animation, nameof(Animation));

            Animation.Start();

            return null!;
        }
    }
}
