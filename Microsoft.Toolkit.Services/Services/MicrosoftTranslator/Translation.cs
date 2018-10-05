// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Services.MicrosoftTranslator
{
    /// <summary>
    /// Strong type for Translation
    /// </summary>
    /// <seealso cref="ITranslatorService.TranslateAsync(string, string, string)"/>
    public class Translation
    {
        /// <summary>
        /// Gets or sets a string representing the language code of the target language.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Gets or sets a string giving the translated text.
        /// </summary>
        public string Text { get; set; }
    }
}
