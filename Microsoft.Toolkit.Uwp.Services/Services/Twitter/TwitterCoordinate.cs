// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    public class TwitterCoordinate
    {
        /// <summary>
        /// Gets or sets latitude
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// Gets or sets longitude
        /// </summary>
        public float Longitude { get; set; }

        public TwitterCoordinate(float[] rawCoordinates)
        {
            Latitude = rawCoordinates[0];
            Longitude = rawCoordinates[1];
        }
    }
}