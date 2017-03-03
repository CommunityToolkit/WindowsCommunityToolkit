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

using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Provides attached properties for interacting with the <see cref="ApplicationView"/> on a window (app view).
    /// </summary>
    public static class ApplicationViewExtensions
    {
        /// <summary>
        /// Gets <see cref="string"/> for <see cref="ApplicationView.Title"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> typically <see cref="Page"/></param>
        /// <returns><see cref="string"/></returns>
        public static string GetTitle(DependencyObject obj)
        {
            var applicationView = GetApplicationView();

            return applicationView?.Title ?? string.Empty;
        }

        /// <summary>
        /// Sets <see cref="string"/> to <see cref="ApplicationView.Title"/>
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

        private static ApplicationView GetApplicationView()
        {
            return ApplicationView.GetForCurrentView();
        }
    }
}
