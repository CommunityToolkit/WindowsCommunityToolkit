// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.Deferred;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Event arguments for <see cref="TokenizingTextBox.TokenItemCreating"/> event.
    /// </summary>
    public class TokenItemCreatingEventArgs : DeferredCancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenItemCreatingEventArgs"/> class.
        /// </summary>
        /// <param name="token">User entered string.</param>
        public TokenItemCreatingEventArgs(string token)
        {
            TokenText = token;
        }

        /// <summary>
        /// Gets token as typed by the user.
        /// </summary>
        public string TokenText { get; private set; }

        /// <summary>
        /// Gets or sets the item to be added to the <see cref="TokenizingTextBox"/>. If null, <see cref="TokenText"/> string will be added.
        /// </summary>
        public object Item { get; set; } = null;
    }
}