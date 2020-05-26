// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable enable

namespace Microsoft.Toolkit.Extensions
{
    /// <summary>
    /// Helpers for working with types.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// The mapping of built-in types to their simple representation.
        /// </summary>
        private static readonly IReadOnlyDictionary<Type, string> BuiltInTypesMap = new Dictionary<Type, string>
        {
            [typeof(bool)] = "bool",
            [typeof(byte)] = "byte",
            [typeof(sbyte)] = "sbyte",
            [typeof(short)] = "short",
            [typeof(ushort)] = "ushort",
            [typeof(char)] = "char",
            [typeof(int)] = "int",
            [typeof(uint)] = "uint",
            [typeof(float)] = "float",
            [typeof(long)] = "long",
            [typeof(ulong)] = "ulong",
            [typeof(double)] = "double",
            [typeof(decimal)] = "decimal",
            [typeof(object)] = "object",
            [typeof(string)] = "string"
        };

        /// <summary>
        /// A thread-safe mapping of precomputed string representation of types.
        /// </summary>
        private static readonly ConditionalWeakTable<Type, string> DisplayNames = new ConditionalWeakTable<Type, string>();

        /// <summary>
        /// Returns a simple string representation of a type.
        /// </summary>
        /// <param name="type">The input type.</param>
        /// <returns>The string representation of <paramref name="type"/>.</returns>
        [Pure]
        public static string ToTypeString(this Type type)
        {
            // Local function to create the formatted string for a given type
            static string FormatDisplayString(Type type)
            {
                // Primitive types use the keyword name
                if (BuiltInTypesMap.TryGetValue(type, out string? typeName))
                {
                    return typeName!;
                }

                // Generic types
                if (type.IsGenericType &&
                    type.FullName is { } fullName &&
                    fullName.Split('`') is { } tokens &&
                    tokens.Length > 0 &&
                    tokens[0] is { } genericName &&
                    genericName.Length > 0)
                {
                    var typeArguments = type.GetGenericArguments().Select(FormatDisplayString);

                    // Nullable<T> types are displayed as T?
                    var genericType = type.GetGenericTypeDefinition();
                    if (genericType == typeof(Nullable<>))
                    {
                        return $"{typeArguments.First()}?";
                    }

                    // ValueTuple<T1, T2> types are displayed as (T1, T2)
                    if (genericType == typeof(ValueTuple<>) ||
                        genericType == typeof(ValueTuple<,>) ||
                        genericType == typeof(ValueTuple<,,>) ||
                        genericType == typeof(ValueTuple<,,,>) ||
                        genericType == typeof(ValueTuple<,,,,>) ||
                        genericType == typeof(ValueTuple<,,,,,>) ||
                        genericType == typeof(ValueTuple<,,,,,,>) ||
                        genericType == typeof(ValueTuple<,,,,,,,>))
                    {
                        return $"({string.Join(", ", typeArguments)})";
                    }

                    // Standard generic types are displayed as Foo<T>
                    return $"{genericName}<{string.Join(", ", typeArguments)}>";
                }

                // Array types are displayed as Foo[]
                if (type.IsArray)
                {
                    var elementType = type.GetElementType();
                    var rank = type.GetArrayRank();

                    return $"{FormatDisplayString(elementType)}[{new string(',', rank - 1)}]";
                }

                return type.ToString();
            }

            // Atomically get or build the display string for the current type.
            // Manually create a static lambda here to enable caching of the generated closure.
            // This is a workaround for the missing caching for method group conversions, and should
            // be removed once this issue is resolved: https://github.com/dotnet/roslyn/issues/5835.
            return DisplayNames.GetValue(type, t => FormatDisplayString(t));
        }
    }
}
