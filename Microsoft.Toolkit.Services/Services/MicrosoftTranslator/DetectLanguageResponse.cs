// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Services.MicrosoftTranslator
{
    /// <summary>
    /// Strong type for Detect Language Response
    /// </summary>
    /// <seealso cref="ITranslatorService.DetectLanguageWithResponseAsync(string)"/>
    public class DetectLanguageResponse : DetectLanguage
    {
        /// <summary>
        /// Gets or sets an array of other possible languages
        /// </summary>
        public IEnumerable<DetectLanguage> Alternatives { get; set; }
    }
}
