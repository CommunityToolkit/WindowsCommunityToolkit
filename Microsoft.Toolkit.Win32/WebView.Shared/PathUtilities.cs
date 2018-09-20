// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;

namespace Microsoft.Toolkit.Win32.UI.Controls
{
    // Contains path parsing utilities
    internal static class PathUtilities
    {
        // true if the character is the platform directory separator character or the alternate directory separator
        public static bool IsDirectorySeparator(char c) => c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar;

        public static bool IsAbsolute(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            // "C:\"
            if (IsDriveRootedAbsolutePath(path))
            {
                // Including invalid paths (e.g. "*:\")
                return true;
            }

            // "\\machine\share"
            // Including invalid/incomplete UNC paths (e.g. "\\foo")
            return path.Length >= 2 &&
                IsDirectorySeparator(path[0]) &&
                IsDirectorySeparator(path[1]);
        }

        // true if given path is absolute and starts with a drive specification (e.g. "C:\"); otherwise, false.
        private static bool IsDriveRootedAbsolutePath(string path)
        {
            return path.Length >= 3 && path[1] == Path.VolumeSeparatorChar && IsDirectorySeparator(path[2]);
        }
    }
}