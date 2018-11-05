// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Mgcg;
using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Wui;
using System;
using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.CodeGen
{
#if !WINDOWS_UWP
    public
#endif
    sealed class CSharpInstantiatorGenerator : InstantiatorGeneratorBase
    {
        readonly CSharpStringifier _stringifier;

        CSharpInstantiatorGenerator(
            CompositionObject graphRoot,
            TimeSpan duration,
            bool setCommentProperties,
            bool disableFieldOptimization,
            CSharpStringifier stringifier)
            : base(graphRoot: graphRoot,
                  duration: duration,
                  setCommentProperties: setCommentProperties,
                  disableFieldOptimization: disableFieldOptimization,
                  stringifier: stringifier)
        {
            _stringifier = stringifier;
        }

        /// <summary>
        /// Returns the C# code for a factory that will instantiate the given <see cref="Visual"/> as a
        /// Windows.UI.Composition Visual.
        /// </summary>
        public static string CreateFactoryCode(
            string className,
            Visual rootVisual,
            float width,
            float height,
            TimeSpan duration,
            // Rarely set options used mostly for testing.
            bool disableFieldOptimization = false)
        {
            var generator = new CSharpInstantiatorGenerator(
                                graphRoot: rootVisual,
                                duration: duration,
                                setCommentProperties: false,
                                disableFieldOptimization: disableFieldOptimization,
                                stringifier: new CSharpStringifier());

            return generator.GenerateCode(className, width, height);
        }

        // Called by the base class to write the start of the file (i.e. everything up to the body of the Instantiator class).
        protected override void WriteFileStart(CodeBuilder builder, CodeGenInfo info)
        {
            if (info.RequiresWin2d)
            {
                builder.WriteLine("using Microsoft.Graphics.Canvas.Geometry;");
            }
            builder.WriteLine("using Microsoft.UI.Xaml.Controls;");
            builder.WriteLine("using System;");
            builder.WriteLine("using System.Numerics;");
            builder.WriteLine("using Windows.UI;");
            builder.WriteLine("using Windows.UI.Composition;");

            builder.WriteLine();
            builder.WriteLine("namespace AnimatedVisuals");
            builder.OpenScope();
            builder.WriteLine($"sealed class {info.ClassName} : IAnimatedVisualSource");
            builder.OpenScope();

            // Generate the method that creates an instance of the animated visual.
            builder.WriteLine("public IAnimatedVisual TryCreateAnimatedVisual(Compositor compositor, out object diagnostics)");
            builder.OpenScope();
            builder.WriteLine("diagnostics = null;");
            builder.WriteLine("if (!IsRuntimeCompatible())");
            builder.OpenScope();
            builder.WriteLine("return null;");
            builder.CloseScope();
            builder.WriteLine("return new AnimatedVisual(compositor);");
            builder.CloseScope();
            builder.WriteLine();
        }

        protected override void WriteInstantiatorStart(CodeBuilder builder, CodeGenInfo info)
        {
            // Start the instantiator class.
            builder.WriteLine("sealed class AnimatedVisual : IAnimatedVisual");
            builder.OpenScope();
        }

        // Called by the base class to write the end of the file (i.e. everything after the body of the Instantiator class).
        protected override void WriteFileEnd(
            CodeBuilder builder,
            CodeGenInfo info)
        {
            // Write the constructor for the instantiator.
            builder.WriteLine("internal AnimatedVisual(Compositor compositor)");
            builder.OpenScope();
            builder.WriteLine("_c = compositor;");
            builder.WriteLine($"{info.ReusableExpressionAnimationFieldName} = compositor.CreateExpressionAnimation();");
            builder.WriteLine("Root();");
            builder.CloseScope();
            builder.WriteLine();

            // Write the IAnimatedVisual implementation.
            builder.WriteLine("Visual IAnimatedVisual.RootVisual => _root;");
            builder.WriteLine($"TimeSpan IAnimatedVisual.Duration => TimeSpan.FromTicks({info.DurationTicksFieldName});");
            builder.WriteLine($"Vector2 IAnimatedVisual.Size => {Vector2(info.CompositionDeclaredSize)};");
            builder.WriteLine("void IDisposable.Dispose() => _root?.Dispose();");

            // Close the scope for the instantiator class.
            builder.CloseScope();

            // Close the scope for the class.
            builder.CloseScope();

            // Close the scope for the namespace
            builder.CloseScope();
        }

        protected override void WriteCanvasGeometryCombinationFactory(CodeBuilder builder, CanvasGeometry.Combination obj, string typeName, string fieldName)
        {
            builder.WriteLine($"var result = {FieldAssignment(fieldName)}{CallFactoryFor(obj.A)}.");
            builder.Indent();
            builder.WriteLine($"CombineWith({CallFactoryFor(obj.B)},");
            if (obj.Matrix.IsIdentity)
            {
                builder.WriteLine("Matrix3x2.Identity,");
            }
            else
            {
                builder.WriteLine($"{_stringifier.Matrix3x2(obj.Matrix)},");
            }
            builder.WriteLine($"{_stringifier.CanvasGeometryCombine(obj.CombineMode)});");
            builder.UnIndent();
        }

        protected override void WriteCanvasGeometryEllipseFactory(CodeBuilder builder, CanvasGeometry.Ellipse obj, string typeName, string fieldName)
        {
            builder.WriteLine($"var result = {FieldAssignment(fieldName)}CanvasGeometry.CreateEllipse(");
            builder.Indent();
            builder.WriteLine($"null,");
            builder.WriteLine($"{Float(obj.X)}, {Float(obj.Y)}, {Float(obj.RadiusX)}, {Float(obj.RadiusY)});");
            builder.UnIndent();
        }

        protected override void WriteCanvasGeometryPathFactory(CodeBuilder builder, CanvasGeometry.Path obj, string typeName, string fieldName)
        {
            builder.WriteLine($"{typeName} result;");
            builder.WriteLine($"using (var builder = new CanvasPathBuilder(null))");
            builder.OpenScope();
            if (obj.FilledRegionDetermination != CanvasFilledRegionDetermination.Alternate)
            {
                builder.WriteLine($"builder.SetFilledRegionDetermination({_stringifier.FilledRegionDetermination(obj.FilledRegionDetermination)});");
            }

            foreach (var command in obj.Commands)
            {
                switch (command.Type)
                {
                    case CanvasPathBuilder.CommandType.BeginFigure:
                        builder.WriteLine($"builder.BeginFigure({Vector2(((CanvasPathBuilder.Command.BeginFigure)command).StartPoint)});");
                        break;
                    case CanvasPathBuilder.CommandType.EndFigure:
                        builder.WriteLine($"builder.EndFigure({_stringifier.CanvasFigureLoop(((CanvasPathBuilder.Command.EndFigure)command).FigureLoop)});");
                        break;
                    case CanvasPathBuilder.CommandType.AddLine:
                        builder.WriteLine($"builder.AddLine({Vector2(((CanvasPathBuilder.Command.AddLine)command).EndPoint)});");
                        break;
                    case CanvasPathBuilder.CommandType.AddCubicBezier:
                        var cb = (CanvasPathBuilder.Command.AddCubicBezier)command;
                        builder.WriteLine($"builder.AddCubicBezier({Vector2(cb.ControlPoint1)}, {Vector2(cb.ControlPoint2)}, {Vector2(cb.EndPoint)});");
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            builder.WriteLine($"result = {FieldAssignment(fieldName)}CanvasGeometry.CreatePath(builder);");
            builder.CloseScope();
        }

        protected override void WriteCanvasGeometryRoundedRectangleFactory(CodeBuilder builder, CanvasGeometry.RoundedRectangle obj, string typeName, string fieldName)
        {
            builder.WriteLine($"var result = {FieldAssignment(fieldName)}CanvasGeometry.CreateRoundedRectangle(");
            builder.Indent();
            builder.WriteLine("null,");
            builder.WriteLine($"{Float(obj.X)},");
            builder.WriteLine($"{Float(obj.Y)},");
            builder.WriteLine($"{Float(obj.W)},");
            builder.WriteLine($"{Float(obj.H)},");
            builder.WriteLine($"{Float(obj.RadiusX)},");
            builder.WriteLine($"{Float(obj.RadiusY)};");
            builder.UnIndent();
        }

        protected override void WriteCanvasGeometryTransformedGeometryFactory(CodeBuilder builder, CanvasGeometry.TransformedGeometry obj, string typeName, string fieldName)
        {
            builder.WriteLine($"var result = {FieldAssignment(fieldName)}{CallFactoryFor(obj.SourceGeometry)}.Transform({_stringifier.Matrix3x2(obj.TransformMatrix)});");
        }

        static string FieldAssignment(string fieldName) => (fieldName != null ? $"{fieldName} = " : "");
        string Float(float value) => _stringifier.Float(value);
        string Vector2(Vector2 value) => _stringifier.Vector2(value);

        sealed class CSharpStringifier : StringifierBase
        {
            public override string Deref => ".";

            public override string ScopeResolve => ".";

            public override string Var => "var";

            public override string New => "new";

            public override string Null => "null";

            public override string IListAdd => "Add";

            public override string FactoryCall(string value) => value;


            public override string CanvasFigureLoop(CanvasFigureLoop value)
            {
                switch (value)
                {
                    case Mgcg.CanvasFigureLoop.Open:
                        return "CanvasFigureLoop.Open";
                    case Mgcg.CanvasFigureLoop.Closed:
                        return "CanvasFigureLoop.Closed";
                    default:
                        throw new InvalidOperationException();
                }
            }

            public override string CanvasGeometryCombine(CanvasGeometryCombine value)
            {
                switch (value)
                {
                    case Mgcg.CanvasGeometryCombine.Union:
                        return "CanvasGeometryCombine.Union";
                    case Mgcg.CanvasGeometryCombine.Exclude:
                        return "CanvasGeometryCombine.Exclude";
                    case Mgcg.CanvasGeometryCombine.Intersect:
                        return "CanvasGeometryCombine.Intersect";
                    case Mgcg.CanvasGeometryCombine.Xor:
                        return "CanvasGeometryCombine.Xor";
                    default:
                        throw new InvalidOperationException();
                }
            }

            public override string Color(Color value) => $"Color.FromArgb({Hex(value.A)}, {Hex(value.R)}, {Hex(value.G)}, {Hex(value.B)})";

            public override string FilledRegionDetermination(CanvasFilledRegionDetermination value)
            {
                switch (value)
                {
                    case CanvasFilledRegionDetermination.Alternate:
                        return "CanvasFilledRegionDetermination.Alternate";
                    case CanvasFilledRegionDetermination.Winding:
                        return "CanvasFilledRegionDetermination.Winding";
                    default:
                        throw new InvalidOperationException();
                }
            }

            public override string Int64(long value) => value.ToString();

            public override string Matrix3x2(Matrix3x2 value)
            {
                return $"new Matrix3x2({Float(value.M11)}, {Float(value.M12)}, {Float(value.M21)}, {Float(value.M22)}, {Float(value.M31)}, {Float(value.M32)})";
            }
            public override string Matrix4x4(Matrix4x4 value)
            {
                return $"new Matrix4x4({Float(value.M11)}, {Float(value.M12)}, {Float(value.M13)}, {Float(value.M14)}, {Float(value.M21)}, {Float(value.M22)}, {Float(value.M23)}, {Float(value.M24)}, {Float(value.M31)}, {Float(value.M32)}, {Float(value.M33)}, {Float(value.M34)}, {Float(value.M41)}, {Float(value.M42)}, {Float(value.M43)}, {Float(value.M44)})";
            }

            public override string Readonly(string value) => $"readonly {value}";

            public override string Int64TypeName => "long";

            public override string ReferenceTypeName(string value) => value;

            public override string TimeSpan(TimeSpan value) => TimeSpan(Int64(value.Ticks));
            public override string TimeSpan(string ticks) => $"TimeSpan.FromTicks({ticks})";

            public override string Vector2(Vector2 value) => $"new Vector2({ Float(value.X) }, { Float(value.Y)})";

            public override string Vector3(Vector3 value) => $"new Vector3({ Float(value.X) }, { Float(value.Y)}, {Float(value.Z)})";

        }
    }

}
