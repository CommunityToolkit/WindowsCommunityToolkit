using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="Eyedropper"/> control can pick up a color from anywhere in your application.
    /// </summary>
    public partial class Eyedropper
    {
        /// <summary>
        /// Identifies the <see cref="Color"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(nameof(Color), typeof(Color), typeof(Eyedropper), new PropertyMetadata(default(Color), OnColorChanged));

        /// <summary>
        /// Identifies the <see cref="Preview"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PreviewProperty =
            DependencyProperty.Register(nameof(Preview), typeof(ImageSource), typeof(Eyedropper), new PropertyMetadata(default(ImageSource)));

        /// <summary>
        /// Identifies the <see cref="WorkArea"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty WorkAreaProperty =
            DependencyProperty.Register(nameof(WorkArea), typeof(Rect), typeof(Eyedropper), new PropertyMetadata(default(Rect), OnWorkAreaChanged));

        /// <summary>
        /// Gets the current color value.
        /// </summary>
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            private set => SetValue(ColorProperty, value);
        }

        /// <summary>
        /// Gets the current color value.
        /// </summary>
        public ImageSource Preview
        {
            get => (ImageSource)GetValue(PreviewProperty);
            private set => SetValue(PreviewProperty, value);
        }

        /// <summary>
        /// Gets the working area of the eyedropper.
        /// </summary>
        public Rect WorkArea
        {
            get => (Rect)GetValue(WorkAreaProperty);
            private set => SetValue(WorkAreaProperty, value);
        }

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Eyedropper eyedropper)
            {
                eyedropper.ColorChanged?.Invoke(eyedropper, new ColorChangedEventArgs { OldColor = (Color)e.OldValue, NewColor = (Color)e.NewValue });
            }
        }

        private static void OnWorkAreaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Eyedropper eyedropper)
            {
                eyedropper.UpadateWorkArea();
            }
        }
    }
}
