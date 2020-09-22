// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace FlexPanelTest
{
    static class UIElementExtensions
    {
        public static bool IsVisible(this UIElement uIElement)
            => uIElement.Visibility == Visibility.Visible;

        public static bool SetIsVisible(this UIElement uiElement, bool isVisible)
        {
            uiElement.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            return uiElement.IsVisible();
        }

        public static void ToggleVisibility(this UIElement uIElement)
            => uIElement.SetIsVisible(!uIElement.IsVisible());


        public static Color WithAlpha(this Color color, double alpha)
            => color.WithAlpha((byte)(255 * alpha));

        public static Color WithAlpha(this Color color, int alpha)
            => color.WithAlpha((byte)(alpha));


        public static Color WithAlpha(this Color color, byte alpha)
        {
            var newColor = color;
            newColor.A = alpha;
            return newColor;
        }

        public static SolidColorBrush WithAlpha(this SolidColorBrush brush, byte alpha)
        {
            var newColor = brush.Color.WithAlpha(alpha);
            return new SolidColorBrush(newColor);
        }

        public static SolidColorBrush WithAlpha(this SolidColorBrush brush, int alpha)
             => brush.WithAlpha((byte)(alpha));

        public static SolidColorBrush WithAlpha(this SolidColorBrush brush, double alpha)
            => brush.WithAlpha((byte)(255 * alpha));

        public static void StartTimer(TimeSpan interval, Func<bool> callback)
        {
            var timerTick = 0L;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            void renderingFrameEventHandler(object sender, object args)
            {
                var newTimerTick = stopWatch.ElapsedMilliseconds / (long)interval.TotalMilliseconds;
                if (newTimerTick == timerTick)
                    return;
                timerTick = newTimerTick;
                bool result = callback();
                if (result)
                    return;
                CompositionTarget.Rendering -= renderingFrameEventHandler;
            }
            CompositionTarget.Rendering += renderingFrameEventHandler;
        }

    }

}
