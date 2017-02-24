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
