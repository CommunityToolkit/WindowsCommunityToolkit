// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.ScalarTransition"/>
    /// </summary>
    public class ScalarTransition
    {
        internal global::Windows.UI.Xaml.ScalarTransition UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarTransition"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.ScalarTransition"/>
        /// </summary>
        public ScalarTransition(global::Windows.UI.Xaml.ScalarTransition instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.ScalarTransition.Duration"/>
        /// </summary>
        public System.TimeSpan Duration
        {
            get => UwpInstance.Duration;
            set => UwpInstance.Duration = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.ScalarTransition"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.ScalarTransition"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.ScalarTransition"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ScalarTransition(
            global::Windows.UI.Xaml.ScalarTransition args)
        {
            return FromScalarTransition(args);
        }

        /// <summary>
        /// Creates a <see cref="ScalarTransition"/> from <see cref="global::Windows.UI.Xaml.ScalarTransition"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.ScalarTransition"/> instance containing the event data.</param>
        /// <returns><see cref="ScalarTransition"/></returns>
        public static ScalarTransition FromScalarTransition(global::Windows.UI.Xaml.ScalarTransition args)
        {
            return new ScalarTransition(args);
        }
    }
}