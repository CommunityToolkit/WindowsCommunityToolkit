// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

#define ReuseExpressionAnimation

using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Wc = Windows.UI.Composition;
using Wd = Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData;

namespace Microsoft.Toolkit.Uwp.UI.Lottie
{
    /// <summary>
    /// Creates instances of a <see cref="Windows.UI.Composition.Visual"/> tree from a description
    /// of the tree.
    /// </summary>
    sealed class Instantiator
    {
        readonly Wc.Compositor _c;
        readonly Dictionary<object, object> _cache = new Dictionary<object, object>(new ReferenceEqualsComparer());
#if ReuseExpressionAnimation
        // The one and only ExpressionAnimation - reset and reparameterized for each time we need one.
        readonly Wc.ExpressionAnimation _expressionAnimation;
#endif

        Instantiator(Wc.Compositor compositor)
        {
            _c = compositor;
#if ReuseExpressionAnimation
            _expressionAnimation = _c.CreateExpressionAnimation();
#endif
        }

        /// <summary>
        /// Creates a new instance of <see cref="Windows.UI.Composition.Visual"/>
        /// described by the given <see cref="WinCompData.Visual"/>.
        /// </summary>
        internal static Wc.Visual CreateVisual(Wc.Compositor compositor, Wd.Visual visual)
        {
            var converter = new Instantiator(compositor);
            var result = converter.GetVisual(visual);
            return result;
        }

        bool GetExisting<T>(object key, out T result)
        {
            if (_cache.TryGetValue(key, out object cached))
            {
                result = (T)cached;
                return true;
            }
            else
            {
                result = default(T);
                return false;
            }
        }

        T CacheAndInitializeCompositionObject<T>(Wd.CompositionObject key, T obj)
            where T : Wc.CompositionObject
        {
            Cache(key, obj);
            InitializeCompositionObject(key, obj);
            return obj;
        }

        T CacheAndInitializeShape<T>(Wd.CompositionShape source, T target)
            where T : Wc.CompositionShape
        {
            CacheAndInitializeCompositionObject(source, target);
            if (source.CenterPoint.HasValue)
            {
                target.CenterPoint = source.CenterPoint.Value;
            }
            if (source.Offset.HasValue)
            {
                target.Offset = source.Offset.Value;
            }
            if (source.RotationAngleInDegrees.HasValue)
            {
                target.RotationAngleInDegrees = source.RotationAngleInDegrees.Value;
            }
            if (source.Scale.HasValue)
            {
                target.Scale = source.Scale.Value;
            }
            if (source.TransformMatrix.HasValue)
            {
                target.TransformMatrix = source.TransformMatrix.Value;
            }
            return target;
        }

        T CacheAndInitializeVisual<T>(Wd.Visual source, T target)
            where T : Wc.Visual
        {
            CacheAndInitializeCompositionObject(source, target);
            if (source.Clip != null)
            {
                target.Clip = GetCompositionClip(source.Clip);
            }
            if (source.CenterPoint.HasValue)
            {
                target.CenterPoint = source.CenterPoint.Value;
            }
            if (source.Offset.HasValue)
            {
                target.Offset = source.Offset.Value;
            }
            if (source.Opacity.HasValue)
            {
                target.Opacity = source.Opacity.Value;
            }
            if (source.RotationAngleInDegrees.HasValue)
            {
                target.RotationAngleInDegrees = source.RotationAngleInDegrees.Value;
            }
            if (source.RotationAxis.HasValue)
            {
                target.RotationAxis = source.RotationAxis.Value;
            }
            if (source.Scale.HasValue)
            {
                target.Scale = source.Scale.Value;
            }
            if (source.Size.HasValue)
            {
                target.Size = source.Size.Value;
            }
            if (source.TransformMatrix.HasValue)
            {
                target.TransformMatrix = source.TransformMatrix.Value;
            }
            return target;
        }

        T CacheAndInitializeAnimation<T>(Wd.CompositionAnimation source, T target)
            where T : Wc.CompositionAnimation
        {
            CacheAndInitializeCompositionObject(source, target);
            foreach (var parameter in source.ReferenceParameters)
            {
                target.SetReferenceParameter(parameter.Key, GetCompositionObject(parameter.Value));
            }
            if (!string.IsNullOrWhiteSpace(source.Target))
            {
                target.Target = source.Target;
            }
            return target;
        }

        T CacheAndInitializeKeyframeAnimation<T>(Wd.KeyFrameAnimation_ source, T target)
            where T : Wc.KeyFrameAnimation
        {
            CacheAndInitializeAnimation(source, target);
            target.Duration = source.Duration;
            return target;
        }

        T CacheAndInitializeCompositionGeometry<T>(Wd.CompositionGeometry source, T target)
            where T : Wc.CompositionGeometry
        {
            CacheAndInitializeCompositionObject(source, target);
            if (source.TrimStart != 0)
            {
                target.TrimStart = source.TrimStart;
            }
            if (source.TrimEnd != 1)
            {
                target.TrimEnd = source.TrimEnd;
            }
            if (source.TrimOffset != 0)
            {
                target.TrimOffset = source.TrimOffset;
            }
            return target;
        }

