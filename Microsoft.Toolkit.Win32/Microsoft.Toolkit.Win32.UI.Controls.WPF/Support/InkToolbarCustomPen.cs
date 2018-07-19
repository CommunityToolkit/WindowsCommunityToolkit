namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomPen"/>
    /// </summary>
    public class InkToolbarCustomPen
    {
        internal global::Windows.UI.Xaml.Controls.InkToolbarCustomPen UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarCustomPen"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomPen"/>
        /// </summary>
        public InkToolbarCustomPen(global::Windows.UI.Xaml.Controls.InkToolbarCustomPen instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomPen"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarCustomPen"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomPen"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkToolbarCustomPen(
            global::Windows.UI.Xaml.Controls.InkToolbarCustomPen args)
        {
            return FromInkToolbarCustomPen(args);
        }

        /// <summary>
        /// Creates a <see cref="InkToolbarCustomPen"/> from <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomPen"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomPen"/> instance containing the event data.</param>
        /// <returns><see cref="InkToolbarCustomPen"/></returns>
        public static InkToolbarCustomPen FromInkToolbarCustomPen(global::Windows.UI.Xaml.Controls.InkToolbarCustomPen args)
        {
            return new InkToolbarCustomPen(args);
        }
    }
}