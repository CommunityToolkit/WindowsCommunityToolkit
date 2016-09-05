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
namespace Microsoft.Toolkit.Uwp.UI
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Windows.Storage;
    using Windows.Storage.Streams;
    using Windows.UI.Xaml.Media.Imaging;

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

        private class ConcurrentRequest
        {
            public Task<BitmapImage> Task { get; set; }

            public bool EnsureCachedCopy { get; set; }
        }

        private const string CacheFolderName = "ImageCache";

        private static readonly SemaphoreSlim _cacheFolderSemaphore = new SemaphoreSlim(1);
        private static readonly Dictionary<string, ConcurrentRequest> _concurrentTasks = new Dictionary<string, ConcurrentRequest>();

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
        /// Gets or sets the count of in-memory cache. Set it to 0 to disable in-memory caching. It is 0 by default.
        /// </summary>
        public static int MaxMemoryCacheCount
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
            DateTime expirationDate = DateTime.Now.Subtract(duration.HasValue ? duration.Value : TimeSpan.Zero);
            try
            {
                var folder = await GetCacheFolderAsync();
                var files = (await folder.GetFilesAsync()).ToList();

                List<StorageFile> filesToDelete = null;
                if (duration == null)
                {
                    filesToDelete = files;
                }
                else
                {
                    filesToDelete = new List<StorageFile>();

                    foreach (var file in files)
                    {
                        if (await IsFileOutOfDate(file, expirationDate))
                        {
                            filesToDelete.Add(file);
                        }
                    }
                }

                foreach (var file in filesToDelete)
                {
                    try
                    {
                        await file.DeleteAsync();
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
                if (duration == null)
                {
                    _memoryCache.Clear();
                    return;
                }

                // clears expired items in in-memory cache
                var keysToDelete = new List<object>();

                foreach (var k in _memoryCache.Keys)
                {
                    if (((MemoryCacheItem)_memoryCache[k]).LastUpdated < expirationDate)
                    {
                        keysToDelete.Add(k);
                    }
                }

                foreach (var key in keysToDelete)
                {
                    _memoryCache.Remove(key);
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
        /// <param name="throwOnError">determines whether errors are handled silently or not</param>
        /// <returns>void</returns>
        public static Task PreCacheAsync(Uri uri, bool storeToMemoryCache = false, bool throwOnError = false)
        {
            return GetItemAsync(uri, throwOnError, !storeToMemoryCache);
        }

        /// <summary>
        /// Load a specific image from the cache. If the image is not in the cache, ImageCache will try to download and store it.
        /// </summary>
        /// <param name="uri">Uri of the image.</param>
        /// <param name="throwOnError">Indicates whether or not exception should be thrown if imagge cannot be loaded</param>
        /// <returns>a BitmapImage</returns>
        public static Task<BitmapImage> GetFromCacheAsync(Uri uri, bool throwOnError = false)
        {
            return GetItemAsync(uri, throwOnError, false);
        }

        private static async Task<BitmapImage> GetItemAsync(Uri uri, bool throwOnError, bool preCacheOnly)
        {
            ConcurrentRequest request;
            string key = GetCacheFileName(uri);
            BitmapImage image = null;

            lock (_concurrentTasks)
            {
                if (_concurrentTasks.ContainsKey(key))
                {
                    request = _concurrentTasks[key];
                }
                else
                {
                    request = new ConcurrentRequest()
                    {
                        Task = GetFromCacheOrDownloadAsync(uri, key, preCacheOnly),
                        EnsureCachedCopy = preCacheOnly
                    };
                    _concurrentTasks.Add(key, request);
                }
            }

            try
            {
                image = await request.Task;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                if (throwOnError)
                {
                    throw; // dont rethrow ex
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

        private static async Task<BitmapImage> GetFromCacheOrDownloadAsync(Uri uri, string key, bool preCacheOnly)
        {
            BitmapImage image = null;
            DateTime expirationDate = DateTime.Now.Subtract(CacheDuration);

            if (MaxMemoryCacheCount > 0)
            {
                image = GetFromMemoryCache(key);
            }

            if (image != null)
            {
                return image;
            }

            var folder = await GetCacheFolderAsync();

            var baseFile = await folder.TryGetItemAsync(key) as StorageFile;
            if (await IsFileOutOfDate(baseFile, expirationDate))
            {
                baseFile = await folder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
                try
                {
                    image = await DownloadFile(uri, baseFile, preCacheOnly);
                }
                catch (Exception)
                {
                    await baseFile.DeleteAsync();
                    throw; // rethrowing the exception changes the stack trace. just throw
                }
            }

            if (image == null && !preCacheOnly)
            {
                using (var fileStream = await baseFile.OpenAsync(FileAccessMode.Read))
                {
                    image = new BitmapImage();
                    image.SetSource(fileStream);
                }

                if (MaxMemoryCacheCount > 0)
                {
                    StoreToMemoryCache(key, image);
                }
            }

            return image;
        }

        private static async Task<BitmapImage> DownloadFile(Uri uri, StorageFile baseFile, bool preCacheOnly)
        {
            BitmapImage image = null;

            using (var webStream = await StreamHelper.GetHttpStreamAsync(uri))
            {
                // if its pre-cache we aren't looking to load items in memory
                if (!preCacheOnly)
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

                _memoryCache[key] = mci;

                FixMemoryCacheSize();
            }
        }

        private static void FixMemoryCacheSize()
        {
            if (MaxMemoryCacheCount <= 0)
            {
                _memoryCache.Clear();
            }
            else
            {
                var removeCount = _memoryCache.Count - MaxMemoryCacheCount;
                for (var i = 1; i <= removeCount; i++)
                {
                    _memoryCache.RemoveAt(0);
                }
            }
        }

        private static async Task<bool> IsFileOutOfDate(StorageFile file, DateTime expirationDate)
        {
            if (file == null)
            {
                return true;
            }

            var properties = await file.GetBasicPropertiesAsync();
            return properties.DateModified < expirationDate;
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
