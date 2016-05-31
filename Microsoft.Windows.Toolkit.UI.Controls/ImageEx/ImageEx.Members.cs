using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.Media.Casting;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    partial class ImageEx
    {
        /// <summary>
        /// Identifies the <see cref="NineGrid"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NineGridProperty = DependencyProperty.Register("NineGrid", typeof(Thickness), typeof(ImageEx), new PropertyMetadata(new Thickness()));

        /// <summary>
        /// Identifies the <see cref="Stretch"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ImageEx), new PropertyMetadata(Stretch.Uniform));

        /// <summary>
        /// Event raised if the image failed loading.
        /// </summary>
        public event ExceptionRoutedEventHandler ImageFailed;
        /// <summary>
        /// Event raised when the image is successfully loaded and opened.
        /// </summary>
        public event RoutedEventHandler ImageOpened;

        /// <summary>
        /// Get or set the stretch of the image.
        /// </summary>
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        /// <summary>
        /// Get or set the nine-grid used by the image.
        /// </summary>
        public Thickness NineGrid
        {
            get { return (Thickness)GetValue(NineGridProperty); }
            set { SetValue(NineGridProperty, value); }
        }

        /// <summary>
        /// Returns the image as a <see cref="CastingSource"/>.
        /// </summary>
        /// <returns>The image as a <see cref="CastingSource"/>.</returns>
        public CastingSource GetAsCastingSource()
        {
            return _isInitialized ? _image.GetAsCastingSource() : null;
        }

        /// <summary>
        /// Enable or disable .
        /// </summary>
        public bool IsCacheEnabled
        {
            get; set;
        }
    }
}
