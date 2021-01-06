// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    internal class JsonObjectSerializer : IObjectSerializer
    {
        public string Serialize<T>(T value)
        {
            using var sr = new MemoryStream();

            new DataContractJsonSerializer(typeof(T)).WriteObject(sr, value);
            var json = sr.ToArray();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        public T Deserialize<T>(string value)
        {
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(value));

            return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
        }
    }
}
