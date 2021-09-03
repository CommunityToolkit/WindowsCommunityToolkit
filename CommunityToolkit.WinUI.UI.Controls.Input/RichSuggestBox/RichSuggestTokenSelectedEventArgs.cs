// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.UI.Text;

namespace CommunityToolkit.WinUI.UI.Controls
{
    /// <summary>
    /// Provides data for <see cref="RichSuggestBox.TokenSelected"/> event.
    /// </summary>
    public class RichSuggestTokenSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the selected token.
        /// </summary>
        public RichSuggestToken Token { get; set; }

        /// <summary>
        /// Gets or sets the range associated with the token.
        /// </summary>
        public ITextRange Range { get; set; }
    }
}
