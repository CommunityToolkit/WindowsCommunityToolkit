﻿// ******************************************************************
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
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.Toolkit.Parsers.Markdown.Enums;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;

namespace Microsoft.Toolkit.Parsers.Markdown.Render
{
    /// <summary>
    /// A base renderer for Rendering Markdown into Controls.
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
        /// <param name="context">UI Context</param>
        public virtual void Render(IRenderContext context)
        {
            RenderBlocks(Document.Blocks, context);
        }

        /// <summary>
        /// Renders a list of block elements.
        /// </summary>
        protected virtual void RenderBlocks(IEnumerable<MarkdownBlock> blockElements, IRenderContext context)
        {
            foreach (MarkdownBlock element in blockElements)
            {
                RenderBlock(element, context);
            }
        }

        /// <summary>
        /// Called to render a block element.
        /// </summary>
        protected void RenderBlock(MarkdownBlock element, IRenderContext context)
        {
            {
                switch (element.Type)
                {
                    case MarkdownBlockType.Paragraph:
                        RenderParagraph((ParagraphBlock)element, context);
                        break;

                    case MarkdownBlockType.Quote:
                        RenderQuote((QuoteBlock)element, context);
                        break;

                    case MarkdownBlockType.Code:
                        RenderCode((CodeBlock)element, context);
                        break;

                    case MarkdownBlockType.Header:
                        RenderHeader((HeaderBlock)element, context);
                        break;

                    case MarkdownBlockType.List:
                        RenderListElement((ListBlock)element, context);
                        break;

                    case MarkdownBlockType.HorizontalRule:
                        RenderHorizontalRule(context);
                        break;

                    case MarkdownBlockType.Table:
                        RenderTable((TableBlock)element, context);
                        break;
                }
            }
        }

        /// <summary>
        /// Renders all of the children for the given element.
        /// </summary>
        /// <param name="inlineElements"> The parsed inline elements to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected void RenderInlineChildren(IList<MarkdownInline> inlineElements, IRenderContext context)
        {
            foreach (MarkdownInline element in inlineElements)
            {
                switch (element.Type)
                {
                    case MarkdownInlineType.Comment:
                    case MarkdownInlineType.LinkReference:
                        break;

                    default:
                        RenderInline(element, context);
                        break;
                }
            }
        }

        /// <summary>
        /// Called to render an inline element.
        /// </summary>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected void RenderInline(MarkdownInline element, IRenderContext context)
        {
            switch (element.Type)
            {
                case MarkdownInlineType.TextRun:
                    RenderTextRun((TextRunInline)element, context);
                    break;

                case MarkdownInlineType.Italic:
                    RenderItalicRun((ItalicTextInline)element, context);
                    break;

                case MarkdownInlineType.Bold:
                    RenderBoldRun((BoldTextInline)element, context);
                    break;

                case MarkdownInlineType.MarkdownLink:
                    CheckRenderMarkdownLink((MarkdownLinkInline)element, context);
                    break;

                case MarkdownInlineType.RawHyperlink:
                    RenderHyperlink((HyperlinkInline)element, context);
                    break;

                case MarkdownInlineType.Strikethrough:
                    RenderStrikethroughRun((StrikethroughTextInline)element, context);
                    break;

                case MarkdownInlineType.Superscript:
                    RenderSuperscriptRun((SuperscriptTextInline)element, context);
                    break;

                case MarkdownInlineType.Code:
                    RenderCodeRun((CodeInline)element, context);
                    break;

                case MarkdownInlineType.Image:
                    RenderImage((ImageInline)element, context);
                    break;

                case MarkdownInlineType.Emoji:
                    RenderEmoji((EmojiInline)element, context);
                    break;
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
        /// Verifies if the link is valid, before processing into a link, or plain text.
        /// </summary>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected void CheckRenderMarkdownLink(MarkdownLinkInline element, IRenderContext context)
        {
            // Avoid processing when link text is empty.
            if (element.Inlines.Count == 0)
            {
                return;
            }

            // Attempt to resolve references.
            element.ResolveReference(Document);
            if (element.Url == null)
            {
                // The element couldn't be resolved, just render it as text.
                RenderInlineChildren(element.Inlines, context);
            }
            else
            {
                // Url is valid, create Link.
                RenderMarkdownLink(element, context);
            }
        }

        /// <summary>
        /// Gets the markdown document that will be rendered.
        /// </summary>
        protected MarkdownDocument Document { get; }
    }
}