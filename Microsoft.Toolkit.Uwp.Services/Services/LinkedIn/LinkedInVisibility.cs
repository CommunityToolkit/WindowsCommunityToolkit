// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.Services.LinkedIn
{
    /// <summary>
    /// Strong type representation of Visibility.
    /// </summary>
    public class LinkedInVisibility
    {
        private const string ANYONE = "anyone";
        private const string CONNECTIONSONLY = "connections-only";

        /// <summary>
        /// Gets or sets code property.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Converts enum counterpart to appropriate data string.
        /// </summary>
        /// <param name="visibility">Enumeration.</param>
        /// <returns>String representation</returns>
        public static string ParseVisibilityEnumToString(LinkedInShareVisibility visibility)
        {
            switch (visibility)
            {
                case LinkedInShareVisibility.Anyone:
                    return ANYONE;
                case LinkedInShareVisibility.ConnectionsOnly:
                    return CONNECTIONSONLY;
            }

            return string.Empty;
        }

        /// <summary>
        /// Converts string to enum counterpart.
        /// </summary>
        /// <param name="visibility">String.</param>
        /// <returns>Enumeration.</returns>
        public static LinkedInShareVisibility ParseVisibilityStringToEnum(string visibility)
        {
            switch (visibility.ToLower())
            {
                case ANYONE:
                    return LinkedInShareVisibility.Anyone;
                case CONNECTIONSONLY:
                    return LinkedInShareVisibility.ConnectionsOnly;
            }

            throw new ArgumentException("Invalid visibility string supplied.");
        }
    }
}
