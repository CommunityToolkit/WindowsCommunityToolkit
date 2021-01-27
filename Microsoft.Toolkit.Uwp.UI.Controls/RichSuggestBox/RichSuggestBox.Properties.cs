// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
                new PropertyMetadata(null, OnHeaderChanged));

        /// <summary>
        /// Identifies the <see cref="HeaderTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(
                nameof(HeaderTemplate),
                typeof(DataTemplate),
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
                new PropertyMetadata(null, OnDescriptionChanged));

        /// <summary>
        /// Identifies the <see cref="SuggestionPopupPlacement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SuggestionPopupPlacementProperty =
            DependencyProperty.Register(
                nameof(SuggestionPopupPlacement),
                typeof(SuggestionPopupPlacementMode),
                typeof(RichSuggestBox),
                new PropertyMetadata(SuggestionPopupPlacementMode.Floating, OnSuggestionPopupPlacementChanged));

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
        /// <remarks>
        /// Suggestion popup relies on the actual size of the text control to calculate its placement on the screen.
        /// It is recommended to set the header using this property instead of using <see cref="RichEditBox.Header"/>.
        /// </remarks>
        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/> used to display the content of the control's header.
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get => (DataTemplate)GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets content that is shown below the control. The content should provide guidance about the input expected by the control.
        /// </summary>
        /// <remarks>
        /// Suggestion popup relies on the actual size of the text control to calculate its placement on the screen.
        /// It is recommended to set the description using this property instead of using <see cref="RichEditBox.Description"/>.
        /// </remarks>
        public object Description
        {
            get => GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        /// <summary>
        /// Gets or sets suggestion popup placement to either Floating or Attached to the text box.
        /// </summary>
        public SuggestionPopupPlacementMode SuggestionPopupPlacement
        {
            get => (SuggestionPopupPlacementMode)this.GetValue(SuggestionPopupPlacementProperty);
            set => SetValue(SuggestionPopupPlacementProperty, value);
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
