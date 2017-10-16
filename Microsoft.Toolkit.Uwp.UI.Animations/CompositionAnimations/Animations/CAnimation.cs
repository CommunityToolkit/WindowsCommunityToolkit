using System;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    [ContentProperty(Name = nameof(KeyFrames))]
    public abstract class CAnimation : DependencyObject
    {
        public event EventHandler AnimationChanged;

        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public CKeyFrameCollection KeyFrames
        {
            get
            {
                var collection = (CKeyFrameCollection)GetValue(ScalarKeyFramesProperty);
                if (collection == null)
                {
                    collection = new CKeyFrameCollection();
                    SetValue(ScalarKeyFramesProperty, collection);
                }

                return collection;
            }
            set { SetValue(ScalarKeyFramesProperty, value); }
        }

        public string Target
        {
            get { return (string)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        public string ImplicitTarget
        {
            get { return (string)GetValue(ImplicitTargetProperty); }
            set { SetValue(ImplicitTargetProperty, value); }
        }

        public TimeSpan Delay
        {
            get { return (TimeSpan)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Target.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target",
                                        typeof(string),
                                        typeof(CAnimation),
                                        new PropertyMetadata(string.Empty, OnAnimationPropertyChanged));

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration",
                                        typeof(TimeSpan),
                                        typeof(CAnimation),
                                        new PropertyMetadata(TimeSpan.FromMilliseconds(400), OnAnimationPropertyChanged));

        // Using a DependencyProperty as the backing store for ScalarKeyFrames.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScalarKeyFramesProperty =
            DependencyProperty.Register("KeyFrames",
                                        typeof(CKeyFrameCollection),
                                        typeof(CAnimation),
                                        new PropertyMetadata(null, OnAnimationPropertyChanged));

        // Using a DependencyProperty as the backing store for ImplicitTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImplicitTargetProperty =
            DependencyProperty.Register("ImplicitTarget",
                                        typeof(string),
                                        typeof(CAnimation),
                                        new PropertyMetadata(null, OnAnimationPropertyChanged));

        // Using a DependencyProperty as the backing store for Delay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register("Delay",
                                        typeof(TimeSpan),
                                        typeof(CAnimation),
                                        new PropertyMetadata(TimeSpan.Zero, OnAnimationPropertyChanged));

        protected static void OnAnimationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CAnimation).AnimationChanged?.Invoke(d, null);
        }

        public abstract CompositionAnimation GetCompositionAnimation(Visual visual);
    }
}
