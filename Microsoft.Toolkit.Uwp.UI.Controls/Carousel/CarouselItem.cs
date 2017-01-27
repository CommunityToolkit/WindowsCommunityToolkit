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
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Control used by <see cref="Carousel"/> to represent each item
    /// </summary>
    public sealed class CarouselItem : ContentControl
    {
        private bool _isCentered = false;
        private int _carouselItemLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarouselItem"/> class.
        /// </summary>
        public CarouselItem()
        {
            this.DefaultStyleKey = typeof(CarouselItem);
            IsTabStop = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Carousel" />
        /// ItemInvoked event is raised for this item
        /// </summary>
        public bool IsActionable
        {
            get { return (bool)GetValue(IsActionableProperty); }
            set { SetValue(IsActionableProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the default animation should
        /// be used when item is centered or not
        /// </summary>
        public bool DefaultAnimationEnabled
        {
            get { return (bool)GetValue(DefaultAnimationEnabledProperty); }
            set { SetValue(DefaultAnimationEnabledProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the item is centered in the <see cref="Carousel"/>.
        /// </summary>
        public bool IsCentered
        {
            get
            {
                return _isCentered;
            }

            internal set
            {
                if (value == IsCentered)
                {
                    return;
                }

                if (value)
                {
                    ElementSoundPlayer.Play(ElementSoundKind.Focus);
                    CarouselItemCentered?.Invoke(this, null);
                }
                else
                {
                    CarouselItemNotCentered?.Invoke(this, null);
                }

                _isCentered = value;
                RunAnimation();
            }
        }

        /// <summary>
        /// Gets a value indicating how far this item is from the center
        /// </summary>
        public int CarouselItemLocation
        {
            get
            {
                return _carouselItemLocation;
            }

            internal set
            {
                if (_carouselItemLocation == value)
                {
                    return;
                }

                var eventArgs = new CarouselItemLocationChangedEventArgs()
                {
                    OldValue = _carouselItemLocation,
                    NewValue = value
                };

                _carouselItemLocation = value;
                CarouselItemLocationChanged?.Invoke(this, eventArgs);
            }
        }

        internal bool RunAnimation()
        {
            if (DefaultAnimationEnabled)
            {
                var scale = IsCentered ? 1.2f : 1f;
                this.Scale(scale, scale, (float)this.DesiredSize.Width / 2, (float)this.DesiredSize.Height / 2).Start();
            }

            return DefaultAnimationEnabled;
        }

        /// <summary>
        /// Identifies the <see cref="IsActionable"/> property
        /// </summary>
        public static readonly DependencyProperty IsActionableProperty =
            DependencyProperty.Register(nameof(IsActionable), typeof(bool), typeof(CarouselItem), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="DefaultAnimationEnabled"/> property
        /// </summary>
        public static readonly DependencyProperty DefaultAnimationEnabledProperty =
            DependencyProperty.Register(nameof(DefaultAnimationEnabled), typeof(bool), typeof(CarouselItem), new PropertyMetadata(true));

        /// <summary>
        /// Occurs when item has been centered
        /// </summary>
        public event EventHandler CarouselItemCentered;

        /// <summary>
        /// Occurs when item is no longer centered
        /// </summary>
        public event EventHandler CarouselItemNotCentered;

        /// <summary>
        /// Occurs when item location changes
        /// </summary>
        public event EventHandler<CarouselItemLocationChangedEventArgs> CarouselItemLocationChanged;
    }
}
