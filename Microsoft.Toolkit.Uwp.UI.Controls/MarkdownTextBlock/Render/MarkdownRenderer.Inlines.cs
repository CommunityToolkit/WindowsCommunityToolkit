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
using Microsoft.Toolkit.Parsers.Markdown.Enums;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;
using Microsoft.Toolkit.Parsers.Markdown.Render;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Render
{
    /// <summary>
    /// Inline UI Methods for UWP UI Creation.
    /// </summary>
    public partial class MarkdownRenderer
    {
        /// <summary>
        /// Renders emoji element.
        /// </summary>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderEmoji(EmojiInline element, IRenderContext context)
        {
            var localContext = context as InlineRenderContext;
            if (localContext == null)
            {
                throw new RenderContextIncorrectException();
            }

            var inlineCollection = localContext.InlineCollection;

            var emoji = new Run
            {
                FontFamily = EmojiFontFamily ?? DefaultEmojiFont,
                Text = element.Text
            };

            inlineCollection.Add(emoji);
        }

        /// <summary>
        /// Renders a text run element.
        /// </summary>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderTextRun(TextRunInline element, IRenderContext context)
        {
            InternalRenderTextRun(element, context);
        }

        private Run InternalRenderTextRun(TextRunInline element, IRenderContext context)
        {
            var localContext = context as InlineRenderContext;
            if (localContext == null)
            {
                throw new RenderContextIncorrectException();
            }

            var inlineCollection = localContext.InlineCollection;

            // Create the text run
            Run textRun = new Run
            {
                Text = CollapseWhitespace(context, element.Text)
            };

            // Add it
            inlineCollection.Add(textRun);
            return textRun;
        }

        /// <summary>
        /// Renders a bold run element.
        /// </summary>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderBoldRun(BoldTextInline element, IRenderContext context)
        {
            var localContext = context as InlineRenderContext;
            if (localContext == null)
            {
                throw new RenderContextIncorrectException();
            }

            // Create the text run
            Span boldSpan = new Span
            {
                FontWeight = FontWeights.Bold
            };

            var childContext = new InlineRenderContext(boldSpan.Inlines, context)
            {
                Parent = boldSpan,
                WithinBold = true
            };

            // Render the children into the bold inline.
            RenderInlineChildren(element.Inlines, childContext);

            // Add it to the current inlines
            localContext.InlineCollection.Add(boldSpan);
        }

        /// <summary>
        /// Renders a link element
        /// </summary>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderMarkdownLink(MarkdownLinkInline element, IRenderContext context)
        {
            var localContext = context as InlineRenderContext;
            if (localContext == null)
            {
                throw new RenderContextIncorrectException();
            }

            // HACK: Superscript is not allowed within a hyperlink.  But if we switch it around, so
            // that the superscript is outside the hyperlink, then it will render correctly.
            // This assumes that the entire hyperlink is to be rendered as superscript.
            if (AllTextIsSuperscript(element) == false)
            {
                // Regular ol' hyperlink.
                var link = new Hyperlink();

                // Register the link
                LinkRegister.RegisterNewHyperLink(link, element.Url);

                // Remove superscripts.
                RemoveSuperscriptRuns(element, insertCaret: true);

                // Render the children into the link inline.
                var childContext = new InlineRenderContext(link.Inlines, context)
                {
                    Parent = link,
                    WithinHyperlink = true
                };

                if (localContext.OverrideForeground)
                {
                    link.Foreground = localContext.Foreground;
                }
                else if (LinkForeground != null)
                {
                    link.Foreground = LinkForeground;
                }

                RenderInlineChildren(element.Inlines, childContext);
                context.TrimLeadingWhitespace = childContext.TrimLeadingWhitespace;

                ToolTipService.SetToolTip(link, element.Tooltip ?? element.Url);

                // Add it to the current inlines
                localContext.InlineCollection.Add(link);
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
                RenderSuperscriptRun(fakeSuperscript, context);
            }
        }

        /// <summary>
        /// Renders a raw link element.
        /// </summary>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderHyperlink(HyperlinkInline element, IRenderContext context)
        {
            var localContext = context as InlineRenderContext;
            if (localContext == null)
            {
                throw new RenderContextIncorrectException();
            }

            var link = new Hyperlink();

            // Register the link
            LinkRegister.RegisterNewHyperLink(link, element.Url);

            var brush = localContext.Foreground;
            if (LinkForeground != null && !localContext.OverrideForeground)
            {
                brush = LinkForeground;
            }

            // Make a text block for the link
            Run linkText = new Run
            {
                Text = CollapseWhitespace(context, element.Text),
                Foreground = brush
            };

            link.Inlines.Add(linkText);

            // Add it to the current inlines
            localContext.InlineCollection.Add(link);
        }

        /// <summary>
        /// Renders an image element.
        /// </summary>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override async void RenderImage(ImageInline element, IRenderContext context)
        {
            var localContext = context as InlineRenderContext;
            if (localContext == null)
            {
                throw new RenderContextIncorrectException();
            }

            var inlineCollection = localContext.InlineCollection;

            var placeholder = InternalRenderTextRun(new TextRunInline { Text = element.Text, Type = MarkdownInlineType.TextRun }, context);
            var resolvedImage = await ImageResolver.ResolveImageAsync(element.Url, element.Tooltip);

            // if image can not be resolved we have to return
            if (resolvedImage == null)
            {
                return;
            }

            var image = new Image();
            var scrollViewer = new ScrollViewer();
            var viewbox = new Viewbox();
            scrollViewer.Content = viewbox;
            viewbox.Child = image;
            var imageContainer = new InlineUIContainer() { Child = scrollViewer };

            LinkRegister.RegisterNewHyperLink(image, element.Url);

            image.Source = resolvedImage;
            image.HorizontalAlignment = HorizontalAlignment.Left;
            image.VerticalAlignment = VerticalAlignment.Top;
            image.Stretch = ImageStretch;
            scrollViewer.VerticalScrollMode = ScrollMode.Disabled;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            viewbox.StretchDirection = StretchDirection.DownOnly;

            if (ImageMaxHeight > 0)
            {
                viewbox.MaxHeight = ImageMaxHeight;
            }

            if (ImageMaxWidth > 0)
            {
                viewbox.MaxWidth = ImageMaxWidth;
            }

            if (element.ImageWidth > 0)
            {
                image.Width = element.ImageWidth;
                image.Stretch = Stretch.UniformToFill;
            }

            if (element.ImageHeight > 0)
            {
                if (element.ImageWidth == 0)
                {
                    image.Width = element.ImageHeight;
                }

                image.Height = element.ImageHeight;
                image.Stretch = Stretch.UniformToFill;
            }

            if (element.ImageHeight > 0 && element.ImageWidth > 0)
            {
                image.Stretch = Stretch.Fill;
            }

            // If image size is given then scroll to view overflown part
            if (element.ImageHeight > 0 || element.ImageWidth > 0)
            {
                scrollViewer.HorizontalScrollMode = ScrollMode.Auto;
                scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            }

            // Else resize the image
            else
            {
                scrollViewer.HorizontalScrollMode = ScrollMode.Disabled;
                scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }

            ToolTipService.SetToolTip(image, element.Tooltip);

            // Try to add it to the current inlines
            // Could fail because some containers like Hyperlink cannot have inlined images
            try
            {
                var placeholderIndex = inlineCollection.IndexOf(placeholder);
                inlineCollection.Remove(placeholder);
                inlineCollection.Insert(placeholderIndex, imageContainer);
            }
            catch
            {
                // Ignore error
            }
        }

        /// <summary>
        /// Renders a text run element.
        /// </summary>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderItalicRun(ItalicTextInline element, IRenderContext context)
        {
            var localContext = context as InlineRenderContext;
            if (localContext == null)
            {
                throw new RenderContextIncorrectException();
            }

            // Create the text run
            Span italicSpan = new Span
            {
                FontStyle = FontStyle.Italic
            };

            var childContext = new InlineRenderContext(italicSpan.Inlines, context)
            {
                Parent = italicSpan,
                WithinItalics = true
            };

            // Render the children into the italic inline.
            RenderInlineChildren(element.Inlines, childContext);

            // Add it to the current inlines
            localContext.InlineCollection.Add(italicSpan);
        }

        /// <summary>
        /// Renders a strikethrough element.
        /// </summary>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderStrikethroughRun(StrikethroughTextInline element, IRenderContext context)
        {
            var localContext = context as InlineRenderContext;
            if (localContext == null)
            {
                throw new RenderContextIncorrectException();
            }

            Span span = new Span();

            if (TextDecorationsSupported)
            {
                span.TextDecorations = TextDecorations.Strikethrough;
            }
            else
            {
                span.FontFamily = new FontFamily("Consolas");
            }

            var childContext = new InlineRenderContext(span.Inlines, context)
            {
                Parent = span
            };

            // Render the children into the inline.
            RenderInlineChildren(element.Inlines, childContext);

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
            localContext.InlineCollection.Add(span);
        }

        /// <summary>
        /// Renders a superscript element.
        /// </summary>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderSuperscriptRun(SuperscriptTextInline element, IRenderContext context)
        {
            var localContext = context as InlineRenderContext;
            var parent = localContext?.Parent as TextElement;
            if (localContext == null && parent == null)
            {
                throw new RenderContextIncorrectException();
            }

            // Le <sigh>, InlineUIContainers are not allowed within hyperlinks.
            if (localContext.WithinHyperlink)
            {
                RenderInlineChildren(element.Inlines, context);
                return;
            }

            var paragraph = new Paragraph
            {
                FontSize = parent.FontSize * 0.8,
                FontFamily = parent.FontFamily,
                FontStyle = parent.FontStyle,
                FontWeight = parent.FontWeight
            };

            var childContext = new InlineRenderContext(paragraph.Inlines, context)
            {
                Parent = paragraph
            };

            RenderInlineChildren(element.Inlines, childContext);

            var richTextBlock = CreateOrReuseRichTextBlock(new UIElementCollectionRenderContext(null, context));
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
            localContext.InlineCollection.Add(inlineUIContainer);
        }

        /// <summary>
        /// Renders a code element
        /// </summary>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        protected override void RenderCodeRun(CodeInline element, IRenderContext context)
        {
            var localContext = context as InlineRenderContext;
            if (localContext == null)
            {
                throw new RenderContextIncorrectException();
            }

            var text = CreateTextBlock(localContext);
            text.Text = CollapseWhitespace(context, element.Text);
            text.FontFamily = InlineCodeFontFamily ?? FontFamily;

            if (localContext.WithinItalics)
            {
                text.FontStyle = FontStyle.Italic;
            }

            if (localContext.WithinBold)
            {
                text.FontWeight = FontWeights.Bold;
            }

            var borderthickness = InlineCodeBorderThickness;
            var padding = InlineCodePadding;
            var spacingoffset = -(borderthickness.Bottom + padding.Bottom);

            var margin = new Thickness(0, spacingoffset, 0, spacingoffset);

            var border = new Border
            {
                BorderThickness = borderthickness,
                BorderBrush = InlineCodeBorderBrush,
                Background = InlineCodeBackground,
                Child = text,
                Padding = padding,
                Margin = margin
            };

            // Aligns content in InlineUI, see https://social.msdn.microsoft.com/Forums/silverlight/en-US/48b5e91e-efc5-4768-8eaf-f897849fcf0b/richtextbox-inlineuicontainer-vertical-alignment-issue?forum=silverlightarchieve
            border.RenderTransform = new TranslateTransform
            {
                Y = 4
            };

            var inlineUIContainer = new InlineUIContainer
            {
                Child = border,
            };

            // Add it to the current inlines
            localContext.InlineCollection.Add(inlineUIContainer);
        }
    }
}