// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;

namespace Microsoft.Toolkit.Uwp.Services.OAuth
{
    /// <summary>
    /// OAuth parameter.
    /// </summary>
    internal class OAuthParameter
    {
        /// <summary>
        /// Gets or sets key property.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets value property.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthParameter"/> class.
        /// Constructor accepting key and value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public OAuthParameter(string key, string value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// ToString override.
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return ToString(false);
        }

        /// <summary>
        /// Format key / value into string.
        /// </summary>
        /// <param name="withQuotes">Whether to create quotes in string.</param>
        /// <returns>Formatted string of key / value.</returns>
        public string ToString(bool withQuotes)
        {
            string format = null;
            if (withQuotes)
            {
                format = "{0}=\"{1}\"";
            }
            else
            {
                format = "{0}={1}";
            }

            return string.Format(CultureInfo.InvariantCulture, format, OAuthEncoder.UrlEncode(Key), OAuthEncoder.UrlEncode(Value));
        }
    }
}
