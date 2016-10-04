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
