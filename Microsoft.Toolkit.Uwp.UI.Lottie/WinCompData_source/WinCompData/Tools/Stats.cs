// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools
{
    /// <summary>
    /// Calculates stats for a WinCompData tree. Used to report the size of data
    /// and the effectiveness of optimization.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class Stats
    {
        readonly int _compositionObjectCount;
        readonly int _compositionPathCount;
        readonly int _canvasGeometryCount;
        readonly int _animationControllerCount;
        readonly int _colorKeyFrameAnimationCount;
        readonly int _colorBrushCount;
        readonly int _containerShapeCount;
        readonly int _ellipseGeometryCount;
        readonly int _geometricClipCount;
        readonly int _pathGeometryCount;
        readonly int _propertySetCount;
        readonly int _rectangleGeometryCount;
        readonly int _roundedRectangleGeometryCount;
        readonly int _spriteShapeCount;
        readonly int _viewBoxCount;
        readonly int _containerVisualCount;
        readonly int _cubicBezierEasingFunctionCount;
        readonly int _expressionAnimationCount;
        readonly int _insetClipCount;
        readonly int _linearEasingFunctionCount;
        readonly int _pathKeyFrameAnimationCount;
        readonly int _scalarKeyFrameAnimationCount;
        readonly int _shapeVisualCount;
        readonly int _stepEasingFunctionCount;
        readonly int _vector2KeyFrameAnimationCount;
        readonly int _vector3KeyFrameAnimationCount;
        readonly int _propertySetPropertyCount;

        public Stats(CompositionObject root)
        {
            var objectGraph = Graph.FromCompositionObject(root, includeVertices: false);

            _compositionPathCount = objectGraph.CompositionPathNodes.Count();
            _canvasGeometryCount = objectGraph.CanvasGeometryNodes.Count();

            foreach (var n in objectGraph.CompositionObjectNodes)
            {
                _compositionObjectCount++;
                switch (n.Object.Type)
                {
                    case CompositionObjectType.AnimationController:
                        _animationControllerCount++;
                        break;
                    case CompositionObjectType.ColorKeyFrameAnimation:
                        _colorKeyFrameAnimationCount++;
                        break;
                    case CompositionObjectType.CompositionColorBrush:
                        _colorBrushCount++;
                        break;
                    case CompositionObjectType.CompositionContainerShape:
                        _containerShapeCount++;
                        break;
                    case CompositionObjectType.CompositionEllipseGeometry:
                        _ellipseGeometryCount++;
                        break;
                    case CompositionObjectType.CompositionGeometricClip:
                        _geometricClipCount++;
                        break;
                    case CompositionObjectType.CompositionPathGeometry:
                        _pathGeometryCount++;
                        break;
                    case CompositionObjectType.CompositionPropertySet:
                        {
                            var propertyCount = ((CompositionPropertySet)n.Object).PropertyNames.Count();
                            if (propertyCount > 0)
                            {
                                _propertySetCount++;
                                _propertySetPropertyCount += propertyCount;
                            }
                        }
                        break;
                    case CompositionObjectType.CompositionRectangleGeometry:
                        _rectangleGeometryCount++;
                        break;
                    case CompositionObjectType.CompositionRoundedRectangleGeometry:
                        _roundedRectangleGeometryCount++;
                        break;
                    case CompositionObjectType.CompositionSpriteShape:
                        _spriteShapeCount++;
                        break;
                    case CompositionObjectType.CompositionViewBox:
                        _viewBoxCount++;
                        break;
                    case CompositionObjectType.ContainerVisual:
                        _containerVisualCount++;
                        break;
                    case CompositionObjectType.CubicBezierEasingFunction:
                        _cubicBezierEasingFunctionCount++;
                        break;
                    case CompositionObjectType.ExpressionAnimation:
                        _expressionAnimationCount++;
                        break;
                    case CompositionObjectType.InsetClip:
                        _insetClipCount++;
                        break;
                    case CompositionObjectType.LinearEasingFunction:
                        _linearEasingFunctionCount++;
                        break;
                    case CompositionObjectType.PathKeyFrameAnimation:
                        _pathKeyFrameAnimationCount++;
                        break;
                    case CompositionObjectType.ScalarKeyFrameAnimation:
                        _scalarKeyFrameAnimationCount++;
                        break;
                    case CompositionObjectType.ShapeVisual:
                        _shapeVisualCount++;
                        break;
                    case CompositionObjectType.StepEasingFunction:
                        _stepEasingFunctionCount++;
                        break;
                    case CompositionObjectType.Vector2KeyFrameAnimation:
                        _vector2KeyFrameAnimationCount++;
                        break;
                    case CompositionObjectType.Vector3KeyFrameAnimation:
                        _vector3KeyFrameAnimationCount++;
                        break;
                    default:
                        throw new InvalidOperationException();
                }

            }
        }

        public int CompositionObjectCount => _compositionObjectCount;

        public int CompositionPathCount => _compositionPathCount;

        public int CanvasGeometryCount => _canvasGeometryCount;

        public int AnimationControllerCount => _animationControllerCount;

        public int ColorKeyFrameAnimationCount => _colorKeyFrameAnimationCount;

        public int ColorBrushCount => _colorBrushCount;

        public int ContainerShapeCount => _containerShapeCount;

        public int EllipseGeometryCount => _ellipseGeometryCount;

        public int GeometricClipCount => _geometricClipCount;

        public int PathGeometryCount => _pathGeometryCount;

        public int PropertySetProperyCount => _propertySetPropertyCount;

        public int PropertySetCount => _propertySetCount;

        public int RectangleGeometryCount => _rectangleGeometryCount;

        public int RoundedRectangleGeometryCount => _roundedRectangleGeometryCount;

        public int SpriteShapeCount => _spriteShapeCount;

        public int ViewBoxCount => _viewBoxCount;

        public int ContainerVisualCount => _containerVisualCount;

        public int CubicBezierEasingFunctionCount => _cubicBezierEasingFunctionCount;

        public int ExpressionAnimationCount => _expressionAnimationCount;

        public int InsetClipCount => _insetClipCount;

        public int LinearEasingFunctionCount => _linearEasingFunctionCount;

        public int PathKeyFrameAnimationCount => _pathKeyFrameAnimationCount;

        public int ScalarKeyFrameAnimationCount => _scalarKeyFrameAnimationCount;

        public int ShapeVisualCount => _shapeVisualCount;

        public int StepEasingFunctionCount => _stepEasingFunctionCount;

        public int Vector2KeyFrameAnimationCount => _vector2KeyFrameAnimationCount;

        public int Vector3KeyFrameAnimationCount => _vector3KeyFrameAnimationCount;
        
    }
}
