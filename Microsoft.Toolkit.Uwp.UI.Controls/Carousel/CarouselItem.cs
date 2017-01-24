using System;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public sealed class CarouselItem : ContentControl
    {
        public event EventHandler CarouselItemCentered;

        public event EventHandler CarouselItemNotCentered;

        public event EventHandler<CarouselItemLocationChangedEventArgs> CarouselItemLocationChanged;

        public CarouselItem()
        {
            this.DefaultStyleKey = typeof(CarouselItem);
            IsTabStop = false;
        }

        public bool IsActionable
        {
            get { return (bool)GetValue(IsActionableProperty); }
            set { SetValue(IsActionableProperty, value); }
        }

        public static readonly DependencyProperty IsActionableProperty =
            DependencyProperty.Register("IsActionable", typeof(bool), typeof(CarouselItem), new PropertyMetadata(true));

        public bool AnimateFocus
        {
            get { return (bool)GetValue(AnimateFocusProperty); }
            set { SetValue(AnimateFocusProperty, value); }
        }

        public static readonly DependencyProperty AnimateFocusProperty =
            DependencyProperty.Register("AnimateFocus", typeof(bool), typeof(CarouselItem), new PropertyMetadata(true));

        private bool _isCentered = false;

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
                    if (AnimateFocus)
                    {
                        this.Scale(1.2f, 1.2f, (float)this.DesiredSize.Width / 2, (float)this.DesiredSize.Height / 2).Start();
                    }

                    ElementSoundPlayer.Play(ElementSoundKind.Focus);
                    CarouselItemCentered?.Invoke(this, null);
                }
                else
                {
                    if (AnimateFocus)
                    {
                        this.Scale(1, 1, (float)this.DesiredSize.Width / 2, (float)this.DesiredSize.Height / 2).Start();
                    }

                    CarouselItemNotCentered?.Invoke(this, null);
                }

                _isCentered = value;
            }
        }

        private int _carouselItemLocation;

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
    }
}
