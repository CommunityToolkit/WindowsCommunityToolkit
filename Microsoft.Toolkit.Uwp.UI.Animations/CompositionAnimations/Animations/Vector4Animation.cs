using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Animation that animates a value of type <see cref="Vector4"/>
    /// </summary>
    public class Vector4Animation : TypedAnimationBase<Vector4KeyFrame, string>
    {
        /// <inheritdoc/>
        protected override KeyFrameAnimation GetTypedAnimationFromCompositor(Compositor compositor)
        {
            return compositor.CreateVector4KeyFrameAnimation();
        }

        /// <inheritdoc/>
        protected override void InsertKeyFrameToTypedAnimation(KeyFrameAnimation animation, Vector4KeyFrame keyFrame)
        {
            (animation as Vector4KeyFrameAnimation).InsertKeyFrame((float)keyFrame.Key, keyFrame.Value.ToVector4());
        }
    }
}
