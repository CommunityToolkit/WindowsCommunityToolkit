// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Composition;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Media.Extensions.System;
using Microsoft.Toolkit.Uwp.UI.Media.Extensions.System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Media.Helpers.Cache;
using Windows.Foundation;
using Windows.Graphics.DirectX;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media.Helpers
{
    /// <summary>
    /// A helper <see langword="class"/> that loads Win2D images and manages an internal cache of <see cref="CompositionBrush"/> instances with the loaded images
    /// </summary>
    public static class Win2DImageHelper
    {
        /// <summary>
        /// Gets the maximum time to wait for the Win2D device to be restored in case of initial failure
        /// </summary>
        private const int DeviceLostRecoveryThreshold = 1000;

        /// <summary>
        /// Synchronization mutex to access the cache and load Win2D images concurrently
        /// </summary>
        private static readonly AsyncMutex Win2DMutex = new AsyncMutex();

        /// <summary>
        /// Gets the local cache mapping for previously loaded Win2D images
        /// </summary>
        private static readonly CompositionObjectCache<Uri, CompositionSurfaceBrush> Cache = new CompositionObjectCache<Uri, CompositionSurfaceBrush>();

        /// <summary>
        /// Loads a <see cref="CompositionSurfaceBrush"/> instance with the target image from the shared <see cref="CanvasDevice"/> instance
        /// </summary>
        /// <param name="uri">The path to the image to load</param>
        /// <param name="dpiMode">Indicates the desired DPI mode to use when loading the image</param>
        /// <param name="cache">Indicates the cache option to use to load the image</param>
        /// <returns>A <see cref="Task{T}"/> that returns the loaded <see cref="CompositionSurfaceBrush"/> instance</returns>
        public static Task<CompositionSurfaceBrush> LoadImageAsync(Uri uri, DpiMode dpiMode, CacheMode cache = CacheMode.Default)
        {
            return LoadImageAsync(Window.Current.Compositor, uri, dpiMode, cache);
        }

        /// <summary>
        /// Loads a <see cref="CompositionSurfaceBrush"/> instance with the target image
        /// </summary>
        /// <param name="canvas">The <see cref="CanvasControl"/> to use to load the target image</param>
        /// <param name="uri">The path to the image to load</param>
        /// <param name="dpiMode">Indicates the desired DPI mode to use when loading the image</param>
        /// <param name="cacheMode">Indicates the cache option to use to load the image</param>
        /// <returns>A <see cref="Task{T}"/> that returns the loaded <see cref="CompositionSurfaceBrush"/> instance</returns>
        public static Task<CompositionSurfaceBrush> LoadImageAsync(this CanvasControl canvas, Uri uri, DpiMode dpiMode, CacheMode cacheMode = CacheMode.Default)
        {
            return LoadImageAsync(Window.Current.Compositor, canvas, uri, dpiMode, cacheMode);
        }

        /// <summary>
        /// Loads a <see cref="CompositionSurfaceBrush"/> from the input <see cref="System.Uri"/>, and prepares it to be used in a tile effect
        /// </summary>
        /// <param name="creator">The resource creator to use to load the image bitmap (it can be the same <see cref="CanvasDevice"/> used later)</param>
        /// <param name="compositor">The compositor instance to use to create the final brush</param>
        /// <param name="canvasDevice">The device to use to process the Win2D image</param>
        /// <param name="uri">The path to the image to load</param>
        /// <param name="dpiMode">Indicates the desired DPI mode to use when loading the image</param>
        /// <returns>A <see cref="Task{T}"/> that returns the loaded <see cref="CompositionSurfaceBrush"/> instance</returns>
        private static async Task<CompositionSurfaceBrush> LoadSurfaceBrushAsync(
            ICanvasResourceCreator creator,
            Compositor compositor,
            CanvasDevice canvasDevice,
            Uri uri,
            DpiMode dpiMode)
        {
            var displayInformation = DisplayInformation.GetForCurrentView();
            float dpi = displayInformation.LogicalDpi;

            // Explicit try/finally block to emulate the using block from C# 8 on switch assignment
            CanvasBitmap bitmap = null;
            try
            {
                // Load the bitmap with the appropriate settings
                switch (dpiMode)
                {
                    case DpiMode.UseSourceDpi: bitmap = await CanvasBitmap.LoadAsync(creator, uri); break;
                    case DpiMode.Default96Dpi: bitmap = await CanvasBitmap.LoadAsync(creator, uri, 96); break;
                    case DpiMode.DisplayDpi: bitmap = await CanvasBitmap.LoadAsync(creator, uri, dpi); break;
                    case DpiMode.DisplayDpiWith96AsLowerBound: bitmap = await CanvasBitmap.LoadAsync(creator, uri, dpi >= 96 ? dpi : 96); break;
                    default: throw new ArgumentOutOfRangeException(nameof(dpiMode), dpiMode, $"Invalid DPI mode: {dpiMode}");
                }

                // Get the device and the target surface
                var device = CanvasComposition.CreateCompositionGraphicsDevice(compositor, canvasDevice);
                var surface = device.CreateDrawingSurface(default, DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);

                // Calculate the surface size
                Size
                    size = bitmap.Size,
                    sizeInPixels = new Size(bitmap.SizeInPixels.Width, bitmap.SizeInPixels.Height);
                CanvasComposition.Resize(surface, sizeInPixels);

                // Create a drawing session for the target surface
                using (var session = CanvasComposition.CreateDrawingSession(surface, new Rect(0, 0, sizeInPixels.Width, sizeInPixels.Height), dpi))
                {
                    // Fill the target surface
                    session.Clear(Color.FromArgb(0, 0, 0, 0));
                    session.DrawImage(bitmap, new Rect(0, 0, size.Width, size.Height), new Rect(0, 0, size.Width, size.Height));
                    session.EffectTileSize = new BitmapSize { Width = (uint)size.Width, Height = (uint)size.Height };
                }

                // Setup the effect brush to use
                var brush = surface.Compositor.CreateSurfaceBrush(surface);
                brush.Stretch = CompositionStretch.None;

                double pixels = displayInformation.RawPixelsPerViewPixel;

                // Adjust the scale if the DPI scaling is greater than 100%
                if (pixels > 1)
                {
                    brush.Scale = new Vector2((float)(1 / pixels));
                    brush.BitmapInterpolationMode = CompositionBitmapInterpolationMode.NearestNeighbor;
                }

                return brush;
            }
            finally
            {
                bitmap?.Dispose();
            }
        }

        /// <summary>
        /// Loads a <see cref="CompositionSurfaceBrush"/> instance with the target image
        /// </summary>
        /// <param name="compositor">The compositor to use to render the Win2D image</param>
        /// <param name="canvas">The <see cref="CanvasControl"/> to use to load the target image</param>
        /// <param name="uri">The path to the image to load</param>
        /// <param name="dpiMode">Indicates the desired DPI mode to use when loading the image</param>
        /// <param name="cacheMode">Indicates the cache option to use to load the image</param>
        /// <returns>A <see cref="Task{T}"/> that returns the loaded <see cref="CompositionSurfaceBrush"/> instance</returns>
        internal static async Task<CompositionSurfaceBrush> LoadImageAsync(
            Compositor compositor,
            CanvasControl canvas,
            Uri uri,
            DpiMode dpiMode,
            CacheMode cacheMode)
        {
            var tcs = new TaskCompletionSource<CompositionSurfaceBrush>();

            // Loads an image using the input CanvasDevice instance
            async Task<CompositionSurfaceBrush> LoadImageAsync(bool shouldThrow)
            {
                // Load the image - this will only succeed when there's an available Win2D device
                try
                {
                    return await LoadSurfaceBrushAsync(canvas, compositor, canvas.Device, uri, dpiMode);
                }
                catch when (!shouldThrow)
                {
                    // Win2D error, just ignore and continue
                    return null;
                }
            }

            // Handler to create the Win2D image from the input CanvasControl
            async void Canvas_CreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
            {
                // Cancel previous actions
                args.GetTrackedAction()?.Cancel();

                // Load the image and notify the canvas
                var task = LoadImageAsync(false);
                var action = task.AsAsyncAction();

                try
                {
                    args.TrackAsyncAction(action);

                    var brush = await task;

                    action.Cancel();

                    tcs.TrySetResult(brush);
                }
                catch (COMException)
                {
                    // Somehow another action was still being tracked
                    tcs.TrySetResult(null);
                }
            }

            // Lock the semaphore and check the cache first
            using (await Win2DMutex.LockAsync())
            {
                if (cacheMode == CacheMode.Default &&
                    Cache.TryGetValue(compositor, uri, out var cached))
                {
                    return cached;
                }

                // Load the image
                canvas.CreateResources += Canvas_CreateResources;
                try
                {
                    // This will throw and the canvas will re-initialize the Win2D device if needed
                    await LoadImageAsync(true);
                }
                catch (ArgumentException)
                {
                    // Just ignore here
                }
                catch
                {
                    // Win2D messed up big time (this should never happen)
                    tcs.TrySetResult(null);
                }

                await Task.WhenAny(tcs.Task, Task.Delay(DeviceLostRecoveryThreshold).ContinueWith(t => tcs.TrySetResult(null)));

                canvas.CreateResources -= Canvas_CreateResources;

                var brush = tcs.Task.Result;

                // Cache when needed and return the result
                if (brush != null &&
                    cacheMode != CacheMode.Disabled)
                {
                    Cache.AddOrUpdate(compositor, uri, brush);
                }

                return brush;
            }
        }

        /// <summary>
        /// Loads a <see cref="CompositionSurfaceBrush"/> instance with the target image from the shared <see cref="CanvasDevice"/> instance
        /// </summary>
        /// <param name="compositor">The compositor to use to render the Win2D image</param>
        /// <param name="uri">The path to the image to load</param>
        /// <param name="dpiMode">Indicates the desired DPI mode to use when loading the image</param>
        /// <param name="cacheMode">Indicates the cache option to use to load the image</param>
        /// <returns>A <see cref="Task{T}"/> that returns the loaded <see cref="CompositionSurfaceBrush"/> instance</returns>
        internal static async Task<CompositionSurfaceBrush> LoadImageAsync(
            Compositor compositor,
            Uri uri,
            DpiMode dpiMode,
            CacheMode cacheMode)
        {
            // Lock and check the cache first
            using (await Win2DMutex.LockAsync())
            {
                uri = uri.ToAppxUri();

                if (cacheMode == CacheMode.Default &&
                    Cache.TryGetValue(compositor, uri, out var cached))
                {
                    return cached;
                }

                // Load the image
                CompositionSurfaceBrush brush;
                try
                {
                    // This will throw and the canvas will re-initialize the Win2D device if needed
                    var sharedDevice = CanvasDevice.GetSharedDevice();
                    brush = await LoadSurfaceBrushAsync(sharedDevice, compositor, sharedDevice, uri, dpiMode);
                }
                catch
                {
                    // Device error
                    brush = null;
                }

                // Cache when needed and return the result
                if (brush != null &&
                    cacheMode != CacheMode.Disabled)
                {
                    Cache.AddOrUpdate(compositor, uri, brush);
                }

                return brush;
            }
        }
    }
}
