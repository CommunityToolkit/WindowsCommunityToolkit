// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.BrushTransition"/>
    /// </summary>
    public class BrushTransition
    {
        internal Windows.UI.Xaml.BrushTransition UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrushTransition"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.BrushTransition"/>
        /// </summary>
        public BrushTransition(Windows.UI.Xaml.BrushTransition instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.BrushTransition.Duration"/>
        /// </summary>
        public System.TimeSpan Duration
        {
            get => UwpInstance.Duration;
            set => UwpInstance.Duration = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.BrushTransition"/> to <see cref="BrushTransition"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.BrushTransition"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator BrushTransition(
            Windows.UI.Xaml.BrushTransition args)
        {
            return FromBrushTransition(args);
        }

        /// <summary>
        /// Creates a <see cref="BrushTransition"/> from <see cref="Windows.UI.Xaml.BrushTransition"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.BrushTransition"/> instance containing the event data.</param>
        /// <returns><see cref="BrushTransition"/></returns>
        public static BrushTransition FromBrushTransition(Windows.UI.Xaml.BrushTransition args)
        {
            return new BrushTransition(args);
        }
    }
}