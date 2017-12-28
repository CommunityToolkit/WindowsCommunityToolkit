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

using Windows.ApplicationModel;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// This class provides static helper methods for <see cref="PackageVersion" />.
    /// </summary>
    public static class PackageVersionHelper
    {
        /// <summary>
        /// Returns a string representing the version number, of the format 'Major.Minor.Build.Revision'
        /// </summary>
        /// <param name="packageVersion">The PackageVersion to convert to a string</param>
        /// <returns>String of the format 'Major.Minor.Build.Revision'</returns>
        public static string ToFormattedString(this PackageVersion packageVersion)
        {
            return $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}.{packageVersion.Revision}";
        }

        /// <summary>
        /// Convert a formatted string representing a version number as a PackageVersion object
        /// </summary>
        /// <param name="formattedVersionNumber">String of the format 'Major.Minor.Build.Revision'</param>
        /// <returns>A Package Version object</returns>
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
