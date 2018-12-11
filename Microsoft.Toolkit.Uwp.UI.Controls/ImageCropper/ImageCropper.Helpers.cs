// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="ImageCropper"/> control allows user to crop image freely.
    /// </summary>
    public partial class ImageCropper
    {
        /// <summary>
        /// Gets the cropped image.
        /// </summary>
        /// <param name="writeableBitmap">The source image.</param>
        /// <param name="croppedRect">The cropped area.</param>
        /// <returns>CroppedBitmap</returns>
        internal static async Task<WriteableBitmap> GetCroppedBitmapAsync(
            WriteableBitmap writeableBitmap,
            Rect croppedRect)
        {
            var x = (uint)Math.Floor(croppedRect.X);
            var y = (uint)Math.Floor(croppedRect.Y);
            var width = (uint)Math.Floor(croppedRect.Width);
            var height = (uint)Math.Floor(croppedRect.Height);
            WriteableBitmap croppedBitmap;
            var sourceStream = writeableBitmap.PixelBuffer.AsStream();
            var buffer = new byte[sourceStream.Length];
            await sourceStream.ReadAsync(buffer, 0, buffer.Length);
            using (var memoryRandom = new InMemoryRandomAccessStream())
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, memoryRandom);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, (uint)writeableBitmap.PixelWidth, (uint)writeableBitmap.PixelHeight, 96.0, 96.0, buffer);
                encoder.BitmapTransform.Bounds = new BitmapBounds
                {
                    X = x,
                    Y = y,
                    Height = height,
                    Width = width
                };
                await encoder.FlushAsync();
                croppedBitmap = new WriteableBitmap(
                    (int)encoder.BitmapTransform.Bounds.Width,
                    (int)encoder.BitmapTransform.Bounds.Height);
                croppedBitmap.SetSource(memoryRandom);
            }

            return croppedBitmap;
        }

        /// <summary>
        /// Save the cropped image to a file.
        /// </summary>
        /// <param name="writeableBitmap">The source image.</param>
        /// <param name="imageFile">The target file.</param>
        /// <param name="encoderId">The encoderId of BitmapEncoder</param>
        /// <returns>Task</returns>
        internal static async Task RenderToFile(WriteableBitmap writeableBitmap, StorageFile imageFile, Guid encoderId)
        {
            using (var stream = await imageFile.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.None))
            {
                var encoder = await BitmapEncoder.CreateAsync(encoderId, stream);
                var pixelStream = writeableBitmap.PixelBuffer.AsStream();
                var pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);
                encoder.SetPixelData(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Premultiplied,
                    (uint)writeableBitmap.PixelWidth,
                    (uint)writeableBitmap.PixelHeight,
                    96.0,
                    96.0,
                    pixels);
                await encoder.FlushAsync();
            }
        }

        /// <summary>
        /// Gets the closest point in the rectangle to a given point.
        /// </summary>
        /// <param name="targetRect">The rectangle.</param>
        /// <param name="point">The test point.</param>
        /// <returns>A point within a rectangle.</returns>
        internal static Point GetSafePoint(Rect targetRect, Point point)
        {
            var safePoint = new Point(point.X, point.Y);
            if (safePoint.X < targetRect.X)
            {
                safePoint.X = targetRect.X;
            }

            if (safePoint.X > targetRect.X + targetRect.Width)
            {
                safePoint.X = targetRect.X + targetRect.Width;
            }

            if (safePoint.Y < targetRect.Y)
            {
                safePoint.Y = targetRect.Y;
            }

            if (safePoint.Y > targetRect.Y + targetRect.Height)
            {
                safePoint.Y = targetRect.Y + targetRect.Height;
            }

            return safePoint;
        }

        /// <summary>
        /// Test whether the point is in the rectangle.
        /// Similar to the Rect.Contains method, this method adds redundancy.
        /// </summary>
        /// <param name="targetRect">the rectangle.</param>
        /// <param name="point">The test point.</param>
        /// <returns>bool</returns>
        internal static bool IsSafePoint(Rect targetRect, Point point)
        {
            if (point.X - targetRect.X < -0.001)
            {
                return false;
            }

            if (point.X - (targetRect.X + targetRect.Width) > 0.001)
            {
                return false;
            }

            if (point.Y - targetRect.Y < -0.001)
            {
                return false;
            }

            if (point.Y - (targetRect.Y + targetRect.Height) > 0.001)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether a rectangle satisfies the minimum size limit.
        /// </summary>
        /// <param name="startPoint">The point on the upper left corner.</param>
        /// <param name="endPoint">The point on the lower right corner.</param>
        /// <param name="minSize">The minimum size.</param>
        /// <returns>bool</returns>
        internal static bool IsSafeRect(Point startPoint, Point endPoint, Size minSize)
        {
            var checkPoint = new Point(startPoint.X + minSize.Width, startPoint.Y + minSize.Height);
            return checkPoint.X - endPoint.X < 0.001
                   && checkPoint.Y - endPoint.Y < 0.001;
        }

        /// <summary>
        /// Gets a rectangle with a minimum size limit.
        /// </summary>
        /// <param name="startPoint">The point on the upper left corner.</param>
        /// <param name="endPoint">The point on the lower right corner.</param>
        /// <param name="minSize">The minimum size.</param>
        /// <param name="positionTag">The control point.</param>
        /// <returns>The right rectangle.</returns>
        internal static Rect GetSafeRect(Point startPoint, Point endPoint, Size minSize, PositionTag positionTag)
        {
            var checkPoint = new Point(startPoint.X + minSize.Width, startPoint.Y + minSize.Height);
            switch (positionTag)
            {
                case PositionTag.Top:
                    if (checkPoint.Y > endPoint.Y)
                    {
                        startPoint.Y = endPoint.Y - minSize.Height;
                    }

                    break;
                case PositionTag.Bottom:
                    if (checkPoint.Y > endPoint.Y)
                    {
                        endPoint.Y = startPoint.Y + minSize.Height;
                    }

                    break;
                case PositionTag.Left:
                    if (checkPoint.X > endPoint.X)
                    {
                        startPoint.X = endPoint.X - minSize.Width;
                    }

                    break;
                case PositionTag.Right:
                    if (checkPoint.X > endPoint.X)
                    {
                        endPoint.X = startPoint.X + minSize.Width;
                    }

                    break;
                case PositionTag.UpperLeft:
                    if (checkPoint.X > endPoint.X)
                    {
                        startPoint.X = endPoint.X - minSize.Width;
                    }

                    if (checkPoint.Y > endPoint.Y)
                    {
                        startPoint.Y = endPoint.Y - minSize.Height;
                    }

                    break;
                case PositionTag.UpperRight:
                    if (checkPoint.X > endPoint.X)
                    {
                        endPoint.X = startPoint.X + minSize.Width;
                    }

                    if (checkPoint.Y > endPoint.Y)
                    {
                        startPoint.Y = endPoint.Y - minSize.Height;
                    }

                    break;
                case PositionTag.LowerLeft:
                    if (checkPoint.X > endPoint.X)
                    {
                        startPoint.X = endPoint.X - minSize.Width;
                    }

                    if (checkPoint.Y > endPoint.Y)
                    {
                        endPoint.Y = startPoint.Y + minSize.Height;
                    }

                    break;
                case PositionTag.LowerRight:
                    if (checkPoint.X > endPoint.X)
                    {
                        endPoint.X = startPoint.X + minSize.Width;
                    }

                    if (checkPoint.Y > endPoint.Y)
                    {
                        endPoint.Y = startPoint.Y + minSize.Height;
                    }

                    break;
            }

            return new Rect(startPoint, endPoint);
        }

        /// <summary>
        /// Gets the maximum rectangle embedded in the rectangle by a given aspect ratio.
        /// </summary>
        /// <param name="targetRect">The rectangle.</param>
        /// <param name="aspectRatio">The aspect ratio.</param>
        /// <returns>The right rectangle.</returns>
        internal static Rect GetUniformRect(Rect targetRect, double aspectRatio)
        {
            var ratio = targetRect.Width / targetRect.Height;
            var cx = targetRect.X + (targetRect.Width / 2);
            var cy = targetRect.Y + (targetRect.Height / 2);
            double width, height;
            if (aspectRatio > ratio)
            {
                width = targetRect.Width;
                height = width / aspectRatio;
            }
            else
            {
                height = targetRect.Height;
                width = height * aspectRatio;
            }

            return new Rect(cx - (width / 2f), cy - (height / 2f), width, height);
        }
    }
}
