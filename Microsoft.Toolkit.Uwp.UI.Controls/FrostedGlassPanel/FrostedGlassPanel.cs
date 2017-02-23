// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="FrostedGlassPanel"/> control allows user to create layout control that enables tint and blur
    /// </summary>
    [TemplatePart(Name = GlassHostElementName, Type = typeof(Border))]
    public partial class FrostedGlassPanel : ContentControl
    {
        private const string GlassHostElementName = "GlassHostElement";

        private Compositor _compositor;
        private CompositionBrush _backdropBrush;
        private GaussianBlurEffect _glassEffect;
        private SpriteVisual _glassVisual;
        private Border _border;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrostedGlassPanel"/> class.
        /// </summary>
        public FrostedGlassPanel()
        {
            DefaultStyleKey = typeof(FrostedGlassPanel);

            Loaded += OnLoaded;
        }

        /// <summary>
        /// Update the visual state of the control when its template is changed.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            if (!IsSupported)
            {
                return;
            }

            _border = GetTemplateChild(GlassHostElementName) as Border;

            base.OnApplyTemplate();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            var hostVisual = ElementCompositionPreview.GetElementVisual(_border);
            _compositor = hostVisual.Compositor;

            // Create a glass effect, requires Win2D NuGet package
            CreateGlassEffect();

            // Create a Visual to contain the frosted glass effect
            _glassVisual = _compositor.CreateSpriteVisual();

            // Create the glass brush to apply the glass effect
            _backdropBrush = _compositor.CreateBackdropBrush();
            UpdateGlassBrush();

            // Add the blur as a child of the host in the visual tree
            ElementCompositionPreview.SetElementChildVisual(_border, _glassVisual);

            // Make sure size of glass host and glass visual always stay in sync
            var bindSizeAnimation = _compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);

            _glassVisual.StartAnimation("Size", bindSizeAnimation);
        }

        private void CreateGlassEffect()
        {
            // Create a glass effect, requires Win2D NuGet package
            _glassEffect = new GaussianBlurEffect
            {
                BlurAmount = (float)BlurAmount,
                BorderMode = EffectBorderMode.Hard,
                Source = new ArithmeticCompositeEffect
                {
                    MultiplyAmount = 0,
                    Source1Amount = (float)Transparency,
                    Source2Amount = 1 - (float)Transparency,
                    Source1 = new CompositionEffectSourceParameter("backdropBrush"),
                    Source2 = new ColorSourceEffect
                    {
                        Color = ((SolidColorBrush)Brush).Color
                    }
                }
            };
        }

        private void UpdateGlassBrush()
        {
            // Create an instance of the effect and set its source to a CompositionBackdropBrush
            var effectFactory = _compositor.CreateEffectFactory(_glassEffect);
            var effectBrush = effectFactory.CreateBrush();

            effectBrush.SetSourceParameter("backdropBrush", _backdropBrush);

            _glassVisual.Brush = effectBrush;
        }

        private void OnBlurAmountChanged(double newValue)
        {
            if (_glassEffect == null)
            {
                return;
            }

            CreateGlassEffect();
            UpdateGlassBrush();
        }

        private void OnBrushChanged(Brush newValue)
        {
            if (_glassEffect == null)
            {
                return;
            }

            CreateGlassEffect();
            UpdateGlassBrush();
        }

        private void OnTransparencyChanged(double newValue)
        {
            if (_glassEffect == null)
            {
                return;
            }

            CreateGlassEffect();
            UpdateGlassBrush();
        }
    }
}
