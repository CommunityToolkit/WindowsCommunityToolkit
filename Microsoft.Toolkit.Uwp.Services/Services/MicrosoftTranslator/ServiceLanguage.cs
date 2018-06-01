// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
