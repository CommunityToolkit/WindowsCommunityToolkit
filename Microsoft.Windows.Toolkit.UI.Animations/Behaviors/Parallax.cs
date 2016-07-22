using Microsoft.Xaml.Interactivity;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

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
            ((Parallax)sender)?.SetBehavior();
        }

        private static T GetChildOfType<T>(DependencyObject depObj) 
            where T : DependencyObject
        {
            if (depObj == null)
            {
                return null;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = child as T ?? GetChildOfType<T>(child);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private void SetBehavior()
        {
            if (Scroller == default(FrameworkElement))
            {
                return;
            }

            var scroller = Scroller as ScrollViewer;
            if (scroller == null)
            {
                scroller = GetChildOfType<ScrollViewer>(Scroller);
                if (scroller == null)
                {
                    return;
                }
            }

            CompositionPropertySet scrollerViewerManipulation = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scroller);

            Compositor compositor = scrollerViewerManipulation.Compositor;

            var manipulationProperty = IsHorizontalEffect ? "X" : "Y";
            ExpressionAnimation expression = compositor.CreateExpressionAnimation($"ScrollManipululation.Translation.{manipulationProperty} * ParallaxMultiplier");

            expression.SetScalarParameter("ParallaxMultiplier", Multiplier);
            expression.SetReferenceParameter("ScrollManipululation", scrollerViewerManipulation);

            Visual textVisual = ElementCompositionPreview.GetElementVisual(AssociatedObject);
            textVisual.StartAnimation($"Offset.{manipulationProperty}", expression);
        }
    }
}
