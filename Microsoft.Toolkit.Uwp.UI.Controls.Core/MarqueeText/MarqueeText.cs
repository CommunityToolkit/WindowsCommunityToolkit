using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A Control that displays Text in a Marquee style.
    /// </summary>
    [TemplatePart(Name = RootGridPartName, Type = typeof(Grid))]
    [TemplatePart(Name = Segment1PartName, Type = typeof(FrameworkTemplate))]
    [TemplatePart(Name = Segment2PartName, Type = typeof(FrameworkTemplate))]
    [TemplatePart(Name = Segment2PartName, Type = typeof(FrameworkTemplate))]
    [TemplatePart(Name = MarqueeTransformPartName, Type = typeof(TranslateTransform))]
    [ContentProperty(Name = nameof(Text))]
    public partial class MarqueeText : Control
    {
        private const string RootGridPartName = "RootGrid";
        private const string Segment1PartName = "Segment1";
        private const string Segment2PartName = "Segment2";
        private const string MarqueeTransformPartName = "MarqueeTransform";

        private const string MarqueeActiveState = "MarqueeActive";
        private const string MarqueeStoppedState = "MarqueeStopped";

        private Grid _rootGrid;
        private FrameworkElement _segment1;
        private FrameworkElement _segment2;
        private TranslateTransform _marqueeTranform;
        private Storyboard _marqueeStoryboad;

        private bool _isActive;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarqueeText"/> class.
        /// </summary>
        public MarqueeText()
        {
            DefaultStyleKey = typeof(MarqueeText);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _rootGrid = (Grid)GetTemplateChild(RootGridPartName);
            _segment1 = (FrameworkElement)GetTemplateChild(Segment1PartName);
            _segment2 = (FrameworkElement)GetTemplateChild(Segment2PartName);
            _marqueeTranform = (TranslateTransform)GetTemplateChild(MarqueeTransformPartName);

            _rootGrid.SizeChanged += RootGrid_SizeChanged;
            Unloaded += MarqueeText_Unloaded;
        }

        /// <summary>
        /// Begins the Marquee animation if not running.
        /// </summary>
        public void StartMarquee()
        {
            bool initial = _isActive;
            _isActive = true;
            bool playing = UpdateAnimation(initial);

            // Invoke MarqueeBegan if Marquee is now playing and was not before
            if (playing && !initial)
            {
                MarqueeBegan?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Stops the Marquee animation.
        /// </summary>
        public void StopMarquee()
        {
            StopMarque(_isActive);
        }

        private void StopMarque(bool stopping)
        {
            _isActive = false;
            bool playing = UpdateAnimation(false);

            // Invoke MarqueeStopped if Marquee is not playing and was before
            if (!playing && stopping)
            {
                MarqueeStopped?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <returns>True if the Animation is now playing</returns>
        private bool UpdateAnimation(bool resume = true)
        {
            if (_rootGrid == null)
            {
                return false;
            }

            if (!_isActive)
            {
                VisualStateManager.GoToState(this, MarqueeStoppedState, false);

                return false;
            }

            double start = IsWrapping ? 0 : _rootGrid.ActualWidth;
            double end = -_segment1.ActualWidth;
            double distance = start - end;

            if (distance == 0)
            {
                return false;
            }

            if (IsWrapping && _segment1.ActualWidth < _rootGrid.ActualWidth)
            {
                StopMarque(resume);
                _segment2.Visibility = Visibility.Collapsed;
                return false;
            }

            _segment2.Visibility = IsWrapping ? Visibility.Visible : Visibility.Collapsed;

            TimeSpan duration = TimeSpan.FromSeconds(distance / Speed);
            RepeatBehavior repeatBehavior = IsRepeating ? RepeatBehavior.Forever : new RepeatBehavior(1);

            if (_marqueeStoryboad != null)
            {
                _marqueeStoryboad.Completed -= StoryBoard_Completed;
            }

            _marqueeStoryboad = new Storyboard()
            {
                Duration = duration,
                RepeatBehavior = repeatBehavior,
            };

            _marqueeStoryboad.Completed += StoryBoard_Completed;

            var animation = new DoubleAnimationUsingKeyFrames
            {
                Duration = duration,
                RepeatBehavior = repeatBehavior,
            };
            var frame1 = new DiscreteDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                Value = start,
            };
            var frame2 = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(duration),
                Value = end,
            };

            animation.KeyFrames.Add(frame1);
            animation.KeyFrames.Add(frame2);
            _marqueeStoryboad.Children.Add(animation);
            Storyboard.SetTarget(animation, _marqueeTranform);
            Storyboard.SetTargetProperty(animation, "(TranslateTransform.X)");

            VisualStateManager.GoToState(this, MarqueeActiveState, true);
            _marqueeStoryboad.Begin();

            if (resume)
            {
                double progress = (start - _marqueeTranform.X) / distance;
                _marqueeStoryboad.Seek(duration * progress);
            }

            return true;
        }
    }
}
