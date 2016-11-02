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

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    [Deprecated("The Blade class has been replaced with the BladeItem class. Please use that going forward", DeprecationType.Deprecate, 1)]
    public class Blade : BladeItem
    {
        /// <summary>
        /// Gets or sets the visual content of this blade
        /// </summary>
        [Deprecated("This property has been replaced with the Content property of the control. It is no longer required to place content within the Element property.", DeprecationType.Deprecate, 1)]
        public UIElement Element
        {
            get { return (UIElement)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Element"/> dependency property.
        /// </summary>
        [Deprecated("This property has been replaced with the Content property of the control. It is no longer required to place content within the Element property.", DeprecationType.Deprecate, 1)]
        public static readonly DependencyProperty ElementProperty = DependencyProperty.Register(nameof(Element), typeof(UIElement), typeof(Blade), new PropertyMetadata(null, OnElementChanged));

        private static void OnElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var blade = (Blade)d;
            blade.Content = e.NewValue;
        }
    }
}
