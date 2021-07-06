// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Provides data for <see cref="RichSuggestBox.TokenSelected"/> event.
    /// </summary>
    public class RichSuggestTokenSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the selected token.
        /// </summary>
        public RichSuggestToken Token { get; }

        /// <summary>
        /// Gets the position and size of the selected rectangle measured from the top left of the <see cref="RichSuggestBox"/> control.
        /// </summary>
        public Rect Rect { get; }

        internal RichSuggestTokenSelectedEventArgs(RichSuggestToken token, Rect rect)
        {
            Token = token;
            Rect = rect;
        }
    }
}
