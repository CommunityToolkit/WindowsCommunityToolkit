// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    /// <summary>
    /// Initialization states of <see cref="WebView"/> object.
    /// </summary>
    /// <seealso cref="WebView"/>
    /// <seealso cref="ISupportInitialize"/>
    internal enum InitializationState
    {
        /// <summary>
        /// The state in which the <see cref="WebView"/> has not been initialized.
        /// At this state, all operations on the object would cause InvalidOperationException.
        /// The object can only transit to 'IsInitializing' state with <see cref="ISupportInitialize.BeginInit"/> call.
        /// </summary>
        Uninitialized,

        /// <summary>
        /// The state in which the <see cref="WebView"/> is being initialized. At this state, user can
        /// set values into the required properties. The object can only transit to 'IsInitialized' state
        /// with <see cref="ISupportInitialize.EndInit"/> call.
        /// </summary>
        IsInitializing,

        /// <summary>
        /// The state in which the <see cref="WebView"/> object is fully initialized. At this state the object
        /// is fully functional. There is no valid transition out of the state.
        /// </summary>
        IsInitialized
    }
}