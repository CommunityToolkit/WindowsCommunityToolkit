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

using Microsoft.Toolkit.Parsers.Markdown.Render;
using Windows.UI.Xaml.Documents;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Render
{
    /// <summary>
    /// The Context of the Current Document Rendering.
    /// </summary>
    public class InlineRenderContext : RenderContext
    {
        internal InlineRenderContext(InlineCollection inlineCollection, IRenderContext context)
        {
            InlineCollection = inlineCollection;
            Foreground = ((RenderContext)context).Foreground;
            TrimLeadingWhitespace = context.TrimLeadingWhitespace;
            Parent = context.Parent;

            if (context is InlineRenderContext inlinecontext)
            {
                WithinBold = inlinecontext.WithinBold;
                WithinItalics = inlinecontext.WithinItalics;
                WithinHyperlink = inlinecontext.WithinHyperlink;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Current Element is being rendered inside an Italics Run.
        /// </summary>
        public bool WithinItalics { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Current Element is being rendered inside a Bold Run.
        /// </summary>
        public bool WithinBold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Current Element is being rendered inside a Link.
        /// </summary>
        public bool WithinHyperlink { get; set; }

        /// <summary>
        /// Gets or sets the list to add to.
        /// </summary>
        public InlineCollection InlineCollection { get; set; }
    }
}