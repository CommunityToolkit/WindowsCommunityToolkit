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
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Provides attached properties for interacting with the <see cref="Windows.UI.ViewManagement.ApplicationView"/> on a window (app view).
    /// </summary>
    [Obsolete("Use Microsoft.Toolkit.Uwp.UI.Extensions.ApplicationViewExtensions")]
    public static class ApplicationView
    {
        /// <summary>
        /// Gets <see cref="string"/> for <see cref="Windows.UI.ViewManagement.ApplicationView.Title"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns><see cref="string"/></returns>
        [Obsolete("Use methods in Microsoft.Toolkit.Uwp.UI.Extensions.ApplicationViewExtensions")]
        public static string GetTitle(Page page)
        {
            return ApplicationViewExtensions.GetTitle(page);
        }

        /// <summary>
        /// Sets <see cref="string"/> to <see cref="Windows.UI.ViewManagement.ApplicationView.Title"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value"><see cref="string"/></param>
        [Obsolete("Use methods in Microsoft.Toolkit.Uwp.UI.Extensions.ApplicationViewExtensions")]
        public static void SetTitle(Page page, string value)
        {
            ApplicationViewExtensions.SetTitle(page, value);
        }

        /// <summary>
        /// Gets <see cref="bool"/> CoreApplicationView.TitleBar.ExtendViewIntoTitleBar
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns><see cref="string"/></returns>
        [Obsolete("Use methods in Microsoft.Toolkit.Uwp.UI.Extensions.ApplicationViewExtensions")]
        public static bool GetExtendViewIntoTitleBar(Page page)
        {
            return ApplicationViewExtensions.GetExtendViewIntoTitleBar(page);
        }

        /// <summary>
        /// Sets <see cref="bool"/> to CoreApplicationView.TitleBar.ExtendViewIntoTitleBar
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value"><see cref="bool"/></param>
        [Obsolete("Use methods in Microsoft.Toolkit.Uwp.UI.Extensions.ApplicationViewExtensions")]
        public static void SetExtendViewIntoTitleBar(Page page, bool value)
        {
            ApplicationViewExtensions.SetExtendViewIntoTitleBar(page, value);
        }

        /// <summary>
        /// Gets <see cref="AppViewBackButtonVisibility"/> for <see cref="SystemNavigationManager.AppViewBackButtonVisibility"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <returns><see cref="string"/></returns>
        [Obsolete("Use methods in Microsoft.Toolkit.Uwp.UI.Extensions.ApplicationViewExtensions")]
        public static AppViewBackButtonVisibility GetBackButtonVisibility(Page page)
        {
            return ApplicationViewExtensions.GetBackButtonVisibility(page);
        }

        /// <summary>
        /// Sets <see cref="AppViewBackButtonVisibility"/> to <see cref="SystemNavigationManager.AppViewBackButtonVisibility"/>
        /// </summary>
        /// <param name="page">The <see cref="Page"/></param>
        /// <param name="value"><see cref="AppViewBackButtonVisibility"/></param>
        [Obsolete("Use methods in Microsoft.Toolkit.Uwp.UI.Extensions.ApplicationViewExtensions")]
        public static void SetBackButtonVisibility(Page page, AppViewBackButtonVisibility value)
        {
            ApplicationViewExtensions.SetBackButtonVisibility(page, value);
        }
    }
}
