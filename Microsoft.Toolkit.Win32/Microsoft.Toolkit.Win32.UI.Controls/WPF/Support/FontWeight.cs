namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Text.FontWeight"/>
    /// </summary>
    public class FontWeight
    {
        internal global::Windows.UI.Text.FontWeight UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontWeight"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Text.FontWeight"/>
        /// </summary>
        public FontWeight(global::Windows.UI.Text.FontWeight instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Text.FontWeight"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.FontWeight"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Text.FontWeight"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator FontWeight(
            global::Windows.UI.Text.FontWeight args)
        {
            return FromFontWeight(args);
        }

        /// <summary>
        /// Creates a <see cref="FontWeight"/> from <see cref="global::Windows.UI.Text.FontWeight"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Text.FontWeight"/> instance containing the event data.</param>
        /// <returns><see cref="FontWeight"/></returns>
        public static FontWeight FromFontWeight(global::Windows.UI.Text.FontWeight args)
        {
            return new FontWeight(args);
        }
    }
}