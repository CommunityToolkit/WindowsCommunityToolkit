// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.DataTemplate"/>
    /// </summary>
    public class DataTemplate
    {
        internal Windows.UI.Xaml.DataTemplate UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTemplate"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.DataTemplate"/>
        /// </summary>
        public DataTemplate(Windows.UI.Xaml.DataTemplate instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.DataTemplate"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.DataTemplate"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.DataTemplate"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator DataTemplate(
            Windows.UI.Xaml.DataTemplate args)
        {
            return FromDataTemplate(args);
        }

        /// <summary>
        /// Creates a <see cref="DataTemplate"/> from <see cref="Windows.UI.Xaml.DataTemplate"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.DataTemplate"/> instance containing the event data.</param>
        /// <returns><see cref="DataTemplate"/></returns>
        public static DataTemplate FromDataTemplate(Windows.UI.Xaml.DataTemplate args)
        {
            return new DataTemplate(args);
        }
    }
}