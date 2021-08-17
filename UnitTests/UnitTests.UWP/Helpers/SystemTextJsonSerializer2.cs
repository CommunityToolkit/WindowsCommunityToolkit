// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;
using Microsoft.Toolkit.Helpers;

namespace UnitTests.Helpers
{
    /// <summary>
    /// Example class of writing a new <see cref="IObjectSerializer"/> that uses System.Text.Json.
    /// Based on <see cref="Microsoft.Toolkit.Helpers.IObjectSerializer"/>.
    /// </summary>
    internal class SystemTextJsonSerializer2 : IObjectSerializer
    {
        public T Deserialize<T>(string value) => JsonSerializer.Deserialize<T>(value);

        public string Serialize<T>(T value) => JsonSerializer.Serialize(value);
    }
}