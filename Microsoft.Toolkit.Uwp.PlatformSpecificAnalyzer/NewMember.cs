// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.PlatformSpecificAnalyzer
{
    /// <summary>
    /// This class wraps a new data members
    /// </summary>
    public struct NewMember
    {
        private static char[] methodCountSeparator = { '#' };

        /// <summary>
        /// Member name
        /// </summary>
        public string Name;

        /// <summary>
        /// Parameter count (if its a method)
        /// </summary>
        public int? ParameterCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewMember"/> struct.
        /// </summary>
        /// <param name="s">data containing name and optionally parameter count</param>
        public NewMember(string s)
        {
            string[] parts = s.Split(methodCountSeparator, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 2)
            {
                Name = parts[0];
                ParameterCount = int.Parse(parts[1]);
            }
            else
            {
                Name = s;
                ParameterCount = null;
            }
        }
    }
}
