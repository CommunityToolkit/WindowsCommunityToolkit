using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Provides attached dependency properties for interacting with the <see cref="ApplicationViewTitleBar"/> on a window (app view).
    /// </summary>
    public static class TitleBarExtensions
    {
        /// <summary>
        /// Gets a value indicating whether TitleBar is supported or not.
        /// </summary>
        public static bool IsTitleBarSupported => Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationViewTitleBar");

        /// <summary>
        /// Gets <see cref="Color"/> for <see cref="ApplicationViewTitleBar.BackgroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="Color"/></returns>
        public static Color GetBackgroundColor(DependencyObject obj)
        {
            Color color;

            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                color = titleBar.BackgroundColor.GetValueOrDefault();
            }

            return color;
        }

        /// <summary>
        /// Sets <see cref="Color"/> to <see cref="ApplicationViewTitleBar.BackgroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"><see cref="Color"/></param>
        public static void SetBackgroundColor(DependencyObject obj, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.BackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets <see cref="Color"/> for <see cref="ApplicationViewTitleBar.ButtonBackgroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="Color"/></returns>
        public static Color GetButtonBackgroundColor(DependencyObject obj)
        {
            Color color;

            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                color = titleBar.ButtonBackgroundColor.GetValueOrDefault();
            }

            return color;
        }

        /// <summary>
        /// Sets <see cref="Color"/> to <see cref="StaApplicationViewTitleBartusBar.ButtonBackgroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"><see cref="Color"/></param>
        public static void SetButtonBackgroundColor(DependencyObject obj, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ButtonBackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets <see cref="Color"/> for <see cref="ApplicationViewTitleBar.ButtonForegroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="Color"/></returns>
        public static Color GetButtonForegroundColor(DependencyObject obj)
        {
            Color color;

            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                color = titleBar.ButtonForegroundColor.GetValueOrDefault();
            }

            return color;
        }

        /// <summary>
        /// Sets <see cref="Color"/> to <see cref="ApplicationViewTitleBar.ButtonForegroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"><see cref="Color"/></param>
        public static void SetButtonForegroundColor(DependencyObject obj, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ButtonForegroundColor = value;
            }
        }

        /// <summary>
        /// Gets <see cref="Color"/> for <see cref="ApplicationViewTitleBar.ButtonHoverBackgroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="Color"/></returns>
        public static Color GetButtonHoverBackgroundColor(DependencyObject obj)
        {
            Color color;

            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                color = titleBar.ButtonHoverBackgroundColor.GetValueOrDefault();
            }

            return color;
        }

        /// <summary>
        /// Sets <see cref="Color"/> to <see cref="ApplicationViewTitleBar.ButtonHoverBackgroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"><see cref="Color"/></param>
        public static void SetButtonHoverBackgroundColor(DependencyObject obj, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ButtonHoverBackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets <see cref="Color"/> for <see cref="ApplicationViewTitleBar.ButtonHoverForegroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="Color"/></returns>
        public static Color GetButtonHoverForegroundColor(DependencyObject obj)
        {
            Color color;

            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                color = titleBar.ButtonHoverForegroundColor.GetValueOrDefault();
            }

            return color;
        }

        /// <summary>
        /// Sets <see cref="Color"/> to <see cref="ApplicationViewTitleBar.ButtonHoverForegroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"><see cref="Color"/></param>
        public static void SetButtonHoverForegroundColor(DependencyObject obj, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ButtonHoverForegroundColor = value;
            }
        }

        /// <summary>
        /// Gets <see cref="Color"/> for <see cref="ApplicationViewTitleBar.ButtonInactiveBackgroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="Color"/></returns>
        public static Color GetButtonInactiveBackgroundColor(DependencyObject obj)
        {
            Color color;

            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                color = titleBar.ButtonInactiveBackgroundColor.GetValueOrDefault();
            }

            return color;
        }

        /// <summary>
        /// Sets <see cref="Color"/> to <see cref="ApplicationViewTitleBar.ButtonInactiveBackgroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"><see cref="Color"/></param>
        public static void SetButtonInactiveBackgroundColor(DependencyObject obj, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ButtonInactiveBackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets <see cref="Color"/> for <see cref="ApplicationViewTitleBar.ButtonInactiveForegroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="Color"/></returns>
        public static Color GetButtonInactiveForegroundColor(DependencyObject obj)
        {
            Color color;

            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                color = titleBar.ButtonInactiveForegroundColor.GetValueOrDefault();
            }

            return color;
        }

        /// <summary>
        /// Sets <see cref="Color"/> to <see cref="ApplicationViewTitleBar.ButtonInactiveForegroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"><see cref="Color"/></param>
        public static void SetButtonInactiveForegroundColor(DependencyObject obj, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ButtonInactiveForegroundColor = value;
            }
        }

        /// <summary>
        /// Gets <see cref="Color"/> for <see cref="ApplicationViewTitleBar.ButtonPressedBackgroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="Color"/></returns>
        public static Color GetButtonPressedBackgroundColor(DependencyObject obj)
        {
            Color color;

            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                color = titleBar.ButtonPressedBackgroundColor.GetValueOrDefault();
            }

            return color;
        }

        /// <summary>
        /// Sets <see cref="Color"/> to <see cref="ApplicationViewTitleBar.ButtonPressedBackgroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"><see cref="Color"/></param>
        public static void SetButtonPressedBackgroundColor(DependencyObject obj, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ButtonPressedBackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets <see cref="Color"/> for <see cref="ApplicationViewTitleBar.ButtonPressedForegroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="Color"/></returns>
        public static Color GetButtonPressedForegroundColor(DependencyObject obj)
        {
            Color color;

            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                color = titleBar.ButtonPressedForegroundColor.GetValueOrDefault();
            }

            return color;
        }

        /// <summary>
        /// Sets <see cref="Color"/> to <see cref="ApplicationViewTitleBar.ButtonPressedForegroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"><see cref="Color"/></param>
        public static void SetButtonPressedForegroundColor(DependencyObject obj, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ButtonPressedForegroundColor = value;
            }
        }

        /// <summary>
        /// Gets <see cref="Color"/> for <see cref="ApplicationViewTitleBar.ForegroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="Color"/></returns>
        public static Color GetForegroundColor(DependencyObject obj)
        {
            Color color;

            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                color = titleBar.ForegroundColor.GetValueOrDefault();
            }

            return color;
        }

        /// <summary>
        /// Sets <see cref="Color"/> to <see cref="ApplicationViewTitleBar.ForegroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"><see cref="Color"/></param>
        public static void SetForegroundColor(DependencyObject obj, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ForegroundColor = value;
            }
        }

        /// <summary>
        /// Gets <see cref="Color"/> for <see cref="ApplicationViewTitleBar.InactiveBackgroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="Color"/></returns>
        public static Color GetInactiveBackgroundColor(DependencyObject obj)
        {
            Color color;

            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                color = titleBar.InactiveBackgroundColor.GetValueOrDefault();
            }

            return color;
        }

        /// <summary>
        /// Sets <see cref="Color"/> to <see cref="ApplicationViewTitleBar.InactiveBackgroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"><see cref="Color"/></param>
        public static void SetInactiveBackgroundColor(DependencyObject obj, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.InactiveBackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets <see cref="Color"/> for <see cref="ApplicationViewTitleBar.InactiveForegroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="Color"/></returns>
        public static Color GetInactiveForegroundColor(DependencyObject obj)
        {
            Color color;

            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                color = titleBar.InactiveForegroundColor.GetValueOrDefault();
            }

            return color;
        }

        /// <summary>
        /// Sets <see cref="Color"/> to <see cref="ApplicationViewTitleBar.InactiveForegroundColor"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"><see cref="Color"/></param>
        public static void SetInactiveForegroundColor(DependencyObject obj, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.InactiveForegroundColor = value;
            }
        }

        private static ApplicationViewTitleBar GetTitleBar()
        {
            return IsTitleBarSupported ? ApplicationView.GetForCurrentView().TitleBar : null;
        }
    }
}
