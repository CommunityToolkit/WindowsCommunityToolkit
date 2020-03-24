// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Provides methods and tools to cache files in a folder
    /// </summary>
    /// <typeparam name="T">Generic type as supplied by consumer of the class</typeparam>
    public abstract class CacheBase<T>
    {
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

        private ConcurrentDictionary<string, ConcurrentRequest> _concurrentTasks = new ConcurrentDictionary<string, ConcurrentRequest>();

        private HttpClient _httpClient = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheBase{T}"/> class.
        /// </summary>
        protected CacheBase()
        {
            CacheDuration = TimeSpan.FromDays(1);
            _inMemoryFileStorage = new InMemoryStorage<T>();
            RetryCount = 1;
        }

        /// <summary>
        /// Gets or sets the life duration of every cache entry.
        /// </summary>
        public TimeSpan CacheDuration { get; set; }

        /// <summary>
        /// Gets or sets the number of retries trying to ensure the file is cached.
        /// </summary>
        public uint RetryCount { get; set; }

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
        /// Gets instance of <see cref="HttpClient"/>
        /// </summary>
        protected HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    var messageHandler = new HttpClientHandler() { MaxConnectionsPerServer = 20 };

                    _httpClient = new HttpClient(messageHandler);
                }

                return _httpClient;
            }
        }

        /// <summary>
        /// Initializes FileCache and provides root folder and cache folder name
        /// </summary>
        /// <param name="folder">Folder that is used as root for cache</param>
        /// <param name="folderName">Cache folder name</param>
        /// <param name="httpMessageHandler">instance of <see cref="HttpMessageHandler"/></param>
        /// <returns>awaitable task</returns>
        public virtual async Task InitializeAsync(StorageFolder folder = null, string folderName = null, HttpMessageHandler httpMessageHandler = null)
        {
            _baseFolder = folder;
            _cacheFolderName = folderName;

            _cacheFolder = await GetCacheFolderAsync().ConfigureAwait(false);

            if (httpMessageHandler != null)
            {
                _httpClient = new HttpClient(httpMessageHandler);
            }
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
            TimeSpan expiryDuration = duration ?? CacheDuration;

            var folder = await GetCacheFolderAsync().ConfigureAwait(false);
            var files = await folder.GetFilesAsync().AsTask().ConfigureAwait(false);

            var filesToDelete = new List<StorageFile>();

            foreach (var file in files)
            {
                if (file == null)
                {
                    continue;
                }

                if (await IsFileOutOfDateAsync(file, expiryDuration, false).ConfigureAwait(false))
                {
                    filesToDelete.Add(file);
                }
            }

            await InternalClearAsync(filesToDelete).ConfigureAwait(false);

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
                if (hashDictionary.TryGetValue(fileName, out var file))
                {
                    filesToDelete.Add(file);
                    keys.Add(fileName);
                }
            }

            await InternalClearAsync(filesToDelete).ConfigureAwait(false);

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
            return GetItemAsync(uri, throwOnError, !storeToMemoryCache, cancellationToken, null);
        }

        /// <summary>
        /// Retrieves item represented by Uri from the cache. If the item is not found in the cache, it will try to downloaded and saved before returning it to the caller.
        /// </summary>
        /// <param name="uri">Uri of the item.</param>
        /// <param name="throwOnError">Indicates whether or not exception should be thrown if item cannot be found / downloaded.</param>
        /// <param name="cancellationToken">instance of <see cref="CancellationToken"/></param>
        /// <param name="initializerKeyValues">key value pairs used when initializing instance of generic type</param>
        /// <returns>an instance of Generic type</returns>
        public Task<T> GetFromCacheAsync(Uri uri, bool throwOnError = false, CancellationToken cancellationToken = default(CancellationToken), List<KeyValuePair<string, object>> initializerKeyValues = null)
        {
            return GetItemAsync(uri, throwOnError, false, cancellationToken, initializerKeyValues);
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
        /// Retrieves item represented by Uri from the in-memory cache if it exists and is not out of date. If item is not found or is out of date, default instance of the generic type is returned.
        /// </summary>
        /// <param name="uri">Uri of the item.</param>
        /// <returns>an instance of Generic type</returns>
        public T GetFromMemoryCache(Uri uri)
        {
            T instance = default(T);

            string fileName = GetCacheFileName(uri);

            if (_inMemoryFileStorage.MaxItemCount > 0)
            {
                var msi = _inMemoryFileStorage.GetItem(fileName, CacheDuration);
                if (msi != null)
                {
                    instance = msi.Item;
                }
            }

            return instance;
        }

        /// <summary>
        /// Cache specific hooks to process items from HTTP response
        /// </summary>
        /// <param name="stream">input stream</param>
        /// <param name="initializerKeyValues">key value pairs used when initializing instance of generic type</param>
        /// <returns>awaitable task</returns>
        protected abstract Task<T> InitializeTypeAsync(Stream stream, List<KeyValuePair<string, object>> initializerKeyValues = null);

        /// <summary>
        /// Cache specific hooks to process items from HTTP response
        /// </summary>
        /// <param name="baseFile">storage file</param>
        /// <param name="initializerKeyValues">key value pairs used when initializing instance of generic type</param>
        /// <returns>awaitable task</returns>
        protected abstract Task<T> InitializeTypeAsync(StorageFile baseFile, List<KeyValuePair<string, object>> initializerKeyValues = null);

        /// <summary>
        /// Override-able method that checks whether file is valid or not.
        /// </summary>
        /// <param name="file">storage file</param>
        /// <param name="duration">cache duration</param>
        /// <param name="treatNullFileAsOutOfDate">option to mark uninitialized file as expired</param>
        /// <returns>bool indicate whether file has expired or not</returns>
        protected virtual async Task<bool> IsFileOutOfDateAsync(StorageFile file, TimeSpan duration, bool treatNullFileAsOutOfDate = true)
        {
            if (file == null)
            {
                return treatNullFileAsOutOfDate;
            }

            var properties = await file.GetBasicPropertiesAsync().AsTask().ConfigureAwait(false);

            return properties.Size == 0 || DateTime.Now.Subtract(properties.DateModified.DateTime) > duration;
        }

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

        private async Task<T> GetItemAsync(Uri uri, bool throwOnError, bool preCacheOnly, CancellationToken cancellationToken, List<KeyValuePair<string, object>> initializerKeyValues)
        {
            T instance = default(T);

            string fileName = GetCacheFileName(uri);
            _concurrentTasks.TryGetValue(fileName, out var request);

            // if similar request exists check if it was preCacheOnly and validate that current request isn't preCacheOnly
            if (request != null && request.EnsureCachedCopy && !preCacheOnly)
            {
                await request.Task.ConfigureAwait(false);
                request = null;
            }

            if (request == null)
            {
                request = new ConcurrentRequest()
                {
                    Task = GetFromCacheOrDownloadAsync(uri, fileName, preCacheOnly, cancellationToken, initializerKeyValues),
                    EnsureCachedCopy = preCacheOnly
                };

                _concurrentTasks[fileName] = request;
            }

            try
            {
                instance = await request.Task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                if (throwOnError)
                {
                    throw;
                }
            }
            finally
            {
                _concurrentTasks.TryRemove(fileName, out _);
            }

            return instance;
        }

        private async Task<T> GetFromCacheOrDownloadAsync(Uri uri, string fileName, bool preCacheOnly, CancellationToken cancellationToken, List<KeyValuePair<string, object>> initializerKeyValues)
        {
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

            var folder = await GetCacheFolderAsync().ConfigureAwait(false);
            var baseFile = await folder.TryGetItemAsync(fileName).AsTask().ConfigureAwait(false) as StorageFile;

            bool downloadDataFile = baseFile == null || await IsFileOutOfDateAsync(baseFile, CacheDuration).ConfigureAwait(false);

            if (baseFile == null)
            {
                baseFile = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting).AsTask().ConfigureAwait(false);
            }

            if (downloadDataFile)
            {
                uint retries = 0;
                try
                {
                    while (retries < RetryCount)
                    {
                        try
                        {
                            instance = await DownloadFileAsync(uri, baseFile, preCacheOnly, cancellationToken, initializerKeyValues).ConfigureAwait(false);

                            if (instance != null)
                            {
                                break;
                            }
                        }
                        catch (FileNotFoundException)
                        {
                        }

                        retries++;
                    }
                }
                catch (Exception)
                {
                    await baseFile.DeleteAsync().AsTask().ConfigureAwait(false);
                    throw; // re-throwing the exception changes the stack trace. just throw
                }
            }

            if (EqualityComparer<T>.Default.Equals(instance, default(T)) && !preCacheOnly)
            {
                instance = await InitializeTypeAsync(baseFile, initializerKeyValues).ConfigureAwait(false);

                if (_inMemoryFileStorage.MaxItemCount > 0)
                {
                    var properties = await baseFile.GetBasicPropertiesAsync().AsTask().ConfigureAwait(false);

                    var msi = new InMemoryStorageItem<T>(fileName, properties.DateModified.DateTime, instance);
                    _inMemoryFileStorage.SetItem(msi);
                }
            }

            return instance;
        }

        private async Task<T> DownloadFileAsync(Uri uri, StorageFile baseFile, bool preCacheOnly, CancellationToken cancellationToken, List<KeyValuePair<string, object>> initializerKeyValues)
        {
            T instance = default(T);

            using (MemoryStream ms = new MemoryStream())
            {
                using (var stream = await HttpClient.GetStreamAsync(uri))
                {
                    stream.CopyTo(ms);
                    ms.Flush();

                    ms.Position = 0;

                    using (var fs = await baseFile.OpenStreamForWriteAsync())
                    {
                        ms.CopyTo(fs);

                        fs.Flush();

                        ms.Position = 0;
                    }
                }

                // if its pre-cache we aren't looking to load items in memory
                if (!preCacheOnly)
                {
                    instance = await InitializeTypeAsync(ms, initializerKeyValues).ConfigureAwait(false);
                }
            }

            return instance;
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
