// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A Control that displays Text in a Marquee style.
    /// </summary>
    public partial class MarqueeText
    {
        private static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(MarqueeText), new PropertyMetadata(null, PropertyChanged));

        private static readonly DependencyProperty SpeedProperty =
            DependencyProperty.Register(nameof(Speed), typeof(double), typeof(MarqueeText), new PropertyMetadata(32d, PropertyChanged));

        private static readonly DependencyProperty RepeatBehaviorProperty =
            DependencyProperty.Register(nameof(RepeatBehavior), typeof(RepeatBehavior), typeof(MarqueeText), new PropertyMetadata(new RepeatBehavior(1), PropertyChanged));

        private static readonly DependencyProperty IsLoopingProperty =
            DependencyProperty.Register(nameof(IsLooping), typeof(bool), typeof(MarqueeText), new PropertyMetadata(false, PropertyChanged));

        private static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(nameof(Direction), typeof(MarqueeDirection), typeof(MarqueeText), new PropertyMetadata(MarqueeDirection.Left, PropertyChanged));

        private static readonly DependencyProperty TextDecorationsProperty =
            DependencyProperty.Register(nameof(TextDecorations), typeof(TextDecorations), typeof(MarqueeText), new PropertyMetadata(TextDecorations.None));

        /// <summary>
        /// Gets or sets the text being displayed in Marquee.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the speed the text moves in the Marquee.
        /// </summary>
        public double Speed
        {
            get { return (double)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the marquee scroll repeats.
        /// </summary>
        public RepeatBehavior RepeatBehavior
        {
            get { return (RepeatBehavior)GetValue(RepeatBehaviorProperty); }
            set { SetValue(RepeatBehaviorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the marquee text wraps.
        /// </summary>
        /// <remarks>
        /// Wrappping text won't scroll if the text can already fit in the screen.
        /// </remarks>
        public bool IsLooping
        {
            get { return (bool)GetValue(IsLoopingProperty); }
            set { SetValue(IsLoopingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the marquee text wraps.
        /// </summary>
        /// <remarks>
        /// Wrappping text won't scroll if the text can already fit in the screen.
        /// </remarks>
        public MarqueeDirection Direction
        {
            get { return (MarqueeDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates what decorations are applied to the text.
        /// </summary>
        public TextDecorations TextDecorations
        {
            get { return (TextDecorations)GetValue(TextDecorationsProperty); }
            set { SetValue(TextDecorationsProperty, value); }
        }

        private static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MarqueeText;

            if (e == null)
            {
                return;
            }

            // Can't resume through IsWrapping change
            if (e.Property == IsLoopingProperty)
            {
                bool active = control._isActive;
                control.StopMarque(false);
                if (active)
                {
                    control.StartMarquee();
                }
            } else
            {
                control.UpdateAnimation(true);
            }
        }
    }
}
