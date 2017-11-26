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
using System.Collections.Generic;
using System.Text;
using Microsoft.Toolkit.Services.Markdown.Display;
using Microsoft.Toolkit.Services.Markdown.Parse;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Display
{
    /// <summary>
    /// Inline UI Methods for UWP UI Creation.
    /// </summary>
    internal partial class UWPMarkdownRenderer
    {
        /// <summary>
        /// Renders emoji element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderEmoji(object inlineCollection, EmojiInline element, IRenderContext context)
        {
            var inlineCollection_ = inlineCollection as InlineCollection;

            var emoji = new Run
            {
                FontFamily = EmojiFontFamily ?? _defaultEmojiFont,
                Text = element.Text
            };

            inlineCollection_.Add(emoji);
        }

        /// <summary>
        /// Renders a text run element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderTextRun(object inlineCollection, TextRunInline element, IRenderContext context)
        {
            InternalRenderTextRun(inlineCollection, element, context);
        }

        private Run InternalRenderTextRun(object inlineCollection, TextRunInline element, IRenderContext context)
        {
            var inlineCollection_ = inlineCollection as InlineCollection;

            // Create the text run
            Run textRun = new Run
            {
                Text = CollapseWhitespace(context, element.Text)
            };

            // Add it
            inlineCollection_.Add(textRun);
            return textRun;
        }

        /// <summary>
        /// Renders a bold run element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderBoldRun(object inlineCollection, BoldTextInline element, IRenderContext context)
        {
            var inlineCollection_ = inlineCollection as InlineCollection;

            // Create the text run
            Span boldSpan = new Span
            {
                FontWeight = FontWeights.Bold
            };

            // Render the children into the bold inline.
            RenderInlineChildren(boldSpan.Inlines, element.Inlines, boldSpan, context);

            // Add it to the current inlines
            inlineCollection_.Add(boldSpan);
        }

        /// <summary>
        /// Renders a link element
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="parent"> The container element. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderMarkdownLink(object inlineCollection, MarkdownLinkInline element, object parent, IRenderContext context)
        {
            var inlineCollection_ = inlineCollection as InlineCollection;

            // HACK: Superscript is not allowed within a hyperlink.  But if we switch it around, so
            // that the superscript is outside the hyperlink, then it will render correctly.
            // This assumes that the entire hyperlink is to be rendered as superscript.
            if (AllTextIsSuperscript(element) == false)
            {
                // Regular ol' hyperlink.
                var link = new Hyperlink();

                // Register the link
                _linkRegister.RegisterNewHyperLink(link, element.Url);

                // Remove superscripts.
                RemoveSuperscriptRuns(element, insertCaret: true);

                // Render the children into the link inline.
                var childContext = context.Clone();
                childContext.WithinHyperlink = true;

                if (LinkForeground != null)
                {
                    link.Foreground = LinkForeground;
                }

                RenderInlineChildren(link.Inlines, element.Inlines, link, childContext);
                context.TrimLeadingWhitespace = childContext.TrimLeadingWhitespace;

                // Add it to the current inlines
                inlineCollection_.Add(link);
            }
            else
            {
                // THE HACK IS ON!

                // Create a fake superscript element.
                var fakeSuperscript = new SuperscriptTextInline
                {
                    Inlines = new List<MarkdownInline>
                    {
                        element
                    }
                };

                // Remove superscripts.
                RemoveSuperscriptRuns(element, insertCaret: false);

                // Now render it.
                RenderSuperscriptRun(inlineCollection, fakeSuperscript, parent, context);
            }
        }

        /// <summary>
        /// Renders a raw link element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderHyperlink(object inlineCollection, HyperlinkInline element, IRenderContext context)
        {
            var inlineCollection_ = inlineCollection as InlineCollection;
            var context_ = context as RenderContext;

            var link = new Hyperlink();

            // Register the link
            _linkRegister.RegisterNewHyperLink(link, element.Url);

            // Make a text block for the link
            Run linkText = new Run
            {
                Text = CollapseWhitespace(context, element.Text),
                Foreground = LinkForeground ?? context_.Foreground
            };

            link.Inlines.Add(linkText);

            // Add it to the current inlines
            inlineCollection_.Add(link);
        }

        /// <summary>
        /// Renders an image element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override async void RenderImage(object inlineCollection, ImageInline element, IRenderContext context)
        {
            var inlineCollection_ = inlineCollection as InlineCollection;

            var placeholder = InternalRenderTextRun(inlineCollection, new TextRunInline { Text = element.Text, Type = MarkdownInlineType.TextRun }, context);
            var resolvedImage = await _imageResolver.ResolveImageAsync(element.Url, element.Tooltip);

            // if image can not be resolved we have to return
            if (resolvedImage == null)
            {
                return;
            }

            var image = new Image();
            var imageContainer = new InlineUIContainer() { Child = image };

            image.Source = resolvedImage;
            image.HorizontalAlignment = HorizontalAlignment.Left;
            image.VerticalAlignment = VerticalAlignment.Top;
            image.Stretch = ImageStretch;

            ToolTipService.SetToolTip(image, element.Tooltip);

            // Try to add it to the current inlines
            // Could fail because some containers like Hyperlink cannot have inlined images
            try
            {
                var placeholderIndex = inlineCollection_.IndexOf(placeholder);
                inlineCollection_.Remove(placeholder);
                inlineCollection_.Insert(placeholderIndex, imageContainer);
            }
            catch
            {
                // Ignore error
            }
        }

        /// <summary>
        /// Renders a text run element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderItalicRun(object inlineCollection, ItalicTextInline element, IRenderContext context)
        {
            var inlineCollection_ = inlineCollection as InlineCollection;

            // Create the text run
            Span italicSpan = new Span
            {
                FontStyle = FontStyle.Italic
            };

            // Render the children into the italic inline.
            RenderInlineChildren(italicSpan.Inlines, element.Inlines, italicSpan, context);

            // Add it to the current inlines
            inlineCollection_.Add(italicSpan);
        }

        /// <summary>
        /// Renders a strikethrough element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderStrikethroughRun(object inlineCollection, StrikethroughTextInline element, IRenderContext context)
        {
            var inlineCollection_ = inlineCollection as InlineCollection;
            Span span = new Span();

            if (TextDecorationsSupported)
            {
                span.TextDecorations = TextDecorations.Strikethrough;
            }
            else
            {
                span.FontFamily = new FontFamily("Consolas");
            }

            // Render the children into the inline.
            RenderInlineChildren(span.Inlines, element.Inlines, span, context);

            if (!TextDecorationsSupported)
            {
                AlterChildRuns(span, (parentSpan, run) =>
                {
                    var text = run.Text;
                    var builder = new StringBuilder(text.Length * 2);
                    foreach (var c in text)
                    {
                        builder.Append((char)0x0336);
                        builder.Append(c);
                    }
                    run.Text = builder.ToString();
                });
            }

            // Add it to the current inlines
            inlineCollection_.Add(span);
        }

        /// <summary>
        /// Renders a superscript element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="parent"> The container element. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderSuperscriptRun(object inlineCollection, SuperscriptTextInline element, object parent, IRenderContext context)
        {
            var inlineCollection_ = inlineCollection as InlineCollection;
            var parent_ = parent as TextElement;

            // Le <sigh>, InlineUIContainers are not allowed within hyperlinks.
            if (context.WithinHyperlink)
            {
                RenderInlineChildren(inlineCollection, element.Inlines, parent, context);
                return;
            }

            var paragraph = new Paragraph
            {
                FontSize = parent_.FontSize * 0.8,
                FontFamily = parent_.FontFamily,
                FontStyle = parent_.FontStyle,
                FontWeight = parent_.FontWeight
            };
            RenderInlineChildren(paragraph.Inlines, element.Inlines, paragraph, context);

            var richTextBlock = CreateOrReuseRichTextBlock(null, context);
            richTextBlock.Blocks.Add(paragraph);

            var border = new Border
            {
                Padding = new Thickness(0, 0, 0, paragraph.FontSize * 0.2),
                Child = richTextBlock
            };

            var inlineUIContainer = new InlineUIContainer
            {
                Child = border
            };

            // Add it to the current inlines
            inlineCollection_.Add(inlineUIContainer);
        }

        /// <summary>
        /// Renders a code element
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderCodeRun(object inlineCollection, CodeInline element, IRenderContext context)
        {
            var inlineCollection_ = inlineCollection as InlineCollection;

            var run = new Run
            {
                FontFamily = CodeFontFamily ?? FontFamily,
                Text = CollapseWhitespace(context, element.Text)
            };

            // Add it to the current inlines
            inlineCollection_.Add(run);
        }
    }
}