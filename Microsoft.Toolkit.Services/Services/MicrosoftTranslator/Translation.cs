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
        /// Gets a string giving the translated text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets a string representing the language code of the target language.
        /// </summary>
        public string To { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Translation"/> class.
        /// </summary>
        /// <param name="text">A string giving the translated text.</param>
        /// <param name="to">a string representing the language code of the target language.</param>
        public Translation(string text, string to)
        {
            Text = text;
            To = to;
        }
    }
}
