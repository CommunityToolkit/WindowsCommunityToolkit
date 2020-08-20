// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using Microsoft.Toolkit.Diagnostics;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Referencable class object we can use to have a reference shared between
    /// our <see cref="UniformGrid.MeasureOverride"/> and
    /// <see cref="UniformGrid.GetFreeSpot"/> iterator.
    /// This is used so we can better isolate our logic and make it easier to test.
    /// </summary>
    internal class TakenSpotsReferenceHolder
    {
        /// <summary>
        /// The <see cref="BitArray"/> instance used to efficiently track empty spots.
        /// </summary>
        private readonly BitArray spotsTaken;

        /// <summary>
        /// Initializes a new instance of the <see cref="TakenSpotsReferenceHolder"/> class.
        /// </summary>
        /// <param name="rows">The number of rows to track.</param>
        /// <param name="columns">The number of columns to track.</param>
        public TakenSpotsReferenceHolder(int rows, int columns)
        {
            Guard.IsGreaterThanOrEqualTo(rows, 0, nameof(rows));
            Guard.IsGreaterThanOrEqualTo(columns, 0, nameof(columns));

            Height = rows;
            Width = columns;

            this.spotsTaken = new BitArray(rows * columns);
        }

        /// <summary>
        /// Gets the height of the grid to monitor.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the width of the grid to monitor.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets or sets the value of a specified grid cell.
        /// </summary>
        /// <param name="i">The vertical offset.</param>
        /// <param name="j">The horizontal offset.</param>
        public bool this[int i, int j]
        {
            get => spotsTaken[(i * Width) + j];
            set => spotsTaken[(i * Width) + j] = value;
        }
    }
}
