// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.Controls.UIElementCollection"/>
    /// </summary>
    public class UIElementCollection
    {
        internal Windows.UI.Xaml.Controls.UIElementCollection UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIElementCollection"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.UIElementCollection"/>
        /// </summary>
        public UIElementCollection(Windows.UI.Xaml.Controls.UIElementCollection instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Controls.UIElementCollection"/> to <see cref="UIElementCollection"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.UIElementCollection"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator UIElementCollection(
            Windows.UI.Xaml.Controls.UIElementCollection args)
        {
            return FromUIElementCollection(args);
        }

        /// <summary>
        /// Creates a <see cref="UIElementCollection"/> from <see cref="Windows.UI.Xaml.Controls.UIElementCollection"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.UIElementCollection"/> instance containing the event data.</param>
        /// <returns><see cref="UIElementCollection"/></returns>
        public static UIElementCollection FromUIElementCollection(Windows.UI.Xaml.Controls.UIElementCollection args)
        {
            return new UIElementCollection(args);
        }
    }
}