namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Controls.Maps.MapScene"/>
    /// </summary>
    public class MapScene
    {
        internal global::Windows.UI.Xaml.Controls.Maps.MapScene UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapScene"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.Maps.MapScene"/>
        /// </summary>
        public MapScene(global::Windows.UI.Xaml.Controls.Maps.MapScene instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.Maps.MapScene.TargetCamera"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.MapCamera TargetCamera
        {
            get => UwpInstance.TargetCamera;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapScene"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.MapScene"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapScene"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MapScene(
            global::Windows.UI.Xaml.Controls.Maps.MapScene args)
        {
            return FromMapScene(args);
        }

        /// <summary>
        /// Creates a <see cref="MapScene"/> from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapScene"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapScene"/> instance containing the event data.</param>
        /// <returns><see cref="MapScene"/></returns>
        public static MapScene FromMapScene(global::Windows.UI.Xaml.Controls.Maps.MapScene args)
        {
            return new MapScene(args);
        }
    }
}