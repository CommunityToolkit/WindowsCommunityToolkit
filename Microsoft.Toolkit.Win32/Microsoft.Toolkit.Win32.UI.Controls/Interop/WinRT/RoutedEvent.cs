// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.RoutedEvent"/>
    /// </summary>
    public class RoutedEvent
    {
        private Windows.UI.Xaml.RoutedEvent UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEvent"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.RoutedEvent"/>
        /// </summary>
        public RoutedEvent(Windows.UI.Xaml.RoutedEvent instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.RoutedEvent"/> to <see cref="RoutedEvent"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.RoutedEvent"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator RoutedEvent(
            Windows.UI.Xaml.RoutedEvent args)
        {
            return FromRoutedEvent(args);
        }

        /// <summary>
        /// Creates a <see cref="RoutedEvent"/> from <see cref="Windows.UI.Xaml.RoutedEvent"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.RoutedEvent"/> instance containing the event data.</param>
        /// <returns><see cref="RoutedEvent"/></returns>
        public static RoutedEvent FromRoutedEvent(Windows.UI.Xaml.RoutedEvent args)
        {
            return new RoutedEvent(args);
        }
    }
}