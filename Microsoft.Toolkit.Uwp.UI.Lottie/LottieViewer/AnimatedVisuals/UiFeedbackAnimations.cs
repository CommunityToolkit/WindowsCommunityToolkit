// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;

namespace AnimatedVisuals
{
    sealed class UiFeedbackAnimations : IAnimatedVisualSource
    {
        sealed class AnimatedVisual : IAnimatedVisual
        {
            public Visual RootVisual { get; set; }
            public TimeSpan Duration { get; set; }
            public Vector2 Size { get; set; }
            public void Dispose()
            {
                RootVisual?.Dispose();
            }
        }

        public IAnimatedVisual TryCreateAnimatedVisual(Compositor compositor, out object diagnostics)
        {
            diagnostics = null;
            return new AnimatedVisual
            {
                RootVisual = Instantiator.InstantiateComposition(compositor),
                Size = new Vector2(337, 317),
                Duration = TimeSpan.FromTicks(c_durationTicks)
            };
        }

        const long c_durationTicks = 23830000;

        sealed class Instantiator
        {
            readonly Compositor _c;
            readonly ExpressionAnimation _expressionAnimation;
            CanvasGeometry _canvasGeometry_0000;
            CanvasGeometry _canvasGeometry_0001;
            CanvasGeometry _canvasGeometry_0002;
            CanvasGeometry _canvasGeometry_0003;
            CanvasGeometry _canvasGeometry_0004;
            CanvasGeometry _canvasGeometry_0005;
            CanvasGeometry _canvasGeometry_0007;
            CanvasGeometry _canvasGeometry_0008;
            CanvasGeometry _canvasGeometry_0009;
            CanvasGeometry _canvasGeometry_0011;
            CanvasGeometry _canvasGeometry_0012;
            CanvasGeometry _canvasGeometry_0013;
            CanvasGeometry _canvasGeometry_0015;
            CanvasGeometry _canvasGeometry_0016;
            CanvasGeometry _canvasGeometry_0017;
            CanvasGeometry _canvasGeometry_0019;
            CanvasGeometry _canvasGeometry_0020;
            CanvasGeometry _canvasGeometry_0021;
            CanvasGeometry _canvasGeometry_0022;
            CanvasGeometry _canvasGeometry_0024;
            CanvasGeometry _canvasGeometry_0025;
            CanvasGeometry _canvasGeometry_0026;
            CanvasGeometry _canvasGeometry_0027;
            CanvasGeometry _canvasGeometry_0028;
            CanvasGeometry _canvasGeometry_0029;
            CanvasGeometry _canvasGeometry_0031;
            CanvasGeometry _canvasGeometry_0032;
            CanvasGeometry _canvasGeometry_0033;
            CanvasGeometry _canvasGeometry_0035;
            CanvasGeometry _canvasGeometry_0036;
            CanvasGeometry _canvasGeometry_0038;
            CanvasGeometry _canvasGeometry_0039;
            CanvasGeometry _canvasGeometry_0040;
            CanvasGeometry _canvasGeometry_0042;
            CanvasGeometry _canvasGeometry_0043;
            CanvasGeometry _canvasGeometry_0044;
            CanvasGeometry _canvasGeometry_0045;
            CanvasGeometry _canvasGeometry_0046;
            CanvasGeometry _canvasGeometry_0047;
            CanvasGeometry _canvasGeometry_0048;
            CanvasGeometry _canvasGeometry_0049;
            CanvasGeometry _canvasGeometry_0050;
            CanvasGeometry _canvasGeometry_0051;
            CanvasGeometry _canvasGeometry_0052;
            CanvasGeometry _canvasGeometry_0053;
            CanvasGeometry _canvasGeometry_0122;
            CanvasGeometry _canvasGeometry_0123;
            ColorKeyFrameAnimation _colorKeyFrameAnimation_0000;
            ColorKeyFrameAnimation _colorKeyFrameAnimation_0002;
            ColorKeyFrameAnimation _colorKeyFrameAnimation_0004;
            CompositionColorBrush _compositionColorBrush_0000;
            CompositionColorBrush _compositionColorBrush_0001;
            CompositionColorBrush _compositionColorBrush_0022;
            CompositionColorBrush _compositionColorBrush_0065;
            CompositionColorBrush _compositionColorBrush_0090;
            CompositionColorBrush _compositionColorBrush_0091;
            ContainerVisual _containerVisual_0000;
            CubicBezierEasingFunction _cubicBezierEasingFunction_0000;
            CubicBezierEasingFunction _cubicBezierEasingFunction_0004;
            CubicBezierEasingFunction _cubicBezierEasingFunction_0005;
            CubicBezierEasingFunction _cubicBezierEasingFunction_0006;
            CubicBezierEasingFunction _cubicBezierEasingFunction_0007;
            CubicBezierEasingFunction _cubicBezierEasingFunction_0012;
            CubicBezierEasingFunction _cubicBezierEasingFunction_0013;
            CubicBezierEasingFunction _cubicBezierEasingFunction_0015;
            CubicBezierEasingFunction _cubicBezierEasingFunction_0016;
            CubicBezierEasingFunction _cubicBezierEasingFunction_0018;
            CubicBezierEasingFunction _cubicBezierEasingFunction_0019;
            CubicBezierEasingFunction _cubicBezierEasingFunction_0020;
            ExpressionAnimation _expressionAnimation_0000;
            ExpressionAnimation _expressionAnimation_0001;
            ExpressionAnimation _expressionAnimation_0002;
            ExpressionAnimation _expressionAnimation_0003;
            ExpressionAnimation _expressionAnimation_0004;
            ExpressionAnimation _expressionAnimation_0005;
            ExpressionAnimation _expressionAnimation_0006;
            ExpressionAnimation _expressionAnimation_0007;
            ExpressionAnimation _expressionAnimation_0008;
            ExpressionAnimation _expressionAnimation_0009;
            LinearEasingFunction _linearEasingFunction_0000;
            ScalarKeyFrameAnimation _scalarKeyFrameAnimation_0001;
            ScalarKeyFrameAnimation _scalarKeyFrameAnimation_0002;
            ScalarKeyFrameAnimation _scalarKeyFrameAnimation_0003;
            ScalarKeyFrameAnimation _scalarKeyFrameAnimation_0004;
            ScalarKeyFrameAnimation _scalarKeyFrameAnimation_0024;
            ScalarKeyFrameAnimation _scalarKeyFrameAnimation_0025;
            ScalarKeyFrameAnimation _scalarKeyFrameAnimation_0026;
            ScalarKeyFrameAnimation _scalarKeyFrameAnimation_0029;
            ScalarKeyFrameAnimation _scalarKeyFrameAnimation_0030;
            ScalarKeyFrameAnimation _scalarKeyFrameAnimation_0031;
            ScalarKeyFrameAnimation _scalarKeyFrameAnimation_0032;
            StepEasingFunction _stepEasingFunction_0000;
            Vector2KeyFrameAnimation _vector2KeyFrameAnimation_0002;
            Vector2KeyFrameAnimation _vector2KeyFrameAnimation_0012;
            
            internal static Visual InstantiateComposition(Compositor compositor)
                => new Instantiator(compositor).ContainerVisual_0000();
            
            Instantiator(Compositor compositor)
            {
                _c = compositor;
                _expressionAnimation = compositor.CreateExpressionAnimation();
            }
            
