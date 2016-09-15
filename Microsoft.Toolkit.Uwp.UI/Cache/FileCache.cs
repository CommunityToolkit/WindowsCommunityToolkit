using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Provides methods and tools to cache files in a folder
    /// </summary>
    public class FileCache : CacheBase<StorageFile>
    {
        static FileCache()
        {
            FileCacheInstance = new FileCache();
        }

        /// <summary>
        /// Gets instance of FileCache. Exposing it as static property will allow inhertance and polymorphism while
        /// exposing the underlying object and its functionality through this property,
        /// </summary>
        public static FileCache FileCacheInstance { get; private set; }

        /// <summary>
        /// Cache specific hooks to proccess items from http response
        /// </summary>
        /// <param name="stream">input stream</param>
        /// <returns>awaitable task</returns>
        protected override async Task<StorageFile> InitializeTypeAsync(IRandomAccessStream stream)
        {
            // nothing to do in this instance;
            return null;
        }

        /// <summary>
        /// Cache specific hooks to proccess items from http response
        /// </summary>
        /// <param name="baseFile">storage file</param>
        /// <returns>awaitable task</returns>
        protected override async Task<StorageFile> InitializeTypeAsync(StorageFile baseFile)
        {
            return baseFile;
        }
    }
}
