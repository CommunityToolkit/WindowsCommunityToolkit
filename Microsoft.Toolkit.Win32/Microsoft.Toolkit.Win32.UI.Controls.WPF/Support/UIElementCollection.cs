namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Controls.UIElementCollection"/>
    /// </summary>
    public class UIElementCollection
    {
        internal global::Windows.UI.Xaml.Controls.UIElementCollection UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIElementCollection"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.UIElementCollection"/>
        /// </summary>
        public UIElementCollection(global::Windows.UI.Xaml.Controls.UIElementCollection instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.UIElementCollection"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.UIElementCollection"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.UIElementCollection"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator UIElementCollection(
            global::Windows.UI.Xaml.Controls.UIElementCollection args)
        {
            return FromUIElementCollection(args);
        }

        /// <summary>
        /// Creates a <see cref="UIElementCollection"/> from <see cref="global::Windows.UI.Xaml.Controls.UIElementCollection"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.UIElementCollection"/> instance containing the event data.</param>
        /// <returns><see cref="UIElementCollection"/></returns>
        public static UIElementCollection FromUIElementCollection(global::Windows.UI.Xaml.Controls.UIElementCollection args)
        {
            return new UIElementCollection(args);
        }
    }
}