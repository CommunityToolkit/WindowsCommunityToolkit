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

using Microsoft.Toolkit.Parsers.Markdown.Blocks;

namespace Microsoft.Toolkit.Parsers.Markdown.Render
{
    /// <summary>
    /// Block Rendering Methods.
    /// </summary>
    public partial class MarkdownRendererBase
    {
        /// <summary>
        /// Renders a paragraph element.
        /// </summary>
        protected abstract void RenderParagraph(ParagraphBlock element, IRenderContext context);

        /// <summary>
        /// Renders a header element.
        /// </summary>
        protected abstract void RenderHeader(HeaderBlock element, IRenderContext context);

        /// <summary>
        /// Renders a list element.
        /// </summary>
        protected abstract void RenderListElement(ListBlock element, IRenderContext context);

        /// <summary>
        /// Renders a horizontal rule element.
        /// </summary>
        protected abstract void RenderHorizontalRule(IRenderContext context);

        /// <summary>
        /// Renders a quote element.
        /// </summary>
        protected abstract void RenderQuote(QuoteBlock element, IRenderContext context);

        /// <summary>
        /// Renders a code element.
        /// </summary>
        protected abstract void RenderCode(CodeBlock element, IRenderContext context);

        /// <summary>
        /// Renders a table element.
        /// </summary>
        protected abstract void RenderTable(TableBlock element, IRenderContext context);
    }
}