using ImageCropper.UWP.Extensions;
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


namespace ImageCropper.UWP
{
    /// <summary>
    /// The <see cref="ImageCropper"/> control allows user to crop image freely.
    /// </summary>
    [TemplatePart(Name = LayoutGridName, Type = typeof(Grid))]
    [TemplatePart(Name = ImageCanvasPartName, Type = typeof(Canvas))]
    [TemplatePart(Name = SourceImagePartName, Type = typeof(Image))]
    [TemplatePart(Name = MaskAreaPathPartName, Type = typeof(Path))]
    [TemplatePart(Name = TopButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = BottomButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = LeftButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = RightButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = UpperLeftButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = UpperRightButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = LowerLeftButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = LowerRightButtonPartName, Type = typeof(Button))]
    public partial class ImageCropper : Control
    {
        private Grid _layoutGrid;
        private Canvas _imageCanvas;
        private Image _sourceImage;
        private Path _maskAreaPath;
        private Button _topButton;
        private Button _bottomButton;
        private Button _leftButton;
        private Button _rigthButton;
        private Button _upperLeftButton;
        private Button _upperRightButton;
        private Button _lowerLeftButton;
        private Button _lowerRigthButton;
        private double _startX;
        private double _startY;
        private double _endX;
        private double _endY;
        private readonly CompositeTransform _imageTransform = new CompositeTransform();
        private readonly GeometryGroup _maskAreaGeometryGroup = new GeometryGroup {FillRule = FillRule.EvenOdd};
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

        private Rect CanvasRect => new Rect(0, 0, _imageCanvas.ActualWidth, _imageCanvas.ActualHeight);
        private bool KeepAspectRatio => UsedAspectRatio > 0;
        private double UsedAspectRatio => CircularCrop ? 1 : AspectRatio;

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
                    size.Width = size.Height * aspectRatio;
                else
                    size.Height = size.Width / aspectRatio;
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
                var realMinSelectSize = _imageTransform.TransformBounds(new Rect(new Point(), MinCropSize));
                var minLength = Math.Min(realMinSelectSize.Width, realMinSelectSize.Height);
                if (minLength < MinSelectedLength)
                {
                    var aspectRatio = KeepAspectRatio ? UsedAspectRatio : 1;
                    var minSelectSize = new Size(MinSelectedLength, MinSelectedLength);
                    if (aspectRatio >= 1)
                        minSelectSize.Width = minSelectSize.Height * aspectRatio;
                    else
                        minSelectSize.Height = minSelectSize.Width / aspectRatio;
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
            _topButton = GetTemplateChild(TopButtonPartName) as Button;
            _bottomButton = GetTemplateChild(BottomButtonPartName) as Button;
            _leftButton = GetTemplateChild(LeftButtonPartName) as Button;
            _rigthButton = GetTemplateChild(RightButtonPartName) as Button;
            _upperLeftButton = GetTemplateChild(UpperLeftButtonPartName) as Button;
            _upperRightButton = GetTemplateChild(UpperRightButtonPartName) as Button;
            _lowerLeftButton = GetTemplateChild(LowerLeftButtonPartName) as Button;
            _lowerRigthButton = GetTemplateChild(LowerRightButtonPartName) as Button;
            HookUpEvents();
            UpdateControlButtonVisibility();
        }

