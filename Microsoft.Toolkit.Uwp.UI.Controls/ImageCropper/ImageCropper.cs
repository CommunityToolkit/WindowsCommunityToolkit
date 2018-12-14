// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="ImageCropper"/> control allows user to crop image freely.
    /// </summary>
    [TemplatePart(Name = LayoutGridName, Type = typeof(Grid))]
    [TemplatePart(Name = ImageCanvasPartName, Type = typeof(Canvas))]
    [TemplatePart(Name = SourceImagePartName, Type = typeof(Image))]
    [TemplatePart(Name = MaskAreaPathPartName, Type = typeof(Path))]
    [TemplatePart(Name = TopThumbPartName, Type = typeof(ImageCropperThumb))]
    [TemplatePart(Name = BottomThumbPartName, Type = typeof(ImageCropperThumb))]
    [TemplatePart(Name = LeftThumbPartName, Type = typeof(ImageCropperThumb))]
    [TemplatePart(Name = RightThumbPartName, Type = typeof(ImageCropperThumb))]
    [TemplatePart(Name = UpperLeftThumbPartName, Type = typeof(ImageCropperThumb))]
    [TemplatePart(Name = UpperRightThumbPartName, Type = typeof(ImageCropperThumb))]
    [TemplatePart(Name = LowerLeftThumbPartName, Type = typeof(ImageCropperThumb))]
    [TemplatePart(Name = LowerRightThumbPartName, Type = typeof(ImageCropperThumb))]
    public partial class ImageCropper : Control
    {
        private readonly CompositeTransform _imageTransform = new CompositeTransform();
        private readonly GeometryGroup _maskAreaGeometryGroup = new GeometryGroup { FillRule = FillRule.EvenOdd };

        private Grid _layoutGrid;
        private Canvas _imageCanvas;
        private Image _sourceImage;
        private Path _maskAreaPath;
        private ImageCropperThumb _topThumb;
        private ImageCropperThumb _bottomThumb;
        private ImageCropperThumb _leftThumb;
        private ImageCropperThumb _rightThumb;
        private ImageCropperThumb _upperLeftThumb;
        private ImageCropperThumb _upperRightThumb;
        private ImageCropperThumb _lowerLeftThumb;
        private ImageCropperThumb _lowerRigthThumb;
        private double _startX;
        private double _startY;
        private double _endX;
        private double _endY;
        private Rect _currentCroppedRect = Rect.Empty;
        private Rect _restrictedCropRect = Rect.Empty;
        private Rect _restrictedSelectRect = Rect.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageCropper"/> class.
        /// </summary>
        public ImageCropper()
        {
            DefaultStyleKey = typeof(ImageCropper);
        }

        private Rect CanvasRect => new Rect(0, 0, _imageCanvas?.ActualWidth ?? 0, _imageCanvas?.ActualHeight ?? 0);

        private bool KeepAspectRatio => UsedAspectRatio > 0;

        private double UsedAspectRatio
        {
            get
            {
                var aspectRatio = AspectRatio;
                switch (CropShape)
                {
                    case CropShape.Rectangular:
                        break;
                    case CropShape.Circular:
                        aspectRatio = 1;
                        break;
                }

                return aspectRatio != null && aspectRatio > 0 ? aspectRatio.Value : -1;
            }
        }

        /// <summary>
        /// Gets the minimum cropped size.
        /// </summary>
        private Size MinCropSize
        {
            get
            {
                var aspectRatio = KeepAspectRatio ? UsedAspectRatio : 1;
                var size = new Size(MinCroppedPixelLength, MinCroppedPixelLength);
                if (aspectRatio >= 1)
                {
                    size.Width = size.Height * aspectRatio;
                }
                else
                {
                    size.Height = size.Width / aspectRatio;
                }

                return size;
            }
        }

        /// <summary>
        /// Gets the minimum selectable size.
        /// </summary>
        private Size MinSelectSize
        {
            get
            {
                var realMinSelectSize = _imageTransform.TransformBounds(new Rect(default(Point), MinCropSize));
                var minLength = Math.Min(realMinSelectSize.Width, realMinSelectSize.Height);
                if (minLength < MinSelectedLength)
                {
                    var aspectRatio = KeepAspectRatio ? UsedAspectRatio : 1;
                    var minSelectSize = new Size(MinSelectedLength, MinSelectedLength);
                    if (aspectRatio >= 1)
                    {
                        minSelectSize.Width = minSelectSize.Height * aspectRatio;
                    }
                    else
                    {
                        minSelectSize.Height = minSelectSize.Width / aspectRatio;
                    }

                    return minSelectSize;
                }

                return new Size(realMinSelectSize.Width, realMinSelectSize.Height);
            }
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            UnhookEvents();
            _layoutGrid = GetTemplateChild(LayoutGridName) as Grid;
            _imageCanvas = GetTemplateChild(ImageCanvasPartName) as Canvas;
            _sourceImage = GetTemplateChild(SourceImagePartName) as Image;
            _maskAreaPath = GetTemplateChild(MaskAreaPathPartName) as Path;
            _topThumb = GetTemplateChild(TopThumbPartName) as ImageCropperThumb;
            _bottomThumb = GetTemplateChild(BottomThumbPartName) as ImageCropperThumb;
            _leftThumb = GetTemplateChild(LeftThumbPartName) as ImageCropperThumb;
            _rightThumb = GetTemplateChild(RightThumbPartName) as ImageCropperThumb;
            _upperLeftThumb = GetTemplateChild(UpperLeftThumbPartName) as ImageCropperThumb;
            _upperRightThumb = GetTemplateChild(UpperRightThumbPartName) as ImageCropperThumb;
            _lowerLeftThumb = GetTemplateChild(LowerLeftThumbPartName) as ImageCropperThumb;
            _lowerRigthThumb = GetTemplateChild(LowerRightThumbPartName) as ImageCropperThumb;
            HookUpEvents();
            UpdateThumbsVisibility();
        }

        private void HookUpEvents()
        {
            if (_imageCanvas != null)
            {
                _imageCanvas.SizeChanged += ImageCanvas_SizeChanged;
            }

            if (_sourceImage != null)
            {
                _sourceImage.RenderTransform = _imageTransform;
                _sourceImage.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                _sourceImage.ManipulationDelta += SourceImage_ManipulationDelta;
            }

            if (_maskAreaPath != null)
            {
                _maskAreaPath.Data = _maskAreaGeometryGroup;
            }

            if (_topThumb != null)
            {
                _topThumb.Position = ThumbPosition.Top;
                _topThumb.ManipulationDelta += ImageCropperThumb_ManipulationDelta;
                _topThumb.ManipulationCompleted += ImageCropperThumb_ManipulationCompleted;
                _topThumb.KeyDown += ImageCropperThumb_KeyDown;
                _topThumb.KeyUp += ImageCropperThumb_KeyUp;
            }

            if (_bottomThumb != null)
            {
                _bottomThumb.Position = ThumbPosition.Bottom;
                _bottomThumb.ManipulationDelta += ImageCropperThumb_ManipulationDelta;
                _bottomThumb.ManipulationCompleted += ImageCropperThumb_ManipulationCompleted;
                _bottomThumb.KeyDown += ImageCropperThumb_KeyDown;
                _bottomThumb.KeyUp += ImageCropperThumb_KeyUp;
            }

            if (_leftThumb != null)
            {
                _leftThumb.Position = ThumbPosition.Left;
                _leftThumb.ManipulationDelta += ImageCropperThumb_ManipulationDelta;
                _leftThumb.ManipulationCompleted += ImageCropperThumb_ManipulationCompleted;
                _leftThumb.KeyDown += ImageCropperThumb_KeyDown;
                _leftThumb.KeyUp += ImageCropperThumb_KeyUp;
            }

            if (_rightThumb != null)
            {
                _rightThumb.Position = ThumbPosition.Right;
                _rightThumb.ManipulationDelta += ImageCropperThumb_ManipulationDelta;
                _rightThumb.ManipulationCompleted += ImageCropperThumb_ManipulationCompleted;
                _rightThumb.KeyDown += ImageCropperThumb_KeyDown;
                _rightThumb.KeyUp += ImageCropperThumb_KeyUp;
            }

            if (_upperLeftThumb != null)
            {
                _upperLeftThumb.Position = ThumbPosition.UpperLeft;
                _upperLeftThumb.ManipulationDelta += ImageCropperThumb_ManipulationDelta;
                _upperLeftThumb.ManipulationCompleted += ImageCropperThumb_ManipulationCompleted;
                _upperLeftThumb.KeyDown += ImageCropperThumb_KeyDown;
                _upperLeftThumb.KeyUp += ImageCropperThumb_KeyUp;
            }

            if (_upperRightThumb != null)
            {
                _upperRightThumb.Position = ThumbPosition.UpperRight;
                _upperRightThumb.ManipulationDelta += ImageCropperThumb_ManipulationDelta;
                _upperRightThumb.ManipulationCompleted += ImageCropperThumb_ManipulationCompleted;
                _upperRightThumb.KeyDown += ImageCropperThumb_KeyDown;
                _upperRightThumb.KeyUp += ImageCropperThumb_KeyUp;
            }

            if (_lowerLeftThumb != null)
            {
                _lowerLeftThumb.Position = ThumbPosition.LowerLeft;
                _lowerLeftThumb.ManipulationDelta += ImageCropperThumb_ManipulationDelta;
                _lowerLeftThumb.ManipulationCompleted += ImageCropperThumb_ManipulationCompleted;
                _lowerLeftThumb.KeyDown += ImageCropperThumb_KeyDown;
                _lowerLeftThumb.KeyUp += ImageCropperThumb_KeyUp;
            }

            if (_lowerRigthThumb != null)
            {
                _lowerRigthThumb.Position = ThumbPosition.LowerRight;
                _lowerRigthThumb.ManipulationDelta += ImageCropperThumb_ManipulationDelta;
                _lowerRigthThumb.ManipulationCompleted += ImageCropperThumb_ManipulationCompleted;
                _lowerRigthThumb.KeyDown += ImageCropperThumb_KeyDown;
                _lowerRigthThumb.KeyUp += ImageCropperThumb_KeyUp;
            }
        }

        private void UnhookEvents()
        {
            if (_imageCanvas != null)
            {
                _imageCanvas.SizeChanged -= ImageCanvas_SizeChanged;
            }

            if (_sourceImage != null)
            {
                _sourceImage.RenderTransform = null;
                _sourceImage.ManipulationDelta -= SourceImage_ManipulationDelta;
            }

            if (_maskAreaPath != null)
            {
                _maskAreaPath.Data = null;
            }

            if (_topThumb != null)
            {
                _topThumb.ManipulationDelta -= ImageCropperThumb_ManipulationDelta;
                _topThumb.ManipulationCompleted -= ImageCropperThumb_ManipulationCompleted;
                _topThumb.KeyDown -= ImageCropperThumb_KeyDown;
                _topThumb.KeyUp -= ImageCropperThumb_KeyUp;
            }

            if (_bottomThumb != null)
            {
                _bottomThumb.ManipulationDelta -= ImageCropperThumb_ManipulationDelta;
                _bottomThumb.ManipulationCompleted -= ImageCropperThumb_ManipulationCompleted;
                _bottomThumb.KeyDown -= ImageCropperThumb_KeyDown;
                _bottomThumb.KeyUp -= ImageCropperThumb_KeyUp;
            }

            if (_leftThumb != null)
            {
                _leftThumb.ManipulationDelta -= ImageCropperThumb_ManipulationDelta;
                _leftThumb.ManipulationCompleted += ImageCropperThumb_ManipulationCompleted;
                _leftThumb.KeyDown -= ImageCropperThumb_KeyDown;
                _leftThumb.KeyUp -= ImageCropperThumb_KeyUp;
            }

            if (_rightThumb != null)
            {
                _rightThumb.ManipulationDelta -= ImageCropperThumb_ManipulationDelta;
                _rightThumb.ManipulationCompleted -= ImageCropperThumb_ManipulationCompleted;
                _rightThumb.KeyDown -= ImageCropperThumb_KeyDown;
                _rightThumb.KeyUp -= ImageCropperThumb_KeyUp;
            }

            if (_upperLeftThumb != null)
            {
                _upperLeftThumb.ManipulationDelta -= ImageCropperThumb_ManipulationDelta;
                _upperLeftThumb.ManipulationCompleted -= ImageCropperThumb_ManipulationCompleted;
                _upperLeftThumb.KeyDown -= ImageCropperThumb_KeyDown;
                _upperLeftThumb.KeyUp -= ImageCropperThumb_KeyUp;
            }

            if (_upperRightThumb != null)
            {
                _upperRightThumb.ManipulationDelta -= ImageCropperThumb_ManipulationDelta;
                _upperRightThumb.ManipulationCompleted -= ImageCropperThumb_ManipulationCompleted;
                _upperRightThumb.KeyDown -= ImageCropperThumb_KeyDown;
                _upperRightThumb.KeyUp -= ImageCropperThumb_KeyUp;
            }

            if (_lowerLeftThumb != null)
            {
                _lowerLeftThumb.ManipulationDelta -= ImageCropperThumb_ManipulationDelta;
                _lowerLeftThumb.ManipulationCompleted -= ImageCropperThumb_ManipulationCompleted;
                _lowerLeftThumb.KeyDown -= ImageCropperThumb_KeyDown;
                _lowerLeftThumb.KeyUp -= ImageCropperThumb_KeyUp;
            }

            if (_lowerRigthThumb != null)
            {
                _lowerRigthThumb.ManipulationDelta -= ImageCropperThumb_ManipulationDelta;
                _lowerRigthThumb.ManipulationCompleted -= ImageCropperThumb_ManipulationCompleted;
                _lowerRigthThumb.KeyDown -= ImageCropperThumb_KeyDown;
                _lowerRigthThumb.KeyUp -= ImageCropperThumb_KeyUp;
            }
        }

        /// <summary>
        /// Load an image from a file.
        /// </summary>
        /// <param name="imageFile">The image file.</param>
        /// <returns>Task</returns>
        public async Task LoadImageFromFile(StorageFile imageFile)
        {
            var writeableBitmap = new WriteableBitmap(1, 1);
            using (var stream = await imageFile.OpenReadAsync())
            {
                await writeableBitmap.SetSourceAsync(stream);
            }

            Source = writeableBitmap;
        }

        /// <summary>
        /// Gets the cropped image.
        /// </summary>
        /// <returns>The cropped image.</returns>
        public async Task<WriteableBitmap> GetCroppedBitmapAsync()
        {
            return await GetCroppedBitmapAsync(Source, _currentCroppedRect);
        }
    }
}