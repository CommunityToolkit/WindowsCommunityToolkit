using System.Collections.ObjectModel;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public partial class RichSuggestBox
    {
        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(
                nameof(PlaceholderText),
                typeof(string),
                typeof(RichSuggestBox),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty RichEditBoxStyleProperty =
            DependencyProperty.Register(
                nameof(RichEditBoxStyle),
                typeof(Style),
                typeof(RichSuggestBox),
                new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                nameof(Header),
                typeof(object),
                typeof(RichSuggestBox),
                new PropertyMetadata(null));

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(
                nameof(Description),
                typeof(object),
                typeof(RichSuggestBox),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register(
                nameof(TextWrapping),
                typeof(TextWrapping),
                typeof(RichSuggestBox),
                new PropertyMetadata(TextWrapping.NoWrap));

        public static readonly DependencyProperty ClipboardCopyFormatProperty =
            DependencyProperty.Register(
                nameof(ClipboardCopyFormat),
                typeof(RichEditClipboardFormat),
                typeof(RichSuggestBox),
                new PropertyMetadata(RichEditClipboardFormat.PlainText));

        public static readonly DependencyProperty SuggestionBackgroundProperty =
            DependencyProperty.Register(
                nameof(SuggestionBackground),
                typeof(SolidColorBrush),
                typeof(RichSuggestBox),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SuggestionForegroundProperty =
            DependencyProperty.Register(
                nameof(SuggestionForeground),
                typeof(SolidColorBrush),
                typeof(RichSuggestBox),
                new PropertyMetadata(null));

        public static readonly DependencyProperty PrefixesProperty =
            DependencyProperty.Register(
                nameof(Prefixes),
                typeof(string),
                typeof(RichSuggestBox),
                new PropertyMetadata("@", OnPrefixesChanged));

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        public Style RichEditBoxStyle
        {
            get => (Style)GetValue(RichEditBoxStyleProperty);
            set => SetValue(RichEditBoxStyleProperty, value);
        }

        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public object Description
        {
            get => GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public TextWrapping TextWrapping
        {
            get => (TextWrapping)GetValue(TextWrappingProperty);
            set => SetValue(TextWrappingProperty, value);
        }

        public RichEditClipboardFormat ClipboardCopyFormat
        {
            get => (RichEditClipboardFormat)GetValue(ClipboardCopyFormatProperty);
            set => SetValue(ClipboardCopyFormatProperty, value);
        }

        public SolidColorBrush SuggestionBackground
        {
            get => (SolidColorBrush)GetValue(SuggestionBackgroundProperty);
            set => SetValue(SuggestionBackgroundProperty, value);
        }

        public SolidColorBrush SuggestionForeground
        {
            get => (SolidColorBrush)GetValue(SuggestionForegroundProperty);
            set => SetValue(SuggestionForegroundProperty, value);
        }

        public string Prefixes
        {
            get => (string)GetValue(PrefixesProperty);
            set => SetValue(PrefixesProperty, value);
        }

        /// <summary>
        /// Gets object used for lock
        /// </summary>
        protected object LockObj { get; }

        public RichEditTextDocument TextDocument => _richEditBox?.TextDocument;

        public ReadOnlyObservableCollection<SuggestionInfo> Tokens { get; }
    }
}
