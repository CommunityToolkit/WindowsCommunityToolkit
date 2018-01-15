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