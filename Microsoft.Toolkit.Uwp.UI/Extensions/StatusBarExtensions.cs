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
        /// Gets <see cref="Color"/> for <see cref="StatusBar.BackgroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="Color"/></returns>
        public static Color GetBackgroundColor(DependencyObject obj)
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
        /// Sets <see cref="Color"/> to <see cref="StatusBar.BackgroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"><see cref="Color"/></param>
        public static void SetBackgroundColor(DependencyObject obj, Color value)
        {
            var statusBar = GetStatusBar();
            if (statusBar != null)
            {
                statusBar.BackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets <see cref="Color"/> from <see cref="StatusBar.ForegroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="Color"/></returns>
        public static Color GetForegroundColor(DependencyObject obj)
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
        /// Sets <see cref="Color"/> to <see cref="StatusBar.ForegroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"> <see cref="Color"/></param>
        public static void SetForegroundColor(DependencyObject obj, Color value)
        {
            var statusBar = GetStatusBar();
            if (statusBar != null)
            {
                statusBar.ForegroundColor = value;
            }
        }

        /// <summary>
        /// Gets <see cref="double"/> from <see cref="StatusBar.BackgroundOpacity"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="double"/></returns>
        public static double GetBackgroundOpacity(DependencyObject obj)
        {
            return GetStatusBar()?.BackgroundOpacity ?? 0;
        }

        /// <summary>
        /// Sets <see cref="double"/> to <see cref="StatusBar.BackgroundOpacity"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"><see cref="double"/></param>
        public static void SetBackgroundOpacity(DependencyObject obj, double value)
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
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="bool"/></returns>
        public static bool GetIsVisible(DependencyObject obj)
        {
            var height = GetStatusBar()?.OccludedRect.Height ?? 0;
            return height > 0;
        }

        /// <summary>
        /// Sets a <see cref="bool"/> resulting in <see cref="StatusBar"/> becoming visible or invisible.
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/></param>
        /// <param name="value"><see cref="bool"/></param>
        public static async void SetIsVisible(DependencyObject obj, bool value)
        {
            var statusBar = GetStatusBar();
            if (statusBar == null)
            {
                return;
            }

            if (value)
            {
                await StatusBar.GetForCurrentView().ShowAsync();
            }
            else
            {
                await StatusBar.GetForCurrentView().HideAsync();
            }
        }

        private static StatusBar GetStatusBar()
        {
            return IsStatusBarSupported ? StatusBar.GetForCurrentView() : null;
        }
    }
}
