// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Windows.ApplicationModel.Resources;

namespace CommunityToolkit.WinUI
{
    /// <summary>
    /// UWP specific helpers for working with strings.
    /// </summary>
    public static class StringExtensions
    {
        private static readonly ResourceLoader Loader;

        static StringExtensions()
        {
            try
            {
                Loader = new ResourceLoader();
            }
            catch
            {
            }
        }

        /*
        /// <summary>
        /// Retrieves the provided resource for the current view context.
        /// </summary>
        /// <param name="resourceKey">Resource key to retrieve.</param>
        /// <param name="uiContext"><see cref="UIContext"/> to be used to get the <paramref name="resourceKey"/> from.
        /// You can retrieve this from a UIElement.UIContext, XamlRoot.UIContext (XamlIslands), or Window.UIContext.</param>
        /// <returns>string value for given resource or empty string if not found.</returns>
        [SupportedOSPlatform("Windows10.0.18362.0")]
        public static string GetViewLocalized(this string resourceKey, UIContext uiContext)
        {
            var resourceLoader = ResourceLoader.GetForUIContext(uiContext);
            return resourceLoader.GetString(resourceKey);
        }
        */

        /// <summary>
        /// Retrieves the provided resource for the given key for use independent of the UI thread.
        /// </summary>
        /// <param name="resourceKey">Resource key to retrieve.</param>
        /// <returns>string value for given resource or empty string if not found.</returns>
        public static string GetLocalized(this string resourceKey)
        {
            return Loader?.GetString(resourceKey);
        }

        /*
        /// <summary>
        /// Retrieves the provided resource for the given key for use independent of the UI thread.
        /// </summary>
        /// <param name="resourceKey">Resource key to retrieve.</param>
        /// <param name="uiContext"><see cref="UIContext"/> to be used to get the <paramref name="resourceKey"/> from.
        /// You can retrieve this from a UIElement.UIContext, XamlRoot.UIContext (XamlIslands), or Window.UIContext.</param>
        /// <returns>string value for given resource or empty string if not found.</returns>
        [SupportedOSPlatform("Windows10.0.18362.0")]
        public static string GetLocalized(this string resourceKey, UIContext uiContext)
        {
            var resourceLoader = ResourceLoader.GetForUIContext(uiContext);
            return resourceLoader.GetString(resourceKey);
        }
        */

        /// <summary>
        /// Retrieves the provided resource for the given key for use independent of the UI thread. First looks up resource at the application level, before falling back to provided resourcePath. This allows for easily overridable resources within a library.
        /// </summary>
        /// <param name="resourceKey">Resource key to retrieve.</param>
        /// <param name="resourcePath">Resource path to fall back to in case resourceKey not found in app resources.</param>
        /// <returns>string value for given resource or empty string if not found.</returns>
        public static string GetLocalized(this string resourceKey, string resourcePath)
        {
            // Try and retrieve resource at app level first.
            var result = Loader?.GetString(resourceKey);

            if (string.IsNullOrEmpty(result))
            {
                result = new ResourceLoader(ResourceLoader.GetDefaultResourceFilePath(), resourcePath).GetString(resourceKey);
            }

            return result;
        }
    }
}