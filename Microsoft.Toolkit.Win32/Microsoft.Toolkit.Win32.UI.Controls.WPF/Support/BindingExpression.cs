namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Data.BindingExpression"/>
    /// </summary>
    public class BindingExpression
    {
        internal global::Windows.UI.Xaml.Data.BindingExpression UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingExpression"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Data.BindingExpression"/>
        /// </summary>
        public BindingExpression(global::Windows.UI.Xaml.Data.BindingExpression instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Data.BindingExpression.DataItem"/>
        /// </summary>
        public object DataItem
        {
            get => UwpInstance.DataItem;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Data.BindingExpression.ParentBinding"/>
        /// </summary>
        public global::Windows.UI.Xaml.Data.Binding ParentBinding
        {
            get => UwpInstance.ParentBinding;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Data.BindingExpression"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.BindingExpression"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Data.BindingExpression"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator BindingExpression(
            global::Windows.UI.Xaml.Data.BindingExpression args)
        {
            return FromBindingExpression(args);
        }

        /// <summary>
        /// Creates a <see cref="BindingExpression"/> from <see cref="global::Windows.UI.Xaml.Data.BindingExpression"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Data.BindingExpression"/> instance containing the event data.</param>
        /// <returns><see cref="BindingExpression"/></returns>
        public static BindingExpression FromBindingExpression(global::Windows.UI.Xaml.Data.BindingExpression args)
        {
            return new BindingExpression(args);
        }
    }
}