using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    public class CVector4Animation : CTypedAnimation<CVector4KeyFrame, string>
    {
        protected override KeyFrameAnimation GetTypedAnimationFromCompositor(Compositor compositor)
        {
            return compositor.CreateVector4KeyFrameAnimation();
        }

        protected override void InsertKeyFrameToTypedAnimation(KeyFrameAnimation animation, CVector4KeyFrame keyFrame)
        {
            (animation as Vector4KeyFrameAnimation).InsertKeyFrame((float)keyFrame.Key, keyFrame.Value.ToVector4());
        }
    }
}
