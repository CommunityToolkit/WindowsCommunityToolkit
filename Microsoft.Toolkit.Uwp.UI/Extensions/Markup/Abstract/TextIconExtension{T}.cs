// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// An abstract <see cref="MarkupExtension"/> which to produce text-based icons.
    /// </summary>
    /// <typeparam name="T">The type representing the glyph for the current icon.</typeparam>
    public abstract class TextIconExtension<T> : TextIconExtension
    {
        /// <summary>
        /// Gets or sets the <typeparamref name="T"/> value representing the icon to display.
        /// </summary>
        public T Glyph { get; set; }
    }
}
