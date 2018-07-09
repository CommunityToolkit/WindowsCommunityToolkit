// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Measurement Properties for elements in the Markdown.
    /// </summary>
    public partial class MarkdownTextBlock
    {
        /// <summary>
        /// Gets the dependency property for <see cref="InlineCodePadding"/>.
        /// </summary>
        public static readonly DependencyProperty InlineCodePaddingProperty =
            DependencyProperty.Register(
                nameof(InlineCodePadding),
                typeof(Thickness),
                typeof(MarkdownTextBlock),
                new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="InlineCodeBorderThickness"/>.
        /// </summary>
        public static readonly DependencyProperty InlineCodeBorderThicknessProperty =
            DependencyProperty.Register(
                nameof(InlineCodeBorderThickness),
                typeof(Thickness),
                typeof(MarkdownTextBlock),
                new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="ImageStretch"/>.
        /// </summary>
        public static readonly DependencyProperty ImageStretchProperty = DependencyProperty.Register(
            nameof(ImageStretch),
            typeof(Stretch),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(Stretch.None, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="CodeBorderThickness"/>.
        /// </summary>
        public static readonly DependencyProperty CodeBorderThicknessProperty = DependencyProperty.Register(
            nameof(CodeBorderThickness),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="CodeMargin"/>.
        /// </summary>
        public static readonly DependencyProperty CodeMarginProperty = DependencyProperty.Register(
            nameof(CodeMargin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="CodePadding"/>.
        /// </summary>
        public static readonly DependencyProperty CodePaddingProperty = DependencyProperty.Register(
            nameof(CodePadding),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="Header1FontSize"/>.
        /// </summary>
        public static readonly DependencyProperty Header1FontSizeProperty = DependencyProperty.Register(
            nameof(Header1FontSize),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="Header1Margin"/>.
        /// </summary>
        public static readonly DependencyProperty Header1MarginProperty = DependencyProperty.Register(
            nameof(Header1Margin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="Header2FontSize"/>.
        /// </summary>
        public static readonly DependencyProperty Header2FontSizeProperty = DependencyProperty.Register(
            nameof(Header2FontSize),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="Header2Margin"/>.
        /// </summary>
        public static readonly DependencyProperty Header2MarginProperty = DependencyProperty.Register(
            nameof(Header2Margin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="Header3FontSize"/>.
        /// </summary>
        public static readonly DependencyProperty Header3FontSizeProperty = DependencyProperty.Register(
            nameof(Header3FontSize),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="Header3Margin"/>.
        /// </summary>
        public static readonly DependencyProperty Header3MarginProperty = DependencyProperty.Register(
            nameof(Header3Margin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="Header4FontSize"/>.
        /// </summary>
        public static readonly DependencyProperty Header4FontSizeProperty = DependencyProperty.Register(
            nameof(Header4FontSize),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="Header4Margin"/>.
        /// </summary>
        public static readonly DependencyProperty Header4MarginProperty = DependencyProperty.Register(
            nameof(Header4Margin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="Header5FontSize"/>.
        /// </summary>
        public static readonly DependencyProperty Header5FontSizeProperty = DependencyProperty.Register(
            nameof(Header5FontSize),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="Header5Margin"/>.
        /// </summary>
        public static readonly DependencyProperty Header5MarginProperty = DependencyProperty.Register(
            nameof(Header5Margin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="Header6Margin"/>.
        /// </summary>
        public static readonly DependencyProperty Header6MarginProperty = DependencyProperty.Register(
            nameof(Header6Margin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="Header6FontSize"/>.
        /// </summary>
        public static readonly DependencyProperty Header6FontSizeProperty = DependencyProperty.Register(
            nameof(Header6FontSize),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="HorizontalRuleMargin"/>.
        /// </summary>
        public static readonly DependencyProperty HorizontalRuleMarginProperty = DependencyProperty.Register(
            nameof(HorizontalRuleMargin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="HorizontalRuleThickness"/>.
        /// </summary>
        public static readonly DependencyProperty HorizontalRuleThicknessProperty = DependencyProperty.Register(
            nameof(HorizontalRuleThickness),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="ListMargin"/>.
        /// </summary>
        public static readonly DependencyProperty ListMarginProperty = DependencyProperty.Register(
            nameof(ListMargin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="ListGutterWidth"/>.
        /// </summary>
        public static readonly DependencyProperty ListGutterWidthProperty = DependencyProperty.Register(
            nameof(ListGutterWidth),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="ListBulletSpacing"/>.
        /// </summary>
        public static readonly DependencyProperty ListBulletSpacingProperty = DependencyProperty.Register(
            nameof(ListBulletSpacing),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="ParagraphMargin"/>.
        /// </summary>
        public static readonly DependencyProperty ParagraphMarginProperty = DependencyProperty.Register(
            nameof(ParagraphMargin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="QuoteBorderThickness"/>.
        /// </summary>
        public static readonly DependencyProperty QuoteBorderThicknessProperty = DependencyProperty.Register(
            nameof(QuoteBorderThickness),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="QuoteMargin"/>.
        /// </summary>
        public static readonly DependencyProperty QuoteMarginProperty = DependencyProperty.Register(
            nameof(QuoteMargin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="QuotePadding"/>.
        /// </summary>
        public static readonly DependencyProperty QuotePaddingProperty = DependencyProperty.Register(
            nameof(QuotePadding),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="TableBorderThickness"/>.
        /// </summary>
        public static readonly DependencyProperty TableBorderThicknessProperty = DependencyProperty.Register(
            nameof(TableBorderThickness),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="TableCellPadding"/>.
        /// </summary>
        public static readonly DependencyProperty TableCellPaddingProperty = DependencyProperty.Register(
            nameof(TableCellPadding),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="TableMargin"/>.
        /// </summary>
        public static readonly DependencyProperty TableMarginProperty = DependencyProperty.Register(
            nameof(TableMargin),
            typeof(Thickness),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="TextWrapping"/>.
        /// </summary>
        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register(
            nameof(TextWrapping),
            typeof(TextWrapping),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="ImageMaxHeight"/>
        /// </summary>
        public static readonly DependencyProperty ImageMaxHeightProperty = DependencyProperty.Register(
            nameof(ImageMaxHeight),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(0.0, OnPropertyChangedStatic));

        /// <summary>
        /// Gets the dependency property for <see cref="ImageMaxWidth"/>
        /// </summary>
        public static readonly DependencyProperty ImageMaxWidthProperty = DependencyProperty.Register(
            nameof(ImageMaxWidth),
            typeof(double),
            typeof(MarkdownTextBlock),
            new PropertyMetadata(0.0, OnPropertyChangedStatic));

        /// <summary>
        /// Gets or sets the MaxWidth for images.
        /// </summary>
        public double ImageMaxWidth
        {
            get { return (double)GetValue(ImageMaxWidthProperty); }
            set { SetValue(ImageMaxWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the MaxHeight for images.
        /// </summary>
        public double ImageMaxHeight
        {
            get { return (double)GetValue(ImageMaxHeightProperty); }
            set { SetValue(ImageMaxHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stretch used for images.
        /// </summary>
        public Stretch ImageStretch
        {
            get { return (Stretch)GetValue(ImageStretchProperty); }
            set { SetValue(ImageStretchProperty, value); }
        }

        /// <summary>
        /// Gets or sets the thickness of the border around code blocks.
        /// </summary>
        public Thickness CodeBorderThickness
        {
            get { return (Thickness)GetValue(CodeBorderThicknessProperty); }
            set { SetValue(CodeBorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the thickness of the border for inline code.
        /// </summary>
        public Thickness InlineCodeBorderThickness
        {
            get { return (Thickness)GetValue(InlineCodeBorderThicknessProperty); }
            set { SetValue(InlineCodeBorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the foreground brush for inline code.
        /// </summary>
        public Thickness InlineCodePadding
        {
            get { return (Thickness)GetValue(InlineCodePaddingProperty); }
            set { SetValue(InlineCodePaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the space between the code border and the text.
        /// </summary>
        public Thickness CodeMargin
        {
            get { return (Thickness)GetValue(CodeMarginProperty); }
            set { SetValue(CodeMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets space between the code border and the text.
        /// </summary>
        public Thickness CodePadding
        {
            get { return (Thickness)GetValue(CodePaddingProperty); }
            set { SetValue(CodePaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font size for level 1 headers.
        /// </summary>
        public double Header1FontSize
        {
            get { return (double)GetValue(Header1FontSizeProperty); }
            set { SetValue(Header1FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the margin for level 1 headers.
        /// </summary>
        public Thickness Header1Margin
        {
            get { return (Thickness)GetValue(Header1MarginProperty); }
            set { SetValue(Header1MarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font size for level 2 headers.
        /// </summary>
        public double Header2FontSize
        {
            get { return (double)GetValue(Header2FontSizeProperty); }
            set { SetValue(Header2FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the margin for level 2 headers.
        /// </summary>
        public Thickness Header2Margin
        {
            get { return (Thickness)GetValue(Header2MarginProperty); }
            set { SetValue(Header2MarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font size for level 3 headers.
        /// </summary>
        public double Header3FontSize
        {
            get { return (double)GetValue(Header3FontSizeProperty); }
            set { SetValue(Header3FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the margin for level 3 headers.
        /// </summary>
        public Thickness Header3Margin
        {
            get { return (Thickness)GetValue(Header3MarginProperty); }
            set { SetValue(Header3MarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font size for level 4 headers.
        /// </summary>
        public double Header4FontSize
        {
            get { return (double)GetValue(Header4FontSizeProperty); }
            set { SetValue(Header4FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the margin for level 4 headers.
        /// </summary>
        public Thickness Header4Margin
        {
            get { return (Thickness)GetValue(Header4MarginProperty); }
            set { SetValue(Header4MarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font size for level 5 headers.
        /// </summary>
        public double Header5FontSize
        {
            get { return (double)GetValue(Header5FontSizeProperty); }
            set { SetValue(Header5FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the margin for level 5 headers.
        /// </summary>
        public Thickness Header5Margin
        {
            get { return (Thickness)GetValue(Header5MarginProperty); }
            set { SetValue(Header5MarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font size for level 6 headers.
        /// </summary>
        public double Header6FontSize
        {
            get { return (double)GetValue(Header6FontSizeProperty); }
            set { SetValue(Header6FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the margin for level 6 headers.
        /// </summary>
        public Thickness Header6Margin
        {
            get { return (Thickness)GetValue(Header6MarginProperty); }
            set { SetValue(Header6MarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the margin used for horizontal rules.
        /// </summary>
        public Thickness HorizontalRuleMargin
        {
            get { return (Thickness)GetValue(HorizontalRuleMarginProperty); }
            set { SetValue(HorizontalRuleMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical thickness of the horizontal rule.
        /// </summary>
        public double HorizontalRuleThickness
        {
            get { return (double)GetValue(HorizontalRuleThicknessProperty); }
            set { SetValue(HorizontalRuleThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the margin used by lists.
        /// </summary>
        public Thickness ListMargin
        {
            get { return (Thickness)GetValue(ListMarginProperty); }
            set { SetValue(ListMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the space used by list item bullets/numbers.
        /// </summary>
        public double ListGutterWidth
        {
            get { return (double)GetValue(ListGutterWidthProperty); }
            set { SetValue(ListGutterWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the space between the list item bullets/numbers and the list item content.
        /// </summary>
        public double ListBulletSpacing
        {
            get { return (double)GetValue(ListBulletSpacingProperty); }
            set { SetValue(ListBulletSpacingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the margin used for paragraphs.
        /// </summary>
        public Thickness ParagraphMargin
        {
            get { return (Thickness)GetValue(ParagraphMarginProperty); }
            set { SetValue(ParagraphMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the thickness of quote borders.
        /// </summary>
        public Thickness QuoteBorderThickness
        {
            get { return (Thickness)GetValue(QuoteBorderThicknessProperty); }
            set { SetValue(QuoteBorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the space outside of quote borders.
        /// </summary>
        public Thickness QuoteMargin
        {
            get { return (Thickness)GetValue(QuoteMarginProperty); }
            set { SetValue(QuoteMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the space between the quote border and the text.
        /// </summary>
        public Thickness QuotePadding
        {
            get { return (Thickness)GetValue(QuotePaddingProperty); }
            set { SetValue(QuotePaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the thickness of any table borders.
        /// </summary>
        public double TableBorderThickness
        {
            get { return (double)GetValue(TableBorderThicknessProperty); }
            set { SetValue(TableBorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the padding inside each cell.
        /// </summary>
        public Thickness TableCellPadding
        {
            get { return (Thickness)GetValue(TableCellPaddingProperty); }
            set { SetValue(TableCellPaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the margin used by tables.
        /// </summary>
        public Thickness TableMargin
        {
            get { return (Thickness)GetValue(TableMarginProperty); }
            set { SetValue(TableMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the word wrapping behavior.
        /// </summary>
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }
    }
}