// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Services
{
    /// <summary>
    /// Exception for config parameter being null.
    /// </summary>
    public class ConfigParameterNullException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigParameterNullException"/> class.
        /// Default constructor.
        /// </summary>
        public ConfigParameterNullException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigParameterNullException"/> class.
        /// Accepts parameter name.
        /// </summary>
        /// <param name="parameter">Name of the parameter.</param>
        public ConfigParameterNullException(string parameter)
            : base(string.Format("The parameter '{0}' in config is null.", parameter))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigParameterNullException"/> class.
        /// Accepts parameter name and inner exception.
        /// </summary>
        /// <param name="message">Name of the parameter.</param>
        /// <param name="innerException">Reference to the inner exception.</param>
        public ConfigParameterNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}