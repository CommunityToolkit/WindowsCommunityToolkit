// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.ApplicationModel.Resources;

namespace Microsoft.Toolkit.Uwp.Extensions
{
    /// <summary>
    /// UWP specific helpers for working with strings.
    /// </summary>
    public static class StringExtensions
    {
        private static readonly ResourceLoader IndependentLoader = ResourceLoader.GetForViewIndependentUse();

        /// <summary>
        /// Retrieves the provided resource for the current view context.
        /// </summary>
        /// <param name="resourceKey">Resource key to retrieve.</param>
        /// <returns>string value for given resource or empty string if not found.</returns>
        public static string GetViewLocalized(this string resourceKey)
            => ResourceLoader.GetForCurrentView().GetString(resourceKey);

        /// <summary>
        /// Retrieves the provided resource for the given key for use independent of the UI thread.
        /// </summary>
        /// <param name="resourceKey">Resource key to retrieve.</param>
        /// <returns>string value for given resource or empty string if not found.</returns>
        public static string GetLocalized(this string resourceKey)
            => IndependentLoader.GetString(resourceKey);

        /// <summary>
        /// Retrieves the provided resource for the given key for use independent of the UI thread.
        /// </summary>
        /// <param name="resourceKey">Resource key to retrieve.</param>
        /// <param name="resourcePath">Resource path to retrieve.</param>
        /// <returns>string value for given resource or empty string if not found.</returns>
        public static string GetLocalized(this string resourceKey, string resourcePath)
            => ResourceLoader.GetForViewIndependentUse(resourcePath).GetString(resourceKey);
    }
}
