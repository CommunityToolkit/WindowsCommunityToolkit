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

using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Provides attached dependency properties for interacting with the <see cref="ApplicationViewTitleBar"/> on a window (app view).
    /// </summary>
    public static class TitleBar
    {
        /// <summary>
        /// Gets a value indicating whether TitleBar is supported or not.
        /// </summary>
        public static bool IsTitleBarSupported => Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationViewTitleBar");

        /// <summary>
        /// Gets Color for <see cref="ApplicationViewTitleBar.BackgroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns>Color</returns>
        public static Color GetBackgroundColor(Page page)
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
        /// Sets Color to <see cref="ApplicationViewTitleBar.BackgroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value">Color</param>
        public static void SetBackgroundColor(Page page, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.BackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets Color for <see cref="ApplicationViewTitleBar.ButtonBackgroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns>Color</returns>
        public static Color GetButtonBackgroundColor(Page page)
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
        /// Sets Color to <see cref="ApplicationViewTitleBar.ButtonBackgroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value">Color</param>
        public static void SetButtonBackgroundColor(Page page, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ButtonBackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets Color for <see cref="ApplicationViewTitleBar.ButtonForegroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns>Color</returns>
        public static Color GetButtonForegroundColor(Page page)
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
        /// Sets Color to <see cref="ApplicationViewTitleBar.ButtonForegroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value">Color</param>
        public static void SetButtonForegroundColor(Page page, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ButtonForegroundColor = value;
            }
        }

        /// <summary>
        /// Gets Color for <see cref="ApplicationViewTitleBar.ButtonHoverBackgroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns>Color</returns>
        public static Color GetButtonHoverBackgroundColor(Page page)
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
        /// Sets Color to <see cref="ApplicationViewTitleBar.ButtonHoverBackgroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value">Color</param>
        public static void SetButtonHoverBackgroundColor(Page page, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ButtonHoverBackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets Color for <see cref="ApplicationViewTitleBar.ButtonHoverForegroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns>Color</returns>
        public static Color GetButtonHoverForegroundColor(Page page)
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
        /// Sets Color to <see cref="ApplicationViewTitleBar.ButtonHoverForegroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value">Color</param>
        public static void SetButtonHoverForegroundColor(Page page, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ButtonHoverForegroundColor = value;
            }
        }

        /// <summary>
        /// Gets Color for <see cref="ApplicationViewTitleBar.ButtonInactiveBackgroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns>Color</returns>
        public static Color GetButtonInactiveBackgroundColor(Page page)
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
        /// Sets Color to <see cref="ApplicationViewTitleBar.ButtonInactiveBackgroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value">Color</param>
        public static void SetButtonInactiveBackgroundColor(Page page, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ButtonInactiveBackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets Color for <see cref="ApplicationViewTitleBar.ButtonInactiveForegroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns>Color</returns>
        public static Color GetButtonInactiveForegroundColor(Page page)
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
        /// Sets Color to <see cref="ApplicationViewTitleBar.ButtonInactiveForegroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value">Color</param>
        public static void SetButtonInactiveForegroundColor(Page page, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ButtonInactiveForegroundColor = value;
            }
        }

        /// <summary>
        /// Gets Color for <see cref="ApplicationViewTitleBar.ButtonPressedBackgroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns>Color</returns>
        public static Color GetButtonPressedBackgroundColor(Page page)
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
        /// Sets Color to <see cref="ApplicationViewTitleBar.ButtonPressedBackgroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value">Color</param>
        public static void SetButtonPressedBackgroundColor(Page page, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ButtonPressedBackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets Color for <see cref="ApplicationViewTitleBar.ButtonPressedForegroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns>Color</returns>
        public static Color GetButtonPressedForegroundColor(Page page)
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
        /// Sets Color to <see cref="ApplicationViewTitleBar.ButtonPressedForegroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value">Color</param>
        public static void SetButtonPressedForegroundColor(Page page, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ButtonPressedForegroundColor = value;
            }
        }

        /// <summary>
        /// Gets Color for <see cref="ApplicationViewTitleBar.ForegroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns>Color</returns>
        public static Color GetForegroundColor(Page page)
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
        /// Sets Color to <see cref="ApplicationViewTitleBar.ForegroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value">Color</param>
        public static void SetForegroundColor(Page page, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.ForegroundColor = value;
            }
        }

        /// <summary>
        /// Gets Color for <see cref="ApplicationViewTitleBar.InactiveBackgroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns>Color</returns>
        public static Color GetInactiveBackgroundColor(Page page)
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
        /// Sets Color to <see cref="ApplicationViewTitleBar.InactiveBackgroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value">Color</param>
        public static void SetInactiveBackgroundColor(Page page, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.InactiveBackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets Color for <see cref="ApplicationViewTitleBar.InactiveForegroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns>Color</returns>
        public static Color GetInactiveForegroundColor(Page page)
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
        /// Sets Color to <see cref="ApplicationViewTitleBar.InactiveForegroundColor"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value">Color</param>
        public static void SetInactiveForegroundColor(Page page, Color value)
        {
            var titleBar = GetTitleBar();
            if (titleBar != null)
            {
                titleBar.InactiveForegroundColor = value;
            }
        }

        private static ApplicationViewTitleBar GetTitleBar()
        {
            return IsTitleBarSupported ? Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar : null;
        }
    }
}
