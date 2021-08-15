// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Toolkit.Uwp.UI.Media.Geometry;
using Windows.Foundation;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Class for rendering an image onto a <see cref="ICompositionSurface"/>.
    /// </summary>
    internal sealed class ImageSurface : IImageSurface
    {
        private readonly object _surfaceLock;
        private ICompositionGeneratorInternal _generator;
        private CompositionDrawingSurface _surface;
        private Uri _uri;
        private CanvasBitmap _canvasBitmap;
        private bool _raiseLoadCompletedEvent;

        /// <summary>
        /// Event that is raised when the image has been downloaded, decoded and loaded
        /// to the underlying IImageSurface. This event fires regardless of success or failure.
        /// </summary>
        public event TypedEventHandler<IImageSurface, ImageSurfaceLoadStatus> LoadCompleted;

        /// <inheritdoc/>
        public ICompositionGenerator Generator => _generator;

        /// <inheritdoc/>
        public ICompositionSurface Surface => _surface;

        /// <inheritdoc/>
        public Uri Uri => _uri;

        /// <inheritdoc/>
        public Size Size { get; private set; }

        /// <inheritdoc/>
        public ImageSurfaceOptions Options { get; private set; }

        /// <inheritdoc/>
        public Size DecodedPhysicalSize { get; private set; }

        /// <inheritdoc/>
        public Size DecodedSize { get; private set; }

        /// <inheritdoc/>
        public ImageSurfaceLoadStatus Status { get; private set; }

        /// <inheritdoc/>
        public CanvasBitmap SurfaceBitmap => _canvasBitmap;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSurface"/> class.
        /// Constructor
        /// </summary>
        /// <param name="generator"><see cref="ICompositionGeneratorInternal"/> object.</param>
        /// <param name="uri"><see cref="System.Uri"/> of the image to be loaded onto the <see cref="ImageSurface"/>.</param>
        /// <param name="size">Size of the <see cref="ImageSurface"/>.</param>
        /// <param name="options">The image's resize and alignment options in the allocated space.</param>
        public ImageSurface(ICompositionGeneratorInternal generator, Uri uri, Size size, ImageSurfaceOptions options)
        {
            _generator = generator ?? throw new ArgumentException("Generator cannot be null!", nameof(generator));

            _generator = generator;
            _surfaceLock = new object();

            // Create the Surface of the IImageSurface
            _surface = _generator.CreateDrawingSurface(_surfaceLock, size);
            Size = _surface?.Size ?? new Size(0, 0);
            _uri = uri;
            _raiseLoadCompletedEvent = _uri != null;
            _canvasBitmap = null;

            // Set the image options
            Options = options;

            // Subscribe to DeviceReplaced event
            _generator.DeviceReplaced += OnDeviceReplaced;
            Status = ImageSurfaceLoadStatus.None;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSurface"/> class.
        /// Constructor
        /// </summary>
        /// <param name="generator"><see cref="ICompositionGeneratorInternal"/> object.</param>
        /// <param name="surfaceBitmap"><see cref="CanvasBitmap"/> which will be rendered on the <see cref="ImageSurface"/>.</param>
        /// <param name="size">Size of the <see cref="ImageSurface"/>.</param>
        /// <param name="options">The image's resize and alignment options in the allocated space.</param>
        internal ImageSurface(ICompositionGeneratorInternal generator, CanvasBitmap surfaceBitmap, Size size, ImageSurfaceOptions options)
        {
            _generator = generator ?? throw new ArgumentException("Generator cannot be null!", nameof(generator));

            _generator = generator;
            _surfaceLock = new object();

            // Create the Surface of the IImageSurface
            _surface = _generator.CreateDrawingSurface(_surfaceLock, size);
            Size = _surface?.Size ?? new Size(0, 0);
            _uri = null;
            _raiseLoadCompletedEvent = false;
            if (surfaceBitmap != null)
            {
                _canvasBitmap = CanvasBitmap.CreateFromBytes(
                    _generator.Device,
                    surfaceBitmap.GetPixelBytes(),
                    (int)surfaceBitmap.Bounds.Width,
                    (int)surfaceBitmap.Bounds.Height,
                    surfaceBitmap.Format);
            }

            // Set the image options
            Options = options;

            // Subscribe to DeviceReplaced event
            _generator.DeviceReplaced += OnDeviceReplaced;
        }

        /// <inheritdoc/>
        public void Redraw()
        {
            // Reload the IImageSurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(ImageSurfaceOptions options)
        {
            // Set the image options
            Options = options;

            // Redraw the IImageSurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(Size size, ImageSurfaceOptions options)
        {
            // Resize the surface only if AutoResize option is disabled
            if (!Options.AutoResize && Size != size)
            {
                // resize the IImageMaskSurface
                _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

                // Set the size
                Size = _surface?.Size ?? new Size(0, 0);
            }

            // Set the image options
            Options = options;

            // Redraw the IImageSurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Redraw(IImageSurface imageSurface)
        {
            if (imageSurface != null)
            {
                Redraw(imageSurface.SurfaceBitmap, imageSurface.Size, imageSurface.Options);
            }
            else
            {
                // Draw an empty surface
                Redraw(surfaceBitmap: null);
            }
        }

        /// <inheritdoc/>
        public void Redraw(IImageSurface imageSurface, ImageSurfaceOptions options)
        {
            if (imageSurface != null)
            {
                Redraw(imageSurface.SurfaceBitmap, imageSurface.Size, options);
            }
            else
            {
                // Draw an empty surface
                Redraw(surfaceBitmap: null, options);
            }
        }

        /// <inheritdoc/>
        public void Redraw(IImageSurface imageSurface, Size size, ImageSurfaceOptions options)
        {
            Redraw(imageSurface?.SurfaceBitmap, size, options);
        }

        /// <inheritdoc/>
        public void Redraw(CanvasBitmap surfaceBitmap)
        {
            Redraw(surfaceBitmap, Size, Options);
        }

        /// <inheritdoc/>
        public void Redraw(CanvasBitmap surfaceBitmap, ImageSurfaceOptions options)
        {
            // Set the image options
            Options = options;

            // Redraw the IImageSurface
            Redraw(surfaceBitmap, Size, Options);
        }

        /// <inheritdoc/>
        public void Redraw(CanvasBitmap surfaceBitmap, Size size, ImageSurfaceOptions options)
        {
            if (_canvasBitmap != surfaceBitmap)
            {
                // Dispose the previous canvas bitmap resource (if any)
                if (_canvasBitmap != null)
                {
                    _canvasBitmap.Dispose();
                    _canvasBitmap = null;
                }

                if (surfaceBitmap != null)
                {
                    if (_canvasBitmap != surfaceBitmap)
                    {
                        // Copy the surface bitmap onto _canvasBitmap
                        _canvasBitmap = CanvasBitmap.CreateFromBytes(
                            _generator.Device,
                            surfaceBitmap.GetPixelBytes(),
                            (int)surfaceBitmap.Bounds.Width,
                            (int)surfaceBitmap.Bounds.Height,
                            surfaceBitmap.Format);
                    }
                }
                else
                {
                    _canvasBitmap = null;
                }
            }

            _uri = null;
            _raiseLoadCompletedEvent = false;

            // Set the options
            Options = options;

            // Resize the surface only if AutoResize option is disabled
            if (!Options.AutoResize && Size != size)
            {
                // resize the IImageMaskSurface
                _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

                // Set the size
                Size = _surface?.Size ?? new Size(0, 0);
            }

            // Redraw the IImageMaskSurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public Task RedrawAsync(Uri uri)
        {
            return RedrawAsync(uri, Size, Options);
        }

        /// <inheritdoc/>
        public Task RedrawAsync(Uri uri, ImageSurfaceOptions options)
        {
            return RedrawAsync(uri, Size, options);
        }

        /// <inheritdoc/>
        public Task RedrawAsync(Uri uri, Size size, ImageSurfaceOptions options)
        {
            // If the given Uri differs from the previously stored Uri or if the ImageSurface was
            // directly created from a CanvasBitmap, dispose the existing canvasBitmap
            if ((_uri != null && !_uri.IsEqualTo(uri))
                || (_uri == null && _canvasBitmap != null))
            {
                _canvasBitmap?.Dispose();
                _canvasBitmap = null;
                _raiseLoadCompletedEvent = uri != null;
            }

            // Set the image options
            Options = options;

            // Resize the surface only if AutoResize option is disabled
            if (!Options.AutoResize)
            {
                // resize the IImageSurface
                _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

                // Set the size
                Size = _surface?.Size ?? new Size(0, 0);
            }

            // Set the new Uri of the image to be loaded
            _uri = uri;

            // Reload the IImageSurface
            return RedrawSurfaceAsync();
        }

        /// <inheritdoc/>
        public void Resize(Size size)
        {
            Resize(size, Options);
        }

        /// <inheritdoc/>
        public void Resize(Size size, ImageSurfaceOptions options)
        {
            // Set the image options
            Options = options;

            // Resize the surface only if AutoResize option is disabled
            if (!Options.AutoResize)
            {
                // resize the IImageSurface
                _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

                // Set the size
                Size = _surface?.Size ?? new Size(0, 0);
            }

            // redraw the IImageSurface
            RedrawSurface();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _surface?.Dispose();
            if (_generator != null)
            {
                _generator.DeviceReplaced -= OnDeviceReplaced;
            }

            _canvasBitmap?.Dispose();
            _canvasBitmap = null;
            _surface = null;
            _generator = null;
            _uri = null;
            Options = null;
        }

        /// <summary>
        /// Redraws the <see cref="ImageSurface"/> asynchronously by loading the image from the Uri.
        /// </summary>
        /// <returns>Task</returns>
        internal Task RedrawAsync()
        {
            // Reload the IImageSurface
            return RedrawSurfaceAsync();
        }

        /// <summary>
        /// Handles the DeviceReplaced event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">object.</param>
        private async void OnDeviceReplaced(object sender, object e)
        {
            // Recreate the ImageSurface
            _surface = _generator.CreateDrawingSurface(_surfaceLock, Size);

            // Reload the IImageSurface
            await RedrawSurfaceAsync();
        }

        /// <summary>
        /// Helper class to redraw the <see cref="ImageSurface"/> synchronously.
        /// </summary>
        private void RedrawSurface()
        {
            // Resize the surface image
            _generator.RedrawImageSurface(_surfaceLock, _surface, Options, _canvasBitmap);

            // If AutoResize is allowed and the image is successfully loaded into the canvasBitmap,
            // then update the Size property of the surface as the surface has been resized to match the canvasBitmap size
            if (Options.AutoResize)
            {
                // If the image is successfully loaded into the canvasBitmap, then update the Size property
                // of the surface as the surface has been resized to match the canvasBitmap size
                Size = _canvasBitmap?.Size ?? new Size(0, 0);
            }

            Status = _canvasBitmap != null ? ImageSurfaceLoadStatus.Success : ImageSurfaceLoadStatus.Error;

            if (_canvasBitmap != null)
            {
                DecodedPhysicalSize = new Size(_canvasBitmap.SizeInPixels.Width, _canvasBitmap.SizeInPixels.Height);
                DecodedSize = _canvasBitmap.Size;
            }
        }

        /// <summary>
        /// Helper class to redraw the <see cref="ImageSurface"/> asynchronously.
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task RedrawSurfaceAsync()
        {
            // Cache the canvasBitmap to avoid reloading of the same image during Resize/Redraw operations
            _canvasBitmap = await _generator.RedrawImageSurfaceAsync(_surfaceLock, _surface, _uri, Options, _canvasBitmap);

            // If AutoResize is allowed and the image is successfully loaded into the canvasBitmap,
            // then update the Size property of the surface as the surface has been resized to match the canvasBitmap size
            if (Options.AutoResize)
            {
                // If the image is successfully loaded into the canvasBitmap, then update the Size property
                // of the surface as the surface has been resized to match the canvasBitmap size
                Size = _canvasBitmap?.Size ?? new Size(0, 0);
            }

            Status = _canvasBitmap != null ? ImageSurfaceLoadStatus.Success : ImageSurfaceLoadStatus.Error;

            // Get the canvasbitmap dimensions
            if (_canvasBitmap != null)
            {
                DecodedPhysicalSize = new Size(_canvasBitmap.SizeInPixels.Width, _canvasBitmap.SizeInPixels.Height);
                DecodedSize = _canvasBitmap.Size;
            }

            // Raise the event
            if (_raiseLoadCompletedEvent)
            {
                LoadCompleted?.Invoke(this, Status);
                _raiseLoadCompletedEvent = false;
            }
        }
    }
}