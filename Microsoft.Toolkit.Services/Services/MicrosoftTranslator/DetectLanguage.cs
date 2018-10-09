// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Services.MicrosoftTranslator
{
    /// <summary>
    /// Strong type for Detect Language
    /// </summary>
    /// <seealso cref="ITranslatorService.DetectLanguageAsync(string)"/>
    public class DetectLanguage : DetectLanguageBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the detected language is one of the languages supported for text translation.
        /// </summary>
        public bool IsTranslationSupported { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the detected language is one of the languages supported for transliteration.
        /// </summary>
        public bool IsTransliterationSupported { get; set; }
    }
}
