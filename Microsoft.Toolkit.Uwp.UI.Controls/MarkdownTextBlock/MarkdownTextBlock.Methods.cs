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
using System.Threading.Tasks;
using ColorCode;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Display;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// An efficient and extensible control that can parse and render markdown.
    /// </summary>
    public partial class MarkdownTextBlock
    {
        /// <summary>
        /// Called to preform a render of the current Markdown.
        /// </summary>
        private void RenderMarkdown()
        {
            // Make sure we have something to parse.
            if (string.IsNullOrWhiteSpace(Text))
            {
                return;
            }

            // Leave if we don't have our root yet.
            if (_rootElement == null)
            {
                return;
            }

            // Disconnect from OnClick handlers.
            UnhookListeners();

            var markdownRenderedArgs = new MarkdownRenderedEventArgs(null);
            try
            {
                // Try to parse the markdown.
                MarkdownDocument markdown = new MarkdownDocument();
                markdown.Parse(Text);

                // Now try to display it
                var renderer = new UWPMarkdownRenderer(markdown, this, this, this)
                {
                    Background = Background,
                    BorderBrush = BorderBrush,
                    BorderThickness = BorderThickness,
                    CharacterSpacing = CharacterSpacing,
                    FontFamily = FontFamily,
                    FontSize = FontSize,
                    FontStretch = FontStretch,
                    FontStyle = FontStyle,
                    FontWeight = FontWeight,
                    Foreground = Foreground,
                    IsTextSelectionEnabled = IsTextSelectionEnabled,
                    Padding = Padding,
                    CodeBackground = CodeBackground,
                    CodeBorderBrush = CodeBorderBrush,
                    CodeBorderThickness = CodeBorderThickness,
                    InlineCodeBorderThickness = InlineCodeBorderThickness,
                    InlineCodeBackground = InlineCodeBackground,
                    InlineCodeBorderBrush = InlineCodeBorderBrush,
                    InlineCodePadding = InlineCodePadding,
                    InlineCodeFontFamily = InlineCodeFontFamily,
                    CodeForeground = CodeForeground,
                    CodeFontFamily = CodeFontFamily,
                    CodePadding = CodePadding,
                    CodeMargin = CodeMargin,
                    EmojiFontFamily = EmojiFontFamily,
                    Header1FontSize = Header1FontSize,
                    Header1FontWeight = Header1FontWeight,
                    Header1Margin = Header1Margin,
                    Header1Foreground = Header1Foreground,
                    Header2FontSize = Header2FontSize,
                    Header2FontWeight = Header2FontWeight,
                    Header2Margin = Header2Margin,
                    Header2Foreground = Header2Foreground,
                    Header3FontSize = Header3FontSize,
                    Header3FontWeight = Header3FontWeight,
                    Header3Margin = Header3Margin,
                    Header3Foreground = Header3Foreground,
                    Header4FontSize = Header4FontSize,
                    Header4FontWeight = Header4FontWeight,
                    Header4Margin = Header4Margin,
                    Header4Foreground = Header4Foreground,
                    Header5FontSize = Header5FontSize,
                    Header5FontWeight = Header5FontWeight,
                    Header5Margin = Header5Margin,
                    Header5Foreground = Header5Foreground,
                    Header6FontSize = Header6FontSize,
                    Header6FontWeight = Header6FontWeight,
                    Header6Margin = Header6Margin,
                    Header6Foreground = Header6Foreground,
                    HorizontalRuleBrush = HorizontalRuleBrush,
                    HorizontalRuleMargin = HorizontalRuleMargin,
                    HorizontalRuleThickness = HorizontalRuleThickness,
                    ListMargin = ListMargin,
                    ListGutterWidth = ListGutterWidth,
                    ListBulletSpacing = ListBulletSpacing,
                    ParagraphMargin = ParagraphMargin,
                    QuoteBackground = QuoteBackground,
                    QuoteBorderBrush = QuoteBorderBrush,
                    QuoteBorderThickness = QuoteBorderThickness,
                    QuoteForeground = QuoteForeground,
                    QuoteMargin = QuoteMargin,
                    QuotePadding = QuotePadding,
                    TableBorderBrush = TableBorderBrush,
                    TableBorderThickness = TableBorderThickness,
                    TableCellPadding = TableCellPadding,
                    TableMargin = TableMargin,
                    TextWrapping = TextWrapping,
                    LinkForeground = LinkForeground,
                    ImageStretch = ImageStretch
                };
                _rootElement.Child = renderer.Render();
            }
            catch (Exception ex)
            {
                DebuggingReporter.ReportCriticalError("Error while parsing and rendering: " + ex.Message);
                markdownRenderedArgs = new MarkdownRenderedEventArgs(ex);
            }

            // Indicate that the parse is done.
            MarkdownRendered?.Invoke(this, markdownRenderedArgs);
        }

        private void UnhookListeners()
        {
            // Clear any hyper link events if we have any
            foreach (Hyperlink link in _listeningHyperlinks)
            {
                link.Click -= Hyperlink_Click;
            }

            // Clear everything that exists.
            _listeningHyperlinks.Clear();
        }

        /// <summary>
        /// Called when the render has a link we need to listen to.
        /// </summary>
        public void RegisterNewHyperLink(Hyperlink newHyperlink, string linkUrl)
        {
            // Setup a listener for clicks.
            newHyperlink.Click += Hyperlink_Click;

            // Associate the URL with the hyperlink.
            newHyperlink.SetValue(HyperlinkUrlProperty, linkUrl);

            // Add it to our list
            _listeningHyperlinks.Add(newHyperlink);
        }

        /// <summary>
        /// Called when the renderer needs to display a image.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        async Task<ImageSource> IImageResolver.ResolveImageAsync(string url, string tooltip)
        {
            var eventArgs = new ImageResolvingEventArgs(url, tooltip);
            ImageResolving?.Invoke(this, eventArgs);

            await eventArgs.WaitForDeferrals();

            try
            {
                return eventArgs.Handled
                                ? eventArgs.Image
                                : new BitmapImage(new Uri(url));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Called when a Code Block is being rendered.
        /// </summary>
        /// <returns>Parsing was handled Successfully</returns>
        bool ICodeBlockResolver.ParseSyntax(InlineCollection inlineCollection, string text, string codeLanguage)
        {
            var eventArgs = new CodeBlockResolvingEventArgs(inlineCollection, text, codeLanguage);
            CodeBlockResolving?.Invoke(this, eventArgs);

            try
            {
                var result = eventArgs.Handled;
                if (UseSyntaxHighlighting && !result && codeLanguage != null)
                {
                    var language = Languages.FindById(codeLanguage);
                    if (language != null)
                    {
                        var theme = RequestedTheme;
                        try
                        {
                            // Tries to get the Actual Theme, if supported.
                            theme = ActualTheme;
                        }
                        catch { }

                        var formatter = new RichTextBlockFormatter(theme);
                        formatter.FormatInlines(text, language, inlineCollection);
                        return true;
                    }
                }

                return result;
            }
            catch
            {
                return false;
            }
        }
    }
}