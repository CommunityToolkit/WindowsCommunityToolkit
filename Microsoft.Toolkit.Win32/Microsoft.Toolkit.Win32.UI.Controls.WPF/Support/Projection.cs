namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Media.Projection"/>
    /// </summary>
    public class Projection
    {
        internal global::Windows.UI.Xaml.Media.Projection UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Projection"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Media.Projection"/>
        /// </summary>
        public Projection(global::Windows.UI.Xaml.Media.Projection instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Media.Projection"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.Projection"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Media.Projection"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Projection(
            global::Windows.UI.Xaml.Media.Projection args)
        {
            return FromProjection(args);
        }

        /// <summary>
        /// Creates a <see cref="Projection"/> from <see cref="global::Windows.UI.Xaml.Media.Projection"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Media.Projection"/> instance containing the event data.</param>
        /// <returns><see cref="Projection"/></returns>
        public static Projection FromProjection(global::Windows.UI.Xaml.Media.Projection args)
        {
            return new Projection(args);
        }
    }
}