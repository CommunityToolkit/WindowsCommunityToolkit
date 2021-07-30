// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Services.MicrosoftTranslator
{
    /// <summary>
    /// Strong type for Base Detected Language
    /// </summary>
    /// <seealso cref="ITranslatorService.DetectLanguageWithResponseAsync(string)"/>
    /// <seealso cref="ITranslatorService.DetectLanguagesWithResponseAsync(IEnumerable{string})"/>
    /// <seealso cref="ITranslatorService.TranslateWithResponseAsync(IEnumerable{string}, IEnumerable{string})"/>
    /// <seealso cref="ITranslatorService.TranslateWithResponseAsync(IEnumerable{string}, string, IEnumerable{string})"/>
    /// <seealso cref="ITranslatorService.TranslateWithResponseAsync(IEnumerable{string}, string, string)"/>
    /// <seealso cref="ITranslatorService.TranslateWithResponseAsync(string, IEnumerable{string})"/>
    /// <seealso cref="ITranslatorService.TranslateWithResponseAsync(string, string)"/>
    /// <seealso cref="ITranslatorService.TranslateWithResponseAsync(string, string, IEnumerable{string})"/>
    /// <seealso cref="ITranslatorService.TranslateWithResponseAsync(string, string, string)"/>
    public class DetectedLanguageBase
    {
        /// <summary>
        /// Gets the code of the detected language.
        /// </summary>
        public string Language { get; }

        /// <summary>
        /// Gets a float value indicating the confidence in the result. The score is between zero and one and a low score indicates a low confidence.
        /// </summary>
        public float Score { get; }

        /// <inheritdoc/>
        public override string ToString() => Language;

        /// <summary>
        /// Initializes a new instance of the <see cref="DetectedLanguageBase"/> class.
        /// Returns the language friendly name.
        /// </summary>
        /// <param name="language">the code of the detected language.</param>
        /// <param name="score">a float value indicating the confidence in the result. The score is between zero and one and a low score indicates a low confidence.</param>
        public DetectedLanguageBase(string language, float score)
        {
            Language = language;
            Score = score;
        }
    }
}
