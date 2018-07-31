// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.TriggerCollection"/>
    /// </summary>
    public class TriggerCollection
    {
        internal Windows.UI.Xaml.TriggerCollection UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerCollection"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.TriggerCollection"/>
        /// </summary>
        public TriggerCollection(Windows.UI.Xaml.TriggerCollection instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.TriggerCollection"/> to <see cref="TriggerCollection"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.TriggerCollection"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TriggerCollection(
            Windows.UI.Xaml.TriggerCollection args)
        {
            return FromTriggerCollection(args);
        }

        /// <summary>
        /// Creates a <see cref="TriggerCollection"/> from <see cref="Windows.UI.Xaml.TriggerCollection"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.TriggerCollection"/> instance containing the event data.</param>
        /// <returns><see cref="TriggerCollection"/></returns>
        public static TriggerCollection FromTriggerCollection(Windows.UI.Xaml.TriggerCollection args)
        {
            return new TriggerCollection(args);
        }
    }
}