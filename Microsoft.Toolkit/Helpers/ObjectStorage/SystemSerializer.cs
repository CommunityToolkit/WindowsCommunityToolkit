// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;

namespace Microsoft.Toolkit.Helpers
{
    /// <summary>
    /// A bare-bones serializer which knows how to deal with primitive types and strings only.
    /// It is recommended for more complex scenarios to implement your own <see cref="IObjectSerializer"/> based on System.Text.Json, Newtonsoft.Json, or DataContractJsonSerializer see https://aka.ms/wct/storagehelper-migration
    /// </summary>
    public class SystemSerializer : IObjectSerializer
    {
        /// <summary>
        /// Take a primitive value from storage and return it as the requested type using the <see cref="Convert.ChangeType(object, Type)"/> API.
        /// </summary>
        /// <typeparam name="T">Type to convert value to.</typeparam>
        /// <param name="value">Value from storage to convert.</param>
        /// <returns>Deserialized value or default value.</returns>
        public T Deserialize<T>(string value)
        {
            var type = typeof(T);
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsPrimitive || type == typeof(string))
            {
                return (T)Convert.ChangeType(value, type);
            }

            throw new NotSupportedException("This serializer can only handle primitive types and strings. Please implement your own IObjectSerializer for more complex scenarios.");
        }

        /// <summary>
        /// Returns the value so that it can be serialized directly.
        /// </summary>
        /// <typeparam name="T">Type to serialize from.</typeparam>
        /// <param name="value">Value to serialize.</param>
        /// <returns>String representation of value.</returns>
        public string? Serialize<T>(T value)
        {
            return value?.ToString();
        }
    }
}