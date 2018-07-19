namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Core.CoreIndependentInputSource"/>
    /// </summary>
    public class CoreIndependentInputSource
    {
        internal global::Windows.UI.Core.CoreIndependentInputSource UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreIndependentInputSource"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Core.CoreIndependentInputSource"/>
        /// </summary>
        public CoreIndependentInputSource(global::Windows.UI.Core.CoreIndependentInputSource instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="global::Windows.UI.Core.CoreIndependentInputSource.IsInputEnabled"/>
        /// </summary>
        public bool IsInputEnabled
        {
            get => UwpInstance.IsInputEnabled;
            set => UwpInstance.IsInputEnabled = value;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Core.CoreIndependentInputSource.Dispatcher"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.CoreDispatcher Dispatcher
        {
            get => UwpInstance.Dispatcher;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Core.CoreIndependentInputSource.PointerCursor"/>
        /// </summary>
        public global::Windows.UI.Core.CoreCursor PointerCursor
        {
            get => UwpInstance.PointerCursor;
            set => UwpInstance.PointerCursor = value;
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="global::Windows.UI.Core.CoreIndependentInputSource.HasCapture"/>
        /// </summary>
        public bool HasCapture
        {
            get => UwpInstance.HasCapture;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Core.CoreIndependentInputSource.PointerPosition"/>
        /// </summary>
        public global::Windows.Foundation.Point PointerPosition
        {
            get => UwpInstance.PointerPosition;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Core.CoreIndependentInputSource.DispatcherQueue"/>
        /// </summary>
        public global::Windows.System.DispatcherQueue DispatcherQueue
        {
            get => UwpInstance.DispatcherQueue;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Core.CoreIndependentInputSource"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.CoreIndependentInputSource"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Core.CoreIndependentInputSource"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator CoreIndependentInputSource(
            global::Windows.UI.Core.CoreIndependentInputSource args)
        {
            return FromCoreIndependentInputSource(args);
        }

        /// <summary>
        /// Creates a <see cref="CoreIndependentInputSource"/> from <see cref="global::Windows.UI.Core.CoreIndependentInputSource"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Core.CoreIndependentInputSource"/> instance containing the event data.</param>
        /// <returns><see cref="CoreIndependentInputSource"/></returns>
        public static CoreIndependentInputSource FromCoreIndependentInputSource(global::Windows.UI.Core.CoreIndependentInputSource args)
        {
            return new CoreIndependentInputSource(args);
        }
    }
}