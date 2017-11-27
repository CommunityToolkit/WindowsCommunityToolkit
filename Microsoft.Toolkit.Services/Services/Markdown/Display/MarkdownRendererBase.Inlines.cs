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

using Microsoft.Toolkit.Services.Markdown.Parse;

namespace Microsoft.Toolkit.Services.Markdown.Display
{
    /// <summary>
    /// Inline Rendering Methods.
    /// </summary>
    public partial class MarkdownRendererBase
    {
        /// <summary>
        /// Renders emoji element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected abstract void RenderEmoji(object inlineCollection, EmojiInline element, IRenderContext context);

        /// <summary>
        /// Renders a text run element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected abstract void RenderTextRun(object inlineCollection, TextRunInline element, IRenderContext context);

        /// <summary>
        /// Renders a bold run element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected abstract void RenderBoldRun(object inlineCollection, BoldTextInline element, IRenderContext context);

        /// <summary>
        /// Renders a link element
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected abstract void RenderMarkdownLink(object inlineCollection, MarkdownLinkInline element, IRenderContext context);

        /// <summary>
        /// Renders an image element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected abstract void RenderImage(object inlineCollection, ImageInline element, IRenderContext context);

        /// <summary>
        /// Renders a raw link element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected abstract void RenderHyperlink(object inlineCollection, HyperlinkInline element, IRenderContext context);

        /// <summary>
        /// Renders a text run element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected abstract void RenderItalicRun(object inlineCollection, ItalicTextInline element, IRenderContext context);

        /// <summary>
        /// Renders a strikethrough element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected abstract void RenderStrikethroughRun(object inlineCollection, StrikethroughTextInline element, IRenderContext context);

        /// <summary>
        /// Renders a superscript element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected abstract void RenderSuperscriptRun(object inlineCollection, SuperscriptTextInline element, IRenderContext context);

        /// <summary>
        /// Renders a code element
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected abstract void RenderCodeRun(object inlineCollection, CodeInline element, IRenderContext context);
    }
}