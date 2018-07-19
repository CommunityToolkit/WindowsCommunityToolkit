namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.Devices.Geolocation.GeoboundingBox"/>
    /// </summary>
    public class GeoboundingBox
    {
        internal global::Windows.Devices.Geolocation.GeoboundingBox UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoboundingBox"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.Devices.Geolocation.GeoboundingBox"/>
        /// </summary>
        public GeoboundingBox(global::Windows.Devices.Geolocation.GeoboundingBox instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.Devices.Geolocation.GeoboundingBox.Center"/>
        /// </summary>
        public global::Windows.Devices.Geolocation.BasicGeoposition Center
        {
            get => UwpInstance.Center;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.Devices.Geolocation.GeoboundingBox.MaxAltitude"/>
        /// </summary>
        public double MaxAltitude
        {
            get => UwpInstance.MaxAltitude;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.Devices.Geolocation.GeoboundingBox.MinAltitude"/>
        /// </summary>
        public double MinAltitude
        {
            get => UwpInstance.MinAltitude;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.Devices.Geolocation.GeoboundingBox.NorthwestCorner"/>
        /// </summary>
        public global::Windows.Devices.Geolocation.BasicGeoposition NorthwestCorner
        {
            get => UwpInstance.NorthwestCorner;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.Devices.Geolocation.GeoboundingBox.SoutheastCorner"/>
        /// </summary>
        public global::Windows.Devices.Geolocation.BasicGeoposition SoutheastCorner
        {
            get => UwpInstance.SoutheastCorner;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.Devices.Geolocation.GeoboundingBox.AltitudeReferenceSystem"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.AltitudeReferenceSystem AltitudeReferenceSystem
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.AltitudeReferenceSystem)(int)UwpInstance.AltitudeReferenceSystem;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.Devices.Geolocation.GeoboundingBox.GeoshapeType"/>
        /// </summary>
        public global::Windows.Devices.Geolocation.GeoshapeType GeoshapeType
        {
            get => UwpInstance.GeoshapeType;
        }

        /// <summary>
        /// Gets <see cref="global::Windows.Devices.Geolocation.GeoboundingBox.SpatialReferenceId"/>
        /// </summary>
        public uint SpatialReferenceId
        {
            get => UwpInstance.SpatialReferenceId;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.Devices.Geolocation.GeoboundingBox"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.GeoboundingBox"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.Devices.Geolocation.GeoboundingBox"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator GeoboundingBox(
            global::Windows.Devices.Geolocation.GeoboundingBox args)
        {
            return FromGeoboundingBox(args);
        }

        /// <summary>
        /// Creates a <see cref="GeoboundingBox"/> from <see cref="global::Windows.Devices.Geolocation.GeoboundingBox"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.Devices.Geolocation.GeoboundingBox"/> instance containing the event data.</param>
        /// <returns><see cref="GeoboundingBox"/></returns>
        public static GeoboundingBox FromGeoboundingBox(global::Windows.Devices.Geolocation.GeoboundingBox args)
        {
            return new GeoboundingBox(args);
        }
    }
}