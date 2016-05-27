using System;
using System.Threading.Tasks;

using Windows.Storage;

namespace Microsoft.Windows.Toolkit
{
    public static partial class Helpers
    {
        /// <summary>
        /// Saves a string value to a <see cref="StorageFile"/> in the given <see cref="StorageFolder"/>.
        /// </summary>
        /// <param name="fileLocation">
        /// The <see cref="StorageFolder"/> to save the file in.
        /// </param>
        /// <param name="text">
        /// The <see cref="string"/> value to save to the file.
        /// </param>
        /// <param name="fileName">
        /// The <see cref="string"/> name for the file.
        /// </param>
        /// <param name="fileExtension">
        /// The extension for the file. Default is .txt.
        /// </param>
        /// <returns>
        /// Returns the saved <see cref="StorageFile"/> containing the text.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the file location or file name are null or empty.
        /// </exception>
        public static async Task<StorageFile> SaveTextToFileAsync(
            StorageFolder fileLocation,
            string text,
            string fileName,
            string fileExtension = ".txt")
        {
            if (fileLocation == null)
            {
                throw new ArgumentNullException(nameof(fileLocation));
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var storageFile = await fileLocation.CreateFileAsync($"{fileName}{fileExtension}");
            await FileIO.WriteTextAsync(storageFile, text);

            return storageFile;
        }

        /// <summary>
        /// Gets a string value from a <see cref="StorageFile"/> based on a file path string.
        /// </summary>
        /// <param name="filePath">
        /// The <see cref="string"/> file path.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="string"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the <paramref name="filePath"/> is null or empty.
        /// </exception>
        public static async Task<string> GetTextFromFileAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            var file = await StorageFile.GetFileFromPathAsync(filePath);
            return await GetTextFromFileAsync(file);
        }

        /// <summary>
        /// Gets a string value from a <see cref="StorageFile"/>.
        /// </summary>
        /// <param name="file">
        /// The <see cref="StorageFile"/>.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="string"/> value.
        /// </returns>
        public static async Task<string> GetTextFromFileAsync(StorageFile file)
        {
            if (file == null)
            {
                return null;
            }

            var textContent = await FileIO.ReadTextAsync(file);
            return textContent;
        }
    }
}