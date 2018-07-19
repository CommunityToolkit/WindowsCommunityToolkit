namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Media.ImageSource"/>
    /// </summary>
    public class ImageSource
    {
        internal global::Windows.UI.Xaml.Media.ImageSource UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSource"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Media.ImageSource"/>
        /// </summary>
        public ImageSource(global::Windows.UI.Xaml.Media.ImageSource instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Media.ImageSource"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.ImageSource"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Media.ImageSource"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ImageSource(
            global::Windows.UI.Xaml.Media.ImageSource args)
        {
            return FromImageSource(args);
        }

        /// <summary>
        /// Creates a <see cref="ImageSource"/> from <see cref="global::Windows.UI.Xaml.Media.ImageSource"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Media.ImageSource"/> instance containing the event data.</param>
        /// <returns><see cref="ImageSource"/></returns>
        public static ImageSource FromImageSource(global::Windows.UI.Xaml.Media.ImageSource args)
        {
            return new ImageSource(args);
        }
    }
}