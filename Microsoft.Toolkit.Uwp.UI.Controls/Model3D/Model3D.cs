using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public sealed class Model3D : Control
    {
        private WebView _view;

        public event EventHandler AssetLoading;
        public event EventHandler AssetLoaded;

        public Model3D()
        {
            this.DefaultStyleKey = typeof(Model3D);
        }

        protected override void OnApplyTemplate()
        {
            if (_view != null)
            {
                _view.ScriptNotify -= _view_ScriptNotify;
                _view.NavigationCompleted -= _view_NavigationCompleted;
            }

            base.OnApplyTemplate();

            _view = GetTemplateChild("View") as WebView;
            if (_view != null)
            {
                _view.ScriptNotify += _view_ScriptNotify;
                _view.NavigationCompleted += _view_NavigationCompleted;
                _view.Navigate(new Uri("ms-appx-web:///Microsoft.Toolkit.Uwp.UI.Controls/Model3D/BabylonView.html"));
            }

        }

        private void _view_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            SetupSource();
            UpdateCamera();
        }

        private void _view_ScriptNotify(object sender, NotifyEventArgs e)
        {
            if (e.Value == "loading")
            {
                // Loading
                AssetLoading?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                // ready
                _view.Opacity = 1;
                AssetLoaded?.Invoke(this, EventArgs.Empty);
            }
        }

        private async Task SetupSource()
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

        private async Task UpdateCamera()
        {
            if (_view == null)
            {
                return;
            }

            float alpha = GetRadianFromDegree(AlphaInDegrees);
            float beta = GetRadianFromDegree(BetaInDegrees);

            await _view.InvokeScriptAsync("updateCameraPositionValues", new string[] { alpha.ToString(), beta.ToString() });
        }

        private float GetRadianFromDegree(float degrees)
        {
            return degrees / 180f * (float)Math.PI;
        }

        private static void OnCameraValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Model3D).UpdateCamera();
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Model3D).SetupSource();
        }

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(Model3D), new PropertyMetadata(string.Empty, OnSourceChanged));

        public float AlphaInDegrees
        {
            get { return (float)GetValue(AlphaInDegreesProperty); }
            set { SetValue(AlphaInDegreesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AlphaInDegrees.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AlphaInDegreesProperty =
            DependencyProperty.Register("AlphaInDegrees", typeof(float), typeof(Model3D), new PropertyMetadata(90f, OnCameraValueChanged));



        public float BetaInDegrees
        {
            get { return (float)GetValue(BetaInDegreesProperty); }
            set { SetValue(BetaInDegreesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BetaInDegrees.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BetaInDegreesProperty =
            DependencyProperty.Register("BetaInDegrees", typeof(float), typeof(Model3D), new PropertyMetadata(90f, OnCameraValueChanged));
    }
}
