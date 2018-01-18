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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents the container for an item in a Carousel control.
    /// </summary>
    public class CarouselItem : SelectorItem
    {
        private const string PointerOverState = "PointerOver";
        private const string PointerOverSelectedState = "PointerOverSelected";
        private const string PressedState = "Pressed";
        private const string PressedSelectedState = "PressedSelected";
        private const string SelectedState = "Selected";
        private const string NormalState = "Normal";

        /// <summary>
        /// Initializes a new instance of the <see cref="CarouselItem"/> class.
        /// </summary>
        public CarouselItem()
        {
            // Set style
            DefaultStyleKey = typeof(CarouselItem);

            RegisterPropertyChangedCallback(SelectorItem.IsSelectedProperty, OnIsSelectedChanged);
        }

        /// <inheritdoc/>
        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);

            VisualStateManager.GoToState(this, IsSelected ? PointerOverSelectedState : PointerOverState, true);
        }

        /// <inheritdoc/>
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);

            VisualStateManager.GoToState(this, IsSelected ? SelectedState : NormalState, true);
        }

        /// <inheritdoc/>
        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);

            VisualStateManager.GoToState(this, IsSelected ? PressedSelectedState : PressedState, true);
        }

        internal event EventHandler Selected;

        private void OnIsSelectedChanged(DependencyObject sender, DependencyProperty dp)
        {
            var item = (CarouselItem)sender;

            if (item.IsSelected)
            {
                Selected?.Invoke(this, EventArgs.Empty);
                VisualStateManager.GoToState(item, SelectedState, true);
            }
            else
            {
                VisualStateManager.GoToState(item, NormalState, true);
            }
        }
    }
}