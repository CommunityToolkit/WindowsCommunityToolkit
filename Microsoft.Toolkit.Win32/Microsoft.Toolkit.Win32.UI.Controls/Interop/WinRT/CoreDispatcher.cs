// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Core.CoreDispatcher"/>
    /// </summary>
    public class CoreDispatcher
    {
        private Windows.UI.Core.CoreDispatcher UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreDispatcher"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Core.CoreDispatcher"/>
        /// </summary>
        public CoreDispatcher(Windows.UI.Core.CoreDispatcher instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Windows.UI.Core.CoreDispatcher.HasThreadAccess"/>
        /// </summary>
        public bool HasThreadAccess
        {
            get => UwpInstance.HasThreadAccess;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Core.CoreDispatcher.CurrentPriority"/>
        /// </summary>
        public Windows.UI.Core.CoreDispatcherPriority CurrentPriority
        {
            get => UwpInstance.CurrentPriority;
            set => UwpInstance.CurrentPriority = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Core.CoreDispatcher"/> to <see cref="CoreDispatcher"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Core.CoreDispatcher"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator CoreDispatcher(
            Windows.UI.Core.CoreDispatcher args)
        {
            return FromCoreDispatcher(args);
        }

        /// <summary>
        /// Creates a <see cref="CoreDispatcher"/> from <see cref="Windows.UI.Core.CoreDispatcher"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Core.CoreDispatcher"/> instance containing the event data.</param>
        /// <returns><see cref="CoreDispatcher"/></returns>
        public static CoreDispatcher FromCoreDispatcher(Windows.UI.Core.CoreDispatcher args)
        {
            return new CoreDispatcher(args);
        }
    }
}