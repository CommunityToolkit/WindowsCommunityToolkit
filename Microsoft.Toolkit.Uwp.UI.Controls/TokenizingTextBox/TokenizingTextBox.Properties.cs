using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A text input control that auto-suggests and displays token items.
    /// </summary>
    public partial class TokenizingTextBox : Control
    {
        /// <summary>
        /// Identifies the <see cref="AutoSuggestBoxStyle"/> property.
        /// </summary>
        public static readonly DependencyProperty AutoSuggestBoxStyleProperty = DependencyProperty.Register(
            nameof(AutoSuggestBoxStyle),
            typeof(Style),
            typeof(TokenizingTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="AutoSuggestBoxTextBoxStyle"/> property.
        /// </summary>
        public static readonly DependencyProperty AutoSuggestBoxTextBoxStyleProperty = DependencyProperty.Register(
            nameof(AutoSuggestBoxTextBoxStyle),
            typeof(Style),
            typeof(TokenizingTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="DisplayMemberPath"/> property.
        /// </summary>
        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register(
            nameof(DisplayMemberPath),
            typeof(string),
            typeof(TokenizingTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="TextMemberPath"/> property.
        /// </summary>
        public static readonly DependencyProperty TextMemberPathProperty = DependencyProperty.Register(
            nameof(TextMemberPath),
            typeof(string),
            typeof(TokenizingTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="TokenItemTemplate"/> property.
        /// </summary>
        public static readonly DependencyProperty TokenItemTemplateProperty = DependencyProperty.Register(
            nameof(TokenItemTemplate),
            typeof(DataTemplate),
            typeof(TokenizingTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="TokenItemTemplateSelector"/> property.
        /// </summary>
        public static readonly DependencyProperty TokenItemTemplateSelectorProperty = DependencyProperty.Register(
            nameof(TokenItemTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(TokenizingTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="TokenItemStyle"/> property.
        /// </summary>
        public static readonly DependencyProperty TokenItemStyleProperty = DependencyProperty.Register(
            nameof(TokenItemStyle),
            typeof(Style),
            typeof(TokenizingTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="TokenDelimiter"/> property.
        /// </summary>
        public static readonly DependencyProperty TokenDelimiterProperty = DependencyProperty.Register(
            nameof(TokenDelimiter),
            typeof(string),
            typeof(TokenizingTextBox),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the <see cref="TokenSpacing"/> property.
        /// </summary>
        public static readonly DependencyProperty TokenSpacingProperty = DependencyProperty.Register(
            nameof(TokenSpacing),
            typeof(double),
            typeof(TokenizingTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="PlaceholderText"/> property.
        /// </summary>
        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
            nameof(PlaceholderText),
            typeof(string),
            typeof(TokenizingTextBox),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the <see cref="QueryIcon"/> property.
        /// </summary>
        public static readonly DependencyProperty QueryIconProperty = DependencyProperty.Register(
            nameof(QueryIcon),
            typeof(object),
            typeof(TokenizingTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="QueryText"/> property.
        /// </summary>
        public static readonly DependencyProperty QueryTextProperty = DependencyProperty.Register(
            nameof(QueryText),
            typeof(string),
            typeof(TokenizingTextBox),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the <see cref="SuggestedItemsSource"/> property.
        /// </summary>
        public static readonly DependencyProperty SuggestedItemsSourceProperty = DependencyProperty.Register(
            nameof(SuggestedItemsSource),
            typeof(object),
            typeof(TokenizingTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SuggestedItemTemplate"/> property.
        /// </summary>
        public static readonly DependencyProperty SuggestedItemTemplateProperty = DependencyProperty.Register(
            nameof(SuggestedItemTemplate),
            typeof(DataTemplate),
            typeof(TokenizingTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SuggestedItemTemplateSelector"/> property.
        /// </summary>
        public static readonly DependencyProperty SuggestedItemTemplateSelectorProperty = DependencyProperty.Register(
            nameof(SuggestedItemTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(TokenizingTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SuggestedItemContainerStyle"/> property.
        /// </summary>
        public static readonly DependencyProperty SuggestedItemContainerStyleProperty = DependencyProperty.Register(
            nameof(SuggestedItemContainerStyle),
            typeof(Style),
            typeof(TokenizingTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the Style for the contained AutoSuggestBox template part.
        /// </summary>
        public Style AutoSuggestBoxStyle
        {
            get => (Style)GetValue(AutoSuggestBoxStyleProperty);
            set => SetValue(AutoSuggestBoxStyleProperty, value);
        }

        /// <summary>
        /// Gets or sets the Style for the TextBox part of the AutoSuggestBox template part.
        /// </summary>
        public Style AutoSuggestBoxTextBoxStyle
        {
            get => (Style)GetValue(AutoSuggestBoxStyleProperty);
            set => SetValue(AutoSuggestBoxStyleProperty, value);
        }

        /// <summary>
        /// Gets or sets the DisplayMemberPath of the AutoSuggestBox template part.
        /// </summary>
        public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }

        /// <summary>
        /// Gets or sets the TextMemberPath of the AutoSuggestBox template part.
        /// </summary>
        public string TextMemberPath
        {
            get => (string)GetValue(TextMemberPathProperty);
            set => SetValue(TextMemberPathProperty, value);
        }

        /// <summary>
        /// Gets or sets the template for token items.
        /// </summary>
        public DataTemplate TokenItemTemplate
        {
            get => (DataTemplate)GetValue(TokenItemTemplateProperty);
            set => SetValue(TokenItemTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets the template selector for token items.
        /// </summary>
        public DataTemplateSelector TokenItemTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(TokenItemTemplateSelectorProperty);
            set => SetValue(TokenItemTemplateSelectorProperty, value);
        }

        /// <summary>
        /// Gets or sets the style for token items.
        /// </summary>
        public Style TokenItemStyle
        {
            get => (Style)GetValue(TokenItemStyleProperty);
            set => SetValue(TokenItemStyleProperty, value);
        }

        /// <summary>
        /// Gets or sets delimiter used to determine when to process text input as a new token item.
        /// </summary>
        public string TokenDelimiter
        {
            get => (string)GetValue(TokenDelimiterProperty);
            set => SetValue(TokenDelimiterProperty, value);
        }

        /// <summary>
        /// Gets or sets the spacing value used to separate token items.
        /// </summary>
        public double TokenSpacing
        {
            get => (double)GetValue(TokenSpacingProperty);
            set => SetValue(TokenSpacingProperty, value);
        }

        /// <summary>
        /// Gets or sets the PlaceholderText for the AutoSuggestBox template part.
        /// </summary>
        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        /// <summary>
        /// Gets or sets the icon to display in the AutoSuggestBox template part.
        /// </summary>
        public object QueryIcon
        {
            get => GetValue(QueryIconProperty);
            set
            {
                // Special case for parsing Symbol enum strings
                if (value is string valueString && Enum.TryParse(valueString, out Symbol symbol))
                {
                    SetValue(QueryIconProperty, new SymbolIcon(symbol));
                }
                else
                {
                    SetValue(QueryIconProperty, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the input text of the AutoSuggestBox template part.
        /// </summary>
        public string QueryText
        {
            get => (string)GetValue(QueryTextProperty);
            set => SetValue(QueryTextProperty, value);
        }

        /// <summary>
        /// Gets or sets the items source for token suggestions.
        /// </summary>
        public object SuggestedItemsSource
        {
            get => GetValue(SuggestedItemsSourceProperty);
            set => SetValue(SuggestedItemsSourceProperty, value);
        }

        /// <summary>
        /// Gets or sets the template for displaying suggested tokens.
        /// </summary>
        public object SuggestedItemTemplate
        {
            get => (DataTemplate)GetValue(SuggestedItemTemplateProperty);
            set => SetValue(SuggestedItemTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets the template selector for displaying suggested tokens.
        /// </summary>
        public DataTemplateSelector SuggestedItemTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(SuggestedItemTemplateSelectorProperty);
            set => SetValue(SuggestedItemTemplateSelectorProperty, value);
        }

        /// <summary>
        /// Gets or sets the item container style for displaying suggested tokens.
        /// </summary>
        public Style SuggestedItemContainerStyle
        {
            get => (Style)GetValue(SuggestedItemContainerStyleProperty);
            set => SetValue(SuggestedItemContainerStyleProperty, value);
        }
    }
}
