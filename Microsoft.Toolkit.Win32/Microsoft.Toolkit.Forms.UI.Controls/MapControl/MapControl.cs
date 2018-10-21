// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
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
        internal Windows.UI.Xaml.Controls.Maps.MapControl UwpControl => this.UwpControl as Windows.UI.Xaml.Controls.Maps.MapControl;

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
        public MapControl(string typeName)
            : base(typeName)
        {
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
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
            ControlAdded += InkToolbar_ControlAdded;
            ControlRemoved += InkToolbar_ControlRemoved;
        }

        private void InkToolbar_ControlRemoved(object sender, System.Windows.Forms.ControlEventArgs e)
        {
            if (e.Control is WindowsXamlHostBase control)
            {
                UwpControl.Children.Remove(control.GetUwpInternalObject() as UIElement);
            }
        }

        private void InkToolbar_ControlAdded(object sender, System.Windows.Forms.ControlEventArgs e)
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
        public Windows.Foundation.IAsyncOperation<bool> TryTiltAsync(double degrees) => UwpControl.TryTiltAsync(degrees);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TryTiltToAsync"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Windows.Foundation.IAsyncOperation<bool> TryTiltToAsync(double angleInDegrees) => UwpControl.TryTiltToAsync(angleInDegrees);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TryZoomInAsync"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Windows.Foundation.IAsyncOperation<bool> TryZoomInAsync() => UwpControl.TryZoomInAsync();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TryZoomOutAsync"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Windows.Foundation.IAsyncOperation<bool> TryZoomOutAsync() => UwpControl.TryZoomOutAsync();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TryZoomToAsync"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Windows.Foundation.IAsyncOperation<bool> TryZoomToAsync(double zoomLevel) => UwpControl.TryZoomToAsync(zoomLevel);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Windows.Foundation.IAsyncOperation<bool> TrySetSceneAsync(MapScene scene) => UwpControl.TrySetSceneAsync(scene.UwpInstance);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Windows.Foundation.IAsyncOperation<bool> TrySetSceneAsync(MapScene scene, MapAnimationKind animationKind) => UwpControl.TrySetSceneAsync(scene.UwpInstance, (Windows.UI.Xaml.Controls.Maps.MapAnimationKind)animationKind);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.GetVisibleRegion"/>
        /// </summary>
        /// <returns>Geopath</returns>
        public Geopath GetVisibleRegion(MapVisibleRegionKind region) => UwpControl.GetVisibleRegion((Windows.UI.Xaml.Controls.Maps.MapVisibleRegionKind)region);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>IReadOnlyList</returns>
        public System.Collections.Generic.IReadOnlyList<MapElement> FindMapElementsAtOffset(Windows.Foundation.Point offset, double radius) => (System.Collections.Generic.IReadOnlyList<MapElement>)UwpControl.FindMapElementsAtOffset(offset, radius);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        public void GetLocationFromOffset(Windows.Foundation.Point offset, AltitudeReferenceSystem desiredReferenceSystem, out Windows.Devices.Geolocation.Geopoint location) => UwpControl.GetLocationFromOffset(offset, (Windows.Devices.Geolocation.AltitudeReferenceSystem)desiredReferenceSystem, out location);

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
        public Windows.Foundation.IAsyncOperation<bool> TryPanAsync(double horizontalPixels, double verticalPixels) => UwpControl.TryPanAsync(horizontalPixels, verticalPixels);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TryPanToAsync"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Windows.Foundation.IAsyncOperation<bool> TryPanToAsync(Geopoint location) => UwpControl.TryPanToAsync(location.UwpInstance);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>bool</returns>
        public bool TryGetLocationFromOffset(Windows.Foundation.Point offset, out Windows.Devices.Geolocation.Geopoint location) => UwpControl.TryGetLocationFromOffset(offset, out location);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>bool</returns>
        public bool TryGetLocationFromOffset(Windows.Foundation.Point offset, AltitudeReferenceSystem desiredReferenceSystem, out Windows.Devices.Geolocation.Geopoint location) => UwpControl.TryGetLocationFromOffset(offset, (Windows.Devices.Geolocation.AltitudeReferenceSystem)(int)desiredReferenceSystem, out location);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>bool</returns>
        public System.Collections.Generic.IReadOnlyList<MapElement> FindMapElementsAtOffset(Windows.Foundation.Point offset) => (System.Collections.Generic.IReadOnlyList<MapElement>)UwpControl.FindMapElementsAtOffset(offset);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        public void GetLocationFromOffset(Windows.Foundation.Point offset, out Windows.Devices.Geolocation.Geopoint location) => UwpControl.GetLocationFromOffset(offset, out location);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.GetOffsetFromLocation"/>
        /// </summary>
        public void GetOffsetFromLocation(Geopoint location, out Windows.Foundation.Point offset) => UwpControl.GetOffsetFromLocation(location.UwpInstance, out offset);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.IsLocationInView"/>
        /// </summary>
        public void IsLocationInView(Geopoint location, out bool isInView) => UwpControl.IsLocationInView(location.UwpInstance, out isInView);

        /*
        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TrySetViewBoundsAsync"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Windows.Foundation.IAsyncOperation<bool> TrySetViewBoundsAsync(GeoboundingBox bounds, Windows.UI.Xaml.Thickness? margin, MapAnimationKind animation) => UwpControl.TrySetViewBoundsAsync(bounds, margin, animation);
        */

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Windows.Foundation.IAsyncOperation<bool> TrySetViewAsync(Geopoint center) => UwpControl.TrySetViewAsync(center.UwpInstance);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Windows.Foundation.IAsyncOperation<bool> TrySetViewAsync(Geopoint center, double? zoomLevel) => UwpControl.TrySetViewAsync(center.UwpInstance, zoomLevel);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Windows.Foundation.IAsyncOperation<bool> TrySetViewAsync(Geopoint center, double? zoomLevel, double? heading, double? desiredPitch) => UwpControl.TrySetViewAsync(center.UwpInstance, zoomLevel, heading, desiredPitch);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Windows.Foundation.IAsyncOperation<bool> TrySetViewAsync(Geopoint center, double? zoomLevel, double? heading, double? desiredPitch, MapAnimationKind animation) => UwpControl.TrySetViewAsync(center.UwpInstance, zoomLevel, heading, desiredPitch, (Windows.UI.Xaml.Controls.Maps.MapAnimationKind)(int)animation);

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
        public Windows.Foundation.IAsyncOperation<bool> TryRotateAsync(double degrees) => UwpControl.TryRotateAsync(degrees);

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TryRotateToAsync"/>
        /// </summary>
        /// <returns>IAsyncOperation</returns>
        public Windows.Foundation.IAsyncOperation<bool> TryRotateToAsync(double angleInDegrees) => UwpControl.TryRotateToAsync(angleInDegrees);

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.WatermarkMode"/>
        /// </summary>
        public MapWatermarkMode WatermarkMode
        {
            get => (MapWatermarkMode)UwpControl.WatermarkMode;
            set => UwpControl.WatermarkMode = (Windows.UI.Xaml.Controls.Maps.MapWatermarkMode)value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Style"/>
        /// </summary>
        public MapStyle Style
        {
            get => (MapStyle)UwpControl.Style;
            set => UwpControl.Style = (Windows.UI.Xaml.Controls.Maps.MapStyle)value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MapServiceToken"/>
        /// </summary>
        public string MapServiceToken
        {
            get => UwpControl.MapServiceToken;
            set => UwpControl.MapServiceToken = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TransformOrigin"/>
        /// </summary>
        public Windows.Foundation.Point TransformOrigin
        {
            get => UwpControl.TransformOrigin;
            set => UwpControl.TransformOrigin = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TrafficFlowVisible"/>
        /// </summary>
        public bool TrafficFlowVisible
        {
            get => UwpControl.TrafficFlowVisible;
            set => UwpControl.TrafficFlowVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.LandmarksVisible"/>
        /// </summary>
        public bool LandmarksVisible
        {
            get => UwpControl.LandmarksVisible;
            set => UwpControl.LandmarksVisible = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Heading"/>
        /// </summary>
        public double Heading
        {
            get => UwpControl.Heading;
            set => UwpControl.Heading = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.DesiredPitch"/>
        /// </summary>
        public double DesiredPitch
        {
            get => UwpControl.DesiredPitch;
            set => UwpControl.DesiredPitch = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.ColorScheme"/>
        /// </summary>
        public MapColorScheme ColorScheme
        {
            get => (MapColorScheme)UwpControl.ColorScheme;
            set => UwpControl.ColorScheme = (Windows.UI.Xaml.Controls.Maps.MapColorScheme)value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.PedestrianFeaturesVisible"/>
        /// </summary>
        public bool PedestrianFeaturesVisible
        {
            get => UwpControl.PedestrianFeaturesVisible;
            set => UwpControl.PedestrianFeaturesVisible = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.ZoomLevel"/>
        /// </summary>
        public double ZoomLevel
        {
            get => UwpControl.ZoomLevel;
            set => UwpControl.ZoomLevel = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Center"/>
        /// </summary>
        public Geopoint Center
        {
            get => UwpControl.Center;
            set => UwpControl.Center = value.UwpInstance;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.LoadingStatus"/>
        /// </summary>
        public MapLoadingStatus LoadingStatus
        {
            get => (MapLoadingStatus)UwpControl.LoadingStatus;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MapElements"/>
        /// </summary>
        public System.Collections.Generic.IList<MapElement> MapElements
        {
            get => UwpControl.MapElements.Cast<MapElement>().ToList();
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MaxZoomLevel"/>
        /// </summary>
        public double MaxZoomLevel
        {
            get => UwpControl.MaxZoomLevel;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MinZoomLevel"/>
        /// </summary>
        public double MinZoomLevel
        {
            get => UwpControl.MinZoomLevel;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Pitch"/>
        /// </summary>
        public double Pitch
        {
            get => UwpControl.Pitch;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Routes"/>
        /// </summary>
        public System.Collections.Generic.IList<Windows.UI.Xaml.Controls.Maps.MapRouteView> Routes
        {
            get => UwpControl.Routes;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TileSources"/>
        /// </summary>
        public System.Collections.Generic.IList<Windows.UI.Xaml.Controls.Maps.MapTileSource> TileSources
        {
            get => UwpControl.TileSources;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.ZoomInteractionMode"/>
        /// </summary>
        public MapInteractionMode ZoomInteractionMode
        {
            get => (MapInteractionMode)UwpControl.ZoomInteractionMode;
            set => UwpControl.ZoomInteractionMode = (Windows.UI.Xaml.Controls.Maps.MapInteractionMode)value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TransitFeaturesVisible"/>
        /// </summary>
        public bool TransitFeaturesVisible
        {
            get => UwpControl.TransitFeaturesVisible;
            set => UwpControl.TransitFeaturesVisible = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TiltInteractionMode"/>
        /// </summary>
        public MapInteractionMode TiltInteractionMode
        {
            get => (MapInteractionMode)UwpControl.TiltInteractionMode;
            set => UwpControl.TiltInteractionMode = (Windows.UI.Xaml.Controls.Maps.MapInteractionMode)value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Scene"/>
        /// </summary>
        public MapScene Scene
        {
            get => UwpControl.Scene;
            set => UwpControl.Scene = value.UwpInstance;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.RotateInteractionMode"/>
        /// </summary>
        public MapInteractionMode RotateInteractionMode
        {
            get => (MapInteractionMode)UwpControl.RotateInteractionMode;
            set => UwpControl.RotateInteractionMode = (Windows.UI.Xaml.Controls.Maps.MapInteractionMode)value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.PanInteractionMode"/>
        /// </summary>
        public MapPanInteractionMode PanInteractionMode
        {
            get => (MapPanInteractionMode)UwpControl.PanInteractionMode;
            set => UwpControl.PanInteractionMode = (Windows.UI.Xaml.Controls.Maps.MapPanInteractionMode)value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.CustomExperience"/>
        /// </summary>
        public MapCustomExperience CustomExperience
        {
            get => UwpControl.CustomExperience;
            set => UwpControl.CustomExperience = value.UwpInstance;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.BusinessLandmarksVisible"/>
        /// </summary>
        public bool BusinessLandmarksVisible
        {
            get => UwpControl.BusinessLandmarksVisible;
            set => UwpControl.BusinessLandmarksVisible = value;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.ActualCamera"/>
        /// </summary>
        public MapCamera ActualCamera
        {
            get => UwpControl.ActualCamera;
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Is3DSupported"/>
        /// </summary>
        public bool Is3DSupported
        {
            get => UwpControl.Is3DSupported;
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.IsStreetsideSupported"/>
        /// </summary>
        public bool IsStreetsideSupported
        {
            get => UwpControl.IsStreetsideSupported;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TargetCamera"/>
        /// </summary>
        public MapCamera TargetCamera
        {
            get => UwpControl.TargetCamera;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.TransitFeaturesEnabled"/>
        /// </summary>
        public bool TransitFeaturesEnabled
        {
            get => UwpControl.TransitFeaturesEnabled;
            set => UwpControl.TransitFeaturesEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.BusinessLandmarksEnabled"/>
        /// </summary>
        public bool BusinessLandmarksEnabled
        {
            get => UwpControl.BusinessLandmarksEnabled;
            set => UwpControl.BusinessLandmarksEnabled = value;
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
        public MapStyleSheet StyleSheet
        {
            get => UwpControl.StyleSheet;
            set => UwpControl.StyleSheet = value.UwpInstance;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.MapProjection"/>
        /// </summary>
        public MapProjection MapProjection
        {
            get => (MapProjection)UwpControl.MapProjection;
            set => UwpControl.MapProjection = (Windows.UI.Xaml.Controls.Maps.MapProjection)value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Layers"/>
        /// </summary>
        public System.Collections.Generic.IList<Windows.UI.Xaml.Controls.Maps.MapLayer> Layers
        {
            get => UwpControl.Layers;
            set => UwpControl.Layers = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.Maps.MapControl.Region"/>
        /// </summary>
        public new string Region
        {
            get => UwpControl.Region;
            set => UwpControl.Region = value;
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
                ControlAdded -= InkToolbar_ControlAdded;
                ControlRemoved -= InkToolbar_ControlRemoved;
            }
        }
    }
}