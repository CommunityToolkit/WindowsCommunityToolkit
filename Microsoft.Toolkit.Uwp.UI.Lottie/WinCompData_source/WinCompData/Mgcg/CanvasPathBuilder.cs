// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Mgc;
using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Mgcg
{
#if !WINDOWS_UWP
    public
#endif
    sealed class CanvasPathBuilder : IDisposable
    {
        readonly ListOfNeverNull<Command> _commands = new ListOfNeverNull<Command>();
        bool _isFilledRegionDeterminationSet;

        public CanvasPathBuilder(CanvasDevice device) { }

        public void BeginFigure(Vector2 startPoint)
        {
            _commands.Add(new Command.BeginFigure(startPoint));
        }
        public void EndFigure(CanvasFigureLoop figureLoop)
        {
            _commands.Add(new Command.EndFigure(figureLoop));
        }
        public void AddCubicBezier(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint)
        {
            _commands.Add(new Command.AddCubicBezier(controlPoint1, controlPoint2, endPoint));
        }

        public void AddLine(Vector2 endPoint)
        {
            _commands.Add(new Command.AddLine(endPoint));
        }

        public void SetFilledRegionDetermination(CanvasFilledRegionDetermination value)
        {
            if (_isFilledRegionDeterminationSet)
            {
                // Throw if someone attempts to set the CanvasFilledRegionDetermination twice.
                // We could handle it, but it almost certainly indicates an accidental use
                // of the API.
                throw new InvalidOperationException();
            }
            _isFilledRegionDeterminationSet = true;
            FilledRegionDetermination = value;
        }

        internal CanvasFilledRegionDetermination FilledRegionDetermination { get; private set; }

        internal IEnumerable<Command> Commands => _commands;

        public void Dispose()
        {
        }

        public abstract class Command : IEquatable<Command>
        {
            Command() { }
            public abstract CommandType Type { get; }

            public bool Equals(Command other)
            {
                if (other == null)
                {
                    return false;
                }

                if (other.Type != Type)
                {
                    return false;
                }

                switch (Type)
                {
                    case CommandType.BeginFigure:
                        return ((BeginFigure)this).Equals((BeginFigure)other);
                    case CommandType.EndFigure:
                        return ((EndFigure)this).Equals((EndFigure)other);
                    case CommandType.AddCubicBezier:
                        return ((AddCubicBezier)this).Equals((AddCubicBezier)other);
                    case CommandType.AddLine:
                        return ((AddLine)this).Equals((AddLine)other);
                    default:
                        throw new InvalidOperationException();
                }
            }

            public sealed class BeginFigure : Command, IEquatable<BeginFigure>
            {
                internal BeginFigure(Vector2 startPoint)
                {
                    StartPoint = startPoint;
                }
                public override CommandType Type => CommandType.BeginFigure;
                public Vector2 StartPoint { get; }

                public bool Equals(BeginFigure other) => other != null && other.StartPoint.Equals(StartPoint);
            }

            public sealed class EndFigure : Command, IEquatable<EndFigure>
            {
                internal EndFigure(CanvasFigureLoop figureLoop)
                {
                    FigureLoop = figureLoop;
                }
                public override CommandType Type => CommandType.EndFigure;
                public CanvasFigureLoop FigureLoop { get; }

                public bool Equals(EndFigure other) => other != null && other.FigureLoop == FigureLoop;
            }

            public sealed class AddCubicBezier : Command, IEquatable<AddCubicBezier>
            {
                internal AddCubicBezier(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint)
                {
                    ControlPoint1 = controlPoint1;
                    ControlPoint2 = controlPoint2;
                    EndPoint = endPoint;
                }
                public override CommandType Type => CommandType.AddCubicBezier;
                public Vector2 ControlPoint1 { get; }
                public Vector2 ControlPoint2 { get; }
                public Vector2 EndPoint { get; }

                public bool Equals(AddCubicBezier other) =>
                    other != null &&
                    other.ControlPoint1.Equals(ControlPoint1) &&
                    other.ControlPoint2.Equals(ControlPoint2) &&
                    other.EndPoint.Equals(EndPoint);
            }

            public sealed class AddLine : Command, IEquatable<AddLine>
            {
                internal AddLine(Vector2 endPoint)
                {
                    EndPoint = endPoint;
                }
                public override CommandType Type => CommandType.AddLine;
                public Vector2 EndPoint { get; }

                public bool Equals(AddLine other) => other != null && other.EndPoint.Equals(EndPoint);
            }
        }

        public enum CommandType
        {
            BeginFigure,
            EndFigure,
            AddCubicBezier,
            AddLine,
        }
    }
}
