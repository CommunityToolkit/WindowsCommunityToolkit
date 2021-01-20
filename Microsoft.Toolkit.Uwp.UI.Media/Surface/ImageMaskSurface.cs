// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Toolkit.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Media.Geometry;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media.Surface
{
    internal sealed class ImageMaskSurface : IImageMaskSurface
    {
        private readonly object _surfaceLock;
        private ICompositionGeneratorInternal _generator;
        private CompositionDrawingSurface _surface;
        private Uri _uri;
        private CanvasBitmap _canvasBitmap;
        private bool _raiseLoadCompletedEvent;

        /// <summary>
        /// Event that is raised when the image has been downloaded,
        /// decoded and loaded to the underlying IImageSurface. This
        /// event fires regardless of success or failure.
        /// </summary>
        public event TypedEventHandler<IImageSurface, ImageSurfaceLoadStatus> LoadCompleted;

        /// <summary>
        /// Gets the CompositionGenerator
        /// </summary>
        public ICompositionGenerator Generator => _generator;

        /// <summary>
        /// Gets the Surface of the IImageMaskSurface
        /// </summary>
        public ICompositionSurface Surface => _surface;

        /// <summary>
        /// Gets the Uri of the image to be loaded onto the IImageSurface
        /// </summary>
        public Uri Uri => _uri;

        /// <summary>
        /// Gets the CanvasBitmap representing the loaded image
        /// </summary>
        public CanvasBitmap SurfaceBitmap => _canvasBitmap;

        /// <summary>
        /// Gets the IImageMaskSurface Size
        /// </summary>
        public Size Size { get; private set; }

        /// <summary>
        /// Gets the image's resize and alignment options in the allocated space.
        /// </summary>
        public ImageSurfaceOptions Options { get; private set; }

        /// <summary>
        /// Gets the size of the decoded image in physical pixels.
        /// </summary>
        public Size DecodedPhysicalSize { get; private set; }

        /// <summary>
        /// Gets the size of the decoded image in device independent pixels.
        /// </summary>
        public Size DecodedSize { get; private set; }

        /// <summary>
        /// Gets the status whether the image was loaded successfully or not.
        /// </summary>
        public ImageSurfaceLoadStatus Status { get; private set; }

        /// <summary>
        /// Gets the padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.
        /// </summary>
        public Thickness MaskPadding { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageMaskSurface"/> class.
        /// Constructor
        /// </summary>
        /// <param name="generator">ICompositionMaskGeneratorInternal object</param>
        /// <param name="uri">Uri of the image to be loaded onto the IImageMaskSurface.</param>
        /// <param name="size">Size of the IImageMaskSurface</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">The image's resize, alignment options and blur radius in the allocated space.</param>
        public ImageMaskSurface(ICompositionGeneratorInternal generator, Uri uri, Size size, Thickness padding, ImageSurfaceOptions options)
        {
            Guard.IsNotNull(generator, nameof(generator));

            _generator = generator;
            _surfaceLock = new object();

            // Create the Surface of the IImageMaskSurface
            _surface = _generator.CreateDrawingSurface(_surfaceLock, size);
            Size = _surface?.Size ?? new Size(0, 0);
            _uri = uri;
            _raiseLoadCompletedEvent = _uri != null;
            _canvasBitmap = null;
            MaskPadding = padding;

            // Set the image options
            Options = options;

            // Subscribe to DeviceReplaced event
            _generator.DeviceReplaced += OnDeviceReplaced;
            Status = ImageSurfaceLoadStatus.None;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageMaskSurface"/> class.
        /// Constructor
        /// </summary>
        /// <param name="generator">ICompositionMaskGeneratorInternal object</param>
        /// <param name="surfaceBitmap">The CanvasBitmap whose alpha values will be used to create the Mask.</param>
        /// <param name="size">Size of the IImageMaskSurface</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">The image's resize, alignment options and blur radius in the allocated space.</param>
        public ImageMaskSurface(ICompositionGeneratorInternal generator, CanvasBitmap surfaceBitmap, Size size, Thickness padding, ImageSurfaceOptions options)
        {
            Guard.IsNotNull(generator, nameof(generator));

            _generator = generator;
            _surfaceLock = new object();

            // Create the Surface of the IImageMaskSurface
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

            // Set the mask padding
            MaskPadding = padding;

            // Set the image options
            Options = options;

            // Subscribe to DeviceReplaced event
            _generator.DeviceReplaced += OnDeviceReplaced;
        }

        /// <summary>
        /// Redraws the IImageMaskSurface
        /// </summary>
        public void Redraw()
        {
            // Reload the IImageMaskSurface
            RedrawSurface();
        }

        /// <summary>
        /// Redraws the IImageMaskSurface with the given image options
        /// </summary>
        /// <param name="options">The image's resize, alignment options and blur radius in the allocated space.</param>
        public void Redraw(ImageSurfaceOptions options)
        {
            // Set the image options
            Options = options;

            // Redraw the IImageMaskSurface
            RedrawSurface();
        }

        /// <summary>
        /// Redraws the IImageMaskSurface using the alpha values of image in the given imageSurface.
        /// </summary>
        /// <param name="imageSurface">IImageSurface whose image is to be loaded on the surface.</param>
        public void Redraw(IImageSurface imageSurface)
        {
            if (imageSurface != null)
            {
                Redraw(imageSurface.SurfaceBitmap, imageSurface.Size, MaskPadding, imageSurface.Options);
            }
            else
            {
                // Draw an empty surface
                Redraw(surfaceBitmap: null, Size, MaskPadding, Options);
            }
        }

        /// <summary>
        /// Redraws the IImageMaskSurface (using the alpha values of image in the given imageSurface).
        /// </summary>
        /// <param name="imageSurface">IImageSurface whose image is to be loaded on the surface.</param>
        /// <param name="options">Describes the image's resize, alignment and blur radius options in the allocated space.</param>
        public void Redraw(IImageSurface imageSurface, ImageSurfaceOptions options)
        {
            if (imageSurface != null)
            {
                Redraw(imageSurface.SurfaceBitmap, imageSurface.Size, MaskPadding, options);
            }
            else
            {
                // Draw an empty surface
                Redraw(surfaceBitmap: null, Size, MaskPadding, options);
            }
        }

        /// <summary>
        /// Resizes and redraws the IImageMaskSurface (using the alpha values of image in the given imageSurface).
        /// </summary>
        /// <param name="imageSurface">IImageSurface whose image is to be loaded on the surface.</param>
        /// <param name="size">New size of the IImageMaskSurface.</param>
        /// <param name="options">Describes the image's resize, alignment and blur radius options in the allocated space.</param>
        public void Redraw(IImageSurface imageSurface, Size size, ImageSurfaceOptions options)
        {
            Redraw(imageSurface?.SurfaceBitmap, size, MaskPadding, options);
        }

        /// <summary>
        /// Redraws the IImageMaskSurface using the alpha values of the image in the given IImageSurface with the given padding.
        /// </summary>
        /// <param name="imageSurface">ImageSurface whose image's alpha values are to be used to create the mask.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        public void Redraw(IImageSurface imageSurface, Thickness padding)
        {
            if (imageSurface != null)
            {
                Redraw(imageSurface.SurfaceBitmap, imageSurface.Size, padding, Options);
            }
            else
            {
                // Draw an empty surface
                Redraw(surfaceBitmap: null, Size, padding, Options);
            }
        }

        /// <summary>
        /// Redraws the IImageMaskSurface using the alpha values of the image in the given IImageSurface with the given padding
        /// using the given options.
        /// </summary>
        /// <param name="imageSurface">ImageSurface whose image's alpha values are to be used to create the mask.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">Describes the image's resize, alignment and blur radius options in the allocated space.</param>
        public void Redraw(IImageSurface imageSurface, Thickness padding, ImageSurfaceOptions options)
        {
            if (imageSurface != null)
            {
                Redraw(imageSurface.SurfaceBitmap, imageSurface.Size, padding, options);
            }
            else
            {
                // Draw an empty surface
                Redraw(surfaceBitmap: null, Size, padding, options);
            }
        }

        /// <summary>
        /// Resizes and redraws the IImageMaskSurface using the alpha values of the image in the given IImageSurface
        /// with the given padding and options.
        /// </summary>
        /// <param name="imageSurface">ImageSurface whose image's alpha values are to be used to create the mask.</param>
        /// <param name="size">New size of the IImageMaskSurface.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">Describes the image's resize, alignment and blur radius options in the allocated space.</param>
        public void Redraw(IImageSurface imageSurface, Size size, Thickness padding, ImageSurfaceOptions options)
        {
            Redraw(imageSurface?.SurfaceBitmap, size, padding, options);
        }

        /// <summary>
        /// Redraws the IImageMaskSurface using the alpha values of the image in the given IImageMaskSurface.
        /// </summary>
        /// <param name="imageMaskSurface">IImageMaskSurface whose image's alpha values are to be used to create the mask.</param>
        public void Redraw(IImageMaskSurface imageMaskSurface)
        {
            if (imageMaskSurface != null)
            {
                Redraw(imageMaskSurface.SurfaceBitmap, imageMaskSurface.Size, imageMaskSurface.MaskPadding, imageMaskSurface.Options);
            }
            else
            {
                // Draw an empty surface
                Redraw(surfaceBitmap: null, Size, MaskPadding, Options);
            }
        }

        /// <summary>
        /// Redraws the IImageMaskSurface using the alpha values of the image in the given IImageMaskSurface with the given padding
        /// and options.
        /// </summary>
        /// <param name="imageMaskSurface">IImageMaskSurface whose image's alpha values are to be used to create the mask.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        public void Redraw(IImageMaskSurface imageMaskSurface, Thickness padding)
        {
            if (imageMaskSurface != null)
            {
                Redraw(imageMaskSurface.SurfaceBitmap, imageMaskSurface.Size, padding, imageMaskSurface.Options);
            }
            else
            {
                // Draw an empty surface
                Redraw(surfaceBitmap: null, Size, padding, Options);
            }
        }

        /// <summary>
        /// Redraws the IImageMaskSurface using the alpha values of the image in the given IImageMaskSurface with the given options.
        /// </summary>
        /// <param name="imageMaskSurface">IImageMaskSurface whose image's alpha values are to be used to create the mask.</param>
        /// <param name="options">Describes the image's resize, alignment and blur radius options in the allocated space.</param>
        public void Redraw(IImageMaskSurface imageMaskSurface, ImageSurfaceOptions options)
        {
            if (imageMaskSurface != null)
            {
                Redraw(imageMaskSurface.SurfaceBitmap, imageMaskSurface.Size, imageMaskSurface.MaskPadding, options);
            }
            else
            {
                // Draw an empty surface
                Redraw(surfaceBitmap: null, Size, MaskPadding, options);
            }
        }

        /// <summary>
        /// Redraws the IImageMaskSurface using the alpha values of the image in the given IImageMaskSurface using the given padding
        /// and options.
        /// </summary>
        /// <param name="imageMaskSurface">IImageMaskSurface whose image's alpha values are to be used to create the mask.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">Describes the image's resize, alignment and blur radius options in the allocated space.</param>
        public void Redraw(IImageMaskSurface imageMaskSurface, Thickness padding, ImageSurfaceOptions options)
        {
            if (imageMaskSurface != null)
            {
                Redraw(imageMaskSurface.SurfaceBitmap, imageMaskSurface.Size, padding, options);
            }
            else
            {
                // Draw an empty surface
                Redraw(surfaceBitmap: null, Size, padding, options);
            }
        }

        /// <summary>
        /// Resizes and redraws the IImageMaskSurface using the alpha values of the image in the given IImageMaskSurface
        /// with the given padding and options.
        /// </summary>
        /// <param name="imageMaskSurface">IImageMaskSurface whose image's alpha values are to be used to create the mask.</param>
        /// <param name="size">New size of the IImageMaskSurface.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">Describes the image's resize, alignment and blur radius options in the allocated space.</param>
        public void Redraw(IImageMaskSurface imageMaskSurface, Size size, Thickness padding, ImageSurfaceOptions options)
        {
            Redraw(imageMaskSurface?.SurfaceBitmap, size, padding, options);
        }

        /// <summary>
        /// Redraws the IImageMaskSurface using the given CanvasBitmap's alpha values.
        /// </summary>
        /// <param name="surfaceBitmap">Image whose alpha values are to be used to create the mask.</param>
        public void Redraw(CanvasBitmap surfaceBitmap)
        {
            Redraw(surfaceBitmap, Size, MaskPadding, Options);
        }

        /// <summary>
        /// Redraws the IImageMaskSurface using the given CanvasBitmap's alpha values with the given padding.
        /// </summary>
        /// <param name="surfaceBitmap">Image whose alpha values are to be used to create the mask.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        public void Redraw(CanvasBitmap surfaceBitmap, Thickness padding)
        {
            Redraw(surfaceBitmap, Size, padding, Options);
        }

        /// <summary>
        /// Redraws the IImageMaskSurface using the given CanvasBitmap's alpha values and the given options.
        /// </summary>
        /// <param name="surfaceBitmap">Image whose alpha values are to be used to create the mask.</param>
        /// <param name="options">The image's resize, alignment options and blur radius in the allocated space.</param>
        public void Redraw(CanvasBitmap surfaceBitmap, ImageSurfaceOptions options)
        {
            Redraw(surfaceBitmap, Size, MaskPadding, options);
        }

        /// <summary>
        /// Resizes and redraws the IImageMaskSurface using the given CanvasBitmap's alpha values and the given options.
        /// </summary>
        /// <param name="surfaceBitmap">Image whose alpha values are to be used to create the mask.</param>
        /// <param name="size">New size of the IImageMaskSurface.</param>
        /// <param name="options">The image's resize, alignment options and blur radius in the allocated space.</param>
        public void Redraw(CanvasBitmap surfaceBitmap, Size size, ImageSurfaceOptions options)
        {
            Redraw(surfaceBitmap, size, MaskPadding, options);
        }

        /// <summary>
        /// Redraws the IImageMaskSurface using the given CanvasBitmap's alpha values with the given padding
        /// using the given options.
        /// </summary>
        /// <param name="surfaceBitmap">Image whose alpha values are to be used to create the mask.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">The image's resize, alignment options and blur radius in the allocated space.</param>
        public void Redraw(CanvasBitmap surfaceBitmap, Thickness padding, ImageSurfaceOptions options)
        {
            Redraw(surfaceBitmap, Size, padding, options);
        }

        /// <summary>
        /// Resizes and redraws the IImageMaskSurface using the given CanvasBitmap's alpha values with the given padding
        /// using the given options.
        /// </summary>
        /// <param name="surfaceBitmap">Image whose alpha values are to be used to create the mask.</param>
        /// <param name="size">New size of the IImageMaskSurface.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">The image's resize, alignment and blur radius options in the allocated space.</param>
        public void Redraw(CanvasBitmap surfaceBitmap, Size size, Thickness padding, ImageSurfaceOptions options)
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
                    // No need to copy again if _canvasBitmap and surfaceBitmap are same
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

            // Resize if required
            if (Size != size)
            {
                // resize the IImageMaskSurface
                _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

                // Set the size
                Size = _surface?.Size ?? new Size(0, 0);
            }

            // Set the mask padding
            MaskPadding = padding;

            // Redraw the IImageMaskSurface
            RedrawSurface();
        }

        /// <summary>
        /// Redraws the IImageMaskSurface by loading image from the new Uri
        /// </summary>
        /// <param name="uri">Uri of the image to be loaded on to the image surface.</param>
        /// <returns>Task</returns>
        public Task RedrawAsync(Uri uri)
        {
            return RedrawAsync(uri, Size, MaskPadding, Options);
        }

        /// <summary>
        /// Redraws the IImageMaskSurface by loading image from the new Uri and image options
        /// </summary>
        /// <param name="uri">Uri of the image to be loaded on to the image surface.</param>
        /// <param name="options">The image's resize, alignment and blur radius options in the allocated space.</param>
        /// <returns>Task</returns>
        public Task RedrawAsync(Uri uri, ImageSurfaceOptions options)
        {
            return RedrawAsync(uri, Size, MaskPadding, options);
        }

        /// <summary>
        /// Redraws the IImageMaskSurface by loading image from the new Uri and image options
        /// </summary>
        /// <param name="uri">Uri of the image to be loaded on to the image surface.</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">The image's resize, alignment and blur radius options in the allocated space.</param>
        /// <returns>Task</returns>
        public Task RedrawAsync(Uri uri, Thickness padding, ImageSurfaceOptions options)
        {
            return RedrawAsync(uri, Size, padding, options);
        }

        /// <summary>
        /// Resizes the IImageMaskSurface with the given size and redraws the IImageMaskSurface by loading
        /// image from the new Uri.
        /// </summary>
        /// <param name="uri">Uri of the image to be loaded onto the IImageMaskSurface.</param>
        /// <param name="size">New size of the IImageMaskSurface</param>
        /// <param name="options">The image's resize and alignment options in the allocated space.</param>
        /// <returns>Task</returns>
        public Task RedrawAsync(Uri uri, Size size, ImageSurfaceOptions options)
        {
            return RedrawAsync(uri, size, MaskPadding, options);
        }

        /// <summary>
        /// Resizes the IImageMaskSurface with the given size and loads the image from the new Uri and uses its
        /// alpha values to redraw the IImageMaskSurface with the given padding using the given options.
        /// </summary>
        /// <param name="uri">Uri of the image to be loaded onto the IImageMaskSurface.</param>
        /// <param name="size">New size of the IImageMaskSurface</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">The image's resize, alignment and blur radius options in the allocated space.</param>
        /// <returns>Task</returns>
        public async Task RedrawAsync(Uri uri, Size size, Thickness padding, ImageSurfaceOptions options)
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

            // Set the mask padding
            MaskPadding = padding;

            // Set the image options
            Options = options;

            // Resize the surface only if AutoResize option is disabled
            if (Size != size)
            {
                // resize the IImageMaskSurface
                _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

                // Set the size
                Size = _surface?.Size ?? new Size(0, 0);
            }

            // Set the new Uri of the image to be loaded
            _uri = uri;

            // Reload the IImageMaskSurface
            await RedrawSurfaceAsync();
        }

        /// <summary>
        /// Resizes the IImageMaskSurface to the new size.
        /// </summary>
        /// <param name="size">New size of the IImageMaskSurface</param>
        public void Resize(Size size)
        {
            Resize(size, MaskPadding, Options);
        }

        /// <summary>
        /// Resizes the IImageMaskSurface to the new size using the given options.
        /// </summary>
        /// <param name="size">New size of the IImageMaskSurface</param>
        /// <param name="options">The image's resize, alignment and blur radius options in the allocated space.</param>
        public void Resize(Size size, ImageSurfaceOptions options)
        {
            Resize(size, MaskPadding, options);
        }

        /// <summary>
        /// Resizes the IImageMaskSurface to the new size and redraws it with the given padding using the given options.
        /// </summary>
        /// <param name="size">New size of the IImageMaskSurface</param>
        /// <param name="padding">The padding between the IImageMaskSurface outer bounds and the bounds of the area where
        /// the mask, created from the loaded image's alpha values, should be rendered.</param>
        /// <param name="options">The image's resize, alignment and blur radius options in the allocated space.</param>
        public void Resize(Size size, Thickness padding, ImageSurfaceOptions options)
        {
            // Set the mask padding
            MaskPadding = padding;

            // Set the image options
            Options = options;

            // Resize the surface only if AutoResize option is disabled
            if (Size != size)
            {
                // resize the IImageMaskSurface
                _generator.ResizeDrawingSurface(_surfaceLock, _surface, size);

                // Set the size
                Size = _surface?.Size ?? new Size(0, 0);
            }

            // Redraw the IImageMaskSurface
            RedrawSurface();
        }

        /// <summary>
        /// Disposes the resources used by the IImageMaskSurface
        /// </summary>
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
        /// Redraws the IImageMaskSurface asynchronously by loading the image from the Uri.
        /// </summary>
        /// <returns>Task</returns>
        internal Task RedrawAsync()
        {
            // Reload the IImageMaskSurface
            return RedrawSurfaceAsync();
        }

        /// <summary>
        /// Handles the DeviceReplaced event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">object</param>
        private async void OnDeviceReplaced(object sender, object e)
        {
            // Recreate the ImageSurface
            _surface = _generator.CreateDrawingSurface(_surfaceLock, Size);

            // Reload the IImageMaskSurface
            await RedrawSurfaceAsync();
        }

        /// <summary>
        /// Helper class to redraw the IImageMaskSurface synchronously
        /// </summary>
        private void RedrawSurface()
        {
            // Resize the surface image
            _generator.RedrawImageMaskSurface(_surfaceLock, _surface, MaskPadding, Options, _canvasBitmap);

            Status = _canvasBitmap != null ? ImageSurfaceLoadStatus.Success : ImageSurfaceLoadStatus.Error;

            if (_canvasBitmap != null)
            {
                DecodedPhysicalSize = new Size(_canvasBitmap.SizeInPixels.Width, _canvasBitmap.SizeInPixels.Height);
                DecodedSize = _canvasBitmap.Size;
            }
        }

        /// <summary>
        /// Helper class to redraw the IImageMaskSurface asynchronously
        /// </summary>
        /// <returns>Task</returns>
        private async Task RedrawSurfaceAsync()
        {
            // Cache the canvasBitmap to avoid reloading of the same image during Resize/Redraw operations
            _canvasBitmap = await _generator.RedrawImageMaskSurfaceAsync(_surfaceLock, _surface, _uri, MaskPadding, Options, _canvasBitmap);

            Status = _canvasBitmap != null ? ImageSurfaceLoadStatus.Success : ImageSurfaceLoadStatus.Error;

            if (_canvasBitmap != null)
            {
                DecodedPhysicalSize = new Size(_canvasBitmap.SizeInPixels.Width, _canvasBitmap.SizeInPixels.Height);
                DecodedSize = _canvasBitmap.Size;
            }

            if (_raiseLoadCompletedEvent)
            {
                LoadCompleted?.Invoke(this, Status);
                _raiseLoadCompletedEvent = false;
            }
        }
    }
}
