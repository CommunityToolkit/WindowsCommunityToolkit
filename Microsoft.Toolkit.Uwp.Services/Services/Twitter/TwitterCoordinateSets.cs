// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    public class TwitterCoordinateSets
    {
        /// <summary>
        /// Gets or sets list of twitter coordinates.
        /// </summary>
        public List<TwitterCoordinate> Coordinates { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterCoordinateSets"/> class.
        /// </summary>
        /// <param name="rawCoordinatesSet"></param>
        public TwitterCoordinateSets(List<float[]> rawCoordinatesSet)
        {
            foreach (float[] rawCoordinate in rawCoordinatesSet)
            {
                this.Coordinates.Add(new TwitterCoordinate(rawCoordinate));
            }
        }
    }
}