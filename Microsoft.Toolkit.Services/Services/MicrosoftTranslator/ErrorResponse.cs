// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Microsoft.Toolkit.Services.MicrosoftTranslator
{
#pragma warning disable SA1402 // File may only contain a single type
    /// <summary>
    /// Holds information about an error occurred while accessing Microsoft Translator Service.
    /// </summary>
    internal class ErrorResponse
    {
        [JsonPropertyName("error")]
        public Error Error { get; set; }
    }

    internal class Error
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
#pragma warning restore SA1402 // File may only contain a single type
}
