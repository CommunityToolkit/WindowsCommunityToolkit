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
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    //
    public delegate void OnClick(object sender, RoutedEventArgs e);

    /// <summary>
    /// The HamburgerButton provides an animated button view between its states
    /// </summary>
    [TemplatePart(Name = "ContainerCanvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "Stripe1", Type = typeof(Rectangle))]
    [TemplatePart(Name = "Stripe2", Type = typeof(Rectangle))]
    [TemplatePart(Name = "Stripe3", Type = typeof(Rectangle))]
    public partial class HamburgerButton : Control
    {
        public OnClick Click { get; set; }

        private Rectangle _stripe1;
        private Rectangle _stripe2;
        private Rectangle _stripe3;
        private Canvas _canvas;
        private double _blockSize;
        private double _stripeWidth;
        private double _stripesTotalHeight;
        private Storyboard _sbMain;

        private bool _isOpen = true;

        private DoubleAnimation _daScale1;
        private DoubleAnimation _daScale2;
        private DoubleAnimation _daScale3;
        private DoubleAnimation _daRot1;
        private DoubleAnimation _daRot2;

        private double _diagScale;

        public HamburgerButton()
        {
            DefaultStyleKey = typeof(HamburgerButton);
            this.PointerPressed += HamburgerButton_PointerPressed;
        }

        private void HamburgerButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ChangeState();
            if (Click != null)
            {
                Click(this, e);
            }
        }

        private void ChangeState()
        {
            if (_isOpen)
            {
                _daScale1.To = _diagScale;
                _daScale2.To = 0;
                _daScale3.To = _diagScale;
                _daRot1.To = 45;
                _daRot2.To = -45;
                _sbMain.Begin();
                _isOpen = false;
            }
            else
            {
                _daScale1.To = 1.0;
                _daScale2.To = 1.0;
                _daScale3.To = 1.0;
                _daRot1.To = 0;
                _daRot2.To = 0;
                _sbMain.Begin();
                _isOpen = true;
            }

        }

        protected override void OnApplyTemplate()
        {
            if (this.Background == null)
            {
                this.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
            }

            // initialize controls from the template
            _canvas = (Canvas)GetTemplateChild("ContainerCanvas");
            _stripe1 = (Rectangle)GetTemplateChild("Stripe1");
            _stripe2 = (Rectangle)GetTemplateChild("Stripe2");
            _stripe3 = (Rectangle)GetTemplateChild("Stripe3");

            // arrange the controls
            _canvas.Width = this.Width;
            _canvas.Height = this.Height;

            // each block size is calculated by dividing the total height by 7
            // 3 stripes + 2 spacing between them + 2 space for the top + 2 space in the bottom
            _blockSize = _canvas.Height / 11;
            _stripeWidth = _canvas.Width - (_blockSize * 4);

            _stripe1.Height = _blockSize;
            _stripe1.Width = _stripeWidth;
            _stripe2.Height = _blockSize;
            _stripe2.Width = _stripeWidth;
            _stripe3.Height = _blockSize;
            _stripe3.Width = _stripeWidth;

            // left padding and right padding is same as the row spacing
            var ctrl = 0;
            Canvas.SetLeft(_stripe1, _blockSize * 2);
            Canvas.SetTop(_stripe1, (_blockSize * ctrl * 2) + (_blockSize * 2));

            ctrl = 1;
            Canvas.SetLeft(_stripe2, _blockSize * 2);
            Canvas.SetTop(_stripe2, (_blockSize * ctrl * 2) + (_blockSize * 2));

            ctrl = 2;
            Canvas.SetLeft(_stripe3, _blockSize * 2);
            Canvas.SetTop(_stripe3, (_blockSize * ctrl * 2) + (_blockSize * 2));

            _stripesTotalHeight = Canvas.GetTop(_stripe3) - Canvas.GetTop(_stripe1);

            base.OnApplyTemplate();

            InitAnimation();
        }

        private void InitAnimation()
        {
            if (_sbMain != null)
            {
                return;
            }

            /*
             Animation Setup - When Closed
             Step 1:
                Stripe 1, Stripe 2, Stripe 3 is scaled to max possible height
            Step 2:
                Stripe 1 and Stripe 3 truns to 45 and -45 degree respectively

            Animation Setup - When Open
            Animation is reversed by changing the angle and scale value to default
             */
            _sbMain = new Storyboard();

            RotateTransform rt1 = new RotateTransform();
            rt1.CenterX = 0;
            rt1.CenterY = _blockSize / 2;

            RotateTransform rt2 = new RotateTransform();
            rt2.CenterX = 0;
            rt2.CenterY = _blockSize / 2;

            var diagSize = (_stripesTotalHeight < _stripeWidth ? _stripesTotalHeight : _stripeWidth) * Math.Sqrt(2);
            _diagScale = diagSize / _stripeWidth;

            ScaleTransform st1 = new ScaleTransform();
            st1.CenterX = 0;
            st1.CenterY = 0;

            ScaleTransform st2 = new ScaleTransform();
            st2.CenterX = 0;
            st2.CenterY = 0;
            _stripe2.RenderTransform = st2;

            ScaleTransform st3 = new ScaleTransform();
            st3.CenterX = 0;
            st3.CenterY = 0;

            var tg1 = new TransformGroup();
            tg1.Children.Add(st1);
            tg1.Children.Add(rt1);

            var tg2 = new TransformGroup();
            tg2.Children.Add(st1);
            tg2.Children.Add(rt2);

            _stripe1.RenderTransform = tg1;
            _stripe3.RenderTransform = tg2;

            var duration = new Duration(TimeSpan.FromSeconds(0.3));
            _daScale1 = new DoubleAnimation();
            _daScale2 = new DoubleAnimation();
            _daScale3 = new DoubleAnimation();
            _daRot1 = new DoubleAnimation();
            _daRot2 = new DoubleAnimation();

            _daScale1.Duration = duration;
            _daScale2.Duration = duration;
            _daScale3.Duration = duration;
            _daRot1.Duration = duration;
            _daRot2.Duration = duration;

            _sbMain.Duration = duration;
            _sbMain.Children.Add(_daScale1);
            _sbMain.Children.Add(_daScale2);
            _sbMain.Children.Add(_daScale3);
            _sbMain.Children.Add(_daRot1);
            _sbMain.Children.Add(_daRot2);

            Storyboard.SetTarget(_daScale1, st1);
            Storyboard.SetTarget(_daScale2, st2);
            Storyboard.SetTarget(_daScale3, st3);
            Storyboard.SetTarget(_daRot1, rt1);
            Storyboard.SetTarget(_daRot2, rt2);

            Storyboard.SetTargetProperty(_daScale1, "ScaleX");
            Storyboard.SetTargetProperty(_daScale2, "ScaleX");
            Storyboard.SetTargetProperty(_daScale3, "ScaleX");
            Storyboard.SetTargetProperty(_daRot1, "Angle");
            Storyboard.SetTargetProperty(_daRot2, "Angle");
        }
    }
}
