using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using Microsoft.Windows.Interop;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    [ContentProperty(nameof(Children))]
    public class MapControl : WindowsXamlHost
    {
        protected global::Windows.UI.Xaml.Controls.Maps.MapControl UwpControl => this.XamlRoot as global::Windows.UI.Xaml.Controls.Maps.MapControl;

        public MapControl()
            : this(typeof(global::Windows.UI.Xaml.Controls.Maps.MapControl).FullName)
        {
        }

        // Summary:
        //     Initializes a new instance of the MapControl class.
        public MapControl(string typeName)
            : base(typeName)
        {
            Children = new List<DependencyObject>();
        }

        protected override void OnInitialized(EventArgs e)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.NameProperty);
            Bind(nameof(Tag), TagProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.DataContextProperty);
            Bind(nameof(Width), WidthProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.WidthProperty);

            // MapControl specific properties
            Bind(nameof(WatermarkMode), WatermarkModeProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.WatermarkModeProperty);
            Bind(nameof(Style), StyleProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.StyleProperty);
            Bind(nameof(MapServiceToken), MapServiceTokenProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.MapServiceTokenProperty);
            Bind(nameof(TransformOrigin), TransformOriginProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.TransformOriginProperty);
            Bind(nameof(TrafficFlowVisible), TrafficFlowVisibleProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.TrafficFlowVisibleProperty);
            Bind(nameof(LandmarksVisible), LandmarksVisibleProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.LandmarksVisibleProperty);
            Bind(nameof(Heading), HeadingProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.HeadingProperty);
            Bind(nameof(DesiredPitch), DesiredPitchProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.DesiredPitchProperty);
            Bind(nameof(ColorScheme), ColorSchemeProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.ColorSchemeProperty);
            Bind(nameof(PedestrianFeaturesVisible), PedestrianFeaturesVisibleProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.PedestrianFeaturesVisibleProperty);
            Bind(nameof(ZoomLevel), ZoomLevelProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.ZoomLevelProperty);
            Bind(nameof(Center), CenterProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.CenterProperty);
            Bind(nameof(LoadingStatus), LoadingStatusProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.LoadingStatusProperty);
            Bind(nameof(MapElements), MapElementsProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.MapElementsProperty);
            Bind(nameof(Pitch), PitchProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.PitchProperty);
            Bind(nameof(Routes), RoutesProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.RoutesProperty);
            Bind(nameof(TileSources), TileSourcesProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.TileSourcesProperty);
            Bind(nameof(ZoomInteractionMode), ZoomInteractionModeProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.ZoomInteractionModeProperty);
            Bind(nameof(TransitFeaturesVisible), TransitFeaturesVisibleProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.TransitFeaturesVisibleProperty);
            Bind(nameof(TiltInteractionMode), TiltInteractionModeProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.TiltInteractionModeProperty);
            Bind(nameof(Scene), SceneProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.SceneProperty);
            Bind(nameof(RotateInteractionMode), RotateInteractionModeProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.RotateInteractionModeProperty);
            Bind(nameof(PanInteractionMode), PanInteractionModeProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.PanInteractionModeProperty);
            Bind(nameof(BusinessLandmarksVisible), BusinessLandmarksVisibleProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.BusinessLandmarksVisibleProperty);
            Bind(nameof(Is3DSupported), Is3DSupportedProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.Is3DSupportedProperty);
            Bind(nameof(IsStreetsideSupported), IsStreetsideSupportedProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.IsStreetsideSupportedProperty);
            Bind(nameof(TransitFeaturesEnabled), TransitFeaturesEnabledProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.TransitFeaturesEnabledProperty);
            Bind(nameof(BusinessLandmarksEnabled), BusinessLandmarksEnabledProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.BusinessLandmarksEnabledProperty);
            Bind(nameof(StyleSheet), StyleSheetProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.StyleSheetProperty);
            Bind(nameof(MapProjection), MapProjectionProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.MapProjectionProperty);
            Bind(nameof(Layers), LayersProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.LayersProperty);
            Bind(nameof(Region), RegionProperty, global::Windows.UI.Xaml.Controls.Maps.MapControl.RegionProperty);

            Children.OfType<WindowsXamlHost>().ToList().ForEach(RelocateChildToUwpControl);
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

            base.OnInitialized(e);
        }

        private void RelocateChildToUwpControl(WindowsXamlHost obj)
        {
            VisualTreeHelper.DisconnectChildrenRecursive(obj.desktopWindowXamlSource.Content);
            obj.desktopWindowXamlSource.Content = null;
            Children.Remove(obj);
            UwpControl.Children.Add(obj.XamlRoot);
        }

        public static DependencyProperty ColorSchemeProperty { get; } = DependencyProperty.Register(nameof(ColorScheme), typeof(global::Windows.UI.Xaml.Controls.Maps.MapColorScheme), typeof(MapControl));

        public static DependencyProperty HeadingProperty { get; } = DependencyProperty.Register(nameof(Heading), typeof(double), typeof(MapControl));

        public static DependencyProperty LandmarksVisibleProperty { get; } = DependencyProperty.Register(nameof(LandmarksVisible), typeof(bool), typeof(MapControl));

        public static DependencyProperty LoadingStatusProperty { get; } = DependencyProperty.Register(nameof(LoadingStatus), typeof(global::Windows.UI.Xaml.Controls.Maps.MapLoadingStatus), typeof(MapControl));

        public static DependencyProperty LocationProperty { get; } = DependencyProperty.Register(nameof(Center), typeof(global::Windows.Devices.Geolocation.Geopoint), typeof(MapControl));

        public static DependencyProperty MapElementsProperty { get; } = DependencyProperty.Register(nameof(MapElements), typeof(System.Collections.Generic.IList<global::Windows.UI.Xaml.Controls.Maps.MapElement>), typeof(MapControl));

        public static DependencyProperty MapServiceTokenProperty { get; } = DependencyProperty.Register(nameof(MapServiceToken), typeof(string), typeof(MapControl));

        public static DependencyProperty NormalizedAnchorPointProperty { get; } = DependencyProperty.Register(nameof(Center), typeof(global::Windows.Devices.Geolocation.Geopoint), typeof(MapControl));

        public static DependencyProperty PedestrianFeaturesVisibleProperty { get; } = DependencyProperty.Register(nameof(PedestrianFeaturesVisible), typeof(bool), typeof(MapControl));

        public static DependencyProperty PitchProperty { get; } = DependencyProperty.Register(nameof(Pitch), typeof(double), typeof(MapControl));

        public static DependencyProperty RoutesProperty { get; } = DependencyProperty.Register(nameof(Routes), typeof(System.Collections.Generic.IList<global::Windows.UI.Xaml.Controls.Maps.MapRouteView>), typeof(MapControl));

        public static DependencyProperty TileSourcesProperty { get; } = DependencyProperty.Register(nameof(TileSources), typeof(System.Collections.Generic.IList<global::Windows.UI.Xaml.Controls.Maps.MapTileSource>), typeof(MapControl));

        public static DependencyProperty TrafficFlowVisibleProperty { get; } = DependencyProperty.Register(nameof(TrafficFlowVisible), typeof(bool), typeof(MapControl));

        public static DependencyProperty TransformOriginProperty { get; } = DependencyProperty.Register(nameof(TransformOrigin), typeof(global::Windows.Foundation.Point), typeof(MapControl));

        public static DependencyProperty WatermarkModeProperty { get; } = DependencyProperty.Register(nameof(WatermarkMode), typeof(global::Windows.UI.Xaml.Controls.Maps.MapWatermarkMode), typeof(MapControl));

        public static DependencyProperty CenterProperty { get; } = DependencyProperty.Register(nameof(Center), typeof(global::Windows.Devices.Geolocation.Geopoint), typeof(MapControl));

        public static DependencyProperty ZoomLevelProperty { get; } = DependencyProperty.Register(nameof(ZoomLevel), typeof(double), typeof(MapControl));

        public static DependencyProperty DesiredPitchProperty { get; } = DependencyProperty.Register(nameof(DesiredPitch), typeof(double), typeof(MapControl));

        public static DependencyProperty BusinessLandmarksVisibleProperty { get; } = DependencyProperty.Register(nameof(BusinessLandmarksVisible), typeof(bool), typeof(MapControl));

        public static DependencyProperty Is3DSupportedProperty { get; } = DependencyProperty.Register(nameof(Is3DSupported), typeof(bool), typeof(MapControl));

        public static DependencyProperty IsStreetsideSupportedProperty { get; } = DependencyProperty.Register(nameof(IsStreetsideSupported), typeof(bool), typeof(MapControl));

        public static DependencyProperty PanInteractionModeProperty { get; } = DependencyProperty.Register(nameof(PanInteractionMode), typeof(global::Windows.UI.Xaml.Controls.Maps.MapPanInteractionMode), typeof(MapControl));

        public static DependencyProperty RotateInteractionModeProperty { get; } = DependencyProperty.Register(nameof(RotateInteractionMode), typeof(global::Windows.UI.Xaml.Controls.Maps.MapInteractionMode), typeof(MapControl));

        public static DependencyProperty SceneProperty { get; } = DependencyProperty.Register(nameof(Scene), typeof(global::Windows.UI.Xaml.Controls.Maps.MapScene), typeof(MapControl));

        public static DependencyProperty TiltInteractionModeProperty { get; } = DependencyProperty.Register(nameof(TiltInteractionMode), typeof(global::Windows.UI.Xaml.Controls.Maps.MapInteractionMode), typeof(MapControl));

        public static DependencyProperty TransitFeaturesVisibleProperty { get; } = DependencyProperty.Register(nameof(TransitFeaturesVisible), typeof(bool), typeof(MapControl));

        public static DependencyProperty ZoomInteractionModeProperty { get; } = DependencyProperty.Register(nameof(ZoomInteractionMode), typeof(global::Windows.UI.Xaml.Controls.Maps.MapInteractionMode), typeof(MapControl));

        public static DependencyProperty BusinessLandmarksEnabledProperty { get; } = DependencyProperty.Register(nameof(BusinessLandmarksEnabled), typeof(bool), typeof(MapControl));

        public static DependencyProperty TransitFeaturesEnabledProperty { get; } = DependencyProperty.Register(nameof(TransitFeaturesEnabled), typeof(bool), typeof(MapControl));

        public static DependencyProperty MapProjectionProperty { get; } = DependencyProperty.Register(nameof(MapProjection), typeof(global::Windows.UI.Xaml.Controls.Maps.MapProjection), typeof(MapControl));

        public static DependencyProperty StyleSheetProperty { get; } = DependencyProperty.Register(nameof(StyleSheet), typeof(global::Windows.UI.Xaml.Controls.Maps.MapStyleSheet), typeof(MapControl));

        public static DependencyProperty LayersProperty { get; } = DependencyProperty.Register(nameof(Layers), typeof(System.Collections.Generic.IList<global::Windows.UI.Xaml.Controls.Maps.MapLayer>), typeof(MapControl));

        public static DependencyProperty RegionProperty { get; } = DependencyProperty.Register(nameof(Region), typeof(string), typeof(MapControl));

        public global::Windows.Foundation.IAsyncOperation<bool> TryTiltAsync(double degrees) => UwpControl.TryTiltAsync(degrees);

        public global::Windows.Foundation.IAsyncOperation<bool> TryTiltToAsync(double angleInDegrees) => UwpControl.TryTiltToAsync(angleInDegrees);

        public global::Windows.Foundation.IAsyncOperation<bool> TryZoomInAsync() => UwpControl.TryZoomInAsync();

        public global::Windows.Foundation.IAsyncOperation<bool> TryZoomOutAsync() => UwpControl.TryZoomOutAsync();

        public global::Windows.Foundation.IAsyncOperation<bool> TryZoomToAsync(double zoomLevel) => UwpControl.TryZoomToAsync(zoomLevel);

        public global::Windows.Foundation.IAsyncOperation<bool> TrySetSceneAsync(global::Windows.UI.Xaml.Controls.Maps.MapScene scene) => UwpControl.TrySetSceneAsync(scene);

        public global::Windows.Foundation.IAsyncOperation<bool> TrySetSceneAsync(global::Windows.UI.Xaml.Controls.Maps.MapScene scene, global::Windows.UI.Xaml.Controls.Maps.MapAnimationKind animationKind) => UwpControl.TrySetSceneAsync(scene, animationKind);

        public global::Windows.Devices.Geolocation.Geopath GetVisibleRegion(global::Windows.UI.Xaml.Controls.Maps.MapVisibleRegionKind region) => UwpControl.GetVisibleRegion(region);

        public System.Collections.Generic.IReadOnlyList<global::Windows.UI.Xaml.Controls.Maps.MapElement> FindMapElementsAtOffset(global::Windows.Foundation.Point offset, double radius) => UwpControl.FindMapElementsAtOffset(offset, radius);

        public void GetLocationFromOffset(global::Windows.Foundation.Point offset, global::Windows.Devices.Geolocation.AltitudeReferenceSystem desiredReferenceSystem, out global::Windows.Devices.Geolocation.Geopoint location) => UwpControl.GetLocationFromOffset(offset, desiredReferenceSystem, out location);

        public void StartContinuousPan(double horizontalPixelsPerSecond, double verticalPixelsPerSecond) => UwpControl.StartContinuousPan(horizontalPixelsPerSecond, verticalPixelsPerSecond);

        public void StopContinuousPan() => UwpControl.StopContinuousPan();

        public global::Windows.Foundation.IAsyncOperation<bool> TryPanAsync(double horizontalPixels, double verticalPixels) => UwpControl.TryPanAsync(horizontalPixels, verticalPixels);

        public global::Windows.Foundation.IAsyncOperation<bool> TryPanToAsync(global::Windows.Devices.Geolocation.Geopoint location) => UwpControl.TryPanToAsync(location);

        public bool TryGetLocationFromOffset(global::Windows.Foundation.Point offset, out global::Windows.Devices.Geolocation.Geopoint location) => UwpControl.TryGetLocationFromOffset(offset, out location);

        public bool TryGetLocationFromOffset(global::Windows.Foundation.Point offset, global::Windows.Devices.Geolocation.AltitudeReferenceSystem desiredReferenceSystem, out global::Windows.Devices.Geolocation.Geopoint location) => UwpControl.TryGetLocationFromOffset(offset, desiredReferenceSystem, out location);

        public System.Collections.Generic.IReadOnlyList<global::Windows.UI.Xaml.Controls.Maps.MapElement> FindMapElementsAtOffset(global::Windows.Foundation.Point offset) => UwpControl.FindMapElementsAtOffset(offset);

        public void GetLocationFromOffset(global::Windows.Foundation.Point offset, out global::Windows.Devices.Geolocation.Geopoint location) => UwpControl.GetLocationFromOffset(offset, out location);

        public void GetOffsetFromLocation(global::Windows.Devices.Geolocation.Geopoint location, out global::Windows.Foundation.Point offset) => UwpControl.GetOffsetFromLocation(location, out offset);

        public global::Windows.Foundation.IAsyncOperation<bool> TrySetViewAsync(global::Windows.Devices.Geolocation.Geopoint center) => UwpControl.TrySetViewAsync(center);

        public global::Windows.Foundation.IAsyncOperation<bool> TrySetViewAsync(global::Windows.Devices.Geolocation.Geopoint center, double? zoomLevel) => UwpControl.TrySetViewAsync(center, zoomLevel);

        public global::Windows.Foundation.IAsyncOperation<bool> TrySetViewAsync(global::Windows.Devices.Geolocation.Geopoint center, double? zoomLevel, double? heading, double? desiredPitch) => UwpControl.TrySetViewAsync(center, zoomLevel, heading, desiredPitch);

        public global::Windows.Foundation.IAsyncOperation<bool> TrySetViewAsync(global::Windows.Devices.Geolocation.Geopoint center, double? zoomLevel, double? heading, double? desiredPitch, global::Windows.UI.Xaml.Controls.Maps.MapAnimationKind animation) => UwpControl.TrySetViewAsync(center, zoomLevel, heading, desiredPitch, animation);

        public void StartContinuousRotate(double rateInDegreesPerSecond) => UwpControl.StartContinuousRotate(rateInDegreesPerSecond);

        public void StopContinuousRotate() => UwpControl.StopContinuousRotate();

        public void StartContinuousTilt(double rateInDegreesPerSecond) => UwpControl.StartContinuousTilt(rateInDegreesPerSecond);

        public void StopContinuousTilt() => UwpControl.StopContinuousTilt();

        public void StartContinuousZoom(double rateOfChangePerSecond) => UwpControl.StartContinuousZoom(rateOfChangePerSecond);

        public void StopContinuousZoom() => UwpControl.StopContinuousZoom();

        public global::Windows.Foundation.IAsyncOperation<bool> TryRotateAsync(double degrees) => UwpControl.TryRotateAsync(degrees);

        public global::Windows.Foundation.IAsyncOperation<bool> TryRotateToAsync(double angleInDegrees) => UwpControl.TryRotateToAsync(angleInDegrees);

        public global::Windows.UI.Xaml.Controls.Maps.MapWatermarkMode WatermarkMode
        {
            get => (global::Windows.UI.Xaml.Controls.Maps.MapWatermarkMode)GetValue(WatermarkModeProperty);
            set => SetValue(WatermarkModeProperty, value);
        }

        public string MapServiceToken
        {
            get => (string)GetValue(MapServiceTokenProperty);
            set => SetValue(MapServiceTokenProperty, value);
        }

        public global::Windows.Foundation.Point TransformOrigin
        {
            get => (global::Windows.Foundation.Point)GetValue(TransformOriginProperty);
            set => SetValue(TransformOriginProperty, value);
        }

        public bool TrafficFlowVisible
        {
            get => (bool)GetValue(TrafficFlowVisibleProperty);
            set => SetValue(TrafficFlowVisibleProperty, value);
        }

        public bool LandmarksVisible
        {
            get => (bool)GetValue(LandmarksVisibleProperty);
            set => SetValue(LandmarksVisibleProperty, value);
        }

        public double Heading
        {
            get => (double)GetValue(HeadingProperty);
            set => SetValue(HeadingProperty, value);
        }

        public double DesiredPitch
        {
            get => (double)GetValue(DesiredPitchProperty);
            set => SetValue(DesiredPitchProperty, value);
        }

        public global::Windows.UI.Xaml.Controls.Maps.MapColorScheme ColorScheme
        {
            get => (global::Windows.UI.Xaml.Controls.Maps.MapColorScheme)GetValue(ColorSchemeProperty);
            set => SetValue(ColorSchemeProperty, value);
        }

        public bool PedestrianFeaturesVisible
        {
            get => (bool)GetValue(PedestrianFeaturesVisibleProperty);
            set => SetValue(PedestrianFeaturesVisibleProperty, value);
        }

        public double ZoomLevel
        {
            get => (double)GetValue(ZoomLevelProperty);
            set => SetValue(ZoomLevelProperty, value);
        }

        public global::Windows.Devices.Geolocation.Geopoint Center
        {
            get => (global::Windows.Devices.Geolocation.Geopoint)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }

        public global::Windows.UI.Xaml.Controls.Maps.MapLoadingStatus LoadingStatus
        {
            get => (global::Windows.UI.Xaml.Controls.Maps.MapLoadingStatus)GetValue(LoadingStatusProperty);
        }

        public System.Collections.Generic.IList<global::Windows.UI.Xaml.Controls.Maps.MapElement> MapElements
        {
            get => (System.Collections.Generic.IList<global::Windows.UI.Xaml.Controls.Maps.MapElement>)GetValue(MapElementsProperty);
        }

        public double MaxZoomLevel
        {
            get => UwpControl.MaxZoomLevel;
        }

        public double MinZoomLevel
        {
            get => UwpControl.MinZoomLevel;
        }

        public double Pitch
        {
            get => (double)GetValue(PitchProperty);
        }

        public System.Collections.Generic.IList<global::Windows.UI.Xaml.Controls.Maps.MapRouteView> Routes
        {
            get => (System.Collections.Generic.IList<global::Windows.UI.Xaml.Controls.Maps.MapRouteView>)GetValue(RoutesProperty);
        }

        public System.Collections.Generic.IList<global::Windows.UI.Xaml.Controls.Maps.MapTileSource> TileSources
        {
            get => (System.Collections.Generic.IList<global::Windows.UI.Xaml.Controls.Maps.MapTileSource>)GetValue(TileSourcesProperty);
        }

        public global::Windows.UI.Xaml.Controls.Maps.MapInteractionMode ZoomInteractionMode
        {
            get => (global::Windows.UI.Xaml.Controls.Maps.MapInteractionMode)GetValue(ZoomInteractionModeProperty);
            set => SetValue(ZoomInteractionModeProperty, value);
        }

        public bool TransitFeaturesVisible
        {
            get => (bool)GetValue(TransitFeaturesVisibleProperty);
            set => SetValue(TransitFeaturesVisibleProperty, value);
        }

        public global::Windows.UI.Xaml.Controls.Maps.MapInteractionMode TiltInteractionMode
        {
            get => (global::Windows.UI.Xaml.Controls.Maps.MapInteractionMode)GetValue(TiltInteractionModeProperty);
            set => SetValue(TiltInteractionModeProperty, value);
        }

        public global::Windows.UI.Xaml.Controls.Maps.MapScene Scene
        {
            get => (global::Windows.UI.Xaml.Controls.Maps.MapScene)GetValue(SceneProperty);
            set => SetValue(SceneProperty, value);
        }

        public global::Windows.UI.Xaml.Controls.Maps.MapInteractionMode RotateInteractionMode
        {
            get => (global::Windows.UI.Xaml.Controls.Maps.MapInteractionMode)GetValue(RotateInteractionModeProperty);
            set => SetValue(RotateInteractionModeProperty, value);
        }

        public global::Windows.UI.Xaml.Controls.Maps.MapPanInteractionMode PanInteractionMode
        {
            get => (global::Windows.UI.Xaml.Controls.Maps.MapPanInteractionMode)GetValue(PanInteractionModeProperty);
            set => SetValue(PanInteractionModeProperty, value);
        }

        public global::Windows.UI.Xaml.Controls.Maps.MapCustomExperience CustomExperience
        {
            get => UwpControl.CustomExperience;
            set => UwpControl.CustomExperience = value;
        }

        public bool BusinessLandmarksVisible
        {
            get => (bool)GetValue(BusinessLandmarksVisibleProperty);
            set => SetValue(BusinessLandmarksVisibleProperty, value);
        }

        public global::Windows.UI.Xaml.Controls.Maps.MapCamera ActualCamera
        {
            get => UwpControl.ActualCamera;
        }

        public bool Is3DSupported
        {
            get => (bool)GetValue(Is3DSupportedProperty);
        }

        public bool IsStreetsideSupported
        {
            get => (bool)GetValue(IsStreetsideSupportedProperty);
        }

        public global::Windows.UI.Xaml.Controls.Maps.MapCamera TargetCamera
        {
            get => UwpControl.TargetCamera;
        }

        public bool TransitFeaturesEnabled
        {
            get => (bool)GetValue(TransitFeaturesEnabledProperty);
            set => SetValue(TransitFeaturesEnabledProperty, value);
        }

        public bool BusinessLandmarksEnabled
        {
            get => (bool)GetValue(BusinessLandmarksEnabledProperty);
            set => SetValue(BusinessLandmarksEnabledProperty, value);
        }

        public global::Windows.UI.Xaml.Controls.Maps.MapStyleSheet StyleSheet
        {
            get => (global::Windows.UI.Xaml.Controls.Maps.MapStyleSheet)GetValue(StyleSheetProperty);
            set => SetValue(StyleSheetProperty, value);
        }

        public global::Windows.UI.Xaml.Controls.Maps.MapProjection MapProjection
        {
            get => (global::Windows.UI.Xaml.Controls.Maps.MapProjection)GetValue(MapProjectionProperty);
            set => SetValue(MapProjectionProperty, value);
        }

        public System.Collections.Generic.IList<global::Windows.UI.Xaml.Controls.Maps.MapLayer> Layers
        {
            get => (System.Collections.Generic.IList<global::Windows.UI.Xaml.Controls.Maps.MapLayer>)GetValue(LayersProperty);
            set => SetValue(LayersProperty, value);
        }

        public string Region
        {
            get => (string)GetValue(RegionProperty);
            set => SetValue(RegionProperty, value);
        }

        public event EventHandler<object> CenterChanged = (sender, args) => { };

        private void OnCenterChanged(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, object args)
        {
            this.CenterChanged?.Invoke(this, args);
        }

        public event EventHandler<object> HeadingChanged = (sender, args) => { };

        private void OnHeadingChanged(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, object args)
        {
            this.HeadingChanged?.Invoke(this, args);
        }

        public event EventHandler<object> LoadingStatusChanged = (sender, args) => { };

        private void OnLoadingStatusChanged(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, object args)
        {
            this.LoadingStatusChanged?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.MapInputEventArgs> MapDoubleTapped = (sender, args) => { };

        private void OnMapDoubleTapped(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, global::Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            this.MapDoubleTapped?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.MapInputEventArgs> MapHolding = (sender, args) => { };

        private void OnMapHolding(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, global::Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            this.MapHolding?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.MapInputEventArgs> MapTapped = (sender, args) => { };

        private void OnMapTapped(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, global::Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            this.MapTapped?.Invoke(this, args);
        }

        public event EventHandler<object> PitchChanged = (sender, args) => { };

        private void OnPitchChanged(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, object args)
        {
            this.PitchChanged?.Invoke(this, args);
        }

        public event EventHandler<object> TransformOriginChanged = (sender, args) => { };

        private void OnTransformOriginChanged(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, object args)
        {
            this.TransformOriginChanged?.Invoke(this, args);
        }

        public event EventHandler<object> ZoomLevelChanged = (sender, args) => { };

        private void OnZoomLevelChanged(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, object args)
        {
            this.ZoomLevelChanged?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.MapActualCameraChangedEventArgs> ActualCameraChanged = (sender, args) => { };

        private void OnActualCameraChanged(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangedEventArgs args)
        {
            this.ActualCameraChanged?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.MapActualCameraChangingEventArgs> ActualCameraChanging = (sender, args) => { };

        private void OnActualCameraChanging(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, global::Windows.UI.Xaml.Controls.Maps.MapActualCameraChangingEventArgs args)
        {
            this.ActualCameraChanging?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.MapCustomExperienceChangedEventArgs> CustomExperienceChanged = (sender, args) => { };

        private void OnCustomExperienceChanged(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, global::Windows.UI.Xaml.Controls.Maps.MapCustomExperienceChangedEventArgs args)
        {
            this.CustomExperienceChanged?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.MapElementClickEventArgs> MapElementClick = (sender, args) => { };

        private void OnMapElementClick(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, global::Windows.UI.Xaml.Controls.Maps.MapElementClickEventArgs args)
        {
            this.MapElementClick?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.MapElementPointerEnteredEventArgs> MapElementPointerEntered = (sender, args) => { };

        private void OnMapElementPointerEntered(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, global::Windows.UI.Xaml.Controls.Maps.MapElementPointerEnteredEventArgs args)
        {
            this.MapElementPointerEntered?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.MapElementPointerExitedEventArgs> MapElementPointerExited = (sender, args) => { };

        private void OnMapElementPointerExited(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, global::Windows.UI.Xaml.Controls.Maps.MapElementPointerExitedEventArgs args)
        {
            this.MapElementPointerExited?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.MapTargetCameraChangedEventArgs> TargetCameraChanged = (sender, args) => { };

        private void OnTargetCameraChanged(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, global::Windows.UI.Xaml.Controls.Maps.MapTargetCameraChangedEventArgs args)
        {
            this.TargetCameraChanged?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.MapRightTappedEventArgs> MapRightTapped = (sender, args) => { };

        private void OnMapRightTapped(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, global::Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs args)
        {
            this.MapRightTapped?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.MapContextRequestedEventArgs> MapContextRequested = (sender, args) => { };

        private void OnMapContextRequested(global::Windows.UI.Xaml.Controls.Maps.MapControl sender, global::Windows.UI.Xaml.Controls.Maps.MapContextRequestedEventArgs args)
        {
            this.MapContextRequested?.Invoke(this, args);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<DependencyObject> Children
        {
            get; set;
        }
    }
}