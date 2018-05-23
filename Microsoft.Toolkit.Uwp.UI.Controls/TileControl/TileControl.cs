// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Toolkit.Uwp.UI.Animations.Expressions;
    using Microsoft.Toolkit.Uwp.UI.Extensions;
    using Robmikh.CompositionSurfaceFactory;
    using Windows.ApplicationModel;
    using Windows.Foundation;
    using Windows.Foundation.Metadata;
    using Windows.Storage;
    using Windows.UI.Composition;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Hosting;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;
    using Windows.UI.Xaml.Shapes;

    /// <summary>
    /// Orientation of the scroll
    /// </summary>
    public enum ScrollOrientation
    {
        /// <summary>
        /// Scroll only Horizontaly (and optimize the number of image used)
        /// </summary>
        Horizontal,

        /// <summary>
        /// Scroll only Verticaly (and optimize the number of image used)
        /// </summary>
        Vertical,

        /// <summary>
        /// Scroll both Horizontaly and verticaly
        /// </summary>
        Both
    }

    /// <summary>
    /// Image alignment
    /// </summary>
    public enum ImageAlignment
    {
        /// <summary>
        /// No alignment needed
        /// </summary>
        None,

        /// <summary>
        /// Align to Left when the property ScrollOrientation is Horizontal
        /// </summary>
        Left,

        /// <summary>
        /// Align to Right when the property ScrollOrientation is Horizontal
        /// </summary>
        Right,

        /// <summary>
        /// Align to Top when the property ScrollOrientation is Vertical
        /// </summary>
        Top,

        /// <summary>
        /// Align to Bottom when the property ScrollOrientation is Vertical
        /// </summary>
        Bottom
    }

    /// <summary>
    /// A ContentControl that show an image repeated many times.
    /// The control can be synchronized with a Scrollviewer and animated easily.
    /// </summary>
    public sealed class TileControl : ContentControl
    {
        /// <summary>
        /// Identifies the <see cref="ScrollViewerContainer"/> property.
        /// </summary>
        public static readonly DependencyProperty ScrollViewerContainerProperty =
            DependencyProperty.Register(nameof(ScrollViewerContainer), typeof(FrameworkElement), typeof(TileControl), new PropertyMetadata(null, OnScrollViewerContainerChange));

        /// <summary>
        /// Identifies the <see cref="ImageAlignment"/> property.
        /// </summary>
        public static readonly DependencyProperty ImageAlignmentProperty =
            DependencyProperty.Register(nameof(ImageAlignment), typeof(ImageAlignment), typeof(TileControl), new PropertyMetadata(ImageAlignment.None, OnAlignmentChange));

        /// <summary>
        /// Identifies the <see cref="ImageSource"/> property.
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(Uri), typeof(TileControl), new PropertyMetadata(null, OnImageSourceChanged));

        /// <summary>
        /// Identifies the <see cref="ScrollOrientation"/> property.
        /// </summary>
        public static readonly DependencyProperty ScrollOrientationProperty =
            DependencyProperty.Register(nameof(ScrollOrientation), typeof(ScrollOrientation), typeof(TileControl), new PropertyMetadata(ScrollOrientation.Both, OnOrientationChanged));

        /// <summary>
        /// Identifies the <see cref="OffsetX"/> property.
        /// </summary>
        public static readonly DependencyProperty OffsetXProperty =
            DependencyProperty.Register(nameof(OffsetX), typeof(double), typeof(TileControl), new PropertyMetadata(0.0, OnOffsetChange));

        /// <summary>
        /// Identifies the <see cref="OffsetY"/> property.
        /// </summary>
        public static readonly DependencyProperty OffsetYProperty =
            DependencyProperty.Register(nameof(OffsetY), typeof(double), typeof(TileControl), new PropertyMetadata(0.0, OnOffsetChange));

        /// <summary>
        /// Identifies the <see cref="ParallaxSpeedRatio"/> property.
        /// </summary>
        public static readonly DependencyProperty ParallaxSpeedRatioProperty =
            DependencyProperty.Register(nameof(ParallaxSpeedRatio), typeof(double), typeof(TileControl), new PropertyMetadata(1.0, OnScrollSpeedRatioChange));

        /// <summary>
        /// Identifies the <see cref="IsAnimated"/> property.
        /// </summary>
        public static readonly DependencyProperty IsAnimatedProperty =
            DependencyProperty.Register(nameof(IsAnimated), typeof(bool), typeof(TileControl), new PropertyMetadata(false, OnIsAnimatedChange));

        /// <summary>
        /// Identifies the <see cref="AnimationStepX"/> property.
        /// </summary>
        public static readonly DependencyProperty AnimationStepXProperty =
            DependencyProperty.Register(nameof(AnimationStepX), typeof(double), typeof(TileControl), new PropertyMetadata(1.0));

        /// <summary>
        /// Identifies the <see cref="AnimationStepY"/> property.
        /// </summary>
        public static readonly DependencyProperty AnimationStepYProperty =
            DependencyProperty.Register(nameof(AnimationStepY), typeof(double), typeof(TileControl), new PropertyMetadata(1.0));

        /// <summary>
        /// Identifies the <see cref="AnimationDuration"/> property.
        /// </summary>
        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register(nameof(AnimationDuration), typeof(double), typeof(TileControl), new PropertyMetadata(30.0, OnAnimationDuration));

        private FrameworkElement _rootElement = null;
        private Canvas _containerElement = null;
        private TranslateTransform _containerTranslate = null;
        private ImageBrush _brushXaml = null;

        private ContainerVisual _containerVisual = null;
        private CompositionSurfaceBrush _brushVisual = null;

        private Size _imageSize = Size.Empty;

        private UriSurface _uriSurface = null;
        private Visual _rootVisual = null;

        private DispatcherTimer _timerAnimation = null;

        /// <summary>
        /// A Scrollviewer used for synchronized the move of the <see cref="TileControl"/>
        /// </summary>
        private ScrollViewer _scrollviewer = null;

        /// <summary>
        /// a flag to lock shared method
        /// </summary>
        private SemaphoreSlim _flag = new SemaphoreSlim(1);

        private List<SpriteVisual> _compositionChildren = new List<SpriteVisual>(50);
        private List<Rectangle> _xamlChildren = new List<Rectangle>(50);

        private bool _isImageSourceLoaded = false;
        private bool _isRootElementSizeChanged = false;

        private CompositionPropertySet _propertySetModulo = null;
        private object _lockerOffset = new object();

        private double _animationX = 0;
        private double _animationY = 0;

        private enum UIStrategy
        {
            /// <summary>
            /// TileControl is created with XAML
            /// </summary>
            PureXaml,

            /// <summary>
            /// TileControl is created with Microsoft Composition
            /// </summary>
            Composition
        }

        /// <summary>
        /// The image loaded event.
        /// </summary>
        public event EventHandler ImageLoaded = null;

        /// <summary>
        /// Gets a value indicating whether the platform supports Composition.
        /// </summary>
        /// <remarks>
        /// On platforms not supporting Composition, this <See cref="UIStrategy"/> is automaticaly set to PureXaml.
        /// </remarks>
        public static bool IsCompositionSupported => !DesignTimeHelpers.IsRunningInLegacyDesignerMode &&
             ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3); // SDK >= 14393

        /// <summary>
        /// Initializes a new instance of the <see cref="TileControl"/> class.
        /// </summary>
        public TileControl()
        {
            DefaultStyleKey = typeof(TileControl);

            InitializeAnimation();
        }

        /// <summary>
        /// Gets or sets a ScrollViewer or a frameworkElement containing a ScrollViewer.
        /// The tile control is synchronized with the offset of the scrollviewer
        /// </summary>
        public FrameworkElement ScrollViewerContainer
        {
            get { return (FrameworkElement)GetValue(ScrollViewerContainerProperty); }
            set { SetValue(ScrollViewerContainerProperty, value); }
        }

        private static async void OnScrollViewerContainerChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TileControl;
            await control.InitializeScrollViewerContainer(e.OldValue as FrameworkElement, e.NewValue as FrameworkElement);
        }

        /// <summary>
        /// Initialize the new ScrollViewer
        /// </summary>
        /// <param name="oldScrollViewerContainer">the old ScrollViewerContainer</param>
        /// <param name="newScrollViewerContainer">the new ScrollViewerContainer</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private Task InitializeScrollViewerContainer(FrameworkElement oldScrollViewerContainer, FrameworkElement newScrollViewerContainer)
        {
            if (oldScrollViewerContainer != null)
            {
                oldScrollViewerContainer.Loaded -= ScrollViewerContainer_Loaded;
            }

            if (newScrollViewerContainer != null)
            {
                // May be the scrollViewerContainer is not completly loaded (and the scrollviewer doesn't exit yet)
                // so we need to wait the loaded event to be sure
                newScrollViewerContainer.Loaded += ScrollViewerContainer_Loaded;
            }

            // try to attach a scrollviewer (the null value is valid)
            return AttachScrollViewer(newScrollViewerContainer);
        }

        /// <summary>
        /// ScrollViewer is loaded
        /// </summary>
        /// <param name="sender">a ScrollViewerContainer</param>
        /// <param name="e">arguments</param>
        private async void ScrollViewerContainer_Loaded(object sender, RoutedEventArgs e)
        {
            await AttachScrollViewer(sender as FrameworkElement);
        }

        /// <summary>
        /// Gets how the <see cref="TileControl"/> is rendered
        /// The default value is Composition.
        /// </summary>
        private UIStrategy Strategy
        {
            get
            {
                if (currentStrategy == null)
                {
                    if (!IsCompositionSupported)
                    {
                        currentStrategy = UIStrategy.PureXaml;
                    }
                    else
                    {
                        currentStrategy = UIStrategy.Composition;
                    }
                }

                return currentStrategy.Value;
            }
        }

        private UIStrategy? currentStrategy = null;

        /// <summary>
        /// Gets or sets the alignment of the tile when the <see cref="ScrollOrientation"/> is set to Vertical or Horizontal.
        /// Valid values are Left or Right for <see cref="ScrollOrientation"/> set to Horizontal and Top or Bottom for <see cref="ScrollOrientation"/> set to Vertical.
        /// </summary>
        public ImageAlignment ImageAlignment
        {
            get { return (ImageAlignment)GetValue(ImageAlignmentProperty); }
            set { SetValue(ImageAlignmentProperty, value); }
        }

        private static async void OnAlignmentChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TileControl;
            await control.RefreshContainerTileLocked();
        }

        /// <summary>
        /// Attach a scrollviewer to the TileControl (parallax effect)
        /// </summary>
        /// <param name="scrollViewerContainer">A ScrollViewer or a container of a ScrollViewer</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task AttachScrollViewer(DependencyObject scrollViewerContainer)
        {
            if (scrollViewerContainer == null)
            {
                return;
            }

            ScrollViewer newScrollviewer = scrollViewerContainer.FindDescendant<ScrollViewer>();

            if (newScrollviewer != _scrollviewer)
            {
                var strategy = Strategy;

                if (strategy == UIStrategy.Composition)
                {
                    // Update the expression
                    await CreateModuloExpression(newScrollviewer);
                }
                else
                {
                    if (_scrollviewer != null)
                    {
                        _scrollviewer.ViewChanging -= Scrollviewer_ViewChanging;
                    }

                    if (newScrollviewer != null)
                    {
                        newScrollviewer.ViewChanging += Scrollviewer_ViewChanging;
                    }
                }

                _scrollviewer = newScrollviewer;
            }
        }

        private void Scrollviewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            RefreshMove();
        }

        /// <summary>
        /// Gets or sets the uri of the image to load
        /// </summary>
        public Uri ImageSource
        {
            get { return (Uri)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        private static async void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TileControl;
            await control.LoadImageBrush(e.NewValue as Uri);
        }

        /// <summary>
        /// Load the image and transform it to a composition brush or a XAML brush (depends of the UIStrategy)
        /// </summary>
        /// <param name="uri">the uri of the image to load</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<bool> LoadImageBrush(Uri uri)
        {
            if (DesignTimeHelpers.IsRunningInLegacyDesignerMode)
            {
                return false;
            }

            var strategy = Strategy;

            if (strategy == UIStrategy.Composition)
            {
                if (_containerVisual == null || uri == null)
                {
                    return false;
                }
            }
            else
            {
                if (uri == null)
                {
                    return false;
                }
            }

            await _flag.WaitAsync();

            try
            {
                bool isAnimated = IsAnimated;

                IsAnimated = false;

                if (_isImageSourceLoaded == true)
                {
                    for (int i = 0; i < _compositionChildren.Count; i++)
                    {
                        if (strategy == UIStrategy.PureXaml)
                        {
                            _xamlChildren[i].Fill = null;
                        }
                        else
                        {
                            _compositionChildren[i].Brush = null;
                        }
                    }

                    if (strategy == UIStrategy.Composition)
                    {
                        _brushVisual.Dispose();
                        _brushVisual = null;

                        _uriSurface.Dispose();
                        _uriSurface = null;
                    }
                }

                _isImageSourceLoaded = false;

                if (strategy == UIStrategy.Composition)
                {
                    var compositor = _containerVisual.Compositor;
                    var surfaceFactory = SurfaceFactory.GetSharedSurfaceFactoryForCompositor(compositor);

                    var surfaceUri = await surfaceFactory.CreateUriSurfaceAsync(uri);

                    _uriSurface = surfaceUri;
                    _brushVisual = compositor.CreateSurfaceBrush(surfaceUri.Surface);

                    _imageSize = surfaceUri.Size;
                }
                else
                {
                    BitmapImage image = new BitmapImage();

                    var storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);

                    using (var stream = await storageFile.OpenReadAsync())
                    {
                        image.SetSource(stream);
                    }

                    _brushXaml = new ImageBrush() { ImageSource = image };
                    _imageSize = new Size(image.PixelWidth, image.PixelHeight);
                }

                _isImageSourceLoaded = true;

                RefreshContainerTile();

                RefreshImageSize(_imageSize.Width, _imageSize.Height);

                if (isAnimated == true)
                {
                    IsAnimated = true;
                }
            }
            finally
            {
                _flag.Release();
            }

            ImageLoaded?.Invoke(this, EventArgs.Empty);

            return true;
        }

        /// <summary>
        /// Gets or sets the scroll orientation of the tile.
        /// Less images are drawn when you choose the Horizontal or Vertical value.
        /// </summary>
        public ScrollOrientation ScrollOrientation
        {
            get { return (ScrollOrientation)GetValue(ScrollOrientationProperty); }
            set { SetValue(ScrollOrientationProperty, value); }
        }

        private static async void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TileControl;
            await control.RefreshContainerTileLocked();
            if (control.Strategy == UIStrategy.Composition)
            {
                await control.CreateModuloExpression(control._scrollviewer);
            }
        }

        /// <inheritdoc/>
        protected override async void OnApplyTemplate()
        {
            var strategy = Strategy;

            var rootElement = _rootElement;

            if (rootElement != null)
            {
                rootElement.SizeChanged -= RootElement_SizeChanged;
            }

            // Gets the XAML root element
            rootElement = GetTemplateChild("RootElement") as FrameworkElement;

            _rootElement = rootElement;

            if (rootElement != null)
            {
                rootElement.SizeChanged += RootElement_SizeChanged;

                if (strategy == UIStrategy.Composition)
                {
                    // Get the Visual of the root element
                    Visual rootVisual = ElementCompositionPreview.GetElementVisual(rootElement);

                    if (rootVisual != null)
                    {
                        // We create a ContainerVisual to insert SpriteVisual with a brush
                        var container = rootVisual.Compositor.CreateContainerVisual();

                        // the containerVisual is now a child of rootVisual
                        ElementCompositionPreview.SetElementChildVisual(rootElement, container);

                        _containerVisual = container;
                        _rootVisual = rootVisual;

                        await CreateModuloExpression();
                    }
                }
                else
                {
                    _containerElement = rootElement.FindName("ContainerElement") as Canvas;
                    _containerTranslate = new TranslateTransform();
                    _containerElement.RenderTransform = _containerTranslate;
                }

                await LoadImageBrush(ImageSource);
            }

            base.OnApplyTemplate();
        }

        /// <summary>
        /// the size of the rootElement is changed
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arguments</param>
        private async void RootElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _isRootElementSizeChanged = true;
            await RefreshContainerTileLocked();
        }

        /// <summary>
        /// Refresh the ContainerVisual or ContainerElement with a lock
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task RefreshContainerTileLocked()
        {
            await _flag.WaitAsync();

            try
            {
                RefreshContainerTile();
            }
            finally
            {
                _flag.Release();
            }
        }

        /// <summary>
        /// Refresh the ContainerVisual or ContainerElement
        /// </summary>
        private void RefreshContainerTile()
        {
            if (_imageSize == Size.Empty || _rootElement == null)
            {
                return;
            }

            RefreshContainerTile(_rootElement.ActualWidth, _rootElement.ActualHeight, _imageSize.Width, _imageSize.Height, ScrollOrientation);
        }

        /// <summary>
        /// Refresh the ContainerVisual or ContainerElement
        /// </summary>
        /// <returns>Return true when the container is refreshed</returns>
        private bool RefreshContainerTile(double width, double height, double imageWidth, double imageHeight, ScrollOrientation orientation)
        {
            if (_isImageSourceLoaded == false || _isRootElementSizeChanged == false)
            {
                return false;
            }

            double numberSpriteToInstanciate = 0;

            int numberImagePerColumn = 1;
            int numberImagePerRow = 1;

            int offsetHorizontalAlignment = 0;
            int offsetVerticalAlignment = 0;

            var strategy = Strategy;

            if (_containerElement != null)
            {
                _containerElement.Width = width;
                _containerElement.Height = height;
            }

            var clip = new RectangleGeometry() { Rect = new Rect(0, 0, width, height) };
            _rootElement.Clip = clip;

            var imageAlignment = ImageAlignment;

            switch (orientation)
            {
                case ScrollOrientation.Horizontal:
                    numberImagePerColumn = (int)Math.Ceiling(width / imageWidth) + 1;

                    if (imageAlignment == ImageAlignment.Top || imageAlignment == ImageAlignment.Bottom)
                    {
                        numberImagePerRow = 1;

                        if (imageAlignment == ImageAlignment.Bottom)
                        {
                            offsetHorizontalAlignment = (int)(height - imageHeight);
                        }
                    }
                    else
                    {
                        numberImagePerRow = (int)Math.Ceiling(height / imageHeight);
                    }

                    numberSpriteToInstanciate = numberImagePerColumn * numberImagePerRow;
                    break;

                case ScrollOrientation.Vertical:
                    numberImagePerRow = (int)Math.Ceiling(height / imageHeight) + 1;

                    if (imageAlignment == ImageAlignment.Left || imageAlignment == ImageAlignment.Right)
                    {
                        numberImagePerColumn = 1;

                        if (ImageAlignment == ImageAlignment.Right)
                        {
                            offsetVerticalAlignment = (int)(width - imageWidth);
                        }
                    }
                    else
                    {
                        numberImagePerColumn = (int)Math.Ceiling(width / imageWidth);
                    }

                    numberSpriteToInstanciate = numberImagePerColumn * numberImagePerRow;

                    break;

                case ScrollOrientation.Both:
                    numberImagePerColumn = (int)Math.Ceiling(width / imageWidth) + 1;
                    numberImagePerRow = (int)(Math.Ceiling(height / imageHeight) + 1);
                    numberSpriteToInstanciate = numberImagePerColumn * numberImagePerRow;
                    break;
            }

            var count = 0;

            if (strategy == UIStrategy.Composition)
            {
                count = _compositionChildren.Count;
            }
            else
            {
                count = _xamlChildren.Count;
            }

            // instanciate all elements not created yet
            for (int x = 0; x < numberSpriteToInstanciate - count; x++)
            {
                if (strategy == UIStrategy.Composition)
                {
                    var sprite = _containerVisual.Compositor.CreateSpriteVisual();
                    _containerVisual.Children.InsertAtTop(sprite);
                    _compositionChildren.Add(sprite);
                }
                else
                {
                    var rectangle = new Rectangle();
                    _containerElement.Children.Add(rectangle);
                    _xamlChildren.Add(rectangle);
                }
            }

            // remove elements not used now
            for (int x = 0; x < count - numberSpriteToInstanciate; x++)
            {
                if (strategy == UIStrategy.Composition)
                {
                    var element = _containerVisual.Children.FirstOrDefault() as SpriteVisual;
                    _containerVisual.Children.Remove(element);
                    _compositionChildren.Remove(element);
                }
                else
                {
                    var element = _containerElement.Children.FirstOrDefault() as Rectangle;
                    _containerElement.Children.Remove(element);
                    _xamlChildren.Remove(element);
                }
            }

            // Change positions+brush for all actives elements
            for (int y = 0; y < numberImagePerRow; y++)
            {
                for (int x = 0; x < numberImagePerColumn; x++)
                {
                    int index = (y * numberImagePerColumn) + x;

                    if (strategy == UIStrategy.Composition)
                    {
                        var sprite = _compositionChildren[index];
                        sprite.Brush = _brushVisual;
                        sprite.Offset = new Vector3((float)((x * imageWidth) + offsetVerticalAlignment), (float)((y * imageHeight) + offsetHorizontalAlignment), 0);
                        sprite.Size = new Vector2((float)imageWidth, (float)imageHeight);
                    }
                    else
                    {
                        var rectangle = _xamlChildren[index];
                        rectangle.Fill = _brushXaml;

                        Canvas.SetLeft(rectangle, (x * imageWidth) + offsetVerticalAlignment);
                        Canvas.SetTop(rectangle, (y * imageHeight) + offsetHorizontalAlignment);
                        rectangle.Width = imageWidth;
                        rectangle.Height = imageHeight;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Gets or sets an X offset of the image
        /// </summary>
        public double OffsetX
        {
            get { return (double)GetValue(OffsetXProperty); }
            set { SetValue(OffsetXProperty, value); }
        }

        private static void OnOffsetChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as TileControl;

            c.RefreshMove();
        }

        /// <summary>
        /// Gets or sets an Y offset of the image
        /// </summary>
        public double OffsetY
        {
            get { return (double)GetValue(OffsetYProperty); }
            set { SetValue(OffsetYProperty, value); }
        }

        /// <summary>
        /// Gets or sets the speed ratio of the parallax effect with the <see cref="ScrollViewerContainer"/>
        /// </summary>
        public double ParallaxSpeedRatio
        {
            get { return (double)GetValue(ParallaxSpeedRatioProperty); }
            set { SetValue(ParallaxSpeedRatioProperty, value); }
        }

        private static void OnScrollSpeedRatioChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as TileControl;
            c.RefreshScrollSpeedRatio((double)e.NewValue);
        }

        /// <summary>
        /// Create the modulo expression and apply it to the ContainerVisual element
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task CreateModuloExpression(ScrollViewer scrollViewer = null)
        {
            await _flag.WaitAsync();

            try
            {
                double w = 0;
                double h = 0;

                if (_imageSize != Size.Empty)
                {
                    w = _imageSize.Width;
                    h = _imageSize.Height;
                }

                CreateModuloExpression(scrollViewer, w, h, ScrollOrientation);
            }
            finally
            {
                _flag.Release();
            }
        }

        /// <summary>
        /// Creation of an expression to manage modulo (positive and negative value)
        /// </summary>
        /// <param name="scrollviewer">The ScrollViewer to synchonized. A null value is valid</param>
        /// <param name="imageWidth">Width of the image</param>
        /// <param name="imageHeight">Height of the image</param>
        /// <param name="scrollOrientation">The ScrollOrientation</param>
        private void CreateModuloExpression(ScrollViewer scrollviewer, double imageWidth, double imageHeight, ScrollOrientation scrollOrientation)
        {
            const string offsetXParam = "offsetX";
            const string offsetYParam = "offsetY";
            const string imageWidthParam = "imageWidth";
            const string imageHeightParam = "imageHeight";
            const string speedParam = "speed";

            if (Strategy == UIStrategy.PureXaml)
            {
                return;
            }

            if (_containerVisual == null)
            {
                return;
            }

            var compositor = _containerVisual.Compositor;

            // Setup the expression
            ExpressionNode expressionX = null;
            ExpressionNode expressionY = null;
            ExpressionNode expressionXVal = null;
            ExpressionNode expressionYVal = null;

            var propertySetModulo = compositor.CreatePropertySet();
            propertySetModulo.InsertScalar(imageWidthParam, (float)imageWidth);
            propertySetModulo.InsertScalar(offsetXParam, (float)OffsetX);
            propertySetModulo.InsertScalar(imageHeightParam, (float)imageHeight);
            propertySetModulo.InsertScalar(offsetYParam, (float)OffsetY);
            propertySetModulo.InsertScalar(speedParam, (float)ParallaxSpeedRatio);

            var propertySetNodeModulo = propertySetModulo.GetReference();

            var imageHeightNode = propertySetNodeModulo.GetScalarProperty(imageHeightParam);
            var imageWidthNode = propertySetNodeModulo.GetScalarProperty(imageWidthParam);
            if (scrollviewer == null)
            {
                var offsetXNode = ExpressionFunctions.Ceil(propertySetNodeModulo.GetScalarProperty(offsetXParam));
                var offsetYNode = ExpressionFunctions.Ceil(propertySetNodeModulo.GetScalarProperty(offsetYParam));

                // expressions are created to simulate a positive and negative modulo with the size of the image and the offset
                expressionXVal = ExpressionFunctions.Conditional(
                    offsetXNode == 0,
                    0,
                    ExpressionFunctions.Conditional(
                        offsetXNode < 0,
                        -(ExpressionFunctions.Abs(offsetXNode - (ExpressionFunctions.Ceil(offsetXNode / imageWidthNode) * imageWidthNode)) % imageWidthNode),
                        -(imageWidthNode - (offsetXNode % imageWidthNode))));

                expressionYVal = ExpressionFunctions.Conditional(
                    offsetYNode == 0,
                    0,
                    ExpressionFunctions.Conditional(
                        offsetYNode < 0,
                        -(ExpressionFunctions.Abs(offsetYNode - (ExpressionFunctions.Ceil(offsetYNode / imageHeightNode) * imageHeightNode)) % imageHeightNode),
                        -(imageHeightNode - (offsetYNode % imageHeightNode))));
            }
            else
            {
                // expressions are created to simulate a positive and negative modulo with the size of the image and the offset and the ScrollViewer offset (Translation)
                var scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scrollviewer);
                var scrollPropSet = scrollProperties.GetSpecializedReference<ManipulationPropertySetReferenceNode>();

                var speed = propertySetNodeModulo.GetScalarProperty(speedParam);
                var xCommon = ExpressionFunctions.Ceil((scrollPropSet.Translation.X * speed) + propertySetNodeModulo.GetScalarProperty(offsetXParam));
                expressionXVal = ExpressionFunctions.Conditional(
                    xCommon == 0,
                    0,
                    ExpressionFunctions.Conditional(
                        xCommon < 0,
                        -(ExpressionFunctions.Abs(xCommon - (ExpressionFunctions.Ceil(xCommon / imageWidthNode) * imageWidthNode)) % imageWidthNode),
                        -(imageWidthNode - (xCommon % imageWidthNode))));

                var yCommon = ExpressionFunctions.Ceil((scrollPropSet.Translation.Y * speed) + propertySetNodeModulo.GetScalarProperty(offsetYParam));
                expressionYVal = ExpressionFunctions.Conditional(
                    yCommon == 0,
                    0,
                    ExpressionFunctions.Conditional(
                        yCommon < 0,
                        -(ExpressionFunctions.Abs(yCommon - (ExpressionFunctions.Ceil(yCommon / imageHeightNode) * imageHeightNode)) % imageHeightNode),
                        -(imageHeightNode - (yCommon % imageHeightNode))));
            }

            if (scrollOrientation == ScrollOrientation.Horizontal || scrollOrientation == ScrollOrientation.Both)
            {
                expressionX = expressionXVal;

                if (scrollOrientation == ScrollOrientation.Horizontal)
                {
                    // In horizontal mode we never move the offset y
                    expressionY = (ScalarNode)0.0f;
                    _containerVisual.Offset = new Vector3((float)OffsetY, 0, 0);
                }
            }

            if (scrollOrientation == ScrollOrientation.Vertical || scrollOrientation == ScrollOrientation.Both)
            {
                expressionY = expressionYVal;

                if (scrollOrientation == ScrollOrientation.Vertical)
                {
                    // In vertical mode we never move the offset x
                    expressionX = (ScalarNode)0.0f;
                    _containerVisual.Offset = new Vector3(0, (float)OffsetX, 0);
                }
            }

            _containerVisual.StopAnimation("Offset.X");
            _containerVisual.StopAnimation("Offset.Y");

            _containerVisual.StartAnimation("Offset.X", expressionX);
            _containerVisual.StartAnimation("Offset.Y", expressionY);

            _propertySetModulo = propertySetModulo;
        }

        private void RefreshMove()
        {
            RefreshMove(OffsetX + _animationX, OffsetY + _animationY);
        }

        /// <summary>
        /// Refresh the move
        /// </summary>
        private void RefreshMove(double x, double y)
        {
            lock (_lockerOffset)
            {
                if (Strategy == UIStrategy.Composition)
                {
                    if (_propertySetModulo == null)
                    {
                        return;
                    }

                    _propertySetModulo.InsertScalar("offsetX", (float)x);
                    _propertySetModulo.InsertScalar("offsetY", (float)y);
                }
                else
                {
                    var orientation = ScrollOrientation;

                    var scrollviewer = _scrollviewer;

                    double scrollX = 0;
                    double scrollY = 0;

                    if (scrollviewer != null)
                    {
                        var speedRatio = ParallaxSpeedRatio;

                        scrollX = -((scrollviewer.HorizontalOffset * scrollviewer.ActualWidth) / scrollviewer.ViewportWidth) * speedRatio;
                        scrollY = -((scrollviewer.VerticalOffset * scrollviewer.ActualHeight) / scrollviewer.ViewportHeight) * speedRatio;
                    }

                    if (orientation == ScrollOrientation.Both || orientation == ScrollOrientation.Horizontal)
                    {
                        _containerTranslate.X = GetOffsetModulo(x + scrollX, _imageSize.Width);

                        if (orientation == ScrollOrientation.Horizontal)
                        {
                            _containerTranslate.Y = 0;
                        }
                    }

                    if (orientation == ScrollOrientation.Both || orientation == ScrollOrientation.Vertical)
                    {
                        _containerTranslate.Y = GetOffsetModulo(y + scrollY, _imageSize.Height);

                        if (orientation == ScrollOrientation.Vertical)
                        {
                            _containerTranslate.X = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the offset after a modulo with the image size
        /// </summary>
        /// <param name="offset">the offset of the tile</param>
        /// <param name="size">the size of the image</param>
        /// <returns>the offset between 0 and the size of the image</returns>
        private double GetOffsetModulo(double offset, double size)
        {
            var offsetCeil = Math.Ceiling(offset);

            if (offsetCeil == 0)
            {
                return 0;
            }

            if (offsetCeil < 0)
            {
                return -(Math.Abs(offsetCeil - (Math.Ceiling(offsetCeil / size) * size)) % size);
            }
            else
            {
                return -(size - (offsetCeil % size));
            }
        }

        private void RefreshImageSize(double width, double height)
        {
            if (_propertySetModulo == null)
            {
                return;
            }

            _propertySetModulo.InsertScalar("imageWidth", (float)width);
            _propertySetModulo.InsertScalar("imageHeight", (float)height);
        }

        private void RefreshScrollSpeedRatio(double speedRatio)
        {
            if (_propertySetModulo == null)
            {
                return;
            }

            _propertySetModulo.InsertScalar("speed", (float)speedRatio);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tile is animated or not
        /// </summary>
        public bool IsAnimated
        {
            get { return (bool)GetValue(IsAnimatedProperty); }
            set { SetValue(IsAnimatedProperty, value); }
        }

        private static void OnIsAnimatedChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as TileControl;

            if ((bool)e.NewValue == true)
            {
                c._timerAnimation.Start();
            }
            else
            {
                c._timerAnimation.Stop();
                c._animationX = 0;
                c._animationY = 0;
            }
        }

        private void InitializeAnimation()
        {
            if (_timerAnimation == null)
            {
                _timerAnimation = new DispatcherTimer();
            }
            else
            {
                _timerAnimation.Stop();
            }

            _timerAnimation.Interval = TimeSpan.FromMilliseconds(AnimationDuration);
            _timerAnimation.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, object e)
        {
            var strategy = Strategy;

            if (strategy == UIStrategy.Composition && _containerVisual == null)
            {
                return;
            }

            var stepX = AnimationStepX;
            var stepY = AnimationStepY;

            if (stepX != 0)
            {
                // OffsetX = OffsetX + AnimationStepX;
                _animationX += stepX;
            }

            if (stepY != 0)
            {
                // OffsetY = OffsetY + AnimationStepY;
                _animationY += stepY;
            }

            if (stepX != 0 || stepY != 0)
            {
                RefreshMove();
            }
        }

        /// <summary>
        /// Gets or sets the animation step of the OffsetX
        /// </summary>
        public double AnimationStepX
        {
            get { return (double)GetValue(AnimationStepXProperty); }
            set { SetValue(AnimationStepXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the animation step of the OffsetY
        /// </summary>
        public double AnimationStepY
        {
            get { return (double)GetValue(AnimationStepYProperty); }
            set { SetValue(AnimationStepYProperty, value); }
        }

        /// <summary>
        /// Gets or sets a duration for the animation of the tile
        /// </summary>
        public double AnimationDuration
        {
            get { return (double)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        private static void OnAnimationDuration(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as TileControl;

            c._timerAnimation.Interval = TimeSpan.FromMilliseconds(c.AnimationDuration);
        }
    }
}
