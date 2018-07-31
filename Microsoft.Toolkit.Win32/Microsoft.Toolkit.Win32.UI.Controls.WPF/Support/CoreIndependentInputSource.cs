// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="Windows.UI.Core.CoreIndependentInputSource"/>
    /// </summary>
    public class CoreIndependentInputSource
    {
        internal Windows.UI.Core.CoreIndependentInputSource UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreIndependentInputSource"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Core.CoreIndependentInputSource"/>
        /// </summary>
        public CoreIndependentInputSource(Windows.UI.Core.CoreIndependentInputSource instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Core.CoreIndependentInputSource.IsInputEnabled"/>
        /// </summary>
        public bool IsInputEnabled
        {
            get => UwpInstance.IsInputEnabled;
            set => UwpInstance.IsInputEnabled = value;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Core.CoreIndependentInputSource.Dispatcher"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.CoreDispatcher Dispatcher
        {
            get => UwpInstance.Dispatcher;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Core.CoreIndependentInputSource.PointerCursor"/>
        /// </summary>
        public Windows.UI.Core.CoreCursor PointerCursor
        {
            get => UwpInstance.PointerCursor;
            set => UwpInstance.PointerCursor = value;
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Windows.UI.Core.CoreIndependentInputSource.HasCapture"/>
        /// </summary>
        public bool HasCapture
        {
            get => UwpInstance.HasCapture;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Core.CoreIndependentInputSource.PointerPosition"/>
        /// </summary>
        public Windows.Foundation.Point PointerPosition
        {
            get => UwpInstance.PointerPosition;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Core.CoreIndependentInputSource.DispatcherQueue"/>
        /// </summary>
        public Windows.System.DispatcherQueue DispatcherQueue
        {
            get => UwpInstance.DispatcherQueue;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Core.CoreIndependentInputSource"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.CoreIndependentInputSource"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Core.CoreIndependentInputSource"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator CoreIndependentInputSource(
            Windows.UI.Core.CoreIndependentInputSource args)
        {
            return FromCoreIndependentInputSource(args);
        }

        /// <summary>
        /// Creates a <see cref="CoreIndependentInputSource"/> from <see cref="Windows.UI.Core.CoreIndependentInputSource"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Core.CoreIndependentInputSource"/> instance containing the event data.</param>
        /// <returns><see cref="CoreIndependentInputSource"/></returns>
        public static CoreIndependentInputSource FromCoreIndependentInputSource(Windows.UI.Core.CoreIndependentInputSource args)
        {
            return new CoreIndependentInputSource(args);
        }
    }
}