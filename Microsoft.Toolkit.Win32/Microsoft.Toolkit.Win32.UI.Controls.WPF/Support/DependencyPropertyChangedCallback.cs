namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.DependencyPropertyChangedCallback"/>
    /// </summary>
    public class DependencyPropertyChangedCallback
    {
        internal global::Windows.UI.Xaml.DependencyPropertyChangedCallback UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyPropertyChangedCallback"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.DependencyPropertyChangedCallback"/>
        /// </summary>
        public DependencyPropertyChangedCallback(global::Windows.UI.Xaml.DependencyPropertyChangedCallback instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.DependencyPropertyChangedCallback"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.DependencyPropertyChangedCallback"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.DependencyPropertyChangedCallback"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator DependencyPropertyChangedCallback(
            global::Windows.UI.Xaml.DependencyPropertyChangedCallback args)
        {
            return FromDependencyPropertyChangedCallback(args);
        }

        /// <summary>
        /// Creates a <see cref="DependencyPropertyChangedCallback"/> from <see cref="global::Windows.UI.Xaml.DependencyPropertyChangedCallback"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.DependencyPropertyChangedCallback"/> instance containing the event data.</param>
        /// <returns><see cref="DependencyPropertyChangedCallback"/></returns>
        public static DependencyPropertyChangedCallback FromDependencyPropertyChangedCallback(global::Windows.UI.Xaml.DependencyPropertyChangedCallback args)
        {
            return new DependencyPropertyChangedCallback(args);
        }
    }
}