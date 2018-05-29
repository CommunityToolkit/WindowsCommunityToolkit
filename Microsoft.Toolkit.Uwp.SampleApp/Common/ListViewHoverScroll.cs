// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public static class ListViewHoverScroll
    {
        private static double _threshold = 100;
        private static double _rate = 0;
        private static Windows.UI.Xaml.Controls.ListViewBase _instance;

        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(Windows.UI.Xaml.Controls.ListViewBase), new PropertyMetadata(false, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listView = d as Windows.UI.Xaml.Controls.ListViewBase;

            if (listView == null)
            {
                return;
            }

            listView.PointerMoved += ListView_PointerMoved;
            listView.PointerCaptureLost += ListView_PointerCaptureLost;
            listView.PointerEntered += ListView_PointerEntered;
            listView.PointerExited += ListView_PointerExited;
        }

        private static async Task ScrollListView()
        {
            var scrollViewer = _instance?.FindDescendant<ScrollViewer>();
            if (scrollViewer == null)
            {
                return;
            }

            while (_instance != null && _rate != 0)
            {
                scrollViewer.ChangeView(scrollViewer.HorizontalOffset + _rate, null, null);
                await Task.Delay(50);
            }
        }

        private static void ListView_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _instance = null;
            _rate = 0;
        }

        private static void ListView_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _instance = sender as Windows.UI.Xaml.Controls.ListViewBase;

            if (e.Pointer.PointerDeviceType != Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                return;
            }

            var position = e.GetCurrentPoint(_instance).Position;

            if (position.X < _threshold)
            {
                _rate = position.X - _threshold;
                var t = ScrollListView();
            }
            else if (_instance.ActualWidth - position.X < _threshold)
            {
                _rate = _threshold - (_instance.ActualWidth - position.X);
                var t = ScrollListView();
            }
        }

        private static void ListView_PointerCaptureLost(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _instance = null;
            _rate = 0;
        }

        private static void ListView_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _instance = sender as Windows.UI.Xaml.Controls.ListViewBase;

            if (e.Pointer.PointerDeviceType != Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                return;
            }

            var position = e.GetCurrentPoint(_instance).Position;
            var oldRate = _rate;

            if (position.X < _threshold)
            {
                _rate = position.X - _threshold;
            }
            else if (_instance.ActualWidth - position.X < _threshold)
            {
                _rate = _threshold - (_instance.ActualWidth - position.X);
            }
            else
            {
                _rate = 0;
            }

            if (oldRate == 0 && _rate != 0)
            {
                var t = ScrollListView();
            }
        }
    }
}
