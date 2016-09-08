using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.UI.Cache
{
    /// <summary>
    /// Provides methods and tools to cache files in a folder
    /// </summary>
    public class CacheBase<T>
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
        public virtual async Task InitialiseAsync(StorageFolder folder, string folderName)
        {
            _baseFolder = folder;
            _cacheFolderName = folderName;

            _cacheFolder = await GetCacheFolderAsync();
        }

        /// <summary>
        /// Clears all files in the cache
        /// </summary>
        /// <returns>awaitable task</returns>
        public async Task ClearAsync()
        {
            var folder = await GetCacheFolderAsync();
            var files = await folder.GetFilesAsync();

            await InternalClearAsync(files);
        }

        /// <summary>
        /// Clears file if it has expired
        /// </summary>
        /// <param name="duration">timespan to compute whether file has expired or not</param>
        /// <returns>awaitable task</returns>
        public async Task ClearAsync(TimeSpan duration)
        {
            DateTime expirationDate = DateTime.Now.Subtract(duration);

            var folder = await GetCacheFolderAsync();
            var files = await folder.GetFilesAsync();

            var filesToDelete = new List<StorageFile>();

            foreach (var file in files)
            {
                if (await IsFileOutOfDate(file, expirationDate))
                {
                    filesToDelete.Add(file);
                }
            }

            await InternalClearAsync(files);
        }

        /// <summary>
        /// Assures that image is available in the cache
        /// </summary>
        /// <param name="uri">Uri of the image</param>
        /// <param name="fileName">fileName to for local storage</param>
        /// <param name="storeToMemoryCache">Indicates if image should be available also in memory cache</param>
        /// <returns>void</returns>
        public Task PreCacheAsync(Uri uri, string fileName, bool storeToMemoryCache = false)
        {
            return GetItemAsync(uri, fileName, true, !storeToMemoryCache);
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
        /// Cache specific hooks to proccess items from http response
        /// </summary>
        /// <param name="stream">input stream</param>
        /// <returns>awaitable task</returns>
        protected virtual async Task<T> InitialiseType(IRandomAccessStream stream)
        {
            // nothing to do in this instance;
            return default(T);
        }

        /// <summary>
        /// Cache specific hooks to proccess items from http response
        /// </summary>
        /// <param name="baseFile">storage file</param>
        /// <returns>awaitable task</returns>
        protected virtual async Task<T> InitialiseType(StorageFile baseFile)
        {
            // nothing to do in this instance;
            return default(T);
        }

        private async Task<T> GetItemAsync(Uri uri, string fileName, bool throwOnError, bool preCacheOnly)
        {
            T t = default(T);

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
                await request.Task;
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
                t = await request.Task;
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

            return t;
        }

        private async Task<T> GetFromCacheOrDownloadAsync(Uri uri, string fileName, bool preCacheOnly)
        {
            StorageFile baseFile = null;
            T t = default(T);
            DateTime expirationDate = DateTime.Now.Subtract(CacheDuration);

            if (_inMemoryFileStorage.MaxItemCount > 0)
            {
                var msi = _inMemoryFileStorage.GetItem(fileName, CacheDuration);
                if (msi != null)
                {
                    t = msi.Item;
                }
            }

            if (t != null)
            {
                return t;
            }

            var folder = await GetCacheFolderAsync();

            baseFile = await folder.TryGetItemAsync(fileName) as StorageFile;
            if (await IsFileOutOfDate(baseFile, expirationDate))
            {
                baseFile = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                try
                {
                    t = await DownloadFile(uri, baseFile, preCacheOnly);
                }
                catch (Exception)
                {
                    await baseFile.DeleteAsync();
                    throw; // rethrowing the exception changes the stack trace. just throw
                }
            }

            if (EqualityComparer<T>.Default.Equals(t, default(T)) && !preCacheOnly)
            {
                using (var fileStream = await baseFile.OpenAsync(FileAccessMode.Read))
                {
                    t = await InitialiseType(fileStream);
                }

                if (_inMemoryFileStorage.MaxItemCount > 0)
                {
                    var properties = await baseFile.GetBasicPropertiesAsync();

                    var msi = new InMemoryStorageItem<T>(fileName, properties.DateModified.DateTime, t);
                    _inMemoryFileStorage.SetItem(msi);
                }
            }

            return t;
        }

        private async Task<T> DownloadFile(Uri uri, StorageFile baseFile, bool preCacheOnly)
        {
            T t = default(T);

            using (var webStream = await StreamHelper.GetHttpStreamAsync(uri))
            {
                // if its pre-cache we aren't looking to load items in memory
                if (!preCacheOnly)
                {
                    t = await InitialiseType(webStream);

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

            return t;
        }

        private async Task<bool> IsFileOutOfDate(StorageFile file, DateTime expirationDate)
        {
            if (file == null)
            {
                return true;
            }

            var properties = await file.GetBasicPropertiesAsync();
            return properties.DateModified < expirationDate;
        }

        private async Task InternalClearAsync(IEnumerable<StorageFile> files)
        {
            foreach (var file in files)
            {
                try
                {
                    await file.DeleteAsync();
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

            await _cacheFolderSemaphore.WaitAsync();

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
                _cacheFolder = await ApplicationData.Current.TemporaryFolder.CreateFolderAsync(_cacheFolderName, CreationCollisionOption.OpenIfExists);
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
                await ForceInitialiseAsync();
            }

            return _cacheFolder;
        }
    }
}
