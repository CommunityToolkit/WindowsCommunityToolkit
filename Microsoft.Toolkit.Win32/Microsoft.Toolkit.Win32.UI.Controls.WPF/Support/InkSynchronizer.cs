// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Input.Inking.InkSynchronizer"/>
    /// </summary>
    public class InkSynchronizer
    {
        private global::Windows.UI.Input.Inking.InkSynchronizer uwpInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkSynchronizer"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Input.Inking.InkSynchronizer"/>
        /// </summary>
        public InkSynchronizer(global::Windows.UI.Input.Inking.InkSynchronizer instance)
        {
            this.uwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkSynchronizer"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkSynchronizer"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkSynchronizer"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkSynchronizer(
            global::Windows.UI.Input.Inking.InkSynchronizer args)
        {
            return FromInkSynchronizer(args);
        }

        /// <summary>
        /// Creates a <see cref="InkSynchronizer"/> from <see cref="global::Windows.UI.Input.Inking.InkSynchronizer"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkSynchronizer"/> instance containing the event data.</param>
        /// <returns><see cref="InkSynchronizer"/></returns>
        public static InkSynchronizer FromInkSynchronizer(global::Windows.UI.Input.Inking.InkSynchronizer args)
        {
            return new InkSynchronizer(args);
        }
    }
}