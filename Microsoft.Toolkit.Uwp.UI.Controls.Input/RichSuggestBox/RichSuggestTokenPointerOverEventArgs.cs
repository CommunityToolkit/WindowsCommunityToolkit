// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Input;
using Windows.UI.Text;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Provides data for <see cref="RichSuggestBox.TokenPointerOver"/> event.
    /// </summary>
    public class RichSuggestTokenPointerOverEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the selected token.
        /// </summary>
        public RichSuggestToken Token { get; set; }

        /// <summary>
        /// Gets or sets the range associated with the token.
        /// </summary>
        public ITextRange Range { get; set; }

        /// <summary>
        /// Gets or sets a PointerPoint object relative to the <see cref="RichSuggestBox"/> control.
        /// </summary>
        public PointerPoint CurrentPoint { get; set; }
    }
}
