using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Toolkit.Uwp.UI.Media.Surface;
using Windows.UI;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media.Brushes
{
    public class SolidColorCanvasBrush : RenderCanvasBrushBase
    {
        /// <summary>
        /// Color Dependency Property
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color",
            typeof(Color),
            typeof(SolidColorCanvasBrush),
            new PropertyMetadata(Colors.Transparent, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the color of the brush.
        /// </summary>
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        /// <summary>
        /// Method that is called whenever the dependency properties of the Brush changes
        /// </summary>
        /// <param name="d">The object whose property has changed</param>
        /// <param name="e">Event arguments</param>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var brush = (SolidColorCanvasBrush)d;

            // Recreate the canvas brush on any property change.
            brush.OnUpdated();
        }

        protected override void OnUpdated()
        {
            CanvasBrush = new CanvasSolidColorBrush(CompositionGenerator.Instance.Device, Color);
            base.OnUpdated();
        }

        public SolidColorCanvasBrush()
        {
        }
    }
}
