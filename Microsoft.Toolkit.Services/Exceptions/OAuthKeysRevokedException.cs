// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Services
{
    /// <summary>
    /// Exception for revoked OAuth keys.
    /// </summary>
    public class OAuthKeysRevokedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthKeysRevokedException"/> class.
        /// Default constructor.
        /// </summary>
        public OAuthKeysRevokedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthKeysRevokedException"/> class.
        /// Constructor with additional message.
        /// </summary>
        /// <param name="message">Additional message</param>
        public OAuthKeysRevokedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthKeysRevokedException"/> class.
        /// Constructor with additional message and inner exception.
        /// </summary>
        /// <param name="message">Additionnal message.</param>
        /// <param name="innerException">Reference to inner exception.</param>
        public OAuthKeysRevokedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
