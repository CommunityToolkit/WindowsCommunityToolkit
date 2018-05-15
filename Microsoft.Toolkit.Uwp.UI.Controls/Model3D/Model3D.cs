using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Class used to display glTF models
    /// </summary>
    [TemplatePart(Name = WebViewPart, Type = typeof(WebView))]
    [TemplatePart(Name = CommandGridPart, Type = typeof(Grid))]
    [TemplatePart(Name = AnimationListPart, Type = typeof(ComboBox))]
    [TemplatePart(Name = AnimationSliderPart, Type = typeof(Slider))]
    [TemplatePart(Name = PlaySymbolPart, Type = typeof(SymbolIcon))]
    [TemplatePart(Name = PauseSymbolPart, Type = typeof(SymbolIcon))]
    [TemplatePart(Name = PlayPauseButtonPart, Type = typeof(Button))]
    public sealed class Model3D : Control
    {
        private WebView _view;
        private ComboBox _animationList;
        private Grid _commandGrid;
        private Slider _animationSlider;
        private SymbolIcon _playSymbol;
        private SymbolIcon _pauseSymbol;
        private Button _playPauseButton;
        private bool _sliderLayoutUpdated = false;
        private double _lastValueSetFromAnimation = 0;

        /// <summary>
        /// Event raised when the assets are loading
        /// </summary>
        public event EventHandler AssetLoading;

        /// <summary>
        /// Event raised when assets are loaded
        /// </summary>
        public event EventHandler AssetLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="Model3D"/> class
        /// </summary>
        public Model3D()
        {
            DefaultStyleKey = typeof(Model3D);
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call
        /// ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays
        /// in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            if (_view != null)
            {
                _view.ScriptNotify -= View_ScriptNotify;
                _view.NavigationCompleted -= View_NavigationCompleted;
            }

            if (_playPauseButton != null)
            {
                _playPauseButton.Click -= PlayPauseButton_Click;
            }

            if (_animationList != null)
            {
                _animationList.SelectionChanged -= AnimationList_SelectionChanged;
            }

            if (_animationSlider != null)
            {
                _animationSlider.LayoutUpdated -= AnimationSlider_LayoutUpdated;
                _animationSlider.ValueChanged -= AnimationSlider_ValueChanged;
            }

            base.OnApplyTemplate();

            _view = GetTemplateChild(WebViewPart) as WebView;
            if (_view != null)
            {
                _view.ScriptNotify += View_ScriptNotify;
                _view.NavigationCompleted += View_NavigationCompleted;
                _view.Navigate(new Uri("ms-appx-web:///Microsoft.Toolkit.Uwp.UI.Controls/Model3D/BabylonView.html"));
            }

            _animationList = GetTemplateChild(AnimationListPart) as ComboBox;
            if (_animationList != null)
            {
                _animationList.SelectionChanged += AnimationList_SelectionChanged;
            }

            _commandGrid = GetTemplateChild(CommandGridPart) as Grid;

            _playSymbol = GetTemplateChild(PlaySymbolPart) as SymbolIcon;
            _pauseSymbol = GetTemplateChild(PauseSymbolPart) as SymbolIcon;

            _animationSlider = GetTemplateChild(AnimationSliderPart) as Slider;

            if (_animationSlider != null)
            {
                _animationSlider.Minimum = 0;
                _animationSlider.Maximum = 100;
                _animationSlider.LayoutUpdated += AnimationSlider_LayoutUpdated;
                _animationSlider.ValueChanged += AnimationSlider_ValueChanged;
            }

            _playPauseButton = GetTemplateChild(PlayPauseButtonPart) as Button;
            if (_playPauseButton != null)
            {
                _playPauseButton.Click += PlayPauseButton_Click;
            }
        }

        private static async void OnCameraControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            await (d as Model3D).UpdateCameraControlAsync();
        }

        private static async void OnRadiusValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            await (d as Model3D).UpdateRadiusAsync();
        }

        private static async void OnCameraValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            await (d as Model3D).UpdateCameraAsync();
        }

        private static async void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            await (d as Model3D).SetupSourceAsync();
        }

        private static async void OnEnvironmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            await (d as Model3D).UpdateEnvironment();
        }

        private async void AnimationSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (e.NewValue == _lastValueSetFromAnimation)
            {
                // This is an animated value
                return;
            }

            await _view.InvokeScriptAsync("goToFrame", new string[] { (e.NewValue / 100.0).ToString() });
        }

        private async void AnimationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await _view.InvokeScriptAsync("switchToAnimationGroup", new string[] { _animationList.SelectedIndex.ToString() });
            await Play();
        }

        private void AnimationSlider_LayoutUpdated(object sender, object e)
        {
            // We will use it to prevent layout cycle exception
            _sliderLayoutUpdated = true;
        }

        private async Task Pause()
        {
            _playSymbol.Visibility = Visibility.Visible;
            _pauseSymbol.Visibility = Visibility.Collapsed;

            await _view.InvokeScriptAsync("pauseAnimations", new string[] { });
        }

        private async Task Play()
        {
            _playSymbol.Visibility = Visibility.Collapsed;
            _pauseSymbol.Visibility = Visibility.Visible;

            await _view.InvokeScriptAsync("playAnimations", new string[] { });
        }

        private async void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_playSymbol == null || _pauseSymbol == null)
            {
                return;
            }

            if (_playSymbol.Visibility == Visibility.Collapsed)
            {
                // Let's pause
                await Pause();
            }
            else
            {
                // Or play
                await Play();
            }
        }

        private async void View_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            await SetupSourceAsync();
            await UpdateCameraAsync();
            await UpdateRadiusAsync();
            await UpdateCameraControlAsync();
            await UpdateEnvironment();
        }

        private void View_ScriptNotify(object sender, NotifyEventArgs e)
        {
            var key = e.Value.Substring(0, 4);
            var value = e.Value.Substring(4);

            switch (key)
            {
                // Loading assets
                case "LOAD":
                    {
                        AssetLoading?.Invoke(this, EventArgs.Empty);
                        break;
                    }

                // Assets loaded
                case "LODD":
                    {
                        _view.Opacity = 1;
                        AssetLoaded?.Invoke(this, EventArgs.Empty);
                        break;
                    }

                // Camera alpha angle in radians
                case "ALPH":
                    {
                        AlphaInDegrees = GetDegreeFromRadian(float.Parse(value));
                        break;
                    }

                // Camera beta angle in radians
                case "BETA":
                    {
                        BetaInDegrees = GetDegreeFromRadian(float.Parse(value));
                        break;
                    }

                // Camera radius
                case "RADI":
                    {
                        RadiusPercentage = float.Parse(value);
                        break;
                    }

                // Animations are supported
                case "ANIM":
                    {
                        if (_commandGrid == null)
                        {
                            return;
                        }

                        _commandGrid.Visibility = Visibility.Visible;

                        var animationNames = value.Split(',');

                        if (_animationList != null)
                        {
                            _animationList.Items.Clear();

                            foreach (var animName in animationNames)
                            {
                                _animationList.Items.Add(animName);
                            }

                            _animationList.SelectedIndex = 0;

                            _playSymbol.Visibility = Visibility.Collapsed;
                            _pauseSymbol.Visibility = Visibility.Visible;
                        }

                        break;
                    }

                // No animation
                case "NANM":
                    {
                        if (_commandGrid != null)
                        {
                            _commandGrid.Visibility = Visibility.Collapsed;
                        }

                        break;
                    }

                // Current animation frame
                case "FRAM":
                    {
                        if (_animationSlider == null || !_sliderLayoutUpdated)
                        {
                            return;
                        }

                        var position = Math.Min(1, Math.Max(0, double.Parse(value)));

                        // Making sure we are not overwhelming the system
                        _sliderLayoutUpdated = false;
                        _lastValueSetFromAnimation = position * 99.0;
                        _animationSlider.Value = _lastValueSetFromAnimation;
                        break;
                    }
            }
        }

        private async Task SetupSourceAsync()
        {
            if (_view == null || string.IsNullOrWhiteSpace(Source))
            {
                return;
            }

            string root = null, fileName = null;

            if (Uri.TryCreate(Source, UriKind.Relative, out var uri))
            {
                root = "ms-appx-web:///";
                int slashIndex = 0;
                while (Source[slashIndex] == '/')
                {
                    slashIndex++;
                }

                fileName = Source.Substring(slashIndex);
            }
            else if (Uri.TryCreate(Source, UriKind.Absolute, out uri))
            {
                var scheme = uri.Scheme == "ms-appx" ? "ms-appx-web" : uri.Scheme;
                root = $"{scheme}://{uri.Authority}/";

                int slashIndex = 0;
                while (uri.LocalPath[slashIndex] == '/')
                {
                    slashIndex++;
                }

                fileName = uri.LocalPath.Substring(slashIndex);
            }
            else
            {
                return;
            }

            await _view.InvokeScriptAsync("setupSource", new string[] { root, fileName });
        }

        private async Task UpdateEnvironment()
        {
            if (_view == null || string.IsNullOrWhiteSpace(Environment))
            {
                return;
            }
 
            await _view.InvokeScriptAsync("updateEnvironment", new string[] { Environment });
        }

        private async Task UpdateCameraAsync()
        {
            if (_view == null)
            {
                return;
            }

            float alpha = GetRadianFromDegree(AlphaInDegrees);
            float beta = GetRadianFromDegree(BetaInDegrees);

            await _view.InvokeScriptAsync("updateCameraPositionValues", new string[] { alpha.ToString(), beta.ToString() });
        }

        private async Task UpdateRadiusAsync()
        {
            if (_view == null)
            {
                return;
            }

            await _view.InvokeScriptAsync("updateCameraRadiusValue", new string[] { RadiusPercentage.ToString() });
        }

        private async Task UpdateCameraControlAsync()
        {
            if (_view == null)
            {
                return;
            }

            await _view.InvokeScriptAsync("updateCameraControl", new string[] { CameraControl.ToString() });
        }

        private float GetRadianFromDegree(float degrees)
        {
            return degrees / 180f * (float)Math.PI;
        }

        private float GetDegreeFromRadian(float radians)
        {
            var value = (radians * 180f / (float)Math.PI) % 360f;

            if (value < 0)
            {
                value += 360f * (float)Math.Ceiling(Math.Abs(value / 360.0f));
            }

            return value;
        }

        /// <summary>
        /// Gets or sets a string indicating the source path for the model
        /// </summary>
        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Source"/> property
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(Model3D), new PropertyMetadata(string.Empty, OnSourceChanged));

        /// <summary>
        /// Gets or sets a string indicating the path for the environment texture (must be a DDS file)
        /// </summary>
        public string Environment
        {
            get { return (string)GetValue(EnvironmentProperty); }
            set { SetValue(EnvironmentProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Environment"/> property
        /// </summary>
        public static readonly DependencyProperty EnvironmentProperty =
            DependencyProperty.Register("Environment", typeof(string), typeof(Model3D), new PropertyMetadata(string.Empty, OnEnvironmentChanged));

        /// <summary>
        /// Gets or sets a float value indicating the alpha angle in degrees (which defines the rotation on horizontal plane)
        /// </summary>
        public float AlphaInDegrees
        {
            get { return (float)GetValue(AlphaInDegreesProperty); }
            set { SetValue(AlphaInDegreesProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="AlphaInDegrees"/> property
        /// </summary>
        public static readonly DependencyProperty AlphaInDegreesProperty =
            DependencyProperty.Register("AlphaInDegrees", typeof(float), typeof(Model3D), new PropertyMetadata(90f, OnCameraValueChanged));

        /// <summary>
        /// Gets or sets a float value indicating the beta angle in degrees (which defines the rotation on vertical plane)
        /// </summary>
        public float BetaInDegrees
        {
            get { return (float)GetValue(BetaInDegreesProperty); }
            set { SetValue(BetaInDegreesProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="BetaInDegrees"/> property
        /// </summary>
        public static readonly DependencyProperty BetaInDegreesProperty =
            DependencyProperty.Register("BetaInDegrees", typeof(float), typeof(Model3D), new PropertyMetadata(90f, OnCameraValueChanged));

        /// <summary>
        /// Gets or sets a float value between 0 and 100 indicating the distance from the center of the scene
        /// </summary>
        public float RadiusPercentage
        {
            get { return (float)GetValue(RadiusPercentageProperty); }
            set { SetValue(RadiusPercentageProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="RadiusPercentage"/> property
        /// </summary>
        public static readonly DependencyProperty RadiusPercentageProperty =
            DependencyProperty.Register("RadiusPercentage", typeof(float), typeof(Model3D), new PropertyMetadata(5f, OnRadiusValueChanged));

        /// <summary>
        /// Gets or sets a value indicating whether the camera can be controlled with pointers (mouse, touch, pencil) or not
        /// </summary>
        public bool CameraControl
        {
            get { return (bool)GetValue(CameraControlProperty); }
            set { SetValue(CameraControlProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="CameraControl"/> property
        /// </summary>
        public static readonly DependencyProperty CameraControlProperty =
            DependencyProperty.Register("CameraControl", typeof(bool), typeof(Model3D), new PropertyMetadata(true, OnCameraControlChanged));

        /// <summary>
        /// Key of the UI Element that contains the webview
        /// </summary>
        private const string WebViewPart = "PART_WebView";

        /// <summary>
        /// Key of the UI Element that contains the command grid
        /// </summary>
        private const string CommandGridPart = "PART_CommandGrid";

        /// <summary>
        /// Key of the UI Element that contains the list of animations
        /// </summary>
        private const string AnimationListPart = "PART_AnimationList";

        /// <summary>
        /// Key of the UI Element that contains the animation slider
        /// </summary>
        private const string AnimationSliderPart = "PART_AnimationSlider";

        /// <summary>
        /// Key of the UI Element that contains the play/pause button
        /// </summary>
        private const string PlayPauseButtonPart = "PART_PlayPauseButton";

        /// <summary>
        /// Key of the UI Element that contains the play symbol
        /// </summary>
        private const string PlaySymbolPart = "PART_PlaySymbol";

        /// <summary>
        /// Key of the UI Element that contains the play symbol
        /// </summary>
        private const string PauseSymbolPart = "PART_PauseSymbol";
    }
}
