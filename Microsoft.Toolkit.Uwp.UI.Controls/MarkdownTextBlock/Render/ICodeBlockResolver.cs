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

using Windows.UI.Xaml.Documents;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Render
{
    /// <summary>
    /// A Parser to parse code strings into Syntax Highlighted text.
    /// </summary>
    public interface ICodeBlockResolver
    {
        /// <summary>
        /// Parses Code Block text into Rich text.
        /// </summary>
        /// <param name="inlineCollection">Block to add formatted Text to.</param>
        /// <param name="text">The raw code block text</param>
        /// <param name="codeLanguage">The language of the Code Block, as specified by ```{Language} on the first line of the block,
        /// e.g. <para/>
        /// ```C# <para/>
        /// public void Method();<para/>
        /// ```<para/>
        /// </param>
        /// <returns>Parsing was handled Successfully</returns>
        bool ParseSyntax(InlineCollection inlineCollection, string text, string codeLanguage);
    }
}