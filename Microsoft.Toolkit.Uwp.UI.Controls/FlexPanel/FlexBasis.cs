// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See the LICENSE file in the project root
// for the license information.
//
// Author(s):
//  - Laurent Sansonetti (native Xamarin flex https://github.com/xamarin/flex)
//  - Stephane Delcroix (.NET port)
//  - Ben Askren (UWP/Uno port)
//
using System;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// This class is used to define the initial main-axis dimension of the UIElement in the FlexLayout or if that value
    /// calculated by FlexPanel (FlexBasis.Auto).  If FlexBasis.IsRelative is false, then this child element's
    /// main-axis dimension will the FlexBasis.Length, in pixels.  Any remaining space will be portioned among all the child
    /// elements with a FlexBasis.IsRelstive set to true.
    /// </summary>
    /// <remarks>The default value for this property is Auto.</remarks>
    public struct FlexBasis
    {
        /// <summary>
        /// Converts a string to a FlexBasis
        /// </summary>
        /// <param name="value">Length. If includes ",relative" argument, then length is in proportion of parent size.  Otherwise, it is in pixels.</param>
        /// <returns>a new FlexBasis instance</returns>
        public static FlexBasis Parse(string value)
        {
            value = value.Trim().ToLower();
            if (value.Contains("auto"))
            {
                return Auto;
            }

            bool isPercent = false;
            if (value.EndsWith('%'))
            {
                value = value.Substring(0, value.Length - 1);
                isPercent = true;
            }

            if (double.TryParse(value.Split(',')[0], out double length))
            {
                if (length < 0)
                {
                    return Auto;
                }
                else if (isPercent)
                {
                    return new FlexBasis(length / 100, true);
                }
                else
                {
                    return new FlexBasis(length, value.Contains("relative"));
                }
            }

            return Auto;
        }

        private readonly bool _isLength;
        private readonly bool _isRelative;

        /// <summary>
        /// Main-axis length of element is calculated by FlexPanel
        /// </summary>
        public static FlexBasis Auto = default;

        /// <summary>
        /// Gets the main-axis length of the element in the FlexPanel
        /// </summary>
        public double Length { get; }

        /// <summary>
        /// Gets a value indicating whether the basis is auto.
        /// </summary>
        internal bool IsAuto => !_isLength && !_isRelative;

        /// <summary>
        /// Gets a value indicating whether the basis length is relative to parent's size.
        /// </summary>
        internal bool IsRelative => _isRelative;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlexBasis"/> struct.
        /// </summary>
        /// <param name="length">Length.</param>
        /// <param name="isRelative">If set to <c>true</c> is relative.</param>
        public FlexBasis(double length, bool isRelative = false)
        {
            if (length < 0)
            {
                throw new ArgumentException("should be a positive value", nameof(length));
            }

            if (isRelative && length > 1)
            {
                throw new ArgumentException("relative length should be in [0, 1]", nameof(length));
            }

            _isLength = !isRelative;
            _isRelative = isRelative;
            Length = length;
        }

        /// <summary>
        /// Converts a double to a FlexBasis
        /// </summary>
        /// <param name="length">Length, in pixels, of element in main-axis direction</param>
        public static implicit operator FlexBasis(double length)
            => new FlexBasis(length);

        /// <summary>
        /// Converts a string to a FlexBasis
        /// </summary>
        /// <param name="value">Length. If includes ",relative" argument, then length is in proportion of parent size.  Otherwise, it is in pixels.</param>
        public static implicit operator FlexBasis(string value)
            => Parse(value);

        /// <summary>
        /// Converts a FlexBasis to a string
        /// </summary>
        /// <returns>string indiating length.  If length is relative to a proportion of parent's size, the also included ",relative".  Otherwise, the length value is pixels.</returns>
        public override string ToString()
        {
            if (IsAuto)
            {
                return "auto";
            }

            if (IsRelative)
            {
                return Length + ",relative";
            }

            return Length.ToString(); ;
        }
    }
}