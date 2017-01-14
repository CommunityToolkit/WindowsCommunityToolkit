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

using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="DropShadowPanel"/> control allows the creation of a DropShadow for any Xaml FrameworkElement in markup
    /// making it easier to add shadows to Xaml without having to directly drop down to Windows.UI.Composition APIs.
    /// </summary>
    [TemplatePart(Name = PartShadow, Type = typeof(Border))]
    [TemplatePart(Name = PartContent, Type = typeof(ContentPresenter))]
    public partial class DropShadowPanel : ContentControl
    {
        private const string PartShadow = "ShadowElement";
        private const string PartContent = "CastingElement";

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

            SizeChanged += CompositionShadow_SizeChanged;

            Loaded += CompositionShadow_Loaded;
        }

        /// <summary>
        /// Update the visual state of the control when its template is changed.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            _border = GetTemplateChild(PartShadow) as Border;

            if (_border != null)
            {
                ElementCompositionPreview.SetElementChildVisual(_border, _shadowVisual);
            }

            base.OnApplyTemplate();
        }
    }
}
