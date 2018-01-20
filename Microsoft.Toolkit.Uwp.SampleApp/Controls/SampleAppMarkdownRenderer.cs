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
    internal class SampleAppMarkdownRenderer : MarkdownRenderer
    {
        public SampleAppMarkdownRenderer(MarkdownDocument document, ILinkRegister linkRegister, IImageResolver imageResolver, ICodeBlockResolver codeBlockResolver)
            : base(document, linkRegister, imageResolver, codeBlockResolver)
        {
        }

        /// <summary>
        /// Adds extra Addornments to Code Block, if it contains a Language.
        /// </summary>
        /// <param name="element">CodeBlock Element</param>
        /// <param name="context">Render Context</param>
        protected override void RenderCode(CodeBlock element, IRenderContext context)
        {
            base.RenderCode(element, context);
            if (string.IsNullOrWhiteSpace(element.CodeLanguage))
            {
                return;
            }

            var language = element.CodeLanguage.ToUpper();
            switch (language)
            {
                case "CSHARP":
                case "CS":
                    language = "C#";
                    break;
            }

            var localContext = context as UIElementCollectionRenderContext;
            var collection = localContext?.BlockUIElementCollection;

            if (localContext == null || collection?.Any() != true)
            {
                return;
            }

            var lastIndex = collection.Count() - 1;

            // Removes the current Code Block UI from the UI Collection, and wraps it in additional UI.
            if (collection[lastIndex] is ScrollViewer viewer)
            {
                collection.RemoveAt(lastIndex);

                var headerGrid = new Grid
                {
                    Background = ColorCode.UWP.Common.ExtensionMethods.GetSolidColorBrush("#11000000")
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
            var localContext = context as UIElementCollectionRenderContext;
            var collection = localContext?.BlockUIElementCollection;
            var format = QuoteFormat.None;
            string header = null;

            if (element.Blocks.First() is ParagraphBlock para)
            {
                if (para.Inlines.First() is TextRunInline textinline)
                {
                    var text = textinline.Text;

                    if (text.StartsWith(NoteQuote))
                    {
                        format = QuoteFormat.Note;
                        textinline.Text = text.Replace(NoteQuote, string.Empty);
                    }
                    else if (text.StartsWith(TipQuote))
                    {
                        format = QuoteFormat.Note;
                        textinline.Text = text.Replace(TipQuote, string.Empty);
                        header = "Tip";
                    }
                    else if (text.StartsWith(WarningQuote))
                    {
                        format = QuoteFormat.Warning;
                        textinline.Text = text.Replace(WarningQuote, string.Empty);
                    }
                    else if (text.StartsWith(ImportantQuote))
                    {
                        format = QuoteFormat.Important;
                        textinline.Text = text.Replace(ImportantQuote, string.Empty);
                    }
                    else if (text.StartsWith(CautionQuote))
                    {
                        format = QuoteFormat.Important;
                        textinline.Text = text.Replace(CautionQuote, string.Empty);
                        header = "Caution";
                    }

                    if (format != QuoteFormat.None)
                    {
                        if (localContext?.Clone() is UIElementCollectionRenderContext newcontext)
                        {
                            localContext = newcontext;

                            localContext.OverrideForeground = true;
                            localContext.TrimLeadingWhitespace = true;
                            header = header ?? format.ToString();
                            switch (format)
                            {
                                case QuoteFormat.Note:
                                    localContext.Foreground = noteForeground;
                                    break;

                                case QuoteFormat.Warning:
                                    localContext.Foreground = warningForeground;
                                    break;

                                case QuoteFormat.Important:
                                    localContext.Foreground = importantForeground;
                                    break;
                            }
                        }
                    }
                }
            }

            base.RenderQuote(element, localContext);

            if (format != QuoteFormat.None)
            {
                if (localContext == null || collection?.Any() != true)
                {
                    return;
                }

                // Gets the current Quote Block UI from the UI Collection, and then styles it.
                if (collection.Last() is Border border)
                {
                    border.BorderThickness = new Thickness(1);
                    border.Padding = new Thickness(10);
                    border.Margin = new Thickness(5);

                    var localborderBrush = noteBorder;
                    var localbackground = noteBackground;
                    var symbolglyph = "";

                    switch (format)
                    {
                        case QuoteFormat.Warning:
                            localborderBrush = warningBorder;
                            localbackground = warningBackground;
                            symbolglyph = "";
                            break;

                        case QuoteFormat.Important:
                            localborderBrush = importantBorder;
                            localbackground = importantBackground;
                            symbolglyph = "";
                            break;
                    }

                    border.BorderBrush = localborderBrush;
                    border.Background = localbackground;

                    var headerPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(0, 0, 0, 5)
                    };

                    headerPanel.Children.Add(new TextBlock
                    {
                        FontSize = 20,
                        Foreground = localContext.Foreground,
                        Text = symbolglyph,
                        FontFamily = new FontFamily("Segoe MDL2 Assets")
                    });

                    headerPanel.Children.Add(new TextBlock
                    {
                        FontSize = 16,
                        Foreground = localContext.Foreground,
                        Margin = new Thickness(5, 0, 0, 0),
                        Text = header.ToUpper(),
                        VerticalAlignment = VerticalAlignment.Center,
                        TextLineBounds = TextLineBounds.Tight,
                        FontWeight = FontWeights.Bold
                    });

                    if (border.Child is StackPanel panel)
                    {
                        panel.Children.Insert(0, headerPanel);
                    }
                }
            }
        }

        private enum QuoteFormat
        {
            None, Note, Warning, Important
        }

        private const string NoteQuote = "[!NOTE]";
        private const string TipQuote = "[!TIP]";
        private const string WarningQuote = "[!WARNING]";
        private const string ImportantQuote = "[!IMPORTANT]";
        private const string CautionQuote = "[!CAUTION]";

        private SolidColorBrush noteBackground = new SolidColorBrush(Color.FromArgb(255, 217, 237, 247));
        private SolidColorBrush noteBorder = new SolidColorBrush(Color.FromArgb(255, 188, 232, 241));
        private SolidColorBrush noteForeground = new SolidColorBrush(Color.FromArgb(255, 49, 112, 143));

        private SolidColorBrush warningBackground = new SolidColorBrush(Color.FromArgb(255, 252, 248, 227));
        private SolidColorBrush warningBorder = new SolidColorBrush(Color.FromArgb(255, 250, 235, 204));
        private SolidColorBrush warningForeground = new SolidColorBrush(Color.FromArgb(255, 138, 109, 59));

        private SolidColorBrush importantBackground = new SolidColorBrush(Color.FromArgb(255, 242, 222, 222));
        private SolidColorBrush importantBorder = new SolidColorBrush(Color.FromArgb(255, 235, 204, 209));
        private SolidColorBrush importantForeground = new SolidColorBrush(Color.FromArgb(255, 169, 68, 66));
    }
}