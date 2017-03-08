using Microsoft.Toolkit.Uwp.UI.Animations.Effects;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    public static partial class AnimationExtensions
    {
        public static Saturation SaturationEffect { get; } = new Saturation();

        public static AnimationSet Saturation(
            this FrameworkElement associatedObject,
            double value = 0d,
            double duration = 500d,
            double delay = 0d)
        {
            if (associatedObject == null)
            {
                return null;
            }

            var animationSet = new AnimationSet(associatedObject);
            return animationSet.Saturation(value, duration, delay);
        }

        public static AnimationSet Saturation(
            this AnimationSet animationSet,
            double value = 0d,
            double duration = 500d,
            double delay = 0d)
        {
            return SaturationEffect.EffectAnimation(animationSet, value, duration, delay);
        }
    }
}
