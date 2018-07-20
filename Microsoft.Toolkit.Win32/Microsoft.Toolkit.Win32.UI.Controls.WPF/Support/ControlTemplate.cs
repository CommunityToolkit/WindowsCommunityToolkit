// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Controls.ControlTemplate"/>
    /// </summary>
    public class ControlTemplate
    {
        internal global::Windows.UI.Xaml.Controls.ControlTemplate UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlTemplate"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.ControlTemplate"/>
        /// </summary>
        public ControlTemplate(global::Windows.UI.Xaml.Controls.ControlTemplate instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Controls.ControlTemplate.TargetType"/>
        /// </summary>
        public System.Type TargetType
        {
            get => UwpInstance.TargetType;
            set => UwpInstance.TargetType = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.ControlTemplate"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.ControlTemplate"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.ControlTemplate"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ControlTemplate(
            global::Windows.UI.Xaml.Controls.ControlTemplate args)
        {
            return FromControlTemplate(args);
        }

        /// <summary>
        /// Creates a <see cref="ControlTemplate"/> from <see cref="global::Windows.UI.Xaml.Controls.ControlTemplate"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.ControlTemplate"/> instance containing the event data.</param>
        /// <returns><see cref="ControlTemplate"/></returns>
        public static ControlTemplate FromControlTemplate(global::Windows.UI.Xaml.Controls.ControlTemplate args)
        {
            return new ControlTemplate(args);
        }
    }
}