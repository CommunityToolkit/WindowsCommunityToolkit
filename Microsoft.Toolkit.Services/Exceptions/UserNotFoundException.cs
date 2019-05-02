// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Services
{
    /// <summary>
    /// Exception for user not found.
    /// </summary>
    public class UserNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
        /// Default constructor.
        /// </summary>
        public UserNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
        /// Constructor with screen/user name information.
        /// </summary>
        /// <param name="screenName">Name of user not found.</param>
        public UserNotFoundException(string screenName)
            : base("User " + screenName + " not found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
        /// Constructor with screen/user name information and inner exception.
        /// </summary>
        /// <param name="screenName">Name of user not found.</param>
        /// <param name="innerException">Reference to inner exception.</param>
        public UserNotFoundException(string screenName, Exception innerException)
            : base("User " + screenName + " not found.", innerException)
        {
        }
    }
}
