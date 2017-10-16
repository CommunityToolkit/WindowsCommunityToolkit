using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    public class CScalarAnimation : CTypedAnimation<CScalarKeyFrame, double>
    {
        protected override KeyFrameAnimation GetTypedAnimationFromCompositor(Compositor compositor)
        {
            return compositor.CreateScalarKeyFrameAnimation();
        }

        protected override void InsertKeyFrameToTypedAnimation(KeyFrameAnimation animation, CScalarKeyFrame keyFrame)
        {
            (animation as ScalarKeyFrameAnimation).InsertKeyFrame((float)keyFrame.Key, (float)keyFrame.Value);
        }
    }
}
