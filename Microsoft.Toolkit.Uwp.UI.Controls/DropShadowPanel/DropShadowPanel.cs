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

using System.Numerics;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Shapes;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="DropShadowPanel"/> control allows the creation of a DropShadow for any Xaml FrameworkElement in markup
    /// making it easier to add shadows to Xaml without having to directly drop down to Windows.UI.Composition APIs.
    /// </summary>
    [ContentProperty(Name = nameof(CastingElement))]
    public sealed partial class DropShadowPanel : UserControl
    {
        private readonly DropShadow _dropShadow;
        private readonly SpriteVisual _shadowVisual;
        private FrameworkElement _contentElement;

        /// <summary>
        /// Gets a value indicating whether the platform supports drop shadows.
        /// </summary>
        /// <remarks>
        /// On platforms not supporting drop shadows, this control has no effect.
        /// </remarks>
        public static bool IsSupported =>
            ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3); // SDK >= 14393

        /// <summary>
        /// Initializes a new instance of the <see cref="DropShadowPanel"/> class.
        /// </summary>
        public DropShadowPanel()
        {
            InitializeComponent();
            DefaultStyleKey = typeof(CompositionShadow);

            SizeChanged += CompositionShadow_SizeChanged;
            Loaded += (object sender, RoutedEventArgs e) =>
            {
                ConfigureShadowVisualForCastingElement();
            };

            Compositor compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            _shadowVisual = compositor.CreateSpriteVisual();

            if (IsShadowSupported)
            {
                _dropShadow = compositor.CreateDropShadow();
                _shadowVisual.Shadow = _dropShadow;
            }

            ElementCompositionPreview.SetElementChildVisual(ShadowElement, _shadowVisual);
        }

        public FrameworkElement CastingElement
        {
            get
            {
                return _contentElement;
            }

            set
            {
                if (_contentElement != null)
                {
                    _contentElement.SizeChanged -= CompositionShadow_SizeChanged;
                }

                _contentElement = value;
                _contentElement.SizeChanged += CompositionShadow_SizeChanged;

                ConfigureShadowVisualForCastingElement();
            }
        }

        /// <summary>
        /// Gets DropShadow. Exposes the underlying composition object to allow custom Windows.UI.Composition animations.
        /// </summary>
        public DropShadow DropShadow => _dropShadow;

        /// <summary>
        /// Gets or sets the mask of the underlying <see cref="Windows.UI.Composition.DropShadow"/>.
        /// Allows for a custom <see cref="Windows.UI.Composition.CompositionBrush"/> to be set.
        /// </summary>
        public CompositionBrush Mask
        {
            get
            {
                return _dropShadow.Mask;
            }

            set
            {
                _dropShadow.Mask = value;
            }
        }

        private void CompositionShadow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateShadowSize();
        }

        private void ConfigureShadowVisualForCastingElement()
        {
            UpdateShadowMask();

            UpdateShadowSize();
        }

        private void OnBlurRadiusChanged(double newValue)
        {
            _dropShadow.BlurRadius = (float)newValue;
        }

        private void OnColorChanged(Color newValue)
        {
            _dropShadow.Color = newValue;
        }

        private void OnOffsetXChanged(double newValue)
        {
            UpdateShadowOffset((float)newValue, _dropShadow.Offset.Y, _dropShadow.Offset.Z);
        }

        private void OnOffsetYChanged(double newValue)
        {
            UpdateShadowOffset(_dropShadow.Offset.X, (float)newValue, _dropShadow.Offset.Z);
        }

        private void OnOffsetZChanged(double newValue)
        {
            UpdateShadowOffset(_dropShadow.Offset.X, _dropShadow.Offset.Y, (float)newValue);
        }

        private void OnShadowOpacityChanged(double newValue)
        {
            _dropShadow.Opacity = (float)newValue;
        }

        private void UpdateShadowMask()
        {
            if (IsShadowSupported)
            {
                if (_contentElement != null)
                {
                    CompositionBrush mask = null;
                    if (_contentElement is Image)
                    {
                        mask = ((Image)_contentElement).GetAlphaMask();
                    }
                    else if (_contentElement is Shape)
                    {
                        mask = ((Shape)_contentElement).GetAlphaMask();
                    }
                    else if (_contentElement is TextBlock)
                    {
                        mask = ((TextBlock)_contentElement).GetAlphaMask();
                    }

                    _dropShadow.Mask = mask;
                }
                else
                {
                    _dropShadow.Mask = null;
                }
            }
        }

        private void UpdateShadowOffset(float x, float y, float z)
        {
            _dropShadow.Offset = new Vector3(x, y, z);
        }

        private void UpdateShadowSize()
        {
            Vector2 newSize = new Vector2(0, 0);
            if (_contentElement != null)
            {
                newSize = new Vector2((float)_contentElement.ActualWidth, (float)_contentElement.ActualHeight);
            }

            _shadowVisual.Size = newSize;
        }
    }
}