            CanvasGeometry CanvasGeometry_0000()
            {
                if (_canvasGeometry_0000 != null)
                {
                    return _canvasGeometry_0000;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-64.5F, 79.5F));
                    builder.AddCubicBezier(new Vector2(-64.502F, 75.91F), new Vector2(-64.761F, 63.69F), new Vector2(-65, 53));
                    builder.AddCubicBezier(new Vector2(-65.933F, 11.355F), new Vector2(-64, -60), new Vector2(-65.605F, -82.653F));
                    builder.AddCubicBezier(new Vector2(-65.739F, -84.551F), new Vector2(-65.557F, -86.615F), new Vector2(-64.388F, -88.116F));
                    builder.AddCubicBezier(new Vector2(-62.915F, -90.008F), new Vector2(-60.232F, -90.338F), new Vector2(-57.84F, -90.508F));
                    builder.AddCubicBezier(new Vector2(-33.568F, -92.235F), new Vector2(-9.224F, -92.948F), new Vector2(15.107F, -92.646F));
                    builder.AddCubicBezier(new Vector2(17.362F, -92.618F), new Vector2(19.708F, -92.561F), new Vector2(21.72F, -91.541F));
                    builder.AddCubicBezier(new Vector2(23.53F, -90.624F), new Vector2(24.88F, -89.022F), new Vector2(26.178F, -87.462F));
                    builder.AddCubicBezier(new Vector2(38.193F, -73.022F), new Vector2(52.259F, -63.436F), new Vector2(66.821F, -51.569F));
                    builder.AddCubicBezier(new Vector2(66.059F, -50.368F), new Vector2(64.434F, -50.134F), new Vector2(63.016F, -50.023F));
                    builder.AddCubicBezier(new Vector2(49.588F, -48.968F), new Vector2(36.133F, -48.254F), new Vector2(22.669F, -47.881F));
                    builder.AddCubicBezier(new Vector2(21.997F, -47.862F), new Vector2(21.177F, -47.928F), new Vector2(20.881F, -48.531F));
                    builder.AddCubicBezier(new Vector2(20.707F, -48.885F), new Vector2(20.786F, -49.305F), new Vector2(20.867F, -49.69F));
                    builder.AddCubicBezier(new Vector2(23.453F, -61.964F), new Vector2(24.462F, -71.569F), new Vector2(23.862F, -84.098F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return  _canvasGeometry_0000 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0001()
            {
                if (_canvasGeometry_0001 != null)
                {
                    return _canvasGeometry_0001;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(68.253F, -45));
                    builder.AddCubicBezier(new Vector2(72.472F, -13.072F), new Vector2(66.645F, 23.409F), new Vector2(70.891F, 55.334F));
                    builder.AddCubicBezier(new Vector2(71.6F, 60.667F), new Vector2(72.592F, 65.986F), new Vector2(72.656F, 71.365F));
                    builder.AddCubicBezier(new Vector2(72.67F, 72.574F), new Vector2(72.611F, 73.867F), new Vector2(71.906F, 74.849F));
                    builder.AddCubicBezier(new Vector2(70.801F, 76.389F), new Vector2(68.613F, 76.56F), new Vector2(66.717F, 76.565F));
                    builder.AddCubicBezier(new Vector2(45.807F, 76.618F), new Vector2(-4.783F, 78.675F), new Vector2(-44, 79));
                    builder.AddCubicBezier(new Vector2(-59.26F, 79.126F), new Vector2(-61.784F, 77.676F), new Vector2(-67.384F, 80.216F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return  _canvasGeometry_0001 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0002()
            {
                if (_canvasGeometry_0002 != null)
                {
                    return _canvasGeometry_0002;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-27.934F, 32.289F));
                    builder.AddCubicBezier(new Vector2(-26.232F, 40.567F), new Vector2(-26.075F, 49.16F), new Vector2(-27.475F, 57.495F));
                    builder.AddCubicBezier(new Vector2(-27.709F, 58.887F), new Vector2(-28.011F, 60.328F), new Vector2(-28.879F, 61.441F));
                    builder.AddCubicBezier(new Vector2(-30.039F, 62.928F), new Vector2(-32.011F, 63.552F), new Vector2(-33.893F, 63.686F));
                    builder.AddCubicBezier(new Vector2(-35.567F, 63.805F), new Vector2(-37.332F, 63.574F), new Vector2(-38.716F, 62.626F));
                    builder.AddCubicBezier(new Vector2(-40.1F, 61.678F), new Vector2(-40.99F, 59.89F), new Vector2(-40.527F, 58.277F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return  _canvasGeometry_0002 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0003()
            {
                if (_canvasGeometry_0003 != null)
                {
                    return _canvasGeometry_0003;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-5.939F, 34.091F));
                    builder.AddCubicBezier(new Vector2(-9.524F, 32.446F), new Vector2(-13.862F, 33.493F), new Vector2(-16.639F, 36.675F));
                    builder.AddCubicBezier(new Vector2(-17.624F, 37.804F), new Vector2(-18.445F, 39.254F), new Vector2(-18.555F, 40.884F));
                    builder.AddCubicBezier(new Vector2(-18.725F, 43.396F), new Vector2(-17.146F, 45.679F), new Vector2(-15.276F, 46.822F));
                    builder.AddCubicBezier(new Vector2(-13.406F, 47.965F), new Vector2(-11.269F, 48.243F), new Vector2(-9.206F, 48.703F));
                    builder.AddCubicBezier(new Vector2(-7.143F, 49.163F), new Vector2(-5.006F, 49.891F), new Vector2(-3.53F, 51.682F));
                    builder.AddCubicBezier(new Vector2(-2.593F, 52.819F), new Vector2(-0.288F, 56.858F), new Vector2(-3.107F, 59.089F));
                    builder.AddCubicBezier(new Vector2(-7.839F, 62.833F), new Vector2(-13.767F, 64.307F), new Vector2(-19.209F, 62.565F));
                    builder.AddCubicBezier(new Vector2(-19.39F, 62.507F), new Vector2(-19.594F, 62.419F), new Vector2(-19.657F, 62.207F));
                    builder.AddCubicBezier(new Vector2(-19.72F, 61.995F), new Vector2(-19.464F, 61.736F), new Vector2(-19.353F, 61.917F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return  _canvasGeometry_0003 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0004()
            {
                if (_canvasGeometry_0004 != null)
                {
                    return _canvasGeometry_0004;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(10.534F, 34.768F));
                    builder.AddCubicBezier(new Vector2(8.948F, 35.347F), new Vector2(8.064F, 37.231F), new Vector2(7.449F, 38.99F));
                    builder.AddCubicBezier(new Vector2(6.291F, 42.299F), new Vector2(5.504F, 45.775F), new Vector2(5.11F, 49.308F));
                    builder.AddCubicBezier(new Vector2(4.79F, 52.178F), new Vector2(4.775F, 55.282F), new Vector2(6.127F, 57.733F));
                    builder.AddCubicBezier(new Vector2(7.083F, 59.465F), new Vector2(8.628F, 60.678F), new Vector2(10.257F, 61.546F));
                    builder.AddCubicBezier(new Vector2(12.111F, 62.534F), new Vector2(14.213F, 63.137F), new Vector2(16.212F, 62.632F));
                    builder.AddCubicBezier(new Vector2(18.229F, 62.123F), new Vector2(19.951F, 60.502F), new Vector2(21.01F, 58.487F));
                    builder.AddCubicBezier(new Vector2(22.069F, 56.472F), new Vector2(22.508F, 54.094F), new Vector2(22.538F, 51.747F));
                    builder.AddCubicBezier(new Vector2(22.639F, 43.702F), new Vector2(17.593F, 35.926F), new Vector2(10.798F, 33.659F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return  _canvasGeometry_0004 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0005()
            {
                if (_canvasGeometry_0005 != null)
                {
                    return _canvasGeometry_0005;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(28.548F, 63.332F));
                    builder.AddCubicBezier(new Vector2(28.547F, 60.06F), new Vector2(28.545F, 56.788F), new Vector2(28.544F, 53.516F));
                    builder.AddCubicBezier(new Vector2(28.541F, 46.442F), new Vector2(28.528F, 39.261F), new Vector2(27.069F, 32.455F));
                    builder.AddCubicBezier(new Vector2(28.63F, 33.482F), new Vector2(29.537F, 35.64F), new Vector2(30.396F, 37.644F));
                    builder.AddCubicBezier(new Vector2(34.28F, 46.699F), new Vector2(39.071F, 55.074F), new Vector2(44.606F, 62.48F));
                    builder.AddCubicBezier(new Vector2(44.589F, 57.629F), new Vector2(44.563F, 52.696F), new Vector2(43.49F, 48.056F));
                    builder.AddCubicBezier(new Vector2(42.875F, 45.396F), new Vector2(42.384F, 29.828F), new Vector2(42.057F, 30.001F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return  _canvasGeometry_0005 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0006()
            {
                var result = CanvasGeometry_0007().
                    CombineWith(CanvasGeometry_0008(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0007()
            {
                if (_canvasGeometry_0007 != null)
                {
                    return _canvasGeometry_0007;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(169.273F, -4.766F));
                    builder.AddCubicBezier(new Vector2(169.372F, -5.464F), new Vector2(169.633F, -6.029F), new Vector2(170.055F, -6.461F));
                    builder.AddCubicBezier(new Vector2(170.477F, -6.893F), new Vector2(170.99F, -7.109F), new Vector2(171.594F, -7.109F));
                    builder.AddCubicBezier(new Vector2(172.219F, -7.109F), new Vector2(172.707F, -6.903F), new Vector2(173.059F, -6.492F));
                    builder.AddCubicBezier(new Vector2(173.411F, -6.081F), new Vector2(173.589F, -5.506F), new Vector2(173.594F, -4.766F));
                    builder.AddCubicBezier(new Vector2(173.594F, -4.766F), new Vector2(169.273F, -4.766F), new Vector2(169.273F, -4.766F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0007 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0008()
            {
                if (_canvasGeometry_0008 != null)
                {
                    return _canvasGeometry_0008;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(174.906F, -4.352F));
                    builder.AddCubicBezier(new Vector2(174.906F, -5.555F), new Vector2(174.62F, -6.495F), new Vector2(174.047F, -7.172F));
                    builder.AddCubicBezier(new Vector2(173.474F, -7.849F), new Vector2(172.664F, -8.188F), new Vector2(171.617F, -8.188F));
                    builder.AddCubicBezier(new Vector2(170.57F, -8.188F), new Vector2(169.694F, -7.796F), new Vector2(168.988F, -7.012F));
                    builder.AddCubicBezier(new Vector2(168.282F, -6.228F), new Vector2(167.93F, -5.214F), new Vector2(167.93F, -3.969F));
                    builder.AddCubicBezier(new Vector2(167.93F, -2.651F), new Vector2(168.252F, -1.629F), new Vector2(168.898F, -0.902F));
                    builder.AddCubicBezier(new Vector2(169.544F, -0.175F), new Vector2(170.433F, 0.188F), new Vector2(171.563F, 0.188F));
                    builder.AddCubicBezier(new Vector2(172.719F, 0.188F), new Vector2(173.649F, -0.068F), new Vector2(174.352F, -0.578F));
                    builder.AddCubicBezier(new Vector2(174.352F, -0.578F), new Vector2(174.352F, -1.781F), new Vector2(174.352F, -1.781F));
                    builder.AddCubicBezier(new Vector2(173.597F, -1.187F), new Vector2(172.768F, -0.891F), new Vector2(171.867F, -0.891F));
                    builder.AddCubicBezier(new Vector2(171.065F, -0.891F), new Vector2(170.435F, -1.133F), new Vector2(169.977F, -1.617F));
                    builder.AddCubicBezier(new Vector2(169.519F, -2.101F), new Vector2(169.279F, -2.789F), new Vector2(169.258F, -3.68F));
                    builder.AddCubicBezier(new Vector2(169.258F, -3.68F), new Vector2(174.906F, -3.68F), new Vector2(174.906F, -3.68F));
                    builder.AddCubicBezier(new Vector2(174.906F, -3.68F), new Vector2(174.906F, -4.352F), new Vector2(174.906F, -4.352F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0008 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0009()
            {
                if (_canvasGeometry_0009 != null)
                {
                    return _canvasGeometry_0009;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(167.086F, -8.031F));
                    builder.AddCubicBezier(new Vector2(166.909F, -8.104F), new Vector2(166.653F, -8.141F), new Vector2(166.32F, -8.141F));
                    builder.AddCubicBezier(new Vector2(165.856F, -8.141F), new Vector2(165.438F, -7.983F), new Vector2(165.063F, -7.668F));
                    builder.AddCubicBezier(new Vector2(164.688F, -7.353F), new Vector2(164.409F, -6.915F), new Vector2(164.227F, -6.352F));
                    builder.AddCubicBezier(new Vector2(164.227F, -6.352F), new Vector2(164.195F, -6.352F), new Vector2(164.195F, -6.352F));
                    builder.AddCubicBezier(new Vector2(164.195F, -6.352F), new Vector2(164.195F, -8), new Vector2(164.195F, -8));
                    builder.AddCubicBezier(new Vector2(164.195F, -8), new Vector2(162.914F, -8), new Vector2(162.914F, -8));
                    builder.AddCubicBezier(new Vector2(162.914F, -8), new Vector2(162.914F, 0), new Vector2(162.914F, 0));
                    builder.AddCubicBezier(new Vector2(162.914F, 0), new Vector2(164.195F, 0), new Vector2(164.195F, 0));
                    builder.AddCubicBezier(new Vector2(164.195F, 0), new Vector2(164.195F, -4.078F), new Vector2(164.195F, -4.078F));
                    builder.AddCubicBezier(new Vector2(164.195F, -4.969F), new Vector2(164.379F, -5.672F), new Vector2(164.746F, -6.188F));
                    builder.AddCubicBezier(new Vector2(165.113F, -6.704F), new Vector2(165.57F, -6.961F), new Vector2(166.117F, -6.961F));
                    builder.AddCubicBezier(new Vector2(166.539F, -6.961F), new Vector2(166.862F, -6.875F), new Vector2(167.086F, -6.703F));
                    builder.AddCubicBezier(new Vector2(167.086F, -6.703F), new Vector2(167.086F, -8.031F), new Vector2(167.086F, -8.031F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0009 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0010()
            {
                var result = CanvasGeometry_0011().
                    CombineWith(CanvasGeometry_0012(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0011()
            {
                if (_canvasGeometry_0011 != null)
                {
                    return _canvasGeometry_0011;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(155.344F, -4.766F));
                    builder.AddCubicBezier(new Vector2(155.443F, -5.464F), new Vector2(155.703F, -6.029F), new Vector2(156.125F, -6.461F));
                    builder.AddCubicBezier(new Vector2(156.547F, -6.893F), new Vector2(157.06F, -7.109F), new Vector2(157.664F, -7.109F));
                    builder.AddCubicBezier(new Vector2(158.289F, -7.109F), new Vector2(158.777F, -6.903F), new Vector2(159.129F, -6.492F));
                    builder.AddCubicBezier(new Vector2(159.481F, -6.081F), new Vector2(159.659F, -5.506F), new Vector2(159.664F, -4.766F));
                    builder.AddCubicBezier(new Vector2(159.664F, -4.766F), new Vector2(155.344F, -4.766F), new Vector2(155.344F, -4.766F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0011 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0012()
            {
                if (_canvasGeometry_0012 != null)
                {
                    return _canvasGeometry_0012;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(160.977F, -4.352F));
                    builder.AddCubicBezier(new Vector2(160.977F, -5.555F), new Vector2(160.69F, -6.495F), new Vector2(160.117F, -7.172F));
                    builder.AddCubicBezier(new Vector2(159.544F, -7.849F), new Vector2(158.735F, -8.188F), new Vector2(157.688F, -8.188F));
                    builder.AddCubicBezier(new Vector2(156.641F, -8.188F), new Vector2(155.765F, -7.796F), new Vector2(155.059F, -7.012F));
                    builder.AddCubicBezier(new Vector2(154.353F, -6.228F), new Vector2(154, -5.214F), new Vector2(154, -3.969F));
                    builder.AddCubicBezier(new Vector2(154, -2.651F), new Vector2(154.323F, -1.629F), new Vector2(154.969F, -0.902F));
                    builder.AddCubicBezier(new Vector2(155.615F, -0.175F), new Vector2(156.503F, 0.188F), new Vector2(157.633F, 0.188F));
                    builder.AddCubicBezier(new Vector2(158.789F, 0.188F), new Vector2(159.719F, -0.068F), new Vector2(160.422F, -0.578F));
                    builder.AddCubicBezier(new Vector2(160.422F, -0.578F), new Vector2(160.422F, -1.781F), new Vector2(160.422F, -1.781F));
                    builder.AddCubicBezier(new Vector2(159.667F, -1.187F), new Vector2(158.839F, -0.891F), new Vector2(157.938F, -0.891F));
                    builder.AddCubicBezier(new Vector2(157.136F, -0.891F), new Vector2(156.505F, -1.133F), new Vector2(156.047F, -1.617F));
                    builder.AddCubicBezier(new Vector2(155.589F, -2.101F), new Vector2(155.349F, -2.789F), new Vector2(155.328F, -3.68F));
                    builder.AddCubicBezier(new Vector2(155.328F, -3.68F), new Vector2(160.977F, -3.68F), new Vector2(160.977F, -3.68F));
                    builder.AddCubicBezier(new Vector2(160.977F, -3.68F), new Vector2(160.977F, -4.352F), new Vector2(160.977F, -4.352F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0012 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0013()
            {
                if (_canvasGeometry_0013 != null)
                {
                    return _canvasGeometry_0013;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(152.133F, -4.93F));
                    builder.AddCubicBezier(new Vector2(152.133F, -7.102F), new Vector2(151.232F, -8.188F), new Vector2(149.43F, -8.188F));
                    builder.AddCubicBezier(new Vector2(148.295F, -8.188F), new Vector2(147.42F, -7.682F), new Vector2(146.805F, -6.672F));
                    builder.AddCubicBezier(new Vector2(146.805F, -6.672F), new Vector2(146.773F, -6.672F), new Vector2(146.773F, -6.672F));
                    builder.AddCubicBezier(new Vector2(146.773F, -6.672F), new Vector2(146.773F, -11.844F), new Vector2(146.773F, -11.844F));
                    builder.AddCubicBezier(new Vector2(146.773F, -11.844F), new Vector2(145.492F, -11.844F), new Vector2(145.492F, -11.844F));
                    builder.AddCubicBezier(new Vector2(145.492F, -11.844F), new Vector2(145.492F, 0), new Vector2(145.492F, 0));
                    builder.AddCubicBezier(new Vector2(145.492F, 0), new Vector2(146.773F, 0), new Vector2(146.773F, 0));
                    builder.AddCubicBezier(new Vector2(146.773F, 0), new Vector2(146.773F, -4.531F), new Vector2(146.773F, -4.531F));
                    builder.AddCubicBezier(new Vector2(146.773F, -5.286F), new Vector2(146.987F, -5.905F), new Vector2(147.414F, -6.387F));
                    builder.AddCubicBezier(new Vector2(147.841F, -6.869F), new Vector2(148.367F, -7.109F), new Vector2(148.992F, -7.109F));
                    builder.AddCubicBezier(new Vector2(150.232F, -7.109F), new Vector2(150.852F, -6.276F), new Vector2(150.852F, -4.609F));
                    builder.AddCubicBezier(new Vector2(150.852F, -4.609F), new Vector2(150.852F, 0), new Vector2(150.852F, 0));
                    builder.AddCubicBezier(new Vector2(150.852F, 0), new Vector2(152.133F, 0), new Vector2(152.133F, 0));
                    builder.AddCubicBezier(new Vector2(152.133F, 0), new Vector2(152.133F, -4.93F), new Vector2(152.133F, -4.93F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0013 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0014()
            {
                var result = CanvasGeometry_0015().
                    CombineWith(CanvasGeometry_0016(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0015()
            {
                if (_canvasGeometry_0015 != null)
                {
                    return _canvasGeometry_0015;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(133.539F, -4.766F));
                    builder.AddCubicBezier(new Vector2(133.638F, -5.464F), new Vector2(133.898F, -6.029F), new Vector2(134.32F, -6.461F));
                    builder.AddCubicBezier(new Vector2(134.742F, -6.893F), new Vector2(135.255F, -7.109F), new Vector2(135.859F, -7.109F));
                    builder.AddCubicBezier(new Vector2(136.484F, -7.109F), new Vector2(136.972F, -6.903F), new Vector2(137.324F, -6.492F));
                    builder.AddCubicBezier(new Vector2(137.676F, -6.081F), new Vector2(137.854F, -5.506F), new Vector2(137.859F, -4.766F));
                    builder.AddCubicBezier(new Vector2(137.859F, -4.766F), new Vector2(133.539F, -4.766F), new Vector2(133.539F, -4.766F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0015 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0016()
            {
                if (_canvasGeometry_0016 != null)
                {
                    return _canvasGeometry_0016;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(139.172F, -4.352F));
                    builder.AddCubicBezier(new Vector2(139.172F, -5.555F), new Vector2(138.886F, -6.495F), new Vector2(138.313F, -7.172F));
                    builder.AddCubicBezier(new Vector2(137.74F, -7.849F), new Vector2(136.93F, -8.188F), new Vector2(135.883F, -8.188F));
                    builder.AddCubicBezier(new Vector2(134.836F, -8.188F), new Vector2(133.96F, -7.796F), new Vector2(133.254F, -7.012F));
                    builder.AddCubicBezier(new Vector2(132.548F, -6.228F), new Vector2(132.195F, -5.214F), new Vector2(132.195F, -3.969F));
                    builder.AddCubicBezier(new Vector2(132.195F, -2.651F), new Vector2(132.518F, -1.629F), new Vector2(133.164F, -0.902F));
                    builder.AddCubicBezier(new Vector2(133.81F, -0.175F), new Vector2(134.698F, 0.188F), new Vector2(135.828F, 0.188F));
                    builder.AddCubicBezier(new Vector2(136.984F, 0.188F), new Vector2(137.914F, -0.068F), new Vector2(138.617F, -0.578F));
                    builder.AddCubicBezier(new Vector2(138.617F, -0.578F), new Vector2(138.617F, -1.781F), new Vector2(138.617F, -1.781F));
                    builder.AddCubicBezier(new Vector2(137.862F, -1.187F), new Vector2(137.034F, -0.891F), new Vector2(136.133F, -0.891F));
                    builder.AddCubicBezier(new Vector2(135.331F, -0.891F), new Vector2(134.7F, -1.133F), new Vector2(134.242F, -1.617F));
                    builder.AddCubicBezier(new Vector2(133.784F, -2.101F), new Vector2(133.544F, -2.789F), new Vector2(133.523F, -3.68F));
                    builder.AddCubicBezier(new Vector2(133.523F, -3.68F), new Vector2(139.172F, -3.68F), new Vector2(139.172F, -3.68F));
                    builder.AddCubicBezier(new Vector2(139.172F, -3.68F), new Vector2(139.172F, -4.352F), new Vector2(139.172F, -4.352F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0016 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0017()
            {
                if (_canvasGeometry_0017 != null)
                {
                    return _canvasGeometry_0017;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(130.148F, -11.844F));
                    builder.AddCubicBezier(new Vector2(130.148F, -11.844F), new Vector2(128.867F, -11.844F), new Vector2(128.867F, -11.844F));
                    builder.AddCubicBezier(new Vector2(128.867F, -11.844F), new Vector2(128.867F, 0), new Vector2(128.867F, 0));
                    builder.AddCubicBezier(new Vector2(128.867F, 0), new Vector2(130.148F, 0), new Vector2(130.148F, 0));
                    builder.AddCubicBezier(new Vector2(130.148F, 0), new Vector2(130.148F, -11.844F), new Vector2(130.148F, -11.844F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0017 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0018()
            {
                var result = CanvasGeometry_0019().
                    CombineWith(CanvasGeometry_0020(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0019()
            {
                if (_canvasGeometry_0019 != null)
                {
                    return _canvasGeometry_0019;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(126.273F, -8));
                    builder.AddCubicBezier(new Vector2(126.273F, -8), new Vector2(124.992F, -8), new Vector2(124.992F, -8));
                    builder.AddCubicBezier(new Vector2(124.992F, -8), new Vector2(124.992F, 0), new Vector2(124.992F, 0));
                    builder.AddCubicBezier(new Vector2(124.992F, 0), new Vector2(126.273F, 0), new Vector2(126.273F, 0));
                    builder.AddCubicBezier(new Vector2(126.273F, 0), new Vector2(126.273F, -8), new Vector2(126.273F, -8));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0019 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0020()
            {
                if (_canvasGeometry_0020 != null)
                {
                    return _canvasGeometry_0020;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(126.246F, -10.273F));
                    builder.AddCubicBezier(new Vector2(126.41F, -10.434F), new Vector2(126.492F, -10.63F), new Vector2(126.492F, -10.859F));
                    builder.AddCubicBezier(new Vector2(126.492F, -11.099F), new Vector2(126.41F, -11.298F), new Vector2(126.246F, -11.457F));
                    builder.AddCubicBezier(new Vector2(126.082F, -11.616F), new Vector2(125.882F, -11.695F), new Vector2(125.648F, -11.695F));
                    builder.AddCubicBezier(new Vector2(125.419F, -11.695F), new Vector2(125.224F, -11.616F), new Vector2(125.063F, -11.457F));
                    builder.AddCubicBezier(new Vector2(124.902F, -11.298F), new Vector2(124.82F, -11.099F), new Vector2(124.82F, -10.859F));
                    builder.AddCubicBezier(new Vector2(124.82F, -10.619F), new Vector2(124.902F, -10.422F), new Vector2(125.063F, -10.266F));
                    builder.AddCubicBezier(new Vector2(125.224F, -10.11F), new Vector2(125.419F, -10.031F), new Vector2(125.648F, -10.031F));
                    builder.AddCubicBezier(new Vector2(125.882F, -10.031F), new Vector2(126.082F, -10.112F), new Vector2(126.246F, -10.273F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0020 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0021()
            {
                if (_canvasGeometry_0021 != null)
                {
                    return _canvasGeometry_0021;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(123.938F, -11.875F));
                    builder.AddCubicBezier(new Vector2(123.709F, -11.969F), new Vector2(123.399F, -12.016F), new Vector2(123.008F, -12.016F));
                    builder.AddCubicBezier(new Vector2(122.284F, -12.016F), new Vector2(121.679F, -11.772F), new Vector2(121.195F, -11.285F));
                    builder.AddCubicBezier(new Vector2(120.711F, -10.798F), new Vector2(120.469F, -10.136F), new Vector2(120.469F, -9.297F));
                    builder.AddCubicBezier(new Vector2(120.469F, -9.297F), new Vector2(120.469F, -8), new Vector2(120.469F, -8));
                    builder.AddCubicBezier(new Vector2(120.469F, -8), new Vector2(119.102F, -8), new Vector2(119.102F, -8));
                    builder.AddCubicBezier(new Vector2(119.102F, -8), new Vector2(119.102F, -6.906F), new Vector2(119.102F, -6.906F));
                    builder.AddCubicBezier(new Vector2(119.102F, -6.906F), new Vector2(120.469F, -6.906F), new Vector2(120.469F, -6.906F));
                    builder.AddCubicBezier(new Vector2(120.469F, -6.906F), new Vector2(120.469F, 0), new Vector2(120.469F, 0));
                    builder.AddCubicBezier(new Vector2(120.469F, 0), new Vector2(121.742F, 0), new Vector2(121.742F, 0));
                    builder.AddCubicBezier(new Vector2(121.742F, 0), new Vector2(121.742F, -6.906F), new Vector2(121.742F, -6.906F));
                    builder.AddCubicBezier(new Vector2(121.742F, -6.906F), new Vector2(123.617F, -6.906F), new Vector2(123.617F, -6.906F));
                    builder.AddCubicBezier(new Vector2(123.617F, -6.906F), new Vector2(123.617F, -8), new Vector2(123.617F, -8));
                    builder.AddCubicBezier(new Vector2(123.617F, -8), new Vector2(121.742F, -8), new Vector2(121.742F, -8));
                    builder.AddCubicBezier(new Vector2(121.742F, -8), new Vector2(121.742F, -9.234F), new Vector2(121.742F, -9.234F));
                    builder.AddCubicBezier(new Vector2(121.742F, -10.364F), new Vector2(122.19F, -10.93F), new Vector2(123.086F, -10.93F));
                    builder.AddCubicBezier(new Vector2(123.404F, -10.93F), new Vector2(123.688F, -10.86F), new Vector2(123.938F, -10.719F));
                    builder.AddCubicBezier(new Vector2(123.938F, -10.719F), new Vector2(123.938F, -11.875F), new Vector2(123.938F, -11.875F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0021 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0022()
            {
                if (_canvasGeometry_0022 != null)
                {
                    return _canvasGeometry_0022;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(112.836F, -11.203F));
                    builder.AddCubicBezier(new Vector2(112.836F, -11.203F), new Vector2(111.523F, -11.203F), new Vector2(111.523F, -11.203F));
                    builder.AddCubicBezier(new Vector2(111.523F, -11.203F), new Vector2(111.523F, -3.313F), new Vector2(111.523F, -3.313F));
                    builder.AddCubicBezier(new Vector2(111.523F, -2.537F), new Vector2(111.55F, -1.987F), new Vector2(111.602F, -1.664F));
                    builder.AddCubicBezier(new Vector2(111.602F, -1.664F), new Vector2(111.57F, -1.664F), new Vector2(111.57F, -1.664F));
                    builder.AddCubicBezier(new Vector2(111.502F, -1.799F), new Vector2(111.351F, -2.049F), new Vector2(111.117F, -2.414F));
                    builder.AddCubicBezier(new Vector2(111.117F, -2.414F), new Vector2(105.508F, -11.203F), new Vector2(105.508F, -11.203F));
                    builder.AddCubicBezier(new Vector2(105.508F, -11.203F), new Vector2(103.805F, -11.203F), new Vector2(103.805F, -11.203F));
                    builder.AddCubicBezier(new Vector2(103.805F, -11.203F), new Vector2(103.805F, 0), new Vector2(103.805F, 0));
                    builder.AddCubicBezier(new Vector2(103.805F, 0), new Vector2(105.117F, 0), new Vector2(105.117F, 0));
                    builder.AddCubicBezier(new Vector2(105.117F, 0), new Vector2(105.117F, -8.094F), new Vector2(105.117F, -8.094F));
                    builder.AddCubicBezier(new Vector2(105.117F, -8.88F), new Vector2(105.097F, -9.393F), new Vector2(105.055F, -9.633F));
                    builder.AddCubicBezier(new Vector2(105.055F, -9.633F), new Vector2(105.102F, -9.633F), new Vector2(105.102F, -9.633F));
                    builder.AddCubicBezier(new Vector2(105.196F, -9.388F), new Vector2(105.315F, -9.154F), new Vector2(105.461F, -8.93F));
                    builder.AddCubicBezier(new Vector2(105.461F, -8.93F), new Vector2(111.227F, 0), new Vector2(111.227F, 0));
                    builder.AddCubicBezier(new Vector2(111.227F, 0), new Vector2(112.836F, 0), new Vector2(112.836F, 0));
                    builder.AddCubicBezier(new Vector2(112.836F, 0), new Vector2(112.836F, -11.203F), new Vector2(112.836F, -11.203F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0022 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0023()
            {
                var result = CanvasGeometry_0024().
                    CombineWith(CanvasGeometry_0025(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0024()
            {
                if (_canvasGeometry_0024 != null)
                {
                    return _canvasGeometry_0024;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(99.195F, -8.992F));
                    builder.AddCubicBezier(new Vector2(99.883F, -8.185F), new Vector2(100.227F, -7.042F), new Vector2(100.227F, -5.563F));
                    builder.AddCubicBezier(new Vector2(100.227F, -4.12F), new Vector2(99.872F, -2.997F), new Vector2(99.164F, -2.195F));
                    builder.AddCubicBezier(new Vector2(98.456F, -1.393F), new Vector2(97.487F, -0.992F), new Vector2(96.258F, -0.992F));
                    builder.AddCubicBezier(new Vector2(95.107F, -0.992F), new Vector2(94.173F, -1.413F), new Vector2(93.457F, -2.254F));
                    builder.AddCubicBezier(new Vector2(92.741F, -3.095F), new Vector2(92.383F, -4.206F), new Vector2(92.383F, -5.586F));
                    builder.AddCubicBezier(new Vector2(92.383F, -6.966F), new Vector2(92.75F, -8.081F), new Vector2(93.484F, -8.93F));
                    builder.AddCubicBezier(new Vector2(94.218F, -9.779F), new Vector2(95.175F, -10.203F), new Vector2(96.352F, -10.203F));
                    builder.AddCubicBezier(new Vector2(97.56F, -10.203F), new Vector2(98.507F, -9.799F), new Vector2(99.195F, -8.992F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0024 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0025()
            {
                if (_canvasGeometry_0025 != null)
                {
                    return _canvasGeometry_0025;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(100.148F, -1.406F));
                    builder.AddCubicBezier(new Vector2(101.117F, -2.469F), new Vector2(101.602F, -3.914F), new Vector2(101.602F, -5.742F));
                    builder.AddCubicBezier(new Vector2(101.602F, -7.424F), new Vector2(101.129F, -8.786F), new Vector2(100.184F, -9.828F));
                    builder.AddCubicBezier(new Vector2(99.239F, -10.87F), new Vector2(97.992F, -11.391F), new Vector2(96.445F, -11.391F));
                    builder.AddCubicBezier(new Vector2(94.768F, -11.391F), new Vector2(93.443F, -10.854F), new Vector2(92.469F, -9.781F));
                    builder.AddCubicBezier(new Vector2(91.495F, -8.708F), new Vector2(91.008F, -7.271F), new Vector2(91.008F, -5.469F));
                    builder.AddCubicBezier(new Vector2(91.008F, -3.792F), new Vector2(91.485F, -2.43F), new Vector2(92.441F, -1.383F));
                    builder.AddCubicBezier(new Vector2(93.397F, -0.336F), new Vector2(94.669F, 0.188F), new Vector2(96.258F, 0.188F));
                    builder.AddCubicBezier(new Vector2(97.883F, 0.188F), new Vector2(99.179F, -0.343F), new Vector2(100.148F, -1.406F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0025 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0026()
            {
                if (_canvasGeometry_0026 != null)
                {
                    return _canvasGeometry_0026;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(83.301F, -0.184F));
                    builder.AddCubicBezier(new Vector2(83.543F, -0.103F), new Vector2(83.799F, -0.035F), new Vector2(84.07F, 0.02F));
                    builder.AddCubicBezier(new Vector2(84.341F, 0.075F), new Vector2(84.607F, 0.116F), new Vector2(84.867F, 0.145F));
                    builder.AddCubicBezier(new Vector2(85.127F, 0.174F), new Vector2(85.346F, 0.188F), new Vector2(85.523F, 0.188F));
                    builder.AddCubicBezier(new Vector2(86.054F, 0.188F), new Vector2(86.56F, 0.134F), new Vector2(87.039F, 0.027F));
                    builder.AddCubicBezier(new Vector2(87.518F, -0.08F), new Vector2(87.942F, -0.252F), new Vector2(88.309F, -0.492F));
                    builder.AddCubicBezier(new Vector2(88.676F, -0.732F), new Vector2(88.968F, -1.043F), new Vector2(89.184F, -1.426F));
                    builder.AddCubicBezier(new Vector2(89.4F, -1.809F), new Vector2(89.508F, -2.276F), new Vector2(89.508F, -2.828F));
                    builder.AddCubicBezier(new Vector2(89.508F, -3.245F), new Vector2(89.429F, -3.615F), new Vector2(89.27F, -3.938F));
                    builder.AddCubicBezier(new Vector2(89.111F, -4.261F), new Vector2(88.896F, -4.554F), new Vector2(88.625F, -4.82F));
                    builder.AddCubicBezier(new Vector2(88.354F, -5.086F), new Vector2(88.039F, -5.328F), new Vector2(87.68F, -5.547F));
                    builder.AddCubicBezier(new Vector2(87.321F, -5.766F), new Vector2(86.94F, -5.974F), new Vector2(86.539F, -6.172F));
                    builder.AddCubicBezier(new Vector2(86.148F, -6.365F), new Vector2(85.803F, -6.542F), new Vector2(85.504F, -6.703F));
                    builder.AddCubicBezier(new Vector2(85.204F, -6.864F), new Vector2(84.95F, -7.031F), new Vector2(84.742F, -7.203F));
                    builder.AddCubicBezier(new Vector2(84.534F, -7.375F), new Vector2(84.377F, -7.565F), new Vector2(84.27F, -7.773F));
                    builder.AddCubicBezier(new Vector2(84.163F, -7.981F), new Vector2(84.109F, -8.23F), new Vector2(84.109F, -8.516F));
                    builder.AddCubicBezier(new Vector2(84.109F, -8.823F), new Vector2(84.179F, -9.083F), new Vector2(84.32F, -9.297F));
                    builder.AddCubicBezier(new Vector2(84.461F, -9.511F), new Vector2(84.643F, -9.685F), new Vector2(84.867F, -9.82F));
                    builder.AddCubicBezier(new Vector2(85.091F, -9.955F), new Vector2(85.347F, -10.053F), new Vector2(85.633F, -10.113F));
                    builder.AddCubicBezier(new Vector2(85.919F, -10.173F), new Vector2(86.206F, -10.203F), new Vector2(86.492F, -10.203F));
                    builder.AddCubicBezier(new Vector2(87.528F, -10.203F), new Vector2(88.378F, -9.974F), new Vector2(89.039F, -9.516F));
                    builder.AddCubicBezier(new Vector2(89.039F, -9.516F), new Vector2(89.039F, -10.992F), new Vector2(89.039F, -10.992F));
                    builder.AddCubicBezier(new Vector2(88.534F, -11.258F), new Vector2(87.729F, -11.391F), new Vector2(86.625F, -11.391F));
                    builder.AddCubicBezier(new Vector2(86.141F, -11.391F), new Vector2(85.665F, -11.331F), new Vector2(85.199F, -11.211F));
                    builder.AddCubicBezier(new Vector2(84.733F, -11.091F), new Vector2(84.318F, -10.909F), new Vector2(83.953F, -10.664F));
                    builder.AddCubicBezier(new Vector2(83.588F, -10.419F), new Vector2(83.294F, -10.108F), new Vector2(83.07F, -9.73F));
                    builder.AddCubicBezier(new Vector2(82.846F, -9.352F), new Vector2(82.734F, -8.908F), new Vector2(82.734F, -8.398F));
                    builder.AddCubicBezier(new Vector2(82.734F, -7.981F), new Vector2(82.806F, -7.619F), new Vector2(82.949F, -7.309F));
                    builder.AddCubicBezier(new Vector2(83.092F, -6.999F), new Vector2(83.289F, -6.722F), new Vector2(83.539F, -6.477F));
                    builder.AddCubicBezier(new Vector2(83.789F, -6.232F), new Vector2(84.083F, -6.008F), new Vector2(84.422F, -5.805F));
                    builder.AddCubicBezier(new Vector2(84.761F, -5.602F), new Vector2(85.125F, -5.401F), new Vector2(85.516F, -5.203F));
                    builder.AddCubicBezier(new Vector2(85.886F, -5.015F), new Vector2(86.231F, -4.837F), new Vector2(86.551F, -4.668F));
                    builder.AddCubicBezier(new Vector2(86.871F, -4.499F), new Vector2(87.149F, -4.322F), new Vector2(87.383F, -4.137F));
                    builder.AddCubicBezier(new Vector2(87.617F, -3.952F), new Vector2(87.801F, -3.747F), new Vector2(87.934F, -3.523F));
                    builder.AddCubicBezier(new Vector2(88.067F, -3.299F), new Vector2(88.133F, -3.036F), new Vector2(88.133F, -2.734F));
                    builder.AddCubicBezier(new Vector2(88.133F, -2.171F), new Vector2(87.933F, -1.74F), new Vector2(87.535F, -1.441F));
                    builder.AddCubicBezier(new Vector2(87.137F, -1.141F), new Vector2(86.534F, -0.992F), new Vector2(85.727F, -0.992F));
                    builder.AddCubicBezier(new Vector2(85.493F, -0.992F), new Vector2(85.237F, -1.016F), new Vector2(84.961F, -1.063F));
                    builder.AddCubicBezier(new Vector2(84.685F, -1.11F), new Vector2(84.41F, -1.176F), new Vector2(84.137F, -1.262F));
                    builder.AddCubicBezier(new Vector2(83.864F, -1.348F), new Vector2(83.602F, -1.453F), new Vector2(83.355F, -1.578F));
                    builder.AddCubicBezier(new Vector2(83.108F, -1.703F), new Vector2(82.896F, -1.844F), new Vector2(82.719F, -2));
                    builder.AddCubicBezier(new Vector2(82.719F, -2), new Vector2(82.719F, -0.453F), new Vector2(82.719F, -0.453F));
                    builder.AddCubicBezier(new Vector2(82.865F, -0.354F), new Vector2(83.059F, -0.265F), new Vector2(83.301F, -0.184F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0026 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0027()
            {
                if (_canvasGeometry_0027 != null)
                {
                    return _canvasGeometry_0027;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(80.398F, -11.203F));
                    builder.AddCubicBezier(new Vector2(80.398F, -11.203F), new Vector2(79.086F, -11.203F), new Vector2(79.086F, -11.203F));
                    builder.AddCubicBezier(new Vector2(79.086F, -11.203F), new Vector2(79.086F, -4), new Vector2(79.086F, -4));
                    builder.AddCubicBezier(new Vector2(79.086F, -1.995F), new Vector2(78.495F, -0.992F), new Vector2(77.313F, -0.992F));
                    builder.AddCubicBezier(new Vector2(76.865F, -0.992F), new Vector2(76.5F, -1.094F), new Vector2(76.219F, -1.297F));
                    builder.AddCubicBezier(new Vector2(76.219F, -1.297F), new Vector2(76.219F, 0), new Vector2(76.219F, 0));
                    builder.AddCubicBezier(new Vector2(76.5F, 0.125F), new Vector2(76.859F, 0.188F), new Vector2(77.297F, 0.188F));
                    builder.AddCubicBezier(new Vector2(78.24F, 0.188F), new Vector2(78.992F, -0.181F), new Vector2(79.555F, -0.918F));
                    builder.AddCubicBezier(new Vector2(80.118F, -1.655F), new Vector2(80.398F, -2.688F), new Vector2(80.398F, -4.016F));
                    builder.AddCubicBezier(new Vector2(80.398F, -4.016F), new Vector2(80.398F, -11.203F), new Vector2(80.398F, -11.203F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0027 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0028()
            {
                if (_canvasGeometry_0028 != null)
                {
                    return _canvasGeometry_0028;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(71.586F, -8.031F));
                    builder.AddCubicBezier(new Vector2(71.409F, -8.104F), new Vector2(71.153F, -8.141F), new Vector2(70.82F, -8.141F));
                    builder.AddCubicBezier(new Vector2(70.356F, -8.141F), new Vector2(69.938F, -7.983F), new Vector2(69.563F, -7.668F));
                    builder.AddCubicBezier(new Vector2(69.188F, -7.353F), new Vector2(68.909F, -6.915F), new Vector2(68.727F, -6.352F));
                    builder.AddCubicBezier(new Vector2(68.727F, -6.352F), new Vector2(68.695F, -6.352F), new Vector2(68.695F, -6.352F));
                    builder.AddCubicBezier(new Vector2(68.695F, -6.352F), new Vector2(68.695F, -8), new Vector2(68.695F, -8));
                    builder.AddCubicBezier(new Vector2(68.695F, -8), new Vector2(67.414F, -8), new Vector2(67.414F, -8));
                    builder.AddCubicBezier(new Vector2(67.414F, -8), new Vector2(67.414F, 0), new Vector2(67.414F, 0));
                    builder.AddCubicBezier(new Vector2(67.414F, 0), new Vector2(68.695F, 0), new Vector2(68.695F, 0));
                    builder.AddCubicBezier(new Vector2(68.695F, 0), new Vector2(68.695F, -4.078F), new Vector2(68.695F, -4.078F));
                    builder.AddCubicBezier(new Vector2(68.695F, -4.969F), new Vector2(68.879F, -5.672F), new Vector2(69.246F, -6.188F));
                    builder.AddCubicBezier(new Vector2(69.613F, -6.704F), new Vector2(70.07F, -6.961F), new Vector2(70.617F, -6.961F));
                    builder.AddCubicBezier(new Vector2(71.039F, -6.961F), new Vector2(71.362F, -6.875F), new Vector2(71.586F, -6.703F));
                    builder.AddCubicBezier(new Vector2(71.586F, -6.703F), new Vector2(71.586F, -8.031F), new Vector2(71.586F, -8.031F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0028 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0029()
            {
                if (_canvasGeometry_0029 != null)
                {
                    return _canvasGeometry_0029;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(64.828F, -8));
                    builder.AddCubicBezier(new Vector2(64.828F, -8), new Vector2(63.547F, -8), new Vector2(63.547F, -8));
                    builder.AddCubicBezier(new Vector2(63.547F, -8), new Vector2(63.547F, -3.391F), new Vector2(63.547F, -3.391F));
                    builder.AddCubicBezier(new Vector2(63.547F, -2.646F), new Vector2(63.345F, -2.043F), new Vector2(62.941F, -1.582F));
                    builder.AddCubicBezier(new Vector2(62.537F, -1.121F), new Vector2(62.023F, -0.891F), new Vector2(61.398F, -0.891F));
                    builder.AddCubicBezier(new Vector2(60.106F, -0.891F), new Vector2(59.461F, -1.734F), new Vector2(59.461F, -3.422F));
                    builder.AddCubicBezier(new Vector2(59.461F, -3.422F), new Vector2(59.461F, -8), new Vector2(59.461F, -8));
                    builder.AddCubicBezier(new Vector2(59.461F, -8), new Vector2(58.188F, -8), new Vector2(58.188F, -8));
                    builder.AddCubicBezier(new Vector2(58.188F, -8), new Vector2(58.188F, -3.219F), new Vector2(58.188F, -3.219F));
                    builder.AddCubicBezier(new Vector2(58.188F, -0.948F), new Vector2(59.141F, 0.188F), new Vector2(61.047F, 0.188F));
                    builder.AddCubicBezier(new Vector2(62.162F, 0.188F), new Vector2(62.985F, -0.297F), new Vector2(63.516F, -1.266F));
                    builder.AddCubicBezier(new Vector2(63.516F, -1.266F), new Vector2(63.547F, -1.266F), new Vector2(63.547F, -1.266F));
                    builder.AddCubicBezier(new Vector2(63.547F, -1.266F), new Vector2(63.547F, 0), new Vector2(63.547F, 0));
                    builder.AddCubicBezier(new Vector2(63.547F, 0), new Vector2(64.828F, 0), new Vector2(64.828F, 0));
                    builder.AddCubicBezier(new Vector2(64.828F, 0), new Vector2(64.828F, -8), new Vector2(64.828F, -8));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0029 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0030()
            {
                var result = CanvasGeometry_0031().
                    CombineWith(CanvasGeometry_0032(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0031()
            {
                if (_canvasGeometry_0031 != null)
                {
                    return _canvasGeometry_0031;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(54.332F, -6.297F));
                    builder.AddCubicBezier(new Vector2(54.777F, -5.755F), new Vector2(55, -4.984F), new Vector2(55, -3.984F));
                    builder.AddCubicBezier(new Vector2(55, -2.994F), new Vector2(54.777F, -2.231F), new Vector2(54.332F, -1.695F));
                    builder.AddCubicBezier(new Vector2(53.887F, -1.159F), new Vector2(53.25F, -0.891F), new Vector2(52.422F, -0.891F));
                    builder.AddCubicBezier(new Vector2(51.609F, -0.891F), new Vector2(50.961F, -1.164F), new Vector2(50.477F, -1.711F));
                    builder.AddCubicBezier(new Vector2(49.993F, -2.258F), new Vector2(49.75F, -3.005F), new Vector2(49.75F, -3.953F));
                    builder.AddCubicBezier(new Vector2(49.75F, -4.937F), new Vector2(49.99F, -5.71F), new Vector2(50.469F, -6.27F));
                    builder.AddCubicBezier(new Vector2(50.948F, -6.83F), new Vector2(51.599F, -7.109F), new Vector2(52.422F, -7.109F));
                    builder.AddCubicBezier(new Vector2(53.25F, -7.109F), new Vector2(53.887F, -6.839F), new Vector2(54.332F, -6.297F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0031 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0032()
            {
                if (_canvasGeometry_0032 != null)
                {
                    return _canvasGeometry_0032;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(55.23F, -0.965F));
                    builder.AddCubicBezier(new Vector2(55.951F, -1.733F), new Vector2(56.313F, -2.755F), new Vector2(56.313F, -4.031F));
                    builder.AddCubicBezier(new Vector2(56.313F, -5.333F), new Vector2(55.978F, -6.352F), new Vector2(55.309F, -7.086F));
                    builder.AddCubicBezier(new Vector2(54.64F, -7.82F), new Vector2(53.709F, -8.188F), new Vector2(52.516F, -8.188F));
                    builder.AddCubicBezier(new Vector2(51.266F, -8.188F), new Vector2(50.273F, -7.81F), new Vector2(49.539F, -7.055F));
                    builder.AddCubicBezier(new Vector2(48.805F, -6.3F), new Vector2(48.438F, -5.25F), new Vector2(48.438F, -3.906F));
                    builder.AddCubicBezier(new Vector2(48.438F, -2.672F), new Vector2(48.79F, -1.681F), new Vector2(49.496F, -0.934F));
                    builder.AddCubicBezier(new Vector2(50.202F, -0.187F), new Vector2(51.146F, 0.188F), new Vector2(52.328F, 0.188F));
                    builder.AddCubicBezier(new Vector2(53.542F, 0.188F), new Vector2(54.509F, -0.197F), new Vector2(55.23F, -0.965F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0032 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0033()
            {
                if (_canvasGeometry_0033 != null)
                {
                    return _canvasGeometry_0033;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(46.281F, -8));
                    builder.AddCubicBezier(new Vector2(46.281F, -8), new Vector2(44.008F, -1.828F), new Vector2(44.008F, -1.828F));
                    builder.AddCubicBezier(new Vector2(43.93F, -1.557F), new Vector2(43.878F, -1.359F), new Vector2(43.852F, -1.234F));
                    builder.AddCubicBezier(new Vector2(43.852F, -1.234F), new Vector2(43.805F, -1.234F), new Vector2(43.805F, -1.234F));
                    builder.AddCubicBezier(new Vector2(43.722F, -1.562F), new Vector2(43.667F, -1.766F), new Vector2(43.641F, -1.844F));
                    builder.AddCubicBezier(new Vector2(43.641F, -1.844F), new Vector2(41.477F, -8), new Vector2(41.477F, -8));
                    builder.AddCubicBezier(new Vector2(41.477F, -8), new Vector2(40.055F, -8), new Vector2(40.055F, -8));
                    builder.AddCubicBezier(new Vector2(40.055F, -8), new Vector2(43.18F, -0.016F), new Vector2(43.18F, -0.016F));
                    builder.AddCubicBezier(new Vector2(43.18F, -0.016F), new Vector2(42.539F, 1.5F), new Vector2(42.539F, 1.5F));
                    builder.AddCubicBezier(new Vector2(42.216F, 2.271F), new Vector2(41.732F, 2.656F), new Vector2(41.086F, 2.656F));
                    builder.AddCubicBezier(new Vector2(40.857F, 2.656F), new Vector2(40.604F, 2.61F), new Vector2(40.328F, 2.516F));
                    builder.AddCubicBezier(new Vector2(40.328F, 2.516F), new Vector2(40.328F, 3.664F), new Vector2(40.328F, 3.664F));
                    builder.AddCubicBezier(new Vector2(40.552F, 3.732F), new Vector2(40.831F, 3.766F), new Vector2(41.164F, 3.766F));
                    builder.AddCubicBezier(new Vector2(42.352F, 3.766F), new Vector2(43.274F, 2.937F), new Vector2(43.93F, 1.281F));
                    builder.AddCubicBezier(new Vector2(43.93F, 1.281F), new Vector2(47.609F, -8), new Vector2(47.609F, -8));
                    builder.AddCubicBezier(new Vector2(47.609F, -8), new Vector2(46.281F, -8), new Vector2(46.281F, -8));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0033 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0034()
            {
                var result = CanvasGeometry_0035().
                    CombineWith(CanvasGeometry_0036(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0035()
            {
                if (_canvasGeometry_0035 != null)
                {
                    return _canvasGeometry_0035;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(29.414F, -6.332F));
                    builder.AddCubicBezier(new Vector2(29.867F, -6.85F), new Vector2(30.466F, -7.109F), new Vector2(31.211F, -7.109F));
                    builder.AddCubicBezier(new Vector2(31.914F, -7.109F), new Vector2(32.472F, -6.857F), new Vector2(32.883F, -6.352F));
                    builder.AddCubicBezier(new Vector2(33.294F, -5.847F), new Vector2(33.5F, -5.149F), new Vector2(33.5F, -4.258F));
                    builder.AddCubicBezier(new Vector2(33.5F, -3.201F), new Vector2(33.28F, -2.375F), new Vector2(32.84F, -1.781F));
                    builder.AddCubicBezier(new Vector2(32.4F, -1.187F), new Vector2(31.792F, -0.891F), new Vector2(31.016F, -0.891F));
                    builder.AddCubicBezier(new Vector2(30.355F, -0.891F), new Vector2(29.809F, -1.121F), new Vector2(29.379F, -1.582F));
                    builder.AddCubicBezier(new Vector2(28.949F, -2.043F), new Vector2(28.734F, -2.605F), new Vector2(28.734F, -3.266F));
                    builder.AddCubicBezier(new Vector2(28.734F, -3.266F), new Vector2(28.734F, -4.383F), new Vector2(28.734F, -4.383F));
                    builder.AddCubicBezier(new Vector2(28.734F, -5.164F), new Vector2(28.961F, -5.814F), new Vector2(29.414F, -6.332F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0035 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0036()
            {
                if (_canvasGeometry_0036 != null)
                {
                    return _canvasGeometry_0036;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(31.164F, 0.188F));
                    builder.AddCubicBezier(new Vector2(32.294F, 0.188F), new Vector2(33.185F, -0.215F), new Vector2(33.836F, -1.02F));
                    builder.AddCubicBezier(new Vector2(34.487F, -1.825F), new Vector2(34.813F, -2.895F), new Vector2(34.813F, -4.234F));
                    builder.AddCubicBezier(new Vector2(34.813F, -5.437F), new Vector2(34.523F, -6.397F), new Vector2(33.945F, -7.113F));
                    builder.AddCubicBezier(new Vector2(33.367F, -7.829F), new Vector2(32.562F, -8.188F), new Vector2(31.531F, -8.188F));
                    builder.AddCubicBezier(new Vector2(30.317F, -8.188F), new Vector2(29.396F, -7.657F), new Vector2(28.766F, -6.594F));
                    builder.AddCubicBezier(new Vector2(28.766F, -6.594F), new Vector2(28.734F, -6.594F), new Vector2(28.734F, -6.594F));
                    builder.AddCubicBezier(new Vector2(28.734F, -6.594F), new Vector2(28.734F, -8), new Vector2(28.734F, -8));
                    builder.AddCubicBezier(new Vector2(28.734F, -8), new Vector2(27.453F, -8), new Vector2(27.453F, -8));
                    builder.AddCubicBezier(new Vector2(27.453F, -8), new Vector2(27.453F, 3.68F), new Vector2(27.453F, 3.68F));
                    builder.AddCubicBezier(new Vector2(27.453F, 3.68F), new Vector2(28.734F, 3.68F), new Vector2(28.734F, 3.68F));
                    builder.AddCubicBezier(new Vector2(28.734F, 3.68F), new Vector2(28.734F, -1.156F), new Vector2(28.734F, -1.156F));
                    builder.AddCubicBezier(new Vector2(28.734F, -1.156F), new Vector2(28.766F, -1.156F), new Vector2(28.766F, -1.156F));
                    builder.AddCubicBezier(new Vector2(29.329F, -0.26F), new Vector2(30.128F, 0.188F), new Vector2(31.164F, 0.188F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0036 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0037()
            {
                var result = CanvasGeometry_0038().
                    CombineWith(CanvasGeometry_0039(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0038()
            {
                if (_canvasGeometry_0038 != null)
                {
                    return _canvasGeometry_0038;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(23.426F, -6.297F));
                    builder.AddCubicBezier(new Vector2(23.871F, -5.755F), new Vector2(24.094F, -4.984F), new Vector2(24.094F, -3.984F));
                    builder.AddCubicBezier(new Vector2(24.094F, -2.994F), new Vector2(23.871F, -2.231F), new Vector2(23.426F, -1.695F));
                    builder.AddCubicBezier(new Vector2(22.981F, -1.159F), new Vector2(22.344F, -0.891F), new Vector2(21.516F, -0.891F));
                    builder.AddCubicBezier(new Vector2(20.703F, -0.891F), new Vector2(20.054F, -1.164F), new Vector2(19.57F, -1.711F));
                    builder.AddCubicBezier(new Vector2(19.086F, -2.258F), new Vector2(18.844F, -3.005F), new Vector2(18.844F, -3.953F));
                    builder.AddCubicBezier(new Vector2(18.844F, -4.937F), new Vector2(19.084F, -5.71F), new Vector2(19.563F, -6.27F));
                    builder.AddCubicBezier(new Vector2(20.042F, -6.83F), new Vector2(20.693F, -7.109F), new Vector2(21.516F, -7.109F));
                    builder.AddCubicBezier(new Vector2(22.344F, -7.109F), new Vector2(22.981F, -6.839F), new Vector2(23.426F, -6.297F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0038 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0039()
            {
                if (_canvasGeometry_0039 != null)
                {
                    return _canvasGeometry_0039;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(24.324F, -0.965F));
                    builder.AddCubicBezier(new Vector2(25.045F, -1.733F), new Vector2(25.406F, -2.755F), new Vector2(25.406F, -4.031F));
                    builder.AddCubicBezier(new Vector2(25.406F, -5.333F), new Vector2(25.071F, -6.352F), new Vector2(24.402F, -7.086F));
                    builder.AddCubicBezier(new Vector2(23.733F, -7.82F), new Vector2(22.802F, -8.188F), new Vector2(21.609F, -8.188F));
                    builder.AddCubicBezier(new Vector2(20.359F, -8.188F), new Vector2(19.367F, -7.81F), new Vector2(18.633F, -7.055F));
                    builder.AddCubicBezier(new Vector2(17.899F, -6.3F), new Vector2(17.531F, -5.25F), new Vector2(17.531F, -3.906F));
                    builder.AddCubicBezier(new Vector2(17.531F, -2.672F), new Vector2(17.884F, -1.681F), new Vector2(18.59F, -0.934F));
                    builder.AddCubicBezier(new Vector2(19.296F, -0.187F), new Vector2(20.24F, 0.188F), new Vector2(21.422F, 0.188F));
                    builder.AddCubicBezier(new Vector2(22.636F, 0.188F), new Vector2(23.603F, -0.197F), new Vector2(24.324F, -0.965F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0039 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0040()
            {
                if (_canvasGeometry_0040 != null)
                {
                    return _canvasGeometry_0040;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(16.688F, -8.031F));
                    builder.AddCubicBezier(new Vector2(16.511F, -8.104F), new Vector2(16.255F, -8.141F), new Vector2(15.922F, -8.141F));
                    builder.AddCubicBezier(new Vector2(15.458F, -8.141F), new Vector2(15.039F, -7.983F), new Vector2(14.664F, -7.668F));
                    builder.AddCubicBezier(new Vector2(14.289F, -7.353F), new Vector2(14.01F, -6.915F), new Vector2(13.828F, -6.352F));
                    builder.AddCubicBezier(new Vector2(13.828F, -6.352F), new Vector2(13.797F, -6.352F), new Vector2(13.797F, -6.352F));
                    builder.AddCubicBezier(new Vector2(13.797F, -6.352F), new Vector2(13.797F, -8), new Vector2(13.797F, -8));
                    builder.AddCubicBezier(new Vector2(13.797F, -8), new Vector2(12.516F, -8), new Vector2(12.516F, -8));
                    builder.AddCubicBezier(new Vector2(12.516F, -8), new Vector2(12.516F, 0), new Vector2(12.516F, 0));
                    builder.AddCubicBezier(new Vector2(12.516F, 0), new Vector2(13.797F, 0), new Vector2(13.797F, 0));
                    builder.AddCubicBezier(new Vector2(13.797F, 0), new Vector2(13.797F, -4.078F), new Vector2(13.797F, -4.078F));
                    builder.AddCubicBezier(new Vector2(13.797F, -4.969F), new Vector2(13.981F, -5.672F), new Vector2(14.348F, -6.188F));
                    builder.AddCubicBezier(new Vector2(14.715F, -6.704F), new Vector2(15.172F, -6.961F), new Vector2(15.719F, -6.961F));
                    builder.AddCubicBezier(new Vector2(16.141F, -6.961F), new Vector2(16.464F, -6.875F), new Vector2(16.688F, -6.703F));
                    builder.AddCubicBezier(new Vector2(16.688F, -6.703F), new Vector2(16.688F, -8.031F), new Vector2(16.688F, -8.031F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0040 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0041()
            {
                var result = CanvasGeometry_0042().
                    CombineWith(CanvasGeometry_0043(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0042()
            {
                if (_canvasGeometry_0042 != null)
                {
                    return _canvasGeometry_0042;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(4.531F, -10.016F));
                    builder.AddCubicBezier(new Vector2(7.583F, -10.016F), new Vector2(9.109F, -8.581F), new Vector2(9.109F, -5.711F));
                    builder.AddCubicBezier(new Vector2(9.109F, -4.268F), new Vector2(8.701F, -3.153F), new Vector2(7.883F, -2.367F));
                    builder.AddCubicBezier(new Vector2(7.065F, -1.581F), new Vector2(5.922F, -1.188F), new Vector2(4.453F, -1.188F));
                    builder.AddCubicBezier(new Vector2(4.453F, -1.188F), new Vector2(2.781F, -1.188F), new Vector2(2.781F, -1.188F));
                    builder.AddCubicBezier(new Vector2(2.781F, -1.188F), new Vector2(2.781F, -10.016F), new Vector2(2.781F, -10.016F));
                    builder.AddCubicBezier(new Vector2(2.781F, -10.016F), new Vector2(4.531F, -10.016F), new Vector2(4.531F, -10.016F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0042 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0043()
            {
                if (_canvasGeometry_0043 != null)
                {
                    return _canvasGeometry_0043;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(4.438F, 0));
                    builder.AddCubicBezier(new Vector2(6.277F, 0), new Vector2(7.744F, -0.524F), new Vector2(8.84F, -1.574F));
                    builder.AddCubicBezier(new Vector2(9.936F, -2.623F), new Vector2(10.484F, -4.013F), new Vector2(10.484F, -5.742F));
                    builder.AddCubicBezier(new Vector2(10.484F, -9.383F), new Vector2(8.511F, -11.203F), new Vector2(4.563F, -11.203F));
                    builder.AddCubicBezier(new Vector2(4.563F, -11.203F), new Vector2(1.469F, -11.203F), new Vector2(1.469F, -11.203F));
                    builder.AddCubicBezier(new Vector2(1.469F, -11.203F), new Vector2(1.469F, 0), new Vector2(1.469F, 0));
                    builder.AddCubicBezier(new Vector2(1.469F, 0), new Vector2(4.438F, 0), new Vector2(4.438F, 0));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0043 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0044()
            {
                if (_canvasGeometry_0044 != null)
                {
                    return _canvasGeometry_0044;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-15, -115.5F));
                    builder.AddCubicBezier(new Vector2(-8.873F, -120.125F), new Vector2(3.38F, -129.375F), new Vector2(9.5F, -134));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return  _canvasGeometry_0044 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0045()
            {
                if (_canvasGeometry_0045 != null)
                {
                    return _canvasGeometry_0045;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-34, -135.25F));
                    builder.AddCubicBezier(new Vector2(-31.617F, -141.618F), new Vector2(-26.852F, -154.354F), new Vector2(-24.5F, -160.75F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return  _canvasGeometry_0045 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0046()
            {
                if (_canvasGeometry_0046 != null)
                {
                    return _canvasGeometry_0046;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-64.75F, -137));
                    builder.AddCubicBezier(new Vector2(-66.265F, -144.798F), new Vector2(-69.296F, -160.395F), new Vector2(-70.75F, -168.25F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return  _canvasGeometry_0046 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0047()
            {
                if (_canvasGeometry_0047 != null)
                {
                    return _canvasGeometry_0047;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-91.75F, -124.5F));
                    builder.AddCubicBezier(new Vector2(-96.125F, -127.752F), new Vector2(-104.875F, -134.255F), new Vector2(-109.25F, -137.5F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return  _canvasGeometry_0047 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0048()
            {
                if (_canvasGeometry_0048 != null)
                {
                    return _canvasGeometry_0048;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-9.25F, -53.75F));
                    builder.AddCubicBezier(new Vector2(-4.295F, -49.923F), new Vector2(5.614F, -42.27F), new Vector2(10.5F, -38.5F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return  _canvasGeometry_0048 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0049()
            {
                if (_canvasGeometry_0049 != null)
                {
                    return _canvasGeometry_0049;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-35.25F, -35.75F));
                    builder.AddCubicBezier(new Vector2(-33.377F, -30.25F), new Vector2(-29.63F, -19.25F), new Vector2(-27.75F, -13.75F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return  _canvasGeometry_0049 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0050()
            {
                if (_canvasGeometry_0050 != null)
                {
                    return _canvasGeometry_0050;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-72.75F, -33.25F));
                    builder.AddCubicBezier(new Vector2(-74.813F, -27.904F), new Vector2(-78.938F, -17.213F), new Vector2(-81, -11.75F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return  _canvasGeometry_0050 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0051()
            {
                if (_canvasGeometry_0051 != null)
                {
                    return _canvasGeometry_0051;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-91.5F, -53));
                    builder.AddCubicBezier(new Vector2(-95.938F, -50.623F), new Vector2(-104.812F, -45.87F), new Vector2(-109.25F, -43.5F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return  _canvasGeometry_0051 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0052()
            {
                if (_canvasGeometry_0052 != null)
                {
                    return _canvasGeometry_0052;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-11.75F, -84.75F));
                    builder.AddCubicBezier(new Vector2(-6.563F, -85.083F), new Vector2(3.812F, -85.75F), new Vector2(9, -86.25F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return  _canvasGeometry_0052 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0053()
            {
                if (_canvasGeometry_0053 != null)
                {
                    return _canvasGeometry_0053;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-89.5F, -88));
                    builder.AddCubicBezier(new Vector2(-95.563F, -88.314F), new Vector2(-107.688F, -88.943F), new Vector2(-113.75F, -89.25F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return  _canvasGeometry_0053 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0054()
            {
                var result = CanvasGeometry_0007().
                    CombineWith(CanvasGeometry_0008(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0055()
            {
                var result = CanvasGeometry_0011().
                    CombineWith(CanvasGeometry_0012(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0056()
            {
                var result = CanvasGeometry_0015().
                    CombineWith(CanvasGeometry_0016(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0057()
            {
                var result = CanvasGeometry_0019().
                    CombineWith(CanvasGeometry_0020(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0058()
            {
                var result = CanvasGeometry_0024().
                    CombineWith(CanvasGeometry_0025(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0059()
            {
                var result = CanvasGeometry_0031().
                    CombineWith(CanvasGeometry_0032(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0060()
            {
                var result = CanvasGeometry_0035().
                    CombineWith(CanvasGeometry_0036(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0061()
            {
                var result = CanvasGeometry_0038().
                    CombineWith(CanvasGeometry_0039(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0062()
            {
                var result = CanvasGeometry_0042().
                    CombineWith(CanvasGeometry_0043(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0063()
            {
                var result = CanvasGeometry_0007().
                    CombineWith(CanvasGeometry_0008(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0064()
            {
                var result = CanvasGeometry_0011().
                    CombineWith(CanvasGeometry_0012(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0065()
            {
                var result = CanvasGeometry_0015().
                    CombineWith(CanvasGeometry_0016(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0066()
            {
                var result = CanvasGeometry_0019().
                    CombineWith(CanvasGeometry_0020(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0067()
            {
                var result = CanvasGeometry_0024().
                    CombineWith(CanvasGeometry_0025(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0068()
            {
                var result = CanvasGeometry_0031().
                    CombineWith(CanvasGeometry_0032(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0069()
            {
                var result = CanvasGeometry_0035().
                    CombineWith(CanvasGeometry_0036(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0070()
            {
                var result = CanvasGeometry_0038().
                    CombineWith(CanvasGeometry_0039(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0071()
            {
                var result = CanvasGeometry_0042().
                    CombineWith(CanvasGeometry_0043(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0072()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-46.75F, -91.563F));
                    builder.AddCubicBezier(new Vector2(-57.25F, -64.313F), new Vector2(-73.25F, -57.5F), new Vector2(-92, -70.25F));
                    builder.AddCubicBezier(new Vector2(-110.222F, -82.641F), new Vector2(-100.132F, -105.161F), new Vector2(-89.25F, -111));
                    builder.AddCubicBezier(new Vector2(-79, -116.5F), new Vector2(-68, -114), new Vector2(-57.625F, -103.75F));
                    builder.AddCubicBezier(new Vector2(-51, -97.5F), new Vector2(-42.625F, -88.625F), new Vector2(-37.875F, -84.5F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0073()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-46.938F, -91.5F));
                    builder.AddCubicBezier(new Vector2(-43.438F, -70.75F), new Vector2(-23.884F, -54.847F), new Vector2(-6.625F, -66.75F));
                    builder.AddCubicBezier(new Vector2(7.875F, -76.75F), new Vector2(14.75F, -96), new Vector2(0.75F, -106.875F));
                    builder.AddCubicBezier(new Vector2(-10.628F, -115.713F), new Vector2(-32.603F, -109.976F), new Vector2(-40.312F, -101.813F));
                    builder.AddCubicBezier(new Vector2(-47.75F, -93.938F), new Vector2(-53.15F, -88.45F), new Vector2(-56.875F, -84.25F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0074()
            {
                var result = CanvasGeometry_0075().
                    CombineWith(CanvasGeometry_0076(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0075()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(172.086F, -0.109F));
                    builder.AddCubicBezier(new Vector2(172.258F, -0.276F), new Vector2(172.344F, -0.479F), new Vector2(172.344F, -0.719F));
                    builder.AddCubicBezier(new Vector2(172.344F, -0.959F), new Vector2(172.258F, -1.163F), new Vector2(172.086F, -1.332F));
                    builder.AddCubicBezier(new Vector2(171.914F, -1.501F), new Vector2(171.711F, -1.586F), new Vector2(171.477F, -1.586F));
                    builder.AddCubicBezier(new Vector2(171.237F, -1.586F), new Vector2(171.032F, -1.501F), new Vector2(170.863F, -1.332F));
                    builder.AddCubicBezier(new Vector2(170.694F, -1.163F), new Vector2(170.609F, -0.959F), new Vector2(170.609F, -0.719F));
                    builder.AddCubicBezier(new Vector2(170.609F, -0.479F), new Vector2(170.694F, -0.276F), new Vector2(170.863F, -0.109F));
                    builder.AddCubicBezier(new Vector2(171.032F, 0.058F), new Vector2(171.237F, 0.141F), new Vector2(171.477F, 0.141F));
                    builder.AddCubicBezier(new Vector2(171.711F, 0.141F), new Vector2(171.914F, 0.058F), new Vector2(172.086F, -0.109F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0076()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(170.789F, -11.203F));
                    builder.AddCubicBezier(new Vector2(170.789F, -11.203F), new Vector2(170.93F, -3.156F), new Vector2(170.93F, -3.156F));
                    builder.AddCubicBezier(new Vector2(170.93F, -3.156F), new Vector2(171.984F, -3.156F), new Vector2(171.984F, -3.156F));
                    builder.AddCubicBezier(new Vector2(171.984F, -3.156F), new Vector2(172.133F, -11.203F), new Vector2(172.133F, -11.203F));
                    builder.AddCubicBezier(new Vector2(172.133F, -11.203F), new Vector2(170.789F, -11.203F), new Vector2(170.789F, -11.203F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0077()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(168.086F, -4.891F));
                    builder.AddCubicBezier(new Vector2(168.086F, -5.959F), new Vector2(167.855F, -6.775F), new Vector2(167.391F, -7.34F));
                    builder.AddCubicBezier(new Vector2(166.927F, -7.905F), new Vector2(166.258F, -8.188F), new Vector2(165.383F, -8.188F));
                    builder.AddCubicBezier(new Vector2(164.237F, -8.188F), new Vector2(163.362F, -7.682F), new Vector2(162.758F, -6.672F));
                    builder.AddCubicBezier(new Vector2(162.758F, -6.672F), new Vector2(162.727F, -6.672F), new Vector2(162.727F, -6.672F));
                    builder.AddCubicBezier(new Vector2(162.727F, -6.672F), new Vector2(162.727F, -8), new Vector2(162.727F, -8));
                    builder.AddCubicBezier(new Vector2(162.727F, -8), new Vector2(161.445F, -8), new Vector2(161.445F, -8));
                    builder.AddCubicBezier(new Vector2(161.445F, -8), new Vector2(161.445F, 0), new Vector2(161.445F, 0));
                    builder.AddCubicBezier(new Vector2(161.445F, 0), new Vector2(162.727F, 0), new Vector2(162.727F, 0));
                    builder.AddCubicBezier(new Vector2(162.727F, 0), new Vector2(162.727F, -4.563F), new Vector2(162.727F, -4.563F));
                    builder.AddCubicBezier(new Vector2(162.727F, -5.297F), new Vector2(162.936F, -5.905F), new Vector2(163.355F, -6.387F));
                    builder.AddCubicBezier(new Vector2(163.774F, -6.869F), new Vector2(164.304F, -7.109F), new Vector2(164.945F, -7.109F));
                    builder.AddCubicBezier(new Vector2(166.185F, -7.109F), new Vector2(166.805F, -6.261F), new Vector2(166.805F, -4.563F));
                    builder.AddCubicBezier(new Vector2(166.805F, -4.563F), new Vector2(166.805F, 0), new Vector2(166.805F, 0));
                    builder.AddCubicBezier(new Vector2(166.805F, 0), new Vector2(168.086F, 0), new Vector2(168.086F, 0));
                    builder.AddCubicBezier(new Vector2(168.086F, 0), new Vector2(168.086F, -4.891F), new Vector2(168.086F, -4.891F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0078()
            {
                var result = CanvasGeometry_0079().
                    CombineWith(CanvasGeometry_0080(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0079()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(158.852F, -8));
                    builder.AddCubicBezier(new Vector2(158.852F, -8), new Vector2(157.57F, -8), new Vector2(157.57F, -8));
                    builder.AddCubicBezier(new Vector2(157.57F, -8), new Vector2(157.57F, 0), new Vector2(157.57F, 0));
                    builder.AddCubicBezier(new Vector2(157.57F, 0), new Vector2(158.852F, 0), new Vector2(158.852F, 0));
                    builder.AddCubicBezier(new Vector2(158.852F, 0), new Vector2(158.852F, -8), new Vector2(158.852F, -8));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0080()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(158.824F, -10.273F));
                    builder.AddCubicBezier(new Vector2(158.988F, -10.434F), new Vector2(159.07F, -10.63F), new Vector2(159.07F, -10.859F));
                    builder.AddCubicBezier(new Vector2(159.07F, -11.099F), new Vector2(158.988F, -11.298F), new Vector2(158.824F, -11.457F));
                    builder.AddCubicBezier(new Vector2(158.66F, -11.616F), new Vector2(158.461F, -11.695F), new Vector2(158.227F, -11.695F));
                    builder.AddCubicBezier(new Vector2(157.998F, -11.695F), new Vector2(157.802F, -11.616F), new Vector2(157.641F, -11.457F));
                    builder.AddCubicBezier(new Vector2(157.48F, -11.298F), new Vector2(157.398F, -11.099F), new Vector2(157.398F, -10.859F));
                    builder.AddCubicBezier(new Vector2(157.398F, -10.619F), new Vector2(157.48F, -10.422F), new Vector2(157.641F, -10.266F));
                    builder.AddCubicBezier(new Vector2(157.802F, -10.11F), new Vector2(157.998F, -10.031F), new Vector2(158.227F, -10.031F));
                    builder.AddCubicBezier(new Vector2(158.461F, -10.031F), new Vector2(158.66F, -10.112F), new Vector2(158.824F, -10.273F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0081()
            {
                var result = CanvasGeometry_0082().
                    CombineWith(CanvasGeometry_0083(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0082()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(153.875F, -3.25F));
                    builder.AddCubicBezier(new Vector2(153.875F, -2.568F), new Vector2(153.668F, -2.004F), new Vector2(153.254F, -1.559F));
                    builder.AddCubicBezier(new Vector2(152.84F, -1.114F), new Vector2(152.315F, -0.891F), new Vector2(151.68F, -0.891F));
                    builder.AddCubicBezier(new Vector2(151.216F, -0.891F), new Vector2(150.845F, -1.015F), new Vector2(150.566F, -1.262F));
                    builder.AddCubicBezier(new Vector2(150.287F, -1.509F), new Vector2(150.148F, -1.828F), new Vector2(150.148F, -2.219F));
                    builder.AddCubicBezier(new Vector2(150.148F, -2.755F), new Vector2(150.3F, -3.129F), new Vector2(150.602F, -3.34F));
                    builder.AddCubicBezier(new Vector2(150.904F, -3.551F), new Vector2(151.351F, -3.698F), new Vector2(151.945F, -3.781F));
                    builder.AddCubicBezier(new Vector2(151.945F, -3.781F), new Vector2(153.875F, -4.047F), new Vector2(153.875F, -4.047F));
                    builder.AddCubicBezier(new Vector2(153.875F, -4.047F), new Vector2(153.875F, -3.25F), new Vector2(153.875F, -3.25F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0083()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(155.156F, -5.203F));
                    builder.AddCubicBezier(new Vector2(155.156F, -7.193F), new Vector2(154.216F, -8.188F), new Vector2(152.336F, -8.188F));
                    builder.AddCubicBezier(new Vector2(151.31F, -8.188F), new Vector2(150.403F, -7.938F), new Vector2(149.617F, -7.438F));
                    builder.AddCubicBezier(new Vector2(149.617F, -7.438F), new Vector2(149.617F, -6.125F), new Vector2(149.617F, -6.125F));
                    builder.AddCubicBezier(new Vector2(150.393F, -6.781F), new Vector2(151.263F, -7.109F), new Vector2(152.227F, -7.109F));
                    builder.AddCubicBezier(new Vector2(153.326F, -7.109F), new Vector2(153.875F, -6.429F), new Vector2(153.875F, -5.07F));
                    builder.AddCubicBezier(new Vector2(153.875F, -5.07F), new Vector2(151.477F, -4.734F), new Vector2(151.477F, -4.734F));
                    builder.AddCubicBezier(new Vector2(149.717F, -4.489F), new Vector2(148.836F, -3.62F), new Vector2(148.836F, -2.125F));
                    builder.AddCubicBezier(new Vector2(148.836F, -1.427F), new Vector2(149.061F, -0.867F), new Vector2(149.512F, -0.445F));
                    builder.AddCubicBezier(new Vector2(149.962F, -0.023F), new Vector2(150.586F, 0.188F), new Vector2(151.383F, 0.188F));
                    builder.AddCubicBezier(new Vector2(152.466F, 0.188F), new Vector2(153.287F, -0.292F), new Vector2(153.844F, -1.25F));
                    builder.AddCubicBezier(new Vector2(153.844F, -1.25F), new Vector2(153.875F, -1.25F), new Vector2(153.875F, -1.25F));
                    builder.AddCubicBezier(new Vector2(153.875F, -1.25F), new Vector2(153.875F, 0), new Vector2(153.875F, 0));
                    builder.AddCubicBezier(new Vector2(153.875F, 0), new Vector2(155.156F, 0), new Vector2(155.156F, 0));
                    builder.AddCubicBezier(new Vector2(155.156F, 0), new Vector2(155.156F, -5.203F), new Vector2(155.156F, -5.203F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0084()
            {
                var result = CanvasGeometry_0085().
                    CombineWith(CanvasGeometry_0086(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0085()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(144.879F, -1.656F));
                    builder.AddCubicBezier(new Vector2(144.428F, -1.146F), new Vector2(143.844F, -0.891F), new Vector2(143.125F, -0.891F));
                    builder.AddCubicBezier(new Vector2(142.417F, -0.891F), new Vector2(141.848F, -1.157F), new Vector2(141.418F, -1.691F));
                    builder.AddCubicBezier(new Vector2(140.988F, -2.225F), new Vector2(140.773F, -2.937F), new Vector2(140.773F, -3.828F));
                    builder.AddCubicBezier(new Vector2(140.773F, -4.864F), new Vector2(140.997F, -5.67F), new Vector2(141.445F, -6.246F));
                    builder.AddCubicBezier(new Vector2(141.893F, -6.821F), new Vector2(142.513F, -7.109F), new Vector2(143.305F, -7.109F));
                    builder.AddCubicBezier(new Vector2(143.946F, -7.109F), new Vector2(144.48F, -6.883F), new Vector2(144.91F, -6.43F));
                    builder.AddCubicBezier(new Vector2(145.34F, -5.977F), new Vector2(145.555F, -5.432F), new Vector2(145.555F, -4.797F));
                    builder.AddCubicBezier(new Vector2(145.555F, -4.797F), new Vector2(145.555F, -3.617F), new Vector2(145.555F, -3.617F));
                    builder.AddCubicBezier(new Vector2(145.555F, -2.82F), new Vector2(145.329F, -2.166F), new Vector2(144.879F, -1.656F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0086()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(146.836F, -8));
                    builder.AddCubicBezier(new Vector2(146.836F, -8), new Vector2(145.555F, -8), new Vector2(145.555F, -8));
                    builder.AddCubicBezier(new Vector2(145.555F, -8), new Vector2(145.555F, -6.891F), new Vector2(145.555F, -6.891F));
                    builder.AddCubicBezier(new Vector2(145.555F, -6.891F), new Vector2(145.523F, -6.891F), new Vector2(145.523F, -6.891F));
                    builder.AddCubicBezier(new Vector2(145.002F, -7.756F), new Vector2(144.203F, -8.188F), new Vector2(143.125F, -8.188F));
                    builder.AddCubicBezier(new Vector2(141.99F, -8.188F), new Vector2(141.095F, -7.786F), new Vector2(140.441F, -6.984F));
                    builder.AddCubicBezier(new Vector2(139.787F, -6.182F), new Vector2(139.461F, -5.101F), new Vector2(139.461F, -3.742F));
                    builder.AddCubicBezier(new Vector2(139.461F, -2.544F), new Vector2(139.764F, -1.59F), new Vector2(140.371F, -0.879F));
                    builder.AddCubicBezier(new Vector2(140.978F, -0.168F), new Vector2(141.778F, 0.188F), new Vector2(142.773F, 0.188F));
                    builder.AddCubicBezier(new Vector2(143.997F, 0.188F), new Vector2(144.914F, -0.323F), new Vector2(145.523F, -1.344F));
                    builder.AddCubicBezier(new Vector2(145.523F, -1.344F), new Vector2(145.555F, -1.344F), new Vector2(145.555F, -1.344F));
                    builder.AddCubicBezier(new Vector2(145.555F, -1.344F), new Vector2(145.555F, -0.469F), new Vector2(145.555F, -0.469F));
                    builder.AddCubicBezier(new Vector2(145.555F, 1.625F), new Vector2(144.571F, 2.672F), new Vector2(142.602F, 2.672F));
                    builder.AddCubicBezier(new Vector2(141.784F, 2.672F), new Vector2(140.924F, 2.422F), new Vector2(140.023F, 1.922F));
                    builder.AddCubicBezier(new Vector2(140.023F, 1.922F), new Vector2(140.023F, 3.203F), new Vector2(140.023F, 3.203F));
                    builder.AddCubicBezier(new Vector2(140.763F, 3.578F), new Vector2(141.627F, 3.766F), new Vector2(142.617F, 3.766F));
                    builder.AddCubicBezier(new Vector2(145.43F, 3.766F), new Vector2(146.836F, 2.297F), new Vector2(146.836F, -0.641F));
                    builder.AddCubicBezier(new Vector2(146.836F, -0.641F), new Vector2(146.836F, -8), new Vector2(146.836F, -8));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0087()
            {
                var result = CanvasGeometry_0088().
                    CombineWith(CanvasGeometry_0089(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0088()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(136.313F, -3.25F));
                    builder.AddCubicBezier(new Vector2(136.313F, -2.568F), new Vector2(136.105F, -2.004F), new Vector2(135.691F, -1.559F));
                    builder.AddCubicBezier(new Vector2(135.277F, -1.114F), new Vector2(134.752F, -0.891F), new Vector2(134.117F, -0.891F));
                    builder.AddCubicBezier(new Vector2(133.653F, -0.891F), new Vector2(133.283F, -1.015F), new Vector2(133.004F, -1.262F));
                    builder.AddCubicBezier(new Vector2(132.725F, -1.509F), new Vector2(132.586F, -1.828F), new Vector2(132.586F, -2.219F));
                    builder.AddCubicBezier(new Vector2(132.586F, -2.755F), new Vector2(132.737F, -3.129F), new Vector2(133.039F, -3.34F));
                    builder.AddCubicBezier(new Vector2(133.341F, -3.551F), new Vector2(133.789F, -3.698F), new Vector2(134.383F, -3.781F));
                    builder.AddCubicBezier(new Vector2(134.383F, -3.781F), new Vector2(136.313F, -4.047F), new Vector2(136.313F, -4.047F));
                    builder.AddCubicBezier(new Vector2(136.313F, -4.047F), new Vector2(136.313F, -3.25F), new Vector2(136.313F, -3.25F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0089()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(137.594F, -5.203F));
                    builder.AddCubicBezier(new Vector2(137.594F, -7.193F), new Vector2(136.653F, -8.188F), new Vector2(134.773F, -8.188F));
                    builder.AddCubicBezier(new Vector2(133.747F, -8.188F), new Vector2(132.841F, -7.938F), new Vector2(132.055F, -7.438F));
                    builder.AddCubicBezier(new Vector2(132.055F, -7.438F), new Vector2(132.055F, -6.125F), new Vector2(132.055F, -6.125F));
                    builder.AddCubicBezier(new Vector2(132.831F, -6.781F), new Vector2(133.7F, -7.109F), new Vector2(134.664F, -7.109F));
                    builder.AddCubicBezier(new Vector2(135.763F, -7.109F), new Vector2(136.313F, -6.429F), new Vector2(136.313F, -5.07F));
                    builder.AddCubicBezier(new Vector2(136.313F, -5.07F), new Vector2(133.914F, -4.734F), new Vector2(133.914F, -4.734F));
                    builder.AddCubicBezier(new Vector2(132.154F, -4.489F), new Vector2(131.273F, -3.62F), new Vector2(131.273F, -2.125F));
                    builder.AddCubicBezier(new Vector2(131.273F, -1.427F), new Vector2(131.498F, -0.867F), new Vector2(131.949F, -0.445F));
                    builder.AddCubicBezier(new Vector2(132.399F, -0.023F), new Vector2(133.023F, 0.188F), new Vector2(133.82F, 0.188F));
                    builder.AddCubicBezier(new Vector2(134.903F, 0.188F), new Vector2(135.724F, -0.292F), new Vector2(136.281F, -1.25F));
                    builder.AddCubicBezier(new Vector2(136.281F, -1.25F), new Vector2(136.313F, -1.25F), new Vector2(136.313F, -1.25F));
                    builder.AddCubicBezier(new Vector2(136.313F, -1.25F), new Vector2(136.313F, 0), new Vector2(136.313F, 0));
                    builder.AddCubicBezier(new Vector2(136.313F, 0), new Vector2(137.594F, 0), new Vector2(137.594F, 0));
                    builder.AddCubicBezier(new Vector2(137.594F, 0), new Vector2(137.594F, -5.203F), new Vector2(137.594F, -5.203F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0090()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(124.781F, -8));
                    builder.AddCubicBezier(new Vector2(124.781F, -8), new Vector2(122.508F, -1.828F), new Vector2(122.508F, -1.828F));
                    builder.AddCubicBezier(new Vector2(122.43F, -1.557F), new Vector2(122.378F, -1.359F), new Vector2(122.352F, -1.234F));
                    builder.AddCubicBezier(new Vector2(122.352F, -1.234F), new Vector2(122.305F, -1.234F), new Vector2(122.305F, -1.234F));
                    builder.AddCubicBezier(new Vector2(122.222F, -1.562F), new Vector2(122.167F, -1.766F), new Vector2(122.141F, -1.844F));
                    builder.AddCubicBezier(new Vector2(122.141F, -1.844F), new Vector2(119.977F, -8), new Vector2(119.977F, -8));
                    builder.AddCubicBezier(new Vector2(119.977F, -8), new Vector2(118.555F, -8), new Vector2(118.555F, -8));
                    builder.AddCubicBezier(new Vector2(118.555F, -8), new Vector2(121.68F, -0.016F), new Vector2(121.68F, -0.016F));
                    builder.AddCubicBezier(new Vector2(121.68F, -0.016F), new Vector2(121.039F, 1.5F), new Vector2(121.039F, 1.5F));
                    builder.AddCubicBezier(new Vector2(120.716F, 2.271F), new Vector2(120.232F, 2.656F), new Vector2(119.586F, 2.656F));
                    builder.AddCubicBezier(new Vector2(119.357F, 2.656F), new Vector2(119.104F, 2.61F), new Vector2(118.828F, 2.516F));
                    builder.AddCubicBezier(new Vector2(118.828F, 2.516F), new Vector2(118.828F, 3.664F), new Vector2(118.828F, 3.664F));
                    builder.AddCubicBezier(new Vector2(119.052F, 3.732F), new Vector2(119.331F, 3.766F), new Vector2(119.664F, 3.766F));
                    builder.AddCubicBezier(new Vector2(120.852F, 3.766F), new Vector2(121.774F, 2.937F), new Vector2(122.43F, 1.281F));
                    builder.AddCubicBezier(new Vector2(122.43F, 1.281F), new Vector2(126.109F, -8), new Vector2(126.109F, -8));
                    builder.AddCubicBezier(new Vector2(126.109F, -8), new Vector2(124.781F, -8), new Vector2(124.781F, -8));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0091()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(118.352F, -8.031F));
                    builder.AddCubicBezier(new Vector2(118.175F, -8.104F), new Vector2(117.919F, -8.141F), new Vector2(117.586F, -8.141F));
                    builder.AddCubicBezier(new Vector2(117.122F, -8.141F), new Vector2(116.703F, -7.983F), new Vector2(116.328F, -7.668F));
                    builder.AddCubicBezier(new Vector2(115.953F, -7.353F), new Vector2(115.674F, -6.915F), new Vector2(115.492F, -6.352F));
                    builder.AddCubicBezier(new Vector2(115.492F, -6.352F), new Vector2(115.461F, -6.352F), new Vector2(115.461F, -6.352F));
                    builder.AddCubicBezier(new Vector2(115.461F, -6.352F), new Vector2(115.461F, -8), new Vector2(115.461F, -8));
                    builder.AddCubicBezier(new Vector2(115.461F, -8), new Vector2(114.18F, -8), new Vector2(114.18F, -8));
                    builder.AddCubicBezier(new Vector2(114.18F, -8), new Vector2(114.18F, 0), new Vector2(114.18F, 0));
                    builder.AddCubicBezier(new Vector2(114.18F, 0), new Vector2(115.461F, 0), new Vector2(115.461F, 0));
                    builder.AddCubicBezier(new Vector2(115.461F, 0), new Vector2(115.461F, -4.078F), new Vector2(115.461F, -4.078F));
                    builder.AddCubicBezier(new Vector2(115.461F, -4.969F), new Vector2(115.645F, -5.672F), new Vector2(116.012F, -6.188F));
                    builder.AddCubicBezier(new Vector2(116.379F, -6.704F), new Vector2(116.836F, -6.961F), new Vector2(117.383F, -6.961F));
                    builder.AddCubicBezier(new Vector2(117.805F, -6.961F), new Vector2(118.128F, -6.875F), new Vector2(118.352F, -6.703F));
                    builder.AddCubicBezier(new Vector2(118.352F, -6.703F), new Vector2(118.352F, -8.031F), new Vector2(118.352F, -8.031F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0092()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(113.984F, -11.203F));
                    builder.AddCubicBezier(new Vector2(113.984F, -11.203F), new Vector2(106.211F, -11.203F), new Vector2(106.211F, -11.203F));
                    builder.AddCubicBezier(new Vector2(106.211F, -11.203F), new Vector2(106.211F, -10.016F), new Vector2(106.211F, -10.016F));
                    builder.AddCubicBezier(new Vector2(106.211F, -10.016F), new Vector2(109.438F, -10.016F), new Vector2(109.438F, -10.016F));
                    builder.AddCubicBezier(new Vector2(109.438F, -10.016F), new Vector2(109.438F, 0), new Vector2(109.438F, 0));
                    builder.AddCubicBezier(new Vector2(109.438F, 0), new Vector2(110.75F, 0), new Vector2(110.75F, 0));
                    builder.AddCubicBezier(new Vector2(110.75F, 0), new Vector2(110.75F, -10.016F), new Vector2(110.75F, -10.016F));
                    builder.AddCubicBezier(new Vector2(110.75F, -10.016F), new Vector2(113.984F, -10.016F), new Vector2(113.984F, -10.016F));
                    builder.AddCubicBezier(new Vector2(113.984F, -10.016F), new Vector2(113.984F, -11.203F), new Vector2(113.984F, -11.203F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0093()
            {
                var result = CanvasGeometry_0094().
                    CombineWith(CanvasGeometry_0095(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0094()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(99.844F, -0.109F));
                    builder.AddCubicBezier(new Vector2(100.016F, -0.276F), new Vector2(100.102F, -0.479F), new Vector2(100.102F, -0.719F));
                    builder.AddCubicBezier(new Vector2(100.102F, -0.959F), new Vector2(100.016F, -1.163F), new Vector2(99.844F, -1.332F));
                    builder.AddCubicBezier(new Vector2(99.672F, -1.501F), new Vector2(99.468F, -1.586F), new Vector2(99.234F, -1.586F));
                    builder.AddCubicBezier(new Vector2(98.994F, -1.586F), new Vector2(98.79F, -1.501F), new Vector2(98.621F, -1.332F));
                    builder.AddCubicBezier(new Vector2(98.452F, -1.163F), new Vector2(98.367F, -0.959F), new Vector2(98.367F, -0.719F));
                    builder.AddCubicBezier(new Vector2(98.367F, -0.479F), new Vector2(98.452F, -0.276F), new Vector2(98.621F, -0.109F));
                    builder.AddCubicBezier(new Vector2(98.79F, 0.058F), new Vector2(98.994F, 0.141F), new Vector2(99.234F, 0.141F));
                    builder.AddCubicBezier(new Vector2(99.468F, 0.141F), new Vector2(99.672F, 0.058F), new Vector2(99.844F, -0.109F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0095()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(98.547F, -11.203F));
                    builder.AddCubicBezier(new Vector2(98.547F, -11.203F), new Vector2(98.688F, -3.156F), new Vector2(98.688F, -3.156F));
                    builder.AddCubicBezier(new Vector2(98.688F, -3.156F), new Vector2(99.742F, -3.156F), new Vector2(99.742F, -3.156F));
                    builder.AddCubicBezier(new Vector2(99.742F, -3.156F), new Vector2(99.891F, -11.203F), new Vector2(99.891F, -11.203F));
                    builder.AddCubicBezier(new Vector2(99.891F, -11.203F), new Vector2(98.547F, -11.203F), new Vector2(98.547F, -11.203F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0096()
            {
                var result = CanvasGeometry_0097().
                    CombineWith(CanvasGeometry_0098(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0097()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(93.715F, -1.664F));
                    builder.AddCubicBezier(new Vector2(93.27F, -1.148F), new Vector2(92.691F, -0.891F), new Vector2(91.977F, -0.891F));
                    builder.AddCubicBezier(new Vector2(91.253F, -0.891F), new Vector2(90.676F, -1.157F), new Vector2(90.246F, -1.691F));
                    builder.AddCubicBezier(new Vector2(89.816F, -2.225F), new Vector2(89.602F, -2.953F), new Vector2(89.602F, -3.875F));
                    builder.AddCubicBezier(new Vector2(89.602F, -4.885F), new Vector2(89.825F, -5.677F), new Vector2(90.273F, -6.25F));
                    builder.AddCubicBezier(new Vector2(90.721F, -6.823F), new Vector2(91.336F, -7.109F), new Vector2(92.117F, -7.109F));
                    builder.AddCubicBezier(new Vector2(92.773F, -7.109F), new Vector2(93.315F, -6.886F), new Vector2(93.742F, -6.438F));
                    builder.AddCubicBezier(new Vector2(94.169F, -5.99F), new Vector2(94.383F, -5.443F), new Vector2(94.383F, -4.797F));
                    builder.AddCubicBezier(new Vector2(94.383F, -4.797F), new Vector2(94.383F, -3.617F), new Vector2(94.383F, -3.617F));
                    builder.AddCubicBezier(new Vector2(94.383F, -2.831F), new Vector2(94.16F, -2.18F), new Vector2(93.715F, -1.664F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0098()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(95.664F, -11.844F));
                    builder.AddCubicBezier(new Vector2(95.664F, -11.844F), new Vector2(94.383F, -11.844F), new Vector2(94.383F, -11.844F));
                    builder.AddCubicBezier(new Vector2(94.383F, -11.844F), new Vector2(94.383F, -6.891F), new Vector2(94.383F, -6.891F));
                    builder.AddCubicBezier(new Vector2(94.383F, -6.891F), new Vector2(94.352F, -6.891F), new Vector2(94.352F, -6.891F));
                    builder.AddCubicBezier(new Vector2(93.852F, -7.756F), new Vector2(93.052F, -8.188F), new Vector2(91.953F, -8.188F));
                    builder.AddCubicBezier(new Vector2(90.844F, -8.188F), new Vector2(89.956F, -7.789F), new Vector2(89.289F, -6.992F));
                    builder.AddCubicBezier(new Vector2(88.622F, -6.195F), new Vector2(88.289F, -5.136F), new Vector2(88.289F, -3.813F));
                    builder.AddCubicBezier(new Vector2(88.289F, -2.579F), new Vector2(88.589F, -1.603F), new Vector2(89.191F, -0.887F));
                    builder.AddCubicBezier(new Vector2(89.793F, -0.171F), new Vector2(90.597F, 0.188F), new Vector2(91.602F, 0.188F));
                    builder.AddCubicBezier(new Vector2(92.842F, 0.188F), new Vector2(93.758F, -0.328F), new Vector2(94.352F, -1.359F));
                    builder.AddCubicBezier(new Vector2(94.352F, -1.359F), new Vector2(94.383F, -1.359F), new Vector2(94.383F, -1.359F));
                    builder.AddCubicBezier(new Vector2(94.383F, -1.359F), new Vector2(94.383F, 0), new Vector2(94.383F, 0));
                    builder.AddCubicBezier(new Vector2(94.383F, 0), new Vector2(95.664F, 0), new Vector2(95.664F, 0));
                    builder.AddCubicBezier(new Vector2(95.664F, 0), new Vector2(95.664F, -11.844F), new Vector2(95.664F, -11.844F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0099()
            {
                var result = CanvasGeometry_0100().
                    CombineWith(CanvasGeometry_0101(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0100()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(81.266F, -4.766F));
                    builder.AddCubicBezier(new Vector2(81.365F, -5.464F), new Vector2(81.625F, -6.029F), new Vector2(82.047F, -6.461F));
                    builder.AddCubicBezier(new Vector2(82.469F, -6.893F), new Vector2(82.982F, -7.109F), new Vector2(83.586F, -7.109F));
                    builder.AddCubicBezier(new Vector2(84.211F, -7.109F), new Vector2(84.699F, -6.903F), new Vector2(85.051F, -6.492F));
                    builder.AddCubicBezier(new Vector2(85.403F, -6.081F), new Vector2(85.581F, -5.506F), new Vector2(85.586F, -4.766F));
                    builder.AddCubicBezier(new Vector2(85.586F, -4.766F), new Vector2(81.266F, -4.766F), new Vector2(81.266F, -4.766F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0101()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(86.898F, -4.352F));
                    builder.AddCubicBezier(new Vector2(86.898F, -5.555F), new Vector2(86.612F, -6.495F), new Vector2(86.039F, -7.172F));
                    builder.AddCubicBezier(new Vector2(85.466F, -7.849F), new Vector2(84.656F, -8.188F), new Vector2(83.609F, -8.188F));
                    builder.AddCubicBezier(new Vector2(82.562F, -8.188F), new Vector2(81.686F, -7.796F), new Vector2(80.98F, -7.012F));
                    builder.AddCubicBezier(new Vector2(80.274F, -6.228F), new Vector2(79.922F, -5.214F), new Vector2(79.922F, -3.969F));
                    builder.AddCubicBezier(new Vector2(79.922F, -2.651F), new Vector2(80.245F, -1.629F), new Vector2(80.891F, -0.902F));
                    builder.AddCubicBezier(new Vector2(81.537F, -0.175F), new Vector2(82.425F, 0.188F), new Vector2(83.555F, 0.188F));
                    builder.AddCubicBezier(new Vector2(84.711F, 0.188F), new Vector2(85.641F, -0.068F), new Vector2(86.344F, -0.578F));
                    builder.AddCubicBezier(new Vector2(86.344F, -0.578F), new Vector2(86.344F, -1.781F), new Vector2(86.344F, -1.781F));
                    builder.AddCubicBezier(new Vector2(85.589F, -1.187F), new Vector2(84.76F, -0.891F), new Vector2(83.859F, -0.891F));
                    builder.AddCubicBezier(new Vector2(83.057F, -0.891F), new Vector2(82.427F, -1.133F), new Vector2(81.969F, -1.617F));
                    builder.AddCubicBezier(new Vector2(81.511F, -2.101F), new Vector2(81.271F, -2.789F), new Vector2(81.25F, -3.68F));
                    builder.AddCubicBezier(new Vector2(81.25F, -3.68F), new Vector2(86.898F, -3.68F), new Vector2(86.898F, -3.68F));
                    builder.AddCubicBezier(new Vector2(86.898F, -3.68F), new Vector2(86.898F, -4.352F), new Vector2(86.898F, -4.352F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0102()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(77.875F, -11.844F));
                    builder.AddCubicBezier(new Vector2(77.875F, -11.844F), new Vector2(76.594F, -11.844F), new Vector2(76.594F, -11.844F));
                    builder.AddCubicBezier(new Vector2(76.594F, -11.844F), new Vector2(76.594F, 0), new Vector2(76.594F, 0));
                    builder.AddCubicBezier(new Vector2(76.594F, 0), new Vector2(77.875F, 0), new Vector2(77.875F, 0));
                    builder.AddCubicBezier(new Vector2(77.875F, 0), new Vector2(77.875F, -11.844F), new Vector2(77.875F, -11.844F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0103()
            {
                var result = CanvasGeometry_0104().
                    CombineWith(CanvasGeometry_0105(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0104()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(74, -8));
                    builder.AddCubicBezier(new Vector2(74, -8), new Vector2(72.719F, -8), new Vector2(72.719F, -8));
                    builder.AddCubicBezier(new Vector2(72.719F, -8), new Vector2(72.719F, 0), new Vector2(72.719F, 0));
                    builder.AddCubicBezier(new Vector2(72.719F, 0), new Vector2(74, 0), new Vector2(74, 0));
                    builder.AddCubicBezier(new Vector2(74, 0), new Vector2(74, -8), new Vector2(74, -8));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0105()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(73.973F, -10.273F));
                    builder.AddCubicBezier(new Vector2(74.137F, -10.434F), new Vector2(74.219F, -10.63F), new Vector2(74.219F, -10.859F));
                    builder.AddCubicBezier(new Vector2(74.219F, -11.099F), new Vector2(74.137F, -11.298F), new Vector2(73.973F, -11.457F));
                    builder.AddCubicBezier(new Vector2(73.809F, -11.616F), new Vector2(73.609F, -11.695F), new Vector2(73.375F, -11.695F));
                    builder.AddCubicBezier(new Vector2(73.146F, -11.695F), new Vector2(72.95F, -11.616F), new Vector2(72.789F, -11.457F));
                    builder.AddCubicBezier(new Vector2(72.628F, -11.298F), new Vector2(72.547F, -11.099F), new Vector2(72.547F, -10.859F));
                    builder.AddCubicBezier(new Vector2(72.547F, -10.619F), new Vector2(72.628F, -10.422F), new Vector2(72.789F, -10.266F));
                    builder.AddCubicBezier(new Vector2(72.95F, -10.11F), new Vector2(73.146F, -10.031F), new Vector2(73.375F, -10.031F));
                    builder.AddCubicBezier(new Vector2(73.609F, -10.031F), new Vector2(73.809F, -10.112F), new Vector2(73.973F, -10.273F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0106()
            {
                var result = CanvasGeometry_0107().
                    CombineWith(CanvasGeometry_0108(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0107()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(69.023F, -3.25F));
                    builder.AddCubicBezier(new Vector2(69.023F, -2.568F), new Vector2(68.816F, -2.004F), new Vector2(68.402F, -1.559F));
                    builder.AddCubicBezier(new Vector2(67.988F, -1.114F), new Vector2(67.463F, -0.891F), new Vector2(66.828F, -0.891F));
                    builder.AddCubicBezier(new Vector2(66.364F, -0.891F), new Vector2(65.994F, -1.015F), new Vector2(65.715F, -1.262F));
                    builder.AddCubicBezier(new Vector2(65.436F, -1.509F), new Vector2(65.297F, -1.828F), new Vector2(65.297F, -2.219F));
                    builder.AddCubicBezier(new Vector2(65.297F, -2.755F), new Vector2(65.448F, -3.129F), new Vector2(65.75F, -3.34F));
                    builder.AddCubicBezier(new Vector2(66.052F, -3.551F), new Vector2(66.5F, -3.698F), new Vector2(67.094F, -3.781F));
                    builder.AddCubicBezier(new Vector2(67.094F, -3.781F), new Vector2(69.023F, -4.047F), new Vector2(69.023F, -4.047F));
                    builder.AddCubicBezier(new Vector2(69.023F, -4.047F), new Vector2(69.023F, -3.25F), new Vector2(69.023F, -3.25F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0108()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(70.305F, -5.203F));
                    builder.AddCubicBezier(new Vector2(70.305F, -7.193F), new Vector2(69.364F, -8.188F), new Vector2(67.484F, -8.188F));
                    builder.AddCubicBezier(new Vector2(66.458F, -8.188F), new Vector2(65.552F, -7.938F), new Vector2(64.766F, -7.438F));
                    builder.AddCubicBezier(new Vector2(64.766F, -7.438F), new Vector2(64.766F, -6.125F), new Vector2(64.766F, -6.125F));
                    builder.AddCubicBezier(new Vector2(65.542F, -6.781F), new Vector2(66.411F, -7.109F), new Vector2(67.375F, -7.109F));
                    builder.AddCubicBezier(new Vector2(68.474F, -7.109F), new Vector2(69.023F, -6.429F), new Vector2(69.023F, -5.07F));
                    builder.AddCubicBezier(new Vector2(69.023F, -5.07F), new Vector2(66.625F, -4.734F), new Vector2(66.625F, -4.734F));
                    builder.AddCubicBezier(new Vector2(64.865F, -4.489F), new Vector2(63.984F, -3.62F), new Vector2(63.984F, -2.125F));
                    builder.AddCubicBezier(new Vector2(63.984F, -1.427F), new Vector2(64.209F, -0.867F), new Vector2(64.66F, -0.445F));
                    builder.AddCubicBezier(new Vector2(65.11F, -0.023F), new Vector2(65.734F, 0.188F), new Vector2(66.531F, 0.188F));
                    builder.AddCubicBezier(new Vector2(67.614F, 0.188F), new Vector2(68.435F, -0.292F), new Vector2(68.992F, -1.25F));
                    builder.AddCubicBezier(new Vector2(68.992F, -1.25F), new Vector2(69.023F, -1.25F), new Vector2(69.023F, -1.25F));
                    builder.AddCubicBezier(new Vector2(69.023F, -1.25F), new Vector2(69.023F, 0), new Vector2(69.023F, 0));
                    builder.AddCubicBezier(new Vector2(69.023F, 0), new Vector2(70.305F, 0), new Vector2(70.305F, 0));
                    builder.AddCubicBezier(new Vector2(70.305F, 0), new Vector2(70.305F, -5.203F), new Vector2(70.305F, -5.203F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0109()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(63.523F, -11.875F));
                    builder.AddCubicBezier(new Vector2(63.294F, -11.969F), new Vector2(62.985F, -12.016F), new Vector2(62.594F, -12.016F));
                    builder.AddCubicBezier(new Vector2(61.87F, -12.016F), new Vector2(61.265F, -11.772F), new Vector2(60.781F, -11.285F));
                    builder.AddCubicBezier(new Vector2(60.297F, -10.798F), new Vector2(60.055F, -10.136F), new Vector2(60.055F, -9.297F));
                    builder.AddCubicBezier(new Vector2(60.055F, -9.297F), new Vector2(60.055F, -8), new Vector2(60.055F, -8));
                    builder.AddCubicBezier(new Vector2(60.055F, -8), new Vector2(58.688F, -8), new Vector2(58.688F, -8));
                    builder.AddCubicBezier(new Vector2(58.688F, -8), new Vector2(58.688F, -6.906F), new Vector2(58.688F, -6.906F));
                    builder.AddCubicBezier(new Vector2(58.688F, -6.906F), new Vector2(60.055F, -6.906F), new Vector2(60.055F, -6.906F));
                    builder.AddCubicBezier(new Vector2(60.055F, -6.906F), new Vector2(60.055F, 0), new Vector2(60.055F, 0));
                    builder.AddCubicBezier(new Vector2(60.055F, 0), new Vector2(61.328F, 0), new Vector2(61.328F, 0));
                    builder.AddCubicBezier(new Vector2(61.328F, 0), new Vector2(61.328F, -6.906F), new Vector2(61.328F, -6.906F));
                    builder.AddCubicBezier(new Vector2(61.328F, -6.906F), new Vector2(63.203F, -6.906F), new Vector2(63.203F, -6.906F));
                    builder.AddCubicBezier(new Vector2(63.203F, -6.906F), new Vector2(63.203F, -8), new Vector2(63.203F, -8));
                    builder.AddCubicBezier(new Vector2(63.203F, -8), new Vector2(61.328F, -8), new Vector2(61.328F, -8));
                    builder.AddCubicBezier(new Vector2(61.328F, -8), new Vector2(61.328F, -9.234F), new Vector2(61.328F, -9.234F));
                    builder.AddCubicBezier(new Vector2(61.328F, -10.364F), new Vector2(61.776F, -10.93F), new Vector2(62.672F, -10.93F));
                    builder.AddCubicBezier(new Vector2(62.99F, -10.93F), new Vector2(63.273F, -10.86F), new Vector2(63.523F, -10.719F));
                    builder.AddCubicBezier(new Vector2(63.523F, -10.719F), new Vector2(63.523F, -11.875F), new Vector2(63.523F, -11.875F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0110()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(53.477F, -1.172F));
                    builder.AddCubicBezier(new Vector2(53.243F, -0.995F), new Vector2(52.964F, -0.906F), new Vector2(52.641F, -0.906F));
                    builder.AddCubicBezier(new Vector2(52.219F, -0.906F), new Vector2(51.916F, -1.021F), new Vector2(51.734F, -1.25F));
                    builder.AddCubicBezier(new Vector2(51.552F, -1.479F), new Vector2(51.461F, -1.862F), new Vector2(51.461F, -2.398F));
                    builder.AddCubicBezier(new Vector2(51.461F, -2.398F), new Vector2(51.461F, -6.906F), new Vector2(51.461F, -6.906F));
                    builder.AddCubicBezier(new Vector2(51.461F, -6.906F), new Vector2(53.477F, -6.906F), new Vector2(53.477F, -6.906F));
                    builder.AddCubicBezier(new Vector2(53.477F, -6.906F), new Vector2(53.477F, -8), new Vector2(53.477F, -8));
                    builder.AddCubicBezier(new Vector2(53.477F, -8), new Vector2(51.461F, -8), new Vector2(51.461F, -8));
                    builder.AddCubicBezier(new Vector2(51.461F, -8), new Vector2(51.461F, -10.367F), new Vector2(51.461F, -10.367F));
                    builder.AddCubicBezier(new Vector2(51.461F, -10.367F), new Vector2(50.18F, -9.953F), new Vector2(50.18F, -9.953F));
                    builder.AddCubicBezier(new Vector2(50.18F, -9.953F), new Vector2(50.18F, -8), new Vector2(50.18F, -8));
                    builder.AddCubicBezier(new Vector2(50.18F, -8), new Vector2(48.805F, -8), new Vector2(48.805F, -8));
                    builder.AddCubicBezier(new Vector2(48.805F, -8), new Vector2(48.805F, -6.906F), new Vector2(48.805F, -6.906F));
                    builder.AddCubicBezier(new Vector2(48.805F, -6.906F), new Vector2(50.18F, -6.906F), new Vector2(50.18F, -6.906F));
                    builder.AddCubicBezier(new Vector2(50.18F, -6.906F), new Vector2(50.18F, -2.172F), new Vector2(50.18F, -2.172F));
                    builder.AddCubicBezier(new Vector2(50.18F, -0.609F), new Vector2(50.88F, 0.172F), new Vector2(52.281F, 0.172F));
                    builder.AddCubicBezier(new Vector2(52.776F, 0.172F), new Vector2(53.175F, 0.089F), new Vector2(53.477F, -0.078F));
                    builder.AddCubicBezier(new Vector2(53.477F, -0.078F), new Vector2(53.477F, -1.172F), new Vector2(53.477F, -1.172F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0111()
            {
                var result = CanvasGeometry_0112().
                    CombineWith(CanvasGeometry_0113(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0112()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(47.172F, -8));
                    builder.AddCubicBezier(new Vector2(47.172F, -8), new Vector2(45.891F, -8), new Vector2(45.891F, -8));
                    builder.AddCubicBezier(new Vector2(45.891F, -8), new Vector2(45.891F, 0), new Vector2(45.891F, 0));
                    builder.AddCubicBezier(new Vector2(45.891F, 0), new Vector2(47.172F, 0), new Vector2(47.172F, 0));
                    builder.AddCubicBezier(new Vector2(47.172F, 0), new Vector2(47.172F, -8), new Vector2(47.172F, -8));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0113()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(47.145F, -10.273F));
                    builder.AddCubicBezier(new Vector2(47.309F, -10.434F), new Vector2(47.391F, -10.63F), new Vector2(47.391F, -10.859F));
                    builder.AddCubicBezier(new Vector2(47.391F, -11.099F), new Vector2(47.309F, -11.298F), new Vector2(47.145F, -11.457F));
                    builder.AddCubicBezier(new Vector2(46.981F, -11.616F), new Vector2(46.781F, -11.695F), new Vector2(46.547F, -11.695F));
                    builder.AddCubicBezier(new Vector2(46.318F, -11.695F), new Vector2(46.122F, -11.616F), new Vector2(45.961F, -11.457F));
                    builder.AddCubicBezier(new Vector2(45.8F, -11.298F), new Vector2(45.719F, -11.099F), new Vector2(45.719F, -10.859F));
                    builder.AddCubicBezier(new Vector2(45.719F, -10.619F), new Vector2(45.8F, -10.422F), new Vector2(45.961F, -10.266F));
                    builder.AddCubicBezier(new Vector2(46.122F, -10.11F), new Vector2(46.318F, -10.031F), new Vector2(46.547F, -10.031F));
                    builder.AddCubicBezier(new Vector2(46.781F, -10.031F), new Vector2(46.981F, -10.112F), new Vector2(47.145F, -10.273F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0114()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(37.961F, -1.781F));
                    builder.AddCubicBezier(new Vector2(37.961F, -1.781F), new Vector2(37.047F, 2.063F), new Vector2(37.047F, 2.063F));
                    builder.AddCubicBezier(new Vector2(37.047F, 2.063F), new Vector2(37.961F, 2.063F), new Vector2(37.961F, 2.063F));
                    builder.AddCubicBezier(new Vector2(37.961F, 2.063F), new Vector2(39.211F, -1.781F), new Vector2(39.211F, -1.781F));
                    builder.AddCubicBezier(new Vector2(39.211F, -1.781F), new Vector2(37.961F, -1.781F), new Vector2(37.961F, -1.781F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0115()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(35.336F, -8));
                    builder.AddCubicBezier(new Vector2(35.336F, -8), new Vector2(33.063F, -1.828F), new Vector2(33.063F, -1.828F));
                    builder.AddCubicBezier(new Vector2(32.985F, -1.557F), new Vector2(32.932F, -1.359F), new Vector2(32.906F, -1.234F));
                    builder.AddCubicBezier(new Vector2(32.906F, -1.234F), new Vector2(32.859F, -1.234F), new Vector2(32.859F, -1.234F));
                    builder.AddCubicBezier(new Vector2(32.776F, -1.562F), new Vector2(32.721F, -1.766F), new Vector2(32.695F, -1.844F));
                    builder.AddCubicBezier(new Vector2(32.695F, -1.844F), new Vector2(30.531F, -8), new Vector2(30.531F, -8));
                    builder.AddCubicBezier(new Vector2(30.531F, -8), new Vector2(29.109F, -8), new Vector2(29.109F, -8));
                    builder.AddCubicBezier(new Vector2(29.109F, -8), new Vector2(32.234F, -0.016F), new Vector2(32.234F, -0.016F));
                    builder.AddCubicBezier(new Vector2(32.234F, -0.016F), new Vector2(31.594F, 1.5F), new Vector2(31.594F, 1.5F));
                    builder.AddCubicBezier(new Vector2(31.271F, 2.271F), new Vector2(30.787F, 2.656F), new Vector2(30.141F, 2.656F));
                    builder.AddCubicBezier(new Vector2(29.912F, 2.656F), new Vector2(29.659F, 2.61F), new Vector2(29.383F, 2.516F));
                    builder.AddCubicBezier(new Vector2(29.383F, 2.516F), new Vector2(29.383F, 3.664F), new Vector2(29.383F, 3.664F));
                    builder.AddCubicBezier(new Vector2(29.607F, 3.732F), new Vector2(29.886F, 3.766F), new Vector2(30.219F, 3.766F));
                    builder.AddCubicBezier(new Vector2(31.407F, 3.766F), new Vector2(32.328F, 2.937F), new Vector2(32.984F, 1.281F));
                    builder.AddCubicBezier(new Vector2(32.984F, 1.281F), new Vector2(36.664F, -8), new Vector2(36.664F, -8));
                    builder.AddCubicBezier(new Vector2(36.664F, -8), new Vector2(35.336F, -8), new Vector2(35.336F, -8));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0116()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(28.906F, -8.031F));
                    builder.AddCubicBezier(new Vector2(28.729F, -8.104F), new Vector2(28.474F, -8.141F), new Vector2(28.141F, -8.141F));
                    builder.AddCubicBezier(new Vector2(27.677F, -8.141F), new Vector2(27.258F, -7.983F), new Vector2(26.883F, -7.668F));
                    builder.AddCubicBezier(new Vector2(26.508F, -7.353F), new Vector2(26.229F, -6.915F), new Vector2(26.047F, -6.352F));
                    builder.AddCubicBezier(new Vector2(26.047F, -6.352F), new Vector2(26.016F, -6.352F), new Vector2(26.016F, -6.352F));
                    builder.AddCubicBezier(new Vector2(26.016F, -6.352F), new Vector2(26.016F, -8), new Vector2(26.016F, -8));
                    builder.AddCubicBezier(new Vector2(26.016F, -8), new Vector2(24.734F, -8), new Vector2(24.734F, -8));
                    builder.AddCubicBezier(new Vector2(24.734F, -8), new Vector2(24.734F, 0), new Vector2(24.734F, 0));
                    builder.AddCubicBezier(new Vector2(24.734F, 0), new Vector2(26.016F, 0), new Vector2(26.016F, 0));
                    builder.AddCubicBezier(new Vector2(26.016F, 0), new Vector2(26.016F, -4.078F), new Vector2(26.016F, -4.078F));
                    builder.AddCubicBezier(new Vector2(26.016F, -4.969F), new Vector2(26.199F, -5.672F), new Vector2(26.566F, -6.188F));
                    builder.AddCubicBezier(new Vector2(26.933F, -6.704F), new Vector2(27.391F, -6.961F), new Vector2(27.938F, -6.961F));
                    builder.AddCubicBezier(new Vector2(28.36F, -6.961F), new Vector2(28.682F, -6.875F), new Vector2(28.906F, -6.703F));
                    builder.AddCubicBezier(new Vector2(28.906F, -6.703F), new Vector2(28.906F, -8.031F), new Vector2(28.906F, -8.031F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0117()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(23.344F, -8.031F));
                    builder.AddCubicBezier(new Vector2(23.167F, -8.104F), new Vector2(22.911F, -8.141F), new Vector2(22.578F, -8.141F));
                    builder.AddCubicBezier(new Vector2(22.114F, -8.141F), new Vector2(21.695F, -7.983F), new Vector2(21.32F, -7.668F));
                    builder.AddCubicBezier(new Vector2(20.945F, -7.353F), new Vector2(20.666F, -6.915F), new Vector2(20.484F, -6.352F));
                    builder.AddCubicBezier(new Vector2(20.484F, -6.352F), new Vector2(20.453F, -6.352F), new Vector2(20.453F, -6.352F));
                    builder.AddCubicBezier(new Vector2(20.453F, -6.352F), new Vector2(20.453F, -8), new Vector2(20.453F, -8));
                    builder.AddCubicBezier(new Vector2(20.453F, -8), new Vector2(19.172F, -8), new Vector2(19.172F, -8));
                    builder.AddCubicBezier(new Vector2(19.172F, -8), new Vector2(19.172F, 0), new Vector2(19.172F, 0));
                    builder.AddCubicBezier(new Vector2(19.172F, 0), new Vector2(20.453F, 0), new Vector2(20.453F, 0));
                    builder.AddCubicBezier(new Vector2(20.453F, 0), new Vector2(20.453F, -4.078F), new Vector2(20.453F, -4.078F));
                    builder.AddCubicBezier(new Vector2(20.453F, -4.969F), new Vector2(20.637F, -5.672F), new Vector2(21.004F, -6.188F));
                    builder.AddCubicBezier(new Vector2(21.371F, -6.704F), new Vector2(21.828F, -6.961F), new Vector2(22.375F, -6.961F));
                    builder.AddCubicBezier(new Vector2(22.797F, -6.961F), new Vector2(23.12F, -6.875F), new Vector2(23.344F, -6.703F));
                    builder.AddCubicBezier(new Vector2(23.344F, -6.703F), new Vector2(23.344F, -8.031F), new Vector2(23.344F, -8.031F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0118()
            {
                var result = CanvasGeometry_0119().
                    CombineWith(CanvasGeometry_0120(),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Xor);
                return result;
            }
            
            CanvasGeometry CanvasGeometry_0119()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(15.145F, -6.297F));
                    builder.AddCubicBezier(new Vector2(15.59F, -5.755F), new Vector2(15.813F, -4.984F), new Vector2(15.813F, -3.984F));
                    builder.AddCubicBezier(new Vector2(15.813F, -2.994F), new Vector2(15.59F, -2.231F), new Vector2(15.145F, -1.695F));
                    builder.AddCubicBezier(new Vector2(14.7F, -1.159F), new Vector2(14.062F, -0.891F), new Vector2(13.234F, -0.891F));
                    builder.AddCubicBezier(new Vector2(12.421F, -0.891F), new Vector2(11.773F, -1.164F), new Vector2(11.289F, -1.711F));
                    builder.AddCubicBezier(new Vector2(10.805F, -2.258F), new Vector2(10.563F, -3.005F), new Vector2(10.563F, -3.953F));
                    builder.AddCubicBezier(new Vector2(10.563F, -4.937F), new Vector2(10.802F, -5.71F), new Vector2(11.281F, -6.27F));
                    builder.AddCubicBezier(new Vector2(11.76F, -6.83F), new Vector2(12.411F, -7.109F), new Vector2(13.234F, -7.109F));
                    builder.AddCubicBezier(new Vector2(14.062F, -7.109F), new Vector2(14.7F, -6.839F), new Vector2(15.145F, -6.297F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0120()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(16.043F, -0.965F));
                    builder.AddCubicBezier(new Vector2(16.764F, -1.733F), new Vector2(17.125F, -2.755F), new Vector2(17.125F, -4.031F));
                    builder.AddCubicBezier(new Vector2(17.125F, -5.333F), new Vector2(16.79F, -6.352F), new Vector2(16.121F, -7.086F));
                    builder.AddCubicBezier(new Vector2(15.452F, -7.82F), new Vector2(14.521F, -8.188F), new Vector2(13.328F, -8.188F));
                    builder.AddCubicBezier(new Vector2(12.078F, -8.188F), new Vector2(11.086F, -7.81F), new Vector2(10.352F, -7.055F));
                    builder.AddCubicBezier(new Vector2(9.618F, -6.3F), new Vector2(9.25F, -5.25F), new Vector2(9.25F, -3.906F));
                    builder.AddCubicBezier(new Vector2(9.25F, -2.672F), new Vector2(9.603F, -1.681F), new Vector2(10.309F, -0.934F));
                    builder.AddCubicBezier(new Vector2(11.015F, -0.187F), new Vector2(11.959F, 0.188F), new Vector2(13.141F, 0.188F));
                    builder.AddCubicBezier(new Vector2(14.355F, 0.188F), new Vector2(15.322F, -0.197F), new Vector2(16.043F, -0.965F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0121()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(1.527F, -0.184F));
                    builder.AddCubicBezier(new Vector2(1.769F, -0.103F), new Vector2(2.026F, -0.035F), new Vector2(2.297F, 0.02F));
                    builder.AddCubicBezier(new Vector2(2.568F, 0.075F), new Vector2(2.834F, 0.116F), new Vector2(3.094F, 0.145F));
                    builder.AddCubicBezier(new Vector2(3.354F, 0.174F), new Vector2(3.573F, 0.188F), new Vector2(3.75F, 0.188F));
                    builder.AddCubicBezier(new Vector2(4.281F, 0.188F), new Vector2(4.787F, 0.134F), new Vector2(5.266F, 0.027F));
                    builder.AddCubicBezier(new Vector2(5.745F, -0.08F), new Vector2(6.168F, -0.252F), new Vector2(6.535F, -0.492F));
                    builder.AddCubicBezier(new Vector2(6.902F, -0.732F), new Vector2(7.194F, -1.043F), new Vector2(7.41F, -1.426F));
                    builder.AddCubicBezier(new Vector2(7.626F, -1.809F), new Vector2(7.734F, -2.276F), new Vector2(7.734F, -2.828F));
                    builder.AddCubicBezier(new Vector2(7.734F, -3.245F), new Vector2(7.655F, -3.615F), new Vector2(7.496F, -3.938F));
                    builder.AddCubicBezier(new Vector2(7.337F, -4.261F), new Vector2(7.123F, -4.554F), new Vector2(6.852F, -4.82F));
                    builder.AddCubicBezier(new Vector2(6.581F, -5.086F), new Vector2(6.265F, -5.328F), new Vector2(5.906F, -5.547F));
                    builder.AddCubicBezier(new Vector2(5.547F, -5.766F), new Vector2(5.167F, -5.974F), new Vector2(4.766F, -6.172F));
                    builder.AddCubicBezier(new Vector2(4.375F, -6.365F), new Vector2(4.029F, -6.542F), new Vector2(3.73F, -6.703F));
                    builder.AddCubicBezier(new Vector2(3.43F, -6.864F), new Vector2(3.177F, -7.031F), new Vector2(2.969F, -7.203F));
                    builder.AddCubicBezier(new Vector2(2.761F, -7.375F), new Vector2(2.603F, -7.565F), new Vector2(2.496F, -7.773F));
                    builder.AddCubicBezier(new Vector2(2.389F, -7.981F), new Vector2(2.336F, -8.23F), new Vector2(2.336F, -8.516F));
                    builder.AddCubicBezier(new Vector2(2.336F, -8.823F), new Vector2(2.406F, -9.083F), new Vector2(2.547F, -9.297F));
                    builder.AddCubicBezier(new Vector2(2.688F, -9.511F), new Vector2(2.87F, -9.685F), new Vector2(3.094F, -9.82F));
                    builder.AddCubicBezier(new Vector2(3.318F, -9.955F), new Vector2(3.573F, -10.053F), new Vector2(3.859F, -10.113F));
                    builder.AddCubicBezier(new Vector2(4.145F, -10.173F), new Vector2(4.433F, -10.203F), new Vector2(4.719F, -10.203F));
                    builder.AddCubicBezier(new Vector2(5.755F, -10.203F), new Vector2(6.605F, -9.974F), new Vector2(7.266F, -9.516F));
                    builder.AddCubicBezier(new Vector2(7.266F, -9.516F), new Vector2(7.266F, -10.992F), new Vector2(7.266F, -10.992F));
                    builder.AddCubicBezier(new Vector2(6.761F, -11.258F), new Vector2(5.956F, -11.391F), new Vector2(4.852F, -11.391F));
                    builder.AddCubicBezier(new Vector2(4.368F, -11.391F), new Vector2(3.892F, -11.331F), new Vector2(3.426F, -11.211F));
                    builder.AddCubicBezier(new Vector2(2.96F, -11.091F), new Vector2(2.545F, -10.909F), new Vector2(2.18F, -10.664F));
                    builder.AddCubicBezier(new Vector2(1.815F, -10.419F), new Vector2(1.521F, -10.108F), new Vector2(1.297F, -9.73F));
                    builder.AddCubicBezier(new Vector2(1.073F, -9.352F), new Vector2(0.961F, -8.908F), new Vector2(0.961F, -8.398F));
                    builder.AddCubicBezier(new Vector2(0.961F, -7.981F), new Vector2(1.033F, -7.619F), new Vector2(1.176F, -7.309F));
                    builder.AddCubicBezier(new Vector2(1.319F, -6.999F), new Vector2(1.516F, -6.722F), new Vector2(1.766F, -6.477F));
                    builder.AddCubicBezier(new Vector2(2.016F, -6.232F), new Vector2(2.309F, -6.008F), new Vector2(2.648F, -5.805F));
                    builder.AddCubicBezier(new Vector2(2.987F, -5.602F), new Vector2(3.351F, -5.401F), new Vector2(3.742F, -5.203F));
                    builder.AddCubicBezier(new Vector2(4.112F, -5.015F), new Vector2(4.457F, -4.837F), new Vector2(4.777F, -4.668F));
                    builder.AddCubicBezier(new Vector2(5.097F, -4.499F), new Vector2(5.375F, -4.322F), new Vector2(5.609F, -4.137F));
                    builder.AddCubicBezier(new Vector2(5.843F, -3.952F), new Vector2(6.027F, -3.747F), new Vector2(6.16F, -3.523F));
                    builder.AddCubicBezier(new Vector2(6.293F, -3.299F), new Vector2(6.359F, -3.036F), new Vector2(6.359F, -2.734F));
                    builder.AddCubicBezier(new Vector2(6.359F, -2.171F), new Vector2(6.16F, -1.74F), new Vector2(5.762F, -1.441F));
                    builder.AddCubicBezier(new Vector2(5.364F, -1.141F), new Vector2(4.76F, -0.992F), new Vector2(3.953F, -0.992F));
                    builder.AddCubicBezier(new Vector2(3.719F, -0.992F), new Vector2(3.464F, -1.016F), new Vector2(3.188F, -1.063F));
                    builder.AddCubicBezier(new Vector2(2.912F, -1.11F), new Vector2(2.636F, -1.176F), new Vector2(2.363F, -1.262F));
                    builder.AddCubicBezier(new Vector2(2.09F, -1.348F), new Vector2(1.829F, -1.453F), new Vector2(1.582F, -1.578F));
                    builder.AddCubicBezier(new Vector2(1.335F, -1.703F), new Vector2(1.122F, -1.844F), new Vector2(0.945F, -2));
                    builder.AddCubicBezier(new Vector2(0.945F, -2), new Vector2(0.945F, -0.453F), new Vector2(0.945F, -0.453F));
                    builder.AddCubicBezier(new Vector2(1.091F, -0.354F), new Vector2(1.285F, -0.265F), new Vector2(1.527F, -0.184F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0122()
            {
                if (_canvasGeometry_0122 != null)
                {
                    return _canvasGeometry_0122;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(20.5F, -20.5F));
                    builder.AddCubicBezier(new Vector2(20.5F, -20.5F), new Vector2(-20.5F, -20.5F), new Vector2(-20.5F, -20.5F));
                    builder.AddCubicBezier(new Vector2(-20.5F, -20.5F), new Vector2(-20.5F, 20.5F), new Vector2(-20.5F, 20.5F));
                    builder.AddCubicBezier(new Vector2(-20.5F, 20.5F), new Vector2(20.5F, 20.5F), new Vector2(20.5F, 20.5F));
                    builder.AddCubicBezier(new Vector2(20.5F, 20.5F), new Vector2(20.5F, -20.5F), new Vector2(20.5F, -20.5F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0122 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0123()
            {
                if (_canvasGeometry_0123 != null)
                {
                    return _canvasGeometry_0123;
                }
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(5.533F, -39.656F));
                    builder.AddCubicBezier(new Vector2(5.533F, -39.656F), new Vector2(-35.467F, 1.344F), new Vector2(-35.467F, 1.344F));
                    builder.AddCubicBezier(new Vector2(-35.467F, 1.344F), new Vector2(20.5F, 20.5F), new Vector2(20.5F, 20.5F));
                    builder.AddCubicBezier(new Vector2(20.5F, 20.5F), new Vector2(5.533F, -39.656F), new Vector2(5.533F, -39.656F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    return  _canvasGeometry_0123 = CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0124()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-36, -95));
                    builder.AddCubicBezier(new Vector2(-6.109F, -111.25F), new Vector2(53.672F, -143.75F), new Vector2(83.5F, -160));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0125()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-47, -104.25F));
                    builder.AddCubicBezier(new Vector2(-45.764F, -132.127F), new Vector2(-43.293F, -187.88F), new Vector2(-42, -215.75F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0126()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-62.75F, -98));
                    builder.AddCubicBezier(new Vector2(-65.578F, -100.163F), new Vector2(-71.233F, -104.49F), new Vector2(-75.639F, -107.47F));
                    builder.AddCubicBezier(new Vector2(-108.57F, -129.742F), new Vector2(-152.69F, -159.081F), new Vector2(-174.75F, -173.75F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0127()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-57.75F, -77.25F));
                    builder.AddCubicBezier(new Vector2(-75.295F, -64.375F), new Vector2(-110.386F, -38.624F), new Vector2(-128, -25.75F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            CanvasGeometry CanvasGeometry_0128()
            {
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Alternate);
                    builder.BeginFigure(new Vector2(-41.75F, -76.75F));
                    builder.AddCubicBezier(new Vector2(-32.172F, -63.811F), new Vector2(-13.016F, -37.932F), new Vector2(-3.5F, -25));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    return CanvasGeometry.CreatePath(builder);
                }
            }
            
            ColorKeyFrameAnimation ColorKeyFrameAnimation_0000()
            {
                if (_colorKeyFrameAnimation_0000 != null)
                {
                    return _colorKeyFrameAnimation_0000;
                }
                var result = _colorKeyFrameAnimation_0000 = _c.CreateColorKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), LinearEasingFunction_0000());
                result.InsertKeyFrame(1, Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), LinearEasingFunction_0000());
                return result;
            }
            
            ColorKeyFrameAnimation ColorKeyFrameAnimation_0001()
            {
                var result = _c.CreateColorKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, Color.FromArgb(0xFF, 0x75, 0x79, 0x80), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.3426574F, Color.FromArgb(0xFF, 0x75, 0x79, 0x80), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.4965035F, Color.FromArgb(0x00, 0x75, 0x79, 0x80), CubicBezierEasingFunction_0009());
                return result;
            }
            
            ColorKeyFrameAnimation ColorKeyFrameAnimation_0002()
            {
                if (_colorKeyFrameAnimation_0002 != null)
                {
                    return _colorKeyFrameAnimation_0002;
                }
                var result = _colorKeyFrameAnimation_0002 = _c.CreateColorKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.3426574F, Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.4895105F, Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF), CubicBezierEasingFunction_0000());
                return result;
            }
            
            ColorKeyFrameAnimation ColorKeyFrameAnimation_0003()
            {
                var result = _c.CreateColorKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, Color.FromArgb(0x00, 0x75, 0x79, 0x80), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.4965035F, Color.FromArgb(0x00, 0x75, 0x79, 0x80), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.6153846F, Color.FromArgb(0xFF, 0x75, 0x79, 0x80), CubicBezierEasingFunction_0011());
                return result;
            }
            
            ColorKeyFrameAnimation ColorKeyFrameAnimation_0004()
            {
                if (_colorKeyFrameAnimation_0004 != null)
                {
                    return _colorKeyFrameAnimation_0004;
                }
                var result = _colorKeyFrameAnimation_0004 = _c.CreateColorKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.4895105F, Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.6363636F, Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), CubicBezierEasingFunction_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0000()
            {
                if (_compositionColorBrush_0000 != null)
                {
                    return _compositionColorBrush_0000;
                }
                var result = _compositionColorBrush_0000 = _c.CreateColorBrush(Color.FromArgb(0xFF, 0x75, 0x79, 0x80));
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0001()
            {
                if (_compositionColorBrush_0001 != null)
                {
                    return _compositionColorBrush_0001;
                }
                var result = _compositionColorBrush_0001 = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0002()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0003()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0004()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0005()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0006()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0007()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0008()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0009()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0010()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0011()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0012()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0013()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0014()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0015()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0016()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0017()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0018()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0019()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0020()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0021()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0022()
            {
                if (_compositionColorBrush_0022 != null)
                {
                    return _compositionColorBrush_0022;
                }
                var result = _compositionColorBrush_0022 = _c.CreateColorBrush(Color.FromArgb(0xFF, 0x00, 0xD0, 0xC0));
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0023()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0024()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0025()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0026()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0027()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0028()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0029()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0030()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0031()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0032()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0033()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0034()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0035()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0036()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0037()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0038()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0039()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0040()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0041()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0042()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0004());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0043()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0x75, 0x79, 0x80));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0001());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0044()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0045()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0046()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0047()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0048()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0049()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0050()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0051()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0052()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0053()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0054()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0055()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0056()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0057()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0058()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0059()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0060()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0061()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0062()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0063()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0064()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0x75, 0x79, 0x80));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0003());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0065()
            {
                if (_compositionColorBrush_0065 != null)
                {
                    return _compositionColorBrush_0065;
                }
                var result = _compositionColorBrush_0065 = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xE2, 0x14, 0x14));
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0066()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0067()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0068()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0069()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0070()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0071()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0072()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0073()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0074()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0075()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0076()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0077()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0078()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0079()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0080()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0081()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0082()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0083()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0084()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0085()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0086()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0087()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0088()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0089()
            {
                var result = _c.CreateColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                result.StartAnimation("Color", ColorKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Color");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0090()
            {
                if (_compositionColorBrush_0090 != null)
                {
                    return _compositionColorBrush_0090;
                }
                var result = _compositionColorBrush_0090 = _c.CreateColorBrush(Color.FromArgb(0xFF, 0x37, 0x3E, 0x44));
                return result;
            }
            
            CompositionColorBrush CompositionColorBrush_0091()
            {
                if (_compositionColorBrush_0091 != null)
                {
                    return _compositionColorBrush_0091;
                }
                var result = _compositionColorBrush_0091 = _c.CreateColorBrush(Color.FromArgb(0xFF, 0x23, 0xF1, 0xC0));
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0000()
            {
                var result = _c.CreateContainerShape();
                result.Offset = new Vector2(168.5F, 158.375F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0001());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0001()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0000());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0001());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0002()
            {
                var result = _c.CreateContainerShape();
                result.CenterPoint = new Vector2(10.464F, -1.864F);
                result.Offset = new Vector2(158.032F, 160.334F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0003());
                result.StartAnimation("Scale", Vector2KeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("Scale");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0002());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0003()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0004());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0001());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0004()
            {
                var result = _c.CreateContainerShape();
                result.Offset = new Vector2(8, 0);
                result.Scale = new Vector2(0.5F, 0.5F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0005());
                shapes.Add(CompositionContainerShape_0009());
                shapes.Add(CompositionContainerShape_0010());
                shapes.Add(CompositionContainerShape_0011());
                shapes.Add(CompositionContainerShape_0012());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0005()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0006());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0006()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0007());
                shapes.Add(CompositionContainerShape_0008());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0007()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0001());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0008()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0002());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0009()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0003());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0010()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0004());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0011()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0005());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0012()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0006());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0013()
            {
                var result = _c.CreateContainerShape();
                result.Offset = new Vector2(168.5F, 158.375F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0014());
                result.StartAnimation("Scale", Vector2KeyFrameAnimation_0001());
                var controller = result.TryGetAnimationController("Scale");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0014()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0007());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0003());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0015()
            {
                var result = _c.CreateContainerShape();
                result.CenterPoint = new Vector2(10.464F, -1.864F);
                result.Offset = new Vector2(158.032F, 160.334F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0016());
                result.StartAnimation("Scale", Vector2KeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("Scale");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0002());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0016()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0017());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0003());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0017()
            {
                var result = _c.CreateContainerShape();
                result.Offset = new Vector2(8, 0);
                result.Scale = new Vector2(0.5F, 0.5F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0018());
                shapes.Add(CompositionContainerShape_0022());
                shapes.Add(CompositionContainerShape_0023());
                shapes.Add(CompositionContainerShape_0024());
                shapes.Add(CompositionContainerShape_0025());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0018()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0019());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0019()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0020());
                shapes.Add(CompositionContainerShape_0021());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0020()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0008());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0021()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0009());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0022()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0010());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0023()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0011());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0024()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0012());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0025()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0013());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0026()
            {
                var result = _c.CreateContainerShape();
                result.CenterPoint = new Vector2(91.45F, -4.125F);
                result.Offset = new Vector2(79.418F, 257);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0027());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0027()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0028());
                shapes.Add(CompositionContainerShape_0029());
                shapes.Add(CompositionContainerShape_0030());
                shapes.Add(CompositionContainerShape_0031());
                shapes.Add(CompositionContainerShape_0032());
                shapes.Add(CompositionContainerShape_0033());
                shapes.Add(CompositionContainerShape_0034());
                shapes.Add(CompositionContainerShape_0035());
                shapes.Add(CompositionContainerShape_0036());
                shapes.Add(CompositionContainerShape_0037());
                shapes.Add(CompositionContainerShape_0038());
                shapes.Add(CompositionContainerShape_0039());
                shapes.Add(CompositionContainerShape_0040());
                shapes.Add(CompositionContainerShape_0041());
                shapes.Add(CompositionContainerShape_0042());
                shapes.Add(CompositionContainerShape_0043());
                shapes.Add(CompositionContainerShape_0044());
                shapes.Add(CompositionContainerShape_0045());
                shapes.Add(CompositionContainerShape_0046());
                shapes.Add(CompositionContainerShape_0047());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0003());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0028()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0014());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0029()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0015());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0030()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0016());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0031()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0017());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0032()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0018());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0033()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0019());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0034()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0020());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0035()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0021());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0036()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0022());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0037()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0023());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0038()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0024());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0039()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0025());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0040()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0026());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0041()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0027());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0042()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0028());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0043()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0029());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0044()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0030());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0045()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0031());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0046()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0032());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0047()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0033());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0048()
            {
                var result = _c.CreateContainerShape();
                result.Offset = new Vector2(168.5F, 158.375F);
                result.Scale = new Vector2(1.05F, 1.05F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0049());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0049()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0034());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0005());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0050()
            {
                var result = _c.CreateContainerShape();
                var propertySet = result.Properties;
                propertySet.InsertVector2("Anchor", new Vector2(10.464F, -1.864F));
                propertySet.InsertVector2("Position", new Vector2(168.496F, 158.47F));
                result.CenterPoint = new Vector2(10.464F, -1.864F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0051());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "my.Position-my.Anchor";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("Offset", _expressionAnimation);
                result.StartAnimation("Position", Vector2KeyFrameAnimation_0003());
                var controller = result.TryGetAnimationController("Position");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("Scale", Vector2KeyFrameAnimation_0002());
                controller = result.TryGetAnimationController("Scale");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0002());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0051()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0052());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0005());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0052()
            {
                var result = _c.CreateContainerShape();
                result.Offset = new Vector2(8, 0);
                result.Scale = new Vector2(0.5F, 0.5F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0053());
                shapes.Add(CompositionContainerShape_0057());
                shapes.Add(CompositionContainerShape_0058());
                shapes.Add(CompositionContainerShape_0059());
                shapes.Add(CompositionContainerShape_0060());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0053()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0054());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0054()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0055());
                shapes.Add(CompositionContainerShape_0056());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0055()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0035());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0056()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0036());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0057()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0037());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0058()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0038());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0059()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0039());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0060()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0040());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0061()
            {
                var result = _c.CreateContainerShape();
                result.CenterPoint = new Vector2(-51.5F, -82);
                result.Offset = new Vector2(220, 241.875F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0062());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0062()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0063());
                shapes.Add(CompositionContainerShape_0064());
                shapes.Add(CompositionContainerShape_0065());
                shapes.Add(CompositionContainerShape_0066());
                shapes.Add(CompositionContainerShape_0067());
                shapes.Add(CompositionContainerShape_0068());
                shapes.Add(CompositionContainerShape_0069());
                shapes.Add(CompositionContainerShape_0070());
                shapes.Add(CompositionContainerShape_0071());
                shapes.Add(CompositionContainerShape_0072());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0005());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0063()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0041());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0064()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0042());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0065()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0043());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0066()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0044());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0067()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0045());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0068()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0046());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0069()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0047());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0070()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0048());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0071()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0049());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0072()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0050());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0073()
            {
                var result = _c.CreateContainerShape();
                result.CenterPoint = new Vector2(91.45F, -4.125F);
                result.Offset = new Vector2(79.418F, 257);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0074());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0074()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0075());
                shapes.Add(CompositionContainerShape_0076());
                shapes.Add(CompositionContainerShape_0077());
                shapes.Add(CompositionContainerShape_0078());
                shapes.Add(CompositionContainerShape_0079());
                shapes.Add(CompositionContainerShape_0080());
                shapes.Add(CompositionContainerShape_0081());
                shapes.Add(CompositionContainerShape_0082());
                shapes.Add(CompositionContainerShape_0083());
                shapes.Add(CompositionContainerShape_0084());
                shapes.Add(CompositionContainerShape_0085());
                shapes.Add(CompositionContainerShape_0086());
                shapes.Add(CompositionContainerShape_0087());
                shapes.Add(CompositionContainerShape_0088());
                shapes.Add(CompositionContainerShape_0089());
                shapes.Add(CompositionContainerShape_0090());
                shapes.Add(CompositionContainerShape_0091());
                shapes.Add(CompositionContainerShape_0092());
                shapes.Add(CompositionContainerShape_0093());
                shapes.Add(CompositionContainerShape_0094());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0005());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0075()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0051());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0076()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0052());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0077()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0053());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0078()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0054());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0079()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0055());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0080()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0056());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0081()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0057());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0082()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0058());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0083()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0059());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0084()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0060());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0085()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0061());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0086()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0062());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0087()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0063());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0088()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0064());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0089()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0065());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0090()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0066());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0091()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0067());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0092()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0068());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0093()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0069());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0094()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0070());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0095()
            {
                var result = _c.CreateContainerShape();
                result.Offset = new Vector2(168.5F, 158.375F);
                result.Scale = new Vector2(1.05F, 1.05F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0096());
                result.StartAnimation("Scale", Vector2KeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("Scale");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0096()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0071());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0006());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0097()
            {
                var result = _c.CreateContainerShape();
                var propertySet = result.Properties;
                propertySet.InsertVector2("Anchor", new Vector2(10.464F, -1.864F));
                propertySet.InsertVector2("Position", new Vector2(168.496F, 158.47F));
                result.CenterPoint = new Vector2(10.464F, -1.864F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0098());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "my.Position-my.Anchor";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("Offset", _expressionAnimation);
                result.StartAnimation("Scale", Vector2KeyFrameAnimation_0005());
                var controller = result.TryGetAnimationController("Scale");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0007());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0098()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0099());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0006());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0099()
            {
                var result = _c.CreateContainerShape();
                result.Offset = new Vector2(8, 0);
                result.Scale = new Vector2(0.5F, 0.5F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0100());
                shapes.Add(CompositionContainerShape_0104());
                shapes.Add(CompositionContainerShape_0105());
                shapes.Add(CompositionContainerShape_0106());
                shapes.Add(CompositionContainerShape_0107());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0100()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0101());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0101()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0102());
                shapes.Add(CompositionContainerShape_0103());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0102()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0072());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0103()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0073());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0104()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0074());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0105()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0075());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0106()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0076());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0107()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0077());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0108()
            {
                var result = _c.CreateContainerShape();
                result.CenterPoint = new Vector2(91.45F, -4.125F);
                result.Offset = new Vector2(79.418F, 257);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0109());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0109()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0110());
                shapes.Add(CompositionContainerShape_0111());
                shapes.Add(CompositionContainerShape_0112());
                shapes.Add(CompositionContainerShape_0113());
                shapes.Add(CompositionContainerShape_0114());
                shapes.Add(CompositionContainerShape_0115());
                shapes.Add(CompositionContainerShape_0116());
                shapes.Add(CompositionContainerShape_0117());
                shapes.Add(CompositionContainerShape_0118());
                shapes.Add(CompositionContainerShape_0119());
                shapes.Add(CompositionContainerShape_0120());
                shapes.Add(CompositionContainerShape_0121());
                shapes.Add(CompositionContainerShape_0122());
                shapes.Add(CompositionContainerShape_0123());
                shapes.Add(CompositionContainerShape_0124());
                shapes.Add(CompositionContainerShape_0125());
                shapes.Add(CompositionContainerShape_0126());
                shapes.Add(CompositionContainerShape_0127());
                shapes.Add(CompositionContainerShape_0128());
                shapes.Add(CompositionContainerShape_0129());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0006());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0110()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0078());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0111()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0079());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0112()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0080());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0113()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0081());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0114()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0082());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0115()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0083());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0116()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0084());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0117()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0085());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0118()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0086());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0119()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0087());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0120()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0088());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0121()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0089());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0122()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0090());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0123()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0091());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0124()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0092());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0125()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0093());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0126()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0094());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0127()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0095());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0128()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0096());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0129()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0097());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0130()
            {
                var result = _c.CreateContainerShape();
                result.Offset = new Vector2(168.5F, 158.375F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0131());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0131()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0098());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0008());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0132()
            {
                var result = _c.CreateContainerShape();
                var propertySet = result.Properties;
                propertySet.InsertVector2("Anchor", new Vector2(10.464F, -1.864F));
                propertySet.InsertVector2("Position", new Vector2(168.496F, 158.47F));
                result.CenterPoint = new Vector2(10.464F, -1.864F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0133());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "my.Position-my.Anchor";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("Offset", _expressionAnimation);
                result.StartAnimation("Scale", Vector2KeyFrameAnimation_0006());
                var controller = result.TryGetAnimationController("Scale");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0007());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0133()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0134());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0008());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0134()
            {
                var result = _c.CreateContainerShape();
                result.Offset = new Vector2(8, 0);
                result.Scale = new Vector2(0.5F, 0.5F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0135());
                shapes.Add(CompositionContainerShape_0139());
                shapes.Add(CompositionContainerShape_0140());
                shapes.Add(CompositionContainerShape_0141());
                shapes.Add(CompositionContainerShape_0142());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0135()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0136());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0136()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0137());
                shapes.Add(CompositionContainerShape_0138());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0137()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0099());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0138()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0100());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0139()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0101());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0140()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0102());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0141()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0103());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0142()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0104());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0143()
            {
                var result = _c.CreateContainerShape();
                result.CenterPoint = new Vector2(-51.5F, -82);
                result.Offset = new Vector2(216, 248.375F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0144());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0144()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0145());
                shapes.Add(CompositionContainerShape_0146());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0008());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0145()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0105());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0146()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0106());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0147()
            {
                var result = _c.CreateContainerShape();
                result.CenterPoint = new Vector2(91.45F, -4.125F);
                result.Offset = new Vector2(79.418F, 257);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0148());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0148()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0149());
                shapes.Add(CompositionContainerShape_0150());
                shapes.Add(CompositionContainerShape_0151());
                shapes.Add(CompositionContainerShape_0152());
                shapes.Add(CompositionContainerShape_0153());
                shapes.Add(CompositionContainerShape_0154());
                shapes.Add(CompositionContainerShape_0155());
                shapes.Add(CompositionContainerShape_0156());
                shapes.Add(CompositionContainerShape_0157());
                shapes.Add(CompositionContainerShape_0158());
                shapes.Add(CompositionContainerShape_0159());
                shapes.Add(CompositionContainerShape_0160());
                shapes.Add(CompositionContainerShape_0161());
                shapes.Add(CompositionContainerShape_0162());
                shapes.Add(CompositionContainerShape_0163());
                shapes.Add(CompositionContainerShape_0164());
                shapes.Add(CompositionContainerShape_0165());
                shapes.Add(CompositionContainerShape_0166());
                shapes.Add(CompositionContainerShape_0167());
                shapes.Add(CompositionContainerShape_0168());
                shapes.Add(CompositionContainerShape_0169());
                shapes.Add(CompositionContainerShape_0170());
                shapes.Add(CompositionContainerShape_0171());
                shapes.Add(CompositionContainerShape_0172());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0008());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0149()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0107());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0150()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0108());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0151()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0109());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0152()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0110());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0153()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0111());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0154()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0112());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0155()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0113());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0156()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0114());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0157()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0115());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0158()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0116());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0159()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0117());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0160()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0118());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0161()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0119());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0162()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0120());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0163()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0121());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0164()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0122());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0165()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0123());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0166()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0124());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0167()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0125());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0168()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0126());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0169()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0127());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0170()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0128());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0171()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0129());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0172()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0130());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0173()
            {
                var result = _c.CreateContainerShape();
                result.CenterPoint = new Vector2(15.5F, -168.25F);
                result.Offset = new Vector2(219.875F, 241.875F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0174());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0174()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0175());
                shapes.Add(CompositionContainerShape_0176());
                shapes.Add(CompositionContainerShape_0177());
                shapes.Add(CompositionContainerShape_0178());
                shapes.Add(CompositionContainerShape_0179());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0009());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0175()
            {
                var result = _c.CreateContainerShape();
                var propertySet = result.Properties;
                propertySet.InsertVector2("Anchor", new Vector2(0, 0));
                propertySet.InsertVector2("Position", new Vector2(-50.75F, -86.75F));
                result.RotationAngleInDegrees = -307;
                result.Scale = new Vector2(0, 0);
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0131());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "my.Position-my.Anchor";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("Offset", _expressionAnimation);
                result.StartAnimation("Position", Vector2KeyFrameAnimation_0007());
                var controller = result.TryGetAnimationController("Position");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("Scale", Vector2KeyFrameAnimation_0008());
                controller = result.TryGetAnimationController("Scale");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("RotationAngleInDegrees", ScalarKeyFrameAnimation_0024());
                controller = result.TryGetAnimationController("RotationAngleInDegrees");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0176()
            {
                var result = _c.CreateContainerShape();
                var propertySet = result.Properties;
                propertySet.InsertVector2("Anchor", new Vector2(0, 0));
                propertySet.InsertVector2("Position", new Vector2(-50.75F, -86.75F));
                result.RotationAngleInDegrees = -307;
                result.Scale = new Vector2(0, 0);
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0132());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "my.Position-my.Anchor";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("Offset", _expressionAnimation);
                result.StartAnimation("Position", Vector2KeyFrameAnimation_0009());
                var controller = result.TryGetAnimationController("Position");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("Scale", Vector2KeyFrameAnimation_0010());
                controller = result.TryGetAnimationController("Scale");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("RotationAngleInDegrees", ScalarKeyFrameAnimation_0024());
                controller = result.TryGetAnimationController("RotationAngleInDegrees");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0177()
            {
                var result = _c.CreateContainerShape();
                var propertySet = result.Properties;
                propertySet.InsertVector2("Anchor", new Vector2(0, 0));
                propertySet.InsertVector2("Position", new Vector2(-50.75F, -86.75F));
                result.RotationAngleInDegrees = -307;
                result.Scale = new Vector2(0, 0);
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0133());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "my.Position-my.Anchor";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("Offset", _expressionAnimation);
                result.StartAnimation("Position", Vector2KeyFrameAnimation_0011());
                var controller = result.TryGetAnimationController("Position");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("Scale", Vector2KeyFrameAnimation_0012());
                controller = result.TryGetAnimationController("Scale");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("RotationAngleInDegrees", ScalarKeyFrameAnimation_0024());
                controller = result.TryGetAnimationController("RotationAngleInDegrees");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0178()
            {
                var result = _c.CreateContainerShape();
                var propertySet = result.Properties;
                propertySet.InsertVector2("Anchor", new Vector2(0, 0));
                propertySet.InsertVector2("Position", new Vector2(-50.75F, -86.75F));
                result.RotationAngleInDegrees = -307;
                result.Scale = new Vector2(0, 0);
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0134());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "my.Position-my.Anchor";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("Offset", _expressionAnimation);
                result.StartAnimation("Position", Vector2KeyFrameAnimation_0013());
                var controller = result.TryGetAnimationController("Position");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("Scale", Vector2KeyFrameAnimation_0012());
                controller = result.TryGetAnimationController("Scale");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("RotationAngleInDegrees", ScalarKeyFrameAnimation_0024());
                controller = result.TryGetAnimationController("RotationAngleInDegrees");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0179()
            {
                var result = _c.CreateContainerShape();
                var propertySet = result.Properties;
                propertySet.InsertVector2("Anchor", new Vector2(0, 0));
                propertySet.InsertVector2("Position", new Vector2(-50.75F, -86.75F));
                result.RotationAngleInDegrees = -307;
                result.Scale = new Vector2(0, 0);
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0135());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "my.Position-my.Anchor";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("Offset", _expressionAnimation);
                result.StartAnimation("Position", Vector2KeyFrameAnimation_0014());
                var controller = result.TryGetAnimationController("Position");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("Scale", Vector2KeyFrameAnimation_0015());
                controller = result.TryGetAnimationController("Scale");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("RotationAngleInDegrees", ScalarKeyFrameAnimation_0024());
                controller = result.TryGetAnimationController("RotationAngleInDegrees");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0180()
            {
                var result = _c.CreateContainerShape();
                result.CenterPoint = new Vector2(-51.5F, -82);
                result.Offset = new Vector2(218.375F, 244.375F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0181());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0181()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0182());
                shapes.Add(CompositionContainerShape_0183());
                shapes.Add(CompositionContainerShape_0184());
                shapes.Add(CompositionContainerShape_0185());
                shapes.Add(CompositionContainerShape_0186());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0009());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0182()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0136());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0183()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0137());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0184()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0138());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0185()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0139());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0186()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0140());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0187()
            {
                var result = _c.CreateContainerShape();
                result.CenterPoint = new Vector2(-51.5F, -82);
                result.Offset = new Vector2(219.875F, 241.875F);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0188());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0188()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0189());
                shapes.Add(CompositionContainerShape_0190());
                shapes.Add(CompositionContainerShape_0191());
                shapes.Add(CompositionContainerShape_0192());
                shapes.Add(CompositionContainerShape_0193());
                shapes.Add(CompositionContainerShape_0194());
                shapes.Add(CompositionContainerShape_0195());
                shapes.Add(CompositionContainerShape_0196());
                shapes.Add(CompositionContainerShape_0197());
                shapes.Add(CompositionContainerShape_0198());
                result.StartAnimation("TransformMatrix", ExpressionAnimation_0009());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0189()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0141());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0190()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0142());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0191()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0143());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0192()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0144());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0193()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0145());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0194()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0146());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0195()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0147());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0196()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0148());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0197()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0149());
                return result;
            }
            
            CompositionContainerShape CompositionContainerShape_0198()
            {
                var result = _c.CreateContainerShape();
                var shapes = result.Shapes;
                shapes.Add(CompositionSpriteShape_0150());
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0000()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0000()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0001()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0001()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0002()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0002()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0003()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0003()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0004()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0004()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0005()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0005()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0006()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0000()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0007()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0001()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0008()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0002()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0009()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0003()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0010()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0004()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0011()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0005()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0012()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0006()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0013()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0009()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0014()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0010()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0015()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0013()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0016()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0014()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0017()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0017()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0018()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0018()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0019()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0021()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0020()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0022()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0021()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0023()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0022()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0026()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0023()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0027()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0024()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0028()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0025()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0029()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0026()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0030()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0027()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0033()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0028()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0034()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0029()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0037()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0030()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0040()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0031()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0041()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0032()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0000()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0033()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0001()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0034()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0002()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0035()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0003()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0036()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0004()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0037()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0005()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0038()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0044()));
                var propertySet = result.Properties;
                propertySet.InsertScalar("TStart", 1);
                propertySet.InsertScalar("TEnd", 1);
                result.StartAnimation("TStart", ScalarKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("TStart");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("TEnd", ScalarKeyFrameAnimation_0003());
                controller = result.TryGetAnimationController("TEnd");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Min(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimStart", _expressionAnimation);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Max(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimEnd", _expressionAnimation);
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0039()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0045()));
                var propertySet = result.Properties;
                propertySet.InsertScalar("TStart", 1);
                propertySet.InsertScalar("TEnd", 1);
                result.StartAnimation("TStart", ScalarKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("TStart");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("TEnd", ScalarKeyFrameAnimation_0005());
                controller = result.TryGetAnimationController("TEnd");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Min(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimStart", _expressionAnimation);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Max(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimEnd", _expressionAnimation);
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0040()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0046()));
                var propertySet = result.Properties;
                propertySet.InsertScalar("TStart", 1);
                propertySet.InsertScalar("TEnd", 1);
                result.StartAnimation("TStart", ScalarKeyFrameAnimation_0006());
                var controller = result.TryGetAnimationController("TStart");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("TEnd", ScalarKeyFrameAnimation_0007());
                controller = result.TryGetAnimationController("TEnd");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Min(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimStart", _expressionAnimation);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Max(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimEnd", _expressionAnimation);
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0041()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0047()));
                var propertySet = result.Properties;
                propertySet.InsertScalar("TStart", 1);
                propertySet.InsertScalar("TEnd", 1);
                result.StartAnimation("TStart", ScalarKeyFrameAnimation_0008());
                var controller = result.TryGetAnimationController("TStart");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("TEnd", ScalarKeyFrameAnimation_0009());
                controller = result.TryGetAnimationController("TEnd");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Min(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimStart", _expressionAnimation);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Max(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimEnd", _expressionAnimation);
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0042()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0048()));
                var propertySet = result.Properties;
                propertySet.InsertScalar("TStart", 1);
                propertySet.InsertScalar("TEnd", 1);
                result.StartAnimation("TStart", ScalarKeyFrameAnimation_0004());
                var controller = result.TryGetAnimationController("TStart");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("TEnd", ScalarKeyFrameAnimation_0010());
                controller = result.TryGetAnimationController("TEnd");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Min(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimStart", _expressionAnimation);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Max(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimEnd", _expressionAnimation);
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0043()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0049()));
                var propertySet = result.Properties;
                propertySet.InsertScalar("TStart", 1);
                propertySet.InsertScalar("TEnd", 1);
                result.StartAnimation("TStart", ScalarKeyFrameAnimation_0002());
                var controller = result.TryGetAnimationController("TStart");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("TEnd", ScalarKeyFrameAnimation_0011());
                controller = result.TryGetAnimationController("TEnd");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Min(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimStart", _expressionAnimation);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Max(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimEnd", _expressionAnimation);
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0044()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0050()));
                var propertySet = result.Properties;
                propertySet.InsertScalar("TStart", 1);
                propertySet.InsertScalar("TEnd", 1);
                result.StartAnimation("TStart", ScalarKeyFrameAnimation_0012());
                var controller = result.TryGetAnimationController("TStart");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("TEnd", ScalarKeyFrameAnimation_0013());
                controller = result.TryGetAnimationController("TEnd");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Min(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimStart", _expressionAnimation);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Max(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimEnd", _expressionAnimation);
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0045()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0051()));
                var propertySet = result.Properties;
                propertySet.InsertScalar("TStart", 1);
                propertySet.InsertScalar("TEnd", 1);
                result.StartAnimation("TStart", ScalarKeyFrameAnimation_0014());
                var controller = result.TryGetAnimationController("TStart");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("TEnd", ScalarKeyFrameAnimation_0015());
                controller = result.TryGetAnimationController("TEnd");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Min(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimStart", _expressionAnimation);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Max(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimEnd", _expressionAnimation);
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0046()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0052()));
                var propertySet = result.Properties;
                propertySet.InsertScalar("TStart", 1);
                propertySet.InsertScalar("TEnd", 1);
                result.StartAnimation("TStart", ScalarKeyFrameAnimation_0016());
                var controller = result.TryGetAnimationController("TStart");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("TEnd", ScalarKeyFrameAnimation_0003());
                controller = result.TryGetAnimationController("TEnd");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Min(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimStart", _expressionAnimation);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Max(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimEnd", _expressionAnimation);
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0047()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0053()));
                var propertySet = result.Properties;
                propertySet.InsertScalar("TStart", 1);
                propertySet.InsertScalar("TEnd", 1);
                result.StartAnimation("TStart", ScalarKeyFrameAnimation_0017());
                var controller = result.TryGetAnimationController("TStart");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("TEnd", ScalarKeyFrameAnimation_0003());
                controller = result.TryGetAnimationController("TEnd");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Min(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimStart", _expressionAnimation);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Max(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimEnd", _expressionAnimation);
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0048()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0054()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0049()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0009()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0050()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0055()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0051()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0013()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0052()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0056()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0053()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0017()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0054()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0057()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0055()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0021()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0056()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0022()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0057()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0058()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0058()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0026()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0059()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0027()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0060()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0028()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0061()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0029()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0062()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0059()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0063()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0033()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0064()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0060()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0065()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0061()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0066()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0040()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0067()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0062()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0068()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0000()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0069()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0001()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0070()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0002()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0071()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0003()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0072()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0004()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0073()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0005()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0074()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0063()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0075()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0009()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0076()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0064()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0077()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0013()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0078()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0065()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0079()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0017()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0080()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0066()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0081()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0021()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0082()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0022()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0083()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0067()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0084()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0026()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0085()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0027()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0086()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0028()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0087()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0029()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0088()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0068()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0089()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0033()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0090()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0069()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0091()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0070()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0092()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0040()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0093()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0071()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0094()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0000()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0095()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0001()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0096()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0002()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0097()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0003()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0098()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0004()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0099()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0005()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0100()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0072()));
                var propertySet = result.Properties;
                propertySet.InsertScalar("TStart", 0);
                propertySet.InsertScalar("TEnd", 0);
                result.StartAnimation("TStart", ScalarKeyFrameAnimation_0019());
                var controller = result.TryGetAnimationController("TStart");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("TEnd", ScalarKeyFrameAnimation_0020());
                controller = result.TryGetAnimationController("TEnd");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Min(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimStart", _expressionAnimation);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Max(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimEnd", _expressionAnimation);
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0101()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0073()));
                var propertySet = result.Properties;
                propertySet.InsertScalar("TStart", 0);
                propertySet.InsertScalar("TEnd", 0);
                result.StartAnimation("TStart", ScalarKeyFrameAnimation_0022());
                var controller = result.TryGetAnimationController("TStart");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("TEnd", ScalarKeyFrameAnimation_0023());
                controller = result.TryGetAnimationController("TEnd");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Min(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimStart", _expressionAnimation);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Max(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimEnd", _expressionAnimation);
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0102()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0074()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0103()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0077()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0104()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0078()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0105()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0081()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0106()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0084()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0107()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0087()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0108()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0090()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0109()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0091()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0110()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0092()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0111()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0093()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0112()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0096()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0113()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0099()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0114()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0102()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0115()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0103()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0116()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0106()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0117()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0109()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0118()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0110()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0119()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0111()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0120()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0114()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0121()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0115()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0122()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0116()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0123()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0117()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0124()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0118()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0125()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0121()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0126()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0122()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0127()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0122()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0128()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0122()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0129()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0123()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0130()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0123()));
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0131()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0124()));
                var propertySet = result.Properties;
                propertySet.InsertScalar("TStart", 0);
                propertySet.InsertScalar("TEnd", 0);
                result.StartAnimation("TStart", ScalarKeyFrameAnimation_0027());
                var controller = result.TryGetAnimationController("TStart");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("TEnd", ScalarKeyFrameAnimation_0028());
                controller = result.TryGetAnimationController("TEnd");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Min(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimStart", _expressionAnimation);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Max(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimEnd", _expressionAnimation);
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0132()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0125()));
                var propertySet = result.Properties;
                propertySet.InsertScalar("TStart", 0);
                propertySet.InsertScalar("TEnd", 0);
                result.StartAnimation("TStart", ScalarKeyFrameAnimation_0029());
                var controller = result.TryGetAnimationController("TStart");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("TEnd", ScalarKeyFrameAnimation_0030());
                controller = result.TryGetAnimationController("TEnd");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Min(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimStart", _expressionAnimation);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Max(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimEnd", _expressionAnimation);
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0133()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0126()));
                var propertySet = result.Properties;
                propertySet.InsertScalar("TStart", 0);
                propertySet.InsertScalar("TEnd", 0);
                result.StartAnimation("TStart", ScalarKeyFrameAnimation_0031());
                var controller = result.TryGetAnimationController("TStart");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("TEnd", ScalarKeyFrameAnimation_0032());
                controller = result.TryGetAnimationController("TEnd");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Min(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimStart", _expressionAnimation);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Max(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimEnd", _expressionAnimation);
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0134()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0127()));
                var propertySet = result.Properties;
                propertySet.InsertScalar("TStart", 0);
                propertySet.InsertScalar("TEnd", 0);
                result.StartAnimation("TStart", ScalarKeyFrameAnimation_0029());
                var controller = result.TryGetAnimationController("TStart");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("TEnd", ScalarKeyFrameAnimation_0030());
                controller = result.TryGetAnimationController("TEnd");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Min(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimStart", _expressionAnimation);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Max(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimEnd", _expressionAnimation);
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0135()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0128()));
                var propertySet = result.Properties;
                propertySet.InsertScalar("TStart", 0);
                propertySet.InsertScalar("TEnd", 0);
                result.StartAnimation("TStart", ScalarKeyFrameAnimation_0031());
                var controller = result.TryGetAnimationController("TStart");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("TEnd", ScalarKeyFrameAnimation_0032());
                controller = result.TryGetAnimationController("TEnd");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Min(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimStart", _expressionAnimation);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Max(my.TStart,my.TEnd)";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("TrimEnd", _expressionAnimation);
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0136()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0044()));
                result.TrimStart = 1;
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0137()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0045()));
                result.TrimStart = 1;
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0138()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0046()));
                result.TrimStart = 1;
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0139()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0047()));
                result.TrimStart = 1;
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0140()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0048()));
                result.TrimStart = 1;
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0141()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0049()));
                result.TrimStart = 1;
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0142()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0050()));
                result.TrimStart = 1;
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0143()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0051()));
                result.TrimStart = 1;
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0144()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0052()));
                result.TrimStart = 1;
                return result;
            }
            
            CompositionPathGeometry CompositionPathGeometry_0145()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(CanvasGeometry_0053()));
                result.TrimStart = 1;
                return result;
            }
            
            CompositionRoundedRectangleGeometry CompositionRoundedRectangleGeometry_0000()
            {
                var result = _c.CreateRoundedRectangleGeometry();
                var propertySet = result.Properties;
                propertySet.InsertVector2("Position", new Vector2(0, 0));
                result.CornerRadius = new Vector2(25, 25);
                result.Size = new Vector2(320, 301);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Vector2(my.Position.X-(my.Size.X/2),my.Position.Y-(my.Size.Y/2))";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("Offset", _expressionAnimation);
                return result;
            }
            
            CompositionRoundedRectangleGeometry CompositionRoundedRectangleGeometry_0001()
            {
                var result = _c.CreateRoundedRectangleGeometry();
                var propertySet = result.Properties;
                propertySet.InsertVector2("Position", new Vector2(0, 0));
                result.CornerRadius = new Vector2(25, 25);
                result.Size = new Vector2(320, 301);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Vector2(my.Position.X-(my.Size.X/2),my.Position.Y-(my.Size.Y/2))";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("Offset", _expressionAnimation);
                return result;
            }
            
            CompositionRoundedRectangleGeometry CompositionRoundedRectangleGeometry_0002()
            {
                var result = _c.CreateRoundedRectangleGeometry();
                var propertySet = result.Properties;
                propertySet.InsertVector2("Position", new Vector2(0, 0));
                result.CornerRadius = new Vector2(25, 25);
                result.Size = new Vector2(320, 301);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Vector2(my.Position.X-(my.Size.X/2),my.Position.Y-(my.Size.Y/2))";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("Offset", _expressionAnimation);
                return result;
            }
            
            CompositionRoundedRectangleGeometry CompositionRoundedRectangleGeometry_0003()
            {
                var result = _c.CreateRoundedRectangleGeometry();
                var propertySet = result.Properties;
                propertySet.InsertVector2("Position", new Vector2(0, 0));
                result.CornerRadius = new Vector2(25, 25);
                result.Size = new Vector2(320, 301);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Vector2(my.Position.X-(my.Size.X/2),my.Position.Y-(my.Size.Y/2))";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("Offset", _expressionAnimation);
                return result;
            }
            
            CompositionRoundedRectangleGeometry CompositionRoundedRectangleGeometry_0004()
            {
                var result = _c.CreateRoundedRectangleGeometry();
                var propertySet = result.Properties;
                propertySet.InsertVector2("Position", new Vector2(0, 0));
                result.CornerRadius = new Vector2(25, 25);
                result.Size = new Vector2(320, 301);
                _expressionAnimation.ClearAllParameters();
                _expressionAnimation.Expression = "Vector2(my.Position.X-(my.Size.X/2),my.Position.Y-(my.Size.Y/2))";
                _expressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("Offset", _expressionAnimation);
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0000()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0000();
                result.Geometry = CompositionRoundedRectangleGeometry_0000();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0001()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0000();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0002()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0001();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0003()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0002();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0004()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0003();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0005()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0004();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0006()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0005();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0007()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0000();
                result.Geometry = CompositionRoundedRectangleGeometry_0001();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0008()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0006();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0009()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0007();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0010()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0008();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0011()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0009();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0012()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0010();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0013()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0011();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0014()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0002();
                result.Geometry = CompositionPathGeometry_0012();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0015()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0003();
                result.Geometry = CompositionPathGeometry_0013();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0016()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0004();
                result.Geometry = CompositionPathGeometry_0014();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0017()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0005();
                result.Geometry = CompositionPathGeometry_0015();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0018()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0006();
                result.Geometry = CompositionPathGeometry_0016();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0019()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0007();
                result.Geometry = CompositionPathGeometry_0017();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0020()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0008();
                result.Geometry = CompositionPathGeometry_0018();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0021()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0009();
                result.Geometry = CompositionPathGeometry_0019();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0022()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0010();
                result.Geometry = CompositionPathGeometry_0020();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0023()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0011();
                result.Geometry = CompositionPathGeometry_0021();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0024()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0012();
                result.Geometry = CompositionPathGeometry_0022();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0025()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0013();
                result.Geometry = CompositionPathGeometry_0023();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0026()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0014();
                result.Geometry = CompositionPathGeometry_0024();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0027()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0015();
                result.Geometry = CompositionPathGeometry_0025();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0028()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0016();
                result.Geometry = CompositionPathGeometry_0026();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0029()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0017();
                result.Geometry = CompositionPathGeometry_0027();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0030()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0018();
                result.Geometry = CompositionPathGeometry_0028();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0031()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0019();
                result.Geometry = CompositionPathGeometry_0029();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0032()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0020();
                result.Geometry = CompositionPathGeometry_0030();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0033()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0021();
                result.Geometry = CompositionPathGeometry_0031();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0034()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0000();
                result.Geometry = CompositionRoundedRectangleGeometry_0002();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0035()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0032();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0036()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0033();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0037()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0034();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0038()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0035();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0039()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0036();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0040()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0037();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0041()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0038();
                result.StrokeBrush = CompositionColorBrush_0022();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeDashOffset = 1;
                var strokeDashArray = result.StrokeDashArray;
                strokeDashArray.Add(13);
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0042()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0039();
                result.StrokeBrush = CompositionColorBrush_0022();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0043()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0040();
                result.StrokeBrush = CompositionColorBrush_0022();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeDashOffset = -2;
                var strokeDashArray = result.StrokeDashArray;
                strokeDashArray.Add(12);
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0044()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0041();
                result.StrokeBrush = CompositionColorBrush_0022();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0045()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0042();
                result.StrokeBrush = CompositionColorBrush_0022();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                var strokeDashArray = result.StrokeDashArray;
                strokeDashArray.Add(11);
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0046()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0043();
                result.StrokeBrush = CompositionColorBrush_0022();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0047()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0044();
                result.StrokeBrush = CompositionColorBrush_0022();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                var strokeDashArray = result.StrokeDashArray;
                strokeDashArray.Add(9);
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0048()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0045();
                result.StrokeBrush = CompositionColorBrush_0022();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0049()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0046();
                result.StrokeBrush = CompositionColorBrush_0022();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0050()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0047();
                result.StrokeBrush = CompositionColorBrush_0022();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeDashOffset = 16;
                var strokeDashArray = result.StrokeDashArray;
                strokeDashArray.Add(7);
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0051()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0023();
                result.Geometry = CompositionPathGeometry_0048();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0052()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0024();
                result.Geometry = CompositionPathGeometry_0049();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0053()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0025();
                result.Geometry = CompositionPathGeometry_0050();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0054()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0026();
                result.Geometry = CompositionPathGeometry_0051();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0055()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0027();
                result.Geometry = CompositionPathGeometry_0052();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0056()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0028();
                result.Geometry = CompositionPathGeometry_0053();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0057()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0029();
                result.Geometry = CompositionPathGeometry_0054();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0058()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0030();
                result.Geometry = CompositionPathGeometry_0055();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0059()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0031();
                result.Geometry = CompositionPathGeometry_0056();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0060()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0032();
                result.Geometry = CompositionPathGeometry_0057();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0061()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0033();
                result.Geometry = CompositionPathGeometry_0058();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0062()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0034();
                result.Geometry = CompositionPathGeometry_0059();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0063()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0035();
                result.Geometry = CompositionPathGeometry_0060();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0064()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0036();
                result.Geometry = CompositionPathGeometry_0061();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0065()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0037();
                result.Geometry = CompositionPathGeometry_0062();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0066()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0038();
                result.Geometry = CompositionPathGeometry_0063();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0067()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0039();
                result.Geometry = CompositionPathGeometry_0064();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0068()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0040();
                result.Geometry = CompositionPathGeometry_0065();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0069()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0041();
                result.Geometry = CompositionPathGeometry_0066();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0070()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0042();
                result.Geometry = CompositionPathGeometry_0067();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0071()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0043();
                result.Geometry = CompositionRoundedRectangleGeometry_0003();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0072()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0068();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0073()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0069();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0074()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0070();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0075()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0071();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0076()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0072();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0077()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0073();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0078()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0044();
                result.Geometry = CompositionPathGeometry_0074();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0079()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0045();
                result.Geometry = CompositionPathGeometry_0075();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0080()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0046();
                result.Geometry = CompositionPathGeometry_0076();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0081()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0047();
                result.Geometry = CompositionPathGeometry_0077();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0082()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0048();
                result.Geometry = CompositionPathGeometry_0078();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0083()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0049();
                result.Geometry = CompositionPathGeometry_0079();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0084()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0050();
                result.Geometry = CompositionPathGeometry_0080();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0085()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0051();
                result.Geometry = CompositionPathGeometry_0081();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0086()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0052();
                result.Geometry = CompositionPathGeometry_0082();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0087()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0053();
                result.Geometry = CompositionPathGeometry_0083();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0088()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0054();
                result.Geometry = CompositionPathGeometry_0084();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0089()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0055();
                result.Geometry = CompositionPathGeometry_0085();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0090()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0056();
                result.Geometry = CompositionPathGeometry_0086();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0091()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0057();
                result.Geometry = CompositionPathGeometry_0087();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0092()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0058();
                result.Geometry = CompositionPathGeometry_0088();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0093()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0059();
                result.Geometry = CompositionPathGeometry_0089();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0094()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0060();
                result.Geometry = CompositionPathGeometry_0090();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0095()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0061();
                result.Geometry = CompositionPathGeometry_0091();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0096()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0062();
                result.Geometry = CompositionPathGeometry_0092();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0097()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0063();
                result.Geometry = CompositionPathGeometry_0093();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0098()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0064();
                result.Geometry = CompositionRoundedRectangleGeometry_0004();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0099()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0094();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0100()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0095();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0101()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0096();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0102()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0097();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0103()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0098();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0104()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0099();
                result.StrokeBrush = CompositionColorBrush_0001();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0105()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0100();
                result.StrokeBrush = CompositionColorBrush_0065();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 0;
                result.StartAnimation("StrokeThickness", ScalarKeyFrameAnimation_0018());
                var controller = result.TryGetAnimationController("StrokeThickness");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0106()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0101();
                result.StrokeBrush = CompositionColorBrush_0065();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 0;
                result.StartAnimation("StrokeThickness", ScalarKeyFrameAnimation_0021());
                var controller = result.TryGetAnimationController("StrokeThickness");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0107()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0066();
                result.Geometry = CompositionPathGeometry_0102();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0108()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0067();
                result.Geometry = CompositionPathGeometry_0103();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0109()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0068();
                result.Geometry = CompositionPathGeometry_0104();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0110()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0069();
                result.Geometry = CompositionPathGeometry_0105();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0111()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0070();
                result.Geometry = CompositionPathGeometry_0106();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0112()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0071();
                result.Geometry = CompositionPathGeometry_0107();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0113()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0072();
                result.Geometry = CompositionPathGeometry_0108();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0114()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0073();
                result.Geometry = CompositionPathGeometry_0109();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0115()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0074();
                result.Geometry = CompositionPathGeometry_0110();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0116()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0075();
                result.Geometry = CompositionPathGeometry_0111();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0117()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0076();
                result.Geometry = CompositionPathGeometry_0112();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0118()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0077();
                result.Geometry = CompositionPathGeometry_0113();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0119()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0078();
                result.Geometry = CompositionPathGeometry_0114();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0120()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0079();
                result.Geometry = CompositionPathGeometry_0115();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0121()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0080();
                result.Geometry = CompositionPathGeometry_0116();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0122()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0081();
                result.Geometry = CompositionPathGeometry_0117();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0123()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0082();
                result.Geometry = CompositionPathGeometry_0118();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0124()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0083();
                result.Geometry = CompositionPathGeometry_0119();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0125()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0084();
                result.Geometry = CompositionPathGeometry_0120();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0126()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0085();
                result.Geometry = CompositionPathGeometry_0121();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0127()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0086();
                result.Geometry = CompositionPathGeometry_0122();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0128()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0087();
                result.Geometry = CompositionPathGeometry_0123();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0129()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0088();
                result.Geometry = CompositionPathGeometry_0124();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0130()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = CompositionColorBrush_0089();
                result.Geometry = CompositionPathGeometry_0125();
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0131()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0126();
                result.StrokeBrush = CompositionColorBrush_0090();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                result.StartAnimation("StrokeThickness", ScalarKeyFrameAnimation_0025());
                var controller = result.TryGetAnimationController("StrokeThickness");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0132()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0127();
                result.StrokeBrush = CompositionColorBrush_0090();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 6;
                result.StartAnimation("StrokeThickness", ScalarKeyFrameAnimation_0026());
                var controller = result.TryGetAnimationController("StrokeThickness");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0133()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0128();
                result.StrokeBrush = CompositionColorBrush_0090();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                result.StartAnimation("StrokeThickness", ScalarKeyFrameAnimation_0025());
                var controller = result.TryGetAnimationController("StrokeThickness");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0134()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0129();
                result.StrokeBrush = CompositionColorBrush_0090();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 4;
                result.StartAnimation("StrokeThickness", ScalarKeyFrameAnimation_0025());
                var controller = result.TryGetAnimationController("StrokeThickness");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0135()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0130();
                result.StrokeBrush = CompositionColorBrush_0090();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 6;
                result.StartAnimation("StrokeThickness", ScalarKeyFrameAnimation_0026());
                var controller = result.TryGetAnimationController("StrokeThickness");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0136()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0131();
                result.StrokeBrush = CompositionColorBrush_0091();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeDashOffset = 12;
                var strokeDashArray = result.StrokeDashArray;
                strokeDashArray.Add(95);
                strokeDashArray.Add(14);
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0137()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0132();
                result.StrokeBrush = CompositionColorBrush_0091();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0138()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0133();
                result.StrokeBrush = CompositionColorBrush_0091();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeDashOffset = -9;
                var strokeDashArray = result.StrokeDashArray;
                strokeDashArray.Add(121);
                strokeDashArray.Add(6);
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0139()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0134();
                result.StrokeBrush = CompositionColorBrush_0091();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0140()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0135();
                result.StrokeBrush = CompositionColorBrush_0091();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeDashOffset = -45;
                var strokeDashArray = result.StrokeDashArray;
                strokeDashArray.Add(28);
                strokeDashArray.Add(18);
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0141()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0136();
                result.StrokeBrush = CompositionColorBrush_0091();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeDashOffset = 1;
                var strokeDashArray = result.StrokeDashArray;
                strokeDashArray.Add(13);
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0142()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0137();
                result.StrokeBrush = CompositionColorBrush_0091();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0143()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0138();
                result.StrokeBrush = CompositionColorBrush_0091();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeDashOffset = -2;
                var strokeDashArray = result.StrokeDashArray;
                strokeDashArray.Add(12);
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0144()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0139();
                result.StrokeBrush = CompositionColorBrush_0091();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0145()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0140();
                result.StrokeBrush = CompositionColorBrush_0091();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                var strokeDashArray = result.StrokeDashArray;
                strokeDashArray.Add(11);
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0146()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0141();
                result.StrokeBrush = CompositionColorBrush_0091();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0147()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0142();
                result.StrokeBrush = CompositionColorBrush_0091();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                var strokeDashArray = result.StrokeDashArray;
                strokeDashArray.Add(9);
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0148()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0143();
                result.StrokeBrush = CompositionColorBrush_0091();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0149()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0144();
                result.StrokeBrush = CompositionColorBrush_0091();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            CompositionSpriteShape CompositionSpriteShape_0150()
            {
                var result = _c.CreateSpriteShape();
                result.Geometry = CompositionPathGeometry_0145();
                result.StrokeBrush = CompositionColorBrush_0091();
                result.StrokeDashCap = CompositionStrokeCap.Round;
                result.StrokeDashOffset = 16;
                var strokeDashArray = result.StrokeDashArray;
                strokeDashArray.Add(7);
                result.StrokeEndCap = CompositionStrokeCap.Round;
                result.StrokeLineJoin = CompositionStrokeLineJoin.Round;
                result.StrokeStartCap = CompositionStrokeCap.Round;
                result.StrokeMiterLimit = 4;
                result.StrokeThickness = 3;
                return result;
            }
            
            ContainerVisual ContainerVisual_0000()
            {
                if (_containerVisual_0000 != null)
                {
                    return _containerVisual_0000;
                }
                var result = _containerVisual_0000 = _c.CreateContainerVisual();
                var propertySet = result.Properties;
                propertySet.InsertScalar("Progress", 0);
                propertySet.InsertScalar("t0", 0);
                propertySet.InsertScalar("t1", 0);
                propertySet.InsertScalar("t2", 0);
                propertySet.InsertScalar("t3", 0);
                propertySet.InsertScalar("t4", 0);
                propertySet.InsertScalar("t5", 0);
                var children = result.Children;
                children.InsertAtTop(ShapeVisual_0000());
                result.StartAnimation("t0", ScalarKeyFrameAnimation_0000());
                var controller = result.TryGetAnimationController("t0");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("t1", ScalarKeyFrameAnimation_0001());
                controller = result.TryGetAnimationController("t1");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("t2", ScalarKeyFrameAnimation_0001());
                controller = result.TryGetAnimationController("t2");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("t3", ScalarKeyFrameAnimation_0001());
                controller = result.TryGetAnimationController("t3");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("t4", ScalarKeyFrameAnimation_0001());
                controller = result.TryGetAnimationController("t4");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                result.StartAnimation("t5", ScalarKeyFrameAnimation_0001());
                controller = result.TryGetAnimationController("t5");
                controller.Pause();
                controller.StartAnimation("Progress", ExpressionAnimation_0000());
                return result;
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0000()
            {
                if (_cubicBezierEasingFunction_0000 != null)
                {
                    return _cubicBezierEasingFunction_0000;
                }
                return _cubicBezierEasingFunction_0000 = _c.CreateCubicBezierEasingFunction(new Vector2(0.167F, 0.167F), new Vector2(0.833F, 0.833F));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0001()
            {
                return _c.CreateCubicBezierEasingFunction(new Vector2(0.205F, 0), new Vector2(0.224F, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0002()
            {
                return _c.CreateCubicBezierEasingFunction(new Vector2(0.751F, 0), new Vector2(0.833F, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0003()
            {
                return _c.CreateCubicBezierEasingFunction(new Vector2(0.746F, 0), new Vector2(0.568F, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0004()
            {
                if (_cubicBezierEasingFunction_0004 != null)
                {
                    return _cubicBezierEasingFunction_0004;
                }
                return _cubicBezierEasingFunction_0004 = _c.CreateCubicBezierEasingFunction(new Vector2(0.751F, 0), new Vector2(0.347F, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0005()
            {
                if (_cubicBezierEasingFunction_0005 != null)
                {
                    return _cubicBezierEasingFunction_0005;
                }
                return _cubicBezierEasingFunction_0005 = _c.CreateCubicBezierEasingFunction(new Vector2(0.333F, 0), new Vector2(0.833F, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0006()
            {
                if (_cubicBezierEasingFunction_0006 != null)
                {
                    return _cubicBezierEasingFunction_0006;
                }
                return _cubicBezierEasingFunction_0006 = _c.CreateCubicBezierEasingFunction(new Vector2(0.073F, 0), new Vector2(0.389F, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0007()
            {
                if (_cubicBezierEasingFunction_0007 != null)
                {
                    return _cubicBezierEasingFunction_0007;
                }
                return _cubicBezierEasingFunction_0007 = _c.CreateCubicBezierEasingFunction(new Vector2(0.699F, 0), new Vector2(1, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0008()
            {
                return _c.CreateCubicBezierEasingFunction(new Vector2(0.898F, 0), new Vector2(0.667F, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0009()
            {
                return _c.CreateCubicBezierEasingFunction(new Vector2(1, 0), new Vector2(0.667F, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0010()
            {
                return _c.CreateCubicBezierEasingFunction(new Vector2(1, 0), new Vector2(0.833F, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0011()
            {
                return _c.CreateCubicBezierEasingFunction(new Vector2(0.333F, 0), new Vector2(0.667F, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0012()
            {
                if (_cubicBezierEasingFunction_0012 != null)
                {
                    return _cubicBezierEasingFunction_0012;
                }
                return _cubicBezierEasingFunction_0012 = _c.CreateCubicBezierEasingFunction(new Vector2(0.167F, 0), new Vector2(0.667F, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0013()
            {
                if (_cubicBezierEasingFunction_0013 != null)
                {
                    return _cubicBezierEasingFunction_0013;
                }
                return _cubicBezierEasingFunction_0013 = _c.CreateCubicBezierEasingFunction(new Vector2(0.112F, 0), new Vector2(0.081F, 0.53F));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0014()
            {
                return _c.CreateCubicBezierEasingFunction(new Vector2(0.67F, 1), new Vector2(0.683F, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0015()
            {
                if (_cubicBezierEasingFunction_0015 != null)
                {
                    return _cubicBezierEasingFunction_0015;
                }
                return _cubicBezierEasingFunction_0015 = _c.CreateCubicBezierEasingFunction(new Vector2(0.828F, 0), new Vector2(0.667F, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0016()
            {
                if (_cubicBezierEasingFunction_0016 != null)
                {
                    return _cubicBezierEasingFunction_0016;
                }
                return _cubicBezierEasingFunction_0016 = _c.CreateCubicBezierEasingFunction(new Vector2(0.49F, 0), new Vector2(1, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0017()
            {
                return _c.CreateCubicBezierEasingFunction(new Vector2(0.67F, 0.895F), new Vector2(0.683F, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0018()
            {
                if (_cubicBezierEasingFunction_0018 != null)
                {
                    return _cubicBezierEasingFunction_0018;
                }
                return _cubicBezierEasingFunction_0018 = _c.CreateCubicBezierEasingFunction(new Vector2(0.167F, 0), new Vector2(0.307F, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0019()
            {
                if (_cubicBezierEasingFunction_0019 != null)
                {
                    return _cubicBezierEasingFunction_0019;
                }
                return _cubicBezierEasingFunction_0019 = _c.CreateCubicBezierEasingFunction(new Vector2(0.611F, 0), new Vector2(0.927F, 1));
            }
            
            CubicBezierEasingFunction CubicBezierEasingFunction_0020()
            {
                if (_cubicBezierEasingFunction_0020 != null)
                {
                    return _cubicBezierEasingFunction_0020;
                }
                return _cubicBezierEasingFunction_0020 = _c.CreateCubicBezierEasingFunction(new Vector2(0, 0), new Vector2(0.301F, 1));
            }
            
            ExpressionAnimation ExpressionAnimation_0000()
            {
                if (_expressionAnimation_0000 != null)
                {
                    return _expressionAnimation_0000;
                }
                var result = _expressionAnimation_0000 = _c.CreateExpressionAnimation();
                result.SetReferenceParameter("_", ContainerVisual_0000());
                result.Expression = "_.Progress";
                return result;
            }
            
            ExpressionAnimation ExpressionAnimation_0001()
            {
                if (_expressionAnimation_0001 != null)
                {
                    return _expressionAnimation_0001;
                }
                var result = _expressionAnimation_0001 = _c.CreateExpressionAnimation();
                result.SetReferenceParameter("_", ContainerVisual_0000());
                result.Expression = "((_.Progress) < 0.01398601) ? (Matrix3x2(1,0,0,1,0,0)) : (Matrix3x2(0,0,0,0,0,0))";
                return result;
            }
            
            ExpressionAnimation ExpressionAnimation_0002()
            {
                if (_expressionAnimation_0002 != null)
                {
                    return _expressionAnimation_0002;
                }
                var result = _expressionAnimation_0002 = _c.CreateExpressionAnimation();
                result.SetReferenceParameter("_", ContainerVisual_0000());
                result.Expression = "_.Progress*0.785714285714286";
                return result;
            }
            
            ExpressionAnimation ExpressionAnimation_0003()
            {
                if (_expressionAnimation_0003 != null)
                {
                    return _expressionAnimation_0003;
                }
                var result = _expressionAnimation_0003 = _c.CreateExpressionAnimation();
                result.SetReferenceParameter("_", ContainerVisual_0000());
                result.Expression = "((_.Progress) < 0.006993007) ? (Matrix3x2(0,0,0,0,0,0)) : (((_.Progress) < 0.1188811) ? (Matrix3x2(1,0,0,1,0,0)) : (Matrix3x2(0,0,0,0,0,0)))";
                return result;
            }
            
            ExpressionAnimation ExpressionAnimation_0004()
            {
                if (_expressionAnimation_0004 != null)
                {
                    return _expressionAnimation_0004;
                }
                var result = _expressionAnimation_0004 = _c.CreateExpressionAnimation();
                result.SetReferenceParameter("_", ContainerVisual_0000());
                result.Expression = "_.Progress*0.82183908045977";
                return result;
            }
            
            ExpressionAnimation ExpressionAnimation_0005()
            {
                if (_expressionAnimation_0005 != null)
                {
                    return _expressionAnimation_0005;
                }
                var result = _expressionAnimation_0005 = _c.CreateExpressionAnimation();
                result.SetReferenceParameter("_", ContainerVisual_0000());
                result.Expression = "((_.Progress) < 0.1188811) ? (Matrix3x2(0,0,0,0,0,0)) : (((_.Progress) < 0.3426574) ? (Matrix3x2(1,0,0,1,0,0)) : (Matrix3x2(0,0,0,0,0,0)))";
                return result;
            }
            
            ExpressionAnimation ExpressionAnimation_0006()
            {
                if (_expressionAnimation_0006 != null)
                {
                    return _expressionAnimation_0006;
                }
                var result = _expressionAnimation_0006 = _c.CreateExpressionAnimation();
                result.SetReferenceParameter("_", ContainerVisual_0000());
                result.Expression = "((_.Progress) < 0.3426574) ? (Matrix3x2(0,0,0,0,0,0)) : (((_.Progress) < 0.4895105) ? (Matrix3x2(1,0,0,1,0,0)) : (Matrix3x2(0,0,0,0,0,0)))";
                return result;
            }
            
            ExpressionAnimation ExpressionAnimation_0007()
            {
                if (_expressionAnimation_0007 != null)
                {
                    return _expressionAnimation_0007;
                }
                var result = _expressionAnimation_0007 = _c.CreateExpressionAnimation();
                result.SetReferenceParameter("_", ContainerVisual_0000());
                result.Expression = "_.Progress*0.569721115537849+0.430278884462151";
                return result;
            }
            
            ExpressionAnimation ExpressionAnimation_0008()
            {
                if (_expressionAnimation_0008 != null)
                {
                    return _expressionAnimation_0008;
                }
                var result = _expressionAnimation_0008 = _c.CreateExpressionAnimation();
                result.SetReferenceParameter("_", ContainerVisual_0000());
                result.Expression = "((_.Progress) < 0.4895105) ? (Matrix3x2(0,0,0,0,0,0)) : (((_.Progress) < 0.6923077) ? (Matrix3x2(1,0,0,1,0,0)) : (Matrix3x2(0,0,0,0,0,0)))";
                return result;
            }
            
            ExpressionAnimation ExpressionAnimation_0009()
            {
                if (_expressionAnimation_0009 != null)
                {
                    return _expressionAnimation_0009;
                }
                var result = _expressionAnimation_0009 = _c.CreateExpressionAnimation();
                result.SetReferenceParameter("_", ContainerVisual_0000());
                result.Expression = "((_.Progress) < 0.6923077) ? (Matrix3x2(0,0,0,0,0,0)) : (Matrix3x2(1,0,0,1,0,0))";
                return result;
            }
            
            LinearEasingFunction LinearEasingFunction_0000()
            {
                if (_linearEasingFunction_0000 != null)
                {
                    return _linearEasingFunction_0000;
                }
                return _linearEasingFunction_0000 = _c.CreateLinearEasingFunction();
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0000()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0.1188812F, 0, StepEasingFunction_0000());
                result.InsertKeyFrame(0.1538461F, 1, CubicBezierEasingFunction_0000());
                result.InsertKeyFrame(0.1538462F, 0, StepEasingFunction_0000());
                result.InsertKeyFrame(0.1888111F, 1, CubicBezierEasingFunction_0000());
                result.InsertKeyFrame(0.1888112F, 0, StepEasingFunction_0000());
                result.InsertKeyFrame(0.2307691F, 1, CubicBezierEasingFunction_0000());
                result.InsertKeyFrame(0.2307692F, 0, StepEasingFunction_0000());
                result.InsertKeyFrame(0.2727272F, 1, CubicBezierEasingFunction_0000());
                result.InsertKeyFrame(0.2727273F, 0, StepEasingFunction_0000());
                result.InsertKeyFrame(0.3076922F, 1, CubicBezierEasingFunction_0000());
                result.InsertKeyFrame(0.3076923F, 0, StepEasingFunction_0000());
                result.InsertKeyFrame(0.3426572F, 1, CubicBezierEasingFunction_0000());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0001()
            {
                if (_scalarKeyFrameAnimation_0001 != null)
                {
                    return _scalarKeyFrameAnimation_0001;
                }
                var result = _scalarKeyFrameAnimation_0001 = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0.6923078F, 0, StepEasingFunction_0000());
                result.InsertKeyFrame(0.9650348F, 1, CubicBezierEasingFunction_0001());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0002()
            {
                if (_scalarKeyFrameAnimation_0002 != null)
                {
                    return _scalarKeyFrameAnimation_0002;
                }
                var result = _scalarKeyFrameAnimation_0002 = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.1188811F, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.2307692F, 0, CubicBezierEasingFunction_0006());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0003()
            {
                if (_scalarKeyFrameAnimation_0003 != null)
                {
                    return _scalarKeyFrameAnimation_0003;
                }
                var result = _scalarKeyFrameAnimation_0003 = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.2307692F, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.3426574F, 0, CubicBezierEasingFunction_0007());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0004()
            {
                if (_scalarKeyFrameAnimation_0004 != null)
                {
                    return _scalarKeyFrameAnimation_0004;
                }
                var result = _scalarKeyFrameAnimation_0004 = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.1188811F, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.2237762F, 0, CubicBezierEasingFunction_0006());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0005()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.2237762F, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.3216783F, 0, CubicBezierEasingFunction_0007());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0006()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.1258741F, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.2447552F, 0, CubicBezierEasingFunction_0006());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0007()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.2447552F, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.3426574F, 0, CubicBezierEasingFunction_0007());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0008()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.1328671F, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.2237762F, 0, CubicBezierEasingFunction_0006());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0009()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.2237762F, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.3426574F, 0, CubicBezierEasingFunction_0007());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0010()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.2237762F, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.3356643F, 0, CubicBezierEasingFunction_0007());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0011()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.2307692F, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.3356643F, 0, CubicBezierEasingFunction_0007());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0012()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.1398601F, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.2377622F, 0, CubicBezierEasingFunction_0006());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0013()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.2377622F, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.3426574F, 0, CubicBezierEasingFunction_0007());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0014()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.1328671F, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.2377622F, 0, CubicBezierEasingFunction_0006());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0015()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.2377622F, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.3286713F, 0, CubicBezierEasingFunction_0007());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0016()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.1188811F, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.2377622F, 0, CubicBezierEasingFunction_0006());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0017()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.1328671F, 1, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.2307692F, 0, CubicBezierEasingFunction_0006());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0018()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.4947972F, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.5794772F, 3, CubicBezierEasingFunction_0000());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0019()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.495986F, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.5692867F, 0.73102F, CubicBezierEasingFunction_0013());
                result.InsertKeyFrame(0.6503497F, 1, CubicBezierEasingFunction_0014());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0020()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.5485804F, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.6500629F, 0.86F, CubicBezierEasingFunction_0015());
                result.InsertKeyFrame(0.6818182F, 0.84F, CubicBezierEasingFunction_0016());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0021()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.4895105F, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.5741778F, 3, CubicBezierEasingFunction_0000());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0022()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.4895105F, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.5715384F, 0.73102F, CubicBezierEasingFunction_0013());
                result.InsertKeyFrame(0.6503497F, 1, CubicBezierEasingFunction_0017());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0023()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.5487342F, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.6496014F, 0.88F, CubicBezierEasingFunction_0015());
                result.InsertKeyFrame(0.6813538F, 0.84F, CubicBezierEasingFunction_0016());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0024()
            {
                if (_scalarKeyFrameAnimation_0024 != null)
                {
                    return _scalarKeyFrameAnimation_0024;
                }
                var result = _scalarKeyFrameAnimation_0024 = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, -307, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.6923077F, -307, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.965035F, -49, CubicBezierEasingFunction_0012());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0025()
            {
                if (_scalarKeyFrameAnimation_0025 != null)
                {
                    return _scalarKeyFrameAnimation_0025;
                }
                var result = _scalarKeyFrameAnimation_0025 = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 4, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.8461539F, 4, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.965035F, 0, CubicBezierEasingFunction_0012());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0026()
            {
                if (_scalarKeyFrameAnimation_0026 != null)
                {
                    return _scalarKeyFrameAnimation_0026;
                }
                var result = _scalarKeyFrameAnimation_0026 = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 6, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.8461539F, 6, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.965035F, 0, CubicBezierEasingFunction_0012());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0027()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.6923077F, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.8096318F, 1, CubicBezierEasingFunction_0019());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0028()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.8096294F, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.9588205F, 1, CubicBezierEasingFunction_0020());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0029()
            {
                if (_scalarKeyFrameAnimation_0029 != null)
                {
                    return _scalarKeyFrameAnimation_0029;
                }
                var result = _scalarKeyFrameAnimation_0029 = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.7195035F, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.8438183F, 1, CubicBezierEasingFunction_0019());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0030()
            {
                if (_scalarKeyFrameAnimation_0030 != null)
                {
                    return _scalarKeyFrameAnimation_0030;
                }
                var result = _scalarKeyFrameAnimation_0030 = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.8438182F, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.993007F, 1, CubicBezierEasingFunction_0020());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0031()
            {
                if (_scalarKeyFrameAnimation_0031 != null)
                {
                    return _scalarKeyFrameAnimation_0031;
                }
                var result = _scalarKeyFrameAnimation_0031 = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.6923077F, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.8166248F, 1, CubicBezierEasingFunction_0019());
                return result;
            }
            
            ScalarKeyFrameAnimation ScalarKeyFrameAnimation_0032()
            {
                if (_scalarKeyFrameAnimation_0032 != null)
                {
                    return _scalarKeyFrameAnimation_0032;
                }
                var result = _scalarKeyFrameAnimation_0032 = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.8166224F, 0, LinearEasingFunction_0000());
                result.InsertKeyFrame(0.9658135F, 1, CubicBezierEasingFunction_0020());
                return result;
            }
            
            ShapeVisual ShapeVisual_0000()
            {
                var result = _c.CreateShapeVisual();
                result.Size = new Vector2(337, 317);
                var shapes = result.Shapes;
                shapes.Add(CompositionContainerShape_0000());
                shapes.Add(CompositionContainerShape_0002());
                shapes.Add(CompositionContainerShape_0013());
                shapes.Add(CompositionContainerShape_0015());
                shapes.Add(CompositionContainerShape_0026());
                shapes.Add(CompositionContainerShape_0048());
                shapes.Add(CompositionContainerShape_0050());
                shapes.Add(CompositionContainerShape_0061());
                shapes.Add(CompositionContainerShape_0073());
                shapes.Add(CompositionContainerShape_0095());
                shapes.Add(CompositionContainerShape_0097());
                shapes.Add(CompositionContainerShape_0108());
                shapes.Add(CompositionContainerShape_0130());
                shapes.Add(CompositionContainerShape_0132());
                shapes.Add(CompositionContainerShape_0143());
                shapes.Add(CompositionContainerShape_0147());
                shapes.Add(CompositionContainerShape_0173());
                shapes.Add(CompositionContainerShape_0180());
                shapes.Add(CompositionContainerShape_0187());
                return result;
            }
            
            StepEasingFunction StepEasingFunction_0000()
            {
                if (_stepEasingFunction_0000 != null)
                {
                    return _stepEasingFunction_0000;
                }
                var result = _stepEasingFunction_0000 = _c.CreateStepEasingFunction();
                result.IsInitialStepSingleFrame  = true;
                return result;
            }
            
            Vector2KeyFrameAnimation Vector2KeyFrameAnimation_0000()
            {
                var result = _c.CreateVector2KeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, new Vector2(1, 1), LinearEasingFunction_0000());
                result.InsertKeyFrame(1, new Vector2(0.85F, 0.85F), CubicBezierEasingFunction_0002());
                return result;
            }
            
            Vector2KeyFrameAnimation Vector2KeyFrameAnimation_0001()
            {
                var result = _c.CreateVector2KeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, new Vector2(1, 1), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.1188811F, new Vector2(1.05F, 1.05F), CubicBezierEasingFunction_0003());
                return result;
            }
            
            Vector2KeyFrameAnimation Vector2KeyFrameAnimation_0002()
            {
                if (_vector2KeyFrameAnimation_0002 != null)
                {
                    return _vector2KeyFrameAnimation_0002;
                }
                var result = _vector2KeyFrameAnimation_0002 = _c.CreateVector2KeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, new Vector2(1, 1), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.0934066F, new Vector2(0.85F, 0.85F), CubicBezierEasingFunction_0004());
                result.InsertKeyFrame(1, new Vector2(0.85F, 0.85F), CubicBezierEasingFunction_0005());
                return result;
            }
            
            Vector2KeyFrameAnimation Vector2KeyFrameAnimation_0003()
            {
                var result = _c.CreateVector2KeyFrameAnimation();
                result.SetReferenceParameter("_", ContainerVisual_0000());
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, new Vector2(168.496F, 158.47F), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.1188811F, new Vector2(168.496F, 158.47F), LinearEasingFunction_0000());
                result.InsertExpressionKeyFrame(0.1538461F, "(Pow(1 - _.t0, 3) * Vector2(168.496,158.47)) + (3 * Square((1 - _.t0)) * _.t0 * Vector2(168.6627,158.1367)) + (3 * (1 - _.t0) * Square(_.t0) * Vector2(169.6627,156.1367)) + (Pow(_.t0, 3) * Vector2(169.496,156.47))", StepEasingFunction_0000());
                result.InsertExpressionKeyFrame(0.1888111F, "(Pow(1 - _.t0, 3) * Vector2(169.496,156.47)) + (3 * Square((1 - _.t0)) * _.t0 * Vector2(169.3293,156.8033)) + (3 * (1 - _.t0) * Square(_.t0) * Vector2(167.8293,160.47)) + (Pow(_.t0, 3) * Vector2(167.496,160.47))", StepEasingFunction_0000());
                result.InsertExpressionKeyFrame(0.2307691F, "(Pow(1 - _.t0, 3) * Vector2(167.496,160.47)) + (3 * Square((1 - _.t0)) * _.t0 * Vector2(167.1627,160.47)) + (3 * (1 - _.t0) * Square(_.t0) * Vector2(167.3293,156.8033)) + (Pow(_.t0, 3) * Vector2(167.496,156.47))", StepEasingFunction_0000());
                result.InsertExpressionKeyFrame(0.2727272F, "(Pow(1 - _.t0, 3) * Vector2(167.496,156.47)) + (3 * Square((1 - _.t0)) * _.t0 * Vector2(167.6627,156.1367)) + (3 * (1 - _.t0) * Square(_.t0) * Vector2(167.996,158.47)) + (Pow(_.t0, 3) * Vector2(168.496,158.47))", StepEasingFunction_0000());
                result.InsertExpressionKeyFrame(0.3076922F, "(Pow(1 - _.t0, 3) * Vector2(168.496,158.47)) + (3 * Square((1 - _.t0)) * _.t0 * Vector2(168.996,158.47)) + (3 * (1 - _.t0) * Square(_.t0) * Vector2(170.496,156.47)) + (Pow(_.t0, 3) * Vector2(170.496,156.47))", StepEasingFunction_0000());
                result.InsertExpressionKeyFrame(0.3426572F, "(Pow(1 - _.t0, 3) * Vector2(170.496,156.47)) + (3 * Square((1 - _.t0)) * _.t0 * Vector2(170.496,156.47)) + (3 * (1 - _.t0) * Square(_.t0) * Vector2(168.8293,158.1367)) + (Pow(_.t0, 3) * Vector2(168.496,158.47))", StepEasingFunction_0000());
                result.InsertKeyFrame(0.3426573F, new Vector2(168.496F, 158.47F), StepEasingFunction_0000());
                return result;
            }
            
            Vector2KeyFrameAnimation Vector2KeyFrameAnimation_0004()
            {
                var result = _c.CreateVector2KeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, new Vector2(1.05F, 1.05F), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.3426574F, new Vector2(1.05F, 1.05F), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.4965035F, new Vector2(1, 1), CubicBezierEasingFunction_0008());
                return result;
            }
            
            Vector2KeyFrameAnimation Vector2KeyFrameAnimation_0005()
            {
                var result = _c.CreateVector2KeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, new Vector2(0.85F, 0.85F), CubicBezierEasingFunction_0004());
                result.InsertKeyFrame(0.6573705F, new Vector2(0.85F, 0.85F), CubicBezierEasingFunction_0005());
                result.InsertKeyFrame(0.7131474F, new Vector2(0, 0), CubicBezierEasingFunction_0010());
                return result;
            }
            
            Vector2KeyFrameAnimation Vector2KeyFrameAnimation_0006()
            {
                var result = _c.CreateVector2KeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, new Vector2(0.85F, 0.85F), CubicBezierEasingFunction_0004());
                result.InsertKeyFrame(0.7171315F, new Vector2(0, 0), CubicBezierEasingFunction_0005());
                result.InsertKeyFrame(0.7729084F, new Vector2(1, 1), CubicBezierEasingFunction_0012());
                return result;
            }
            
            Vector2KeyFrameAnimation Vector2KeyFrameAnimation_0007()
            {
                var result = _c.CreateVector2KeyFrameAnimation();
                result.SetReferenceParameter("_", ContainerVisual_0000());
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, new Vector2(-50.75F, -86.75F), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.6923077F, new Vector2(-50.75F, -86.75F), LinearEasingFunction_0000());
                result.InsertExpressionKeyFrame(0.9650348F, "(Pow(1 - _.t1, 3) * Vector2((-50.75),(-86.75))) + (3 * Square((1 - _.t1)) * _.t1 * Vector2((-34.625),(-105.5833))) + (3 * (1 - _.t1) * Square(_.t1) * Vector2(29.875,(-180.9167))) + (Pow(_.t1, 3) * Vector2(46,(-199.75)))", StepEasingFunction_0000());
                result.InsertKeyFrame(0.965035F, new Vector2(46, -199.75F), StepEasingFunction_0000());
                return result;
            }
            
            Vector2KeyFrameAnimation Vector2KeyFrameAnimation_0008()
            {
                var result = _c.CreateVector2KeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, new Vector2(0, 0), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.6923077F, new Vector2(0, 0), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.965035F, new Vector2(0.77F, 0.77F), CubicBezierEasingFunction_0018());
                return result;
            }
            
            Vector2KeyFrameAnimation Vector2KeyFrameAnimation_0009()
            {
                var result = _c.CreateVector2KeyFrameAnimation();
                result.SetReferenceParameter("_", ContainerVisual_0000());
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, new Vector2(-50.75F, -86.75F), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.6923077F, new Vector2(-50.75F, -86.75F), LinearEasingFunction_0000());
                result.InsertExpressionKeyFrame(0.9650348F, "(Pow(1 - _.t2, 3) * Vector2((-50.75),(-86.75))) + (3 * Square((1 - _.t2)) * _.t2 * Vector2((-53.375),(-76.83334))) + (3 * (1 - _.t2) * Square(_.t2) * Vector2((-63.875),(-37.16667))) + (Pow(_.t2, 3) * Vector2((-66.5),(-27.25)))", StepEasingFunction_0000());
                result.InsertKeyFrame(0.965035F, new Vector2(-66.5F, -27.25F), StepEasingFunction_0000());
                return result;
            }
            
            Vector2KeyFrameAnimation Vector2KeyFrameAnimation_0010()
            {
                var result = _c.CreateVector2KeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, new Vector2(0, 0), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.6923077F, new Vector2(0, 0), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.965035F, new Vector2(0.45F, 0.45F), CubicBezierEasingFunction_0018());
                return result;
            }
            
            Vector2KeyFrameAnimation Vector2KeyFrameAnimation_0011()
            {
                var result = _c.CreateVector2KeyFrameAnimation();
                result.SetReferenceParameter("_", ContainerVisual_0000());
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, new Vector2(-50.75F, -86.75F), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.6923077F, new Vector2(-50.75F, -86.75F), LinearEasingFunction_0000());
                result.InsertExpressionKeyFrame(0.9650348F, "(Pow(1 - _.t3, 3) * Vector2((-50.75),(-86.75))) + (3 * Square((1 - _.t3)) * _.t3 * Vector2((-72.79166),(-87))) + (3 * (1 - _.t3) * Square(_.t3) * Vector2((-160.9583),(-88))) + (Pow(_.t3, 3) * Vector2((-183),(-88.25)))", StepEasingFunction_0000());
                result.InsertKeyFrame(0.965035F, new Vector2(-183, -88.25F), StepEasingFunction_0000());
                return result;
            }
            
            Vector2KeyFrameAnimation Vector2KeyFrameAnimation_0012()
            {
                if (_vector2KeyFrameAnimation_0012 != null)
                {
                    return _vector2KeyFrameAnimation_0012;
                }
                var result = _vector2KeyFrameAnimation_0012 = _c.CreateVector2KeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, new Vector2(0, 0), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.6923077F, new Vector2(0, 0), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.965035F, new Vector2(0.82F, 0.82F), CubicBezierEasingFunction_0018());
                return result;
            }
            
            Vector2KeyFrameAnimation Vector2KeyFrameAnimation_0013()
            {
                var result = _c.CreateVector2KeyFrameAnimation();
                result.SetReferenceParameter("_", ContainerVisual_0000());
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, new Vector2(-50.75F, -86.75F), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.6923077F, new Vector2(-50.75F, -86.75F), LinearEasingFunction_0000());
                result.InsertExpressionKeyFrame(0.9650348F, "(Pow(1 - _.t4, 3) * Vector2((-50.75),(-86.75))) + (3 * Square((1 - _.t4)) * _.t4 * Vector2((-63.54167),(-107.4167))) + (3 * (1 - _.t4) * Square(_.t4) * Vector2((-114.7083),(-190.0833))) + (Pow(_.t4, 3) * Vector2((-127.5),(-210.75)))", StepEasingFunction_0000());
                result.InsertKeyFrame(0.965035F, new Vector2(-127.5F, -210.75F), StepEasingFunction_0000());
                return result;
            }
            
            Vector2KeyFrameAnimation Vector2KeyFrameAnimation_0014()
            {
                var result = _c.CreateVector2KeyFrameAnimation();
                result.SetReferenceParameter("_", ContainerVisual_0000());
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, new Vector2(-50.75F, -86.75F), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.6923077F, new Vector2(-50.75F, -86.75F), LinearEasingFunction_0000());
                result.InsertExpressionKeyFrame(0.9650348F, "(Pow(1 - _.t5, 3) * Vector2((-50.75),(-86.75))) + (3 * Square((1 - _.t5)) * _.t5 * Vector2((-30.70833),(-78.75))) + (3 * (1 - _.t5) * Square(_.t5) * Vector2(49.45834,(-46.75))) + (Pow(_.t5, 3) * Vector2(69.5,(-38.75)))", StepEasingFunction_0000());
                result.InsertKeyFrame(0.965035F, new Vector2(69.5F, -38.75F), StepEasingFunction_0000());
                return result;
            }
            
            Vector2KeyFrameAnimation Vector2KeyFrameAnimation_0015()
            {
                var result = _c.CreateVector2KeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(23830000);
                result.InsertKeyFrame(0, new Vector2(0, 0), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.6923077F, new Vector2(0, 0), LinearEasingFunction_0000());
                result.InsertKeyFrame(0.965035F, new Vector2(0.49F, 0.49F), CubicBezierEasingFunction_0018());
                return result;
            }
            
        }
    }
}
