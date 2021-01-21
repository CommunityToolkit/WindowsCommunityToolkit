// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The RichSuggestBox control extends <see cref="RichEditBox"/> control that suggests and embeds custom data in a rich document.
    /// </summary>
    public partial class RichSuggestBox
    {
        /// <summary>
        /// Identifies the <see cref="PlaceholderText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(
                nameof(PlaceholderText),
                typeof(string),
                typeof(RichSuggestBox),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the <see cref="RichEditBoxStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RichEditBoxStyleProperty =
            DependencyProperty.Register(
                nameof(RichEditBoxStyle),
                typeof(Style),
                typeof(RichSuggestBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Header"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                nameof(Header),
                typeof(object),
                typeof(RichSuggestBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Description"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(
                nameof(Description),
                typeof(object),
                typeof(RichSuggestBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="TextWrapping"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register(
                nameof(TextWrapping),
                typeof(TextWrapping),
                typeof(RichSuggestBox),
                new PropertyMetadata(TextWrapping.NoWrap));

        /// <summary>
        /// Identifies the <see cref="ClipboardCopyFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ClipboardCopyFormatProperty =
            DependencyProperty.Register(
                nameof(ClipboardCopyFormat),
                typeof(RichEditClipboardFormat),
                typeof(RichSuggestBox),
                new PropertyMetadata(RichEditClipboardFormat.AllFormats));

        /// <summary>
        /// Identifies the <see cref="SelectionFlyout"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectionFlyoutProperty =
            DependencyProperty.Register(
                nameof(SelectionFlyout),
                typeof(FlyoutBase),
                typeof(RichSuggestBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="DisabledFormattingAccelerators"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisabledFormattingAcceleratorsProperty =
            DependencyProperty.Register(
                nameof(DisabledFormattingAccelerators),
                typeof(DisabledFormattingAccelerators),
                typeof(RichSuggestBox),
                new PropertyMetadata(DisabledFormattingAccelerators.None));

        /// <summary>
        /// Identifies the <see cref="SuggestionBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SuggestionBackgroundProperty =
            DependencyProperty.Register(
                nameof(SuggestionBackground),
                typeof(SolidColorBrush),
                typeof(RichSuggestBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SuggestionForeground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SuggestionForegroundProperty =
            DependencyProperty.Register(
                nameof(SuggestionForeground),
                typeof(SolidColorBrush),
                typeof(RichSuggestBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Prefixes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PrefixesProperty =
            DependencyProperty.Register(
                nameof(Prefixes),
                typeof(string),
                typeof(RichSuggestBox),
                new PropertyMetadata(null, OnPrefixesChanged));

        /// <summary>
        /// Gets or sets the text that is displayed in the control until the value is changed by a user action or some other operation.
        /// </summary>
        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        /// <summary>
        /// Gets or sets the style of the underlying <see cref="RichEditBox"/>.
        /// </summary>
        public Style RichEditBoxStyle
        {
            get => (Style)GetValue(RichEditBoxStyleProperty);
            set => SetValue(RichEditBoxStyleProperty, value);
        }

        /// <summary>
        /// Gets or sets the content for the control's header.
        /// </summary>
        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// Gets or sets content that is shown below the control. The content should provide guidance about the input expected by the control.
        /// </summary>
        public object Description
        {
            get => GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        /// <summary>
        /// Gets or sets a value that indicates how text wrapping occurs if a line of text extends beyond the available width of the control.
        /// </summary>
        public TextWrapping TextWrapping
        {
            get => (TextWrapping)GetValue(TextWrappingProperty);
            set => SetValue(TextWrappingProperty, value);
        }

        /// <summary>
        /// Gets or sets a value that specifies whether text is copied with all formats, or as plain text only.
        /// </summary>
        public RichEditClipboardFormat ClipboardCopyFormat
        {
            get => (RichEditClipboardFormat)GetValue(ClipboardCopyFormatProperty);
            set => SetValue(ClipboardCopyFormatProperty, value);
        }

        /// <summary>
        /// Gets or sets the flyout that is shown when text is selected using mouse, touch, or pen; or null if no flyout is shown.
        /// </summary>
        public FlyoutBase SelectionFlyout
        {
            get => (FlyoutBase)GetValue(SelectionFlyoutProperty);
            set => SetValue(SelectionFlyoutProperty, value);
        }

        /// <summary>
        /// Gets or sets a value that indicates which keyboard shortcuts for formatting are disabled.
        /// </summary>
        public DisabledFormattingAccelerators DisabledFormattingAccelerators
        {
            get => (DisabledFormattingAccelerators)GetValue(DisabledFormattingAcceleratorsProperty);
            set => SetValue(DisabledFormattingAcceleratorsProperty, value);
        }

        /// <summary>
        /// Gets or sets the default brush used to color the suggestion token background.
        /// </summary>
        public SolidColorBrush SuggestionBackground
        {
            get => (SolidColorBrush)GetValue(SuggestionBackgroundProperty);
            set => SetValue(SuggestionBackgroundProperty, value);
        }

        /// <summary>
        /// Gets or sets the default brush used to color the suggestion token foreground.
        /// </summary>
        public SolidColorBrush SuggestionForeground
        {
            get => (SolidColorBrush)GetValue(SuggestionForegroundProperty);
            set => SetValue(SuggestionForegroundProperty, value);
        }

        /// <summary>
        /// Gets or sets prefix characters to start a query.
        /// </summary>
        public string Prefixes
        {
            get => (string)GetValue(PrefixesProperty);
            set => SetValue(PrefixesProperty, value);
        }

        /// <summary>
        /// Gets object used for lock
        /// </summary>
        protected object LockObj { get; }

        /// <summary>
        /// Gets an object that enables access to the text object model for the text contained in a <see cref="RichEditBox"/>.
        /// </summary>
        public RichEditTextDocument TextDocument => _richEditBox?.TextDocument;

        /// <summary>
        /// Gets a collection of suggestion tokens that are present in the document.
        /// </summary>
        public ReadOnlyObservableCollection<SuggestionInfo> Tokens { get; }
    }
}
