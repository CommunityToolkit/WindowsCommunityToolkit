using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;

namespace Microsoft.Windows.Toolkit.UI
{
    /// <summary>
    /// Provides methods and tools to cache images in a temporary local folder
    /// </summary>
    public  static class ImageCache
    {
        const string CacheFolderName = "ImageCache";
        
        private static readonly Dictionary<string, Task> _concurrentTasks = new Dictionary<string, Task>();

        private static StorageFolder _cacheFolder;
        private static readonly SemaphoreSlim _cacheFolderSemaphore = new SemaphoreSlim(1);

        static ImageCache()
        {
            CacheDuration = TimeSpan.FromHours(24);
        }

        /// <summary>
        /// Define the life duration of every cache entry.
        /// </summary>
        public static TimeSpan CacheDuration { get; set; }
        
        /// <summary>
        /// call this method to clear the entire cache.
        /// </summary>
        /// <param name="duration">Use this parameter to define a timespan from now to select cache entries to delete.</param>
        /// <returns></returns>
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
        }

        /// <summary>
        /// Load a specific image from the cache. If the image is not in the cache, ImageCache will try to download and store it.
        /// </summary>
        /// <param name="uri">Uri of the image.</param>
        /// <returns></returns>
        public static async Task<BitmapImage> LoadFromCacheAsync(Uri uri)
        {
            Task busy;
            string key = BuildFileName(uri);

            lock (_concurrentTasks)
            {
                if (_concurrentTasks.ContainsKey(key))
                {
                    busy = _concurrentTasks[key];
                }
                else
                {
                    busy = EnsureFileAsync(uri);
                    _concurrentTasks.Add(key, busy);
                }
            }

            try
            {
                await busy;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                lock (_concurrentTasks)
                {
                    if (_concurrentTasks.ContainsKey(key))
                    {
                        _concurrentTasks.Remove(key);
                    }
                }
            }

            return CreateBitmapImage(key);
        }

        private static BitmapImage CreateBitmapImage(string fileName)
        {
            return new BitmapImage(new Uri($"ms-appdata:///temp/{CacheFolderName}/{fileName}"));
        }

        private static async Task EnsureFileAsync(Uri uri)
        {
            DateTime expirationDate = DateTime.Now.Subtract(CacheDuration);

            var folder = await GetCacheFolderAsync();

            string fileName = BuildFileName(uri);
            var baseFile = await folder.TryGetItemAsync(fileName) as StorageFile;
            if (await IsFileOutOfDate(baseFile, expirationDate))
            {
                baseFile = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                if (!await Helpers.DownloadHTTPStreamAsync(uri, baseFile))
                {
                    await baseFile.DeleteAsync();
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
                catch { }
                finally
                {
                    _cacheFolderSemaphore.Release();
                }
            }
            return _cacheFolder;
        }

        private static string BuildFileName(Uri uri)
        {
            ulong uriHash = CreateHash64(uri);

            return $"{uriHash}.jpg";
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
