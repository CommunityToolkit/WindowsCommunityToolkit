namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Data.BindingBase"/>
    /// </summary>
    public class BindingBase
    {
        internal global::Windows.UI.Xaml.Data.BindingBase UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingBase"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Data.BindingBase"/>
        /// </summary>
        public BindingBase(global::Windows.UI.Xaml.Data.BindingBase instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Data.BindingBase"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.BindingBase"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Data.BindingBase"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator BindingBase(
            global::Windows.UI.Xaml.Data.BindingBase args)
        {
            return FromBindingBase(args);
        }

        /// <summary>
        /// Creates a <see cref="BindingBase"/> from <see cref="global::Windows.UI.Xaml.Data.BindingBase"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Data.BindingBase"/> instance containing the event data.</param>
        /// <returns><see cref="BindingBase"/></returns>
        public static BindingBase FromBindingBase(global::Windows.UI.Xaml.Data.BindingBase args)
        {
            return new BindingBase(args);
        }
    }
}