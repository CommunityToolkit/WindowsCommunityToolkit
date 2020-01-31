// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using Microsoft.Toolkit.Uwp.UI.Media.Extensions;
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
        /// <param name="cacheMode">Indicates the cache option to use to load the image</param>
        /// <returns>A <see cref="Task{T}"/> that returns the loaded <see cref="CompositionSurfaceBrush"/> instance</returns>
        public static async Task<CompositionSurfaceBrush> LoadImageAsync(Uri uri, DpiMode dpiMode, CacheMode cacheMode = CacheMode.Default)
        {
            var compositor = Window.Current.Compositor;

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
                    brush = await LoadSurfaceBrushAsync(sharedDevice, compositor, uri, dpiMode);
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

        /// <summary>
        /// Loads a <see cref="CompositionSurfaceBrush"/> from the input <see cref="System.Uri"/>, and prepares it to be used in a tile effect
        /// </summary>
        /// <param name="canvasDevice">The device to use to process the Win2D image</param>
        /// <param name="compositor">The compositor instance to use to create the final brush</param>
        /// <param name="uri">The path to the image to load</param>
        /// <param name="dpiMode">Indicates the desired DPI mode to use when loading the image</param>
        /// <returns>A <see cref="Task{T}"/> that returns the loaded <see cref="CompositionSurfaceBrush"/> instance</returns>
        private static async Task<CompositionSurfaceBrush> LoadSurfaceBrushAsync(
            CanvasDevice canvasDevice,
            Compositor compositor,
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
                    case DpiMode.UseSourceDpi: bitmap = await CanvasBitmap.LoadAsync(canvasDevice, uri); break;
                    case DpiMode.Default96Dpi: bitmap = await CanvasBitmap.LoadAsync(canvasDevice, uri, 96); break;
                    case DpiMode.DisplayDpi: bitmap = await CanvasBitmap.LoadAsync(canvasDevice, uri, dpi); break;
                    case DpiMode.DisplayDpiWith96AsLowerBound: bitmap = await CanvasBitmap.LoadAsync(canvasDevice, uri, dpi >= 96 ? dpi : 96); break;
                    default: throw new ArgumentOutOfRangeException(nameof(dpiMode), dpiMode, $"Invalid DPI mode: {dpiMode}");
                }

                // Get the device and the target surface
                using (var graphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(compositor, canvasDevice))
                {
                    var drawingSurface = graphicsDevice.CreateDrawingSurface(default, DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);

                    // Calculate the surface size
                    Size
                        size = bitmap.Size,
                        sizeInPixels = new Size(bitmap.SizeInPixels.Width, bitmap.SizeInPixels.Height);

                    CanvasComposition.Resize(drawingSurface, sizeInPixels);

                    // Create a drawing session for the target surface
                    using (var drawingSession = CanvasComposition.CreateDrawingSession(drawingSurface, new Rect(0, 0, sizeInPixels.Width, sizeInPixels.Height), dpi))
                    {
                        // Fill the target surface
                        drawingSession.Clear(Color.FromArgb(0, 0, 0, 0));
                        drawingSession.DrawImage(bitmap, new Rect(0, 0, size.Width, size.Height), new Rect(0, 0, size.Width, size.Height));
                        drawingSession.EffectTileSize = new BitmapSize { Width = (uint)size.Width, Height = (uint)size.Height };
                    }

                    // Setup the effect brush to use
                    var surfaceBrush = compositor.CreateSurfaceBrush(drawingSurface);
                    surfaceBrush.Stretch = CompositionStretch.None;

                    double pixels = displayInformation.RawPixelsPerViewPixel;

                    // Adjust the scale if the DPI scaling is greater than 100%
                    if (pixels > 1)
                    {
                        surfaceBrush.Scale = new Vector2((float)(1 / pixels));
                        surfaceBrush.BitmapInterpolationMode = CompositionBitmapInterpolationMode.NearestNeighbor;
                    }

                    return surfaceBrush;
                }
            }
            finally
            {
                bitmap?.Dispose();
            }
        }
    }
}
