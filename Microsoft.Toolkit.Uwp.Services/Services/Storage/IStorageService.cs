using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Services.Services.Storage
{
    /// <summary>
    /// Service used to store data
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Retrieve single item by its key
        /// </summary>
        /// <typeparam name="T">Type of object retrieved</typeparam>
        /// <param name="key">Key of the object</param>
        /// <param name="default">Default value of the object</param>
        /// <returns>The T object</returns>
        T Read<T>(string key, T @default = default(T));

        /// <summary>
        /// Save single item by its key
        /// </summary>
        /// <typeparam name="T">Type of object saved</typeparam>
        /// <param name="key">Key of the value saved</param>
        /// <param name="value">Object to save</param>
        void Save<T>(string key, T value);

        /// <summary>
        /// Retrieve object from file
        /// </summary>
        /// <typeparam name="T">Type of object retrieved</typeparam>
        /// <param name="filePath">Path to the file that contains the object</param>
        /// <param name="default">Default value of the object</param>
        /// <returns>Waiting task until completion with the object in the file</returns>
        Task<T> ReadFileAsync<T>(string filePath, T @default = default(T));

        /// <summary>
        /// Save object inside file
        /// </summary>
        /// <typeparam name="T">Type of object saved</typeparam>
        /// <param name="filePath">Path to the file that will contain the object</param>
        /// <param name="value">Object to save</param>
        /// <returns>Waiting task until completion</returns>
        Task SaveFileAsync<T>(string filePath, T value);
    }
}
