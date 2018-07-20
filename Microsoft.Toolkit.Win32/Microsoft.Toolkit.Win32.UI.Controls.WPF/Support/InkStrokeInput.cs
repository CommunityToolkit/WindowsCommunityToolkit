// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Input.Inking.InkStrokeInput"/>
    /// </summary>
    public class InkStrokeInput
    {
        private global::Windows.UI.Input.Inking.InkStrokeInput uwpInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkStrokeInput"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Input.Inking.InkStrokeInput"/>
        /// </summary>
        public InkStrokeInput(global::Windows.UI.Input.Inking.InkStrokeInput instance)
        {
            this.uwpInstance = instance;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Input.Inking.InkStrokeInput.InkPresenter"/>
        /// </summary>
        public global::Windows.UI.Input.Inking.InkPresenter InkPresenter
        {
            get => uwpInstance.InkPresenter;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkStrokeInput"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkStrokeInput"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkStrokeInput"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkStrokeInput(
            global::Windows.UI.Input.Inking.InkStrokeInput args)
        {
            return FromInkStrokeInput(args);
        }

        /// <summary>
        /// Creates a <see cref="InkStrokeInput"/> from <see cref="global::Windows.UI.Input.Inking.InkStrokeInput"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkStrokeInput"/> instance containing the event data.</param>
        /// <returns><see cref="InkStrokeInput"/></returns>
        public static InkStrokeInput FromInkStrokeInput(global::Windows.UI.Input.Inking.InkStrokeInput args)
        {
            return new InkStrokeInput(args);
        }
    }
}