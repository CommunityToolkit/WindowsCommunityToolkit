// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using System.Text.RegularExpressions;
using Microsoft.Graphics.Canvas.Geometry;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Class representing the RoundRectangle Figure in a Path Geometry
    /// </summary>
    internal sealed class CanvasRoundRectangleFigure : AbstractPathElement
    {
        private float _x;
        private float _y;
        private float _width;
        private float _height;
        private float _radiusX;
        private float _radiusY;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasRoundRectangleFigure"/> class.
        /// </summary>
        public CanvasRoundRectangleFigure()
        {
            _x = _y = _width = _height = _radiusX = _radiusY = 0f;
        }

        /// <summary>
        /// Adds the Path Element to the Path.
        /// </summary>
        /// <param name="pathBuilder">CanvasPathBuilder object</param>
        /// <param name="currentPoint">The last active location in the Path before adding
        /// the PolygonFigure</param>
        /// <param name="lastElement">The previous PathElement in the Path.</param>
        /// <returns>The latest location in the Path after adding the PolygonFigure</returns>
        public override Vector2 CreatePath(CanvasPathBuilder pathBuilder, Vector2 currentPoint, ref ICanvasPathElement lastElement)
        {
            // Calculate coordinates
            var topLeft = new Vector2(_x, _y);
            if (IsRelative)
            {
                topLeft += currentPoint;
            }

            // Execute command
            pathBuilder.AddRoundedRectangleFigure(topLeft.X, topLeft.Y, _width, _height, _radiusX, _radiusY);

            // No need to update the lastElement or currentPoint here as we are creating
            // a separate closed figure here.So current point will not change.
            return currentPoint;
        }

        /// <summary>
        /// Get the Regex for extracting Path Element Attributes
        /// </summary>
        /// <returns>Instance of <see cref="Regex"/></returns>
        protected override Regex GetAttributesRegex()
        {
            return RegexFactory.GetAttributesRegex(PathFigureType.RoundedRectangleFigure);
        }

        /// <summary>
        /// Gets the Path Element Attributes from the Match
        /// </summary>
        /// <param name="match">Match object</param>
        protected override void GetAttributes(Match match)
        {
            // X
            float.TryParse(match.Groups["X"].Value, out _x);

            // Y
            float.TryParse(match.Groups["Y"].Value, out _y);

            // Width
            float.TryParse(match.Groups["Width"].Value, out _width);

            // Sanitize by taking the absolute value
            _width = Math.Abs(_width);

            // Height
            float.TryParse(match.Groups["Height"].Value, out _height);

            // Sanitize by taking the absolute value
            _height = Math.Abs(_height);

            // RadiusX
            float.TryParse(match.Groups["RadiusX"].Value, out _radiusX);

            // Sanitize by taking the absolute value
            _radiusX = Math.Abs(_radiusX);

            // RadiusY
            float.TryParse(match.Groups["RadiusY"].Value, out _radiusY);

            // Sanitize by taking the absolute value
            _radiusY = Math.Abs(_radiusY);
        }
    }
}
