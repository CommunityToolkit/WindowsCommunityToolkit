// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.RegularExpressions;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Toolkit.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Media.Geometry.Core;
using Microsoft.Toolkit.Uwp.UI.Media.Geometry.Elements.Stroke;

namespace Microsoft.Toolkit.Uwp.UI.Media.Geometry.Parsers
{
    /// <summary>
    /// Parser for the CanvasStrokeStyle
    /// </summary>
    internal static class CanvasStrokeStyleParser
    {
        /// <summary>
        /// Parses the given style data and converts it to CanvasStrokeStyle.
        /// </summary>
        /// <param name="styleData">Style data</param>
        /// <returns>CanvasStrokeStyle</returns>
        internal static CanvasStrokeStyle Parse(string styleData)
        {
            var matches = RegexFactory.CanvasStrokeStyleRegex.Matches(styleData);

            // If no match is found or no captures in the match, then it means that the style data is invalid.
            Guard.IsFalse(matches.Count == 0, nameof(styleData), $"Invalid CanvasStrokeStyle data!\nCanvasStrokeStyle Data: {styleData}");

            // If the match contains more than one captures, it means that there
            // are multiple CanvasStrokeStyles present in the CanvasStrokeStyle data. There should
            // be only one CanvasStrokeStyle defined in the CanvasStrokeStyle data.
            Guard.IsFalse(matches.Count > 1, nameof(styleData), "Multiple CanvasStrokeStyles defined in CanvasStrokeStyle Data! " +
                                                                "There should be only one CanvasStrokeStyle definition within the CanvasStrokeStyle Data. " +
                                                                "You can either remove CanvasStrokeStyle definitions or split the CanvasStrokeStyle Data " +
                                                                "into multiple CanvasStrokeStyle Data and call the CanvasPathGeometry.CreateStrokeStyle() method on each of them." +
                                                                $"\nCanvasStrokeStyle Data: {styleData}");

            // There should be only one match
            var match = matches[0];
            var styleElement = new CanvasStrokeStyleElement(match);

            // Perform validation to check if there are any invalid characters in the brush data that were not captured
            var preValidationCount = RegexFactory.ValidationRegex.Replace(styleData, string.Empty).Length;
            var postValidationCount = styleElement.ValidationCount;

            Guard.IsTrue(preValidationCount == postValidationCount, nameof(styleData), $"CanvasStrokeStyle data contains invalid characters!\nCanvasStrokeStyle Data: {styleData}");

            return styleElement.Style;
        }

        /// <summary>
        /// Parses and constructs a ICanvasStrokeStyleElement from the specified Match object.
        /// </summary>
        /// <param name="match">Match object</param>
        /// <returns>ICanvasStrokeStyleElement</returns>
        internal static ICanvasStrokeStyleElement Parse(Match match)
        {
            return new CanvasStrokeStyleElement(match);
        }
    }
}
