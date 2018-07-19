namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Controls.DataTemplateSelector"/>
    /// </summary>
    public class DataTemplateSelector
    {
        internal global::Windows.UI.Xaml.Controls.DataTemplateSelector UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTemplateSelector"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.DataTemplateSelector"/>
        /// </summary>
        public DataTemplateSelector(global::Windows.UI.Xaml.Controls.DataTemplateSelector instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.DataTemplateSelector"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.DataTemplateSelector"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.DataTemplateSelector"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator DataTemplateSelector(
            global::Windows.UI.Xaml.Controls.DataTemplateSelector args)
        {
            return FromDataTemplateSelector(args);
        }

        /// <summary>
        /// Creates a <see cref="DataTemplateSelector"/> from <see cref="global::Windows.UI.Xaml.Controls.DataTemplateSelector"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.DataTemplateSelector"/> instance containing the event data.</param>
        /// <returns><see cref="DataTemplateSelector"/></returns>
        public static DataTemplateSelector FromDataTemplateSelector(global::Windows.UI.Xaml.Controls.DataTemplateSelector args)
        {
            return new DataTemplateSelector(args);
        }
    }
}