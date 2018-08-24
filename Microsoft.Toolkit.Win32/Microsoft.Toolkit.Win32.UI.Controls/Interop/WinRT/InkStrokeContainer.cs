// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Input.Inking.InkStrokeContainer"/>
    /// </summary>
    public class InkStrokeContainer
    {
        private Windows.UI.Input.Inking.InkStrokeContainer uwpInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkStrokeContainer"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Input.Inking.InkStrokeContainer"/>
        /// </summary>
        public InkStrokeContainer(Windows.UI.Input.Inking.InkStrokeContainer instance)
        {
            this.uwpInstance = instance;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Input.Inking.InkStrokeContainer.BoundingRect"/>
        /// </summary>
        public Windows.Foundation.Rect BoundingRect
        {
            get => uwpInstance.BoundingRect;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Input.Inking.InkStrokeContainer"/> to <see cref="InkStrokeContainer"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Input.Inking.InkStrokeContainer"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkStrokeContainer(
            Windows.UI.Input.Inking.InkStrokeContainer args)
        {
            return FromInkStrokeContainer(args);
        }

        /// <summary>
        /// Creates a <see cref="InkStrokeContainer"/> from <see cref="Windows.UI.Input.Inking.InkStrokeContainer"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Input.Inking.InkStrokeContainer"/> instance containing the event data.</param>
        /// <returns><see cref="InkStrokeContainer"/></returns>
        public static InkStrokeContainer FromInkStrokeContainer(Windows.UI.Input.Inking.InkStrokeContainer args)
        {
            return new InkStrokeContainer(args);
        }
    }
}