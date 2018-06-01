// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.LinkedIn
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
                results = JsonConvert.DeserializeObject<List<T>>(data);
            }
            catch (JsonSerializationException)
            {
                T linkedInResult = JsonConvert.DeserializeObject<T>(data);
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
            return JsonConvert.SerializeObject(dataToShare, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
