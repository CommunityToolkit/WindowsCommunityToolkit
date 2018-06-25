namespace Microsoft.Toolkit.Services.Core
{
    /// <summary>
    /// This interface store the key value
    /// </summary>
    public interface IStorageManager
    {
        /// <summary>
        /// Gets the key value
        /// </summary>
        /// <param name="key"> Token value </param>
        /// <returns> Returns a string value</returns>
        string Get(string key);

        /// <summary>
        /// Sets the key value
        /// </summary>
        /// <param name="key"> Token key </param>
        /// <param name="value"> String value </param>
        void Set(string key, string value);
    }
}
