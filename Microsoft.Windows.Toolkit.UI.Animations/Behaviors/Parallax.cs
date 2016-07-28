using Microsoft.Windows.Toolkit.UI.Animations.Extensions;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace Microsoft.Windows.Toolkit.UI.Animations.Behaviors
{
    /// <summary>
    /// Behavior to give a parallax effect when scrolling
    /// </summary>
    /// <seealso>
    ///     <cref>Microsoft.Xaml.Interactivity.Behavior{Windows.UI.Xaml.UIElement}</cref>
    /// </seealso>
    public class Parallax : Behavior<UIElement>
    {
        /// <summary>
        /// The scroller property
        /// </summary>
        public static readonly DependencyProperty ScrollerProperty = DependencyProperty.Register(
            nameof(Scroller), typeof(FrameworkElement), typeof(Parallax), new PropertyMetadata(default(FrameworkElement), PropertyChanged));

        /// <summary>
        /// Gets or sets the scroller.
        /// </summary>
        /// <value>
        /// The scroller.
        /// </value>
        public FrameworkElement Scroller
        {
            get { return (FrameworkElement)GetValue(ScrollerProperty); }
            set { SetValue(ScrollerProperty, value); }
        }

        /// <summary>
        /// The multiplier property
        /// </summary>
        public static readonly DependencyProperty MultiplierProperty = DependencyProperty.Register(
            nameof(Multiplier), typeof(float), typeof(Parallax), new PropertyMetadata(0.3f, PropertyChanged));

        /// <summary>
        /// Gets or sets the multiplier.
        /// </summary>
        /// <value>
        /// The multiplier.
        /// </value>
        public float Multiplier
        {
            get { return (float)GetValue(MultiplierProperty); }
            set { SetValue(MultiplierProperty, value); }
        }

        /// <summary>
        /// The is horizontal effect property
        /// </summary>
        public static readonly DependencyProperty IsHorizontalEffectProperty = DependencyProperty.Register(
            nameof(IsHorizontalEffect), typeof(bool), typeof(Parallax), new PropertyMetadata(default(bool), PropertyChanged));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is horizontal effect.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is horizontal effect; otherwise, <c>false</c>.
        /// </value>
        public bool IsHorizontalEffect
        {
            get { return (bool)GetValue(IsHorizontalEffectProperty); }
            set { SetValue(IsHorizontalEffectProperty, value); }
        }

        private static void PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as Parallax)?.SetBehavior();
        }

        private void SetBehavior()
        {
            AssociatedObject.Parallax(Scroller, IsHorizontalEffect, Multiplier);
        }
    }
}
