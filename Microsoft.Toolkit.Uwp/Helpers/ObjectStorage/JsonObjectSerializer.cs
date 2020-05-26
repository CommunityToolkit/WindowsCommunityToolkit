// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    internal class JsonObjectSerializer : IObjectSerializer
    {
        public string Serialize<T>(T value) => JsonConvert.SerializeObject(value);

        public T Deserialize<T>(string value) => JsonConvert.DeserializeObject<T>(value);
    }
}
