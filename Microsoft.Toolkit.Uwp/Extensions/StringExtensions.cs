// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.Resources;

namespace Microsoft.Toolkit.Uwp.Extensions
{
    /// <summary>
    /// UWP specific helpers for working with strings.
    /// </summary>
    public static class StringExtensions
    {
        private static ResourceLoader _independentLoader;

        /// <summary>
        /// Retrieves the provided resource for the current view context.
        /// </summary>
        /// <param name="resourceKey">Resource key to retrieve.</param>
        /// <returns>string value for given resource or empty string if not found.</returns>
        public static string GetViewLocalized(this string resourceKey)
        {
            _independentLoader = ResourceLoader.GetForViewIndependentUse();
            return ResourceLoader.GetForCurrentView().GetString(resourceKey);
        }

        /// <summary>
        /// Retrieves the provided resource for the given key for use independent of the UI thread.
        /// </summary>
        /// <param name="resourceKey">Resource key to retrieve.</param>
        /// <returns>string value for given resource or empty string if not found.</returns>
        public static string GetLocalized(this string resourceKey)
        {
            _independentLoader = ResourceLoader.GetForViewIndependentUse();
            return _independentLoader.GetString(resourceKey);
        }

        public static string GetLocalized(string resourcePath, string resourceKey)
        {
            _independentLoader = ResourceLoader.GetForViewIndependentUse(resourcePath);
            return _independentLoader.GetString(resourceKey);
        }
    }
}
