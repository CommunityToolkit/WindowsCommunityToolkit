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

namespace Microsoft.Toolkit.Uwp.Services.CognitiveServices
{
    /// <summary>
    /// Vision Service Exception for handling request errors
    /// </summary>
    public class VisionServiceException : Exception
    {
        /// <summary>
        /// Gets Error details
        /// </summary>
        public RequestExceptionDetails Details { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisionServiceException"/> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="details">Request Exception details</param>
        public VisionServiceException(string message, RequestExceptionDetails details)
            : base(message)
        {
            Details = details;
        }
    }
}
