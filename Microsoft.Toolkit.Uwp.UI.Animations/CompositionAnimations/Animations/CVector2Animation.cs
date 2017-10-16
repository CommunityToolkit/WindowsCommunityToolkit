using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    public class CVector2Animation : CTypedAnimation<CVector2KeyFrame, string>
    {
        protected override KeyFrameAnimation GetTypedAnimationFromCompositor(Compositor compositor)
        {
            return compositor.CreateVector2KeyFrameAnimation();
        }

        protected override void InsertKeyFrameToTypedAnimation(KeyFrameAnimation animation, CVector2KeyFrame keyFrame)
        {
            (animation as Vector2KeyFrameAnimation).InsertKeyFrame((float)keyFrame.Key, keyFrame.Value.ToVector2());
        }
    }
}
