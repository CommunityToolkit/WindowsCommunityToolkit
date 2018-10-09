// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Services.MicrosoftTranslator
{
    /// <summary>
    /// Strong type for Base Detect Language
    /// </summary>
    /// <seealso cref="ITranslatorService.DetectLanguageWithResponseAsync(string)"/>
    /// <seealso cref="ITranslatorService.DetectLanguageWithResponseAsync(IEnumerable{string})"/>
    /// <seealso cref="ITranslatorService.TranslateWithResponseAsync(IEnumerable{string}, IEnumerable{string})"/>
    /// <seealso cref="ITranslatorService.TranslateWithResponseAsync(IEnumerable{string}, string, IEnumerable{string})"/>
    /// <seealso cref="ITranslatorService.TranslateWithResponseAsync(IEnumerable{string}, string, string)"/>
    /// <seealso cref="ITranslatorService.TranslateWithResponseAsync(string, IEnumerable{string})"/>
    /// <seealso cref="ITranslatorService.TranslateWithResponseAsync(string, string)"/>
    /// <seealso cref="ITranslatorService.TranslateWithResponseAsync(string, string, IEnumerable{string})"/>
    /// <seealso cref="ITranslatorService.TranslateWithResponseAsync(string, string, string)"/>
    public class DetectLanguageBase
    {
        /// <summary>
        /// Gets or sets the code of the detected language.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets a float value indicating the confidence in the result. The score is between zero and one and a low score indicates a low confidence.
        /// </summary>
        public double Score { get; set; }

        /// <inheritdoc/>
        public override string ToString() => Language;
    }
}
