// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
