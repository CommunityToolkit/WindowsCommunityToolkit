namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Controls.ColumnDefinitionCollection"/>
    /// </summary>
    public class ColumnDefinitionCollection
    {
        internal global::Windows.UI.Xaml.Controls.ColumnDefinitionCollection UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDefinitionCollection"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.ColumnDefinitionCollection"/>
        /// </summary>
        public ColumnDefinitionCollection(global::Windows.UI.Xaml.Controls.ColumnDefinitionCollection instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.ColumnDefinitionCollection"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.ColumnDefinitionCollection"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.ColumnDefinitionCollection"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ColumnDefinitionCollection(
            global::Windows.UI.Xaml.Controls.ColumnDefinitionCollection args)
        {
            return FromColumnDefinitionCollection(args);
        }

        /// <summary>
        /// Creates a <see cref="ColumnDefinitionCollection"/> from <see cref="global::Windows.UI.Xaml.Controls.ColumnDefinitionCollection"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.ColumnDefinitionCollection"/> instance containing the event data.</param>
        /// <returns><see cref="ColumnDefinitionCollection"/></returns>
        public static ColumnDefinitionCollection FromColumnDefinitionCollection(global::Windows.UI.Xaml.Controls.ColumnDefinitionCollection args)
        {
            return new ColumnDefinitionCollection(args);
        }
    }
}