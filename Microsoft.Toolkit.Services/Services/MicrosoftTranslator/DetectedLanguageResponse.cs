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
    /// <seealso cref="ITranslatorService.DetectLanguagesWithResponseAsync(IEnumerable{string})"/>
    public class DetectedLanguageResponse : DetectedLanguage
    {
        /// <summary>
        /// Gets an array of other possible languages.
        /// </summary>
        public IEnumerable<DetectedLanguage> Alternatives { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetectedLanguageResponse"/> class.
        /// </summary>
        /// <param name="language">The code of the detected language.</param>
        /// <param name="score">A float value indicating the confidence in the result. The score is between zero and one and a low score indicates a low confidence.</param>
        /// <param name="isTranslationSupported">A value indicating whether the detected language is one of the languages supported for text translation.</param>
        /// <param name="isTransliterationSupported">A value indicating whether the detected language is one of the languages supported for transliteration.</param>
        /// <param name="alternatives">An array of other possible languages</param>
        /// <seealso cref="DetectedLanguage"/>
        public DetectedLanguageResponse(string language, float score, bool isTranslationSupported, bool isTransliterationSupported, IEnumerable<DetectedLanguage> alternatives)
            : base(language, score, isTranslationSupported, isTransliterationSupported)
        {
            Alternatives = alternatives;
        }
    }
}
