namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Controls.Maps.MapCustomExperience"/>
    /// </summary>
    public class MapCustomExperience
    {
        internal global::Windows.UI.Xaml.Controls.Maps.MapCustomExperience UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapCustomExperience"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.Maps.MapCustomExperience"/>
        /// </summary>
        public MapCustomExperience(global::Windows.UI.Xaml.Controls.Maps.MapCustomExperience instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapCustomExperience"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.MapCustomExperience"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapCustomExperience"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MapCustomExperience(
            global::Windows.UI.Xaml.Controls.Maps.MapCustomExperience args)
        {
            return FromMapCustomExperience(args);
        }

        /// <summary>
        /// Creates a <see cref="MapCustomExperience"/> from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapCustomExperience"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapCustomExperience"/> instance containing the event data.</param>
        /// <returns><see cref="MapCustomExperience"/></returns>
        public static MapCustomExperience FromMapCustomExperience(global::Windows.UI.Xaml.Controls.Maps.MapCustomExperience args)
        {
            return new MapCustomExperience(args);
        }
    }
}