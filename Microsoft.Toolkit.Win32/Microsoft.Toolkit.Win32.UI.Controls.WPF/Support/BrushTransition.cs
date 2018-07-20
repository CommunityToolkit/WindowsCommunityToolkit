// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.BrushTransition"/>
    /// </summary>
    public class BrushTransition
    {
        internal global::Windows.UI.Xaml.BrushTransition UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrushTransition"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.BrushTransition"/>
        /// </summary>
        public BrushTransition(global::Windows.UI.Xaml.BrushTransition instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.BrushTransition.Duration"/>
        /// </summary>
        public System.TimeSpan Duration
        {
            get => UwpInstance.Duration;
            set => UwpInstance.Duration = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.BrushTransition"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.BrushTransition"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.BrushTransition"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator BrushTransition(
            global::Windows.UI.Xaml.BrushTransition args)
        {
            return FromBrushTransition(args);
        }

        /// <summary>
        /// Creates a <see cref="BrushTransition"/> from <see cref="global::Windows.UI.Xaml.BrushTransition"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.BrushTransition"/> instance containing the event data.</param>
        /// <returns><see cref="BrushTransition"/></returns>
        public static BrushTransition FromBrushTransition(global::Windows.UI.Xaml.BrushTransition args)
        {
            return new BrushTransition(args);
        }
    }
}