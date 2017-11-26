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

using System.Collections.Generic;
using System.Text;
using Microsoft.Toolkit.Services.Markdown.Helpers;
using Microsoft.Toolkit.Services.Markdown.Parse;

namespace Microsoft.Toolkit.Services.Markdown.Display
{
    /// <summary>
    /// A base structure for Rendering Markdown into Controls.
    /// </summary>
    public abstract partial class MarkdownRendererBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownRendererBase"/> class.
        /// </summary>
        /// <param name="document">Markdown Document to Render</param>
        public MarkdownRendererBase(MarkdownDocument document)
        {
            Document = document;
        }

        /// <summary>
        /// Renders all Content to the Provided Parent UI.
        /// </summary>
        /// <param name="parent">Parent UI Container.</param>
        /// <param name="context">UI Context</param>
        public virtual void Render(object parent, IRenderContext context)
        {
            RenderBlocks(Document.Blocks, parent, context);
        }

        /// <summary>
        /// Renders a list of block elements.
        /// </summary>
        protected abstract void RenderBlocks(IEnumerable<MarkdownBlock> blockElements, object blockUIElementCollection, IRenderContext context);

        /// <summary>
        /// Called to render a block element.
        /// </summary>
        protected void RenderBlock(MarkdownBlock element, object blockUIElementCollection, IRenderContext context)
        {
            {
                switch (element.Type)
                {
                    case MarkdownBlockType.Paragraph:
                        RenderParagraph((ParagraphBlock)element, blockUIElementCollection, context);
                        break;

                    case MarkdownBlockType.Quote:
                        RenderQuote((QuoteBlock)element, blockUIElementCollection, context);
                        break;

                    case MarkdownBlockType.Code:
                        RenderCode((CodeBlock)element, blockUIElementCollection, context);
                        break;

                    case MarkdownBlockType.Header:
                        RenderHeader((HeaderBlock)element, blockUIElementCollection, context);
                        break;

                    case MarkdownBlockType.List:
                        RenderListElement((ListBlock)element, blockUIElementCollection, context);
                        break;

                    case MarkdownBlockType.HorizontalRule:
                        RenderHorizontalRule(blockUIElementCollection, context);
                        break;

                    case MarkdownBlockType.Table:
                        RenderTable((TableBlock)element, blockUIElementCollection, context);
                        break;
                }
            }
        }

        /// <summary>
        /// Renders all of the children for the given element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="inlineElements"> The parsed inline elements to render. </param>
        /// <param name="parent"> The container element. </param>
        /// <param name="context"> Persistent state. </param>
        protected void RenderInlineChildren(object inlineCollection, IList<MarkdownInline> inlineElements, object parent, IRenderContext context)
        {
            foreach (MarkdownInline element in inlineElements)
            {
                RenderInline(inlineCollection, element, parent, context);
            }
        }

        /// <summary>
        /// Called to render an inline element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="parent"> The container element. </param>
        /// <param name="context"> Persistent state. </param>
        protected void RenderInline(object inlineCollection, MarkdownInline element, object parent, IRenderContext context)
        {
            switch (element.Type)
            {
                case MarkdownInlineType.TextRun:
                    RenderTextRun(inlineCollection, (TextRunInline)element, context);
                    break;

                case MarkdownInlineType.Italic:
                    RenderItalicRun(inlineCollection, (ItalicTextInline)element, context);
                    break;

                case MarkdownInlineType.Bold:
                    RenderBoldRun(inlineCollection, (BoldTextInline)element, context);
                    break;

                case MarkdownInlineType.MarkdownLink:
                    RenderMarkdownLink(inlineCollection, (MarkdownLinkInline)element, parent, context);
                    break;

                case MarkdownInlineType.RawHyperlink:
                    RenderHyperlink(inlineCollection, (HyperlinkInline)element, context);
                    break;

                case MarkdownInlineType.Strikethrough:
                    RenderStrikethroughRun(inlineCollection, (StrikethroughTextInline)element, context);
                    break;

                case MarkdownInlineType.Superscript:
                    RenderSuperscriptRun(inlineCollection, (SuperscriptTextInline)element, parent, context);
                    break;

                case MarkdownInlineType.Code:
                    RenderCodeRun(inlineCollection, (CodeInline)element, context);
                    break;

                case MarkdownInlineType.Image:
                    RenderImage(inlineCollection, (ImageInline)element, context);
                    break;

                case MarkdownInlineType.Emoji:
                    RenderEmoji(inlineCollection, (EmojiInline)element, context);
                    break;
            }
        }

        /// <summary>
        /// Checks if all text elements inside the given container are superscript.
        /// </summary>
        /// <returns> <c>true</c> if all text is superscript (level 1); <c>false</c> otherwise. </returns>
        protected bool AllTextIsSuperscript(IInlineContainer container, int superscriptLevel = 0)
        {
            foreach (var inline in container.Inlines)
            {
                var textInline = inline as SuperscriptTextInline;
                if (textInline != null)
                {
                    // Remove any nested superscripts.
                    if (AllTextIsSuperscript(textInline, superscriptLevel + 1) == false)
                    {
                        return false;
                    }
                }
                else if (inline is IInlineContainer)
                {
                    // Remove any superscripts.
                    if (AllTextIsSuperscript((IInlineContainer)inline, superscriptLevel) == false)
                    {
                        return false;
                    }
                }
                else if (inline is IInlineLeaf && !Common.IsBlankOrWhiteSpace(((IInlineLeaf)inline).Text))
                {
                    if (superscriptLevel != 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Removes all superscript elements from the given container.
        /// </summary>
        protected void RemoveSuperscriptRuns(IInlineContainer container, bool insertCaret)
        {
            for (int i = 0; i < container.Inlines.Count; i++)
            {
                var inline = container.Inlines[i];
                var textInline = inline as SuperscriptTextInline;
                if (textInline != null)
                {
                    // Remove any nested superscripts.
                    RemoveSuperscriptRuns(textInline, insertCaret);

                    // Remove the superscript element, insert all the children.
                    container.Inlines.RemoveAt(i);
                    if (insertCaret)
                    {
                        container.Inlines.Insert(i++, new TextRunInline { Text = "^" });
                    }

                    foreach (var superscriptInline in textInline.Inlines)
                    {
                        container.Inlines.Insert(i++, superscriptInline);
                    }

                    i--;
                }
                else if (inline is IInlineContainer)
                {
                    // Remove any superscripts.
                    RemoveSuperscriptRuns((IInlineContainer)inline, insertCaret);
                }
            }
        }

        /// <summary>
        /// Removes leading whitespace, but only if this is the first run in the block.
        /// </summary>
        /// <returns>The corrected string</returns>
        protected string CollapseWhitespace(IRenderContext context, string text)
        {
            bool dontOutputWhitespace = context.TrimLeadingWhitespace;
            StringBuilder result = null;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c == ' ' || c == '\t')
                {
                    if (dontOutputWhitespace)
                    {
                        if (result == null)
                        {
                            result = new StringBuilder(text.Substring(0, i), text.Length);
                        }
                    }
                    else
                    {
                        result?.Append(c);

                        dontOutputWhitespace = true;
                    }
                }
                else
                {
                    result?.Append(c);

                    dontOutputWhitespace = false;
                }
            }

            context.TrimLeadingWhitespace = false;
            return result == null ? text : result.ToString();
        }

        /// <summary>
        /// Gets the markdown document that will be rendered.
        /// </summary>
        protected MarkdownDocument Document { get; }
    }
}