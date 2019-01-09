// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.ApplicationModel;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// This class provides static helper methods for <see cref="PackageVersion"/>.
    /// </summary>
    public static class PackageVersionHelper
    {
        /// <summary>
        /// Returns a string representation of a version with the format 'Major.Minor.Build.Revision'.
        /// </summary>
        /// <param name="packageVersion">The <see cref="PackageVersion"/> to convert to a string</param>
        /// <returns>Version string of the format 'Major.Minor.Build.Revision'</returns>
        public static string ToFormattedString(this PackageVersion packageVersion)
        {
            return $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}.{packageVersion.Revision}";
        }

        /// <summary>
        /// Converts a string representation of a version number to an equivalent <see cref="PackageVersion"/>.
        /// </summary>
        /// <param name="formattedVersionNumber">Version string of the format 'Major.Minor.Build.Revision'</param>
        /// <returns>The parsed <see cref="PackageVersion"/></returns>
        public static PackageVersion ToPackageVersion(this string formattedVersionNumber)
        {
            var parts = formattedVersionNumber.Split('.');

            return new PackageVersion
            {
                Major = ushort.Parse(parts[0]),
                Minor = ushort.Parse(parts[1]),
                Build = ushort.Parse(parts[2]),
                Revision = ushort.Parse(parts[3])
            };
        }
    }
}
