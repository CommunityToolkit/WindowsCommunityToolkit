// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.RegularExpressions;
using Microsoft.Toolkit.Uwp.UI.Media.Geometry.Elements.Path;

namespace Microsoft.Toolkit.Uwp.UI.Media.Geometry.Core
{
    /// <summary>
    /// Factory class to instantiate various PathElements.
    /// </summary>
    internal static class PathElementFactory
    {
        /// <summary>
        /// Creates a default Path Element for the given PathFigureType
        /// </summary>
        /// <param name="figureType">PathFigureType</param>
        /// <returns>ICanvasPathElement</returns>
        internal static ICanvasPathElement CreateDefaultPathElement(PathFigureType figureType)
        {
            ICanvasPathElement result;

            switch (figureType)
            {
                case PathFigureType.FillRule:
                    result = new FillRuleElement();
                    break;
                default:
                    throw new ArgumentException("Creation of Only Default FillRuleElement is supported.", nameof(figureType));
            }

            return result;
        }

        /// <summary>
        /// Creates a default Path Element for the given PathElementType
        /// </summary>
        /// <param name="elementType">PathElementType</param>
        /// <returns>ICanvasPathElement</returns>
        internal static ICanvasPathElement CreateDefaultPathElement(PathElementType elementType)
        {
            ICanvasPathElement result;

            switch (elementType)
            {
                case PathElementType.ClosePath:
                    result = new ClosePathElement();
                    break;
                default:
                    throw new ArgumentException("Creation of Only Default ClosePathElement is supported.", nameof(elementType));
            }

            return result;
        }

        /// <summary>
        /// Instantiates a PathElement based on the PathFigureType
        /// </summary>
        /// <param name="figureType">PathFigureType</param>
        /// <param name="match">Match object</param>
        /// <param name="index">Index of the path element in the Path data</param>
        /// <returns>ICanvasPathElement</returns>
        internal static ICanvasPathElement CreatePathFigure(PathFigureType figureType, Match match, int index)
        {
            var element = CreatePathElement(figureType);
            element?.Initialize(match, index);
            return element;
        }

        /// <summary>
        /// Instantiates a PathElement based on the PathFigureType
        /// </summary>
        /// <param name="figureType">PathFigureType</param>
        /// <param name="capture">Capture object</param>
        /// <param name="index">Index of the capture</param>
        /// <param name="isRelative">Indicates whether the coordinates are absolute or relative</param>
        /// <returns>ICanvasPathElement</returns>
        internal static ICanvasPathElement CreateAdditionalPathFigure(PathFigureType figureType, Capture capture, int index, bool isRelative)
        {
            var element = CreatePathElement(figureType);
            element?.InitializeAdditional(capture, index, isRelative);
            return element;
        }

        /// <summary>
        /// Instantiates a PathElement based on the PathElementType
        /// </summary>
        /// <param name="elementType">PathElementType</param>
        /// <param name="match">Match object</param>
        /// <param name="index">Index of the path element in the Path data</param>
        /// <returns>ICanvasPathElement</returns>
        internal static ICanvasPathElement CreatePathElement(PathElementType elementType, Match match, int index)
        {
            var element = CreatePathElement(elementType);
            element?.Initialize(match, index);
            return element;
        }

        /// <summary>
        /// Instantiates a PathElement based on the PathElementType
        /// </summary>
        /// <param name="elementType">PathElementType</param>
        /// <param name="capture">Capture object</param>
        /// <param name="index">Index of the capture</param>
        /// <param name="isRelative">Indicates whether the coordinates are absolute or relative</param>
        /// <returns>ICanvasPathElement</returns>
        internal static ICanvasPathElement CreateAdditionalPathElement(PathElementType elementType, Capture capture, int index, bool isRelative)
        {
            // Additional attributes in MoveTo Command must be converted
            // to Line commands
            if (elementType == PathElementType.MoveTo)
            {
                elementType = PathElementType.Line;
            }

            var element = CreatePathElement(elementType);
            element?.InitializeAdditional(capture, index, isRelative);
            return element;
        }

        /// <summary>
        /// Instantiates a PathElement based on the PathFigureType
        /// </summary>
        /// <param name="figureType">PathFigureType</param>
        /// <returns>ICanvasPathElement</returns>
        private static ICanvasPathElement CreatePathElement(PathFigureType figureType)
        {
            ICanvasPathElement result = null;

            switch (figureType)
            {
                case PathFigureType.FillRule:
                    result = new FillRuleElement();
                    break;
                case PathFigureType.PathFigure:
                    result = new CanvasPathFigure();
                    break;
                case PathFigureType.EllipseFigure:
                    result = new CanvasEllipseFigure();
                    break;
                case PathFigureType.PolygonFigure:
                    result = new CanvasPolygonFigure();
                    break;
                case PathFigureType.RectangleFigure:
                    result = new CanvasRectangleFigure();
                    break;
                case PathFigureType.RoundedRectangleFigure:
                    result = new CanvasRoundRectangleFigure();
                    break;
            }

            return result;
        }

        /// <summary>
        /// Instantiates a PathElement based on the PathElementType
        /// </summary>
        /// <param name="elementType">PathElementType</param>
        /// <returns>ICanvasPathElement</returns>
        private static ICanvasPathElement CreatePathElement(PathElementType elementType)
        {
            ICanvasPathElement result = null;

            switch (elementType)
            {
                case PathElementType.MoveTo:
                    result = new MoveToElement();
                    break;
                case PathElementType.Line:
                    result = new LineElement();
                    break;
                case PathElementType.HorizontalLine:
                    result = new HorizontalLineElement();
                    break;
                case PathElementType.VerticalLine:
                    result = new VerticalLineElement();
                    break;
                case PathElementType.QuadraticBezier:
                    result = new QuadraticBezierElement();
                    break;
                case PathElementType.SmoothQuadraticBezier:
                    result = new SmoothQuadraticBezierElement();
                    break;
                case PathElementType.CubicBezier:
                    result = new CubicBezierElement();
                    break;
                case PathElementType.SmoothCubicBezier:
                    result = new SmoothCubicBezierElement();
                    break;
                case PathElementType.Arc:
                    result = new ArcElement();
                    break;
                case PathElementType.ClosePath:
                    result = new ClosePathElement();
                    break;
            }

            return result;
        }
    }
}
