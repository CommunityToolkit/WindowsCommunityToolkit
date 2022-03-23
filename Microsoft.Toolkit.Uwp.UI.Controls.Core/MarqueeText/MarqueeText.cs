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
    [TemplateVisualState(GroupName = DirectionVisualStateGroupName, Name = LeftwardsVisualStateName)]
    [TemplateVisualState(GroupName = DirectionVisualStateGroupName, Name = RightwardsVisualStateName)]
    [TemplateVisualState(GroupName = DirectionVisualStateGroupName, Name = UpwardsVisualStateName)]
    [TemplateVisualState(GroupName = DirectionVisualStateGroupName, Name = DownwardsVisualStateName)]
    [ContentProperty(Name = nameof(Text))]
    public partial class MarqueeText : Control
    {
        private const string MarqueeContainerPartName = "MarqueeContainer";
        private const string Segment1PartName = "Segment1";
        private const string Segment2PartName = "Segment2";
        private const string MarqueeTransformPartName = "MarqueeTransform";

        private const string MarqueeActiveState = "MarqueeActive";
        private const string MarqueeStoppedState = "MarqueeStopped";

        private const string DirectionVisualStateGroupName = "DirectionStateGroup";
        private const string LeftwardsVisualStateName = "Leftwards";
        private const string RightwardsVisualStateName = "Rightwards";
        private const string UpwardsVisualStateName = "Upwards";
        private const string DownwardsVisualStateName = "Downwards";

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

        private static string GetVisualStateName(MarqueeDirection direction)
        {
            return direction switch
            {
                MarqueeDirection.Left => LeftwardsVisualStateName,
                MarqueeDirection.Right => RightwardsVisualStateName,
                MarqueeDirection.Up => UpwardsVisualStateName,
                MarqueeDirection.Down => DownwardsVisualStateName,
                _ => LeftwardsVisualStateName,
            };
        }

        private static bool IsInverseDirection(MarqueeDirection direction)
        {
            return direction == MarqueeDirection.Right || direction == MarqueeDirection.Up;
        }

        private static bool IsDirectionHorizontal(MarqueeDirection direction)
        {
            return direction == MarqueeDirection.Left || direction == MarqueeDirection.Right;
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
            StopMarquee(_isActive);
        }

        private void StopMarquee(bool stopping)
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

            // Get the size (width horizontal, height if vertical) of the
            // contain and segment.
            // Also track the property to adjust based on the orientation.
            double containerSize;
            double segmentSize;
            double value;
            string property;

            if (IsDirectionHorizontal(Direction))
            {
                containerSize = _marqueeContainer.ActualWidth;
                segmentSize = _segment1.ActualWidth;
                value = _marqueeTranform.X;
                property = "(TranslateTransform.X)";
            }
            else
            {
                containerSize = _marqueeContainer.ActualHeight;
                segmentSize = _segment1.ActualHeight;
                value = _marqueeTranform.Y;
                property = "(TranslateTransform.Y)";
            }

            if (IsLooping && segmentSize < containerSize)
            {
                StopMarquee(resume);
                _segment2.Visibility = Visibility.Collapsed;
                return false;
            }

            double start = IsLooping ? 0 : containerSize;
            double end = -segmentSize;
            double distance = start - end;

            if (distance == 0)
            {
                return false;
            }

            // Swap the start and end to inverse direction for right or upwards
            if (IsInverseDirection(Direction))
            {
                double swap = start;
                start = end;
                end = swap;
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
                double progress = Math.Abs(start - value) / distance;
                _marqueeStoryboad.Seek(duration * progress);
            }

            return true;
        }
    }
}
