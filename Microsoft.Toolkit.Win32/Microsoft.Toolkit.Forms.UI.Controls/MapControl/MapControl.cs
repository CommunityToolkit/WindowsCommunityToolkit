// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Forms.UI.XamlHost;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    /// <summary>
    /// Forms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
    /// </summary>
    public class MapControl : WindowsXamlHostBase
    {
        internal Windows.UI.Xaml.Controls.Maps.MapControl UwpControl => GetUwpInternalObject() as Windows.UI.Xaml.Controls.Maps.MapControl;

        private System.Collections.Generic.Dictionary<string, object> DesignerProperties { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapControl"/> class, a
        /// Forms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        public MapControl()
            : this(typeof(Windows.UI.Xaml.Controls.Maps.MapControl).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapControl"/> class, a
        /// Forms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>.
        /// Intended for internal framework use only.
        /// </summary>
        internal MapControl(string typeName)
            : base(typeName)
        {
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (UwpControl != null)
            {
                UwpControl.CenterChanged += OnCenterChanged;
                UwpControl.HeadingChanged += OnHeadingChanged;
                UwpControl.LoadingStatusChanged += OnLoadingStatusChanged;
                UwpControl.MapDoubleTapped += OnMapDoubleTapped;
                UwpControl.MapHolding += OnMapHolding;
                UwpControl.MapTapped += OnMapTapped;
                UwpControl.PitchChanged += OnPitchChanged;
                UwpControl.TransformOriginChanged += OnTransformOriginChanged;
                UwpControl.ZoomLevelChanged += OnZoomLevelChanged;
                UwpControl.ActualCameraChanged += OnActualCameraChanged;
                UwpControl.ActualCameraChanging += OnActualCameraChanging;
                UwpControl.CustomExperienceChanged += OnCustomExperienceChanged;
                UwpControl.MapElementClick += OnMapElementClick;
                UwpControl.MapElementPointerEntered += OnMapElementPointerEntered;
                UwpControl.MapElementPointerExited += OnMapElementPointerExited;
                UwpControl.TargetCameraChanged += OnTargetCameraChanged;
                UwpControl.MapRightTapped += OnMapRightTapped;
                UwpControl.MapContextRequested += OnMapContextRequested;
                ControlAdded += MapControl_ControlAdded;
                ControlRemoved += MapControl_ControlRemoved;
            }
        }

        private void MapControl_ControlRemoved(object sender, System.Windows.Forms.ControlEventArgs e)
        {
            if (e.Control is WindowsXamlHostBase control)
            {
                UwpControl.Children.Remove(control.GetUwpInternalObject() as UIElement);
            }
        }

        private void MapControl_ControlAdded(object sender, System.Windows.Forms.ControlEventArgs e)
        {
            if (e.Control is WindowsXamlHostBase control)
            {
                UwpControl.Children.Add(control.GetUwpInternalObject() as UIElement);
            }
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TryTiltAsync"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Task<bool> TryTiltAsync(double degrees) => UwpControl.TryTiltAsync(degrees).AsTask();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TryTiltToAsync"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Task<bool> TryTiltToAsync(double angleInDegrees) => UwpControl.TryTiltToAsync(angleInDegrees).AsTask();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TryZoomInAsync"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Task<bool> TryZoomInAsync() => UwpControl.TryZoomInAsync().AsTask();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TryZoomOutAsync"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Task<bool> TryZoomOutAsync() => UwpControl.TryZoomOutAsync().AsTask();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TryZoomToAsync"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Task<bool> TryZoomToAsync(double zoomLevel) => UwpControl.TryZoomToAsync(zoomLevel).AsTask();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Task<bool> TrySetSceneAsync(MapScene scene) => UwpControl.TrySetSceneAsync(scene.UwpInstance).AsTask();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Task<bool> TrySetSceneAsync(MapScene scene, MapAnimationKind animationKind) => UwpControl.TrySetSceneAsync(scene.UwpInstance, (Windows.UI.Xaml.Controls.Maps.MapAnimationKind)animationKind).AsTask();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.GetVisibleRegion"/>
        /// </summary>
        /// <returns>Geopath</returns>
        public Geopath GetVisibleRegion(MapVisibleRegionKind region) => UwpControl.GetVisibleRegion((Windows.UI.Xaml.Controls.Maps.MapVisibleRegionKind)region);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>IReadOnlyList</returns>
        public System.Collections.Generic.IReadOnlyList<MapElement> FindMapElementsAtOffset(Point offset, double radius) => (System.Collections.Generic.IReadOnlyList<MapElement>)UwpControl.FindMapElementsAtOffset(offset.UwpInstance, radius);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        public void GetLocationFromOffset(Point offset, AltitudeReferenceSystem desiredReferenceSystem, out Windows.Devices.Geolocation.Geopoint location) => UwpControl.GetLocationFromOffset(offset.UwpInstance, (Windows.Devices.Geolocation.AltitudeReferenceSystem)desiredReferenceSystem, out location);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.StartContinuousPan"/>
        /// </summary>
        public void StartContinuousPan(double horizontalPixelsPerSecond, double verticalPixelsPerSecond) => UwpControl.StartContinuousPan(horizontalPixelsPerSecond, verticalPixelsPerSecond);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.StopContinuousPan"/>
        /// </summary>
        public void StopContinuousPan() => UwpControl.StopContinuousPan();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TryPanAsync"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Task<bool> TryPanAsync(double horizontalPixels, double verticalPixels) => UwpControl.TryPanAsync(horizontalPixels, verticalPixels).AsTask();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TryPanToAsync"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Task<bool> TryPanToAsync(Geopoint location) => UwpControl.TryPanToAsync(location.UwpInstance).AsTask();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>bool</returns>
        public bool TryGetLocationFromOffset(Point offset, out Windows.Devices.Geolocation.Geopoint location) => UwpControl.TryGetLocationFromOffset(offset.UwpInstance, out location);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>bool</returns>
        public bool TryGetLocationFromOffset(Point offset, AltitudeReferenceSystem desiredReferenceSystem, out Windows.Devices.Geolocation.Geopoint location) => UwpControl.TryGetLocationFromOffset(offset.UwpInstance, (Windows.Devices.Geolocation.AltitudeReferenceSystem)(int)desiredReferenceSystem, out location);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>bool</returns>
        public System.Collections.Generic.IReadOnlyList<MapElement> FindMapElementsAtOffset(Point offset) => (System.Collections.Generic.IReadOnlyList<MapElement>)UwpControl.FindMapElementsAtOffset(offset.UwpInstance);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        public void GetLocationFromOffset(Point offset, out Windows.Devices.Geolocation.Geopoint location) => UwpControl.GetLocationFromOffset(offset.UwpInstance, out location);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.GetOffsetFromLocation"/>
        /// </summary>
        public void GetOffsetFromLocation(Geopoint location, out Point offset)
        {
            UwpControl.GetOffsetFromLocation(location.UwpInstance, out Windows.Foundation.Point uwpOffset);
            offset = new Point(uwpOffset);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.IsLocationInView"/>
        /// </summary>
        public void IsLocationInView(Geopoint location, out bool isInView) => UwpControl.IsLocationInView(location.UwpInstance, out isInView);

        /*
        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TrySetViewBoundsAsync"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Task<bool> TrySetViewBoundsAsync(GeoboundingBox bounds, Windows.UI.Xaml.Thickness? margin, MapAnimationKind animation) => UwpControl.TrySetViewBoundsAsync(bounds, margin, animation);
        */

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Task<bool> TrySetViewAsync(Geopoint center) => UwpControl.TrySetViewAsync(center.UwpInstance).AsTask();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Task<bool> TrySetViewAsync(Geopoint center, double? zoomLevel) => UwpControl.TrySetViewAsync(center.UwpInstance, zoomLevel).AsTask();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Task<bool> TrySetViewAsync(Geopoint center, double? zoomLevel, double? heading, double? desiredPitch) => UwpControl.TrySetViewAsync(center.UwpInstance, zoomLevel, heading, desiredPitch).AsTask();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Task<bool> TrySetViewAsync(Geopoint center, double? zoomLevel, double? heading, double? desiredPitch, MapAnimationKind animation) => UwpControl.TrySetViewAsync(center.UwpInstance, zoomLevel, heading, desiredPitch, (Windows.UI.Xaml.Controls.Maps.MapAnimationKind)(int)animation).AsTask();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.StartContinuousRotate"/>
        /// </summary>
        public void StartContinuousRotate(double rateInDegreesPerSecond) => UwpControl.StartContinuousRotate(rateInDegreesPerSecond);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.StopContinuousRotate"/>
        /// </summary>
        public void StopContinuousRotate() => UwpControl.StopContinuousRotate();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.StartContinuousTilt"/>
        /// </summary>
        public void StartContinuousTilt(double rateInDegreesPerSecond) => UwpControl.StartContinuousTilt(rateInDegreesPerSecond);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.StopContinuousTilt"/>
        /// </summary>
        public void StopContinuousTilt() => UwpControl.StopContinuousTilt();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.StartContinuousZoom"/>
        /// </summary>
        public void StartContinuousZoom(double rateOfChangePerSecond) => UwpControl.StartContinuousZoom(rateOfChangePerSecond);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.StopContinuousZoom"/>
        /// </summary>
        public void StopContinuousZoom() => UwpControl.StopContinuousZoom();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TryRotateAsync"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Task<bool> TryRotateAsync(double degrees) => UwpControl.TryRotateAsync(degrees).AsTask();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TryRotateToAsync"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Task<bool> TryRotateToAsync(double angleInDegrees) => UwpControl.TryRotateToAsync(angleInDegrees).AsTask();

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.WatermarkMode"/>
        /// </summary>
        [DefaultValue(MapWatermarkMode.Automatic)]
        public MapWatermarkMode WatermarkMode
        {
            get => (MapWatermarkMode)this.GetUwpControlValue();
            set => this.SetUwpControlValue((Windows.UI.Xaml.Controls.Maps.MapWatermarkMode)value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Style"/>
        /// </summary>
        [DefaultValue(MapStyle.Road)]
        public MapStyle Style
        {
            get => UwpControl != null ? (MapStyle)UwpControl.Style : (MapStyle)this.GetUwpControlValue(); // Style property is ambiguous
            set
            {
                if (UwpControl != null)
                {
                    UwpControl.Style = (Windows.UI.Xaml.Controls.Maps.MapStyle)value;
                }
                else
                {
                    this.SetUwpControlValue((Windows.UI.Xaml.Controls.Maps.MapStyle)value);
                }
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MapServiceToken"/>
        /// </summary>
        [DefaultValue("")]
        public string MapServiceToken
        {
            get => (string)this.GetUwpControlValue(string.Empty);
            set => this.SetUwpControlValue(value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TransformOrigin"/>
        /// </summary>
        public Point TransformOrigin
        {
            get => (Point)this.GetUwpControlValue(new Point(0.5f, 0.5f));
            set => this.SetUwpControlValue(value.UwpInstance);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TrafficFlowVisible"/>
        /// </summary>
        [DefaultValue(false)]
        public bool TrafficFlowVisible
        {
            get => (bool)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.LandmarksVisible"/>
        /// </summary>
        [DefaultValue(true)]
        public bool LandmarksVisible
        {
            get => (bool)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Heading"/>
        /// </summary>
        [DefaultValue((double)0)]
        public double Heading
        {
            get => (double)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.DesiredPitch"/>
        /// </summary>
        [DefaultValue((double)0)]
        public double DesiredPitch
        {
            get => (double)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.ColorScheme"/>
        /// </summary>
        [DefaultValue(MapColorScheme.Light)]
        public MapColorScheme ColorScheme
        {
            get => (MapColorScheme)this.GetUwpControlValue();
            set => this.SetUwpControlValue((Windows.UI.Xaml.Controls.Maps.MapColorScheme)value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.PedestrianFeaturesVisible"/>
        /// </summary>
        [DefaultValue(false)]
        public bool PedestrianFeaturesVisible
        {
            get => (bool)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.ZoomLevel"/>
        /// </summary>
        [DefaultValue((double)2)]
        public double ZoomLevel
        {
            get => (double)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Center"/>
        /// </summary>
        public Geopoint Center
        {
            get => (Geopoint)this.GetUwpControlValue(new Geopoint(new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition() { Latitude = 0, Longitude = 23.383333 })));
            set => this.SetUwpControlValue(value.UwpInstance);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.LoadingStatus"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MapLoadingStatus LoadingStatus
        {
            get => (MapLoadingStatus)UwpControl.LoadingStatus;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MapElements"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Collections.Generic.IList<MapElement> MapElements
        {
            get => UwpControl.MapElements.Cast<MapElement>().ToList();
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MaxZoomLevel"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double MaxZoomLevel
        {
            get => UwpControl.MaxZoomLevel;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MinZoomLevel"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double MinZoomLevel
        {
            get => UwpControl.MinZoomLevel;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Pitch"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Pitch
        {
            get => UwpControl.Pitch;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Routes"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Collections.Generic.IList<Windows.UI.Xaml.Controls.Maps.MapRouteView> Routes
        {
            get => UwpControl.Routes;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TileSources"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Collections.Generic.IList<Windows.UI.Xaml.Controls.Maps.MapTileSource> TileSources
        {
            get => UwpControl.TileSources;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.ZoomInteractionMode"/>
        /// </summary>
        [DefaultValue(MapInteractionMode.Auto)]
        public MapInteractionMode ZoomInteractionMode
        {
            get => (MapInteractionMode)this.GetUwpControlValue();
            set => this.SetUwpControlValue((Windows.UI.Xaml.Controls.Maps.MapInteractionMode)value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TransitFeaturesVisible"/>
        /// </summary>
        [DefaultValue(true)]
        public bool TransitFeaturesVisible
        {
            get => (bool)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TiltInteractionMode"/>
        /// </summary>
        [DefaultValue(MapInteractionMode.Auto)]
        public MapInteractionMode TiltInteractionMode
        {
            get => (MapInteractionMode)this.GetUwpControlValue();
            set => this.SetUwpControlValue((Windows.UI.Xaml.Controls.Maps.MapInteractionMode)value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Scene"/>
        /// </summary>
        [DefaultValue(null)]
        public MapScene Scene
        {
            get => (MapScene)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value.UwpInstance);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.RotateInteractionMode"/>
        /// </summary>
        [DefaultValue(MapInteractionMode.Auto)]
        public MapInteractionMode RotateInteractionMode
        {
            get => (MapInteractionMode)this.GetUwpControlValue();
            set => this.SetUwpControlValue((Windows.UI.Xaml.Controls.Maps.MapInteractionMode)value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.PanInteractionMode"/>
        /// </summary>
        [DefaultValue(MapPanInteractionMode.Auto)]
        public MapPanInteractionMode PanInteractionMode
        {
            get => (MapPanInteractionMode)this.GetUwpControlValue();
            set => this.SetUwpControlValue((Windows.UI.Xaml.Controls.Maps.MapPanInteractionMode)value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.CustomExperience"/>
        /// </summary>
        [DefaultValue(null)]
        public MapCustomExperience CustomExperience
        {
            get => (MapCustomExperience)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value.UwpInstance);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.BusinessLandmarksVisible"/>
        /// </summary>
        [DefaultValue(true)]
        public bool BusinessLandmarksVisible
        {
            get => (bool)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.ActualCamera"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MapCamera ActualCamera
        {
            get => UwpControl.ActualCamera;
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Is3DSupported"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Is3DSupported
        {
            get => UwpControl.Is3DSupported;
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.IsStreetsideSupported"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsStreetsideSupported
        {
            get => UwpControl.IsStreetsideSupported;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TargetCamera"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MapCamera TargetCamera
        {
            get => UwpControl.TargetCamera;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TransitFeaturesEnabled"/>
        /// </summary>
        [DefaultValue(false)]
        public bool TransitFeaturesEnabled
        {
            get => (bool)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.BusinessLandmarksEnabled"/>
        /// </summary>
        [DefaultValue(false)]
        public bool BusinessLandmarksEnabled
        {
            get => (bool)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value);
        }

        /*
        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.ViewPadding"/>
        /// </summary>
        public Windows.UI.Xaml.Thickness ViewPadding
        {
            get => (Windows.UI.Xaml.Thickness)GetValue(ViewPaddingProperty);
            set => UwpControl.ViewPadding = value;
        }
        */

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.StyleSheet"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MapStyleSheet StyleSheet
        {
            get => (MapStyleSheet)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value.UwpInstance);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MapProjection"/>
        /// </summary>
        [DefaultValue(MapProjection.WebMercator)]
        public MapProjection MapProjection
        {
            get => (MapProjection)this.GetUwpControlValue();
            set => this.SetUwpControlValue((Windows.UI.Xaml.Controls.Maps.MapProjection)value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Layers"/>
        /// </summary>
        [DefaultValue(null)]
        public System.Collections.Generic.IList<Windows.UI.Xaml.Controls.Maps.MapLayer> Layers
        {
            get => (System.Collections.Generic.IList<Windows.UI.Xaml.Controls.Maps.MapLayer>)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Region"/>
        /// </summary>
        [DefaultValue("")]
        public new string Region
        {
            get => (string)this.GetUwpControlValue(string.Empty);
            set => this.SetUwpControlValue(value);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.CenterChanged"/>
        /// </summary>
        public event EventHandler<object> CenterChanged = (sender, args) => { };

        private void OnCenterChanged(Windows.UI.Xaml.Controls.Maps.MapControl sender, object args)
        {
            this.CenterChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.HeadingChanged"/>
        /// </summary>
        public event EventHandler<object> HeadingChanged = (sender, args) => { };

        private void OnHeadingChanged(Windows.UI.Xaml.Controls.Maps.MapControl sender, object args)
        {
            this.HeadingChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.LoadingStatusChanged"/>
        /// </summary>
        public event EventHandler<object> LoadingStatusChanged = (sender, args) => { };

        private void OnLoadingStatusChanged(Windows.UI.Xaml.Controls.Maps.MapControl sender, object args)
        {
            this.LoadingStatusChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MapDoubleTapped"/>
        /// </summary>
        public event EventHandler<MapInputEventArgs> MapDoubleTapped = (sender, args) => { };

        private void OnMapDoubleTapped(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            this.MapDoubleTapped?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MapHolding"/>
        /// </summary>
        public event EventHandler<MapInputEventArgs> MapHolding = (sender, args) => { };

        private void OnMapHolding(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            this.MapHolding?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MapTapped"/>
        /// </summary>
        public event EventHandler<MapInputEventArgs> MapTapped = (sender, args) => { };

        private void OnMapTapped(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            this.MapTapped?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.PitchChanged"/>
        /// </summary>
        public event EventHandler<object> PitchChanged = (sender, args) => { };

        private void OnPitchChanged(Windows.UI.Xaml.Controls.Maps.MapControl sender, object args)
        {
            this.PitchChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TransformOriginChanged"/>
        /// </summary>
        public event EventHandler<object> TransformOriginChanged = (sender, args) => { };

        private void OnTransformOriginChanged(Windows.UI.Xaml.Controls.Maps.MapControl sender, object args)
        {
            this.TransformOriginChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.ZoomLevelChanged"/>
        /// </summary>
        public event EventHandler<object> ZoomLevelChanged = (sender, args) => { };

        private void OnZoomLevelChanged(Windows.UI.Xaml.Controls.Maps.MapControl sender, object args)
        {
            this.ZoomLevelChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.ActualCameraChanged"/>
        /// </summary>
        public event EventHandler<MapActualCameraChangedEventArgs> ActualCameraChanged = (sender, args) => { };

        private void OnActualCameraChanged(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs args)
        {
            this.ActualCameraChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.ActualCameraChanging"/>
        /// </summary>
        public event EventHandler<MapActualCameraChangingEventArgs> ActualCameraChanging = (sender, args) => { };

        private void OnActualCameraChanging(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapActualCameraChangingEventArgs args)
        {
            this.ActualCameraChanging?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.CustomExperienceChanged"/>
        /// </summary>
        public event EventHandler<MapCustomExperienceChangedEventArgs> CustomExperienceChanged = (sender, args) => { };

        private void OnCustomExperienceChanged(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapCustomExperienceChangedEventArgs args)
        {
            this.CustomExperienceChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MapElementClick"/>
        /// </summary>
        public event EventHandler<MapElementClickEventArgs> MapElementClick = (sender, args) => { };

        private void OnMapElementClick(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs args)
        {
            this.MapElementClick?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MapElementPointerEntered"/>
        /// </summary>
        public event EventHandler<MapElementPointerEnteredEventArgs> MapElementPointerEntered = (sender, args) => { };

        private void OnMapElementPointerEntered(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs args)
        {
            this.MapElementPointerEntered?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MapElementPointerExited"/>
        /// </summary>
        public event EventHandler<MapElementPointerExitedEventArgs> MapElementPointerExited = (sender, args) => { };

        private void OnMapElementPointerExited(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapElementPointerExitedEventArgs args)
        {
            this.MapElementPointerExited?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TargetCameraChanged"/>
        /// </summary>
        public event EventHandler<MapTargetCameraChangedEventArgs> TargetCameraChanged = (sender, args) => { };

        private void OnTargetCameraChanged(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs args)
        {
            this.TargetCameraChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MapRightTapped"/>
        /// </summary>
        public event EventHandler<MapRightTappedEventArgs> MapRightTapped = (sender, args) => { };

        private void OnMapRightTapped(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs args)
        {
            this.MapRightTapped?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MapContextRequested"/>
        /// </summary>
        public event EventHandler<MapContextRequestedEventArgs> MapContextRequested = (sender, args) => { };

        private void OnMapContextRequested(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs args)
        {
            this.MapContextRequested?.Invoke(this, args);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                UwpControl.CenterChanged -= OnCenterChanged;
                UwpControl.HeadingChanged -= OnHeadingChanged;
                UwpControl.LoadingStatusChanged -= OnLoadingStatusChanged;
                UwpControl.MapDoubleTapped -= OnMapDoubleTapped;
                UwpControl.MapHolding -= OnMapHolding;
                UwpControl.MapTapped -= OnMapTapped;
                UwpControl.PitchChanged -= OnPitchChanged;
                UwpControl.TransformOriginChanged -= OnTransformOriginChanged;
                UwpControl.ZoomLevelChanged -= OnZoomLevelChanged;
                UwpControl.ActualCameraChanged -= OnActualCameraChanged;
                UwpControl.ActualCameraChanging -= OnActualCameraChanging;
                UwpControl.CustomExperienceChanged -= OnCustomExperienceChanged;
                UwpControl.MapElementClick -= OnMapElementClick;
                UwpControl.MapElementPointerEntered -= OnMapElementPointerEntered;
                UwpControl.MapElementPointerExited -= OnMapElementPointerExited;
                UwpControl.TargetCameraChanged -= OnTargetCameraChanged;
                UwpControl.MapRightTapped -= OnMapRightTapped;
                UwpControl.MapContextRequested -= OnMapContextRequested;
                ControlAdded -= MapControl_ControlAdded;
                ControlRemoved -= MapControl_ControlRemoved;
            }
        }
    }
}