using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Animation that animates a value of type <see cref="Vector2"/>
    /// </summary>
    public class Vector2Animation : TypedAnimationBase<Vector2KeyFrame, string>
    {
        /// <inheritdoc/>
        protected override KeyFrameAnimation GetTypedAnimationFromCompositor(Compositor compositor)
        {
            return compositor.CreateVector2KeyFrameAnimation();
        }

        /// <inheritdoc/>
        protected override void InsertKeyFrameToTypedAnimation(KeyFrameAnimation animation, Vector2KeyFrame keyFrame)
        {
            (animation as Vector2KeyFrameAnimation).InsertKeyFrame((float)keyFrame.Key, keyFrame.Value.ToVector2());
        }
    }
}