        private void HookUpEvents()
        {
            if (_imageCanvas != null)
                _imageCanvas.SizeChanged += ImageCanvas_SizeChanged;
            if (_sourceImage != null)
            {
                _sourceImage.RenderTransform = _imageTransform;
                _sourceImage.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                _sourceImage.ManipulationDelta += SourceImage_ManipulationDelta;
            }

            if (_maskAreaPath != null) _maskAreaPath.Data = _maskAreaGeometryGroup;

            if (_topButton != null)
            {
                _topButton.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                _topButton.Tag = DragPosition.Top;
                _topButton.ManipulationDelta += ControlButton_ManipulationDelta;
                _topButton.ManipulationCompleted += ControlButton_ManipulationCompleted;
                _topButton.KeyDown += ControlButton_KeyDown;
                _topButton.KeyUp += ControlButton_KeyUp;
            }

            if (_bottomButton != null)
            {
                _bottomButton.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                _bottomButton.Tag = DragPosition.Bottom;
                _bottomButton.ManipulationDelta += ControlButton_ManipulationDelta;
                _bottomButton.ManipulationCompleted += ControlButton_ManipulationCompleted;
                _bottomButton.KeyDown += ControlButton_KeyDown;
                _bottomButton.KeyUp += ControlButton_KeyUp;
            }

            if (_leftButton != null)
            {
                _leftButton.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                _leftButton.Tag = DragPosition.Left;
                _leftButton.ManipulationDelta += ControlButton_ManipulationDelta;
                _leftButton.ManipulationCompleted += ControlButton_ManipulationCompleted;
                _leftButton.KeyDown += ControlButton_KeyDown;
                _leftButton.KeyUp += ControlButton_KeyUp;
            }

            if (_rigthButton != null)
            {
                _rigthButton.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                _rigthButton.Tag = DragPosition.Right;
                _rigthButton.ManipulationDelta += ControlButton_ManipulationDelta;
                _rigthButton.ManipulationCompleted += ControlButton_ManipulationCompleted;
                _rigthButton.KeyDown += ControlButton_KeyDown;
                _rigthButton.KeyUp += ControlButton_KeyUp;
            }

            if (_upperLeftButton != null)
            {
                _upperLeftButton.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                _upperLeftButton.Tag = DragPosition.UpperLeft;
                _upperLeftButton.ManipulationDelta += ControlButton_ManipulationDelta;
                _upperLeftButton.ManipulationCompleted += ControlButton_ManipulationCompleted;
                _upperLeftButton.KeyDown += ControlButton_KeyDown;
                _upperLeftButton.KeyUp += ControlButton_KeyUp;
            }

            if (_upperRightButton != null)
            {
                _upperRightButton.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                _upperRightButton.Tag = DragPosition.UpperRight;
                _upperRightButton.ManipulationDelta += ControlButton_ManipulationDelta;
                _upperRightButton.ManipulationCompleted += ControlButton_ManipulationCompleted;
                _upperRightButton.KeyDown += ControlButton_KeyDown;
                _upperRightButton.KeyUp += ControlButton_KeyUp;
            }

            if (_lowerLeftButton != null)
            {
                _lowerLeftButton.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                _lowerLeftButton.Tag = DragPosition.LowerLeft;
                _lowerLeftButton.ManipulationDelta += ControlButton_ManipulationDelta;
                _lowerLeftButton.ManipulationCompleted += ControlButton_ManipulationCompleted;
                _lowerLeftButton.KeyDown += ControlButton_KeyDown;
                _lowerLeftButton.KeyUp += ControlButton_KeyUp;
            }

            if (_lowerRigthButton != null)
            {
                _lowerRigthButton.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                _lowerRigthButton.Tag = DragPosition.LowerRight;
                _lowerRigthButton.ManipulationDelta += ControlButton_ManipulationDelta;
                _lowerRigthButton.ManipulationCompleted += ControlButton_ManipulationCompleted;
                _lowerRigthButton.KeyDown += ControlButton_KeyDown;
                _lowerRigthButton.KeyUp += ControlButton_KeyUp;
            }
        }

