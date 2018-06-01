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

using System;
using Windows.UI.Xaml.Documents;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Arguments for the <see cref="MarkdownTextBlock.CodeBlockResolving"/> event when a Code Block is being rendered.
    /// </summary>
    public class CodeBlockResolvingEventArgs : EventArgs
    {
        internal CodeBlockResolvingEventArgs(InlineCollection inlineCollection, string text, string codeLanguage)
        {
            InlineCollection = inlineCollection;
            Text = text;
            CodeLanguage = codeLanguage;
        }

        /// <summary>
        /// Gets the language of the Code Block, as specified by ```{Language} on the first line of the block,
        /// e.g. <para/>
        /// ```C# <para/>
        /// public void Method();<para/>
        /// ```<para/>
        /// </summary>
        public string CodeLanguage { get; }

        /// <summary>
        /// Gets the raw code block text
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets Collection to add formatted Text to.
        /// </summary>
        public InlineCollection InlineCollection { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this event was handled successfully.
        /// </summary>
        public bool Handled { get; set; } = false;
    }
}