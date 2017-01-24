﻿// ******************************************************************
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
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Shapes;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="DropShadowPanel"/> control allows the creation of a DropShadow for any Xaml FrameworkElement in markup
    /// making it easier to add shadows to Xaml without having to directly drop down to Windows.UI.Composition APIs.
    /// </summary>
    [TemplatePart(Name = PartShadow, Type = typeof(Border))]
    public partial class DropShadowPanel : ContentControl
    {
        private const string PartShadow = "ShadowElement";

        private readonly DropShadow _dropShadow;
        private readonly SpriteVisual _shadowVisual;
        private Border _border;

        /// <summary>
        /// Initializes a new instance of the <see cref="DropShadowPanel"/> class.
        /// </summary>
        public DropShadowPanel()
        {
            this.DefaultStyleKey = typeof(DropShadowPanel);

            Compositor compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            _shadowVisual = compositor.CreateSpriteVisual();

            if (IsSupported)
            {
                _dropShadow = compositor.CreateDropShadow();
                _shadowVisual.Shadow = _dropShadow;
            }

            SizeChanged += OnSizeChanged;
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

            _border = GetTemplateChild(PartShadow) as Border;

            if (_border != null)
            {
                ElementCompositionPreview.SetElementChildVisual(_border, _shadowVisual);
            }

            ConfigureShadowVisualForCastingElement();

            base.OnApplyTemplate();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
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
            if (_dropShadow != null)
            {
                _dropShadow.BlurRadius = (float)newValue;
            }
        }

        private void OnColorChanged(Color newValue)
        {
            if (_dropShadow != null)
            {
                _dropShadow.Color = newValue;
            }
        }

        private void OnOffsetXChanged(double newValue)
        {
            if (_dropShadow != null)
            {
                UpdateShadowOffset((float)newValue, _dropShadow.Offset.Y, _dropShadow.Offset.Z);
            }
        }

        private void OnOffsetYChanged(double newValue)
        {
            if (_dropShadow != null)
            {
                UpdateShadowOffset(_dropShadow.Offset.X, (float)newValue, _dropShadow.Offset.Z);
            }
        }

        private void OnOffsetZChanged(double newValue)
        {
            if (_dropShadow != null)
            {
                UpdateShadowOffset(_dropShadow.Offset.X, _dropShadow.Offset.Y, (float)newValue);
            }
        }

        private void OnShadowOpacityChanged(double newValue)
        {
            if (_dropShadow != null)
            {
                _dropShadow.Opacity = (float)newValue;
            }
        }

        private void UpdateShadowMask()
        {
            if (!IsSupported)
            {
                return;
            }

            if (Content != null)
            {
                CompositionBrush mask = null;
                if (Content is Image)
                {
                    mask = ((Image)Content).GetAlphaMask();
                }
                else if (Content is Shape)
                {
                    mask = ((Shape)Content).GetAlphaMask();
                }
                else if (Content is TextBlock)
                {
                    mask = ((TextBlock)Content).GetAlphaMask();
                }

                _dropShadow.Mask = mask;
            }
            else
            {
                _dropShadow.Mask = null;
            }
        }

        private void UpdateShadowOffset(float x, float y, float z)
        {
            if (_dropShadow != null)
            {
                _dropShadow.Offset = new Vector3(x, y, z);
            }
        }

        private void UpdateShadowSize()
        {
            if (_shadowVisual != null)
            {
                Vector2 newSize = new Vector2(0, 0);
                FrameworkElement contentFE = Content as FrameworkElement;
                if (contentFE != null)
                {
                    newSize = new Vector2((float)contentFE.ActualWidth, (float)contentFE.ActualHeight);
                }

                _shadowVisual.Size = newSize;
            }
        }
    }
}
