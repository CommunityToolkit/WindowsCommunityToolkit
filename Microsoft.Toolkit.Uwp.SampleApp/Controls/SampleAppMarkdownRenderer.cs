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
using System.Linq;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;
using Microsoft.Toolkit.Parsers.Markdown.Render;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Render;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.SampleApp.Controls
{
    /// <summary>
    /// A Rendering Superclass for the Markdown Renderer, allowing custom styling of Elements in Markdown.
    /// </summary>
    internal class SampleAppMarkdownRenderer : MarkdownRenderer
    {
        public SampleAppMarkdownRenderer(MarkdownDocument document, ILinkRegister linkRegister, IImageResolver imageResolver, ICodeBlockResolver codeBlockResolver)
            : base(document, linkRegister, imageResolver, codeBlockResolver)
        {
        }

        /// <summary>
        /// Adds extra adornments to Code Block, if it contains a Language.
        /// </summary>
        /// <param name="element">CodeBlock Element</param>
        /// <param name="context">Render Context</param>
        protected override void RenderCode(CodeBlock element, IRenderContext context)
        {
            // Renders the Code Block in the standard fashion.
            base.RenderCode(element, context);

            // Don't do any manipulations if the CodeLanguage isn't specified.
            if (string.IsNullOrWhiteSpace(element.CodeLanguage))
            {
                return;
            }

            // Unify all Code Language headers for C#.
            var language = element.CodeLanguage.ToUpper();
            switch (language)
            {
                case "CSHARP":
                case "CS":
                    language = "C#";
                    break;
            }

            // Grab the Local context and cast it.
            var localContext = context as UIElementCollectionRenderContext;
            var collection = localContext?.BlockUIElementCollection;

            // Don't go through with it, if there is an issue with the context or collection.
            if (localContext == null || collection?.Any() != true)
            {
                return;
            }

            var lastIndex = collection.Count() - 1;

            // Removes the current Code Block UI from the UI Collection, and wraps it in additional UI.
            if (collection[lastIndex] is ScrollViewer viewer)
            {
                collection.RemoveAt(lastIndex);

                // Creates a Header to specify Language and provide a copy button.
                var headerGrid = new Grid
                {
                    Background = new SolidColorBrush(Color.FromArgb(17, 0, 0, 0))
                };
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition());
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                var languageBlock = new TextBlock
                {
                    Text = language,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10, 0, 0, 0)
                };
                headerGrid.Children.Add(languageBlock);

                var copyButton = new Button
                {
                    Content = "Copy",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };

                copyButton.Click += (s, e) =>
                {
                    var content = new DataPackage();
                    content.SetText(element.Text);
                    Clipboard.SetContent(content);
                };

                headerGrid.Children.Add(copyButton);
                Grid.SetColumn(copyButton, 1);

                // Collection the adornment and the standard UI, add them to a Stackpanel, and add it back to the collection.
                var panel = new StackPanel();
                panel.Children.Add(headerGrid);
                panel.Children.Add(viewer);
                panel.Background = viewer.Background;
                panel.Margin = viewer.Margin;

                collection.Add(panel);
            }
        }

        /// <summary>
        /// Adds DocFX Note Support on top of Quote Formatting.
        /// </summary>
        /// <param name="element">QuoteBlock Element</param>
        /// <param name="context">Render Context</param>
        protected override void RenderQuote(QuoteBlock element, IRenderContext context)
        {
            // Grab the Local context and cast it.
            var localContext = context as UIElementCollectionRenderContext;
            var collection = localContext?.BlockUIElementCollection;

            // Store these, they will be changed temporarily.
            var originalQuoteForeground = QuoteForeground;
            var originalLinkForeground = LinkForeground;

            DocFXNote noteType = null;
            string header = null;
            SolidColorBrush localforeground = null;
            SolidColorBrush localbackground = null;
            string symbolglyph = string.Empty;

            // Check the required structure of the Quote is correct. Determine if it is a DocFX Note.
            if (element.Blocks.First() is ParagraphBlock para)
            {
                if (para.Inlines.First() is TextRunInline textinline)
                {
                    // Find the matching DocFX note style and header.
                    foreach (var style in styles)
                    {
                        // Search between stylisticly matching notes with different headers.
                        foreach (var identifier in style.Identifiers)
                        {
                            // Match the identifier with the start of the Quote to match.
                            if (textinline.Text.StartsWith(identifier.Key))
                            {
                                noteType = style;
                                header = identifier.Value;
                                symbolglyph = style.Glyph;

                                // Removes the identifier from the text
                                textinline.Text = textinline.Text.Replace(identifier.Key, string.Empty).TrimStart();

                                localforeground = style.LightForeground;
                                localbackground = style.LightBackground;
                            }
                        }
                    }

                    // Apply special formatting context.
                    if (noteType != null)
                    {
                        if (localContext?.Clone() is UIElementCollectionRenderContext newcontext)
                        {
                            localContext = newcontext;

                            localContext.TrimLeadingWhitespace = true;
                            QuoteForeground = Foreground;
                            LinkForeground = localforeground;
                        }
                    }
                }
            }

            // Begins the standard rendering.
            base.RenderQuote(element, localContext);

            // Add styling to render if DocFX note.
            if (noteType != null)
            {
                // Restore original formatting properties.
                QuoteForeground = originalQuoteForeground;
                LinkForeground = originalLinkForeground;

                if (localContext == null || collection?.Any() != true)
                {
                    return;
                }

                // Gets the current Quote Block UI from the UI Collection, and then styles it. Adds a header.
                if (collection.Last() is Border border)
                {
                    border.CornerRadius = new CornerRadius(6);
                    border.BorderThickness = new Thickness(0);
                    border.Padding = new Thickness(20);
                    border.Margin = new Thickness(0, 5, 0, 5);
                    border.Background = localbackground;

                    var headerPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(0, 0, 0, 10)
                    };

                    headerPanel.Children.Add(new TextBlock
                    {
                        FontSize = 18,
                        Foreground = localforeground,
                        Text = symbolglyph,
                        FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    });

                    headerPanel.Children.Add(new TextBlock
                    {
                        FontSize = 16,
                        Foreground = localforeground,
                        Margin = new Thickness(5, 0, 0, 0),
                        Text = header,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextLineBounds = TextLineBounds.Tight,
                        FontWeight = FontWeights.SemiBold
                    });

                    if (border.Child is StackPanel panel)
                    {
                        panel.Children.Insert(0, headerPanel);
                    }
                }
            }
        }

        private const string NoteGlyph = "\uE946";

        /// <summary>
        /// DocFX note types and styling info.
        /// </summary>
        private List<DocFXNote> styles = new List<DocFXNote>
        {
            new DocFXNote
            {
                Identifiers = new Dictionary<string, string> { { "[!NOTE]", "Note" } },
                Glyph = NoteGlyph,
                LightBackground = new SolidColorBrush(Color.FromArgb(255, 217, 246, 255)),
                LightForeground = new SolidColorBrush(Color.FromArgb(255, 0, 109, 140)),
                DarkBackground = new SolidColorBrush(Color.FromArgb(255, 0, 69, 89))
            },
            new DocFXNote
            {
                Identifiers = new Dictionary<string, string> { { "[!TIP]", "Tip" } },
                Glyph = "\uEA80",
                LightBackground = new SolidColorBrush(Color.FromArgb(255, 233, 250, 245)),
                LightForeground = new SolidColorBrush(Color.FromArgb(255, 0, 100, 73)),
                DarkBackground = new SolidColorBrush(Color.FromArgb(255, 0, 49, 36))
            },
            new DocFXNote
            {
                Identifiers = new Dictionary<string, string> { { "[!WARNING]", "Warning" }, { "[!CAUTION]", "Caution" } },
                Glyph = "\uEA39",
                LightBackground = new SolidColorBrush(Color.FromArgb(255, 253, 237, 238)),
                LightForeground = new SolidColorBrush(Color.FromArgb(255, 126, 17, 22)),
                DarkBackground = new SolidColorBrush(Color.FromArgb(255, 67, 9, 12))
            },
            new DocFXNote
            {
                Identifiers = new Dictionary<string, string> { { "[!IMPORTANT]", "Important" } },
                Glyph = NoteGlyph,
                LightBackground = new SolidColorBrush(Color.FromArgb(255, 238, 233, 248)),
                LightForeground = new SolidColorBrush(Color.FromArgb(255, 53, 30, 94)),
                DarkBackground = new SolidColorBrush(Color.FromArgb(255, 53, 30, 94))
            }
        };

        /// <summary>
        /// Identification and styles related to each Note type.
        /// </summary>
        private class DocFXNote
        {
            public Dictionary<string, string> Identifiers { get; set; }

            public string Glyph { get; set; }

            public string Header { get; set; }

            public SolidColorBrush LightForeground { get; set; }

            public SolidColorBrush LightBackground { get; set; }

            public SolidColorBrush DarkBackground { get; set; }
        }
    }
}