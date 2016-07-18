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

using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    using System;

    using global::Windows.Foundation;
    using global::Windows.UI.Xaml.Media;

    internal class CarouselSlot : ContentControl
    {
        internal static readonly DependencyProperty ItemClickCommandProperty = DependencyProperty.Register("ItemClickCommand", typeof(ICommand), typeof(CarouselSlot), new PropertyMetadata(null));

        private Storyboard _storyboard;

        internal event EventHandler<object> ItemClick;

        internal CarouselSlot()
        {
            Tapped += OnTapped;
            RenderTransform = new ScaleTransform();
            RenderTransformOrigin = new Point { X = 0.5, Y = 0.5 };
            PointerPressed += OnPointerPressed;
            PointerReleased += OnPointerReleased;
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            RenderTransform.AnimateDoubleProperty("ScaleX", 0.98, 1.0, 150);
            RenderTransform.AnimateDoubleProperty("ScaleY", 0.98, 1.0, 150);
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            RenderTransform.AnimateDoubleProperty("ScaleX", 1.0, 0.98, 150);
            RenderTransform.AnimateDoubleProperty("ScaleY", 1.0, 0.98, 150);
        }

        internal double X { get; set; }

        internal ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        internal void MoveX(double x, double duration = 0)
        {
            if (_storyboard != null)
            {
                _storyboard.Pause();
                _storyboard = null;
            }

            if (duration > 0)
            {
                _storyboard = this.AnimateX(x, duration, new CircleEase());
            }
            else
            {
                this.TranslateX(x);
            }

            X = x;
        }

        private void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (ItemClickCommand != null)
            {
                var contentControl = sender as ContentControl;
                if (contentControl != null)
                {
                    if (ItemClickCommand.CanExecute(contentControl.Content))
                    {
                        ItemClickCommand.Execute(contentControl.Content);
                    }
                }
            }

            ItemClick?.Invoke(this, Content);
        }
    }
}
