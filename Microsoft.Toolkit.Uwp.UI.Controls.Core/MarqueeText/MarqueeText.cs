// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
    [TemplatePart(Name = MarqueeContainerPartName, Type = typeof(Panel))]
    [TemplatePart(Name = Segment1PartName, Type = typeof(FrameworkTemplate))]
    [TemplatePart(Name = Segment2PartName, Type = typeof(FrameworkTemplate))]
    [TemplatePart(Name = Segment2PartName, Type = typeof(FrameworkTemplate))]
    [TemplatePart(Name = MarqueeTransformPartName, Type = typeof(TranslateTransform))]
    [ContentProperty(Name = nameof(Text))]
    public partial class MarqueeText : Control
    {
        private const string MarqueeContainerPartName = "MarqueeContainer";
        private const string Segment1PartName = "Segment1";
        private const string Segment2PartName = "Segment2";
        private const string MarqueeTransformPartName = "MarqueeTransform";

        private const string MarqueeActiveState = "MarqueeActive";
        private const string MarqueeStoppedState = "MarqueeStopped";

        private Panel _marqueeContainer;
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

            _marqueeContainer = (Panel)GetTemplateChild(MarqueeContainerPartName);
            _segment1 = (FrameworkElement)GetTemplateChild(Segment1PartName);
            _segment2 = (FrameworkElement)GetTemplateChild(Segment2PartName);
            _marqueeTranform = (TranslateTransform)GetTemplateChild(MarqueeTransformPartName);

            _marqueeContainer.SizeChanged += Container_SizeChanged;
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
            if (_marqueeContainer == null)
            {
                return false;
            }

            if (!_isActive)
            {
                VisualStateManager.GoToState(this, MarqueeStoppedState, false);

                return false;
            }

            double start;
            double end;
            double value;
            string property;
            switch (Direction)
            {
                default:
                case MarqueeDirection.Left:
                    start = _marqueeContainer.ActualWidth;
                    end = -_segment1.ActualWidth;
                    value = _marqueeTranform.X;
                    property = "(TranslateTransform.X)";
                    break;
                case MarqueeDirection.Right:
                    start = -_segment1.ActualWidth;
                    end = _marqueeContainer.ActualWidth;
                    value = _marqueeTranform.X;
                    property = "(TranslateTransform.X)";
                    break;
                case MarqueeDirection.Up:
                    start = _marqueeContainer.ActualHeight;
                    end = -_segment1.ActualHeight;
                    value = _marqueeTranform.Y;
                    property = "(TranslateTransform.Y)";
                    break;
                case MarqueeDirection.Down:
                    start = -_segment1.ActualHeight;
                    end = _marqueeContainer.ActualHeight;
                    value = _marqueeTranform.Y;
                    property = "(TranslateTransform.Y)";
                    break;
            }

            if (IsLooping)
            {
                start = 0;
            }

            double distance = Math.Abs(start - end);

            if (distance == 0)
            {
                return false;
            }

            if (IsLooping && Math.Abs(end) < Math.Abs(start))
            {
                StopMarque(resume);
                _segment2.Visibility = Visibility.Collapsed;
                return false;
            }

            _segment2.Visibility = IsLooping ? Visibility.Visible : Visibility.Collapsed;

            TimeSpan duration = TimeSpan.FromSeconds(distance / Speed);

            if (_marqueeStoryboad != null)
            {
                _marqueeStoryboad.Completed -= StoryBoard_Completed;
            }

            _marqueeStoryboad = new Storyboard()
            {
                Duration = duration,
                RepeatBehavior = RepeatBehavior,
            };

            _marqueeStoryboad.Completed += StoryBoard_Completed;

            var animation = new DoubleAnimationUsingKeyFrames
            {
                Duration = duration,
                RepeatBehavior = RepeatBehavior,
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
            Storyboard.SetTargetProperty(animation, property);

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
