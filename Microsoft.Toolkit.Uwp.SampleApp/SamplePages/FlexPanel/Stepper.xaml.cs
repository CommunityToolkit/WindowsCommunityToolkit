// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FlexPanelTest
{
    public partial class Stepper : UserControl
    {
        #region Increment Property
        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
            nameof(Increment),
            typeof(double),
            typeof(Stepper),
            new PropertyMetadata(1.0) //, new PropertyChangedCallback((d,e) => ((Stepper)d).OnIncrementChanged(e)))
        );

        public double Increment
        {
            get => (double)GetValue(IncrementProperty);
            set => SetValue(IncrementProperty, value);
        }
        #endregion Increment Property


        #region MinValue Property
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            nameof(MinValue),
            typeof(double),
            typeof(Stepper),
            new PropertyMetadata(0.0, new PropertyChangedCallback((d,e) => ((Stepper)d).OnMinValueChanged(e)))
        );
        protected virtual void OnMinValueChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is double newValue)
                Value = Math.Max(newValue, Value);
        }
        public double MinValue
        {
            get => (double)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }
        #endregion MinValue Property


        #region MaxValue Property
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            nameof(MaxValue),
            typeof(double),
            typeof(Stepper),
            new PropertyMetadata(100.0, new PropertyChangedCallback((d,e) => ((Stepper)d).OnMaxValueChanged(e)))
        );
        protected virtual void OnMaxValueChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is double newValue)
                Value = Math.Min(newValue, Value);
        }
        public double MaxValue
        {
            get => (double)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }
        #endregion MaxValue Property


        #region PressedDateTime Property
        public static readonly DependencyProperty PressedDateTimeProperty = DependencyProperty.RegisterAttached(
            "PressedDateTime",
            typeof(DateTime),
            typeof(Stepper),
            new PropertyMetadata(DateTime.MinValue)
        );
        public static DateTime GetPressedDateTime(FrameworkElement element)
            => (DateTime)element.GetValue(PressedDateTimeProperty);
        public static void SetPressedDateTime(FrameworkElement element, DateTime value)
            => element.SetValue(PressedDateTimeProperty, value);
        #endregion PressedDateTime Property



        #region Value Property


        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(double),
            typeof(Stepper),
            new PropertyMetadata(0.0, new PropertyChangedCallback((d,e) => ((Stepper)d).OnValueChanged(e)))
        );
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            Value = Math.Max(MinValue, Value);
            Value = Math.Min(MaxValue, Value);
            _valueTextBox.Text = Value.ToString();

            if (Value != (double)e.OldValue && Value == (double)e.NewValue)
                ValueChanged?.Invoke(this, Value);
        }
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        #endregion Value Property


        public event EventHandler<double> ValueChanged;

        public Stepper()
        {
            this.InitializeComponent();
        }

        private void Border_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
                border.Background = ((SolidColorBrush)border.Background).WithAlpha(0x18);
        }

        private void Border_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
                Cancel(border);
        }

        private async void Border_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = ((SolidColorBrush)border.Background).WithAlpha(0x80);
                if (border.Child is TextBlock tb)
                {
                    var animation = tb.Scale(0.5f, 0.5f, (float)(tb.ActualWidth * 0.5), (float)(tb.ActualHeight * 0.5), 100);
                    //await Task.Delay(5);
                    SetPressedDateTime(border, DateTime.Now);
                    await animation.StartAsync();
                }
            }
            if (sender == _leftSegment)
                Value -= Increment;
            else if (sender == _rightSegment)
                Value += Increment;
        }

        private void Border_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
                Cancel(border);
        }

        private void Border_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                var point = e.GetCurrentPoint(border);
                var properties = point.Properties;
                var delta = properties.MouseWheelDelta;

                Value += Math.Sign(delta) * Increment;
            }
        }

        private void Border_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
                Cancel(border);
        }

        private void Border_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
                Cancel(border);

        }

        void PressedCancel(Border border)
        {
            var pressed = (DateTime)GetPressedDateTime(border);
            var elapsed = DateTime.Now - pressed;
            var timeSpanRemaining = TimeSpan.FromMilliseconds(200) - elapsed;
            if (timeSpanRemaining <= TimeSpan.Zero)
                Cancel(border);
            else
            {
                UIElementExtensions.StartTimer(timeSpanRemaining, () =>
                {
                    Cancel(border);
                    return false;
                });
            }
        }

        void Cancel(Border border)
        {
            border.Background = ((SolidColorBrush)border.Background).WithAlpha(0x33);
            if (border.Child is TextBlock tb)
            {
                var animation = tb.Scale(1f, 1f, (float)(tb.ActualWidth * 0.5), (float)(tb.ActualHeight * 0.5), 500);
                animation.Start();
            }
        }
    }
}
