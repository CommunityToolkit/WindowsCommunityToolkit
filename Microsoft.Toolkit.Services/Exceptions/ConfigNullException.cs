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