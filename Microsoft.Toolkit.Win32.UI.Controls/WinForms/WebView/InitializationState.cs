namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{

    /// <summary>
    /// Initialization states of WebView object.
    /// </summary>
    internal enum InitializationState
    {
        /// <summary>
        /// The state in which the WebView has not been initialized.
        /// At this state, all operations on the object would cause InvalidOperationException.
        /// The object can only transit to 'IsInitializing' state with BeginInit() call.
        /// </summary>
        Uninitialized,

        /// <summary>
        /// The state in which the WebView is being initialized. At this state, user can
        /// set values into the required properties. The object can only transit to 'IsInitialized' state
        /// with EndInit() call.
        /// </summary>
        IsInitializing,

        /// <summary>
        /// The state in which the WebView object is fully initialized. At this state the object
        /// is fully functional. There is no valid transition out of the state.
        /// </summary>
        IsInitialized,
    }

}