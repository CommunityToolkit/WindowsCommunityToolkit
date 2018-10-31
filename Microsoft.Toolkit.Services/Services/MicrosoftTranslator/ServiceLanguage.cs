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
        /// Gets the language code.
        /// </summary>
        public string Code { get; internal set; }

        /// <summary>
        /// Gets the language friendly name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the display name of the language in the locale native for this language.
        /// </summary>
        public string NativeName { get; }

        /// <summary>
        /// Gets the directionality, which is rtl for right-to-left languages or ltr for left-to-right languages.
        /// </summary>
        [JsonProperty("dir")]
        public string Directionality { get; }

        /// <summary>
        /// Returns the language friendly name.
        /// </summary>
        /// <returns>The language friendly name.</returns>
        public override string ToString() => Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLanguage"/> class.
        /// Returns the language friendly name.
        /// </summary>
        /// <param name="code">The language code.</param>
        /// <param name="name">The language friendly name.</param>
        /// <param name="nativeName">The display name of the language in the locale native for this language.</param>
        /// <param name="directionality">The directionality, which is rtl for right-to-left languages or ltr for left-to-right languages</param>
        public ServiceLanguage(string code, string name, string nativeName = null, string directionality = null)
        {
            Code = code;
            Name = name;
            NativeName = nativeName ?? name;
            Directionality = directionality ?? "ltr";
        }
    }
}