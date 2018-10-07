// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.MicrosoftTranslator
{
    /// <summary>
    /// Holds information about langagues supported for text translation and speech synthesis.
    /// </summary>
    /// <seealso cref="ITranslatorService.GetLanguageNamesAsync(string)"/>
    public class ServiceLanguage
    {
        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the language friendly name.
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

        /// <summary>
        /// Returns the language friendly name.
        /// </summary>
        /// <returns>The language friendly name.</returns>
        public override string ToString() => Name;
    }
}