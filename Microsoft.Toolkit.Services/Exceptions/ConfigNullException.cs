// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Services
{
    /// <summary>
    /// Exception for null Config.
    /// </summary>
    public class ConfigNullException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigNullException"/> class.
        /// Default constructor.
        /// </summary>
        public ConfigNullException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigNullException"/> class.
        /// Constructor accepting additional message string.
        /// </summary>
        /// <param name="message">Additional error information.</param>
        public ConfigNullException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigNullException"/> class.
        /// Constructor accepting additonal message string and inner exception
        /// </summary>
        /// <param name="message">Additional error information.</param>
        /// <param name="innerException">Reference to inner exception.</param>
        public ConfigNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}