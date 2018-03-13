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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Graphics.Canvas;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Manager
{
    internal class ImageAssetManager : IDisposable
    {
        private readonly string _imagesFolder;
        private readonly Dictionary<string, LottieImageAsset> _imageAssets;
        private readonly Dictionary<string, CanvasBitmap> _bitmaps = new Dictionary<string, CanvasBitmap>();

        private IImageAssetDelegate _delegate;

        internal ImageAssetManager(string imagesFolder, IImageAssetDelegate @delegate, Dictionary<string, LottieImageAsset> imageAssets)
        {
            _imagesFolder = imagesFolder;
            if (!string.IsNullOrEmpty(imagesFolder) && _imagesFolder[_imagesFolder.Length - 1] != '/')
            {
                _imagesFolder += '/';
            }

            // if (!(callback is UIElement)) // TODO: Makes sense on UWP?
            // {
            //    Debug.WriteLine("LottieDrawable must be inside of a view for images to work.", L.TAG);
            //    this.imageAssets = new Dictionary<string, LottieImageAsset>();
            //    return;
            // }
            _imageAssets = imageAssets;
            Delegate = @delegate;
        }

        internal virtual IImageAssetDelegate Delegate
        {
            set
            {
                lock (this)
                {
                    _delegate = value;
                }
            }
        }

        /// <summary>
        /// Updates the bitmap of the given id with a new one, or removes it if the bitmap is null.
        /// </summary>
        /// <param name="id">The id of the bitmap to be updated.</param>
        /// <param name="bitmap">The new bitmap, or null to remove it.</param>
        /// <returns>If the bitmap parameter is null, returns the previously set bitmap, or else returns the same bitmap send thru the parameter.</returns>
        internal CanvasBitmap UpdateBitmap(string id, CanvasBitmap bitmap)
        {
            lock (this)
            {
                if (bitmap == null)
                {
                    if (_bitmaps.TryGetValue(id, out var removed))
                    {
                        _bitmaps.Remove(id);
                    }

                    return removed;
                }

                _bitmaps.Add(id, bitmap);
                return bitmap;
            }
        }

        internal virtual CanvasBitmap BitmapForId(CanvasDevice device, string id)
        {
            lock (this)
            {
                if (!_bitmaps.TryGetValue(id, out CanvasBitmap bitmap))
                {
                    var imageAsset = _imageAssets[id];
                    if (imageAsset == null)
                    {
                        return null;
                    }

                    if (_delegate != null)
                    {
                        bitmap = _delegate.FetchBitmap(imageAsset);
                        if (bitmap != null)
                        {
                            _bitmaps[id] = bitmap;
                        }

                        return bitmap;
                    }

                    Stream @is;
                    try
                    {
                        if (string.IsNullOrEmpty(_imagesFolder))
                        {
                            throw new InvalidOperationException("You must set an images folder before loading an image." + " Set it with LottieDrawable.ImageAssetsFolder");
                        }

                        @is = File.OpenRead(_imagesFolder + imageAsset.FileName);
                    }
                    catch (IOException e)
                    {
                        Debug.WriteLine($"Unable to open asset. {e}", LottieLog.Tag);
                        return null;
                    }

                    var task = CanvasBitmap.LoadAsync(device, @is.AsRandomAccessStream(), 160).AsTask();
                    task.Wait();
                    bitmap = task.Result;

                    @is.Dispose();

                    _bitmaps[id] = bitmap;
                }

                return bitmap;
            }
        }

        internal virtual void RecycleBitmaps()
        {
            lock (this)
            {
                for (var i = _bitmaps.Count - 1; i >= 0; i--)
                {
                    var entry = _bitmaps.ElementAt(i);
                    entry.Value.Dispose();
                    _bitmaps.Remove(entry.Key);
                }
            }
        }

        private void Dispose(bool disposing)
        {
            RecycleBitmaps();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ImageAssetManager()
        {
            Dispose(false);
        }
    }
}