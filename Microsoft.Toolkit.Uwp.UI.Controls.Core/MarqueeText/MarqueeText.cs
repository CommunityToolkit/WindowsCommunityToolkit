using System;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A Control that displays Text in a Marquee style.
    /// </summary>
    [TemplatePart(Name = CanvasPartName, Type = typeof(Canvas))]
    [TemplatePart(Name = TextBlock1PartName, Type = typeof(TextBlock))]
    [ContentProperty(Name = nameof(Text))]
    public partial class MarqueeText : Control
    {
        private const string CanvasPartName = "Canvas";
        private const string TextBlock1PartName = "TextBlock1";

        private Canvas _canvas;
        private TextBlock _textBlock1;

        private Storyboard _marqueeStoryboad;

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

            _canvas = (Canvas)GetTemplateChild(CanvasPartName);
            _textBlock1 = (TextBlock)GetTemplateChild(TextBlock1PartName);
        }

        private void StartAnimation()
        {
            if (_canvas == null)
            {
                return;
            }

            if (!IsActive)
            {
                return;
            }

            TimeSpan duration = TimeSpan.FromSeconds(_textBlock1.ActualWidth / Speed);
            RepeatBehavior repeatBehavior = IsRepeating ? RepeatBehavior.Forever : new RepeatBehavior(1);
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
                Value = _canvas.ActualWidth,
            };
            var frame2 = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(duration),
                Value = -_textBlock1.ActualWidth,
            };

            animation.KeyFrames.Add(frame1);
            animation.KeyFrames.Add(frame2);
            _marqueeStoryboad.Children.Add(animation);
            Storyboard.SetTarget(animation, _textBlock1.RenderTransform);
            Storyboard.SetTargetProperty(animation, "(TranslateTransform.X)");

            _marqueeStoryboad.Begin();
        }

        private void StoryBoard_Completed(object sender, object e)
        {
            IsActive = false;
        }
    }
}
