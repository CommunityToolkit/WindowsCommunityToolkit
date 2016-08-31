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
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Provides methods and tools to cache images in a temporary local folder
    /// </summary>
    public static class ImageCache
    {
        /// <summary>
        /// Helper class used to store BitmapImages to in-memory cache
        /// </summary>
        private class MemoryCacheItem
        {
            /// <summary>
            /// Gets or sets dateTime when image was last stored to in-memory cache
            /// </summary>
            public DateTime LastUpdated { get; set; }

            /// <summary>
            /// Gets or sets BitmapImage of in-memory cached item
            /// </summary>
            public BitmapImage Image { get; set; }
        }

        private class ConcurrentTask
        {
            public Task<BitmapImage> Task { get; set; }

            public bool IsAssureOnly { get; set; }
        }

        private const string CacheFolderName = "ImageCache";

        private static readonly SemaphoreSlim _cacheFolderSemaphore = new SemaphoreSlim(1);
        private static readonly Dictionary<string, ConcurrentTask> _concurrentTasks = new Dictionary<string, ConcurrentTask>();

        private static readonly object _concurrencyLock = new object();
        private static StorageFolder _cacheFolder;
        private static OrderedDictionary _memoryCache = new OrderedDictionary();
        private static int _maxMemoryCacheSize = 0;

        static ImageCache()
        {
            CacheDuration = TimeSpan.FromHours(24);
        }

        /// <summary>
        /// Gets or sets the life duration of every cache entry.
        /// </summary>
        public static TimeSpan CacheDuration { get; set; }

        /// <summary>
        /// Gets or sets the size of additional in-memory cache. Set it to 0 to disable in-memory caching. It is 0 by default.
        /// </summary>
        public static int MaxMemoryCacheSize
        {
            get
            {
                return _maxMemoryCacheSize;
            }

            set
            {
                _maxMemoryCacheSize = value;
                lock (_memoryCache)
                {
                    FixMemoryCacheSize();
                }
            }
        }

        /// <summary>
        /// call this method to clear the entire cache.
        /// </summary>
        /// <param name="duration">Use this parameter to define a timespan from now to select cache entries to delete.</param>
        /// <returns>Task</returns>
        public static async Task ClearAsync(TimeSpan? duration = null)
        {
            duration = duration ?? TimeSpan.FromSeconds(0);
            DateTime expirationDate = DateTime.Now.Subtract(duration.Value);
            try
            {
                var folder = await GetCacheFolderAsync();
                foreach (var file in await folder.GetFilesAsync())
                {
                    try
                    {
                        if (file.DateCreated < expirationDate)
                        {
                            await file.DeleteAsync();
                        }
                    }
                    catch
                    {
                        // Just ignore errors for now
                    }
                }
            }
            catch
            {
                // Just ignore errors for now
            }

            lock (_memoryCache)
            {
                // clears expired items in in-memory cache
                foreach (var k in _memoryCache.Keys)
                {
                    if (((MemoryCacheItem)_memoryCache[k]).LastUpdated < expirationDate)
                    {
                        _memoryCache.Remove(k);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the local cache file name associated with a specified Uri.
        /// </summary>
        /// <param name="uri">Uri of the resource.</param>
        /// <returns>Filename associated with the Uri.</returns>
        public static string GetCacheFileName(Uri uri)
        {
            ulong uriHash = CreateHash64(uri);

            return $"{uriHash}.jpg";
        }

        /// <summary>
        /// Assures that image is available in the cache
        /// </summary>
        /// <param name="uri">Uri of the image</param>
        /// <param name="storeToMemoryCache">Indicates if image should be available also in memory cache</param>
        /// <returns>void</returns>
        public static Task PreCacheAsync(Uri uri, bool storeToMemoryCache = false)
        {
            return GetFromCacheInternalAsync(uri, true, !storeToMemoryCache);
        }

        /// <summary>
        /// Load a specific image from the cache. If the image is not in the cache, ImageCache will try to download and store it.
        /// </summary>
        /// <param name="uri">Uri of the image.</param>
        /// <param name="throwOnError">Indicates whether or not exception should be thrown if imagge cannot be loaded</param>
        /// <returns>a BitmapImage</returns>
        public static Task<BitmapImage> GetFromCacheAsync(Uri uri, bool throwOnError = false)
        {
            return GetFromCacheInternalAsync(uri, throwOnError, false);
        }

        private static async Task<BitmapImage> GetFromCacheInternalAsync(Uri uri, bool throwOnError, bool assureOnly)
        {
            ConcurrentTask busy;
            string key = GetCacheFileName(uri);
            BitmapImage image = null;

            lock (_concurrentTasks)
            {
                if (_concurrentTasks.ContainsKey(key))
                {
                    busy = _concurrentTasks[key];
                }
                else
                {
                    busy = new ConcurrentTask()
                    {
                        Task = GetFromCacheOrDownloadAsync(uri, key, assureOnly),
                        IsAssureOnly = assureOnly
                    };
                    _concurrentTasks.Add(key, busy);
                }
            }

            // if current task is assureOnly and we need !assureOnly we wait for it to complete and create new one that is not assureOnly
            if (!assureOnly && busy.IsAssureOnly)
            {
                try
                {
                    await busy.Task;
                }
                catch
                {
                    // ignore errors because we will create another task
                }

                lock (_concurrentTasks)
                {
                    if (_concurrentTasks.ContainsKey(key))
                    {
                        _concurrentTasks.Remove(key);
                    }

                    busy = new ConcurrentTask()
                    {
                        Task = GetFromCacheOrDownloadAsync(uri, key, assureOnly),
                        IsAssureOnly = assureOnly
                    };
                    _concurrentTasks.Add(key, busy);
                }
            }

            try
            {
                image = await busy.Task;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                if (throwOnError)
                {
                    throw ex;
                }
            }
            finally
            {
                lock (_concurrencyLock)
                {
                    if (_concurrentTasks.ContainsKey(key))
                    {
                        _concurrentTasks.Remove(key);
                    }
                }
            }

            return image;
        }

        private static async Task<BitmapImage> GetFromCacheOrDownloadAsync(Uri uri, string key, bool assureOnly)
        {
            BitmapImage image = null;
            DateTime expirationDate = DateTime.Now.Subtract(CacheDuration);

            if (MaxMemoryCacheSize > 0)
            {
                image = GetFromMemoryCache(key);
                if (assureOnly && image != null)
                {
                    return null;
                }
            }

            if (image == null)
            {
                var folder = await GetCacheFolderAsync();

                var baseFile = await folder.TryGetItemAsync(key) as StorageFile;
                if (await IsFileOutOfDate(baseFile, expirationDate))
                {
                    baseFile = await folder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
                    try
                    {
                        using (var webStream = await StreamHelper.GetHttpStreamAsync(uri))
                        {
                            if (!assureOnly)
                            {
                                image = new BitmapImage();
                                image.SetSource(webStream);
                                webStream.Seek(0);
                            }

                            using (var reader = new DataReader(webStream))
                            {
                                await reader.LoadAsync((uint)webStream.Size);
                                var buffer = new byte[(int)webStream.Size];
                                reader.ReadBytes(buffer);
                                await FileIO.WriteBytesAsync(baseFile, buffer);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        await baseFile.DeleteAsync();
                        throw e;
                    }
                }
                else
                {
                    if (assureOnly)
                    {
                        return null;
                    }

                    using (var fileStream = await baseFile.OpenAsync(FileAccessMode.Read))
                    {
                        image = new BitmapImage();
                        image.SetSource(fileStream);
                    }
                }

                if (MaxMemoryCacheSize > 0 && image != null)
                {
                    StoreToMemoryCache(key, image);
                }
            }

            return image;
        }

        private static BitmapImage GetFromMemoryCache(string key)
        {
            BitmapImage resImage = null;
            lock (_memoryCache)
            {
                if (_memoryCache.Contains(key))
                {
                    var mci = _memoryCache[key] as MemoryCacheItem;
                    if (mci.LastUpdated > DateTime.Now.Subtract(CacheDuration))
                    {
                        resImage = mci.Image as BitmapImage;
                    }
                    else
                    {
                        _memoryCache.Remove(key);
                    }
                }
            }

            return resImage;
        }

        private static void StoreToMemoryCache(string key, BitmapImage image)
        {
            lock (_memoryCache)
            {
                var mci = new MemoryCacheItem()
                {
                    LastUpdated = DateTime.Now,
                    Image = image
                };
                if (_memoryCache.Contains(key))
                {
                    _memoryCache[key] = mci;
                }
                else
                {
                    _memoryCache.Add(key, mci);
                }

                FixMemoryCacheSize();
            }
        }

        private static void FixMemoryCacheSize()
        {
            if (MaxMemoryCacheSize <= 0)
            {
                _memoryCache.Clear();
            }
            else
            {
                var removeCount = _memoryCache.Count - MaxMemoryCacheSize;
                for (var i = 1; i <= removeCount; i++)
                {
                    _memoryCache.RemoveAt(0);
                }
            }
        }

        private static async Task<bool> IsFileOutOfDate(StorageFile file, DateTime expirationDate)
        {
            if (file != null)
            {
                var properties = await file.GetBasicPropertiesAsync();
                return properties.DateModified < expirationDate;
            }

            return true;
        }

        private static async Task<StorageFolder> GetCacheFolderAsync()
        {
            if (_cacheFolder == null)
            {
                await _cacheFolderSemaphore.WaitAsync();
                try
                {
                    _cacheFolder = await ApplicationData.Current.TemporaryFolder.CreateFolderAsync(CacheFolderName, CreationCollisionOption.OpenIfExists);
                }
                catch
                {
                }
                finally
                {
                    _cacheFolderSemaphore.Release();
                }
            }

            return _cacheFolder;
        }

        private static ulong CreateHash64(Uri uri)
        {
            return CreateHash64(uri.Host + uri.PathAndQuery);
        }

        private static ulong CreateHash64(string str)
        {
            byte[] utf8 = System.Text.Encoding.UTF8.GetBytes(str);

            ulong value = (ulong)utf8.Length;
            for (int n = 0; n < utf8.Length; n++)
            {
                value += (ulong)utf8[n] << ((n * 5) % 56);
            }

            return value;
        }
    }
}
