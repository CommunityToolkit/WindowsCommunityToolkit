﻿// ******************************************************************
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
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Provides methods and tools to cache files in a folder
    /// </summary>
    public class ImageCache : CacheBase<BitmapImage>
    {
        private const string DateAccessedProperty = "System.DateAccessed";

        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static ImageCache _instance;

        private List<string> _extendedPropertyNames = new List<string>();

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static ImageCache Instance => _instance ?? (_instance = new ImageCache());

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageCache"/> class.
        /// </summary>
        public ImageCache()
        {
            _extendedPropertyNames.Add(DateAccessedProperty);
            MaintainContext = true;
        }

        /// <summary>
        /// Cache specific hooks to process items from HTTP response
        /// </summary>
        /// <param name="stream">input stream</param>
        /// <returns>awaitable task</returns>
        protected override async Task<BitmapImage> InitializeTypeAsync(IRandomAccessStream stream)
        {
            if (stream.Size == 0)
            {
                throw new FileNotFoundException();
            }

            BitmapImage image = new BitmapImage();
            await image.SetSourceAsync(stream).AsTask().ConfigureAwait(false);

            return image;
        }

        /// <summary>
        /// Cache specific hooks to process items from HTTP response
        /// </summary>
        /// <param name="baseFile">storage file</param>
        /// <returns>awaitable task</returns>
        protected override async Task<BitmapImage> InitializeTypeAsync(StorageFile baseFile)
        {
            using (var stream = await baseFile.OpenReadAsync().AsTask().ConfigureAwait(MaintainContext))
            {
                return await InitializeTypeAsync(stream).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Override-able method that checks whether file is valid or not.
        /// </summary>
        /// <param name="file">storage file</param>
        /// <param name="duration">cache duration</param>
        /// <param name="treatNullFileAsOutOfDate">option to mark uninitialized file as expired</param>
        /// <returns>bool indicate whether file has expired or not</returns>
        protected override async Task<bool> IsFileOutOfDateAsync(StorageFile file, TimeSpan duration, bool treatNullFileAsOutOfDate = true)
        {
            if (file == null)
            {
                return treatNullFileAsOutOfDate;
            }

            // Get extended properties.
            IDictionary<string, object> extraProperties =
                await file.Properties.RetrievePropertiesAsync(_extendedPropertyNames).AsTask().ConfigureAwait(false);

            // Get date-accessed property.
            var propValue = extraProperties[DateAccessedProperty];

            if (propValue != null)
            {
                var lastAccess = propValue as DateTimeOffset?;

                if (lastAccess.HasValue)
                {
                    return DateTime.Now.Subtract(lastAccess.Value.DateTime) > duration;
                }
            }

            var properties = await file.GetBasicPropertiesAsync().AsTask().ConfigureAwait(false);

            return properties.Size == 0 || DateTime.Now.Subtract(properties.DateModified.DateTime) > duration;
        }
    }
}
