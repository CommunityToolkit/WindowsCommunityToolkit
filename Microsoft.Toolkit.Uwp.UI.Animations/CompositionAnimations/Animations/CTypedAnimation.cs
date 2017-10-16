using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// 
    /// 
    /// </summary>
    /// <typeparam name="T">Type of <see cref="CTypedKeyFrame{U}" to use/></typeparam>
    /// <typeparam name="U">Type of value being animated. Only nullable types supported</typeparam>
    public abstract class CTypedAnimation<T, U> : CAnimation where T : CTypedKeyFrame<U>, new()
    {
        private T FromKeyFrame;
        private T ToKeyFrame;

        // Using a DependencyProperty as the backing store for From.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From", typeof(U), typeof(CTypedAnimation<T, U>), new PropertyMetadata(GetDefaultValue(), OnAnimationPropertyChanged));

        // Using a DependencyProperty as the backing store for To.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To", typeof(U), typeof(CTypedAnimation<T, U>), new PropertyMetadata(GetDefaultValue(), OnAnimationPropertyChanged));

        public U From
        {
            get { return (U)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        public U To
        {
            get { return (U)GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        public override CompositionAnimation GetCompositionAnimation(Visual visual)
        {
            var compositor = visual.Compositor;

            if (string.IsNullOrWhiteSpace(Target))
            {
                return null;
            }

            PrepareKeyFrames();
            var animation = GetTypedAnimationFromCompositor(compositor);
            animation.Target = Target;
            animation.Duration = Duration;
            animation.DelayTime = Delay;

            if (KeyFrames.Count == 0)
            {
                animation.InsertExpressionKeyFrame(1.0f, "this.FinalValue");
                return animation;
            }

            foreach (var keyFrame in KeyFrames)
            {
                if (keyFrame is T typedKeyFrame)
                {
                    //animation.InsertKeyFrame((float)keyFrame.Key, vectorKeyFrame.Value);
                    InsertKeyFrameToTypedAnimation(animation, typedKeyFrame);
                }
                else if (keyFrame is CExpressionKeyFrame expressionKeyFrame)
                {
                    animation.InsertExpressionKeyFrame((float)keyFrame.Key, expressionKeyFrame.Value);
                }
            }

            return animation;
        }

        protected void PrepareKeyFrames()
        {
            if (FromKeyFrame != null)
                KeyFrames.Remove(FromKeyFrame);

            if (ToKeyFrame != null)
                KeyFrames.Remove(ToKeyFrame);

            if (!IsValueNull(From))
            {
                FromKeyFrame = new T();
                FromKeyFrame.Key = 0f;
                FromKeyFrame.Value = From;
                KeyFrames.Add(FromKeyFrame);
            }

            if (!IsValueNull(To))
            {
                ToKeyFrame = new T();
                ToKeyFrame.Key = 1f;
                ToKeyFrame.Value = To;
                KeyFrames.Add(ToKeyFrame);
            }
        }

        protected abstract KeyFrameAnimation GetTypedAnimationFromCompositor(Compositor compositor);

        protected abstract void InsertKeyFrameToTypedAnimation(KeyFrameAnimation animation, T keyFrame);

        // these two methods are required to support double (non nullable type)
        private static object GetDefaultValue()
        {
            if (typeof(U) == typeof(double))
                return double.NaN;

            return default(U);
        }

        private static bool IsValueNull(U value)
        {
            if (typeof(U) == typeof(double))
                return double.IsNaN((double)(object)value);

            return value == null;
        }
    }
}
