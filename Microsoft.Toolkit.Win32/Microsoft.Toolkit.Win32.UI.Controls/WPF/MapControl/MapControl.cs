using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Windows.Interop;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using uwpMaps = global::Windows.UI.Xaml.Controls.Maps;
using uwpXaml = global::Windows.UI.Xaml;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class MapControl : WindowsXamlHost
    {
        protected uwpMaps.MapControl UwpControl => this.XamlRoot as uwpMaps.MapControl;

        // Summary:
        //     Initializes a new instance of the MapControl class.
        public MapControl()
            : base()
        {
            TypeName = "Windows.UI.Xaml.Controls.MapControl";
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, uwpMaps.MapControl.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, uwpMaps.MapControl.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, uwpMaps.MapControl.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, uwpMaps.MapControl.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, uwpMaps.MapControl.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, uwpMaps.MapControl.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, uwpMaps.MapControl.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, uwpMaps.MapControl.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, uwpMaps.MapControl.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, uwpMaps.MapControl.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, uwpMaps.MapControl.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, uwpMaps.MapControl.NameProperty);
            Bind(nameof(Tag), TagProperty, uwpMaps.MapControl.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, uwpMaps.MapControl.DataContextProperty);
            Bind(nameof(Width), WidthProperty, uwpMaps.MapControl.WidthProperty);

            // MapControl specific properties
            Bind(nameof(WatermarkMode), WatermarkModeProperty, uwpMaps.MapControl.WatermarkModeProperty);
            Bind(nameof(Style), StyleProperty, uwpMaps.MapControl.StyleProperty);
            Bind(nameof(MapServiceToken), MapServiceTokenProperty, uwpMaps.MapControl.MapServiceTokenProperty);
            Bind(nameof(TransformOrigin), TransformOriginProperty, uwpMaps.MapControl.TransformOriginProperty);
            Bind(nameof(TrafficFlowVisible), TrafficFlowVisibleProperty, uwpMaps.MapControl.TrafficFlowVisibleProperty);
            Bind(nameof(LandmarksVisible), LandmarksVisibleProperty, uwpMaps.MapControl.LandmarksVisibleProperty);
            Bind(nameof(Heading), HeadingProperty, uwpMaps.MapControl.HeadingProperty);
            Bind(nameof(DesiredPitch), DesiredPitchProperty, uwpMaps.MapControl.DesiredPitchProperty);
            Bind(nameof(ColorScheme), ColorSchemeProperty, uwpMaps.MapControl.ColorSchemeProperty);
            Bind(nameof(PedestrianFeaturesVisible), PedestrianFeaturesVisibleProperty, uwpMaps.MapControl.PedestrianFeaturesVisibleProperty);
            Bind(nameof(ZoomLevel), ZoomLevelProperty, uwpMaps.MapControl.ZoomLevelProperty);
            Bind(nameof(Center), CenterProperty, uwpMaps.MapControl.CenterProperty);
            Bind(nameof(Children), ChildrenProperty, uwpMaps.MapControl.ChildrenProperty);
            Bind(nameof(LoadingStatus), LoadingStatusProperty, uwpMaps.MapControl.LoadingStatusProperty);
            Bind(nameof(MapElements), MapElementsProperty, uwpMaps.MapControl.MapElementsProperty);
            Bind(nameof(Pitch), PitchProperty, uwpMaps.MapControl.PitchProperty);
            Bind(nameof(Routes), RoutesProperty, uwpMaps.MapControl.RoutesProperty);
            Bind(nameof(TileSources), TileSourcesProperty, uwpMaps.MapControl.TileSourcesProperty);
            Bind(nameof(ZoomInteractionMode), ZoomInteractionModeProperty, uwpMaps.MapControl.ZoomInteractionModeProperty);
            Bind(nameof(TransitFeaturesVisible), TransitFeaturesVisibleProperty, uwpMaps.MapControl.TransitFeaturesVisibleProperty);
            Bind(nameof(TiltInteractionMode), TiltInteractionModeProperty, uwpMaps.MapControl.TiltInteractionModeProperty);
            Bind(nameof(Scene), SceneProperty, uwpMaps.MapControl.SceneProperty);
            Bind(nameof(RotateInteractionMode), RotateInteractionModeProperty, uwpMaps.MapControl.RotateInteractionModeProperty);
            Bind(nameof(PanInteractionMode), PanInteractionModeProperty, uwpMaps.MapControl.PanInteractionModeProperty);
            Bind(nameof(BusinessLandmarksVisible), BusinessLandmarksVisibleProperty, uwpMaps.MapControl.BusinessLandmarksVisibleProperty);
            Bind(nameof(Is3DSupported), Is3DSupportedProperty, uwpMaps.MapControl.Is3DSupportedProperty);
            Bind(nameof(IsStreetsideSupported), IsStreetsideSupportedProperty, uwpMaps.MapControl.IsStreetsideSupportedProperty);
            Bind(nameof(TransitFeaturesEnabled), TransitFeaturesEnabledProperty, uwpMaps.MapControl.TransitFeaturesEnabledProperty);
            Bind(nameof(BusinessLandmarksEnabled), BusinessLandmarksEnabledProperty, uwpMaps.MapControl.BusinessLandmarksEnabledProperty);
            Bind(nameof(ViewPadding), ViewPaddingProperty, uwpMaps.MapControl.ViewPaddingProperty);
            Bind(nameof(StyleSheet), StyleSheetProperty, uwpMaps.MapControl.StyleSheetProperty);
            Bind(nameof(MapProjection), MapProjectionProperty, uwpMaps.MapControl.MapProjectionProperty);
            Bind(nameof(Layers), LayersProperty, uwpMaps.MapControl.LayersProperty);
            Bind(nameof(Region), RegionProperty, uwpMaps.MapControl.RegionProperty);

            // Not dependency properties:
            // Bind(nameof(ActualCamera), ActualCameraProperty, uwpMaps.MapControl.ActualCameraProperty);
            // Bind(nameof(TargetCamera), TargetCameraProperty, uwpMaps.MapControl.TargetCameraProperty);
            // Bind(nameof(CustomExperience), CustomExperienceProperty, uwpMaps.MapControl.CustomExperienceProperty);
            // Bind(nameof(MaxZoomLevel), MaxZoomLevelProperty, uwpMaps.MapControl.MaxZoomLevelProperty);
            // Bind(nameof(MinZoomLevel), MinZoomLevelProperty, uwpMaps.MapControl.MinZoomLevelProperty);
        }

        public static DependencyProperty ColorSchemeProperty { get; } = DependencyProperty.Register(nameof(ColorScheme), typeof(uwpMaps.MapColorScheme), typeof(MapControl));

        public static DependencyProperty HeadingProperty { get; } = DependencyProperty.Register(nameof(Heading), typeof(double), typeof(MapControl));

        public static DependencyProperty LandmarksVisibleProperty { get; } = DependencyProperty.Register(nameof(LandmarksVisible), typeof(bool), typeof(MapControl));

        public static DependencyProperty LoadingStatusProperty { get; } = DependencyProperty.Register(nameof(LoadingStatus), typeof(uwpMaps.MapLoadingStatus), typeof(MapControl));

        public static DependencyProperty LocationProperty { get => CenterProperty; }

        public static DependencyProperty MapElementsProperty { get; } = DependencyProperty.Register(nameof(MapElements), typeof(IList<uwpMaps.MapElement>), typeof(MapControl));

        public static DependencyProperty MapServiceTokenProperty { get; } = DependencyProperty.Register(nameof(MapServiceToken), typeof(string), typeof(MapControl));

        // public static DependencyProperty NormalizedAnchorPointProperty { get; } = DependencyProperty.Register("NormalizedAnchor", typeof(uwpMaps.MapColorSchemex), typeof(MapControl));
        public static DependencyProperty PedestrianFeaturesVisibleProperty { get; } = DependencyProperty.Register(nameof(PedestrianFeaturesVisible), typeof(bool), typeof(MapControl));

        public static DependencyProperty PitchProperty { get; } = DependencyProperty.Register(nameof(Pitch), typeof(double), typeof(MapControl));

        public static DependencyProperty RoutesProperty { get; } = DependencyProperty.Register(nameof(Routes), typeof(uwpMaps.MapRouteView), typeof(MapControl));

        public static DependencyProperty TileSourcesProperty { get; } = DependencyProperty.Register(nameof(TileSources), typeof(uwpMaps.MapTileSource), typeof(MapControl));

        public static DependencyProperty TrafficFlowVisibleProperty { get; } = DependencyProperty.Register(nameof(TrafficFlowVisible), typeof(bool), typeof(MapControl));

        public static DependencyProperty TransformOriginProperty { get; } = DependencyProperty.Register(nameof(TransformOrigin), typeof(global::Windows.Foundation.Point), typeof(MapControl));

        public static DependencyProperty WatermarkModeProperty { get; } = DependencyProperty.Register(nameof(WatermarkMode), typeof(uwpMaps.MapWatermarkMode), typeof(MapControl));

        public static DependencyProperty CenterProperty { get; } = DependencyProperty.Register(nameof(Center), typeof(Geopoint), typeof(MapControl));

        public static DependencyProperty ChildrenProperty { get; } = DependencyProperty.Register(nameof(Children), typeof(IList<DependencyObject>), typeof(MapControl));

        public static DependencyProperty ZoomLevelProperty { get; } = DependencyProperty.Register(nameof(ZoomLevel), typeof(double), typeof(MapControl));

        public static DependencyProperty DesiredPitchProperty { get; } = DependencyProperty.Register(nameof(DesiredPitch), typeof(double), typeof(MapControl));

        public static DependencyProperty BusinessLandmarksVisibleProperty { get; } = DependencyProperty.Register(nameof(BusinessLandmarksVisible), typeof(bool), typeof(MapControl));

        public static DependencyProperty Is3DSupportedProperty { get; } = DependencyProperty.Register(nameof(Is3DSupported), typeof(bool), typeof(MapControl));

        public static DependencyProperty IsStreetsideSupportedProperty { get; } = DependencyProperty.Register(nameof(IsStreetsideSupported), typeof(bool), typeof(MapControl));

        public static DependencyProperty PanInteractionModeProperty { get; } = DependencyProperty.Register(nameof(PanInteractionMode), typeof(uwpMaps.MapPanInteractionMode), typeof(MapControl));

        public static DependencyProperty RotateInteractionModeProperty { get; } = DependencyProperty.Register(nameof(RotateInteractionMode), typeof(uwpMaps.MapInteractionMode), typeof(MapControl));

        public static DependencyProperty SceneProperty { get; } = DependencyProperty.Register(nameof(Scene), typeof(uwpMaps.MapScene), typeof(MapControl));

        public static DependencyProperty TiltInteractionModeProperty { get; } = DependencyProperty.Register(nameof(TiltInteractionMode), typeof(uwpMaps.MapInteractionMode), typeof(MapControl));

        public static DependencyProperty TransitFeaturesVisibleProperty { get; } = DependencyProperty.Register(nameof(TransitFeaturesVisible), typeof(bool), typeof(MapControl));

        public static DependencyProperty ZoomInteractionModeProperty { get; } = DependencyProperty.Register(nameof(ZoomInteractionMode), typeof(uwpMaps.MapInteractionMode), typeof(MapControl));

        public static DependencyProperty BusinessLandmarksEnabledProperty { get; } = DependencyProperty.Register(nameof(BusinessLandmarksEnabled), typeof(bool), typeof(MapControl));

        public static DependencyProperty TransitFeaturesEnabledProperty { get; } = DependencyProperty.Register(nameof(TransitFeaturesEnabled), typeof(bool), typeof(MapControl));

        public static DependencyProperty MapProjectionProperty { get; } = DependencyProperty.Register(nameof(MapProjection), typeof(uwpMaps.MapProjection), typeof(MapControl));

        public static DependencyProperty StyleSheetProperty { get; } = DependencyProperty.Register(nameof(StyleSheet), typeof(uwpMaps.MapStyleSheet), typeof(MapControl));

        public static DependencyProperty ViewPaddingProperty { get; } = DependencyProperty.Register(nameof(ViewPadding), typeof(Thickness), typeof(MapControl));

        public static DependencyProperty LayersProperty { get; } = DependencyProperty.Register(nameof(Layers), typeof(uwpMaps.MapLayer), typeof(MapControl));

        public static DependencyProperty RegionProperty { get; } = DependencyProperty.Register(nameof(Region), typeof(string), typeof(MapControl));

        public static Geopoint GetLocation(uwpXaml.DependencyObject element) => uwpMaps.MapControl.GetLocation(element);

        public static void SetLocation(uwpXaml.DependencyObject element, Geopoint value) => uwpMaps.MapControl.SetLocation(element, value);

        public static global::Windows.Foundation.Point GetNormalizedAnchorPoint(uwpXaml.DependencyObject element) => uwpMaps.MapControl.GetNormalizedAnchorPoint(element);

        public static void SetNormalizedAnchorPoint(uwpXaml.DependencyObject element, global::Windows.Foundation.Point value) => uwpMaps.MapControl.SetNormalizedAnchorPoint(element, value);

        public IReadOnlyList<uwpMaps.MapElement> FindMapElementsAtOffset(global::Windows.Foundation.Point offset) => UwpControl.FindMapElementsAtOffset(offset);

        public void GetLocationFromOffset(global::Windows.Foundation.Point offset, out Geopoint location) => UwpControl.GetLocationFromOffset(offset, out location);

        public void GetOffsetFromLocation(Geopoint location, out global::Windows.Foundation.Point offset) => UwpControl.GetOffsetFromLocation(location, out offset);

        public void IsLocationInView(Geopoint location, out bool isInView) => UwpControl.IsLocationInView(location, out isInView);

        // public IAsyncOperation<bool> TrySetViewBoundsAsync(GeoboundingBox bounds, uwpXaml.Thickness? margin, uwpMaps.MapAnimationKind animation) => UwpControl.TrySetViewBoundsAsync(bounds, margin, animation);
        public IAsyncOperation<bool> TrySetViewAsync(Geopoint center) => UwpControl.TrySetViewAsync(center);

        public IAsyncOperation<bool> TrySetViewAsync(Geopoint center, double? zoomLevel) => UwpControl.TrySetViewAsync(center, zoomLevel);

        public IAsyncOperation<bool> TrySetViewAsync(Geopoint center, double? zoomLevel, double? heading, double? desiredPitch) => UwpControl.TrySetViewAsync(center, zoomLevel, heading, desiredPitch);

        public IAsyncOperation<bool> TrySetViewAsync(Geopoint center, double? zoomLevel, double? heading, double? desiredPitch, uwpMaps.MapAnimationKind animation) => UwpControl.TrySetViewAsync(center, zoomLevel, heading, desiredPitch, animation);

        public void StartContinuousRotate(double rateInDegreesPerSecond) => UwpControl.StartContinuousRotate(rateInDegreesPerSecond);

        public void StopContinuousRotate() => UwpControl.StopContinuousRotate();

        public void StartContinuousTilt(double rateInDegreesPerSecond) => UwpControl.StartContinuousTilt(rateInDegreesPerSecond);

        public void StopContinuousTilt() => UwpControl.StopContinuousTilt();

        public void StartContinuousZoom(double rateOfChangePerSecond) => UwpControl.StartContinuousZoom(rateOfChangePerSecond);

        public void StopContinuousZoom() => UwpControl.StopContinuousZoom();

        public IAsyncOperation<bool> TryRotateAsync(double degrees) => UwpControl.TryRotateAsync(degrees);

        public IAsyncOperation<bool> TryRotateToAsync(double angleInDegrees) => UwpControl.TryRotateToAsync(angleInDegrees);

        public IAsyncOperation<bool> TryTiltAsync(double degrees) => UwpControl.TryTiltToAsync(degrees);

        public IAsyncOperation<bool> TryTiltToAsync(double angleInDegrees) => UwpControl.TryTiltToAsync(angleInDegrees);

        public IAsyncOperation<bool> TryZoomInAsync() => UwpControl.TryZoomInAsync();

        public IAsyncOperation<bool> TryZoomOutAsync() => UwpControl.TryZoomOutAsync();

        public IAsyncOperation<bool> TryZoomToAsync(double zoomLevel) => UwpControl.TryZoomToAsync(zoomLevel);

        public IAsyncOperation<bool> TrySetSceneAsync(uwpMaps.MapScene scene) => UwpControl.TrySetSceneAsync(scene);

        public IAsyncOperation<bool> TrySetSceneAsync(uwpMaps.MapScene scene, uwpMaps.MapAnimationKind animationKind) => UwpControl.TrySetSceneAsync(scene, animationKind);

        public Geopath GetVisibleRegion(uwpMaps.MapVisibleRegionKind region) => UwpControl.GetVisibleRegion(region);

        public IReadOnlyList<uwpMaps.MapElement> FindMapElementsAtOffset(global::Windows.Foundation.Point offset, double radius) => UwpControl.FindMapElementsAtOffset(offset, radius);

        public void GetLocationFromOffset(global::Windows.Foundation.Point offset, AltitudeReferenceSystem desiredReferenceSystem, out Geopoint location) => UwpControl.GetLocationFromOffset(offset, out location);

        public void StartContinuousPan(double horizontalPixelsPerSecond, double verticalPixelsPerSecond) => UwpControl.StartContinuousPan(horizontalPixelsPerSecond, verticalPixelsPerSecond);

        public void StopContinuousPan() => UwpControl.StopContinuousPan();

        public IAsyncOperation<bool> TryPanAsync(double horizontalPixels, double verticalPixels) => UwpControl.TryPanAsync(horizontalPixels, verticalPixels);

        public IAsyncOperation<bool> TryPanToAsync(Geopoint location) => UwpControl.TryPanToAsync(location);

        public bool TryGetLocationFromOffset(global::Windows.Foundation.Point offset, out Geopoint location) => UwpControl.TryGetLocationFromOffset(offset, out location);

        public bool TryGetLocationFromOffset(global::Windows.Foundation.Point offset, AltitudeReferenceSystem desiredReferenceSystem, out Geopoint location) => UwpControl.TryGetLocationFromOffset(offset, desiredReferenceSystem, out location);

        public uwpMaps.MapWatermarkMode WatermarkMode
        {
            get { return (uwpMaps.MapWatermarkMode)GetValue(WatermarkModeProperty); } set => SetValue(WatermarkModeProperty, value);
        }

        public new uwpMaps.MapStyle Style
        {
            get { return (uwpMaps.MapStyle)GetValue(StyleProperty); } set => SetValue(StyleProperty, value);
        }

        public string MapServiceToken
        {
            get { return (string)GetValue(MapServiceTokenProperty); } set => SetValue(MapServiceTokenProperty, value);
        }

        public global::Windows.Foundation.Point TransformOrigin { get; set; }

        public bool TrafficFlowVisible
        {
            get { return (bool)GetValue(TrafficFlowVisibleProperty); } set => SetValue(TrafficFlowVisibleProperty, value);
        }

        public bool LandmarksVisible
        {
            get { return (bool)GetValue(LandmarksVisibleProperty); } set => SetValue(LandmarksVisibleProperty, value);
        }

        public double Heading
        {
            get { return (double)GetValue(HeadingProperty); } set => SetValue(HeadingProperty, value);
        }

        public double DesiredPitch
        {
            get { return (double)GetValue(DesiredPitchProperty); } set => SetValue(DesiredPitchProperty, value);
        }

        public uwpMaps.MapColorScheme ColorScheme
        {
            get { return (uwpMaps.MapColorScheme)GetValue(ColorSchemeProperty); } set => SetValue(ColorSchemeProperty, value);
        }

        public bool PedestrianFeaturesVisible
        {
            get { return (bool)GetValue(PedestrianFeaturesVisibleProperty); } set => SetValue(PedestrianFeaturesVisibleProperty, value);
        }

        public double ZoomLevel
        {
            get { return (double)GetValue(ZoomLevelProperty); } set => SetValue(ZoomLevelProperty, value);
        }

        public Geopoint Center
        {
            get { return (Geopoint)GetValue(CenterProperty); } set => SetValue(CenterProperty, value);
        }

        public IList<DependencyObject> Children { get; }

        public uwpMaps.MapLoadingStatus LoadingStatus
        {
            get { return (uwpMaps.MapLoadingStatus)GetValue(LoadingStatusProperty); }
        }

        public IList<uwpMaps.MapElement> MapElements { get; }

        public double MaxZoomLevel { get => UwpControl.MaxZoomLevel; }

        public double MinZoomLevel { get => UwpControl.MinZoomLevel; }

        public double Pitch
        {
            get { return (double)GetValue(PitchProperty); }
        }

        public IList<uwpMaps.MapRouteView> Routes { get; }

        public IList<uwpMaps.MapTileSource> TileSources { get; }

        public uwpMaps.MapInteractionMode ZoomInteractionMode
        {
            get { return (uwpMaps.MapInteractionMode)GetValue(ZoomInteractionModeProperty); } set => SetValue(ZoomInteractionModeProperty, value);
        }

        public bool TransitFeaturesVisible
        {
            get { return (bool)GetValue(TransitFeaturesVisibleProperty); } set => SetValue(TransitFeaturesVisibleProperty, value);
        }

        public uwpMaps.MapInteractionMode TiltInteractionMode
        {
            get { return (uwpMaps.MapInteractionMode)GetValue(TiltInteractionModeProperty); } set => SetValue(TiltInteractionModeProperty, value);
        }

        public uwpMaps.MapScene Scene
        {
            get { return (uwpMaps.MapScene)GetValue(SceneProperty); } set => SetValue(SceneProperty, value);
        }

        public uwpMaps.MapInteractionMode RotateInteractionMode
        {
            get { return (uwpMaps.MapInteractionMode)GetValue(RotateInteractionModeProperty); } set => SetValue(RotateInteractionModeProperty, value);
        }

        public uwpMaps.MapPanInteractionMode PanInteractionMode
        {
            get { return (uwpMaps.MapPanInteractionMode)GetValue(PanInteractionModeProperty); } set => SetValue(PanInteractionModeProperty, value);
        }

        public uwpMaps.MapCustomExperience CustomExperience
        {
            get => UwpControl.CustomExperience; set { UwpControl.CustomExperience = value; }
        }

        public bool BusinessLandmarksVisible
        {
            get { return (bool)GetValue(BusinessLandmarksVisibleProperty); } set => SetValue(BusinessLandmarksVisibleProperty, value);
        }

        public uwpMaps.MapCamera ActualCamera { get => UwpControl.ActualCamera; }

        public bool Is3DSupported
        {
            get { return (bool)GetValue(Is3DSupportedProperty); }
        }

        public bool IsStreetsideSupported
        {
            get { return (bool)GetValue(IsStreetsideSupportedProperty); }
        }

        public uwpMaps.MapCamera TargetCamera { get => UwpControl.TargetCamera; }

        public bool TransitFeaturesEnabled
        {
            get { return (bool)GetValue(TransitFeaturesEnabledProperty); } set => SetValue(TransitFeaturesEnabledProperty, value);
        }

        public bool BusinessLandmarksEnabled
        {
            get { return (bool)GetValue(BusinessLandmarksEnabledProperty); } set => SetValue(BusinessLandmarksEnabledProperty, value);
        }

        public Thickness ViewPadding
        {
            get { return (Thickness)GetValue(ViewPaddingProperty); } set => SetValue(ViewPaddingProperty, value);
        }

        public uwpMaps.MapStyleSheet StyleSheet
        {
            get { return (uwpMaps.MapStyleSheet)GetValue(StyleSheetProperty); } set => SetValue(StyleSheetProperty, value);
        }

        public uwpMaps.MapProjection MapProjection
        {
            get { return (uwpMaps.MapProjection)GetValue(MapProjectionProperty); } set => SetValue(MapProjectionProperty, value);
        }

        public IList<uwpMaps.MapLayer> Layers { get; set; }

        public string Region
        {
            get { return (string)GetValue(RegionProperty); } set => SetValue(RegionProperty, value);
        }

        public event TypedEventHandler<uwpMaps.MapControl, object> CenterChanged
        {
            add { UwpControl.CenterChanged += value; }
            remove { UwpControl.CenterChanged -= value; }
        }

        public event TypedEventHandler<uwpMaps.MapControl, object> HeadingChanged
        {
            add { UwpControl.HeadingChanged += value; }
            remove { UwpControl.HeadingChanged -= value; }
        }

        public event TypedEventHandler<uwpMaps.MapControl, object> LoadingStatusChanged
        {
            add { UwpControl.LoadingStatusChanged += value; }
            remove { UwpControl.LoadingStatusChanged -= value; }
        }

        public event TypedEventHandler<uwpMaps.MapControl, uwpMaps.MapInputEventArgs> MapDoubleTapped
        {
            add { UwpControl.MapDoubleTapped += value; }
            remove { UwpControl.MapDoubleTapped -= value; }
        }

        public event TypedEventHandler<uwpMaps.MapControl, uwpMaps.MapInputEventArgs> MapHolding
        {
            add { UwpControl.MapHolding += value; }
            remove { UwpControl.MapHolding -= value; }
        }

        public event TypedEventHandler<uwpMaps.MapControl, uwpMaps.MapInputEventArgs> MapTapped
        {
            add { UwpControl.MapTapped += value; }
            remove { UwpControl.MapTapped -= value; }
        }

        public event TypedEventHandler<uwpMaps.MapControl, object> PitchChanged
        {
            add { UwpControl.PitchChanged += value; }
            remove { UwpControl.PitchChanged -= value; }
        }

        public event TypedEventHandler<uwpMaps.MapControl, object> TransformOriginChanged
        {
            add { UwpControl.TransformOriginChanged += value; }
            remove { UwpControl.TransformOriginChanged -= value; }
        }

        public event TypedEventHandler<uwpMaps.MapControl, object> ZoomLevelChanged
        {
            add { UwpControl.ZoomLevelChanged += value; }
            remove { UwpControl.ZoomLevelChanged -= value; }
        }

        public event TypedEventHandler<uwpMaps.MapControl, uwpMaps.MapActualCameraChangedEventArgs> ActualCameraChanged
        {
            add { UwpControl.ActualCameraChanged += value; }
            remove { UwpControl.ActualCameraChanged -= value; }
        }

        public event TypedEventHandler<uwpMaps.MapControl, uwpMaps.MapActualCameraChangingEventArgs> ActualCameraChanging
        {
            add { UwpControl.ActualCameraChanging += value; }
            remove { UwpControl.ActualCameraChanging -= value; }
        }

        public event TypedEventHandler<uwpMaps.MapControl, uwpMaps.MapCustomExperienceChangedEventArgs> CustomExperienceChanged
        {
            add { UwpControl.CustomExperienceChanged += value; }
            remove { UwpControl.CustomExperienceChanged -= value; }
        }

        public event TypedEventHandler<uwpMaps.MapControl, uwpMaps.MapElementClickEventArgs> MapElementClick
        {
            add { UwpControl.MapElementClick += value; }
            remove { UwpControl.MapElementClick -= value; }
        }

        public event TypedEventHandler<uwpMaps.MapControl, uwpMaps.MapElementPointerEnteredEventArgs> MapElementPointerEntered
        {
            add { UwpControl.MapElementPointerEntered += value; }
            remove { UwpControl.MapElementPointerEntered -= value; }
        }

        public event TypedEventHandler<uwpMaps.MapControl, uwpMaps.MapElementPointerExitedEventArgs> MapElementPointerExited
        {
            add { UwpControl.MapElementPointerExited += value; }
            remove { UwpControl.MapElementPointerExited -= value; }
        }

        public event TypedEventHandler<uwpMaps.MapControl, uwpMaps.MapTargetCameraChangedEventArgs> TargetCameraChanged
        {
            add { UwpControl.TargetCameraChanged += value; }
            remove { UwpControl.TargetCameraChanged -= value; }
        }

        public event TypedEventHandler<uwpMaps.MapControl, uwpMaps.MapRightTappedEventArgs> MapRightTapped
        {
            add { UwpControl.MapRightTapped += value; }
            remove { UwpControl.MapRightTapped -= value; }
        }

        public event TypedEventHandler<uwpMaps.MapControl, uwpMaps.MapContextRequestedEventArgs> MapContextRequested
        {
            add { UwpControl.MapContextRequested += value; }
            remove { UwpControl.MapContextRequested -= value; }
        }
    }
}