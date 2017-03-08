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

using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Provides attached properties for interacting with the <see cref="Windows.UI.ViewManagement.ApplicationView"/> on a window (app view).
    /// </summary>
    public static class ApplicationView
    {
        /// <summary>
        /// Gets <see cref="string"/> for <see cref="Windows.UI.ViewManagement.ApplicationView.Title"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="string"/></returns>
        public static string GetTitle(DependencyObject obj)
        {
            var applicationView = GetApplicationView();

            return applicationView?.Title ?? string.Empty;
        }

        /// <summary>
        /// Sets <see cref="string"/> to <see cref="Windows.UI.ViewManagement.ApplicationView.Title"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"><see cref="string"/></param>
        public static void SetTitle(DependencyObject obj, string value)
        {
            var applicationView = GetApplicationView();
            if (applicationView != null)
            {
                applicationView.Title = value;
            }
        }

        /// <summary>
        /// Gets <see cref="bool"/> for <see cref="CoreApplicationView.TitleBar.ExtendViewIntoTitleBar"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="string"/></returns>
        public static bool GetExtendViewIntoTitleBar(DependencyObject obj)
        {
            var applicationView = GetCoreApplicationView();

            return applicationView?.TitleBar?.ExtendViewIntoTitleBar ?? false;
        }

        /// <summary>
        /// Sets <see cref="bool"/> to <see cref="CoreApplicationView.TitleBar.ExtendViewIntoTitleBar"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"><see cref="bool"/></param>
        public static void SetExtendViewIntoTitleBar(DependencyObject obj, bool value)
        {
            var applicationView = GetCoreApplicationView();
            if (applicationView != null && applicationView.TitleBar != null)
            {
                applicationView.TitleBar.ExtendViewIntoTitleBar = value;
            }
        }

        /// <summary>
        /// Gets <see cref="AppViewBackButtonVisibility"/> for <see cref="SystemNavigationManager.AppViewBackButtonVisibility"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="string"/></returns>
        public static AppViewBackButtonVisibility GetBackButtonVisibility(DependencyObject obj)
        {
            var systemNavigationManager = GetSystemNavigationManager();

            return systemNavigationManager?.AppViewBackButtonVisibility ?? AppViewBackButtonVisibility.Collapsed;
        }

        /// <summary>
        /// Sets <see cref="AppViewBackButtonVisibility"/> to <see cref="SystemNavigationManager.AppViewBackButtonVisibility"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <param name="value"><see cref="AppViewBackButtonVisibility"/></param>
        public static void SetBackButtonVisibility(DependencyObject obj, AppViewBackButtonVisibility value)
        {
            var systemNavigationManager = GetSystemNavigationManager();

            if (systemNavigationManager != null)
            {
                systemNavigationManager.AppViewBackButtonVisibility = value;
            }
        }

        private static SystemNavigationManager GetSystemNavigationManager()
        {
            return SystemNavigationManager.GetForCurrentView();
        }

        private static CoreApplicationView GetCoreApplicationView()
        {
            return CoreApplication.GetCurrentView();
        }

        private static Windows.UI.ViewManagement.ApplicationView GetApplicationView()
        {
            return Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
        }
    }
}
