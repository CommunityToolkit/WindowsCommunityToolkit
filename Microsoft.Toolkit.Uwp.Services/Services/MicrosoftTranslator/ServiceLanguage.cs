// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftTranslator
{
    /// <summary>
    /// Holds information about langagues supported for text translation and speech synthesis.
    /// </summary>
    public class ServiceLanguage
    {
        /// <summary>
        /// Gets the language code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the language friendly name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns the language friendly name.
        /// </summary>
        /// <returns>The language friendly name.</returns>
        public override string ToString() => Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLanguage"/> class, using the specified code and name.
        /// </summary>
        /// <param name="code">The language code.</param>
        /// <param name="name">The language friendly name.</param>
        public ServiceLanguage(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
