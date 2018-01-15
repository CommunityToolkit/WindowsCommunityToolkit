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
