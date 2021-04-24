using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Media.Surface;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media.Brushes
{
    /// <summary>
    /// Creates a Render Surface brush using an image
    /// </summary>
    public class ImageSurfaceBrush : RenderSurfaceBrushBase
    {
        private Uri _uri;

        private static bool IsHttpUri(Uri uri)
        {
            return uri != null && uri.IsAbsoluteUri && (uri.Scheme == "http" || uri.Scheme == "https");
        }

        /// <summary>
        /// Background Dependency Property
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background",
            typeof(Color),
            typeof(ImageSurfaceBrush),
            new PropertyMetadata(Colors.Transparent, OnBackgroundChanged));

        /// <summary>
        /// Gets or sets the color that is rendered in the transparent areas of the Image. The default value is Colors.Transparent.
        /// </summary>
        public Color Background
        {
            get => (Color)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        /// <summary>
        /// Handles changes to the Background property.
        /// </summary>
        /// <param name="d"><see cref="ImageSurfaceBrush" /></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageSurfaceBrush = (ImageSurfaceBrush)d;
            imageSurfaceBrush.OnBackgroundChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Background dependency property.
        /// </summary>
        private void OnBackgroundChanged()
        {
            OnSurfaceBrushUpdated();
        }

        /// <summary>
        /// Source Dependency Property
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source",
            typeof(object),
            typeof(ImageSurfaceBrush),
            new PropertyMetadata(null, OnSourceChanged));

        /// <summary>
        /// Gets or sets the the .
        /// </summary>
        public object Source
        {
            get => (object)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        /// <summary>
        /// Handles changes to the Source property.
        /// </summary>
        /// <param name="d"><see cref="ImageSurfaceBrush" /></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageSurfaceBrush)d;
            target.OnSourceChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Source dependency property.
        /// </summary>
        private void OnSourceChanged()
        {
            if (Source == null)
            {
                return;
            }

            var uri = Source as Uri;
            if (uri == null)
            {
                var url = Source as string ?? Source.ToString();
                if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
                {
                    _uri = null;
                    return;
                }
            }

            if (!IsHttpUri(uri) && !uri.IsAbsoluteUri)
            {
                _uri = new Uri("ms-appx:///" + uri.OriginalString.TrimStart('/'));
                return;
            }

            _uri = uri;

            OnSurfaceBrushUpdated();
        }

        /// <inheritdoc/>
        protected async override void OnSurfaceBrushUpdated()
        {
            base.OnSurfaceBrushUpdated();

            CompositionBrush?.Dispose();

            if (Generator == null)
            {
                GetGeneratorInstance();
            }

            if (_uri != null && Generator != null)
            {
                RenderSurface = await Generator.CreateImageSurfaceAsync(_uri, new Size(SurfaceWidth, SurfaceHeight), ImageSurfaceOptions.Default);
                CompositionBrush = Window.Current.Compositor.CreateSurfaceBrush(RenderSurface.Surface);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSurfaceBrush"/> class.
        /// </summary>
        public ImageSurfaceBrush()
        {

        }
    }
}
