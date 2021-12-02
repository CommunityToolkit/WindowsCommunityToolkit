// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

#if USING_TAEF
using WEX.Logging.Interop;
#endif

namespace UITests.Tests
{
    /// <summary>
    /// Helper class to access some VisualTree info through our communication pipeline to the host app
    /// using TestAssembly.SendMessageToApp.
    /// </summary>
    internal static class VisualTreeHelper
    {
        private static JsonSerializerOptions SerializerOptions { get; } = new JsonSerializerOptions(JsonSerializerDefaults.General)
        {
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
        };

        /// <summary>
        /// Looks for the specified element by name and retrieves the specified property path.
        /// </summary>
        /// <param name="name">Name of element to search for.</param>
        /// <param name="property">Name of property to retrieve from element.</param>
        /// <typeparam name="T">Type of data to serialize result back as.</typeparam>
        /// <returns>Retrieved value or default.</returns>
        public static async Task<T> FindElementPropertyAsync<T>(string name, string property)
        {
            try
            {
                var str = await TestAssembly.FindElementProperty(name, property);

                if (!string.IsNullOrEmpty(str))
                {
                    Log.Comment("[Harness] VisualTreeHelper.FindElementPropertyAsync - Received: {0}", str);

                    try
                    {
                        return JsonSerializer.Deserialize<T>(str, SerializerOptions);
                    }
                    catch
                    {
                        Log.Error("[Harness] VisualTreeHelper.FindElementPropertyAsync - Couldn't deserialize result as {0}", typeof(T));
                    }
                }
            }
            catch
            {
                Log.Error("[Harness] VisualTreeHelper: Error trying to retrieve property {0} from element named {1}.", property, name);
            }

            return default(T);
        }
    }
}
