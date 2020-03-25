// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Event arguments for <see cref="TokenizingTextBox.TokenItemRemoved"/> event.
    /// </summary>
    public class TokenItemRemovedEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenItemRemovedEventArgs"/> class.
        /// </summary>
        /// <param name="item">Item being removed.</param>
        /// <param name="token"><see cref="TokenizingTextBoxItem"/> container being closed.</param>
        public TokenItemRemovedEventArgs(object item, TokenizingTextBoxItem token)
        {
            Item = item;
            Token = token;
        }

        /// <summary>
        /// Gets the Item being closed.
        /// </summary>
        public object Item { get; private set; }

        /// <summary>
        /// Gets the <see cref="TokenizingTextBoxItem"/> being removed.
        /// </summary>
        public TokenizingTextBoxItem Token { get; private set; }
    }
}