        T Cache<T>(object key, T obj)
        {
            _cache.Add(key, obj);
            return obj;
        }

        Wc.ShapeVisual GetShapeVisual(Wd.ShapeVisual obj)
        {
            if (GetExisting(obj, out Wc.ShapeVisual result))
            {
                return result;
            }

            result = CacheAndInitializeVisual(obj, _c.CreateShapeVisual());

            if (obj.ViewBox != null)
            {
                result.ViewBox = GetCompositionViewBox(obj.ViewBox);
            }

            var shapesCollection = result.Shapes;
            foreach (var child in obj.Shapes)
            {
                shapesCollection.Add(GetCompositionShape(child));
            }

            InitializeContainerVisual(obj, result);
            StartAnimations(obj, result);
            return result;
        }

        Wc.ContainerVisual GetContainerVisual(Wd.ContainerVisual obj)
        {
            if (GetExisting(obj, out Wc.ContainerVisual result))
            {
                return result;
            }

            result = CacheAndInitializeVisual(obj, _c.CreateContainerVisual());
            InitializeContainerVisual(obj, result);
            StartAnimations(obj, result);
            return result;
        }

        void InitializeContainerVisual(Wd.ContainerVisual source, Wc.ContainerVisual target)
        {
            var children = target.Children;
            foreach (var child in source.Children)
            {
                children.InsertAtTop(GetVisual(child));
            }
        }


        void InitializeCompositionObject(Wd.CompositionObject source, Wc.CompositionObject target)
        {
            // Get the CompositionPropertySet on this object. This has the side-effect of initializing
            // it and starting any animations.
            // Prevent infinite recursion - the Properties on a CompositionPropertySet is itself.
            if (source.Type != Wd.CompositionObjectType.CompositionPropertySet)
            {
                GetCompositionPropertySet(source.Properties);
            }

            if (source.Comment != null)
            {
                target.Comment = source.Comment;
            }
        }

        void StartAnimations(Wd.CompositionObject source, Wc.CompositionObject target)
        {
            foreach (var animator in source.Animators)
            {
                var animation = GetCompositionAnimation(animator.Animation);
                target.StartAnimation(animator.AnimatedProperty, animation);
                var controller = animator.Controller;
                if (controller != null)
                {
                    var animationController = GetAnimationController(controller);
                    if (controller.IsPaused)
                    {
                        animationController.Pause();
                    }
                }
            }
        }



        Wc.AnimationController GetAnimationController(Wd.AnimationController obj)
        {
            if (GetExisting(obj, out Wc.AnimationController result))
            {
                return result;
            }
            var targetObject = GetCompositionObject(obj.TargetObject);

            result = CacheAndInitializeCompositionObject(obj, targetObject.TryGetAnimationController(obj.TargetProperty));
            StartAnimations(obj, result);
            return result;
        }

        Wc.CompositionObject GetCompositionObject(Wd.CompositionObject obj)
        {
            switch (obj.Type)
            {
                case Wd.CompositionObjectType.AnimationController:
                    return GetAnimationController((Wd.AnimationController)obj);
                case Wd.CompositionObjectType.ColorKeyFrameAnimation:
                    return GetColorKeyFrameAnimation((Wd.ColorKeyFrameAnimation)obj);
                case Wd.CompositionObjectType.CompositionColorBrush:
                    return GetCompositionColorBrush((Wd.CompositionColorBrush)obj);
                case Wd.CompositionObjectType.CompositionContainerShape:
                    return GetCompositionContainerShape((Wd.CompositionContainerShape)obj);
                case Wd.CompositionObjectType.CompositionEllipseGeometry:
                    return GetCompositionEllipseGeometry((Wd.CompositionEllipseGeometry)obj);
                case Wd.CompositionObjectType.CompositionPathGeometry:
                    return GetCompositionPathGeometry((Wd.CompositionPathGeometry)obj);
                case Wd.CompositionObjectType.CompositionPropertySet:
                    return GetCompositionPropertySet((Wd.CompositionPropertySet)obj);
                case Wd.CompositionObjectType.CompositionRectangleGeometry:
                    return GetCompositionRectangleGeometry((Wd.CompositionRectangleGeometry)obj);
                case Wd.CompositionObjectType.CompositionRoundedRectangleGeometry:
                    return GetCompositionRoundedRectangleGeometry((Wd.CompositionRoundedRectangleGeometry)obj);
                case Wd.CompositionObjectType.CompositionSpriteShape:
                    return GetCompositionSpriteShape((Wd.CompositionSpriteShape)obj);
                case Wd.CompositionObjectType.CompositionViewBox:
                    return GetCompositionViewBox((Wd.CompositionViewBox)obj);
                case Wd.CompositionObjectType.ContainerVisual:
                    return GetContainerVisual((Wd.ContainerVisual)obj);
                case Wd.CompositionObjectType.CubicBezierEasingFunction:
                    return GetCubicBezierEasingFunction((Wd.CubicBezierEasingFunction)obj);
                case Wd.CompositionObjectType.ExpressionAnimation:
                    return GetExpressionAnimation((Wd.ExpressionAnimation)obj);
                case Wd.CompositionObjectType.InsetClip:
                    return GetInsetClip((Wd.InsetClip)obj);
                case Wd.CompositionObjectType.CompositionGeometricClip:
                    return GetCompositionGeometricClip((Wd.CompositionGeometricClip)obj);
                case Wd.CompositionObjectType.LinearEasingFunction:
                    return GetLinearEasingFunction((Wd.LinearEasingFunction)obj);
                case Wd.CompositionObjectType.PathKeyFrameAnimation:
                    return GetPathKeyFrameAnimation((Wd.PathKeyFrameAnimation)obj);
                case Wd.CompositionObjectType.ScalarKeyFrameAnimation:
                    return GetScalarKeyFrameAnimation((Wd.ScalarKeyFrameAnimation)obj);
                case Wd.CompositionObjectType.ShapeVisual:
                    return GetShapeVisual((Wd.ShapeVisual)obj);
                case Wd.CompositionObjectType.StepEasingFunction:
                    return GetStepEasingFunction((Wd.StepEasingFunction)obj);
                case Wd.CompositionObjectType.Vector2KeyFrameAnimation:
                    return GetVector2KeyFrameAnimation((Wd.Vector2KeyFrameAnimation)obj);
                case Wd.CompositionObjectType.Vector3KeyFrameAnimation:
                    return GetVector3KeyFrameAnimation((Wd.Vector3KeyFrameAnimation)obj);
                default:
                    throw new InvalidOperationException();
            }
        }

