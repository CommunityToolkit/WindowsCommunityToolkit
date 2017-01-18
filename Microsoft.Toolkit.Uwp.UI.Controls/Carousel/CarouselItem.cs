using System;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public sealed class CarouselItem : ContentControl
    {
        public int DistanceFromFocusedCarouselItem { get; internal set; }

        public static readonly DependencyProperty IsInCarouselFocusProperty =
            DependencyProperty.Register("IsInFocus", typeof(bool), typeof(CarouselItem), new PropertyMetadata(false));

        public event EventHandler ItemGotCarouselFocus;

        public event EventHandler ItemLostCarouselFocus;

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

        public bool IsInFocus
        {
            get
            {
                return (bool)GetValue(IsInCarouselFocusProperty);
            }

            internal set
            {
                if (value == IsInFocus)
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
                    ItemGotCarouselFocus?.Invoke(this, null);
                }
                else
                {
                    if (AnimateFocus)
                    {
                        this.Scale(1, 1, (float)this.DesiredSize.Width / 2, (float)this.DesiredSize.Height / 2).Start();
                    }

                    ItemLostCarouselFocus?.Invoke(this, null);
                }

                SetValue(IsInCarouselFocusProperty, value);
            }
        }
    }
}
