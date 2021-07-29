// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Services.MicrosoftTranslator
{
    /// <summary>
    /// Strong type for Translate Response
    /// </summary>
    /// <seealso cref="ITranslatorService.TranslateAsync(string, string, string)"/>
    public class TranslationResponse
    {
        /// <summary>
        /// Gets a <see cref="DetectedLanguageBase"/> object describing the detected language.
        /// </summary>
        /// <remarks>This property has a value only when the <see cref="ITranslatorService.TranslateWithResponseAsync(string, string)"/> method is invoked without the <strong>from</strong> parameter, so that automatic language detection is applied to determine the source language.
        /// </remarks>
        public DetectedLanguageBase DetectedLanguage { get; }

        /// <summary>
        /// Gets an array of <see cref="MicrosoftTranslator.Translation"/> results.
        /// </summary>
        public IEnumerable<Translation> Translations { get; }

        /// <summary>
        /// Gets the first translation result.
        /// </summary>
        public Translation Translation => Translations?.FirstOrDefault();

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationResponse"/> class.
        /// </summary>
        /// <param name="detectedLanguage">A <see cref="DetectedLanguageBase"/> object describing the detected language.</param>
        /// <param name="translations">an array of <see cref="MicrosoftTranslator.Translation"/> results.</param>
        /// <seealso cref="MicrosoftTranslator.Translation"/>
        /// <seealso cref="DetectedLanguageBase"/>
        public TranslationResponse(DetectedLanguageBase detectedLanguage, IEnumerable<Translation> translations)
        {
            DetectedLanguage = detectedLanguage;
            Translations = translations;
        }
    }
}
