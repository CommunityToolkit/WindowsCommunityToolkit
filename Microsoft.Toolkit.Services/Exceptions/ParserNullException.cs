// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Services
{
    /// <summary>
    /// Exception for null Parser.
    /// </summary>
    public class ParserNullException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParserNullException"/> class.
        /// Default constructor.
        /// </summary>
        public ParserNullException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserNullException"/> class.
        /// Constructor with additional message.
        /// </summary>
        /// <param name="message">Additional message</param>
        public ParserNullException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserNullException"/> class.
        /// Constructor with additional message and inner exception.
        /// </summary>
        /// <param name="message">Additonal message.</param>
        /// <param name="innerException">Reference to inner exception.</param>
        public ParserNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}