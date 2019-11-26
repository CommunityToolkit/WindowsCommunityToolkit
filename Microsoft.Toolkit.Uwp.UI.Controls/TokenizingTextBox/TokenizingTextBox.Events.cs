// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A text input control that auto-suggests and displays token items.
    /// </summary>
    public partial class TokenizingTextBox : Control
    {
        /// <summary>
        /// Event raised when the text input value has changed.
        /// </summary>
        public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxTextChangedEventArgs> TextChanged;

        /// <summary>
        /// Event raised when a suggested item is chosen by the user.
        /// </summary>
        public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxSuggestionChosenEventArgs> SuggestionChosen;

        /// <summary>
        /// Event raised when the user submits the text query.
        /// </summary>
        public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxQuerySubmittedEventArgs> QuerySubmitted;

        /// <summary>
        /// Event raised when a new token item has been added.
        /// </summary>
        public event TypedEventHandler<TokenizingTextBox, TokenizingTextBoxItem> TokenItemAdded;

        /// <summary>
        /// Event raised before a new token item is created from a string, can be used to transform data type from text user entered.
        /// </summary>
        public event TypedEventHandler<TokenizingTextBox, TokenItemCreatingEventArgs> TokenItemCreating;

        /// <summary>
        /// Event raised when a token item has been clicked.
        /// </summary>
        public event TypedEventHandler<TokenizingTextBox, TokenizingTextBoxItem> TokenItemClicked;

        /// <summary>
        /// Event raised when a token item has been removed.
        /// </summary>
        public event TypedEventHandler<TokenizingTextBox, TokenItemRemovedEventArgs> TokenItemRemoved;
    }
}
