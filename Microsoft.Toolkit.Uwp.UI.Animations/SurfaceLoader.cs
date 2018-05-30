// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Composition;
using Windows.Foundation;
using Windows.Graphics.DirectX;
using Windows.UI;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A delegate for load time effects.
    /// </summary>
    /// <param name="bitmap">The bitmap.</param>
    /// <param name="device">The device.</param>
    /// <param name="sizeTarget">The size target.</param>
    /// <returns>A CompositeDrawingSurface</returns>
    public delegate CompositionDrawingSurface LoadTimeEffectHandler(CanvasBitmap bitmap, CompositionGraphicsDevice device, Size sizeTarget);

    /// <summary>
    /// The SurfaceLoader is responsible to loading images into Composition Objects.
    /// </summary>
    public class SurfaceLoader
    {
        /// <summary>
        /// A flag to store the intialized state.
        /// </summary>
        private static bool _intialized;

        /// <summary>
        /// The compositor
        /// </summary>
        private static Compositor _compositor;

        /// <summary>
        /// The canvas device
        /// </summary>
        private static CanvasDevice _canvasDevice;

        /// <summary>
        /// The composition graphic device to determinde which GPU is handling the request.
        /// </summary>
        private static CompositionGraphicsDevice _compositionDevice;

        /// <summary>
        /// Initializes the specified compositor.
        /// </summary>
        /// <param name="compositor">The compositor.</param>
        public static void Initialize(Compositor compositor)
        {
            if (!_intialized)
            {
                _compositor = compositor;
                _canvasDevice = new CanvasDevice();
                _compositionDevice = CanvasComposition.CreateCompositionGraphicsDevice(_compositor, _canvasDevice);

                _intialized = true;
            }
        }

        /// <summary>
        /// Uninitializes this instance.
        /// </summary>
        public static void Uninitialize()
        {
            _compositor = null;

            if (_compositionDevice != null)
            {
                _compositionDevice.Dispose();
                _compositionDevice = null;
            }

            if (_canvasDevice != null)
            {
                _canvasDevice.Dispose();
                _canvasDevice = null;
            }

            _intialized = false;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInitialized
        {
            get
            {
                return _intialized;
            }
        }

        /// <summary>
        /// Loads an image from the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns><see cref="CompositionDrawingSurface"/></returns>
        public static async Task<CompositionDrawingSurface> LoadFromUri(Uri uri)
        {
            return await LoadFromUri(uri, Size.Empty);
        }

        /// <summary>
        /// Loads an image from URI with a specified size.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="sizeTarget">The size target.</param>
        /// <returns><see cref="CompositionDrawingSurface"/></returns>
        public static async Task<CompositionDrawingSurface> LoadFromUri(Uri uri, Size sizeTarget)
        {
            CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(_canvasDevice, uri);
            Size sizeSource = bitmap.Size;

            if (sizeTarget.IsEmpty)
            {
                sizeTarget = sizeSource;
            }

            CompositionDrawingSurface surface = _compositionDevice.CreateDrawingSurface(
                sizeTarget,
                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                DirectXAlphaMode.Premultiplied);

            using (var ds = CanvasComposition.CreateDrawingSession(surface))
            {
                ds.Clear(Color.FromArgb(0, 0, 0, 0));
                ds.DrawImage(bitmap, new Rect(0, 0, sizeTarget.Width, sizeTarget.Height), new Rect(0, 0, sizeSource.Width, sizeSource.Height));
            }

            return surface;
        }

        /// <summary>
        /// Loads the text on to a <see cref="CompositionDrawingSurface"/>.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="sizeTarget">The size target.</param>
        /// <param name="textFormat">The text format.</param>
        /// <param name="textColor">Color of the text.</param>
        /// <param name="bgColor">Color of the bg.</param>
        /// <returns><see cref="CompositionDrawingSurface"/></returns>
        public static CompositionDrawingSurface LoadText(string text, Size sizeTarget, CanvasTextFormat textFormat, Color textColor, Color bgColor)
        {
            CompositionDrawingSurface surface = _compositionDevice.CreateDrawingSurface(
                sizeTarget,
                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                DirectXAlphaMode.Premultiplied);

            using (var ds = CanvasComposition.CreateDrawingSession(surface))
            {
                ds.Clear(bgColor);
                ds.DrawText(text, new Rect(0, 0, sizeTarget.Width, sizeTarget.Height), textColor, textFormat);
            }

            return surface;
        }

        /// <summary>
        /// Loads an image from URI, with a specified size.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="sizeTarget">The size target.</param>
        /// <param name="loadEffectHandler">The load effect handler callback.</param>
        /// <returns><see cref="CompositionDrawingSurface"/></returns>
        public static async Task<CompositionDrawingSurface> LoadFromUri(Uri uri, Size sizeTarget, LoadTimeEffectHandler loadEffectHandler)
        {
            if (loadEffectHandler != null)
            {
                var bitmap = await CanvasBitmap.LoadAsync(_canvasDevice, uri);
                return loadEffectHandler(bitmap, _compositionDevice, sizeTarget);
            }
            else
            {
                return await LoadFromUri(uri, sizeTarget);
            }
        }
    }
}