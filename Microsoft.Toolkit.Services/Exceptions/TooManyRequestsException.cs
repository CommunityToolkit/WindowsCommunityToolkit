// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Services
{
    /// <summary>
    /// Exception for too many requests.
    /// </summary>
    public class TooManyRequestsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TooManyRequestsException"/> class.
        /// Default constructor.
        /// </summary>
        public TooManyRequestsException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TooManyRequestsException"/> class.
        /// Constructor with additional message.
        /// </summary>
        /// <param name="message">Additional message.</param>
        public TooManyRequestsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TooManyRequestsException"/> class.
        /// Constructor with additional message and reference to inner exception.
        /// </summary>
        /// <param name="message">Additional message.</param>
        /// <param name="innerException">Reference to inner exception.</param>
        public TooManyRequestsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
