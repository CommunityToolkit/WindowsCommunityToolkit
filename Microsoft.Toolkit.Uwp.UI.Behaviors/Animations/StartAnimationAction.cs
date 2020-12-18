using Microsoft.Toolkit.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Animations.Xaml;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors.Animations
{
    /// <summary>
    /// An <see cref="IAction"/> implementation that can trigger a target <see cref="ITimeline"/> animation.
    /// </summary>
    public sealed class StartAnimationAction : DependencyObject, IAction
    {
        /// <summary>
        /// Gets or sets the linked <see cref="ITimeline"/> animation to invoke.
        /// </summary>
        public ITimeline Animation
        {
            get => (ITimeline)GetValue(AnimationProperty);
            set => SetValue(AnimationProperty, value);
        }

        /// <summary>
        /// Identifies the <seealso cref="Animation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AnimationProperty = DependencyProperty.Register(
            "Animation",
            typeof(ITimeline),
            typeof(StartAnimationAction),
            new PropertyMetadata(null));

        /// <inheritdoc/>
        public object Execute(object sender, object parameter)
        {
            Guard.IsNotNull(sender, nameof(sender));
            Guard.IsAssignableToType<UIElement>(sender, nameof(sender));
            Guard.IsNotNull(Animation, nameof(Animation));

            Animation.AppendToBuilder(new AnimationBuilder()).Start((UIElement)sender);

            return null;
        }
    }
}
