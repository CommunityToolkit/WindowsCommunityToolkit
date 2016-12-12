// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;
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
    public sealed class MosaicControl : ContentControl
    {
        // Using a DependencyProperty as the backing store for ScrollViewer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollViewerContainerProperty =
            DependencyProperty.Register(nameof(ScrollViewerContainer), typeof(FrameworkElement), typeof(MosaicControl), new PropertyMetadata(null, OnScrollViewerContainerChange));

        // Using a DependencyProperty as the backing store for ImageAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageAlignmentProperty =
            DependencyProperty.Register(nameof(ImageAlignment), typeof(ImageAlignment), typeof(MosaicControl), new PropertyMetadata(ImageAlignment.None, OnAlignmentChange));

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(Uri), typeof(MosaicControl), new PropertyMetadata(null, OnImageSourceChanged));

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollOrientationProperty =
            DependencyProperty.Register(nameof(ScrollOrientation), typeof(ScrollOrientation), typeof(MosaicControl), new PropertyMetadata(ScrollOrientation.Both, OnOrientationChanged));

        // Using a DependencyProperty as the backing store for OffsetX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OffsetXProperty =
            DependencyProperty.Register(nameof(OffsetX), typeof(double), typeof(MosaicControl), new PropertyMetadata(0.0, OnOffsetChange));

        // Using a DependencyProperty as the backing store for OffsetY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OffsetYProperty =
            DependencyProperty.Register(nameof(OffsetY), typeof(double), typeof(MosaicControl), new PropertyMetadata(0.0, OnOffsetChange));

        // Using a DependencyProperty as the backing store for ScrollSpeedRatio.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParallaxSpeedRatioProperty =
            DependencyProperty.Register(nameof(ParallaxSpeedRatio), typeof(double), typeof(MosaicControl), new PropertyMetadata(1.0, OnScrollSpeedRatioChange));

        // Using a DependencyProperty as the backing store for IsAnimated.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAnimatedProperty =
            DependencyProperty.Register(nameof(IsAnimated), typeof(bool), typeof(MosaicControl), new PropertyMetadata(false, OnIsAnimatedChange));

        // Using a DependencyProperty as the backing store for AnimationStepX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimationStepXProperty =
            DependencyProperty.Register(nameof(AnimationStepX), typeof(double), typeof(MosaicControl), new PropertyMetadata(1.0));

        // Using a DependencyProperty as the backing store for AnimationStepY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimationStepYProperty =
            DependencyProperty.Register(nameof(AnimationStepY), typeof(double), typeof(MosaicControl), new PropertyMetadata(1.0));

        // Using a DependencyProperty as the backing store for AnimationDuration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register(nameof(AnimationDuration), typeof(double), typeof(MosaicControl), new PropertyMetadata(30.0, OnAnimationDuration));

        private FrameworkElement rootElement = null;
        private Canvas containerElement = null;
        private TranslateTransform containerTranslate = null;
        private ImageBrush brushXaml = null;

        private ContainerVisual containerVisual = null;
        private CompositionSurfaceBrush brushVisual = null;

        private Size imageSize = Size.Empty;

        private UriSurface uriSurface = null;
        private Visual rootVisual = null;

        private DispatcherTimer timerAnimation = null;

        /// <summary>
        /// A Scrollviewer used for synchronized the move of the <see cref="MosaicControl"/>
        /// </summary>
        private ScrollViewer scrollviewer = null;

        /// <summary>
        /// a flag to lock shared method
        /// </summary>
        private SemaphoreSlim flag = new SemaphoreSlim(1);

        private List<SpriteVisual> compositionChildren = new List<SpriteVisual>(50);
        private List<Rectangle> xamlChildren = new List<Rectangle>(50);

        private bool isImageSourceLoaded = false;
        private bool isRootElementSizeChanged = false;

        private CompositionPropertySet propertyOffsetModulo = null;
        private object lockerOffset = new object();

        private double animationX = 0;
        private double animationY = 0;

        private enum UIStrategy
        {
            /// <summary>
            /// MosaicControl is created with XAML
            /// </summary>
            PureXaml,

            /// <summary>
            /// MosaicControl is created with Microsoft Composition
            /// </summary>
            Composition
        }

        public event EventHandler ImageLoaded = null;

        /// <summary>
        /// Gets a value indicating whether the platform supports Composition.
        /// </summary>
        /// <remarks>
        /// On platforms not supporting Composition, this <See cref="UIStrategy"/> is automaticaly set to PureXaml.
        /// </remarks>
        public static bool IsCompositionSupported =>
            ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3); // SDK >= 14393

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicControl"/> class.
        /// </summary>
        public MosaicControl()
        {
            this.DefaultStyleKey = typeof(MosaicControl);

            this.InitializeAnimation();
        }

        /// <summary>
        /// Gets or sets a ScrollViewer or a frameworkElement containing a ScrollViewer.
        /// The mosaic control is synchronized with the offset of the scrollviewer
        /// </summary>
        public FrameworkElement ScrollViewerContainer
        {
            get { return (FrameworkElement)GetValue(ScrollViewerContainerProperty); }
            set { SetValue(ScrollViewerContainerProperty, value); }
        }

        private static async void OnScrollViewerContainerChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MosaicControl;
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
            return this.AttachScrollViewer(newScrollViewerContainer);
        }

        /// <summary>
        /// ScrollViewer is loaded
        /// </summary>
        /// <param name="sender">a ScrollViewerContainer</param>
        /// <param name="e">arguments</param>
        private async void ScrollViewerContainer_Loaded(object sender, RoutedEventArgs e)
        {
            await this.AttachScrollViewer(sender as FrameworkElement);
        }

        /// <summary>
        /// Gets how the <see cref="MosaicControl"/> is rendered
        /// The default value is Composition.
        /// </summary>
        private UIStrategy Strategy
        {
            get
            {
                if (currentStrategy == null)
                {
                    if (DesignMode.DesignModeEnabled == true || IsCompositionSupported == false)
                    {
                        currentStrategy = UIStrategy.PureXaml;
                    }

                    currentStrategy = UIStrategy.Composition;
                }

                return currentStrategy.Value;
            }
        }

        private UIStrategy? currentStrategy = null;

        /// <summary>
        /// Gets or sets the alignment of the mosaic when the <see cref="ScrollOrientation"/> is set to Vertical or Horizontal.
        /// Valid values are Left or Right for <see cref="ScrollOrientation"/> set to Horizontal and Top or Bottom for <see cref="ScrollOrientation"/> set to Vertical.
        /// </summary>
        public ImageAlignment ImageAlignment
        {
            get { return (ImageAlignment)GetValue(ImageAlignmentProperty); }
            set { SetValue(ImageAlignmentProperty, value); }
        }

        private static async void OnAlignmentChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MosaicControl;
            await control.RefreshContainerMosaicLocked();
        }

        /// <summary>
        /// Attach a scrollviewer to the MosaicControl (parallax effect)
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

            if (newScrollviewer != scrollviewer)
            {
                var strategy = this.Strategy;

                if (strategy == UIStrategy.Composition)
                {
                    // Update the expression
                    await this.CreateModuloExpression(newScrollviewer);
                }
                else
                {
                    if (this.scrollviewer != null)
                    {
                        this.scrollviewer.ViewChanging -= Scrollviewer_ViewChanging;
                    }

                    if (newScrollviewer != null)
                    {
                        newScrollviewer.ViewChanging += Scrollviewer_ViewChanging;
                    }
                }

                scrollviewer = newScrollviewer;
            }
        }

        private void Scrollviewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            this.RefreshMove();
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
            var control = d as MosaicControl;
            await control.LoadImageBrush(e.NewValue as Uri);
        }

        /// <summary>
        /// Load the image and transform it to a composition brush or a XAML brush (depends of the UIStrategy)
        /// </summary>
        /// <param name="uri">the uri of the image to load</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<bool> LoadImageBrush(Uri uri)
        {
            var strategy = this.Strategy;

            if (strategy == UIStrategy.Composition)
            {
                if (this.containerVisual == null || uri == null)
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

            await flag.WaitAsync();

            try
            {
                bool isAnimated = this.IsAnimated;

                this.IsAnimated = false;

                if (this.isImageSourceLoaded == true)
                {
                    for (int i = 0; i < this.compositionChildren.Count; i++)
                    {
                        if (strategy == UIStrategy.PureXaml)
                        {
                            this.xamlChildren[i].Fill = null;
                        }
                        else
                        {
                            this.compositionChildren[i].Brush = null;
                        }
                    }

                    if (strategy == UIStrategy.Composition)
                    {
                        this.brushVisual.Dispose();
                        this.brushVisual = null;

                        this.uriSurface.Dispose();
                        this.uriSurface = null;
                    }
                }

                this.isImageSourceLoaded = false;

                if (strategy == UIStrategy.Composition)
                {
                    var compositor = this.containerVisual.Compositor;

                    using (var surfaceFactory = SurfaceFactory.CreateFromCompositor(compositor))
                    {
                        var surfaceUri = await surfaceFactory.CreateUriSurfaceAsync(uri);

                        this.uriSurface = surfaceUri;
                        this.brushVisual = compositor.CreateSurfaceBrush(surfaceUri.Surface);

                        imageSize = surfaceUri.Size;
                    }
                }
                else
                {
                    BitmapImage image = new BitmapImage();

                    var storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);

                    using (var stream = await storageFile.OpenReadAsync())
                    {
                        image.SetSource(stream);
                    }

                    this.brushXaml = new ImageBrush() { ImageSource = image };
                    this.imageSize = new Size(image.PixelWidth, image.PixelHeight);
                }

                this.isImageSourceLoaded = true;

                this.RefreshContainerMosaic();

                this.RefreshImageSize(imageSize.Width, imageSize.Height);

                if (isAnimated == true)
                {
                    this.IsAnimated = true;
                }
            }
            finally
            {
                flag.Release();
            }

            if (this.ImageLoaded != null)
            {
                this.ImageLoaded(this, EventArgs.Empty);
            }

            return true;
        }

        /// <summary>
        /// Gets or sets the scroll orientation of the mosaic.
        /// Less images are drawn when you choose the Horizontal or Vertical value.
        /// </summary>
        public ScrollOrientation ScrollOrientation
        {
            get { return (ScrollOrientation)GetValue(ScrollOrientationProperty); }
            set { SetValue(ScrollOrientationProperty, value); }
        }

        private static async void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MosaicControl;
            await control.RefreshContainerMosaicLocked();
            await control.CreateModuloExpression(control.scrollviewer);
        }

        /// <inheritdoc/>
        protected override async void OnApplyTemplate()
        {
            var strategy = this.Strategy;

            // Gets the XAML root element
            var rootElement = this.GetTemplateChild("RootElement") as FrameworkElement;

            if (rootElement != null)
            {
                this.rootElement = rootElement;
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

                        this.containerVisual = container;
                        this.rootVisual = rootVisual;

                        await this.CreateModuloExpression();
                    }
                }
                else
                {
                    this.containerElement = rootElement.FindName("ContainerElement") as Canvas;
                    this.containerTranslate = new TranslateTransform();
                    this.containerElement.RenderTransform = this.containerTranslate;
                }

                await this.LoadImageBrush(this.ImageSource);
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
            Debug.WriteLine("sizeChanged=" + e.NewSize.Width);
            this.isRootElementSizeChanged = true;
            await this.RefreshContainerMosaicLocked();
        }

        /// <summary>
        /// Refresh the ContainerVisual or ContainerElement with a lock
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task RefreshContainerMosaicLocked()
        {
            await flag.WaitAsync();

            try
            {
                this.RefreshContainerMosaic();
            }
            finally
            {
                flag.Release();
            }
        }

        /// <summary>
        /// Refresh the ContainerVisual or ContainerElement
        /// </summary>
        private void RefreshContainerMosaic()
        {
            if (this.imageSize == Size.Empty || this.rootElement == null)
            {
                return;
            }

            Debug.WriteLine("RefreshContainerVisual=" + this.rootElement.ActualWidth);
                this.RefreshContainerMosaic(this.rootElement.ActualWidth, this.rootElement.ActualHeight, this.imageSize.Width, this.imageSize.Height, this.ScrollOrientation);
        }

        /// <summary>
        /// Refresh the ContainerVisual or ContainerElement
        /// </summary>
        /// <returns>Return true when the container is refreshed</returns>
        private bool RefreshContainerMosaic(double width, double height, double imageWidth, double imageHeight, ScrollOrientation orientation)
        {
            if (isImageSourceLoaded == false || this.isRootElementSizeChanged == false)
            {
                return false;
            }

            double numberSpriteToInstanciate = 0;

            int numberImagePerColumn = 1;
            int numberImagePerRow = 1;

            int offsetHorizontalAlignment = 0;
            int offsetVerticalAlignment = 0;

            var strategy = this.Strategy;

            if (this.containerElement != null)
            {
                this.containerElement.Width = width;
                this.containerElement.Height = height;
            }

            var clip = new RectangleGeometry() { Rect = new Rect(0, 0, width, height) };
            this.rootElement.Clip = clip;

            var imageAlignment = this.ImageAlignment;

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
                count = compositionChildren.Count;
            }
            else
            {
                count = xamlChildren.Count;
            }

            // instanciate all elements not created yet
            for (int x = 0; x < numberSpriteToInstanciate - count; x++)
            {
                if (strategy == UIStrategy.Composition)
                {
                    var sprite = this.containerVisual.Compositor.CreateSpriteVisual();
                    this.containerVisual.Children.InsertAtTop(sprite);
                    compositionChildren.Add(sprite);
                }
                else
                {
                    var rectangle = new Rectangle();
                    this.containerElement.Children.Add(rectangle);
                    xamlChildren.Add(rectangle);
                }
            }

            // remove elements not used now
            for (int x = 0; x < count - numberSpriteToInstanciate; x++)
            {
                if (strategy == UIStrategy.Composition)
                {
                    var element = this.containerVisual.Children.FirstOrDefault() as SpriteVisual;
                    this.containerVisual.Children.Remove(element);
                    compositionChildren.Remove(element);
                }
                else
                {
                    var element = this.containerElement.Children.FirstOrDefault() as Rectangle;
                    this.containerElement.Children.Remove(element);
                    xamlChildren.Remove(element);
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
                        var sprite = compositionChildren[index];
                        sprite.Brush = this.brushVisual;
                        sprite.Offset = new Vector3((float)((x * imageWidth) + offsetVerticalAlignment), (float)((y * imageHeight) + offsetHorizontalAlignment), 0);
                        sprite.Size = new Vector2((float)imageWidth, (float)imageHeight);
                    }
                    else
                    {
                        var rectangle = xamlChildren[index];
                        rectangle.Fill = this.brushXaml;

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
            var c = d as MosaicControl;

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
            var c = d as MosaicControl;
            c.RefreshScrollSpeedRatio((double)e.NewValue);
        }

        /// <summary>
        /// Create the modulo expression and apply it to the ContainerVisual element
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task CreateModuloExpression(ScrollViewer scrollViewer = null)
        {
            await flag.WaitAsync();

            try
            {
                double w = 0;
                double h = 0;

                if (this.imageSize != Size.Empty)
                {
                    w = this.imageSize.Width;
                    h = this.imageSize.Height;
                }

                this.CreateModuloExpression(scrollViewer, w, h, this.ScrollOrientation);
            }
            finally
            {
                flag.Release();
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
            if (this.Strategy == UIStrategy.PureXaml)
            {
                return;
            }

            if (this.containerVisual == null)
            {
                return;
            }

            var compositor = this.containerVisual.Compositor;

            // Setup the expression
            var expressionX = compositor.CreateExpressionAnimation();
            var expressionY = compositor.CreateExpressionAnimation();

            var propertyOffsetModulo = compositor.CreatePropertySet();

            propertyOffsetModulo.InsertScalar("imageWidth", (float)imageWidth);
            propertyOffsetModulo.InsertScalar("offsetX", (float)this.OffsetX);
            propertyOffsetModulo.InsertScalar("imageHeight", (float)imageHeight);
            propertyOffsetModulo.InsertScalar("offsetY", (float)this.OffsetY);
            propertyOffsetModulo.InsertScalar("speed", (float)this.ParallaxSpeedRatio);

            expressionX.SetReferenceParameter("p", propertyOffsetModulo);
            expressionY.SetReferenceParameter("p", propertyOffsetModulo);

            string expressionXString = null;
            string expressionYString = null;

            if (scrollviewer == null)
            {
                // expressions are create to simulate a positive and negative modulo with the size of the image and the offset
                expressionXString = "Ceil(p.offsetX) == 0 ? 0 : (Ceil(p.offsetX) < 0 ? -(Abs(Ceil(p.offsetX) - Ceil(Ceil(p.offsetX) / p.imageWidth) * p.imageWidth) % p.imageWidth) : -(p.imageWidth - (Ceil(p.offsetX) % p.imageWidth)))";
                expressionYString = "Ceil(p.offsetY) == 0 ? 0 : (Ceil(p.offsetY) < 0 ? -(Abs(Ceil(p.offsetY) - Ceil(Ceil(p.offsetY) / p.imageHeight) * p.imageHeight) % p.imageHeight) : -(p.imageHeight - (Ceil(p.offsetY) % p.imageHeight)))";
            }
            else
            {
                // expressions are create to simulate a positive and negative modulo with the size of the image and the offset and the ScrollViewer offset (Translation)
                var scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scrollviewer);

                expressionX.SetReferenceParameter("s", scrollProperties);
                expressionY.SetReferenceParameter("s", scrollProperties);

                expressionXString = "Ceil((s.Translation.X * p.speed) + p.offsetX) == 0 ? 0 : (Ceil((s.Translation.X * p.speed) + p.offsetX) < 0 ? -(Abs(Ceil((s.Translation.X * p.speed) + p.offsetX) - Ceil(Ceil((s.Translation.X * p.speed) + p.offsetX) / p.imageWidth) * p.imageWidth) % p.imageWidth) : -(p.imageWidth - (Ceil((s.Translation.X * p.speed) + p.offsetX) % p.imageWidth)))";
                expressionYString = "Ceil((s.Translation.Y * p.speed) + p.offsetY) == 0 ? 0 : (Ceil((s.Translation.Y * p.speed) + p.offsetY) < 0 ? -(Abs(Ceil((s.Translation.Y * p.speed) + p.offsetY) - Ceil(Ceil((s.Translation.Y * p.speed) + p.offsetY) / p.imageHeight) * p.imageHeight) % p.imageHeight) : -(p.imageHeight - (Ceil((s.Translation.Y * p.speed) + p.offsetY) % p.imageHeight)))";
            }

            if (scrollOrientation == ScrollOrientation.Horizontal || scrollOrientation == ScrollOrientation.Both)
            {
                expressionX.Expression = expressionXString;

                if (scrollOrientation == ScrollOrientation.Horizontal)
                {
                    // In horizontal mode we never move the offset y
                    expressionY.Expression = "0";
                    this.containerVisual.Offset = new Vector3((float)this.OffsetY, 0, 0);
                }
            }

            if (scrollOrientation == ScrollOrientation.Vertical || scrollOrientation == ScrollOrientation.Both)
            {
                expressionY.Expression = expressionYString;

                if (scrollOrientation == ScrollOrientation.Vertical)
                {
                    // In vertical mode we never move the offset x
                    expressionX.Expression = "0";
                    this.containerVisual.Offset = new Vector3(0, (float)this.OffsetX, 0);
                }
            }

            this.containerVisual.StopAnimation("Offset.X");
            this.containerVisual.StopAnimation("Offset.Y");

            this.containerVisual.StartAnimation("Offset.X", expressionX);
            this.containerVisual.StartAnimation("Offset.Y", expressionY);

            this.propertyOffsetModulo = propertyOffsetModulo;
        }

        private void RefreshMove()
        {
            this.RefreshMove(this.OffsetX + this.animationX, this.OffsetY + this.animationY);
        }

        /// <summary>
        /// Refresh the move
        /// </summary>
        private void RefreshMove(double x, double y)
        {
            lock (lockerOffset)
            {
                if (this.Strategy == UIStrategy.Composition)
                {
                    if (propertyOffsetModulo == null)
                    {
                        return;
                    }

                    propertyOffsetModulo.InsertScalar("offsetX", (float)x);
                    propertyOffsetModulo.InsertScalar("offsetY", (float)y);
                }
                else
                {
                    var orientation = this.ScrollOrientation;

                    var scrollviewer = this.scrollviewer;

                    double scrollX = 0;
                    double scrollY = 0;

                    if (scrollviewer != null)
                    {
                        var speedRatio = this.ParallaxSpeedRatio;

                        scrollX = -((scrollviewer.HorizontalOffset * scrollviewer.ActualWidth) / scrollviewer.ViewportWidth) * speedRatio;
                        scrollY = -((scrollviewer.VerticalOffset * scrollviewer.ActualHeight) / scrollviewer.ViewportHeight) * speedRatio;
                    }

                    if (orientation == ScrollOrientation.Both || orientation == ScrollOrientation.Horizontal)
                    {
                        this.containerTranslate.X = GetOffsetModulo(x + scrollX, this.imageSize.Width);

                        if (orientation == ScrollOrientation.Horizontal)
                        {
                            this.containerTranslate.Y = 0;
                        }
                    }

                    if (orientation == ScrollOrientation.Both || orientation == ScrollOrientation.Vertical)
                    {
                        this.containerTranslate.Y = GetOffsetModulo(y + scrollY, this.imageSize.Height);

                        if (orientation == ScrollOrientation.Vertical)
                        {
                            this.containerTranslate.X = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the offset after a modulo with the image size
        /// </summary>
        /// <param name="offset">the offset of the mosaic</param>
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
            if (propertyOffsetModulo == null)
            {
                return;
            }

            propertyOffsetModulo.InsertScalar("imageWidth", (float)width);
            propertyOffsetModulo.InsertScalar("imageHeight", (float)height);
        }

        private void RefreshScrollSpeedRatio(double speedRatio)
        {
            if (propertyOffsetModulo == null)
            {
                return;
            }

            propertyOffsetModulo.InsertScalar("speed", (float)speedRatio);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the mosaic is animated or not
        /// </summary>
        public bool IsAnimated
        {
            get { return (bool)GetValue(IsAnimatedProperty); }
            set { SetValue(IsAnimatedProperty, value); }
        }

        private static void OnIsAnimatedChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as MosaicControl;

            if ((bool)e.NewValue == true)
            {
                c.timerAnimation.Start();
            }
            else
            {
                c.timerAnimation.Stop();
                c.animationX = 0;
                c.animationY = 0;
            }
        }

        private void InitializeAnimation()
        {
            if (timerAnimation == null)
            {
                timerAnimation = new DispatcherTimer();
            }
            else
            {
                timerAnimation.Stop();
            }

            timerAnimation.Interval = TimeSpan.FromMilliseconds(this.AnimationDuration);
            timerAnimation.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, object e)
        {
            var strategy = this.Strategy;

            if (strategy == UIStrategy.Composition && this.containerVisual == null)
            {
                return;
            }

            var stepX = this.AnimationStepX;
            var stepY = this.AnimationStepY;

            if (stepX != 0)
            {
                // this.OffsetX = this.OffsetX + this.AnimationStepX;
                this.animationX += stepX;
            }

            if (stepY != 0)
            {
                // this.OffsetY = this.OffsetY + this.AnimationStepY;
                this.animationY += stepY;
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
        /// Gets or sets a duration for the animation of the mosaic
        /// </summary>
        public double AnimationDuration
        {
            get { return (double)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        private static void OnAnimationDuration(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as MosaicControl;

            c.timerAnimation.Interval = TimeSpan.FromMilliseconds(c.AnimationDuration);
        }
    }
}
