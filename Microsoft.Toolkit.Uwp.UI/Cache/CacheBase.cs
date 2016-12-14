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
using System.Linq;
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
        /// Initializes FileCache and provides root folder and cache folder name
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
        /// Removed items based on uri list passed
        /// </summary>
        /// <param name="uriForCachedItems">Enumerable uri list</param>
        /// <returns>awaitable Task</returns>
        public async Task RemoveAsync(IEnumerable<Uri> uriForCachedItems)
        {
            if (uriForCachedItems == null || !uriForCachedItems.Any())
            {
                return;
            }

            var folder = await GetCacheFolderAsync().ConfigureAwait(false);
            var files = await folder.GetFilesAsync().AsTask().ConfigureAwait(false);

            var filesToDelete = new List<StorageFile>();
            var keys = new List<string>();

            Dictionary<string, StorageFile> hashDictionary = new Dictionary<string, StorageFile>();

            foreach (var file in files)
            {
                hashDictionary.Add(file.Name, file);
            }

            foreach (var uri in uriForCachedItems)
            {
                string fileName = GetCacheFileName(uri);

                StorageFile file = null;

                if (hashDictionary.TryGetValue(fileName, out file))
                {
                    filesToDelete.Add(file);
                    keys.Add(fileName);
                }
            }

            await InternalClearAsync(files).ConfigureAwait(false);

            _inMemoryFileStorage.Remove(keys);
        }

        /// <summary>
        /// Assures that item represented by Uri is cached.
        /// </summary>
        /// <param name="uri">Uri of the item</param>
        /// <param name="throwOnError">Indicates whether or not exception should be thrown if item cannot be cached</param>
        /// <param name="storeToMemoryCache">Indicates if item should be loaded into the in-memory storage</param>
        /// <param name="cancellationToken">instance of <see cref="CancellationToken"/></param>
        /// <returns>Awaitable Task</returns>
        public Task PreCacheAsync(Uri uri, bool throwOnError = false, bool storeToMemoryCache = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetItemAsync(uri, throwOnError, !storeToMemoryCache, cancellationToken);
        }

        /// <summary>
        /// Retrieves item represented by Uri from the cache. If the item is not found in the cache, it will try to downloaded and saved before returning it to the caller.
        /// </summary>
        /// <param name="uri">Uri of the item.</param>
        /// <param name="throwOnError">Indicates whether or not exception should be thrown if item cannot be found / downloaded.</param>
        /// <returns>an instance of Generic type</returns>
        public Task<T> GetFromCacheAsync(Uri uri, bool throwOnError = false)
        {
            return GetItemAsync(uri, throwOnError, false, default(CancellationToken));
        }

        /// <summary>
        /// Gets the StorageFile containing cached item for given Uri
        /// </summary>
        /// <param name="uri">Uri of the item.</param>
        /// <returns>a StorageFile</returns>
        public async Task<StorageFile> GetFileFromCacheAsync(Uri uri)
        {
            var folder = await GetCacheFolderAsync().ConfigureAwait(false);

            string fileName = GetCacheFileName(uri);

            var item = await folder.TryGetItemAsync(fileName).AsTask().ConfigureAwait(false);

            return item as StorageFile;
        }

        /// <summary>
        /// Cache specific hooks to process items from HTTP response
        /// </summary>
        /// <param name="stream">input stream</param>
        /// <returns>awaitable task</returns>
        protected abstract Task<T> InitializeTypeAsync(IRandomAccessStream stream);

        /// <summary>
        /// Cache specific hooks to process items from HTTP response
        /// </summary>
        /// <param name="baseFile">storage file</param>
        /// <returns>awaitable task</returns>
        protected abstract Task<T> InitializeTypeAsync(StorageFile baseFile);

        private static string GetCacheFileName(Uri uri)
        {
            return CreateHash64(uri.ToString()).ToString();
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

        private async Task<T> GetItemAsync(Uri uri, bool throwOnError, bool preCacheOnly, CancellationToken cancellationToken)
        {
            T instance = default(T);

            string fileName = GetCacheFileName(uri);

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
                    Task = GetFromCacheOrDownloadAsync(uri, fileName, preCacheOnly, cancellationToken),
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

        private async Task<T> GetFromCacheOrDownloadAsync(Uri uri, string fileName, bool preCacheOnly, CancellationToken cancellationToken)
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
            baseFile = await folder.TryGetItemAsync(fileName).AsTask().ConfigureAwait(MaintainContext) as StorageFile;

            if (baseFile == null || await IsFileOutOfDate(baseFile, CacheDuration).ConfigureAwait(MaintainContext))
            {
                baseFile = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting).AsTask().ConfigureAwait(MaintainContext);
                try
                {
                    instance = await DownloadFileAsync(uri, baseFile, preCacheOnly, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    await baseFile.DeleteAsync().AsTask().ConfigureAwait(false);
                    throw; // re-throwing the exception changes the stack trace. just throw
                }
            }

            if (EqualityComparer<T>.Default.Equals(instance, default(T)) && !preCacheOnly)
            {
                instance = await InitializeTypeAsync(baseFile).ConfigureAwait(false);

                if (_inMemoryFileStorage.MaxItemCount > 0)
                {
                    var properties = await baseFile.GetBasicPropertiesAsync().AsTask().ConfigureAwait(false);

                    var msi = new InMemoryStorageItem<T>(fileName, properties.DateModified.DateTime, instance);
                    _inMemoryFileStorage.SetItem(msi);
                }
            }

            return instance;
        }

        private async Task<T> DownloadFileAsync(Uri uri, StorageFile baseFile, bool preCacheOnly, CancellationToken cancellationToken)
        {
            T instance = default(T);

            using (var webStream = await StreamHelper.GetHttpStreamAsync(uri, cancellationToken).ConfigureAwait(MaintainContext))
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
            return properties.Size == 0 || DateTime.Now.Subtract(properties.DateModified.DateTime) > duration;
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
        /// Initializes with default values if user has not initialized explicitly
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