        Wc.CompositionPropertySet GetCompositionPropertySet(Wd.CompositionPropertySet obj)
        {
            if (GetExisting(obj, out Wc.CompositionPropertySet result))
            {
                return result;
            }

            // CompositionPropertySets are usually created implicitly by CompositionObjects that own them.
            // If the CompositionPropertySet is not owned, then create it now.
            if (obj.Owner == null)
            {
                result = _c.CreatePropertySet();
            }
            else
            {
                result = GetCompositionObject(obj.Owner).Properties;
            }

            result = CacheAndInitializeCompositionObject(obj, result);

            foreach (var prop in obj.ScalarProperties)
            {
                result.InsertScalar(prop.Key, prop.Value);
            }

            foreach (var prop in obj.Vector2Properties)
            {
                result.InsertVector2(prop.Key, prop.Value);
            }

            StartAnimations(obj, result);
            return result;
        }

        Wc.Visual GetVisual(Wd.Visual obj)
        {
            switch (obj.Type)
            {
                case Wd.CompositionObjectType.ContainerVisual:
                    return GetContainerVisual((Wd.ContainerVisual)obj);
                case Wd.CompositionObjectType.ShapeVisual:
                    return GetShapeVisual((Wd.ShapeVisual)obj);
                default:
                    throw new InvalidOperationException();
            }
        }

        Wc.CompositionAnimation GetCompositionAnimation(Wd.CompositionAnimation obj)
        {
            switch (obj.Type)
            {
                case Wd.CompositionObjectType.ExpressionAnimation:
                    return GetExpressionAnimation((Wd.ExpressionAnimation)obj);
                case Wd.CompositionObjectType.ColorKeyFrameAnimation:
                    return GetColorKeyFrameAnimation((Wd.ColorKeyFrameAnimation)obj);
                case Wd.CompositionObjectType.PathKeyFrameAnimation:
                    return GetPathKeyFrameAnimation((Wd.PathKeyFrameAnimation)obj);
                case Wd.CompositionObjectType.ScalarKeyFrameAnimation:
                    return GetScalarKeyFrameAnimation((Wd.ScalarKeyFrameAnimation)obj);
                case Wd.CompositionObjectType.Vector2KeyFrameAnimation:
                    return GetVector2KeyFrameAnimation((Wd.Vector2KeyFrameAnimation)obj);
                case Wd.CompositionObjectType.Vector3KeyFrameAnimation:
                    return GetVector3KeyFrameAnimation((Wd.Vector3KeyFrameAnimation)obj);
                default:
                    throw new InvalidOperationException();
            }
        }

        Wc.ExpressionAnimation GetExpressionAnimation(Wd.ExpressionAnimation obj)
        {
#if ReuseExpressionAnimation
            // Reset and reuse the same ExpressionAnimation each time.
            var result = _expressionAnimation;
            result.Comment = obj.Comment ?? "";

            // If there is a Target set it. Note however that the Target isn't used for anything
            // interesting in this scenario, and there is no way to reset the Target to an
            // empty string (the Target API disallows empty). In reality, for all our uses
            // the Target will not be set and it doesn't matter if it was set previously.
            if (!string.IsNullOrWhiteSpace(obj.Target))
            {
                result.Target = obj.Target;
            }
            result.Expression = obj.Expression.Simplified.ToString();
            result.ClearAllParameters();
            foreach (var parameter in obj.ReferenceParameters)
            {
                result.SetReferenceParameter(parameter.Key, GetCompositionObject(parameter.Value));
            }
#else
            if (GetExisting(obj, out Wc.ExpressionAnimation result))
            {
                return result;
            }
            result = CacheAndInitializeAnimation(obj, _c.CreateExpressionAnimation(obj.Expression));
#endif
            StartAnimations(obj, result);
            return result;

        }

