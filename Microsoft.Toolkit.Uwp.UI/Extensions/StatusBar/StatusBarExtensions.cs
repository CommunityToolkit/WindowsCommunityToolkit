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
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Provides attached dependency properties for interacting with the <see cref="StatusBar"/> on a window (app view).
    /// </summary>
    public static class StatusBarExtensions
    {
        /// <summary>
        /// Gets a value indicating whether StatusBar is supported or not.
        /// </summary>
        public static bool IsStatusBarSupported => Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar");

        /// <summary>
        /// Gets Color for StatusBar.BackgroundColor
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns>Color</returns>
        public static Color GetBackgroundColor(Page page)
        {
            Color color;

            var statusBar = GetStatusBar();
            if (statusBar != null)
            {
                color = statusBar.BackgroundColor.GetValueOrDefault();
            }

            return color;
        }

        /// <summary>
        /// Sets Color to StatusBar.BackgroundColor
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value">Color</param>
        public static void SetBackgroundColor(Page page, Color value)
        {
            var statusBar = GetStatusBar();
            if (statusBar != null)
            {
                statusBar.BackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets Color from StatusBar.ForegroundColor
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns>Color</returns>
        public static Color GetForegroundColor(Page page)
        {
            Color color;

            var statusBar = GetStatusBar();
            if (statusBar != null)
            {
                color = statusBar.ForegroundColor.GetValueOrDefault();
            }

            return color;
        }

        /// <summary>
        /// Sets Color to StatusBar.ForegroundColor
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value"> Color</param>
        public static void SetForegroundColor(Page page, Color value)
        {
            var statusBar = GetStatusBar();
            if (statusBar != null)
            {
                statusBar.ForegroundColor = value;
            }
        }

        /// <summary>
        /// Gets <see cref="double"/> from StatusBar.BackgroundOpacity
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns><see cref="double"/></returns>
        public static double GetBackgroundOpacity(Page page)
        {
            return GetStatusBar()?.BackgroundOpacity ?? 0;
        }

        /// <summary>
        /// Sets <see cref="double"/> to StatusBar.BackgroundOpacity
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value"><see cref="double"/></param>
        public static void SetBackgroundOpacity(Page page, double value)
        {
            var statusBar = GetStatusBar();
            if (statusBar != null)
            {
                statusBar.BackgroundOpacity = value;
            }
        }

        /// <summary>
        /// Gets <see cref="bool"/> indicating whether <see cref="StatusBar"/> is visible or not.
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns><see cref="bool"/></returns>
        public static bool GetIsVisible(Page page)
        {
            var statusBar = GetStatusBar();

            return statusBar?.OccludedRect.Height > 0;
        }

        /// <summary>
        /// Sets a <see cref="bool"/> resulting in <see cref="StatusBar"/> becoming visible or invisible.
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value"><see cref="bool"/></param>
        public static void SetIsVisible(Page page, bool value)
        {
            page.SetValue(IsVisibleProperty, value);
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for IsVisible.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.RegisterAttached("IsVisible", typeof(bool), typeof(StatusBarExtensions), new PropertyMetadata(true, OnIsVisibleChanged));

        private static async void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var statusBar = GetStatusBar();

            if (statusBar == null)
            {
                return;
            }

            bool isVisible = (bool)e.NewValue;

            if (isVisible)
            {
                await statusBar.ShowAsync();
            }
            else
            {
                await statusBar.HideAsync();
            }
        }

        private static Windows.UI.ViewManagement.StatusBar GetStatusBar()
        {
            return IsStatusBarSupported ? Windows.UI.ViewManagement.StatusBar.GetForCurrentView() : null;
        }
    }
}
