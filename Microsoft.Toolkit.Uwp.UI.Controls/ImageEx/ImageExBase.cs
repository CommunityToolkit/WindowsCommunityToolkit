using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Shared Code for ImageEx and RoundImageEx
    /// </summary>
    [TemplateVisualState(Name = LoadingState, GroupName = CommonGroup)]
    [TemplateVisualState(Name = LoadedState, GroupName = CommonGroup)]
    [TemplateVisualState(Name = UnloadedState, GroupName = CommonGroup)]
    [TemplateVisualState(Name = FailedState, GroupName = CommonGroup)]
    [TemplatePart(Name = PartImage, Type = typeof(object))]
    [TemplatePart(Name = PartProgress, Type = typeof(ProgressRing))]
    public abstract partial class ImageExBase : Control
    {
        private const string PartImage = "Image";
        private const string PartProgress = "Progress";
        private const string CommonGroup = "CommonStates";
        private const string LoadingState = "Loading";
        private const string LoadedState = "Loaded";
        private const string UnloadedState = "Unloaded";
        private const string FailedState = "Failed";
        private const string ShowStrokeState = "ShowStroke";
        private const string StrokeUnloaded = "StokeUnloaded";

        protected object _image;
        protected ProgressRing _progress;
        protected bool _isInitialized;
        protected object _lockObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageExBase"/> class.
        /// </summary>
        public ImageExBase()
        {
            _lockObj = new object();
        }

        /// <summary>
        /// Update the visual state of the control when its template is changed.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            if (_image is ImageBrush)
            {
                (_image as ImageBrush).ImageOpened -= OnImageOpened;
                (_image as ImageBrush).ImageFailed -= OnImageFailed;
            }
            else if (_image is Image)
            {
                (_image as Image).ImageOpened -= OnImageOpened;
                (_image as Image).ImageFailed -= OnImageFailed;
            }

            _image = GetTemplateChild(PartImage) as object;
            _progress = GetTemplateChild(PartProgress) as ProgressRing;

            _isInitialized = true;

            SetSource(Source);

            if (_image is ImageBrush)
            {
                (_image as ImageBrush).ImageOpened += OnImageOpened;
                (_image as ImageBrush).ImageFailed += OnImageFailed;
            }
            else if (_image is Image)
            {
                (_image as Image).ImageOpened += OnImageOpened;
                (_image as Image).ImageFailed += OnImageFailed;
            }

            base.OnApplyTemplate();
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var newSquareSize = Math.Min(finalSize.Width, finalSize.Height) / 8.0;

            if (_progress?.Width == newSquareSize)
            {
                _progress.Height = newSquareSize;
            }

            return base.ArrangeOverride(finalSize);
        }

        private void OnImageOpened(object sender, RoutedEventArgs e)
        {
            ImageOpened?.Invoke(this, e);
            ImageExOpened?.Invoke(this, new ImageExOpenedEventArgs());
            VisualStateManager.GoToState(this, LoadedState, true);
        }

        private void OnImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            ImageFailed?.Invoke(this, e);
            ImageExFailed?.Invoke(this, new ImageExFailedEventArgs(new Exception(e.ErrorMessage)));
            VisualStateManager.GoToState(this, FailedState, true);
        }
    }
}