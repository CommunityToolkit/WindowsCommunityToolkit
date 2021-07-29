// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text.Json;

namespace Microsoft.Toolkit.Services.LinkedIn
{
    /// <summary>
    /// Parse results into strong type.
    /// </summary>
    /// <typeparam name="T">Type to parse into.</typeparam>
    public class LinkedInParser<T>
    {
        /// <summary>
        /// Take string data and parse into strong data type.
        /// </summary>
        /// <param name="data">String data.</param>
        /// <returns>Returns strong type.</returns>
        public IEnumerable<T> Parse(string data)
        {
            List<T> results;

            try
            {
                results = JsonSerializer.Deserialize<List<T>>(data);
            }
            catch (JsonException)
            {
                T linkedInResult = JsonSerializer.Deserialize<T>(data);
                results = new List<T> { linkedInResult };
            }

            return results;
        }

        /// <summary>
        /// Take strong type and return corresponding JSON string.
        /// </summary>
        /// <param name="dataToShare">Strong typed instance.</param>
        /// <returns>Returns string data.</returns>
        public string Parse(T dataToShare)
        {
            return JsonSerializer.Serialize(dataToShare, typeof(T), new JsonSerializerOptions { IgnoreNullValues = true });
        }
    }
}
