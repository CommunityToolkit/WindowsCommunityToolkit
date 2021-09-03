// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace CommunityToolkit.WinUI.UI.Controls
{
    /// <summary>
    /// The RichSuggestBox control extends <see cref="RichEditBox"/> control that suggests and embeds custom data in a rich document.
    /// </summary>
    public partial class RichSuggestBox
    {
        /// <summary>
        /// Event raised when the control needs to show suggestions.
        /// </summary>
        public event TypedEventHandler<RichSuggestBox, SuggestionRequestedEventArgs> SuggestionRequested;

        /// <summary>
        /// Event raised when user click on a suggestion.
        /// This event lets you customize the token appearance in the document.
        /// </summary>
        public event TypedEventHandler<RichSuggestBox, SuggestionChosenEventArgs> SuggestionChosen;

        /// <summary>
        /// Event raised when a token is fully highlighted.
        /// </summary>
        public event TypedEventHandler<RichSuggestBox, RichSuggestTokenSelectedEventArgs> TokenSelected;

        /// <summary>
        /// Event raised when a pointer is hovering over a token.
        /// </summary>
        public event TypedEventHandler<RichSuggestBox, RichSuggestTokenPointerOverEventArgs> TokenPointerOver;

        /// <summary>
        /// Event raised when text is changed, either by user or by internal formatting.
        /// </summary>
        public event TypedEventHandler<RichSuggestBox, RoutedEventArgs> TextChanged;

        /// <summary>
        /// Event raised when the text selection has changed.
        /// </summary>
        public event TypedEventHandler<RichSuggestBox, RoutedEventArgs> SelectionChanged;

        /// <summary>
        /// Event raised when text is pasted into the control.
        /// </summary>
        public event TypedEventHandler<RichSuggestBox, TextControlPasteEventArgs> Paste;
    }
}
