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
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Provides methods and tools to cache files in a folder
    /// </summary>
    /// <typeparam name="T">Generic type as supplied by consumer of the class</typeparam>
    public abstract class CacheBase<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether context should be maintained until type has been instantiated or not.
        /// </summary>
        protected bool MaintainContext { get; set; }

        private class ConcurrentRequest
        {
            public Task<T> Task { get; set; }

            public bool EnsureCachedCopy { get; set; }
        }

        private readonly SemaphoreSlim _cacheFolderSemaphore = new SemaphoreSlim(1);

        private StorageFolder _baseFolder = null;
        private string _cacheFolderName = null;

        private StorageFolder _cacheFolder = null;
        private InMemoryStorage<T> _inMemoryFileStorage = null;

        private Dictionary<string, ConcurrentRequest> _concurrentTasks = new Dictionary<string, ConcurrentRequest>();
        private object _concurrencyLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheBase{T}"/> class.
        /// </summary>
        protected CacheBase()
        {
            CacheDuration = TimeSpan.FromDays(1);
            _inMemoryFileStorage = new InMemoryStorage<T>();
        }

        /// <summary>
        /// Gets or sets the life duration of every cache entry.
        /// </summary>
        public TimeSpan CacheDuration { get; set; }

        /// <summary>
        /// Gets or sets max in-memory item storage count
        /// </summary>
        public int MaxMemoryCacheCount
        {
            get
            {
                return _inMemoryFileStorage.MaxItemCount;
            }

            set
            {
                _inMemoryFileStorage.MaxItemCount = value;
            }
        }

        /// <summary>
        /// Initialises FileCache and provides root folder and cache folder name
        /// </summary>
        /// <param name="folder">Folder that is used as root for cache</param>
        /// <param name="folderName">Cache folder name</param>
        /// <returns>awaitable task</returns>
        public virtual async Task InitializeAsync(StorageFolder folder, string folderName)
        {
            _baseFolder = folder;
            _cacheFolderName = folderName;

            _cacheFolder = await GetCacheFolderAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Clears all files in the cache
        /// </summary>
        /// <returns>awaitable task</returns>
        public async Task ClearAsync()
        {
            var folder = await GetCacheFolderAsync().ConfigureAwait(false);
            var files = await folder.GetFilesAsync().AsTask().ConfigureAwait(false);

            await InternalClearAsync(files).ConfigureAwait(false);

            _inMemoryFileStorage.Clear();
        }

        /// <summary>
        /// Clears file if it has expired
        /// </summary>
        /// <param name="duration">timespan to compute whether file has expired or not</param>
        /// <returns>awaitable task</returns>
        public Task ClearAsync(TimeSpan duration)
        {
            return RemoveExpiredAsync(duration);
        }

        /// <summary>
        /// Removes cached files that have expired
        /// </summary>
        /// <param name="duration">Optional timespan to compute whether file has expired or not. If no value is supplied, <see cref="CacheDuration"/> is used.</param>
        /// <returns>awaitable task</returns>
        public async Task RemoveExpiredAsync(TimeSpan? duration = null)
        {
            TimeSpan expiryDuration = duration.HasValue ? duration.Value : CacheDuration;

            var folder = await GetCacheFolderAsync().ConfigureAwait(false);
            var files = await folder.GetFilesAsync().AsTask().ConfigureAwait(false);

            var filesToDelete = new List<StorageFile>();

            foreach (var file in files)
            {
                if (file == null)
                {
                    continue;
                }

                if (await IsFileOutOfDate(file, expiryDuration, false).ConfigureAwait(false))
                {
                    filesToDelete.Add(file);
                }
            }

            await InternalClearAsync(files).ConfigureAwait(false);

            _inMemoryFileStorage.Clear(expiryDuration);
        }

        /// <summary>
        /// Assures that image is available in the cache
        /// </summary>
        /// <param name="uri">Uri of the image</param>
        /// <param name="fileName">fileName to for local storage</param>
        /// <param name="throwOnError">Indicates whether or not exception should be thrown if imagge cannot be loaded</param>
        /// <param name="storeToMemoryCache">Indicates if image should be available also in memory cache</param>
        /// <returns>void</returns>
        public Task PreCacheAsync(Uri uri, string fileName, bool throwOnError = false, bool storeToMemoryCache = false)
        {
            return GetItemAsync(uri, fileName, throwOnError, !storeToMemoryCache);
        }

        /// <summary>
        /// Load a specific image from the cache. If the image is not in the cache, ImageCache will try to download and store it.
        /// </summary>
        /// <param name="uri">Uri of the image.</param>
        /// <param name="fileName">fileName to for local storage</param>
        /// <param name="throwOnError">Indicates whether or not exception should be thrown if imagge cannot be loaded</param>
        /// <returns>a BitmapImage</returns>
        public Task<T> GetFromCacheAsync(Uri uri, string fileName, bool throwOnError = false)
        {
            return GetItemAsync(uri, fileName, throwOnError, false);
        }

        /// <summary>
        /// Cache specific hooks to process items from http response
        /// </summary>
        /// <param name="stream">input stream</param>
        /// <returns>awaitable task</returns>
        protected abstract Task<T> InitializeTypeAsync(IRandomAccessStream stream);

        /// <summary>
        /// Cache specific hooks to process items from http response
        /// </summary>
        /// <param name="baseFile">storage file</param>
        /// <returns>awaitable task</returns>
        protected abstract Task<T> InitializeTypeAsync(StorageFile baseFile);

        private async Task<T> GetItemAsync(Uri uri, string fileName, bool throwOnError, bool preCacheOnly)
        {
            T instance = default(T);

            ConcurrentRequest request = null;

            lock (_concurrencyLock)
            {
                if (_concurrentTasks.ContainsKey(fileName))
                {
                    request = _concurrentTasks[fileName];
                }
            }

            // if similar request exists check if it was preCacheOnly and validate that current request isn't preCacheOnly
            if (request != null && request.EnsureCachedCopy && !preCacheOnly)
            {
                await request.Task.ConfigureAwait(MaintainContext);
                request = null;
            }

            if (request == null)
            {
                request = new ConcurrentRequest()
                {
                    Task = GetFromCacheOrDownloadAsync(uri, fileName, preCacheOnly),
                    EnsureCachedCopy = preCacheOnly
                };

                lock (_concurrencyLock)
                {
                    _concurrentTasks.Add(fileName, request);
                }
            }

            try
            {
                instance = await request.Task.ConfigureAwait(MaintainContext);
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
                    if (_concurrentTasks.ContainsKey(fileName))
                    {
                        _concurrentTasks.Remove(fileName);
                    }
                }
            }

            return instance;
        }

        private async Task<T> GetFromCacheOrDownloadAsync(Uri uri, string fileName, bool preCacheOnly)
        {
            StorageFile baseFile = null;
            T instance = default(T);

            if (_inMemoryFileStorage.MaxItemCount > 0)
            {
                var msi = _inMemoryFileStorage.GetItem(fileName, CacheDuration);
                if (msi != null)
                {
                    instance = msi.Item;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            var folder = await GetCacheFolderAsync().ConfigureAwait(MaintainContext);

            if (baseFile == null || await IsFileOutOfDate(baseFile, CacheDuration).ConfigureAwait(MaintainContext))
            {
                baseFile = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting).AsTask().ConfigureAwait(MaintainContext);
                try
                {
                    instance = await DownloadFileAsync(uri, baseFile, preCacheOnly).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    await baseFile.DeleteAsync().AsTask().ConfigureAwait(false);
                    throw; // rethrowing the exception changes the stack trace. just throw
                }
            }

            if (EqualityComparer<T>.Default.Equals(instance, default(T)) && !preCacheOnly)
            {
                using (var fileStream = await baseFile.OpenAsync(FileAccessMode.Read).AsTask().ConfigureAwait(MaintainContext))
                {
                    instance = await InitializeTypeAsync(fileStream).ConfigureAwait(false);
                }

                if (_inMemoryFileStorage.MaxItemCount > 0)
                {
                    var properties = await baseFile.GetBasicPropertiesAsync().AsTask().ConfigureAwait(false);

                    var msi = new InMemoryStorageItem<T>(fileName, properties.DateModified.DateTime, instance);
                    _inMemoryFileStorage.SetItem(msi);
                }
            }

            return instance;
        }

        private async Task<T> DownloadFileAsync(Uri uri, StorageFile baseFile, bool preCacheOnly)
        {
            T instance = default(T);

            using (var webStream = await StreamHelper.GetHttpStreamAsync(uri).ConfigureAwait(MaintainContext))
            {
                // if its pre-cache we aren't looking to load items in memory
                if (!preCacheOnly)
                {
                    instance = await InitializeTypeAsync(webStream).ConfigureAwait(false);

                    webStream.Seek(0);
                }

                using (var reader = new DataReader(webStream))
                {
                    await reader.LoadAsync((uint)webStream.Size).AsTask().ConfigureAwait(false);
                    var buffer = new byte[(int)webStream.Size];
                    reader.ReadBytes(buffer);
                    await FileIO.WriteBytesAsync(baseFile, buffer).AsTask().ConfigureAwait(false);
                }
            }

            return instance;
        }

        private async Task<bool> IsFileOutOfDate(StorageFile file, TimeSpan duration, bool treatNullFileAsOutOfDate = true)
        {
            if (file == null)
            {
                return treatNullFileAsOutOfDate;
            }

            var properties = await file.GetBasicPropertiesAsync().AsTask().ConfigureAwait(false);
            return DateTime.Now.Subtract(properties.DateModified.DateTime) > duration;
        }

        private async Task InternalClearAsync(IEnumerable<StorageFile> files)
        {
            foreach (var file in files)
            {
                try
                {
                    await file.DeleteAsync().AsTask().ConfigureAwait(false);
                }
                catch
                {
                    // Just ignore errors for now}
                }
            }
        }

        /// <summary>
        /// Initialises with default values if user has not initialised explicitly
        /// </summary>
        /// <returns>awaitable task</returns>
        private async Task ForceInitialiseAsync()
        {
            if (_cacheFolder != null)
            {
                return;
            }

            await _cacheFolderSemaphore.WaitAsync().ConfigureAwait(false);

            _inMemoryFileStorage = new InMemoryStorage<T>();

            if (_baseFolder == null)
            {
                _baseFolder = ApplicationData.Current.TemporaryFolder;
            }

            if (string.IsNullOrWhiteSpace(_cacheFolderName))
            {
                _cacheFolderName = GetType().Name;
            }

            try
            {
                _cacheFolder = await _baseFolder.CreateFolderAsync(_cacheFolderName, CreationCollisionOption.OpenIfExists).AsTask().ConfigureAwait(false);
            }
            finally
            {
                _cacheFolderSemaphore.Release();
            }
        }

        private async Task<StorageFolder> GetCacheFolderAsync()
        {
            if (_cacheFolder == null)
            {
                await ForceInitialiseAsync().ConfigureAwait(false);
            }

            return _cacheFolder;
        }
    }
}
