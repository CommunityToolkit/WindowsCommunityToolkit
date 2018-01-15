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

using System;

namespace Microsoft.Toolkit.Services.Exceptions
{
    /// <summary>
    /// Exception for no OAuth keys being present.
    /// </summary>
    public class OAuthKeysNotPresentException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthKeysNotPresentException"/> class.
        /// Default constructor.
        /// </summary>
        public OAuthKeysNotPresentException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthKeysNotPresentException"/> class.
        /// Constructor with information on missing key.
        /// </summary>
        /// <param name="key">Name of the missing key.</param>
        public OAuthKeysNotPresentException(string key)
            : base(string.Format("Open Authentication Key '{0}' not present", key))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthKeysNotPresentException"/> class.
        /// Constructor with additional message and inner exception.
        /// </summary>
        /// <param name="message">Additional exception message.</param>
        /// <param name="innerException">Reference to inner exception.</param>
        public OAuthKeysNotPresentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}