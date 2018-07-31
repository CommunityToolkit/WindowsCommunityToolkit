// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.Media.CacheMode"/>
    /// </summary>
    public class CacheMode
    {
        internal Windows.UI.Xaml.Media.CacheMode UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheMode"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Media.CacheMode"/>
        /// </summary>
        public CacheMode(Windows.UI.Xaml.Media.CacheMode instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Media.CacheMode"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.CacheMode"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Media.CacheMode"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator CacheMode(
            Windows.UI.Xaml.Media.CacheMode args)
        {
            return FromCacheMode(args);
        }

        /// <summary>
        /// Creates a <see cref="CacheMode"/> from <see cref="Windows.UI.Xaml.Media.CacheMode"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Media.CacheMode"/> instance containing the event data.</param>
        /// <returns><see cref="CacheMode"/></returns>
        public static CacheMode FromCacheMode(Windows.UI.Xaml.Media.CacheMode args)
        {
            return new CacheMode(args);
        }
    }
}