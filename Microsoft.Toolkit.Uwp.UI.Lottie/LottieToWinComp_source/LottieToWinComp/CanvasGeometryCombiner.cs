// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

#if PreCombineGeometries
using System;
using System.Diagnostics;
using System.Linq;
using Wcd = WinCompData;
using Win2D = Microsoft.Graphics.Canvas.Geometry;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieToWinComp
{
    /// <summary>
    /// Combines <see cref="CanvasGeometry"/>s into a single path.
    /// </summary>
    static class CanvasGeometryCombiner
    {
        internal static Wcd.Mgcg.CanvasGeometry CombineGeometries(Wcd.Mgcg.CanvasGeometry[] geometries, Wcd.Mgcg.CanvasGeometryCombine combineMode)
        {
            // Convert the geometries to Win2D geometries.
            var win2dGeometry = CombineWin2DGeometries(geometries.Select(ToWin2dCanvasGeometry).ToArray(), Win2dCanvasGeometryCombine(combineMode));

            // Convert to a Win2D path.
            var builder = new CpBuilder();
            win2dGeometry.SendPathTo(builder);

            // Convert back to a CanvasGeometry.
            return builder.ToCanvasGeometry();
        }

            static Win2D.CanvasGeometry CombineWin2DGeometries(Win2D.CanvasGeometry[] geometries, Win2D.CanvasGeometryCombine combineMode)
        {
            Debug.Assert(geometries.Length > 1);
            var accumulator = geometries[0];
            for (var i = 1; i < geometries.Length; i++)
            {
                accumulator = accumulator.CombineWith(geometries[i], System.Numerics.Matrix3x2.Identity, combineMode);
            }
            return accumulator;
        }

        sealed class CpBuilder : Win2D.ICanvasPathReceiver
        {
            Wcd.Mgcg.CanvasPathBuilder _builder = new Wcd.Mgcg.CanvasPathBuilder(null);

            public Wcd.Mgcg.CanvasGeometry ToCanvasGeometry()
            {
                return Wcd.Mgcg.CanvasGeometry.CreatePath(_builder);
            }

            void Win2D.ICanvasPathReceiver.BeginFigure(System.Numerics.Vector2 startPoint, Win2D.CanvasFigureFill figureFill)
            {
                // TODO - handle different figureFill types.
                _builder.BeginFigure(new WinCompData.Sn.Vector2(startPoint.X, startPoint.Y));
            }

            void Win2D.ICanvasPathReceiver.AddArc(System.Numerics.Vector2 endPoint, float radiusX, float radiusY, float rotationAngle, Win2D.CanvasSweepDirection sweepDirection, Win2D.CanvasArcSize arcSize)
            {
                // Should never be called because we never add arcs.
                throw new InvalidOperationException();
            }

            void Win2D.ICanvasPathReceiver.AddCubicBezier(System.Numerics.Vector2 controlPoint1, System.Numerics.Vector2 controlPoint2, System.Numerics.Vector2 endPoint)
            {
                _builder.AddCubicBezier(
                    new Wcd.Sn.Vector2(controlPoint1.X, controlPoint1.Y),
                    new Wcd.Sn.Vector2(controlPoint2.X, controlPoint2.Y),
                    new Wcd.Sn.Vector2(endPoint.X, endPoint.Y));
            }

            void Win2D.ICanvasPathReceiver.AddLine(System.Numerics.Vector2 endPoint)
            {
                _builder.AddLine(new Wcd.Sn.Vector2(endPoint.X, endPoint.Y));
            }

            void Win2D.ICanvasPathReceiver.AddQuadraticBezier(System.Numerics.Vector2 controlPoint, System.Numerics.Vector2 endPoint)
            {
                // Should never be called because we never add quadratic beziers.
                throw new InvalidOperationException();
            }

            void Win2D.ICanvasPathReceiver.SetFilledRegionDetermination(Win2D.CanvasFilledRegionDetermination filledRegionDetermination)
            {
                _builder.SetFilledRegionDetermination(FilledRegionDetermination(filledRegionDetermination));
            }

            void Win2D.ICanvasPathReceiver.SetSegmentOptions(Win2D.CanvasFigureSegmentOptions figureSegmentOptions)
            {
                // Should never be called because we never set segoment options.
                throw new InvalidOperationException();
            }

            void Win2D.ICanvasPathReceiver.EndFigure(Win2D.CanvasFigureLoop figureLoop)
            {
                _builder.EndFigure(ToCanvasFigureLoop(figureLoop));
            }
        }

        static Win2D.CanvasGeometry ToWin2dCanvasGeometry(Wcd.Mgcg.CanvasGeometry geometry)
        {
            switch (geometry.Type)
            {
                case Wcd.Mgcg.CanvasGeometry.GeometryType.Ellipse:
                    var ellipse = (Wcd.Mgcg.CanvasGeometry.Ellipse)geometry;
                    return Win2D.CanvasGeometry.CreateEllipse(
                        null,
                        ellipse.X,
                        ellipse.Y,
                        ellipse.RadiusX,
                        ellipse.RadiusY);
                case Wcd.Mgcg.CanvasGeometry.GeometryType.Path:
                    using (var builder = new Win2D.CanvasPathBuilder(null))
                    {
                        var path = (Wcd.Mgcg.CanvasGeometry.Path)geometry;
                        builder.SetFilledRegionDetermination(Win2DFilledRegionDetermination(path.FilledRegionDetermination));

                        foreach (var command in path.Commands)
                        {
                            switch (command.Type)
                            {
                                case Wcd.Mgcg.CanvasPathBuilder.CommandType.BeginFigure:
                                    builder.BeginFigure( SnVector2(((Wcd.Mgcg.CanvasPathBuilder.Command.BeginFigure)command).StartPoint));
                                    break;
                                case Wcd.Mgcg.CanvasPathBuilder.CommandType.EndFigure:
                                    builder.EndFigure(Win2DCanvasFigureLoop(((Wcd.Mgcg.CanvasPathBuilder.Command.EndFigure)command).FigureLoop));
                                    break;
                                case Wcd.Mgcg.CanvasPathBuilder.CommandType.AddLine:
                                    builder.AddLine(SnVector2(((Wcd.Mgcg.CanvasPathBuilder.Command.AddLine)command).EndPoint));
                                    break;
                                case Wcd.Mgcg.CanvasPathBuilder.CommandType.AddCubicBezier:
                                    var cb = (Wcd.Mgcg.CanvasPathBuilder.Command.AddCubicBezier)command;
                                    builder.AddCubicBezier(SnVector2(cb.ControlPoint1), SnVector2(cb.ControlPoint2), SnVector2(cb.EndPoint));
                                    break;
                                default:
                                    throw new InvalidOperationException();
                            }
                        }
                        return Win2D.CanvasGeometry.CreatePath(builder);
                    }
                case Wcd.Mgcg.CanvasGeometry.GeometryType.RoundedRectangle:
                    var roundedRectangle = (Wcd.Mgcg.CanvasGeometry.RoundedRectangle)geometry;
                    return Win2D.CanvasGeometry.CreateRoundedRectangle(
                        null,
                        roundedRectangle.X,
                        roundedRectangle.Y,
                        roundedRectangle.W,
                        roundedRectangle.H,
                        roundedRectangle.RadiusX,
                        roundedRectangle.RadiusY);
                case Wcd.Mgcg.CanvasGeometry.GeometryType.Combination:
                default:
                    throw new InvalidOperationException();
            }
        }

        static System.Numerics.Vector2 SnVector2(Wcd.Sn.Vector2 value) => new System.Numerics.Vector2(value.X, value.Y);

        static Wcd.Mgcg.CanvasFilledRegionDetermination FilledRegionDetermination(Win2D.CanvasFilledRegionDetermination fillType)
        {
            switch (fillType)
            {
                case Win2D.CanvasFilledRegionDetermination.Alternate:
                    return Wcd.Mgcg.CanvasFilledRegionDetermination.Alternate;
                case Win2D.CanvasFilledRegionDetermination.Winding:
                    return Wcd.Mgcg.CanvasFilledRegionDetermination.Winding;
                default:
                    throw new InvalidOperationException();
            }
        }

        static Win2D.CanvasFilledRegionDetermination Win2DFilledRegionDetermination(Wcd.Mgcg.CanvasFilledRegionDetermination fillType)
        {
            switch (fillType)
            {
                case Wcd.Mgcg.CanvasFilledRegionDetermination.Alternate:
                    return Win2D.CanvasFilledRegionDetermination.Alternate;
                case Wcd.Mgcg.CanvasFilledRegionDetermination.Winding:
                    return Win2D.CanvasFilledRegionDetermination.Winding;
                default:
                    throw new InvalidOperationException();
            }
        }

        static Win2D.CanvasGeometryCombine Win2dCanvasGeometryCombine(Wcd.Mgcg.CanvasGeometryCombine combine)
        {
            switch (combine)
            {
                case Wcd.Mgcg.CanvasGeometryCombine.Union:
                    return Win2D.CanvasGeometryCombine.Union;
                case Wcd.Mgcg.CanvasGeometryCombine.Exclude:
                    return Win2D.CanvasGeometryCombine.Exclude;
                case Wcd.Mgcg.CanvasGeometryCombine.Intersect:
                    return Win2D.CanvasGeometryCombine.Intersect;
                case Wcd.Mgcg.CanvasGeometryCombine.Xor:
                    return Win2D.CanvasGeometryCombine.Xor;
                default:
                    throw new InvalidOperationException();
            }
        }

        static Win2D.CanvasFigureLoop Win2DCanvasFigureLoop(Wcd.Mgcg.CanvasFigureLoop canvasFigureLoop)
        {
            switch (canvasFigureLoop)
            {
                case Wcd.Mgcg.CanvasFigureLoop.Open:
                    return Win2D.CanvasFigureLoop.Open;
                case Wcd.Mgcg.CanvasFigureLoop.Closed:
                    return Win2D.CanvasFigureLoop.Closed;
                default:
                    throw new InvalidOperationException();
            }
        }

        static Wcd.Mgcg.CanvasFigureLoop ToCanvasFigureLoop(Win2D.CanvasFigureLoop canvasFigureLoop)
        {
            switch (canvasFigureLoop)
            {
                case Win2D.CanvasFigureLoop.Open:
                    return Wcd.Mgcg.CanvasFigureLoop.Open;
                case Win2D.CanvasFigureLoop.Closed:
                    return Wcd.Mgcg.CanvasFigureLoop.Closed;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
#endif
