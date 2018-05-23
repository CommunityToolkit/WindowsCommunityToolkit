// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.Graphics.Canvas;
using Newtonsoft.Json;
using Windows.Foundation;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InkDrawable : IDrawable
    {
        [JsonIgnore]
        public IReadOnlyList<InkStroke> Strokes { get; set; }

        public List<SerializableStroke> SerializableStrokeList { get; set; }

        public Rect Bounds { get; set; }

        public bool IsActive { get; set; }

        internal static readonly InkStrokeBuilder StrokeBuilder = new InkStrokeBuilder();

        public InkDrawable(IReadOnlyList<InkStroke> strokes)
        {
            if (strokes == null || !strokes.Any())
            {
                return;
            }

            Strokes = strokes;

            var first = strokes.First();
            double top = first.BoundingRect.Top, bottom = first.BoundingRect.Bottom, left = first.BoundingRect.Left, right = first.BoundingRect.Right;

            for (var index = 1; index < strokes.Count; index++)
            {
                var stroke = strokes[index];
                bottom = Math.Max(stroke.BoundingRect.Bottom, bottom);
                right = Math.Max(stroke.BoundingRect.Right, right);
                top = Math.Min(stroke.BoundingRect.Top, top);
                left = Math.Min(stroke.BoundingRect.Left, left);
            }

            Bounds = new Rect(left, top, right - left, bottom - top);
        }

        public bool IsVisible(Rect viewPort)
        {
            IsActive = RectHelper.Intersect(viewPort, Bounds) != Rect.Empty;
            return IsActive;
        }

        public void Draw(CanvasDrawingSession drawingSession, Rect sessionBounds)
        {
            var finalStrokeList = new List<InkStroke>(Strokes.Count);

            foreach (var stroke in Strokes)
            {
                var points = stroke.GetInkPoints();
                var finalPointList = new List<InkPoint>(points.Count);
                foreach (var point in points)
                {
                    finalPointList.Add(MapPointToToSessionBounds(point, sessionBounds));
                }

                StrokeBuilder.SetDefaultDrawingAttributes(stroke.DrawingAttributes);
                var newStroke = StrokeBuilder.CreateStrokeFromInkPoints(finalPointList, stroke.PointTransform);
                finalStrokeList.Add(newStroke);
            }

            drawingSession.DrawInk(finalStrokeList);
        }

        private static InkPoint MapPointToToSessionBounds(InkPoint point, Rect sessionBounds)
        {
            return new InkPoint(new Point(point.Position.X - sessionBounds.X, point.Position.Y - sessionBounds.Y), point.Pressure, point.TiltX, point.TiltY, point.Timestamp);
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            SerializableStrokeList = new List<SerializableStroke>(Strokes.Count);
            foreach (var stroke in Strokes)
            {
                var serializableStroke = new SerializableStroke();
                var points = stroke.GetInkPoints();
                var finalPointList = new List<InkPoint>(points.Count);
                foreach (var point in points)
                {
                    finalPointList.Add(point);
                }

                serializableStroke.FinalPointList = finalPointList;

                serializableStroke.DrawingAttributes = stroke.DrawingAttributes;
                serializableStroke.PointTransform = stroke.PointTransform;
                SerializableStrokeList.Add(serializableStroke);
            }
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            var finalStrokeList = new List<InkStroke>(SerializableStrokeList.Count);

            foreach (var stroke in SerializableStrokeList)
            {
                StrokeBuilder.SetDefaultDrawingAttributes(stroke.DrawingAttributes);
                var newStroke = StrokeBuilder.CreateStrokeFromInkPoints(stroke.FinalPointList, stroke.PointTransform);
                finalStrokeList.Add(newStroke);
            }

            Strokes = finalStrokeList;
            SerializableStrokeList = null;
        }

        [OnSerialized]
        internal void OnSerializedMethod(StreamingContext context)
        {
            SerializableStrokeList = null;
        }
    }
}
