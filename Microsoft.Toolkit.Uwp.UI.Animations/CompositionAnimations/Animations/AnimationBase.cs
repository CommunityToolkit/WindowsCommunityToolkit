using System;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Abstract class providing common dependency properties for composition animations
    /// </summary>
    [ContentProperty(Name = nameof(KeyFrames))]
    public abstract class AnimationBase : DependencyObject
    {
        /// <summary>
        /// Identifies the <see cref="Target"/> property
        /// </summary>
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(string), typeof(AnimationBase), new PropertyMetadata(string.Empty, OnAnimationPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="Duration"/> property
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(AnimationBase), new PropertyMetadata(TimeSpan.FromMilliseconds(400), OnAnimationPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="KeyFrames"/> property
        /// </summary>
        public static readonly DependencyProperty ScalarKeyFramesProperty =
            DependencyProperty.Register("KeyFrames", typeof(KeyFrameCollection), typeof(AnimationBase), new PropertyMetadata(null, OnAnimationPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="ImplicitTarget"/> property
        /// </summary>
        public static readonly DependencyProperty ImplicitTargetProperty =
            DependencyProperty.Register("ImplicitTarget", typeof(string), typeof(AnimationBase), new PropertyMetadata(null, OnAnimationPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="Delay"/> property
        /// </summary>
        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register("Delay", typeof(TimeSpan), typeof(AnimationBase), new PropertyMetadata(TimeSpan.Zero, OnAnimationPropertyChanged));

        /// <summary>
        /// Raised when a property changes
        /// </summary>
        public event EventHandler AnimationChanged;

        /// <summary>
        /// Gets or sets the duration of the animation
        /// </summary>
        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="KeyFrameCollection"/> of the animations
        /// </summary>
        public KeyFrameCollection KeyFrames
        {
            get
            {
                var collection = (KeyFrameCollection)GetValue(ScalarKeyFramesProperty);
                if (collection == null)
                {
                    collection = new KeyFrameCollection();
                    SetValue(ScalarKeyFramesProperty, collection);
                }

                return collection;
            }

            set
            {
                SetValue(ScalarKeyFramesProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the target property to be animated
        /// </summary>
        public string Target
        {
            get { return (string)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the property that should start the implicit animation
        /// </summary>
        public string ImplicitTarget
        {
            get { return (string)GetValue(ImplicitTargetProperty); }
            set { SetValue(ImplicitTargetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the delay of the animation
        /// </summary>
        public TimeSpan Delay
        {
            get { return (TimeSpan)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        /// <summary>
        /// Gets a <see cref="CompositionAnimation"/> that can be used on the Composition layer
        /// </summary>
        /// <param name="compositor">The <see cref="Compositor"/> to use to create the animation</param>
        /// <returns><see cref="CompositionAnimation"/></returns>
        public abstract CompositionAnimation GetCompositionAnimation(Compositor compositor);

        /// <summary>
        /// Called when any property of the animation changes
        /// </summary>
        /// <param name="d">The animation where a property has changed</param>
        /// <param name="e">The details about the property change</param>
        protected static void OnAnimationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AnimationBase).AnimationChanged?.Invoke(d, null);
        }
    }
}
