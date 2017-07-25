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
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Display;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Helpers;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Parse;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// An efficient and extensible control that can parse and render markdown.
    /// </summary>
    public sealed class MarkdownTextBlock : Control, ILinkRegister, IImageResolver
    {
        /// <summary>
        /// Holds a list of hyperlinks we are listening to.
        /// </summary>
        private readonly List<Hyperlink> _listeningHyperlinks = new List<Hyperlink>();

        /// <summary>
        /// The root element for our rendering.
        /// </summary>
        private Border _rootElement;

        /// <summary>
        /// Fired when the text is done parsing and formatting. Fires each time the markdown is rendered.
        /// </summary>
        public event EventHandler<MarkdownRenderedEventArgs> MarkdownRendered;

        /// <summary>
        /// Fired when a link element in the markdown was tapped.
        /// </summary>
        public event EventHandler<LinkClickedEventArgs> LinkClicked;

        /// <summary>
        /// Fired when an image from the markdown document needs to be resolved.
        /// The default implementation is basically <code>new BitmapImage(new Uri(e.Url));</code>.
        /// </summary>
        public event EventHandler<ImageResolvingEventArgs> ImageResolving;

        /// <summary>
        /// Gets the dependency property for <see cref="ImageStretch"/>.
        /// </summary>
        public static readonly DependencyProperty ImageStretchProperty = DependencyProperty.Register(
            nameof(ImageStretch),
            typeof(Stretch),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(Stretch.None, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the stretch used for images.
        /// </summary>
        public Stretch ImageStretch
        {
            get { return (Stretch)GetValue(ImageStretchProperty); }
            set { SetValue(ImageStretchProperty, value); }
        }

        /// <summary>
        /// Gets or sets the markdown text to display.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Text"/>.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(string.Empty, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets a value indicating whether text selection is enabled.
        /// </summary>
        public bool IsTextSelectionEnabled
        {
            get { return (bool)GetValue(IsTextSelectionEnabledProperty); }
            set { SetValue(IsTextSelectionEnabledProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="IsTextSelectionEnabled"/>.
        /// </summary>
        public static readonly DependencyProperty IsTextSelectionEnabledProperty = DependencyProperty.Register(
            nameof(IsTextSelectionEnabled),
            typeof(bool),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(true, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the brush used to render links.  If this is
        /// <c>null</c>, then Foreground is used.
        /// </summary>
        public Brush LinkForeground
        {
            get { return (Brush)GetValue(LinkForegroundProperty); }
            set { SetValue(LinkForegroundProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="LinkForeground"/>.
        /// </summary>
        public static readonly DependencyProperty LinkForegroundProperty = DependencyProperty.Register(
            nameof(LinkForeground),
            typeof(Brush),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the brush used to fill the background of a code block.
        /// </summary>
        public Brush CodeBackground
        {
            get { return (Brush)GetValue(CodeBackgroundProperty); }
            set { SetValue(CodeBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="CodeBackground"/>.
        /// </summary>
        public static readonly DependencyProperty CodeBackgroundProperty = DependencyProperty.Register(
            nameof(CodeBackground),
            typeof(Brush),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the brush used to render the border fill of a code block.
        /// </summary>
        public Brush CodeBorderBrush
        {
            get { return (Brush)GetValue(CodeBorderBrushProperty); }
            set { SetValue(CodeBorderBrushProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="CodeBorderBrush"/>.
        /// </summary>
        public static readonly DependencyProperty CodeBorderBrushProperty = DependencyProperty.Register(
            nameof(CodeBorderBrush),
            typeof(Brush),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the thickness of the border around code blocks.
        /// </summary>
        public Thickness CodeBorderThickness
        {
            get { return (Thickness)GetValue(CodeBorderThicknessProperty); }
            set { SetValue(CodeBorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="CodeBorderThickness"/>.
        /// </summary>
        public static readonly DependencyProperty CodeBorderThicknessProperty = DependencyProperty.Register(
            nameof(CodeBorderThickness),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the brush used to render the text inside a code block.  If this is
        /// <c>null</c>, then Foreground is used.
        /// </summary>
        public Brush CodeForeground
        {
            get { return (Brush)GetValue(CodeForegroundProperty); }
            set { SetValue(CodeForegroundProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="CodeForeground"/>.
        /// </summary>
        public static readonly DependencyProperty CodeForegroundProperty = DependencyProperty.Register(
            nameof(CodeForeground),
            typeof(Brush),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the font used to display code.  If this is <c>null</c>, then
        /// <see cref="FontFamily"/> is used.
        /// </summary>
        public FontFamily CodeFontFamily
        {
            get { return (FontFamily)GetValue(CodeFontFamilyProperty); }
            set { SetValue(CodeFontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="CodeFontFamily"/>.
        /// </summary>
        public static readonly DependencyProperty CodeFontFamilyProperty = DependencyProperty.Register(
            nameof(CodeFontFamily),
            typeof(FontFamily),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the space between the code border and the text.
        /// </summary>
        public Thickness CodeMargin
        {
            get { return (Thickness)GetValue(CodeMarginProperty); }
            set { SetValue(CodeMarginProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="CodeMargin"/>.
        /// </summary>
        public static readonly DependencyProperty CodeMarginProperty = DependencyProperty.Register(
            nameof(CodeMargin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets space between the code border and the text.
        /// </summary>
        public Thickness CodePadding
        {
            get { return (Thickness)GetValue(CodePaddingProperty); }
            set { SetValue(CodePaddingProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="CodePadding"/>.
        /// </summary>
        public static readonly DependencyProperty CodePaddingProperty = DependencyProperty.Register(
            nameof(CodePadding),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the font weight to use for level 1 headers.
        /// </summary>
        public FontWeight Header1FontWeight
        {
            get { return (FontWeight)GetValue(Header1FontWeightProperty); }
            set { SetValue(Header1FontWeightProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header1FontWeight"/>.
        /// </summary>
        public static readonly DependencyProperty Header1FontWeightProperty = DependencyProperty.Register(
            nameof(Header1FontWeight),
            typeof(FontWeight),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the font size for level 1 headers.
        /// </summary>
        public double Header1FontSize
        {
            get { return (double)GetValue(Header1FontSizeProperty); }
            set { SetValue(Header1FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header1FontSize"/>.
        /// </summary>
        public static readonly DependencyProperty Header1FontSizeProperty = DependencyProperty.Register(
            nameof(Header1FontSize),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the margin for level 1 headers.
        /// </summary>
        public Thickness Header1Margin
        {
            get { return (Thickness)GetValue(Header1MarginProperty); }
            set { SetValue(Header1MarginProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header1Margin"/>.
        /// </summary>
        public static readonly DependencyProperty Header1MarginProperty = DependencyProperty.Register(
            nameof(Header1Margin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the foreground brush for level 1 headers.
        /// </summary>
        public Brush Header1Foreground
        {
            get { return (Brush)GetValue(Header1ForegroundProperty); }
            set { SetValue(Header1ForegroundProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header1Foreground"/>.
        /// </summary>
        public static readonly DependencyProperty Header1ForegroundProperty = DependencyProperty.Register(
            nameof(Header1Foreground),
            typeof(Brush),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the font weight to use for level 2 headers.
        /// </summary>
        public FontWeight Header2FontWeight
        {
            get { return (FontWeight)GetValue(Header2FontWeightProperty); }
            set { SetValue(Header2FontWeightProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header2FontWeight"/>.
        /// </summary>
        public static readonly DependencyProperty Header2FontWeightProperty = DependencyProperty.Register(
            nameof(Header2FontWeight),
            typeof(FontWeight),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the font size for level 2 headers.
        /// </summary>
        public double Header2FontSize
        {
            get { return (double)GetValue(Header2FontSizeProperty); }
            set { SetValue(Header2FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header2FontSize"/>.
        /// </summary>
        public static readonly DependencyProperty Header2FontSizeProperty = DependencyProperty.Register(
            nameof(Header2FontSize),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the margin for level 2 headers.
        /// </summary>
        public Thickness Header2Margin
        {
            get { return (Thickness)GetValue(Header2MarginProperty); }
            set { SetValue(Header2MarginProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header2Margin"/>.
        /// </summary>
        public static readonly DependencyProperty Header2MarginProperty = DependencyProperty.Register(
            nameof(Header2Margin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the foreground brush for level 2 headers.
        /// </summary>
        public Brush Header2Foreground
        {
            get { return (Brush)GetValue(Header2ForegroundProperty); }
            set { SetValue(Header2ForegroundProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header2Foreground"/>.
        /// </summary>
        public static readonly DependencyProperty Header2ForegroundProperty = DependencyProperty.Register(
            nameof(Header2Foreground),
            typeof(Brush),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the font weight to use for level 3 headers.
        /// </summary>
        public FontWeight Header3FontWeight
        {
            get { return (FontWeight)GetValue(Header3FontWeightProperty); }
            set { SetValue(Header3FontWeightProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header3FontWeight"/>.
        /// </summary>
        public static readonly DependencyProperty Header3FontWeightProperty = DependencyProperty.Register(
            nameof(Header3FontWeight),
            typeof(FontWeight),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the font size for level 3 headers.
        /// </summary>
        public double Header3FontSize
        {
            get { return (double)GetValue(Header3FontSizeProperty); }
            set { SetValue(Header3FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header3FontSize"/>.
        /// </summary>
        public static readonly DependencyProperty Header3FontSizeProperty = DependencyProperty.Register(
            nameof(Header3FontSize),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the margin for level 3 headers.
        /// </summary>
        public Thickness Header3Margin
        {
            get { return (Thickness)GetValue(Header3MarginProperty); }
            set { SetValue(Header3MarginProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header3Margin"/>.
        /// </summary>
        public static readonly DependencyProperty Header3MarginProperty = DependencyProperty.Register(
            nameof(Header3Margin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the foreground brush for level 3 headers.
        /// </summary>
        public Brush Header3Foreground
        {
            get { return (Brush)GetValue(Header3ForegroundProperty); }
            set { SetValue(Header3ForegroundProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header3Foreground"/>.
        /// </summary>
        public static readonly DependencyProperty Header3ForegroundProperty = DependencyProperty.Register(
            nameof(Header3Foreground),
            typeof(Brush),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the font weight to use for level 4 headers.
        /// </summary>
        public FontWeight Header4FontWeight
        {
            get { return (FontWeight)GetValue(Header4FontWeightProperty); }
            set { SetValue(Header4FontWeightProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header4FontWeight"/>.
        /// </summary>
        public static readonly DependencyProperty Header4FontWeightProperty = DependencyProperty.Register(
            nameof(Header4FontWeight),
            typeof(FontWeight),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the font size for level 4 headers.
        /// </summary>
        public double Header4FontSize
        {
            get { return (double)GetValue(Header4FontSizeProperty); }
            set { SetValue(Header4FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header4FontSize"/>.
        /// </summary>
        public static readonly DependencyProperty Header4FontSizeProperty = DependencyProperty.Register(
            nameof(Header4FontSize),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the margin for level 4 headers.
        /// </summary>
        public Thickness Header4Margin
        {
            get { return (Thickness)GetValue(Header4MarginProperty); }
            set { SetValue(Header4MarginProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header4Margin"/>.
        /// </summary>
        public static readonly DependencyProperty Header4MarginProperty = DependencyProperty.Register(
            nameof(Header4Margin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the foreground brush for level 4 headers.
        /// </summary>
        public Brush Header4Foreground
        {
            get { return (Brush)GetValue(Header4ForegroundProperty); }
            set { SetValue(Header4ForegroundProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header4Foreground"/>.
        /// </summary>
        public static readonly DependencyProperty Header4ForegroundProperty = DependencyProperty.Register(
            nameof(Header4Foreground),
            typeof(Brush),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the font weight to use for level 5 headers.
        /// </summary>
        public FontWeight Header5FontWeight
        {
            get { return (FontWeight)GetValue(Header5FontWeightProperty); }
            set { SetValue(Header5FontWeightProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header5FontWeight"/>.
        /// </summary>
        public static readonly DependencyProperty Header5FontWeightProperty = DependencyProperty.Register(
            nameof(Header5FontWeight),
            typeof(FontWeight),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the font size for level 5 headers.
        /// </summary>
        public double Header5FontSize
        {
            get { return (double)GetValue(Header5FontSizeProperty); }
            set { SetValue(Header5FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header5FontSize"/>.
        /// </summary>
        public static readonly DependencyProperty Header5FontSizeProperty = DependencyProperty.Register(
            nameof(Header5FontSize),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the margin for level 5 headers.
        /// </summary>
        public Thickness Header5Margin
        {
            get { return (Thickness)GetValue(Header5MarginProperty); }
            set { SetValue(Header5MarginProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header5Margin"/>.
        /// </summary>
        public static readonly DependencyProperty Header5MarginProperty = DependencyProperty.Register(
            nameof(Header5Margin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the foreground brush for level 5 headers.
        /// </summary>
        public Brush Header5Foreground
        {
            get { return (Brush)GetValue(Header5ForegroundProperty); }
            set { SetValue(Header5ForegroundProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header5Foreground"/>.
        /// </summary>
        public static readonly DependencyProperty Header5ForegroundProperty = DependencyProperty.Register(
            nameof(Header5Foreground),
            typeof(Brush),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the font weight to use for level 6 headers.
        /// </summary>
        public FontWeight Header6FontWeight
        {
            get { return (FontWeight)GetValue(Header6FontWeightProperty); }
            set { SetValue(Header6FontWeightProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header6FontWeight"/>.
        /// </summary>
        public static readonly DependencyProperty Header6FontWeightProperty = DependencyProperty.Register(
            nameof(Header6FontWeight),
            typeof(FontWeight),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the font size for level 6 headers.
        /// </summary>
        public double Header6FontSize
        {
            get { return (double)GetValue(Header6FontSizeProperty); }
            set { SetValue(Header6FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header6FontSize"/>.
        /// </summary>
        public static readonly DependencyProperty Header6FontSizeProperty = DependencyProperty.Register(
            nameof(Header6FontSize),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the margin for level 6 headers.
        /// </summary>
        public Thickness Header6Margin
        {
            get { return (Thickness)GetValue(Header6MarginProperty); }
            set { SetValue(Header6MarginProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header6Margin"/>.
        /// </summary>
        public static readonly DependencyProperty Header6MarginProperty = DependencyProperty.Register(
            nameof(Header6Margin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the foreground brush for level 6 headers.
        /// </summary>
        public Brush Header6Foreground
        {
            get { return (Brush)GetValue(Header6ForegroundProperty); }
            set { SetValue(Header6ForegroundProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="Header6Foreground"/>.
        /// </summary>
        public static readonly DependencyProperty Header6ForegroundProperty = DependencyProperty.Register(
            nameof(Header6Foreground),
            typeof(Brush),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the brush used to render a horizontal rule.  If this is <c>null</c>, then
        /// <see cref="HorizontalRuleBrush"/> is used.
        /// </summary>
        public Brush HorizontalRuleBrush
        {
            get { return (Brush)GetValue(HorizontalRuleBrushProperty); }
            set { SetValue(HorizontalRuleBrushProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="HorizontalRuleBrush"/>.
        /// </summary>
        public static readonly DependencyProperty HorizontalRuleBrushProperty = DependencyProperty.Register(
            nameof(HorizontalRuleBrush),
            typeof(Brush),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the margin used for horizontal rules.
        /// </summary>
        public Thickness HorizontalRuleMargin
        {
            get { return (Thickness)GetValue(HorizontalRuleMarginProperty); }
            set { SetValue(HorizontalRuleMarginProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="HorizontalRuleMargin"/>.
        /// </summary>
        public static readonly DependencyProperty HorizontalRuleMarginProperty = DependencyProperty.Register(
            nameof(HorizontalRuleMargin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the vertical thickness of the horizontal rule.
        /// </summary>
        public double HorizontalRuleThickness
        {
            get { return (double)GetValue(HorizontalRuleThicknessProperty); }
            set { SetValue(HorizontalRuleThicknessProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="HorizontalRuleThickness"/>.
        /// </summary>
        public static readonly DependencyProperty HorizontalRuleThicknessProperty = DependencyProperty.Register(
            nameof(HorizontalRuleThickness),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the margin used by lists.
        /// </summary>
        public Thickness ListMargin
        {
            get { return (Thickness)GetValue(ListMarginProperty); }
            set { SetValue(ListMarginProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="ListMargin"/>.
        /// </summary>
        public static readonly DependencyProperty ListMarginProperty = DependencyProperty.Register(
            nameof(ListMargin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the width of the space used by list item bullets/numbers.
        /// </summary>
        public double ListGutterWidth
        {
            get { return (double)GetValue(ListGutterWidthProperty); }
            set { SetValue(ListGutterWidthProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="ListGutterWidth"/>.
        /// </summary>
        public static readonly DependencyProperty ListGutterWidthProperty = DependencyProperty.Register(
            nameof(ListGutterWidth),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the space between the list item bullets/numbers and the list item content.
        /// </summary>
        public double ListBulletSpacing
        {
            get { return (double)GetValue(ListBulletSpacingProperty); }
            set { SetValue(ListBulletSpacingProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="ListBulletSpacing"/>.
        /// </summary>
        public static readonly DependencyProperty ListBulletSpacingProperty = DependencyProperty.Register(
            nameof(ListBulletSpacing),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the margin used for paragraphs.
        /// </summary>
        public Thickness ParagraphMargin
        {
            get { return (Thickness)GetValue(ParagraphMarginProperty); }
            set { SetValue(ParagraphMarginProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="ParagraphMargin"/>.
        /// </summary>
        public static readonly DependencyProperty ParagraphMarginProperty = DependencyProperty.Register(
            nameof(ParagraphMargin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the brush used to fill the background of a quote block.
        /// </summary>
        public Brush QuoteBackground
        {
            get { return (Brush)GetValue(QuoteBackgroundProperty); }
            set { SetValue(QuoteBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="QuoteBackground"/>.
        /// </summary>
        public static readonly DependencyProperty QuoteBackgroundProperty = DependencyProperty.Register(
            nameof(QuoteBackground),
            typeof(Brush),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the brush used to render a quote border.  If this is <c>null</c>, then
        /// <see cref="QuoteBorderBrush"/> is used.
        /// </summary>
        public Brush QuoteBorderBrush
        {
            get { return (Brush)GetValue(QuoteBorderBrushProperty); }
            set { SetValue(QuoteBorderBrushProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="QuoteBorderBrush"/>.
        /// </summary>
        public static readonly DependencyProperty QuoteBorderBrushProperty = DependencyProperty.Register(
            nameof(QuoteBorderBrush),
            typeof(Brush),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the thickness of quote borders.
        /// </summary>
        public Thickness QuoteBorderThickness
        {
            get { return (Thickness)GetValue(QuoteBorderThicknessProperty); }
            set { SetValue(QuoteBorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="QuoteBorderThickness"/>.
        /// </summary>
        public static readonly DependencyProperty QuoteBorderThicknessProperty = DependencyProperty.Register(
            nameof(QuoteBorderThickness),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the brush used to render the text inside a quote block.  If this is
        /// <c>null</c>, then Foreground is used.
        /// </summary>
        public Brush QuoteForeground
        {
            get { return (Brush)GetValue(QuoteForegroundProperty); }
            set { SetValue(QuoteForegroundProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="QuoteForeground"/>.
        /// </summary>
        public static readonly DependencyProperty QuoteForegroundProperty = DependencyProperty.Register(
            nameof(QuoteForeground),
            typeof(Brush),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the space outside of quote borders.
        /// </summary>
        public Thickness QuoteMargin
        {
            get { return (Thickness)GetValue(QuoteMarginProperty); }
            set { SetValue(QuoteMarginProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="QuoteMargin"/>.
        /// </summary>
        public static readonly DependencyProperty QuoteMarginProperty = DependencyProperty.Register(
            nameof(QuoteMargin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the space between the quote border and the text.
        /// </summary>
        public Thickness QuotePadding
        {
            get { return (Thickness)GetValue(QuotePaddingProperty); }
            set { SetValue(QuotePaddingProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="QuotePadding"/>.
        /// </summary>
        public static readonly DependencyProperty QuotePaddingProperty = DependencyProperty.Register(
            nameof(QuotePadding),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the brush used to render table borders.  If this is <c>null</c>, then
        /// <see cref="TableBorderBrush"/> is used.
        /// </summary>
        public Brush TableBorderBrush
        {
            get { return (Brush)GetValue(TableBorderBrushProperty); }
            set { SetValue(TableBorderBrushProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="TableBorderBrush"/>.
        /// </summary>
        public static readonly DependencyProperty TableBorderBrushProperty = DependencyProperty.Register(
            nameof(TableBorderBrush),
            typeof(Brush),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the thickness of any table borders.
        /// </summary>
        public double TableBorderThickness
        {
            get { return (double)GetValue(TableBorderThicknessProperty); }
            set { SetValue(TableBorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="TableBorderThickness"/>.
        /// </summary>
        public static readonly DependencyProperty TableBorderThicknessProperty = DependencyProperty.Register(
            nameof(TableBorderThickness),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the padding inside each cell.
        /// </summary>
        public Thickness TableCellPadding
        {
            get { return (Thickness)GetValue(TableCellPaddingProperty); }
            set { SetValue(TableCellPaddingProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="TableCellPadding"/>.
        /// </summary>
        public static readonly DependencyProperty TableCellPaddingProperty = DependencyProperty.Register(
            nameof(TableCellPadding),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the margin used by tables.
        /// </summary>
        public Thickness TableMargin
        {
            get { return (Thickness)GetValue(TableMarginProperty); }
            set { SetValue(TableMarginProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="TableMargin"/>.
        /// </summary>
        public static readonly DependencyProperty TableMarginProperty = DependencyProperty.Register(
            nameof(TableMargin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the word wrapping behavior.
        /// </summary>
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="TextWrapping"/>.
        /// </summary>
        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register(
            nameof(TextWrapping),
            typeof(TextWrapping),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Calls OnPropertyChanged.
        /// </summary>
        private static void OnPropertyChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as MarkdownTextBlock;

            // Defer to the instance method.
            instance?.OnPropertyChanged(d, e.Property);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownTextBlock"/> class.
        /// </summary>
        public MarkdownTextBlock()
        {
            // Set our style.
            DefaultStyleKey = typeof(MarkdownTextBlock);

            // Register for property callbacks that are owned by our parent class.
            RegisterPropertyChangedCallback(FontSizeProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(BackgroundProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(BorderBrushProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(BorderThicknessProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(CharacterSpacingProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(FontFamilyProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(FontSizeProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(FontStretchProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(FontStyleProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(FontWeightProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(ForegroundProperty, OnPropertyChanged);
            RegisterPropertyChangedCallback(PaddingProperty, OnPropertyChanged);
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            // Grab our root
            _rootElement = GetTemplateChild("RootElement") as Border;

            // And make sure to render any markdown we have.
            RenderMarkdown();
        }

        /// <summary>
        /// Fired when the value of a DependencyProperty is changed.
        /// </summary>
        private void OnPropertyChanged(DependencyObject d, DependencyProperty prop)
        {
            RenderMarkdown();
        }

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

            MarkdownRenderedEventArgs markdownRenderedArgs = new MarkdownRenderedEventArgs(null);
            try
            {
                // Try to parse the markdown.
                MarkdownDocument markdown = new MarkdownDocument();
                markdown.Parse(Text);

                // Now try to display it
                var renderer = new XamlRenderer(markdown, this, this)
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
                    CodeForeground = CodeForeground,
                    CodeFontFamily = CodeFontFamily,
                    CodePadding = CodePadding,
                    CodeMargin = CodeMargin,
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

        // Used to attach the URL to hyperlinks.
        private static readonly DependencyProperty HyperlinkUrlProperty =
            DependencyProperty.RegisterAttached("HyperlinkUrl", typeof(string), typeof(MarkdownTextBlock), new PropertyMetadata(null));

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

        private bool multiClickDetectionTriggered;

        /// <summary>
        /// Fired when a user taps one of the link elements
        /// </summary>
        private async void Hyperlink_Click(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            // Links that are nested within superscript elements cause the Click event to fire multiple times.
            // e.g. this markdown "[^bot](http://www.reddit.com/r/youtubefactsbot/wiki/index)"
            // Therefore we detect and ignore multiple clicks.
            if (multiClickDetectionTriggered)
            {
                return;
            }

            multiClickDetectionTriggered = true;
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () => multiClickDetectionTriggered = false);

            // Get the hyperlink URL.
            var url = (string)sender.GetValue(HyperlinkUrlProperty);
            if (url == null)
            {
                return;
            }

            // Fire off the event.
            var eventArgs = new LinkClickedEventArgs(url);
            LinkClicked?.Invoke(this, eventArgs);
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
    }
}
