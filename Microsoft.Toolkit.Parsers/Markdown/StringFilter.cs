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

        //
        // Summary:
        //     Gets the number of characters in the current System.String object.
        //
        // Returns:
        //     The number of characters in the current string.
        public int Length => this.str.Length;

        // Summary:
        //     Determines whether the end of this string instance matches the specified string
        //     when compared using the specified comparison option.
        //
        // Parameters:
        //   value:
        //     The string to compare to the substring at the end of this instance.
        //
        //   comparisonType:
        //     One of the enumeration values that determines how this string and value are compared.
        //
        // Returns:
        //     true if the value parameter matches the end of this string; otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        //
        //   T:System.ArgumentException:
        //     comparisonType is not a System.StringComparison value.
        public bool EndsWith(String value, StringComparison comparisonType) => this.str.EndsWith(value, comparisonType);

        // Summary:
        //     Determines whether the end of this string instance matches the specified string
        //     when compared using the specified culture.
        //
        // Parameters:
        //   value:
        //     The string to compare to the substring at the end of this instance.
        //
        //   ignoreCase:
        //     true to ignore case during the comparison; otherwise, false.
        //
        //   culture:
        //     Cultural information that determines how this instance and value are compared.
        //     If culture is null, the current culture is used.
        //
        // Returns:
        //     true if the value parameter matches the end of this string; otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        public bool EndsWith(String value, bool ignoreCase, CultureInfo culture) => this.str.EndsWith(value, ignoreCase, culture);

        // Summary:
        //     Determines whether the end of this string instance matches the specified string.
        //
        // Parameters:
        //   value:
        //     The string to compare to the substring at the end of this instance.
        //
        // Returns:
        //     true if value matches the end of this instance; otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        public bool EndsWith(String value) => this.str.EndsWith(value);

        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified string
        //     in the current System.String object. Parameters specify the starting search position
        //     in the current string and the type of search to use for the specified string.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   startIndex:
        //     The search starting position.
        //
        //   comparisonType:
        //     One of the enumeration values that specifies the rules for the search.
        //
        // Returns:
        //     The zero-based index position of the value parameter from the start of the current
        //     instance if that string is found, or -1 if it is not. If value is System.String.Empty,
        //     the return value is startIndex.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     startIndex is less than 0 (zero) or greater than the length of this string.
        //
        //   T:System.ArgumentException:
        //     comparisonType is not a valid System.StringComparison value.
        public int IndexOf(String value, int startIndex, StringComparison comparisonType) => this.str.IndexOf(value, startIndex, comparisonType);

        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified string
        //     in the current System.String object. A parameter specifies the type of search
        //     to use for the specified string.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   comparisonType:
        //     One of the enumeration values that specifies the rules for the search.
        //
        // Returns:
        //     The index position of the value parameter if that string is found, or -1 if it
        //     is not. If value is System.String.Empty, the return value is 0.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        //
        //   T:System.ArgumentException:
        //     comparisonType is not a valid System.StringComparison value.
        public int IndexOf(String value, StringComparison comparisonType) => this.str.IndexOf(value, comparisonType);

        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified string
        //     in this instance. The search starts at a specified character position and examines
        //     a specified number of character positions.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   startIndex:
        //     The search starting position.
        //
        //   count:
        //     The number of character positions to examine.
        //
        // Returns:
        //     The zero-based index position of value from the start of the current instance
        //     if that string is found, or -1 if it is not. If value is System.String.Empty,
        //     the return value is startIndex.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     count or startIndex is negative. -or- startIndex is greater than the length of
        //     this string. -or- count is greater than the length of this string minus startIndex.
        public int IndexOf(String value, int startIndex, int count) => this.str.IndexOf(value, startIndex, count);

        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified string
        //     in this instance.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        // Returns:
        //     The zero-based index position of value if that string is found, or -1 if it is
        //     not. If value is System.String.Empty, the return value is 0.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        public int IndexOf(String value) => this.str.IndexOf(value);

        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified character
        //     in this instance. The search starts at a specified character position and examines
        //     a specified number of character positions.
        //
        // Parameters:
        //   value:
        //     A Unicode character to seek.
        //
        //   startIndex:
        //     The search starting position.
        //
        //   count:
        //     The number of character positions to examine.
        //
        // Returns:
        //     The zero-based index position of value from the start of the string if that character
        //     is found, or -1 if it is not.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     count or startIndex is negative. -or- startIndex is greater than the length of
        //     this string. -or- count is greater than the length of this string minus startIndex.
        public int IndexOf(char value, int startIndex, int count) => this.str.IndexOf(value, startIndex, count);

        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified Unicode
        //     character in this string. The search starts at a specified character position.
        //
        // Parameters:
        //   value:
        //     A Unicode character to seek.
        //
        //   startIndex:
        //     The search starting position.
        //
        // Returns:
        //     The zero-based index position of value from the start of the string if that character
        //     is found, or -1 if it is not.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     startIndex is less than 0 (zero) or greater than the length of the string.
        public int IndexOf(char value, int startIndex) => this.str.IndexOf(value, startIndex);

        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified Unicode
        //     character in this string.
        //
        // Parameters:
        //   value:
        //     A Unicode character to seek.
        //
        // Returns:
        //     The zero-based index position of value if that character is found, or -1 if it
        //     is not.
        public int IndexOf(char value) => this.str.IndexOf(value);

        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified string
        //     in the current System.String object. Parameters specify the starting search position
        //     in the current string, the number of characters in the current string to search,
        //     and the type of search to use for the specified string.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   startIndex:
        //     The search starting position.
        //
        //   count:
        //     The number of character positions to examine.
        //
        //   comparisonType:
        //     One of the enumeration values that specifies the rules for the search.
        //
        // Returns:
        //     The zero-based index position of the value parameter from the start of the current
        //     instance if that string is found, or -1 if it is not. If value is System.String.Empty,
        //     the return value is startIndex.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     count or startIndex is negative. -or- startIndex is greater than the length of
        //     this instance. -or- count is greater than the length of this string minus startIndex.
        //
        //   T:System.ArgumentException:
        //     comparisonType is not a valid System.StringComparison value.
        public int IndexOf(String value, int startIndex, int count, StringComparison comparisonType) => this.str.IndexOf(value, startIndex, count, comparisonType);

        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified string
        //     in this instance. The search starts at a specified character position.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   startIndex:
        //     The search starting position.
        //
        // Returns:
        //     The zero-based index position of value from the start of the current instance
        //     if that string is found, or -1 if it is not. If value is System.String.Empty,
        //     the return value is startIndex.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     startIndex is less than 0 (zero) or greater than the length of this string.
        public int IndexOf(String value, int startIndex) => this.str.IndexOf(value, startIndex);

        // Summary:
        //     Reports the zero-based index of the first occurrence in this instance of any
        //     character in a specified array of Unicode characters.
        //
        // Parameters:
        //   anyOf:
        //     A Unicode character array containing one or more characters to seek.
        //
        // Returns:
        //     The zero-based index position of the first occurrence in this instance where
        //     any character in anyOf was found; -1 if no character in anyOf was found.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     anyOf is null.
        public int IndexOfAny(char[] anyOf) => this.str.IndexOfAny(anyOf);

        // Summary:
        //     Reports the zero-based index of the first occurrence in this instance of any
        //     character in a specified array of Unicode characters. The search starts at a
        //     specified character position and examines a specified number of character positions.
        //
        // Parameters:
        //   anyOf:
        //     A Unicode character array containing one or more characters to seek.
        //
        //   startIndex:
        //     The search starting position.
        //
        //   count:
        //     The number of character positions to examine.
        //
        // Returns:
        //     The zero-based index position of the first occurrence in this instance where
        //     any character in anyOf was found; -1 if no character in anyOf was found.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     anyOf is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     count or startIndex is negative. -or- count + startIndex is greater than the
        //     number of characters in this instance.
        public int IndexOfAny(char[] anyOf, int startIndex, int count) => this.str.IndexOfAny(anyOf, startIndex, count);

        // Summary:
        //     Reports the zero-based index of the first occurrence in this instance of any
        //     character in a specified array of Unicode characters. The search starts at a
        //     specified character position.
        //
        // Parameters:
        //   anyOf:
        //     A Unicode character array containing one or more characters to seek.
        //
        //   startIndex:
        //     The search starting position.
        //
        // Returns:
        //     The zero-based index position of the first occurrence in this instance where
        //     any character in anyOf was found; -1 if no character in anyOf was found.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     anyOf is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     startIndex is negative. -or- startIndex is greater than the number of characters
        //     in this instance.
        public int IndexOfAny(char[] anyOf, int startIndex) => this.str.IndexOfAny(anyOf, startIndex);

        // Summary:
        //     Reports the zero-based index of the last occurrence of a specified string within
        //     the current System.String object. The search starts at a specified character
        //     position and proceeds backward toward the beginning of the string. A parameter
        //     specifies the type of comparison to perform when searching for the specified
        //     string.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   startIndex:
        //     The search starting position. The search proceeds from startIndex toward the
        //     beginning of this instance.
        //
        //   comparisonType:
        //     One of the enumeration values that specifies the rules for the search.
        //
        // Returns:
        //     The zero-based starting index position of the value parameter if that string
        //     is found, or -1 if it is not found or if the current instance equals System.String.Empty.
        //     If value is System.String.Empty, the return value is the smaller of startIndex
        //     and the last index position in this instance.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     The current instance does not equal System.String.Empty, and startIndex is less
        //     than zero or greater than the length of the current instance. -or- The current
        //     instance equals System.String.Empty, and startIndex is less than -1 or greater
        //     than zero.
        //
        //   T:System.ArgumentException:
        //     comparisonType is not a valid System.StringComparison value.
        public int LastIndexOf(String value, int startIndex, StringComparison comparisonType) => this.str.LastIndexOf(value, startIndex, comparisonType);

        // Summary:
        //     Reports the zero-based index position of the last occurrence of a specified string
        //     within this instance. The search starts at a specified character position and
        //     proceeds backward toward the beginning of the string for the specified number
        //     of character positions. A parameter specifies the type of comparison to perform
        //     when searching for the specified string.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   startIndex:
        //     The search starting position. The search proceeds from startIndex toward the
        //     beginning of this instance.
        //
        //   count:
        //     The number of character positions to examine.
        //
        //   comparisonType:
        //     One of the enumeration values that specifies the rules for the search.
        //
        // Returns:
        //     The zero-based starting index position of the value parameter if that string
        //     is found, or -1 if it is not found or if the current instance equals System.String.Empty.
        //     If value is System.String.Empty, the return value is the smaller of startIndex
        //     and the last index position in this instance.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     count is negative. -or- The current instance does not equal System.String.Empty,
        //     and startIndex is negative. -or- The current instance does not equal System.String.Empty,
        //     and startIndex is greater than the length of this instance. -or- The current
        //     instance does not equal System.String.Empty, and startIndex + 1 - count specifies
        //     a position that is not within this instance. -or- The current instance equals
        //     System.String.Empty and start is less than -1 or greater than zero. -or- The
        //     current instance equals System.String.Empty and count is greater than 1.
        //
        //   T:System.ArgumentException:
        //     comparisonType is not a valid System.StringComparison value.
        public int LastIndexOf(String value, int startIndex, int count, StringComparison comparisonType) => this.str.LastIndexOf(value, startIndex, count, comparisonType);

        // Summary:
        //     Reports the zero-based index position of the last occurrence of a specified string
        //     within this instance. The search starts at a specified character position and
        //     proceeds backward toward the beginning of the string for a specified number of
        //     character positions.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   startIndex:
        //     The search starting position. The search proceeds from startIndex toward the
        //     beginning of this instance.
        //
        //   count:
        //     The number of character positions to examine.
        //
        // Returns:
        //     The zero-based starting index position of value if that string is found, or -1
        //     if it is not found or if the current instance equals System.String.Empty. If
        //     value is System.String.Empty, the return value is the smaller of startIndex and
        //     the last index position in this instance.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     count is negative. -or- The current instance does not equal System.String.Empty,
        //     and startIndex is negative. -or- The current instance does not equal System.String.Empty,
        //     and startIndex is greater than the length of this instance. -or- The current
        //     instance does not equal System.String.Empty, and startIndex - count+ 1 specifies
        //     a position that is not within this instance. -or- The current instance equals
        //     System.String.Empty and start is less than -1 or greater than zero. -or- The
        //     current instance equals System.String.Empty and count is greater than 1.
        public int LastIndexOf(String value, int startIndex, int count) => this.str.LastIndexOf(value, startIndex, count);

        // Summary:
        //     Reports the zero-based index of the last occurrence of a specified string within
        //     the current System.String object. A parameter specifies the type of search to
        //     use for the specified string.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   comparisonType:
        //     One of the enumeration values that specifies the rules for the search.
        //
        // Returns:
        //     The zero-based starting index position of the value parameter if that string
        //     is found, or -1 if it is not. If value is System.String.Empty, the return value
        //     is the last index position in this instance.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        //
        //   T:System.ArgumentException:
        //     comparisonType is not a valid System.StringComparison value.
        public int LastIndexOf(String value, StringComparison comparisonType) => this.str.LastIndexOf(value, comparisonType);

        // Summary:
        //     Reports the zero-based index position of the last occurrence of a specified string
        //     within this instance.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        // Returns:
        //     The zero-based starting index position of value if that string is found, or -1
        //     if it is not. If value is System.String.Empty, the return value is the last index
        //     position in this instance.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        public int LastIndexOf(String value) => this.str.LastIndexOf(value);

        // Summary:
        //     Reports the zero-based index position of the last occurrence of the specified
        //     Unicode character in a substring within this instance. The search starts at a
        //     specified character position and proceeds backward toward the beginning of the
        //     string for a specified number of character positions.
        //
        // Parameters:
        //   value:
        //     The Unicode character to seek.
        //
        //   startIndex:
        //     The starting position of the search. The search proceeds from startIndex toward
        //     the beginning of this instance.
        //
        //   count:
        //     The number of character positions to examine.
        //
        // Returns:
        //     The zero-based index position of value if that character is found, or -1 if it
        //     is not found or if the current instance equals System.String.Empty.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     The current instance does not equal System.String.Empty, and startIndex is less
        //     than zero or greater than or equal to the length of this instance. -or- The current
        //     instance does not equal System.String.Empty, and startIndex - count + 1 is less
        //     than zero.
        public int LastIndexOf(char value, int startIndex, int count) => this.str.LastIndexOf(value, startIndex, count);

        // Summary:
        //     Reports the zero-based index position of the last occurrence of a specified Unicode
        //     character within this instance. The search starts at a specified character position
        //     and proceeds backward toward the beginning of the string.
        //
        // Parameters:
        //   value:
        //     The Unicode character to seek.
        //
        //   startIndex:
        //     The starting position of the search. The search proceeds from startIndex toward
        //     the beginning of this instance.
        //
        // Returns:
        //     The zero-based index position of value if that character is found, or -1 if it
        //     is not found or if the current instance equals System.String.Empty.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     The current instance does not equal System.String.Empty, and startIndex is less
        //     than zero or greater than or equal to the length of this instance.
        public int LastIndexOf(char value, int startIndex) => this.str.LastIndexOf(value, startIndex);

        // Summary:
        //     Reports the zero-based index position of the last occurrence of a specified string
        //     within this instance. The search starts at a specified character position and
        //     proceeds backward toward the beginning of the string.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   startIndex:
        //     The search starting position. The search proceeds from startIndex toward the
        //     beginning of this instance.
        //
        // Returns:
        //     The zero-based starting index position of value if that string is found, or -1
        //     if it is not found or if the current instance equals System.String.Empty. If
        //     value is System.String.Empty, the return value is the smaller of startIndex and
        //     the last index position in this instance.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     The current instance does not equal System.String.Empty, and startIndex is less
        //     than zero or greater than the length of the current instance. -or- The current
        //     instance equals System.String.Empty, and startIndex is less than -1 or greater
        //     than zero.
        public int LastIndexOf(String value, int startIndex) => this.str.LastIndexOf(value, startIndex);

        // Summary:
        //     Reports the zero-based index position of the last occurrence of a specified Unicode
        //     character within this instance.
        //
        // Parameters:
        //   value:
        //     The Unicode character to seek.
        //
        // Returns:
        //     The zero-based index position of value if that character is found, or -1 if it
        //     is not.
        public int LastIndexOf(char value) => this.str.LastIndexOf(value);

        // Summary:
        //     Reports the zero-based index position of the last occurrence in this instance
        //     of one or more characters specified in a Unicode array.
        //
        // Parameters:
        //   anyOf:
        //     A Unicode character array containing one or more characters to seek.
        //
        // Returns:
        //     The index position of the last occurrence in this instance where any character
        //     in anyOf was found; -1 if no character in anyOf was found.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     anyOf is null.
        public int LastIndexOfAny(char[] anyOf) => this.str.LastIndexOfAny(anyOf);

        // Summary:
        //     Reports the zero-based index position of the last occurrence in this instance
        //     of one or more characters specified in a Unicode array. The search starts at
        //     a specified character position and proceeds backward toward the beginning of
        //     the string.
        //
        // Parameters:
        //   anyOf:
        //     A Unicode character array containing one or more characters to seek.
        //
        //   startIndex:
        //     The search starting position. The search proceeds from startIndex toward the
        //     beginning of this instance.
        //
        // Returns:
        //     The index position of the last occurrence in this instance where any character
        //     in anyOf was found; -1 if no character in anyOf was found or if the current instance
        //     equals System.String.Empty.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     anyOf is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     The current instance does not equal System.String.Empty, and startIndex specifies
        //     a position that is not within this instance.
        public int LastIndexOfAny(char[] anyOf, int startIndex) => this.str.LastIndexOfAny(anyOf, startIndex);

        // Summary:
        //     Reports the zero-based index position of the last occurrence in this instance
        //     of one or more characters specified in a Unicode array. The search starts at
        //     a specified character position and proceeds backward toward the beginning of
        //     the string for a specified number of character positions.
        //
        // Parameters:
        //   anyOf:
        //     A Unicode character array containing one or more characters to seek.
        //
        //   startIndex:
        //     The search starting position. The search proceeds from startIndex toward the
        //     beginning of this instance.
        //
        //   count:
        //     The number of character positions to examine.
        //
        // Returns:
        //     The index position of the last occurrence in this instance where any character
        //     in anyOf was found; -1 if no character in anyOf was found or if the current instance
        //     equals System.String.Empty.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     anyOf is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     The current instance does not equal System.String.Empty, and count or startIndex
        //     is negative. -or- The current instance does not equal System.String.Empty, and
        //     startIndex minus count + 1 is less than zero.
        public int LastIndexOfAny(char[] anyOf, int startIndex, int count) => this.str.LastIndexOfAny(anyOf, startIndex, count);

        // Summary:
        //     Determines whether the beginning of this string instance matches the specified
        //     string.
        //
        // Parameters:
        //   value:
        //     The string to compare.
        //
        // Returns:
        //     true if value matches the beginning of this string; otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        public bool StartsWith(String value) => this.str.StartsWith(value);

        // Summary:
        //     Determines whether the beginning of this string instance matches the specified
        //     string when compared using the specified culture.
        //
        // Parameters:
        //   value:
        //     The string to compare.
        //
        //   ignoreCase:
        //     true to ignore case during the comparison; otherwise, false.
        //
        //   culture:
        //     Cultural information that determines how this string and value are compared.
        //     If culture is null, the current culture is used.
        //
        // Returns:
        //     true if the value parameter matches the beginning of this string; otherwise,
        //     false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        public bool StartsWith(String value, bool ignoreCase, CultureInfo culture) => this.str.StartsWith(value, ignoreCase, culture);

        // Summary:
        //     Determines whether the beginning of this string instance matches the specified
        //     string when compared using the specified comparison option.
        //
        // Parameters:
        //   value:
        //     The string to compare.
        //
        //   comparisonType:
        //     One of the enumeration values that determines how this string and value are compared.
        //
        // Returns:
        //     true if this instance begins with value; otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        //
        //   T:System.ArgumentException:
        //     comparisonType is not a System.StringComparison value.
        public bool StartsWith(String value, StringComparison comparisonType) => this.str.StartsWith(value, comparisonType);

        // Summary:
        //     Concatenates three specified instances of System.String.
        //
        // Parameters:
        //   str0:
        //     The first string to concatenate.
        //
        //   str1:
        //     The second string to concatenate.
        //
        //   str2:
        //     The third string to concatenate.
        //
        // Returns:
        //     The concatenation of str0, str1, and str2.
        public static StringFilter Concat(String str0, String str1, String str2) => string.Concat(str0, str1, str2);

        // Summary:
        //     Concatenates the members of an System.Collections.Generic.IEnumerable`1 implementation.
        //
        // Parameters:
        //   values:
        //     A collection object that implements the System.Collections.Generic.IEnumerable`1
        //     interface.
        //
        // Type parameters:
        //   T:
        //     The type of the members of values.
        //
        // Returns:
        //     The concatenated members in values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     values is null.
        public static StringFilter Concat<T>(IEnumerable<T> values) => string.Concat(values);

        // Summary:
        //     Concatenates the elements of a specified System.String array.
        //
        // Parameters:
        //   values:
        //     An array of string instances.
        //
        // Returns:
        //     The concatenated elements of values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     values is null.
        //
        //   T:System.OutOfMemoryException:
        //     Out of memory.
        public static StringFilter Concat(params String[] values) => string.Concat(values);

        // Summary:
        //     Concatenates four specified instances of System.String.
        //
        // Parameters:
        //   str0:
        //     The first string to concatenate.
        //
        //   str1:
        //     The second string to concatenate.
        //
        //   str2:
        //     The third string to concatenate.
        //
        //   str3:
        //     The fourth string to concatenate.
        //
        // Returns:
        //     The concatenation of str0, str1, str2, and str3.
        public static StringFilter Concat(String str0, String str1, String str2, String str3) => string.Concat(str0, str1, str2, str3);

        // Summary:
        //     Concatenates two specified instances of System.String.
        //
        // Parameters:
        //   str0:
        //     The first string to concatenate.
        //
        //   str1:
        //     The second string to concatenate.
        //
        // Returns:
        //     The concatenation of str0 and str1.
        public static StringFilter Concat(String str0, String str1) => string.Concat(str0, str1);

        // Summary:
        //     Concatenates the string representations of three specified objects.
        //
        // Parameters:
        //   arg0:
        //     The first object to concatenate.
        //
        //   arg1:
        //     The second object to concatenate.
        //
        //   arg2:
        //     The third object to concatenate.
        //
        // Returns:
        //     The concatenated string representations of the values of arg0, arg1, and arg2.
        public static StringFilter Concat(object arg0, object arg1, object arg2) => string.Concat(arg0, arg1, arg2);

        // Summary:
        //     Concatenates the string representations of two specified objects.
        //
        // Parameters:
        //   arg0:
        //     The first object to concatenate.
        //
        //   arg1:
        //     The second object to concatenate.
        //
        // Returns:
        //     The concatenated string representations of the values of arg0 and arg1.
        public static StringFilter Concat(object arg0, object arg1) => string.Concat(arg0, arg1);

        // Summary:
        //     Creates the string representation of a specified object.
        //
        // Parameters:
        //   arg0:
        //     The object to represent, or null.
        //
        // Returns:
        //     The string representation of the value of arg0, or System.String.Empty if arg0
        //     is null.
        public static StringFilter Concat(object arg0) => string.Concat(arg0);

        // Summary:
        //     Concatenates the members of a constructed System.Collections.Generic.IEnumerable`1
        //     collection of type System.String.
        //
        // Parameters:
        //   values:
        //     A collection object that implements System.Collections.Generic.IEnumerable`1
        //     and whose generic type argument is System.String.
        //
        // Returns:
        //     The concatenated strings in values, or System.String.Empty if values is an empty
        //     IEnumerable(Of String).
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     values is null.
        public static StringFilter Concat(IEnumerable<String> values) => string.Concat(values);

        // Summary:
        //     Concatenates the string representations of the elements in a specified System.Object
        //     array.
        //
        // Parameters:
        //   args:
        //     An object array that contains the elements to concatenate.
        //
        // Returns:
        //     The concatenated string representations of the values of the elements in args.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     args is null.
        //
        //   T:System.OutOfMemoryException:
        //     Out of memory.
        public static StringFilter Concat(params object[] args) => string.Concat(args);

        // Summary:
        //     Splits a string into a maximum number of substrings based on the strings in an
        //     array. You can specify whether the substrings include empty array elements.
        //
        // Parameters:
        //   separator:
        //     A string array that delimits the substrings in this string, an empty array that
        //     contains no delimiters, or null.
        //
        //   count:
        //     The maximum number of substrings to return.
        //
        //   options:
        //     System.StringSplitOptions.RemoveEmptyEntries to omit empty array elements from
        //     the array returned; or System.StringSplitOptions.None to include empty array
        //     elements in the array returned.
        //
        // Returns:
        //     An array whose elements contain the substrings in this string that are delimited
        //     by one or more strings in separator. For more information, see the Remarks section.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     count is negative.
        //
        //   T:System.ArgumentException:
        //     options is not one of the System.StringSplitOptions values.
        public StringFilter[] Split(String[] separator, int count, StringSplitOptions options) => this.str.Split(separator, count, options).Cast<StringFilter>().ToArray();

        // Summary:
        //     Splits a string into substrings that are based on the characters in an array.
        //
        // Parameters:
        //   separator:
        //     A character array that delimits the substrings in this string, an empty array
        //     that contains no delimiters, or null.
        //
        // Returns:
        //     An array whose elements contain the substrings from this instance that are delimited
        //     by one or more characters in separator. For more information, see the Remarks
        //     section.
        public StringFilter[] Split(params char[] separator) => this.str.Split(separator).Cast<StringFilter>().ToArray();

        // Summary:
        //     Splits a string into a maximum number of substrings based on the characters in
        //     an array. You also specify the maximum number of substrings to return.
        //
        // Parameters:
        //   separator:
        //     A character array that delimits the substrings in this string, an empty array
        //     that contains no delimiters, or null.
        //
        //   count:
        //     The maximum number of substrings to return.
        //
        // Returns:
        //     An array whose elements contain the substrings in this instance that are delimited
        //     by one or more characters in separator. For more information, see the Remarks
        //     section.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     count is negative.
        public StringFilter[] Split(char[] separator, int count) => this.str.Split(separator, count).Cast<StringFilter>().ToArray();

        // Summary:
        //     Splits a string into a maximum number of substrings based on the characters in
        //     an array.
        //
        // Parameters:
        //   separator:
        //     A character array that delimits the substrings in this string, an empty array
        //     that contains no delimiters, or null.
        //
        //   count:
        //     The maximum number of substrings to return.
        //
        //   options:
        //     System.StringSplitOptions.RemoveEmptyEntries to omit empty array elements from
        //     the array returned; or System.StringSplitOptions.None to include empty array
        //     elements in the array returned.
        //
        // Returns:
        //     An array whose elements contain the substrings in this string that are delimited
        //     by one or more characters in separator. For more information, see the Remarks
        //     section.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     count is negative.
        //
        //   T:System.ArgumentException:
        //     options is not one of the System.StringSplitOptions values.
        public StringFilter[] Split(char[] separator, int count, StringSplitOptions options) => this.str.Split(separator, count, options).Cast<StringFilter>().ToArray();

        // Summary:
        //     Splits a string into substrings based on the characters in an array. You can
        //     specify whether the substrings include empty array elements.
        //
        // Parameters:
        //   separator:
        //     A character array that delimits the substrings in this string, an empty array
        //     that contains no delimiters, or null.
        //
        //   options:
        //     System.StringSplitOptions.RemoveEmptyEntries to omit empty array elements from
        //     the array returned; or System.StringSplitOptions.None to include empty array
        //     elements in the array returned.
        //
        // Returns:
        //     An array whose elements contain the substrings in this string that are delimited
        //     by one or more characters in separator. For more information, see the Remarks
        //     section.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     options is not one of the System.StringSplitOptions values.
        public StringFilter[] Split(char[] separator, StringSplitOptions options) => this.str.Split(separator, options).Cast<StringFilter>().ToArray();

        // Summary:
        //     Splits a string into substrings based on the strings in an array. You can specify
        //     whether the substrings include empty array elements.
        //
        // Parameters:
        //   separator:
        //     A string array that delimits the substrings in this string, an empty array that
        //     contains no delimiters, or null.
        //
        //   options:
        //     System.StringSplitOptions.RemoveEmptyEntries to omit empty array elements from
        //     the array returned; or System.StringSplitOptions.None to include empty array
        //     elements in the array returned.
        //
        // Returns:
        //     An array whose elements contain the substrings in this string that are delimited
        //     by one or more strings in separator. For more information, see the Remarks section.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     options is not one of the System.StringSplitOptions values.
        public StringFilter[] Split(String[] separator, StringSplitOptions options) => this.str.Split(separator, options).Cast<StringFilter>().ToArray();

        // Summary:
        //     Returns a new string in which all the characters in the current instance, beginning
        //     at a specified position and continuing through the last position, have been deleted.
        //
        // Parameters:
        //   startIndex:
        //     The zero-based position to begin deleting characters.
        //
        // Returns:
        //     A new string that is equivalent to this string except for the removed characters.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     startIndex is less than zero. -or- startIndex specifies a position that is not
        //     within this string.
        public StringFilter Remove(int startIndex) => this.str.Remove(startIndex);

        // Summary:
        //     Returns a new string in which a specified number of characters in the current
        //     instance beginning at a specified position have been deleted.
        //
        // Parameters:
        //   startIndex:
        //     The zero-based position to begin deleting characters.
        //
        //   count:
        //     The number of characters to delete.
        //
        // Returns:
        //     A new string that is equivalent to this instance except for the removed characters.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     Either startIndex or count is less than zero. -or- startIndex plus count specify
        //     a position outside this instance.
        public StringFilter Remove(int startIndex, int count) => this.str.Remove(startIndex, count);

        // Summary:
        //     Returns a new string in which all occurrences of a specified string in the current
        //     instance are replaced with another specified string.
        //
        // Parameters:
        //   oldValue:
        //     The string to be replaced.
        //
        //   newValue:
        //     The string to replace all occurrences of oldValue.
        //
        // Returns:
        //     A string that is equivalent to the current string except that all instances of
        //     oldValue are replaced with newValue. If oldValue is not found in the current
        //     instance, the method returns the current instance unchanged.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     oldValue is null.
        //
        //   T:System.ArgumentException:
        //     oldValue is the empty string ("").
        public StringFilter Replace(String oldValue, String newValue) => this.str.Replace(oldValue, newValue);

        // Summary:
        //     Returns a new string in which all occurrences of a specified Unicode character
        //     in this instance are replaced with another specified Unicode character.
        //
        // Parameters:
        //   oldChar:
        //     The Unicode character to be replaced.
        //
        //   newChar:
        //     The Unicode character to replace all occurrences of oldChar.
        //
        // Returns:
        //     A string that is equivalent to this instance except that all instances of oldChar
        //     are replaced with newChar. If oldChar is not found in the current instance, the
        //     method returns the current instance unchanged.
        public StringFilter Replace(char oldChar, char newChar) => this.str.Replace(oldChar, newChar);

        // Summary:
        //     Retrieves a substring from this instance. The substring starts at a specified
        //     character position and continues to the end of the string.
        //
        // Parameters:
        //   startIndex:
        //     The zero-based starting character position of a substring in this instance.
        //
        // Returns:
        //     A string that is equivalent to the substring that begins at startIndex in this
        //     instance, or System.String.Empty if startIndex is equal to the length of this
        //     instance.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     startIndex is less than zero or greater than the length of this instance.
        public StringFilter Substring(int startIndex) => this.str.Substring(startIndex);

        // Summary:
        //     Retrieves a substring from this instance. The substring starts at a specified
        //     character position and has a specified length.
        //
        // Parameters:
        //   startIndex:
        //     The zero-based starting character position of a substring in this instance.
        //
        //   length:
        //     The number of characters in the substring.
        //
        // Returns:
        //     A string that is equivalent to the substring of length length that begins at
        //     startIndex in this instance, or System.String.Empty if startIndex is equal to
        //     the length of this instance and length is zero.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     startIndex plus length indicates a position not within this instance. -or- startIndex
        //     or length is less than zero.
        public StringFilter Substring(int startIndex, int length) => this.str.Substring(startIndex, length);

        // Summary:
        //     Removes all leading and trailing white-space characters from the current System.String
        //     object.
        //
        // Returns:
        //     The string that remains after all white-space characters are removed from the
        //     start and end of the current string. If no characters can be trimmed from the
        //     current instance, the method returns the current instance unchanged.
        public StringFilter Trim() => this.str.Trim();

        // Summary:
        //     Removes all leading and trailing occurrences of a set of characters specified
        //     in an array from the current System.String object.
        //
        // Parameters:
        //   trimChars:
        //     An array of Unicode characters to remove, or null.
        //
        // Returns:
        //     The string that remains after all occurrences of the characters in the trimChars
        //     parameter are removed from the start and end of the current string. If trimChars
        //     is null or an empty array, white-space characters are removed instead. If no
        //     characters can be trimmed from the current instance, the method returns the current
        //     instance unchanged.
        public StringFilter Trim(params char[] trimChars) => this.str.Trim(trimChars);

        // Summary:
        //     Removes all trailing occurrences of a set of characters specified in an array
        //     from the current System.String object.
        //
        // Parameters:
        //   trimChars:
        //     An array of Unicode characters to remove, or null.
        //
        // Returns:
        //     The string that remains after all occurrences of the characters in the trimChars
        //     parameter are removed from the end of the current string. If trimChars is null
        //     or an empty array, Unicode white-space characters are removed instead. If no
        //     characters can be trimmed from the current instance, the method returns the current
        //     instance unchanged.
        public StringFilter TrimEnd(params char[] trimChars) => this.str.TrimEnd(trimChars);

        // Summary:
        //     Removes all leading occurrences of a set of characters specified in an array
        //     from the current System.String object.
        //
        // Parameters:
        //   trimChars:
        //     An array of Unicode characters to remove, or null.
        //
        // Returns:
        //     The string that remains after all occurrences of characters in the trimChars
        //     parameter are removed from the start of the current string. If trimChars is null
        //     or an empty array, white-space characters are removed instead.
        public StringFilter TrimStart(params char[] trimChars) => this.str.TrimStart(trimChars);

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

        public override string ToString()
        {
            return this.str.ToString();
        }

        public static implicit operator StringFilter(string str) => new StringFilter(new ReadOnlyMemory<char>[] { str.AsMemory() });

        public static explicit operator string(StringFilter str) => str.ToString();

    }
}