// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Longitude and Latitude for a tweet
    /// </summary>
    public class TwitterCoordinates
    {
        /// <summary>
        /// Gets the numeric latitude (null if the value could not be converted)
        /// </summary>
        public double Latitude { get; internal set; }

        /// <summary>
        /// Gets the numeric longitude (null if the value could not be converted)
        /// </summary>
        public double Longitude { get; internal set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"({Latitude}, {Longitude})";
        }
    }
}