// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Common.Deferred;

namespace CommunityToolkit.WinUI.UI.Controls
{
    /// <summary>
    /// Provide data for <see cref="RichSuggestBox.SuggestionRequested"/> event.
    /// </summary>
    public class SuggestionRequestedEventArgs : DeferredCancelEventArgs
    {
        /// <summary>
        /// Gets or sets the prefix character used for the query.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Gets or sets the query for suggestions.
        /// </summary>
        public string QueryText { get; set; }
    }
}
