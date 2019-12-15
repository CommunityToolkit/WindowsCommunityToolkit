// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Microsoft.Toolkit.Parsers.Markdown
{
    /// <summary>
    /// Filters parts of a string.
    /// </summary>
    public class StringFilter
    {
        // private readonly ReadOnlyMemory<char>[] spans;
        private readonly string str;

        // public char this[int index]
        // {
        //     get
        //     {
        //         if (index < 0)
        //         {
        //             throw new IndexOutOfRangeException($"The provided {nameof(index)} ({index}) was < 0");
        //         }
        //
        //         int position = 0;
        //         for (int i = 0; i < spans.Length; i++)
        //         {
        //             if (index < position + spans[i].Length)
        //             {
        //                 return spans[i].Span[position - index];
        //             }
        //
        //             position += spans[i].Length;
        //         }
        //
        //         throw new IndexOutOfRangeException($"The provided {nameof(index)} ({index}) was >= then the length ({position})");
        //     }
        // }

        /// <summary>
        /// Gets the number of characters in the current System.String object.
        /// </summary>
        public int Length => this.str.Length;

        /// <summary>
        /// Concatenates three specified instances of System.String.
        /// </summary>
        /// <param name="str0">The first string to concatenate.</param>
        /// <param name="str1">The second string to concatenate.</param>
        /// <param name="str2">The third string to concatenate.</param>
        /// <returns>The concatenation of str0, str1, and str2.</returns>
        public static StringFilter Concat(string str0, string str1, string str2)
        {
            return string.Concat(str0, str1, str2);
        }

        /// <summary>
        /// Concatenates the members of an System.Collections.Generic.IEnumerable`1 implementation.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the members of values.
        /// interface.
        /// </typeparam>
        /// <param name="values">
        /// A collection object that implements the System.Collections.Generic.IEnumerable`1
        /// </param>
        /// <returns>
        /// The concatenated members in values.
        /// </returns>
        public static StringFilter Concat<T>(IEnumerable<T> values) => string.Concat(values);

        /// <summary>
        /// Concatenates the elements of a specified System.String array.
        /// </summary>
        /// <param name="values">An array of string instances.</param>
        /// <returns>The concatenated elements of values.</returns>
        public static StringFilter Concat(params string[] values) => string.Concat(values);

        /// <summary>
        /// Concatenates four specified instances of System.String.
        /// </summary>
        /// <param name="str0">The first string to concatenate.</param>
        /// <param name="str1">The second string to concatenate.</param>
        /// <param name="str2">The third string to concatenate.</param>
        /// <param name="str3">The fourth string to concatenate.</param>
        /// <returns>The concatenation of str0, str1, str2, and str3.</returns>
        public static StringFilter Concat(string str0, string str1, string str2, string str3) => string.Concat(str0, str1, str2, str3);

        /// <summary>
        /// Concatenates two specified instances of System.String.
        /// </summary>
        /// <param name="str0">The first string to concatenate.</param>
        /// <param name="str1">The second string to concatenate.</param>
        /// <returns>The concatenation of str0 and str1.</returns>
        public static StringFilter Concat(string str0, string str1) => string.Concat(str0, str1);

        /// <summary>
        /// Concatenates the string representations of three specified objects.
        /// </summary>
        /// <param name="arg0">The first object to concatenate.</param>
        /// <param name="arg1">The second object to concatenate.</param>
        /// <param name="arg2">The third object to concatenate.</param>
        /// <returns>The concatenated string representations of the values of arg0, arg1, and arg2.</returns>
        public static StringFilter Concat(object arg0, object arg1, object arg2) => string.Concat(arg0, arg1, arg2);

        /// <summary>
        /// Concatenates the string representations of two specified objects.
        /// </summary>
        /// <param name="arg0">The first object to concatenate.</param>
        /// <param name="arg1">The second object to concatenate.</param>
        /// <returns>The concatenated string representations of the values of arg0 and arg1.</returns>
        public static StringFilter Concat(object arg0, object arg1) => string.Concat(arg0, arg1);

        /// <summary>
        /// Creates the string representation of a specified object.
        /// </summary>
        /// <param name="arg0">The object to represent, or null.</param>
        /// <returns>
        /// The string representation of the value of arg0, or System.String.Empty if arg0
        /// is null.
        /// </returns>
        public static StringFilter Concat(object arg0) => string.Concat(arg0);

        /// <summary>
        /// Concatenates the members of a constructed System.Collections.Generic.IEnumerable`1
        /// collection of type System.String.</summary>
        /// <param name="values">
        /// A collection object that implements System.Collections.Generic.IEnumerable`1
        /// and whose generic type argument is System.String.</param>
        /// <returns>
        /// The concatenated strings in values, or System.String.Empty if values is an empty
        /// IEnumerable(Of String).
        /// </returns>
        public static StringFilter Concat(IEnumerable<string> values) => string.Concat(values);

        /// <summary>
        /// Concatenates the string representations of the elements in a specified System.Object
        /// array.
        /// </summary>
        /// <param name="args">An object array that contains the elements to concatenate.</param>
        /// <returns>The concatenated string representations of the values of the elements in args.</returns>
        public static StringFilter Concat(params object[] args) => string.Concat(args);

        /// <summary>
        /// Determines whether the end of this string instance matches the specified string
        /// when compared using the specified comparison option.</summary>
        /// <param name="value">The string to compare to the substring at the end of this instance.</param>
        /// <param name="comparisonType">One of the enumeration values that determines how this string and value are compared.</param>
        /// <returns>true if the value parameter matches the end of this string; otherwise, false.</returns>
        public bool EndsWith(string value, StringComparison comparisonType) => this.str.EndsWith(value, comparisonType);

        /// <summary>
        /// Determines whether the end of this string instance matches the specified string
        /// when compared using the specified culture.</summary>
        /// <param name="value">The string to compare to the substring at the end of this instance.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <param name="culture">Cultural information that determines how this instance and value are compared.
        /// If culture is null, the current culture is used.</param>
        /// <returns>true if the value parameter matches the end of this string; otherwise, false.</returns>
        public bool EndsWith(string value, bool ignoreCase, CultureInfo culture) => this.str.EndsWith(value, ignoreCase, culture);

        /// <summary>
        /// Determines whether the end of this string instance matches the specified string.
        /// </summary>
        /// <param name="value">The string to compare to the substring at the end of this instance.</param>
        /// <returns>true if value matches the end of this instance; otherwise, false.</returns>
        public bool EndsWith(string value) => this.str.EndsWith(value);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified string
        /// in the current System.String object. Parameters specify the starting search position
        /// in the current string and the type of search to use for the specified string.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>
        /// The zero-based index position of the value parameter from the start of the current
        /// instance if that string is found, or -1 if it is not. If value is System.String.Empty,
        /// the return value is startIndex.
        /// </returns>
        public int IndexOf(string value, int startIndex, StringComparison comparisonType) => this.str.IndexOf(value, startIndex, comparisonType);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified string
        /// in the current System.String object. A parameter specifies the type of search
        /// to use for the specified string.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>
        /// The index position of the value parameter if that string is found, or -1 if it
        /// is not. If value is System.String.Empty, the return value is 0.
        /// </returns>
        public int IndexOf(string value, StringComparison comparisonType) => this.str.IndexOf(value, comparisonType);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified string
        /// in this instance. The search starts at a specified character position and examines
        /// a specified number of character positions.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <returns>
        /// The zero-based index position of value from the start of the current instance
        /// if that string is found, or -1 if it is not. If value is System.String.Empty,
        /// the return value is startIndex.
        /// </returns>
        public int IndexOf(string value, int startIndex, int count) => this.str.IndexOf(value, startIndex, count);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified string
        /// in this instance.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <returns>
        /// The zero-based index position of value if that string is found, or -1 if it is
        /// not. If value is System.String.Empty, the return value is 0.
        /// </returns>
        public int IndexOf(string value) => this.str.IndexOf(value);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified character
        /// in this instance. The search starts at a specified character position and examines
        /// a specified number of character positions.
        /// </summary>
        /// <param name="value">A Unicode character to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <returns>
        /// The zero-based index position of value from the start of the string if that character
        /// is found, or -1 if it is not.
        /// </returns>
        public int IndexOf(char value, int startIndex, int count) => this.str.IndexOf(value, startIndex, count);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified Unicode
        /// character in this string. The search starts at a specified character position.
        /// </summary>
        /// <param name="value">A Unicode character to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <returns>
        /// The zero-based index position of value from the start of the string if that character
        /// is found, or -1 if it is not.
        /// </returns>
        public int IndexOf(char value, int startIndex) => this.str.IndexOf(value, startIndex);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified Unicode
        /// character in this string.
        /// </summary>
        /// <param name="value">A Unicode character to seek.</param>
        /// <returns>
        /// The zero-based index position of value if that character is found, or -1 if it
        /// is not.
        /// </returns>
        public int IndexOf(char value) => this.str.IndexOf(value);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified string
        /// in the current System.String object. Parameters specify the starting search position
        /// in the current string, the number of characters in the current string to search,
        /// and the type of search to use for the specified string.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>
        /// The zero-based index position of the value parameter from the start of the current
        /// instance if that string is found, or -1 if it is not. If value is System.String.Empty,
        /// the return value is startIndex.
        /// </returns>
        public int IndexOf(string value, int startIndex, int count, StringComparison comparisonType) => this.str.IndexOf(value, startIndex, count, comparisonType);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified string
        /// in this instance. The search starts at a specified character position.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <returns>
        /// The zero-based index position of value from the start of the current instance
        /// if that string is found, or -1 if it is not. If value is System.String.Empty,
        /// the return value is startIndex.
        /// </returns>
        public int IndexOf(string value, int startIndex) => this.str.IndexOf(value, startIndex);

        /// <summary>
        /// Reports the zero-based index of the first occurrence in this instance of any
        /// character in a specified array of Unicode characters.
        /// </summary>
        /// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
        /// <returns>
        /// The zero-based index position of the first occurrence in this instance where
        /// any character in anyOf was found; -1 if no character in anyOf was found.
        /// </returns>
        public int IndexOfAny(char[] anyOf) => this.str.IndexOfAny(anyOf);

        /// <summary>
        /// Reports the zero-based index of the first occurrence in this instance of any
        /// character in a specified array of Unicode characters. The search starts at a
        /// specified character position and examines a specified number of character positions.
        /// </summary>
        /// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <returns>
        /// The zero-based index position of the first occurrence in this instance where
        /// any character in anyOf was found; -1 if no character in anyOf was found.
        /// </returns>
        public int IndexOfAny(char[] anyOf, int startIndex, int count) => this.str.IndexOfAny(anyOf, startIndex, count);

        /// <summary>
        /// Reports the zero-based index of the first occurrence in this instance of any
        /// character in a specified array of Unicode characters. The search starts at a
        /// specified character position.
        /// </summary>
        /// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <returns>
        /// The zero-based index position of the first occurrence in this instance where
        /// any character in anyOf was found; -1 if no character in anyOf was found.
        /// </returns>
        public int IndexOfAny(char[] anyOf, int startIndex) => this.str.IndexOfAny(anyOf, startIndex);

        /// <summary>
        /// Reports the zero-based index of the last occurrence of a specified string within
        /// the current System.String object. The search starts at a specified character
        /// position and proceeds backward toward the beginning of the string. A parameter
        /// specifies the type of comparison to perform when searching for the specified
        /// string.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">
        /// The search starting position. The search proceeds from startIndex toward the
        /// beginning of this instance.
        /// </param>
        /// <param name="comparisonType">
        /// One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <returns>
        /// The zero-based starting index position of the value parameter if that string
        /// is found, or -1 if it is not found or if the current instance equals System.String.Empty.
        /// If value is System.String.Empty, the return value is the smaller of startIndex
        /// and the last index position in this instance.
        /// </returns>
        public int LastIndexOf(string value, int startIndex, StringComparison comparisonType) => this.str.LastIndexOf(value, startIndex, comparisonType);

        /// <summary>
        /// Reports the zero-based index position of the last occurrence of a specified string
        /// within this instance. The search starts at a specified character position and
        /// proceeds backward toward the beginning of the string for the specified number
        /// of character positions. A parameter specifies the type of comparison to perform
        /// when searching for the specified string.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">
        /// The search starting position. The search proceeds from startIndex toward the
        /// beginning of this instance.
        /// </param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>
        /// The zero-based starting index position of the value parameter if that string
        /// is found, or -1 if it is not found or if the current instance equals System.String.Empty.
        /// If value is System.String.Empty, the return value is the smaller of startIndex
        /// and the last index position in this instance.</returns>
        public int LastIndexOf(string value, int startIndex, int count, StringComparison comparisonType) => this.str.LastIndexOf(value, startIndex, count, comparisonType);

        /// <summary>
        /// Reports the zero-based index position of the last occurrence of a specified string
        /// within this instance. The search starts at a specified character position and
        /// proceeds backward toward the beginning of the string for a specified number of
        /// character positions.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">
        /// The search starting position. The search proceeds from startIndex toward the
        /// beginning of this instance.
        /// </param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <returns>
        /// The zero-based starting index position of value if that string is found, or -1
        /// if it is not found or if the current instance equals System.String.Empty. If
        /// value is System.String.Empty, the return value is the smaller of startIndex and
        /// the last index position in this instance.
        /// </returns>
        public int LastIndexOf(string value, int startIndex, int count) => this.str.LastIndexOf(value, startIndex, count);

        /// <summary>
        /// Reports the zero-based index of the last occurrence of a specified string within
        /// the current System.String object. A parameter specifies the type of search to</summary>
        /// use for the specified string.<param name="value">The string to seek.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>
        /// The zero-based starting index position of the value parameter if that string
        /// is found, or -1 if it is not. If value is System.String.Empty, the return value
        /// is the last index position in this instance.
        /// </returns>
        public int LastIndexOf(string value, StringComparison comparisonType) => this.str.LastIndexOf(value, comparisonType);

        /// <summary>
        /// Reports the zero-based index position of the last occurrence of a specified string
        /// within this instance.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <returns>
        /// The zero-based starting index position of value if that string is found, or -1
        /// if it is not. If value is System.String.Empty, the return value is the last index
        /// position in this instance.
        /// </returns>
        public int LastIndexOf(string value) => this.str.LastIndexOf(value);

        /// <summary>
        /// Reports the zero-based index position of the last occurrence of the specified
        /// Unicode character in a substring within this instance. The search starts at a
        /// specified character position and proceeds backward toward the beginning of the
        /// string for a specified number of character positions.
        /// </summary>
        /// <param name="value">The Unicode character to seek.</param>
        /// <param name="startIndex">
        /// The starting position of the search. The search proceeds from startIndex toward
        /// the beginning of this instance.
        /// </param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <returns>
        /// The zero-based index position of value if that character is found, or -1 if it
        /// is not found or if the current instance equals System.String.Empty.
        /// </returns>
        public int LastIndexOf(char value, int startIndex, int count) => this.str.LastIndexOf(value, startIndex, count);

        /// <summary>
        /// Reports the zero-based index position of the last occurrence of a specified Unicode
        /// character within this instance. The search starts at a specified character position
        /// and proceeds backward toward the beginning of the string.
        /// </summary>
        /// <param name="value">The Unicode character to seek.</param>
        /// <param name="startIndex">
        /// The starting position of the search. The search proceeds from startIndex toward
        /// the beginning of this instance.
        /// </param>
        /// <returns>
        /// The zero-based index position of value if that character is found, or -1 if it
        /// is not found or if the current instance equals System.String.Empty.
        /// </returns>
        public int LastIndexOf(char value, int startIndex) => this.str.LastIndexOf(value, startIndex);

        /// <summary>
        /// Reports the zero-based index position of the last occurrence of a specified string
        /// within this instance. The search starts at a specified character position and
        /// proceeds backward toward the beginning of the string.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">
        /// The search starting position. The search proceeds from startIndex toward the
        /// beginning of this instance.
        /// </param>
        /// <returns>
        /// The zero-based starting index position of value if that string is found, or -1
        /// if it is not found or if the current instance equals System.String.Empty. If
        /// value is System.String.Empty, the return value is the smaller of startIndex and
        /// the last index position in this instance.
        /// </returns>
        public int LastIndexOf(string value, int startIndex) => this.str.LastIndexOf(value, startIndex);

        /// <summary>
        /// Reports the zero-based index position of the last occurrence of a specified Unicode
        /// character within this instance.
        /// </summary>
        /// <param name="value">The Unicode character to seek.</param>
        /// <returns>
        /// The zero-based index position of value if that character is found, or -1 if it
        /// is not.</returns>
        public int LastIndexOf(char value) => this.str.LastIndexOf(value);

        /// <summary>
        /// Reports the zero-based index position of the last occurrence in this instance
        /// of one or more characters specified in a Unicode array.
        /// </summary>
        /// <param name="anyOf">
        /// A Unicode character array containing one or more characters to seek.
        /// </param>
        /// <returns>
        /// The index position of the last occurrence in this instance where any character
        /// in anyOf was found; -1 if no character in anyOf was found.
        /// </returns>
        public int LastIndexOfAny(char[] anyOf) => this.str.LastIndexOfAny(anyOf);

        /// <summary>
        /// Reports the zero-based index position of the last occurrence in this instance
        /// of one or more characters specified in a Unicode array. The search starts at
        /// a specified character position and proceeds backward toward the beginning of
        /// the string.
        /// </summary>
        /// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
        /// <param name="startIndex">
        /// The search starting position. The search proceeds from startIndex toward the
        /// beginning of this instance.
        /// </param>
        /// <returns>
        /// The index position of the last occurrence in this instance where any character
        /// in anyOf was found; -1 if no character in anyOf was found or if the current instance
        /// equals System.String.Empty.
        /// </returns>
        public int LastIndexOfAny(char[] anyOf, int startIndex) => this.str.LastIndexOfAny(anyOf, startIndex);

        /// <summary>
        /// Reports the zero-based index position of the last occurrence in this instance
        /// of one or more characters specified in a Unicode array. The search starts at
        /// a specified character position and proceeds backward toward the beginning of
        /// the string for a specified number of character positions.
        /// </summary>
        /// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
        /// <param name="startIndex">
        /// The search starting position. The search proceeds from startIndex toward the
        /// beginning of this instance.
        /// </param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <returns>
        /// The index position of the last occurrence in this instance where any character
        /// in anyOf was found; -1 if no character in anyOf was found or if the current instance
        /// equals System.String.Empty.
        /// </returns>
        public int LastIndexOfAny(char[] anyOf, int startIndex, int count) => this.str.LastIndexOfAny(anyOf, startIndex, count);

        /// <summary>
        /// Determines whether the beginning of this string instance matches the specified
        /// string.
        /// </summary>
        /// <param name="value">The string to compare.</param>
        /// <returns>true if value matches the beginning of this string; otherwise, false.</returns>
        public bool StartsWith(string value) => this.str.StartsWith(value);

        /// <summary>
        /// Determines whether the beginning of this string instance matches the specified
        /// string when compared using the specified culture.
        /// </summary>
        /// <param name="value">The string to compare.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <param name="culture">
        /// Cultural information that determines how this string and value are compared.
        /// If culture is null, the current culture is used.
        /// </param>
        /// <returns>
        /// true if the value parameter matches the beginning of this string; otherwise,
        /// false.
        /// </returns>
        public bool StartsWith(string value, bool ignoreCase, CultureInfo culture) => this.str.StartsWith(value, ignoreCase, culture);

        /// <summary>
        /// Determines whether the beginning of this string instance matches the specified
        /// string when compared using the specified comparison option.
        /// </summary>
        /// <param name="value">The string to compare.</param>
        /// <param name="comparisonType">One of the enumeration values that determines how this string and value are compared.</param>
        /// <returns>true if this instance begins with value; otherwise, false.</returns>
        public bool StartsWith(string value, StringComparison comparisonType) => this.str.StartsWith(value, comparisonType);

        /// <summary>
        /// Splits a string into a maximum number of substrings based on the strings in an
        /// array. You can specify whether the substrings include empty array elements.
        /// </summary>
        /// <param name="separator">
        /// A string array that delimits the substrings in this string, an empty array that
        /// contains no delimiters, or null.
        /// </param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <param name="options">
        /// System.StringSplitOptions.RemoveEmptyEntries to omit empty array elements from
        /// the array returned; or System.StringSplitOptions.None to include empty array
        /// elements in the array returned.
        /// </param>
        /// <returns>
        /// An array whose elements contain the substrings in this string that are delimited
        /// by one or more strings in separator. For more information, see the Remarks section.
        /// </returns>
        public StringFilter[] Split(string[] separator, int count, StringSplitOptions options) => this.str.Split(separator, count, options).Cast<StringFilter>().ToArray();

        /// <summary>
        /// Splits a string into substrings that are based on the characters in an array.
        /// </summary>
        /// <param name="separator">
        /// A character array that delimits the substrings in this string, an empty array
        /// that contains no delimiters, or null.
        /// </param>
        /// <returns>
        /// An array whose elements contain the substrings from this instance that are delimited
        /// by one or more characters in separator. For more information, see the Remarks
        /// section.
        /// </returns>
        public StringFilter[] Split(params char[] separator) => this.str.Split(separator).Cast<StringFilter>().ToArray();

        /// <summary>
        /// Splits a string into a maximum number of substrings based on the characters in
        /// an array. You also specify the maximum number of substrings to return.
        /// </summary>
        /// <param name="separator">
        /// A character array that delimits the substrings in this string, an empty array
        /// that contains no delimiters, or null.
        /// </param>
        /// <param name="count">
        /// The maximum number of substrings to return.
        /// </param>
        /// <returns>
        /// An array whose elements contain the substrings in this instance that are delimited
        /// by one or more characters in separator. For more information, see the Remarks
        /// section.
        /// </returns>
        public StringFilter[] Split(char[] separator, int count) => this.str.Split(separator, count).Cast<StringFilter>().ToArray();

        /// <summary>
        /// Splits a string into a maximum number of substrings based on the characters in
        /// an array.
        /// </summary>
        /// <param name="separator">
        /// A character array that delimits the substrings in this string, an empty array
        /// that contains no delimiters, or null.
        /// </param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <param name="options">
        /// System.StringSplitOptions.RemoveEmptyEntries to omit empty array elements from
        /// the array returned; or System.StringSplitOptions.None to include empty array
        /// elements in the array returned.
        /// </param>
        /// <returns>
        /// An array whose elements contain the substrings in this string that are delimited
        /// by one or more characters in separator. For more information, see the Remarks
        /// section.
        /// </returns>
        public StringFilter[] Split(char[] separator, int count, StringSplitOptions options) => this.str.Split(separator, count, options).Cast<StringFilter>().ToArray();

        /// <summary>
        /// Splits a string into substrings based on the characters in an array. You can
        /// specify whether the substrings include empty array elements.
        /// </summary>
        /// <param name="separator">
        /// A character array that delimits the substrings in this string, an empty array
        /// that contains no delimiters, or null.
        /// </param>
        /// <param name="options">
        /// System.StringSplitOptions.RemoveEmptyEntries to omit empty array elements from
        /// the array returned; or System.StringSplitOptions.None to include empty array
        /// elements in the array returned.
        /// </param>
        /// <returns>
        /// An array whose elements contain the substrings in this string that are delimited
        /// by one or more characters in separator. For more information, see the Remarks
        /// section.
        /// </returns>
        public StringFilter[] Split(char[] separator, StringSplitOptions options) => this.str.Split(separator, options).Cast<StringFilter>().ToArray();

        /// <summary>
        /// Splits a string into substrings based on the strings in an array. You can specify
        /// whether the substrings include empty array elements.
        /// </summary>
        /// <param name="separator">
        /// A string array that delimits the substrings in this string, an empty array that
        /// contains no delimiters, or null.
        /// </param>
        /// <param name="options">
        /// System.StringSplitOptions.RemoveEmptyEntries to omit empty array elements from
        /// the array returned; or System.StringSplitOptions.None to include empty array
        /// elements in the array returned.
        /// </param>
        /// <returns>
        /// An array whose elements contain the substrings in this string that are delimited
        /// by one or more strings in separator. For more information, see the Remarks section.
        /// </returns>
        public StringFilter[] Split(string[] separator, StringSplitOptions options) => this.str.Split(separator, options).Cast<StringFilter>().ToArray();

        /// <summary>
        /// Returns a new string in which all the characters in the current instance, beginning
        /// at a specified position and continuing through the last position, have been deleted.
        /// </summary>
        /// <param name="startIndex">
        /// The zero-based position to begin deleting characters.
        /// </param>
        /// <returns>A new string that is equivalent to this string except for the removed characters.</returns>
        public StringFilter Remove(int startIndex) => this.str.Remove(startIndex);

        /// <summary>
        /// Returns a new string in which a specified number of characters in the current
        /// instance beginning at a specified position have been deleted.
        /// </summary>
        /// <param name="startIndex">The zero-based position to begin deleting characters.</param>
        /// <param name="count">The number of characters to delete.</param>
        /// <returns>A new string that is equivalent to this instance except for the removed characters.</returns>
        public StringFilter Remove(int startIndex, int count) => this.str.Remove(startIndex, count);

        /// <summary>
        /// Returns a new string in which all occurrences of a specified string in the current
        /// instance are replaced with another specified string.</summary>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string to replace all occurrences of oldValue.</param>
        /// <returns>
        /// A string that is equivalent to the current string except that all instances of
        /// oldValue are replaced with newValue. If oldValue is not found in the current
        /// instance, the method returns the current instance unchanged.
        /// </returns>
        public StringFilter Replace(string oldValue, string newValue) => this.str.Replace(oldValue, newValue);

        /// <summary>
        /// Returns a new string in which all occurrences of a specified Unicode character
        /// in this instance are replaced with another specified Unicode character.
        /// </summary>
        /// <param name="oldChar">The Unicode character to be replaced.</param>
        /// <param name="newChar">The Unicode character to replace all occurrences of oldChar.</param>
        /// <returns>
        /// A string that is equivalent to this instance except that all instances of oldChar
        /// are replaced with newChar. If oldChar is not found in the current instance, the
        /// method returns the current instance unchanged.
        /// </returns>
        public StringFilter Replace(char oldChar, char newChar) => this.str.Replace(oldChar, newChar);

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified
        /// character position and continues to the end of the string.
        /// </summary>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <returns>
        /// A string that is equivalent to the substring that begins at startIndex in this
        /// instance, or System.String.Empty if startIndex is equal to the length of this
        /// instance.
        /// </returns>
        public StringFilter Substring(int startIndex) => this.str.Substring(startIndex);

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified
        /// character position and has a specified length.
        /// </summary>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>
        /// A string that is equivalent to the substring of length length that begins at
        /// startIndex in this instance, or System.String.Empty if startIndex is equal to
        /// the length of this instance and length is zero.
        /// </returns>
        public StringFilter Substring(int startIndex, int length) => this.str.Substring(startIndex, length);

        /// <summary>
        /// Removes all leading and trailing white-space characters from the current System.String
        /// object.
        /// </summary>
        /// <returns>
        /// The string that remains after all white-space characters are removed from the
        /// start and end of the current string. If no characters can be trimmed from the
        /// current instance, the method returns the current instance unchanged.
        /// </returns>
        public StringFilter Trim() => this.str.Trim();

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of characters specified
        /// in an array from the current System.String object.</summary>
        /// <param name="trimChars">An array of Unicode characters to remove, or null.</param>
        /// <returns>
        /// The string that remains after all occurrences of the characters in the trimChars
        /// parameter are removed from the start and end of the current string. If trimChars
        /// is null or an empty array, white-space characters are removed instead. If no
        /// characters can be trimmed from the current instance, the method returns the current
        /// instance unchanged.</returns>
        public StringFilter Trim(params char[] trimChars) => this.str.Trim(trimChars);

        /// <summary>
        /// Removes all trailing occurrences of a set of characters specified in an array
        /// from the current System.String object.
        /// </summary>
        /// <param name="trimChars">An array of Unicode characters to remove, or null.</param>
        /// <returns>
        /// The string that remains after all occurrences of the characters in the trimChars
        /// parameter are removed from the end of the current string. If trimChars is null
        /// or an empty array, Unicode white-space characters are removed instead. If no
        /// characters can be trimmed from the current instance, the method returns the current
        /// instance unchanged.
        /// </returns>
        public StringFilter TrimEnd(params char[] trimChars) => this.str.TrimEnd(trimChars);

        /// <summary>
        /// Removes all leading occurrences of a set of characters specified in an array
        /// from the current System.String object.
        /// </summary>
        /// <param name="trimChars">An array of Unicode characters to remove, or null.</param>
        /// <returns>
        /// The string that remains after all occurrences of characters in the trimChars
        /// parameter are removed from the start of the current string. If trimChars is null
        /// or an empty array, white-space characters are removed instead.
        /// </returns>
        public StringFilter TrimStart(params char[] trimChars) => this.str.TrimStart(trimChars);

        /// <summary>
        /// Initializes a new instance of the <see cref="StringFilter"/> class.
        /// </summary>
        /// <param name="spans">The spans that will construct the filter.</param>
        public StringFilter(IEnumerable<ReadOnlyMemory<char>> spans)
        {
            var builder = new StringBuilder();
            foreach (var memory in spans)
            {
                var span = memory.Span;
                foreach (var c in span)
                {
                    builder.Append(c);
                }
            }

            this.str = builder.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringFilter"/> class.
        /// </summary>
        /// <param name="str">The string that will construct the filter.</param>
        public StringFilter(string str)
        {
            this.str = str;
        }

        /// <summary>
        /// Creates a string from the filterd string.
        /// </summary>
        /// <returns>A String</returns>
        public override string ToString()
        {
            return this.str.ToString();
        }

        /// <summary>
        /// Creates a filter from String.
        /// </summary>
        /// <param name="str">The string</param>
        public static implicit operator StringFilter(string str) => new StringFilter(str);

        /// <summary>
        /// Creates a string from StringFilter.
        /// </summary>
        /// <param name="str">the string filter</param>
        public static explicit operator string(StringFilter str) => str.ToString();
    }
}