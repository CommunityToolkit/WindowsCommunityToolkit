// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Services
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