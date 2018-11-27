// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.Controls.RowDefinitionCollection"/>
    /// </summary>
    public class RowDefinitionCollection
    {
        private Windows.UI.Xaml.Controls.RowDefinitionCollection UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RowDefinitionCollection"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.RowDefinitionCollection"/>
        /// </summary>
        public RowDefinitionCollection(Windows.UI.Xaml.Controls.RowDefinitionCollection instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Controls.RowDefinitionCollection"/> to <see cref="RowDefinitionCollection"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.RowDefinitionCollection"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator RowDefinitionCollection(
            Windows.UI.Xaml.Controls.RowDefinitionCollection args)
        {
            return FromRowDefinitionCollection(args);
        }

        /// <summary>
        /// Creates a <see cref="RowDefinitionCollection"/> from <see cref="Windows.UI.Xaml.Controls.RowDefinitionCollection"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.RowDefinitionCollection"/> instance containing the event data.</param>
        /// <returns><see cref="RowDefinitionCollection"/></returns>
        public static RowDefinitionCollection FromRowDefinitionCollection(Windows.UI.Xaml.Controls.RowDefinitionCollection args)
        {
            return new RowDefinitionCollection(args);
        }
    }
}