        Wc.ColorKeyFrameAnimation GetColorKeyFrameAnimation(Wd.ColorKeyFrameAnimation obj)
        {
            if (GetExisting(obj, out Wc.ColorKeyFrameAnimation result))
            {
                return result;
            }

            result = CacheAndInitializeKeyframeAnimation(obj, _c.CreateColorKeyFrameAnimation());
            foreach (var kf in obj.KeyFrames)
            {
                switch (kf.Type)
                {
                    case Wd.KeyFrameAnimation<Wd.Wui.Color>.KeyFrameType.Expression:
                        var expressionKeyFrame = (Wd.KeyFrameAnimation<Wd.Wui.Color>.ExpressionKeyFrame)kf;
                        result.InsertExpressionKeyFrame(kf.Progress, expressionKeyFrame.Expression, GetCompositionEasingFunction(kf.Easing));
                        break;
                    case Wd.KeyFrameAnimation<Wd.Wui.Color>.KeyFrameType.Value:
                        var valueKeyFrame = (Wd.KeyFrameAnimation<Wd.Wui.Color>.ValueKeyFrame)kf;
                        result.InsertKeyFrame(kf.Progress, Color(valueKeyFrame.Value), GetCompositionEasingFunction(kf.Easing));
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            StartAnimations(obj, result);
            return result;
        }

        Wc.ScalarKeyFrameAnimation GetScalarKeyFrameAnimation(Wd.ScalarKeyFrameAnimation obj)
        {
            if (GetExisting(obj, out Wc.ScalarKeyFrameAnimation result))
            {
                return result;
            }

            result = CacheAndInitializeKeyframeAnimation(obj, _c.CreateScalarKeyFrameAnimation());
            foreach (var kf in obj.KeyFrames)
            {
                switch (kf.Type)
                {
                    case Wd.KeyFrameAnimation<float>.KeyFrameType.Expression:
                        var expressionKeyFrame = (Wd.KeyFrameAnimation<float>.ExpressionKeyFrame)kf;
                        result.InsertExpressionKeyFrame(kf.Progress, expressionKeyFrame.Expression, GetCompositionEasingFunction(kf.Easing));
                        break;
                    case Wd.KeyFrameAnimation<float>.KeyFrameType.Value:
                        var valueKeyFrame = (Wd.KeyFrameAnimation<float>.ValueKeyFrame)kf;
                        result.InsertKeyFrame(kf.Progress, valueKeyFrame.Value, GetCompositionEasingFunction(kf.Easing));
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            StartAnimations(obj, result);
            return result;
        }

        Wc.Vector2KeyFrameAnimation GetVector2KeyFrameAnimation(Wd.Vector2KeyFrameAnimation obj)
        {
            if (GetExisting(obj, out Wc.Vector2KeyFrameAnimation result))
            {
                return result;
            }

            result = CacheAndInitializeKeyframeAnimation(obj, _c.CreateVector2KeyFrameAnimation());
            foreach (var kf in obj.KeyFrames)
            {
                switch (kf.Type)
                {
                    case Wd.KeyFrameAnimation<Vector2>.KeyFrameType.Expression:
                        var expressionKeyFrame = (Wd.KeyFrameAnimation<Vector2>.ExpressionKeyFrame)kf;
                        result.InsertExpressionKeyFrame(kf.Progress, expressionKeyFrame.Expression, GetCompositionEasingFunction(kf.Easing));
                        break;
                    case Wd.KeyFrameAnimation<Vector2>.KeyFrameType.Value:
                        var valueKeyFrame = (Wd.KeyFrameAnimation<Vector2>.ValueKeyFrame)kf;
                        result.InsertKeyFrame(kf.Progress, valueKeyFrame.Value, GetCompositionEasingFunction(kf.Easing));
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            StartAnimations(obj, result);
            return result;
        }

        Wc.Vector3KeyFrameAnimation GetVector3KeyFrameAnimation(Wd.Vector3KeyFrameAnimation obj)
        {
            if (GetExisting(obj, out Wc.Vector3KeyFrameAnimation result))
            {
                return result;
            }

            result = CacheAndInitializeKeyframeAnimation(obj, _c.CreateVector3KeyFrameAnimation());
            foreach (var kf in obj.KeyFrames)
            {
                switch (kf.Type)
                {
                    case Wd.KeyFrameAnimation<Vector3>.KeyFrameType.Expression:
                        var expressionKeyFrame = (Wd.KeyFrameAnimation<Vector3>.ExpressionKeyFrame)kf;
                        result.InsertExpressionKeyFrame(kf.Progress, expressionKeyFrame.Expression, GetCompositionEasingFunction(kf.Easing));
                        break;
                    case Wd.KeyFrameAnimation<Vector3>.KeyFrameType.Value:
                        var valueKeyFrame = (Wd.KeyFrameAnimation<Vector3>.ValueKeyFrame)kf;
                        result.InsertKeyFrame(kf.Progress, valueKeyFrame.Value, GetCompositionEasingFunction(kf.Easing));
                        break;
                    default:
                        throw new InvalidCastException();
                }
            }
            StartAnimations(obj, result);
            return result;
        }

        Wc.PathKeyFrameAnimation GetPathKeyFrameAnimation(Wd.PathKeyFrameAnimation obj)
        {
            if (GetExisting(obj, out Wc.PathKeyFrameAnimation result))
            {
                return result;
            }

            result = CacheAndInitializeKeyframeAnimation(obj, _c.CreatePathKeyFrameAnimation());
            foreach (var kf in obj.KeyFrames)
            {
                result.InsertKeyFrame(kf.Progress, GetCompositionPath(((Wd.PathKeyFrameAnimation.ValueKeyFrame)kf).Value), GetCompositionEasingFunction(kf.Easing));
            }
            StartAnimations(obj, result);
            return result;
        }

        Wc.CompositionEasingFunction GetCompositionEasingFunction(Wd.CompositionEasingFunction obj)
        {
            switch (obj.Type)
            {
                case Wd.CompositionObjectType.LinearEasingFunction:
                    return GetLinearEasingFunction((Wd.LinearEasingFunction)obj);
                case Wd.CompositionObjectType.StepEasingFunction:
                    return GetStepEasingFunction((Wd.StepEasingFunction)obj);
                case Wd.CompositionObjectType.CubicBezierEasingFunction:
                    return GetCubicBezierEasingFunction((Wd.CubicBezierEasingFunction)obj);
                default:
                    throw new InvalidOperationException();
            }
        }

        Wc.CompositionClip GetCompositionClip(Wd.CompositionClip obj)
        {
            switch (obj.Type)
            {
                case Wd.CompositionObjectType.InsetClip:
                    return GetInsetClip((Wd.InsetClip)obj);
                case Wd.CompositionObjectType.CompositionGeometricClip:
                    return GetCompositionGeometricClip((Wd.CompositionGeometricClip)obj);
                default:
                    throw new InvalidOperationException();
            }
        }

        Wc.InsetClip GetInsetClip(Wd.InsetClip obj)
        {
            if (GetExisting(obj, out Wc.InsetClip result))
            {
                return result;
            }

            result = CacheAndInitializeCompositionObject(obj, _c.CreateInsetClip());
            // CompositionClip properties
            if (obj.CenterPoint.X != 0 || obj.CenterPoint.Y != 0)
            {
                result.CenterPoint = obj.CenterPoint;
            }
            if (obj.Scale.X != 1 || obj.Scale.Y != 1)
            {
                result.Scale = obj.Scale;
            }
            // InsetClip properties
            if (obj.LeftInset != 0)
            {
                result.LeftInset = obj.LeftInset;
            }
            if (obj.RightInset != 0)
            {
                result.RightInset = obj.RightInset;
            }
            if (obj.TopInset != 0)
            {
                result.TopInset = obj.TopInset;
            }
            if (obj.BottomInset != 0)
            {
                result.BottomInset = obj.BottomInset;
            }
            StartAnimations(obj, result);
            return result;
        }

        Wc.CompositionGeometricClip GetCompositionGeometricClip(Wd.CompositionGeometricClip obj)
        {
            if (GetExisting(obj, out Wc.CompositionGeometricClip result))
            {
                return result;
            }

            result = CacheAndInitializeCompositionObject(obj, _c.CreateGeometricClip(GetCompositionGeometry(obj.Geometry)));
            StartAnimations(obj, result);
            return result;
        }

        Wc.LinearEasingFunction GetLinearEasingFunction(Wd.LinearEasingFunction obj)
        {
            if (GetExisting(obj, out Wc.LinearEasingFunction result))
            {
                return result;
            }

            result = CacheAndInitializeCompositionObject(obj, _c.CreateLinearEasingFunction());
            StartAnimations(obj, result);
            return result;
        }

        Wc.StepEasingFunction GetStepEasingFunction(Wd.StepEasingFunction obj)
        {
            if (GetExisting(obj, out Wc.StepEasingFunction result))
            {
                return result;
            }

            result = CacheAndInitializeCompositionObject(obj, _c.CreateStepEasingFunction());
            if (obj.FinalStep != 1)
            {
                result.FinalStep = obj.FinalStep;
            }
            if (obj.InitialStep != 0)
            {
                result.InitialStep = obj.InitialStep;
            }
            if (obj.IsFinalStepSingleFrame)
            {
                result.IsFinalStepSingleFrame = obj.IsFinalStepSingleFrame;
            }
            if (obj.IsInitialStepSingleFrame)
            {
                result.IsInitialStepSingleFrame = obj.IsInitialStepSingleFrame;
            }
            if (obj.StepCount != 1)
            {
                result.StepCount = obj.StepCount;
            }
            StartAnimations(obj, result);
            return result;
        }

        Wc.CubicBezierEasingFunction GetCubicBezierEasingFunction(Wd.CubicBezierEasingFunction obj)
        {
            if (GetExisting(obj, out Wc.CubicBezierEasingFunction result))
            {
                return result;
            }

            result = CacheAndInitializeCompositionObject(obj, _c.CreateCubicBezierEasingFunction(obj.ControlPoint1, obj.ControlPoint2));
            StartAnimations(obj, result);
            return result;
        }
        Wc.CompositionViewBox GetCompositionViewBox(Wd.CompositionViewBox obj)
        {
            if (GetExisting(obj, out Wc.CompositionViewBox result))
            {
                return result;
            }

            result = CacheAndInitializeCompositionObject(obj, _c.CreateViewBox());
            result.Size = obj.Size;
            StartAnimations(obj, result);
            return result;
        }

        Wc.CompositionShape GetCompositionShape(Wd.CompositionShape obj)
        {
            switch (obj.Type)
            {
                case Wd.CompositionObjectType.CompositionSpriteShape:
                    return GetCompositionSpriteShape((Wd.CompositionSpriteShape)obj);
                case Wd.CompositionObjectType.CompositionContainerShape:
                    return GetCompositionContainerShape((Wd.CompositionContainerShape)obj);
                default:
                    throw new InvalidOperationException();
            }
        }

        Wc.CompositionContainerShape GetCompositionContainerShape(Wd.CompositionContainerShape obj)
        {
            if (GetExisting(obj, out Wc.CompositionContainerShape result))
            {
                return result;
            }

            // If this container has only 1 child, it might be coalescable with its child.
            if (obj.Shapes.Count == 1)
            {
                var child = obj.Shapes[0];
                if (!obj.Animators.Any())
                {
                    // The container has no animations. It can be replaced with its child as
                    // long as the child doesn't animate any of the non-default properties and
                    // the container isn't referenced by an animation.

                }
                else if (!child.Animators.Any() && child.Type == Wd.CompositionObjectType.CompositionContainerShape)
                {
                    // The child has no animations. It can be replaced with its parent as long
                    // as the parent doesn't animate any of the child's non-default properties
                    // and the child isn't referenced by an animation.
                }
            }

            result = CacheAndInitializeShape(obj, _c.CreateContainerShape());
            var shapeCollection = result.Shapes;
            foreach (var child in obj.Shapes)
            {
                shapeCollection.Add(GetCompositionShape(child));
            }
            StartAnimations(obj, result);
            return result;
        }

        Wc.CompositionSpriteShape GetCompositionSpriteShape(Wd.CompositionSpriteShape obj)
        {
            if (GetExisting(obj, out Wc.CompositionSpriteShape result))
            {
                return result;
            }

            result = CacheAndInitializeShape(obj, _c.CreateSpriteShape());

            if (obj.StrokeBrush != null)
            {
                result.StrokeBrush = GetCompositionBrush(obj.StrokeBrush);
                if (obj.StrokeDashCap != Wd.CompositionStrokeCap.Flat)
                {
                    result.StrokeDashCap = StrokeCap(obj.StrokeDashCap);
                }
                if (obj.StrokeStartCap != Wd.CompositionStrokeCap.Flat)
                {
                    result.StrokeStartCap = StrokeCap(obj.StrokeStartCap);
                }
                if (obj.StrokeEndCap != Wd.CompositionStrokeCap.Flat)
                {
                    result.StrokeEndCap = StrokeCap(obj.StrokeEndCap);
                }
                if (obj.StrokeThickness != 1)
                {
                    result.StrokeThickness = obj.StrokeThickness;
                }
                if (obj.StrokeMiterLimit != 1)
                {
                    result.StrokeMiterLimit = obj.StrokeMiterLimit;
                }
                if (obj.StrokeLineJoin != Wd.CompositionStrokeLineJoin.Miter)
                {
                    result.StrokeLineJoin = StrokeLineJoin(obj.StrokeLineJoin);
                }
                if (obj.StrokeDashOffset != 0)
                {
                    result.StrokeDashOffset = obj.StrokeDashOffset;
                }
                if (obj.IsStrokeNonScaling)
                {
                    result.IsStrokeNonScaling = obj.IsStrokeNonScaling;
                }
                var strokeDashArray = result.StrokeDashArray;
                foreach (var strokeDash in obj.StrokeDashArray)
                {
                    strokeDashArray.Add(strokeDash);
                }
            }
            result.Geometry = GetCompositionGeometry(obj.Geometry);
            if (obj.FillBrush != null)
            {
                result.FillBrush = GetCompositionBrush(obj.FillBrush);
            }
            StartAnimations(obj, result);
            return result;
        }

        Wc.CompositionGeometry GetCompositionGeometry(Wd.CompositionGeometry obj)
        {
            switch (obj.Type)
            {
                case Wd.CompositionObjectType.CompositionPathGeometry:
                    return GetCompositionPathGeometry((Wd.CompositionPathGeometry)obj);

                case Wd.CompositionObjectType.CompositionEllipseGeometry:
                    return GetCompositionEllipseGeometry((Wd.CompositionEllipseGeometry)obj);

                case Wd.CompositionObjectType.CompositionRectangleGeometry:
                    return GetCompositionRectangleGeometry((Wd.CompositionRectangleGeometry)obj);

                case Wd.CompositionObjectType.CompositionRoundedRectangleGeometry:
                    return GetCompositionRoundedRectangleGeometry((Wd.CompositionRoundedRectangleGeometry)obj);

                default:
                    throw new InvalidOperationException();
            }
        }

        Wc.CompositionEllipseGeometry GetCompositionEllipseGeometry(Wd.CompositionEllipseGeometry obj)
        {
            if (GetExisting(obj, out Wc.CompositionEllipseGeometry result))
            {
                return result;
            }
            result = CacheAndInitializeCompositionGeometry(obj, _c.CreateEllipseGeometry());
            if (obj.Center.X != 0 || obj.Center.Y != 0)
            {
                result.Center = obj.Center;
            }
            result.Radius = obj.Radius;
            StartAnimations(obj, result);
            return result;
        }

        Wc.CompositionRectangleGeometry GetCompositionRectangleGeometry(Wd.CompositionRectangleGeometry obj)
        {
            if (GetExisting(obj, out Wc.CompositionRectangleGeometry result))
            {
                return result;
            }
            result = CacheAndInitializeCompositionGeometry(obj, _c.CreateRectangleGeometry());
            if (obj.Offset.HasValue)
            {
                result.Offset = obj.Offset.Value;
            }
            result.Size = obj.Size;
            StartAnimations(obj, result);
            return result;
        }

        Wc.CompositionRoundedRectangleGeometry GetCompositionRoundedRectangleGeometry(Wd.CompositionRoundedRectangleGeometry obj)
        {
            if (GetExisting(obj, out Wc.CompositionRoundedRectangleGeometry result))
            {
                return result;
            }
            result = CacheAndInitializeCompositionGeometry(obj, _c.CreateRoundedRectangleGeometry());
            if (obj.Offset.HasValue)
            {
                result.Offset = obj.Offset.Value;
            }
            result.Size = obj.Size;
            result.CornerRadius = obj.CornerRadius;
            StartAnimations(obj, result);
            return result;
        }

        Wc.CompositionPathGeometry GetCompositionPathGeometry(Wd.CompositionPathGeometry obj)
        {
            if (GetExisting(obj, out Wc.CompositionPathGeometry result))
            {
                return result;
            }
            result = CacheAndInitializeCompositionGeometry(obj, _c.CreatePathGeometry(GetCompositionPath(obj.Path)));
            StartAnimations(obj, result);
            return result;
        }

        Wc.CompositionPath GetCompositionPath(Wd.CompositionPath obj)
        {
            if (GetExisting(obj, out Wc.CompositionPath result))
            {
                return result;
            }

            result = Cache(obj, new Wc.CompositionPath(GetCanvasGeometry(obj.Source)));
            return result;
        }

        CanvasGeometry GetCanvasGeometry(Wd.Wg.IGeometrySource2D obj)
        {
            if (GetExisting(obj, out CanvasGeometry result))
            {
                return result;
            }

            var canvasGeometry = (Wd.Mgcg.CanvasGeometry)obj;
            switch (canvasGeometry.Type)
            {
                case Wd.Mgcg.CanvasGeometry.GeometryType.Combination:
                    {
                        var combination = (Wd.Mgcg.CanvasGeometry.Combination)canvasGeometry;
                        return Cache(obj, GetCanvasGeometry(combination.A).CombineWith(
                            GetCanvasGeometry(combination.B),
                            combination.Matrix,
                            Combine(combination.CombineMode)));
                    }
                case Wd.Mgcg.CanvasGeometry.GeometryType.Ellipse:
                    var ellipse = (Wd.Mgcg.CanvasGeometry.Ellipse)canvasGeometry;
                    return CanvasGeometry.CreateEllipse(
                        null,
                        ellipse.X,
                        ellipse.Y,
                        ellipse.RadiusX,
                        ellipse.RadiusY);
                case Wd.Mgcg.CanvasGeometry.GeometryType.Path:
                    using (var builder = new CanvasPathBuilder(null))
                    {
                        var path = (Wd.Mgcg.CanvasGeometry.Path)canvasGeometry;

                        if (path.FilledRegionDetermination != Wd.Mgcg.CanvasFilledRegionDetermination.Alternate)
                        {
                            builder.SetFilledRegionDetermination(FilledRegionDetermination(path.FilledRegionDetermination));
                        }

                        foreach (var command in path.Commands)
                        {
                            switch (command.Type)
                            {
                                case Wd.Mgcg.CanvasPathBuilder.CommandType.BeginFigure:
                                    builder.BeginFigure(((Wd.Mgcg.CanvasPathBuilder.Command.BeginFigure)command).StartPoint);
                                    break;
                                case Wd.Mgcg.CanvasPathBuilder.CommandType.EndFigure:
                                    builder.EndFigure(CanvasFigureLoop(((Wd.Mgcg.CanvasPathBuilder.Command.EndFigure)command).FigureLoop));
                                    break;
                                case Wd.Mgcg.CanvasPathBuilder.CommandType.AddLine:
                                    builder.AddLine(((Wd.Mgcg.CanvasPathBuilder.Command.AddLine)command).EndPoint);
                                    break;
                                case Wd.Mgcg.CanvasPathBuilder.CommandType.AddCubicBezier:
                                    var cb = (Wd.Mgcg.CanvasPathBuilder.Command.AddCubicBezier)command;
                                    builder.AddCubicBezier(cb.ControlPoint1, cb.ControlPoint2, cb.EndPoint);
                                    break;
                                default:
                                    throw new InvalidOperationException();
                            }
                        }
                        return Cache(obj, CanvasGeometry.CreatePath(builder));
                    }
                case Wd.Mgcg.CanvasGeometry.GeometryType.RoundedRectangle:
                    var roundedRectangle = (Wd.Mgcg.CanvasGeometry.RoundedRectangle)canvasGeometry;
                    return CanvasGeometry.CreateRoundedRectangle(
                        null,
                        roundedRectangle.X,
                        roundedRectangle.Y,
                        roundedRectangle.W,
                        roundedRectangle.H,
                        roundedRectangle.RadiusX,
                        roundedRectangle.RadiusY);
                case Wd.Mgcg.CanvasGeometry.GeometryType.TransformedGeometry:
                    var transformedGeometry = (Wd.Mgcg.CanvasGeometry.TransformedGeometry)canvasGeometry;
                    return GetCanvasGeometry(transformedGeometry.SourceGeometry).Transform(transformedGeometry.TransformMatrix);
                default:
                    throw new InvalidOperationException();
            }
        }

        Wc.CompositionBrush GetCompositionBrush(Wd.CompositionBrush obj)
        {
            return GetCompositionColorBrush((Wd.CompositionColorBrush)obj);
        }

        Wc.CompositionColorBrush GetCompositionColorBrush(Wd.CompositionColorBrush obj)
        {
            if (GetExisting(obj, out Wc.CompositionColorBrush result))
            {
                return result;
            }
            result = CacheAndInitializeCompositionObject(obj, _c.CreateColorBrush(Color(obj.Color)));
            StartAnimations(obj, result);
            return result;
        }

        static Wc.CompositionStrokeLineJoin StrokeLineJoin(Wd.CompositionStrokeLineJoin value)
        {
            switch (value)
            {
                case Wd.CompositionStrokeLineJoin.Miter:
                    return Wc.CompositionStrokeLineJoin.Miter;
                case Wd.CompositionStrokeLineJoin.Bevel:
                    return Wc.CompositionStrokeLineJoin.Bevel;
                case Wd.CompositionStrokeLineJoin.Round:
                    return Wc.CompositionStrokeLineJoin.Round;
                case Wd.CompositionStrokeLineJoin.MiterOrBevel:
                    return Wc.CompositionStrokeLineJoin.MiterOrBevel;
                default:
                    throw new InvalidOperationException();
            }
        }

        static Wc.CompositionStrokeCap StrokeCap(Wd.CompositionStrokeCap value)
        {
            switch (value)
            {
                case Wd.CompositionStrokeCap.Flat:
                    return Wc.CompositionStrokeCap.Flat;
                case Wd.CompositionStrokeCap.Square:
                    return Wc.CompositionStrokeCap.Square;
                case Wd.CompositionStrokeCap.Round:
                    return Wc.CompositionStrokeCap.Round;
                case Wd.CompositionStrokeCap.Triangle:
                    return Wc.CompositionStrokeCap.Triangle;
                default:
                    throw new InvalidOperationException();
            }
        }

        static Windows.UI.Color Color(Wd.Wui.Color color) =>
            Windows.UI.Color.FromArgb(color.A, color.R, color.G, color.B);

        static CanvasFilledRegionDetermination FilledRegionDetermination(
            Wd.Mgcg.CanvasFilledRegionDetermination value)
        {
            switch (value)
            {
                case Wd.Mgcg.CanvasFilledRegionDetermination.Alternate:
                    return CanvasFilledRegionDetermination.Alternate;
                case Wd.Mgcg.CanvasFilledRegionDetermination.Winding:
                    return CanvasFilledRegionDetermination.Winding;
                default:
                    throw new InvalidOperationException();
            }
        }

        static CanvasFigureLoop CanvasFigureLoop(Wd.Mgcg.CanvasFigureLoop value)
        {
            switch (value)
            {
                case Wd.Mgcg.CanvasFigureLoop.Open:
                    return Microsoft.Graphics.Canvas.Geometry.CanvasFigureLoop.Open;
                case Wd.Mgcg.CanvasFigureLoop.Closed:
                    return Microsoft.Graphics.Canvas.Geometry.CanvasFigureLoop.Closed;
                default:
                    throw new InvalidOperationException();
            }
        }

        static CanvasGeometryCombine Combine(Wd.Mgcg.CanvasGeometryCombine value)
        {
            switch (value)
            {
                case Wd.Mgcg.CanvasGeometryCombine.Union:
                    return CanvasGeometryCombine.Union;
                case Wd.Mgcg.CanvasGeometryCombine.Exclude:
                    return CanvasGeometryCombine.Exclude;
                case Wd.Mgcg.CanvasGeometryCombine.Intersect:
                    return CanvasGeometryCombine.Intersect;
                case Wd.Mgcg.CanvasGeometryCombine.Xor:
                    return CanvasGeometryCombine.Xor;
                default:
                    throw new InvalidOperationException();
            }
        }

        sealed class ReferenceEqualsComparer : IEqualityComparer<object>
        {
            bool IEqualityComparer<object>.Equals(object x, object y) => ReferenceEquals(x, y);
            int IEqualityComparer<object>.GetHashCode(object obj) => obj.GetHashCode();
        }
    }
}
