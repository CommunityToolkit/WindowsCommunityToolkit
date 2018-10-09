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
        /// Gets or sets a <see cref="DetectLanguageBase"/> object describing the detected language.
        /// </summary>
        /// <remarks>This property has a value only when the <see cref="ITranslatorService.TranslateWithResponseAsync(string, string)"/> method is invoked without the <strong>from</strong> parameter, so that automatic language detection is applied to determine the source language.
        /// </remarks>
        public DetectLanguageBase DetectedLanguage { get; set; }

        /// <summary>
        /// Gets or sets an array of translation results.
        /// </summary>
        public IEnumerable<Translation> Translations { get; set; }

        /// <summary>
        /// Gets the first translation result.
        /// </summary>
        public Translation Translation => Translations?.FirstOrDefault();
    }
}
