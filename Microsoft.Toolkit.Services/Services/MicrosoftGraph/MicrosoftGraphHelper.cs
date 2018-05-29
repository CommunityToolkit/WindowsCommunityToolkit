// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;

namespace Microsoft.Toolkit.Services.MicrosoftGraph
{
    /// <summary>
    /// Microsoft Graph Helper
    /// </summary>
    internal class MicrosoftGraphHelper
    {
        /// <summary>
        /// Build string with an array's items
        /// </summary>
        /// <typeparam name='T'>enum type</typeparam>
        /// <param name='t'>an array of enum containing the fields</param>
        /// <returns>a string with all fields separate by a comma.</returns>
        internal static string BuildString<T>(T[] t)
        {
            if (t.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            foreach (var field in t)
            {
                sb.Append(field.ToString());
                sb.Append(',');
            }

            string tempo = sb.ToString();

            // Remove the trailing comma character
            int lastPosition = tempo.Length - 1;

            return tempo.Substring(0, lastPosition);
        }
    }
}
