// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Input.Inking.InkUnprocessedInput"/>
    /// </summary>
    public class InkUnprocessedInput
    {
        private global::Windows.UI.Input.Inking.InkUnprocessedInput uwpInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkUnprocessedInput"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Input.Inking.InkUnprocessedInput"/>
        /// </summary>
        public InkUnprocessedInput(global::Windows.UI.Input.Inking.InkUnprocessedInput instance)
        {
            this.uwpInstance = instance;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Input.Inking.InkUnprocessedInput.InkPresenter"/>
        /// </summary>
        public global::Windows.UI.Input.Inking.InkPresenter InkPresenter
        {
            get => uwpInstance.InkPresenter;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkUnprocessedInput"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkUnprocessedInput"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkUnprocessedInput"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkUnprocessedInput(
            global::Windows.UI.Input.Inking.InkUnprocessedInput args)
        {
            return FromInkUnprocessedInput(args);
        }

        /// <summary>
        /// Creates a <see cref="InkUnprocessedInput"/> from <see cref="global::Windows.UI.Input.Inking.InkUnprocessedInput"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkUnprocessedInput"/> instance containing the event data.</param>
        /// <returns><see cref="InkUnprocessedInput"/></returns>
        public static InkUnprocessedInput FromInkUnprocessedInput(global::Windows.UI.Input.Inking.InkUnprocessedInput args)
        {
            return new InkUnprocessedInput(args);
        }
    }
}