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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="DropShadowPanel"/> control allows the creation of a DropShadow for any Xaml FrameworkElement in markup
    /// making it easier to add shadows to Xaml without having to directly drop down to Windows.UI.Composition APIs.
    /// </summary>
    [TemplatePart(Name = PartShadow, Type = typeof(Border))]
    [TemplatePart(Name = PartContent, Type = typeof(ContentPresenter))]
    [ContentProperty(Name = PartContent)]
    public partial class DropShadowPanel : Control
    {
        private const string PartShadow = "ShadowElement";
        private const string PartContent = "CastingElement";

        private readonly DropShadow _dropShadow;
        private readonly SpriteVisual _shadowVisual;
        private Border _border;
        private ContentPresenter _contentPresenter;
        private FrameworkElement _contentElement;

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
        }

        /// <summary>
        /// Update the visual state of the control when its template is changed.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            _border = GetTemplateChild(PartShadow) as Border;
            _contentPresenter = GetTemplateChild(PartContent) as ContentPresenter;

            var contentBinding = new Binding()
            {
                Source = this,
                Path = new PropertyPath("CastingElement"),
                Mode = BindingMode.OneWay
            };

            _contentPresenter.SetBinding(ContentPresenter.ContentProperty, contentBinding);

            SizeChanged -= CompositionShadow_SizeChanged;
            SizeChanged += CompositionShadow_SizeChanged;

            Loaded -= CompositionShadow_Loaded;
            Loaded += CompositionShadow_Loaded;

            ElementCompositionPreview.SetElementChildVisual(_border, _shadowVisual);

            base.OnApplyTemplate();
        }
    }
}
