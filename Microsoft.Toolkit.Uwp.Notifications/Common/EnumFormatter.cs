// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

#nullable enable

namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// A helper class that can be used to format <see cref="Enum"/> values.
    /// </summary>
    internal static class EnumFormatter
    {
        /// <summary>
        /// Returns a <see cref="string"/> representation of an enum value with pascal casing.
        /// </summary>
        /// <typeparam name="T">The <see cref="Enum"/> type to format.</typeparam>
        /// <param name="value">The <typeparamref name="T"/> value to format.</param>
        /// <returns>The pascal case <see cref="string"/> representation of <paramref name="value"/>.</returns>
        public static string? ToPascalCaseString<T>(this T? value)
            where T : unmanaged, Enum
        {
            if (value is null)
            {
                return null;
            }

            return ToPascalCaseString(value.Value);
        }

        /// <summary>
        /// Returns a <see cref="string"/> representation of an enum value with pascal casing.
        /// </summary>
        /// <typeparam name="T">The <see cref="Enum"/> type to format.</typeparam>
        /// <param name="value">The <typeparamref name="T"/> value to format.</param>
        /// <returns>The pascal case <see cref="string"/> representation of <paramref name="value"/>.</returns>
        public static string? ToPascalCaseString<T>(this T value)
            where T : unmanaged, Enum
        {
            string? text = value.ToString();

            if (text is null or { Length: 0 })
            {
                return text;
            }

            if (text is { Length: 1 })
            {
                return text.ToLowerInvariant();
            }

            return $"{char.ToLowerInvariant(text[0])}{text.Substring(1)}";
        }
    }
}