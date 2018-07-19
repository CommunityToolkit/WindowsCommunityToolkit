namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.Devices.Geolocation.Geopath"/>
    /// </summary>
    public class Geopath
    {
        internal global::Windows.Devices.Geolocation.Geopath UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Geopath"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.Devices.Geolocation.Geopath"/>
        /// </summary>
        public Geopath(global::Windows.Devices.Geolocation.Geopath instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.Devices.Geolocation.Geopath.Positions"/>
        /// </summary>
        public System.Collections.Generic.IReadOnlyList<global::Windows.Devices.Geolocation.BasicGeoposition> Positions
        {
            get => UwpInstance.Positions;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.Devices.Geolocation.Geopath.AltitudeReferenceSystem"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.AltitudeReferenceSystem AltitudeReferenceSystem
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.AltitudeReferenceSystem)(int)UwpInstance.AltitudeReferenceSystem;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.Devices.Geolocation.Geopath.GeoshapeType"/>
        /// </summary>
        public global::Windows.Devices.Geolocation.GeoshapeType GeoshapeType
        {
            get => UwpInstance.GeoshapeType;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.Devices.Geolocation.Geopath.SpatialReferenceId"/>
        /// </summary>
        public uint SpatialReferenceId
        {
            get => UwpInstance.SpatialReferenceId;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.Devices.Geolocation.Geopath"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.Geopath"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.Devices.Geolocation.Geopath"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Geopath(
            global::Windows.Devices.Geolocation.Geopath args)
        {
            return FromGeopath(args);
        }

        /// <summary>
        /// Creates a <see cref="Geopath"/> from <see cref="global::Windows.Devices.Geolocation.Geopath"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.Devices.Geolocation.Geopath"/> instance containing the event data.</param>
        /// <returns><see cref="Geopath"/></returns>
        public static Geopath FromGeopath(global::Windows.Devices.Geolocation.Geopath args)
        {
            return new Geopath(args);
        }
    }
}