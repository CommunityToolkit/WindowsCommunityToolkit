// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftTranslator
{
    /// <summary>
    /// Holds information about an error occurred while accessing Microsoft Translator Service.
    /// </summary>
    internal class ErrorResponse
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        public int StatusCode { get; set; }
    }
}
