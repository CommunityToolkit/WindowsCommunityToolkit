// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.MicrosoftTranslator
{
    /// <summary>
    /// Strong type for Language
    /// </summary>
    /// <seealso cref="ITranslatorService.GetLanguagesAsync(string)"/>
    public class Language
    {
        /// <summary>
        /// Gets or sets the display name of the language in the locale requested via Accept-Language header.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display name of the language in the locale native for this language.
        /// </summary>
        public string NativeName { get; set; }

        /// <summary>
        /// Gets or sets the directionality, which is rtl for right-to-left languages or ltr for left-to-right languages.
        /// </summary>
        [JsonProperty("dir")]
        public string Directionality { get; set; }

        /// <inheritdoc/>
        public override string ToString() => Name;
    }
}
