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
        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static FileCache _instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static FileCache Instance => _instance ?? (_instance = new FileCache());

        /// <summary>
        /// Cache specific hooks to proccess items from http response
        /// </summary>
        /// <param name="stream">input stream</param>
        /// <returns>awaitable task</returns>
        protected override Task<StorageFile> InitializeTypeAsync(IRandomAccessStream stream)
        {
            // nothing to do in this instance;
            return null;
        }

        /// <summary>
        /// Cache specific hooks to proccess items from http response
        /// </summary>
        /// <param name="baseFile">storage file</param>
        /// <returns>awaitable task</returns>
        protected override Task<StorageFile> InitializeTypeAsync(StorageFile baseFile)
        {
            return Task.Run(() => baseFile);
        }
    }
}
