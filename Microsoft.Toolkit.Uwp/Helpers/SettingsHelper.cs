// ******************************************************************
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
using Windows.Storage;

namespace Extensions
{
    /// <summary>
    /// This class provides static helper methods for Local/Roaming Settings.
    /// </summary>
    public static class SettingsHelper
    {
        /// <summary>
        /// Determines if value with the given key exists.
        /// </summary>
        /// <param name="key">
        /// The specified key.
        /// </param>
        /// <param name="useRoaming">
        /// True if RoamingSettings should be used instead of LocalSettings.
        /// </param>
        /// <returns>
        /// Boolean value depending on value existance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the key is null or empty.
        /// </exception>
        public static bool Contains(
            string key,
            bool useRoaming = false)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return GetSettings(useRoaming).Values.ContainsKey(key);
        }

        /// <summary>
        /// Returns object stored under given key.
        /// </summary>
        /// <param name="key">
        /// The specified key.
        /// </param>
        /// <param name="useRoaming">
        /// true if RoamingSettings should be used instead of LocalSettings.
        /// </param>
        /// <returns>
        /// object if that exists in settings, otherwise null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the key is null or empty.
        /// </exception>
        public static object Read(
            string key,
            bool useRoaming = false)
            => Contains(key, useRoaming) ? GetSettings(useRoaming).Values[key] : null;

        /// <summary>
        /// Removes value with given key.
        /// </summary>
        /// <param name="key">
        /// The specified key.
        /// </param>
        /// <param name="useRoaming">
        /// true if RoamingSettings should be used instead of LocalSettings.
        /// </param>
        /// <returns>
        /// Boolean informing if the value has been removed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the key is null or empty.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Exception thrown if something here is not supported.
        /// </exception>
        public static bool Remove(
            string key,
            bool useRoaming = false)
            => GetSettings(useRoaming).Values.Remove(key);

        /// <summary>
        /// Saves value under given key.
        /// </summary>
        /// <param name="key">
        /// The specified key.
        /// </param>
        /// <param name="value">
        /// Given value.
        /// </param>
        /// <param name="useRoaming">
        /// true if RoamingSettings should be used instead of LocalSettings.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Exception thrown if the value is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if key or value are null or key is empty.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Exception thrown if value is not supported.
        /// </exception>
        public static void Save(
            string key,
            object value,
            bool useRoaming = false)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            GetSettings(useRoaming).Values[key] = value;
        }

        /// <summary>
        /// Gets value with given key or default one.
        /// </summary>
        /// <param name="key">
        /// The specified key.
        /// </param>
        /// <param name="defaultValue">
        /// Default value if value in settings doesn't exit.
        /// </param>
        /// <param name="useRoaming">
        /// true if RoamingSettings should be used instead of LocalSettings.
        /// </param>
        /// <returns>
        /// value found under given key in settings or a provided defualt one.
        /// </returns>
        /// <typeparam name="T">
        /// type of velue saved in settings.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if key is null or empty.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// Exception thrown if stored value is different type than expected.
        /// </exception>
        public static T ReadOrDefault<T>(
            string key,
            T defaultValue,
            bool useRoaming = false)
            => Contains(key, useRoaming) ? (T)GetSettings(useRoaming).Values[key] : defaultValue;

        /// <summary>
        /// Gets value with given key and creates setting if that doesn't exist.
        /// </summary>
        /// <param name="key">
        /// The specified key.
        /// </param>
        /// <param name="createdValue">
        /// value to save if suitable one not found in settings.
        /// </param>
        /// <param name="useRoaming">
        /// true if RoamingSettings should be used instead of LocalSettings.
        /// </param>
        /// <returns>
        /// value found under given key in settings or a created one.
        /// </returns>
        /// <typeparam name="T">
        /// type of velue saved in settings.
        /// </typeparam>
        /// <exception cref="ArgumentException">
        /// Exception thrown if the value is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if key or value are null or key is empty.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// Exception thrown if stored value is different type than expected.
        /// </exception>
        public static T ReadOrCreate<T>(string key, T createdValue, bool useRoaming = false)
        {
            if (Contains(key))
            {
                return (T)GetSettings(useRoaming).Values[key];
            }

            Save(key, createdValue);
            return createdValue;
        }

        /// <summary>
        /// Gets and removes value with given key if that exists.
        /// </summary>
        /// <param name="key">
        /// The specified key.
        /// </param>
        /// <param name="useRoaming">
        /// true if RoamingSettings should be used instead of LocalSettings.
        /// </param>
        /// <returns>
        /// object with the given key or null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the key is null or empty.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Exception thrown if something here is not supported.
        /// </exception>
        public static object ReadAndRemove(string key, bool useRoaming = false)
        {
            object value = Read(key);
            if (value != null)
            {
                Remove(key);
            }

            return value;
        }

        // A helper method used to choose between Roaming/Local settings
        private static ApplicationDataContainer GetSettings(bool useRoaming) =>
            useRoaming ? ApplicationData.Current.RoamingSettings : ApplicationData.Current.LocalSettings;

    }
}
