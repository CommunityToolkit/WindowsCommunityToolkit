// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
