// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;
using Microsoft.Toolkit.Parsers.Markdown.Render;
using Microsoft.Toolkit.Uwp.Helpers;
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
            LanguageRequested += SampleAppMarkdownRenderer_LanguageRequested;
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
            var prevIndex = lastIndex - 1;

            // Removes the current Code Block UI from the UI Collection, and wraps it in additional UI.
            if (collection[lastIndex] is ScrollViewer viewer)
            {
                collection.RemoveAt(lastIndex);

                // Combine Code Blocks if a Different Language.
                if (language != "XAML"
                    && prevIndex >= 0
                    && collection[prevIndex] is StackPanel prevPanel
                    && prevPanel.Tag is CustCodeBlock block
                    && !block.Languages.ContainsKey("XAML") // Prevent combining of XAML Code Blocks.
                    && !block.Languages.ContainsKey(language))
                {
                    // Add New Lang to Existing Block
#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
                    block.Languages.Add(language, (viewer, element.Text));
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly

                    if (prevPanel.Children.FirstOrDefault() is Grid headerGrid)
                    {
                        var langHead = headerGrid.Children.FirstOrDefault();
                        if (langHead is TextBlock textLangHead)
                        {
                            // Replace TextBlock with ComboBox
                            headerGrid.Children.Remove(textLangHead);
                            var combLangHead = new ComboBox
                            {
                                Items =
                                {
                                    textLangHead.Text,
                                    language
                                },
                                SelectedIndex = 0,
                                MinWidth = 80
                            };

                            headerGrid.Children.Add(combLangHead);

                            combLangHead.SelectionChanged += (s, e) =>
                            {
                                var newLang = combLangHead.SelectedItem as string;
                                block.CurrentLanguage = newLang;
                                LanguageRequested?.Invoke(combLangHead, newLang);

                                var newViewer = block.Languages[newLang].viewer;

                                // Remove old Viewer.
                                var lastItem = prevPanel.Children.Count - 1;
                                if (lastItem >= 0)
                                {
                                    prevPanel.Children.RemoveAt(lastItem);
                                }

                                prevPanel.Children.Add(newViewer);
                            };

                            LanguageRequested += (s, e) =>
                            {
                                if (s != combLangHead)
                                {
                                    if (combLangHead.Items.Contains(e))
                                    {
                                        combLangHead.SelectedItem = e;
                                        block.CurrentLanguage = e;
                                    }
                                }
                            };

                            if (DesiredLang == language)
                            {
                                combLangHead.SelectedItem = language;
                                block.CurrentLanguage = language;
                            }
                        }
                        else if (langHead is ComboBox combLangHead)
                        {
                            // Add Lang to ComboBox
#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
                            block.Languages.Add(language, (viewer, element.Text));
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
                            combLangHead.Items.Add(language);

                            if (DesiredLang == language)
                            {
                                combLangHead.SelectedItem = language;
                                block.CurrentLanguage = language;
                            }
                        }
                    }
                }
                else
                {
                    block = new CustCodeBlock();
#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
                    block.Languages.Add(language, (viewer, element.Text));
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
                    block.CurrentLanguage = language;

                    // Creates a Header to specify Language and provide a copy button.
                    var headerGrid = new Grid
                    {
                        Background = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0))
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
                        var text = block.Languages[block.CurrentLanguage].text;

                        var content = new DataPackage();
                        content.SetText(text);
                        Clipboard.SetContent(content);
                    };

                    headerGrid.Children.Add(copyButton);
                    Grid.SetColumn(copyButton, 1);

                    // Collection the adornment and the standard UI, add them to a Stackpanel, and add it back to the collection.
                    var panel = new StackPanel
                    {
                        Background = viewer.Background,
                        Margin = viewer.Margin,
                        Tag = block
                    };

                    panel.Children.Add(headerGrid);
                    panel.Children.Add(viewer);

                    collection.Add(panel);
                }
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

            var theme = Shell.Current.GetActualTheme();

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
                                textinline.Text = textinline.Text.Replace(identifier.Key, string.Empty);

                                if (theme == ElementTheme.Light)
                                {
                                    localforeground = style.LightForeground;
                                    localbackground = style.LightBackground;
                                }
                                else
                                {
                                    localforeground = new SolidColorBrush(Colors.White);
                                    localbackground = style.DarkBackground;
                                }
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

                    if (theme == ElementTheme.Light)
                    {
                        border.BorderThickness = new Thickness(0.5);
                        border.BorderBrush = localforeground;
                    }

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

        /// <summary>
        /// Sets the Desired Language for the Sample App.
        /// </summary>
        /// <param name="sender">Language Combobox</param>
        /// <param name="e">New Language</param>
        private void SampleAppMarkdownRenderer_LanguageRequested(object sender, string e)
        {
            DesiredLang = e;
        }

        /// <summary>
        /// The Note Glyph.
        /// </summary>
        private const string NoteGlyph = "\uE946";

        /// <summary>
        /// The Key for Settings.
        /// </summary>
        private const string DesiredLangKey = "Docs-DesiredLang";

        /// <summary>
        /// Gets or sets the Desired Language from Settings.
        /// </summary>
        public string DesiredLang
        {
            get
            {
                return storage.Read<string>(DesiredLangKey);
            }

            set
            {
                storage.Save(DesiredLangKey, value);
            }
        }

        /// <summary>
        /// The Local Storage Helper.
        /// </summary>
        private LocalObjectStorageHelper storage = new LocalObjectStorageHelper();

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
        /// The Event if a Language change is requested.
        /// </summary>
        public event EventHandler<string> LanguageRequested;

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

        /// <summary>
        /// Code Block Tag Information to track current Language and Alternate Views.
        /// </summary>
        private class CustCodeBlock
        {
#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
#pragma warning disable SA1009 // Closing parenthesis must be spaced correctly
            public Dictionary<string, (FrameworkElement viewer, string text)> Languages { get; } = new Dictionary<string, (FrameworkElement viewer, string text)>();
#pragma warning restore SA1009 // Closing parenthesis must be spaced correctly
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly

            public string CurrentLanguage { get; set; }
        }
    }
}