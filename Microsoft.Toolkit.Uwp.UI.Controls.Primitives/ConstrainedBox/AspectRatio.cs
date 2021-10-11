// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="AspectRatio"/> structure is used by the <see cref="ConstrainedBox"/> control to
    /// define a specific ratio to restrict its content.
    /// </summary>
    [Windows.Foundation.Metadata.CreateFromString(MethodName = "Microsoft.Toolkit.Uwp.UI.Controls.AspectRatio.ConvertToAspectRatio")]
    public readonly struct AspectRatio
    {
        /// <summary>
        /// Gets the width component of the aspect ratio or the aspect ratio itself (and height will be 1).
        /// </summary>
        public double Width { get; }

        /// <summary>
        /// Gets the height component of the aspect ratio.
        /// </summary>
        public double Height { get; }

        /// <summary>
        /// Gets the raw numeriucal aspect ratio value itself (Width / Height).
        /// </summary>
        public double Value => Width / Height;

        /// <summary>
        /// Initializes a new instance of the <see cref="AspectRatio"/> struct with the provided width and height.
        /// </summary>
        /// <param name="width">Width side of the ratio.</param>
        /// <param name="height">Height side of the ratio.</param>
        public AspectRatio(double width, double height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AspectRatio"/> struct with the specific numerical aspect ratio.
        /// </summary>
        /// <param name="ratio">Raw Aspect Ratio, Height will be 1.</param>
        public AspectRatio(double ratio)
        {
            Width = ratio;
            Height = 1;
        }

        /// <summary>
        /// Implicit conversion operator to convert an <see cref="AspectRatio"/> to a <see cref="double"/> value.
        /// This lets you use them easily in mathmatical expressions.
        /// </summary>
        /// <param name="aspect"><see cref="AspectRatio"/> instance.</param>
        public static implicit operator double(AspectRatio aspect) => aspect.Value;

        /// <summary>
        /// Implicit conversion operator to convert a <see cref="double"/> to an <see cref="AspectRatio"/> value.
        /// This allows for x:Bind to bind to a double value.
        /// </summary>
        /// <param name="ratio"><see cref="double"/> value representing the <see cref="AspectRatio"/>.</param>
        public static implicit operator AspectRatio(double ratio) => new AspectRatio(ratio);

        /// <summary>
        /// Implicit conversion operator to convert a <see cref="int"/> to an <see cref="AspectRatio"/> value.
        /// Creates a simple aspect ratio of N:1, where N is int
        /// </summary>
        /// <param name="width"><see cref="int"/> value representing the <see cref="AspectRatio"/>.</param>
        public static implicit operator AspectRatio(int width) => new AspectRatio(width, 1.0);

        /// <summary>
        /// Converter to take a string aspect ration like "16:9" and convert it to an <see cref="AspectRatio"/> struct.
        /// Used automatically by XAML.
        /// </summary>
        /// <param name="rawString">The string to be converted in format "Width:Height" or a decimal value.</param>
        /// <returns>The <see cref="AspectRatio"/> struct representing that ratio.</returns>
        public static AspectRatio ConvertToAspectRatio(string rawString)
        {
            string[] ratio = rawString.Split(":");

            if (ratio.Length == 2)
            {
                double width = double.Parse(ratio[0], NumberStyles.Float, CultureInfo.InvariantCulture);
                double height = double.Parse(ratio[1], NumberStyles.Float, CultureInfo.InvariantCulture);

                return new AspectRatio(width, height);
            }
            else if (ratio.Length == 1)
            {
                return new AspectRatio(double.Parse(ratio[0], NumberStyles.Float, CultureInfo.InvariantCulture));
            }

            return new AspectRatio(1);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Width + ":" + Height;
        }
    }
}
