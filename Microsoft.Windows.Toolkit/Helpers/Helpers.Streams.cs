using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace Microsoft.Windows.Toolkit
{
    public static partial class Helpers
    {
        /// <summary>
        /// Get the response stream returned by a HTTP get request.
        /// </summary>
        /// <param name="uri">Uri to request.</param>
        /// <returns>Response stream</returns>
        public static async Task<IRandomAccessStream> GetHTTPStreamAsync(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException();
            }

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(uri))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }

                    var outputStream = new InMemoryRandomAccessStream();

                    using (var content = response.Content)
                    {
                        var responseBuffer = await content.ReadAsBufferAsync();
                        await outputStream.WriteAsync(responseBuffer);

                        outputStream.Seek(0);

                        return outputStream;
                    }
                }
            }
        }

        /// <summary>
        /// Return a stream to a specified file from the installation folder.
        /// </summary>
        /// <param name="fullFileName">Full name of the file to open. Can contains subfolders.</param>
        /// <param name="accessMode">File access mode. Default is read.</param>
        /// <returns>File stream</returns>
        public static async Task<IRandomAccessStream> GetPackagedFileAsync(string fullFileName, FileAccessMode accessMode = FileAccessMode.Read)
        {
            StorageFolder workingFolder = Package.Current.InstalledLocation;

            return await GetFileAsync(fullFileName, accessMode, workingFolder);
        }

        /// <summary>
        /// Return a stream to a specified file from the application local folder.
        /// </summary>
        /// <param name="fullFileName">Full name of the file to open. Can contains subfolders.</param>
        /// <param name="accessMode">File access mode. Default is read.</param>
        /// <returns>File stream</returns>
        public static async Task<IRandomAccessStream> GetLocalFileAsync(string fullFileName, FileAccessMode accessMode = FileAccessMode.Read)
        {
            StorageFolder workingFolder = ApplicationData.Current.LocalFolder;

            return await GetFileAsync(fullFileName, accessMode, workingFolder);
        }

        private static async Task<IRandomAccessStream> GetFileAsync(string fullFileName, FileAccessMode accessMode, StorageFolder workingFolder)
        {
            var fileName = Path.GetFileName(fullFileName);
            workingFolder = await ExtractSubFolder(fullFileName, workingFolder);

            var file = await workingFolder.GetFileAsync(fileName);

            return await file.OpenAsync(accessMode);
        }

        private static async Task<StorageFolder> ExtractSubFolder(string fullFileName, StorageFolder workingFolder)
        {
            var folderName = Path.GetDirectoryName(fullFileName);

            if (!string.IsNullOrEmpty(folderName) && folderName != @"\")
            {
                return await workingFolder.GetFolderAsync(folderName);
            }

            return workingFolder;
        }

        /// <summary>
        /// Test if a file exists in the application local folder.
        /// </summary>
        /// <param name="fullFileName">Full name of the file to open. Can contains subfolders.</param>
        /// <returns>True if file exists.</returns>
        public static async Task<bool> IsLocalFileExistsAsync(string fullFileName)
        {
            StorageFolder workingFolder = ApplicationData.Current.LocalFolder;
            var fileName = Path.GetFileName(fullFileName);
            workingFolder = await ExtractSubFolder(fullFileName, workingFolder);

            var item = await workingFolder.TryGetItemAsync(fileName);

            if (item == null)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Read stream content as a string.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <param name="encoding">Encoding to use. Can be set to null (ASCII will be used in this case).</param>
        /// <returns>Stream content.</returns>
        public static async Task<string> ReadTextAsync(this IRandomAccessStream stream, Encoding encoding = null)
        {
            var reader = new DataReader(stream.GetInputStreamAt(0));
            await reader.LoadAsync((uint)stream.Size);

            var bytes = new byte[stream.Size];
            reader.ReadBytes(bytes);

            if (encoding == null)
            {
                // Let's revert back to Unicode
                encoding = Encoding.ASCII;
            }

            return encoding.GetString(bytes);
        }
    }
}
