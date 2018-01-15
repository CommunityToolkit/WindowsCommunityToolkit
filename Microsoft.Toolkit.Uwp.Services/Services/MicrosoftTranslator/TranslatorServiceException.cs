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

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftTranslator
{
    /// <summary>
    /// The <strong>TranslatorServiceException</strong> class holds information about Exception related to <see cref="TranslatorService"/>.
    /// </summary>
    /// <seealso cref="TranslatorService"/>
    public class TranslatorServiceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatorServiceException"/> class using the specified error message.
        /// </summary>
        /// <param name="message">Message that describes the error</param>
        public TranslatorServiceException(string message)
            : base(message)
        {
        }
    }
}
