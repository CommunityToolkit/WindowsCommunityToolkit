// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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