        private void UnhookEvents()
        {
            if (_imageCanvas != null)
                _imageCanvas.SizeChanged -= ImageCanvas_SizeChanged;
            if (_sourceImage != null)
            {
                _sourceImage.RenderTransform = null;
                _sourceImage.ManipulationDelta -= SourceImage_ManipulationDelta;
            }

            if (_maskAreaPath != null) _maskAreaPath.Data = null;

            if (_topButton != null)
            {
                _topButton.ManipulationDelta -= ControlButton_ManipulationDelta;
                _topButton.ManipulationCompleted -= ControlButton_ManipulationCompleted;
                _topButton.KeyDown -= ControlButton_KeyDown;
                _topButton.KeyUp -= ControlButton_KeyUp;
            }

            if (_bottomButton != null)
            {
                _bottomButton.ManipulationDelta -= ControlButton_ManipulationDelta;
                _bottomButton.ManipulationCompleted -= ControlButton_ManipulationCompleted;
                _bottomButton.KeyDown -= ControlButton_KeyDown;
                _bottomButton.KeyUp -= ControlButton_KeyUp;
            }

            if (_leftButton != null)
            {
                _leftButton.ManipulationDelta -= ControlButton_ManipulationDelta;
                _leftButton.ManipulationCompleted += ControlButton_ManipulationCompleted;
                _leftButton.KeyDown -= ControlButton_KeyDown;
                _leftButton.KeyUp -= ControlButton_KeyUp;
            }

            if (_rigthButton != null)
            {
                _rigthButton.ManipulationDelta -= ControlButton_ManipulationDelta;
                _rigthButton.ManipulationCompleted -= ControlButton_ManipulationCompleted;
                _rigthButton.KeyDown -= ControlButton_KeyDown;
                _rigthButton.KeyUp -= ControlButton_KeyUp;
            }

            if (_upperLeftButton != null)
            {
                _upperLeftButton.ManipulationDelta -= ControlButton_ManipulationDelta;
                _upperLeftButton.ManipulationCompleted -= ControlButton_ManipulationCompleted;
                _upperLeftButton.KeyDown -= ControlButton_KeyDown;
                _upperLeftButton.KeyUp -= ControlButton_KeyUp;
            }

            if (_upperRightButton != null)
            {
                _upperRightButton.ManipulationDelta -= ControlButton_ManipulationDelta;
                _upperRightButton.ManipulationCompleted -= ControlButton_ManipulationCompleted;
                _upperRightButton.KeyDown -= ControlButton_KeyDown;
                _upperRightButton.KeyUp -= ControlButton_KeyUp;
            }

            if (_lowerLeftButton != null)
            {
                _lowerLeftButton.ManipulationDelta -= ControlButton_ManipulationDelta;
                _lowerLeftButton.ManipulationCompleted -= ControlButton_ManipulationCompleted;
                _lowerLeftButton.KeyDown -= ControlButton_KeyDown;
                _lowerLeftButton.KeyUp -= ControlButton_KeyUp;
            }

            if (_lowerRigthButton != null)
            {
                _lowerRigthButton.ManipulationDelta -= ControlButton_ManipulationDelta;
                _lowerRigthButton.ManipulationCompleted -= ControlButton_ManipulationCompleted;
                _lowerRigthButton.KeyDown -= ControlButton_KeyDown;
                _lowerRigthButton.KeyUp -= ControlButton_KeyUp;
            }
        }

        /// <summary>
        /// Load an image from a file.
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns></returns>
        public async Task LoadImageFromFile(StorageFile imageFile)
        {
            var writeableBitmap = new WriteableBitmap(1, 1);
            using (var stream = await imageFile.OpenReadAsync())
            {
                await writeableBitmap.SetSourceAsync(stream);
            }

            SourceImage = writeableBitmap;
        }

        /// <summary>
        /// Gets the cropped image.
        /// </summary>
        /// <returns>WriteableBitmap</returns>
        public async Task<WriteableBitmap> GetCroppedBitmapAsync()
        {
            if (SourceImage == null)
                return null;
            return await SourceImage.GetCroppedBitmapAsync(_currentCroppedRect);
        }

        /// <summary>
        /// Save the cropped image to a file.
        /// </summary>
        /// <param name="imageFile">The target file.</param>
        /// <param name="encoderId">The encoderId of BitmapEncoder</param>
        /// <returns></returns>
        public async Task SaveCroppedBitmapAsync(StorageFile imageFile, Guid encoderId)
        {
            if (SourceImage == null)
                return;
            var croppedBitmap = await SourceImage.GetCroppedBitmapAsync(_currentCroppedRect);
            await croppedBitmap.RenderToFile(imageFile, encoderId);
        }
    }
}