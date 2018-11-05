using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Mgcg;
using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.CodeGen
{
    /// <summary>
    /// Transforms a WinCompData tree to an equivalent tree, optimizing the tree
    /// where possible.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class Optimizer
    {
        readonly Compositor _c = new Compositor();
        readonly ObjectGraph<ObjectData> _graph;

        public static Visual Optimize(Visual root, bool ignoreCommentProperties)
        {
            // Build the object graph.
            var graph = ObjectGraph<ObjectData>.FromCompositionObject(root, includeVertices: true);

            // Find the canonical objects in the graph.
            Canonicalizer.Canonicalize(graph, ignoreCommentProperties: ignoreCommentProperties);

            // Create a copy of the WinCompData objects from the canonical objects.
            // The copy is needed so that we can modify the tree without affecting the graph that
            // was given to us.
            var result = (Visual)new Optimizer(graph).GetCompositionObject(root);

            AssertGraphsAreDisjoint(result, root);

            // Try to optimize away redundant containers.
            result = TreeReducer.OptimizeContainers(result);

            return result;
        }

        // Asserts that the 2 graphs are disjoint. If this assert ever fires then
        // the graph copier is broken - it is reusing an object rather than making
        // a copy.
        [Conditional("DEBUG")]
        static void AssertGraphsAreDisjoint(CompositionObject root1, CompositionObject root2)
        {
            var graph1 = Graph.FromCompositionObject(root1, includeVertices: false);
            var graph2 = Graph.FromCompositionObject(root2, includeVertices: false);

            var graph1Objects = new HashSet<object>(graph1.Nodes.Select(n => n.Object));
            foreach (var obj in graph2.Nodes.Select(n => n.Object))
            {
                Debug.Assert(!graph1Objects.Contains(obj));
            }
        }

        sealed class ObjectData : CanonicalizedNode<ObjectData>
        {
            // The copied object.
            internal object Copied { get; set; }
        }

        Optimizer(ObjectGraph<ObjectData> graph)
        {
            _graph = graph;
        }

        ObjectData NodeFor(CanvasGeometry obj)
        {
            return _graph[obj].Canonical;
        }

        ObjectData NodeFor(CompositionObject obj)
        {
            return _graph[obj].Canonical;
        }

        ObjectData NodeFor(CompositionPath obj)
        {
            return _graph[obj].Canonical;
        }

        bool GetExisting<T>(T key, out T result) where T : CompositionObject
        {
            result = (T)NodeFor(key).Copied;
            return result != null;
        }

        bool GetExistingCanvasGeometry(CanvasGeometry key, out CanvasGeometry result)
        {
            result = (CanvasGeometry)NodeFor(key).Copied;
            return result != null;
        }

        bool GetExisting(CompositionPath key, out CompositionPath result)
        {
            result = (CompositionPath)NodeFor(key).Copied;
            return result != null;
        }

        T CacheAndInitializeCompositionObject<T>(T key, T obj)
            where T : CompositionObject
        {
            Cache(key, obj);
            InitializeCompositionObject(key, obj);
            return obj;
        }

        T CacheAndInitializeShape<T>(T source, T target)
            where T : CompositionShape
        {
            CacheAndInitializeCompositionObject(source, target);
            if (source.CenterPoint.HasValue)
            {
                target.CenterPoint = source.CenterPoint;
            }
            if (source.Offset.HasValue)
            {
                target.Offset = source.Offset;
            }
            if (source.RotationAngleInDegrees.HasValue)
            {
                target.RotationAngleInDegrees = source.RotationAngleInDegrees.Value;
            }
            if (source.Scale.HasValue)
            {
                target.Scale = source.Scale;
            }
            if (source.TransformMatrix.HasValue)
            {
                target.TransformMatrix = source.TransformMatrix.Value;
            }
            return target;
        }

        T CacheAndInitializeVisual<T>(Visual source, T target)
            where T : Visual
        {
            CacheAndInitializeCompositionObject(source, target);
            if (source.Clip != null)
            {
                target.Clip = GetCompositionClip(source.Clip);
            }
            if (source.CenterPoint.HasValue)
            {
                target.CenterPoint = source.CenterPoint;
            }
            if (source.Offset.HasValue)
            {
                target.Offset = source.Offset;
            }
            if (source.Opacity.HasValue)
            {
                target.Opacity = source.Opacity.Value;
            }
            if (source.RotationAngleInDegrees.HasValue)
            {
                target.RotationAngleInDegrees = source.RotationAngleInDegrees.Value;
            }
            if (source.Scale.HasValue)
            {
                target.Scale = source.Scale;
            }
            if (source.Size.HasValue)
            {
                target.Size = source.Size;
            }
            return target;
        }

        T CacheAndInitializeAnimation<T>(T source, T target)
            where T : CompositionAnimation
        {
            CacheAndInitializeCompositionObject(source, target);
            foreach (var parameter in source.ReferenceParameters)
            {
                var referenceObject = GetCompositionObject(parameter.Value);
                target.SetReferenceParameter(parameter.Key, referenceObject);
            }
            if (!string.IsNullOrWhiteSpace(source.Target))
            {
                target.Target = source.Target;
            }
            return target;
        }

        T CacheAndInitializeKeyframeAnimation<T>(KeyFrameAnimation_ source, T target)
            where T : KeyFrameAnimation_
        {
            CacheAndInitializeAnimation(source, target);
            target.Duration = source.Duration;
            return target;
        }

        T CacheAndInitializeCompositionGeometry<T>(CompositionGeometry source, T target)
            where T : CompositionGeometry
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

        CanvasGeometry CacheCanvasGeometry(CanvasGeometry key, CanvasGeometry obj)
        {
            var node = NodeFor(key);
            Debug.Assert(node.Copied == null);
            Debug.Assert(!ReferenceEquals(key, obj));
            node.Copied = obj;
            return obj;
        }

        T Cache<T>(T key, T obj) where T : CompositionObject
        {
            var node = NodeFor(key);
            Debug.Assert(node.Copied == null);
            Debug.Assert(!ReferenceEquals(key, obj));
            node.Copied = obj;
            return obj;
        }

        CompositionPath Cache(CompositionPath key, CompositionPath obj)
        {
            var node = NodeFor(key);
            Debug.Assert(node.Copied == null);
            Debug.Assert(!ReferenceEquals(key, obj));
            node.Copied = obj;
            return obj;
        }


        ShapeVisual GetShapeVisual(ShapeVisual obj)
        {
            if (GetExisting(obj, out ShapeVisual result))
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
            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        ContainerVisual GetContainerVisual(ContainerVisual obj)
        {
            if (GetExisting(obj, out ContainerVisual result))
            {
                return result;
            }

            result = CacheAndInitializeVisual(obj, _c.CreateContainerVisual());
            InitializeContainerVisual(obj, result);
            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        void InitializeContainerVisual<T>(T source, T target)
            where T : ContainerVisual
        {
            var children = target.Children;
            foreach (var child in source.Children)
            {
                children.Add(GetVisual(child));
            }
        }

        void InitializeIDescribable<T>(T source, T target)
            where T : IDescribable
        {
            target.LongDescription = source.LongDescription;
            target.ShortDescription = source.ShortDescription;
        }

        void InitializeCompositionObject<T>(T source, T target)
            where T : CompositionObject
        {
            // Get the CompositionPropertySet on this object. This has the side-effect of initializing
            // it and starting any animations.
            // Prevent infinite recursion - the Properties on a CompositionPropertySet is itself.
            if (source.Type != CompositionObjectType.CompositionPropertySet)
            {
                GetCompositionPropertySet(source.Properties);
            }

            target.Comment = source.Comment;
            InitializeIDescribable(source, target);
        }

        void StartAnimationsAndFreeze(CompositionObject source, CompositionObject target)
        {
            foreach (var animator in source.Animators)
            {
                var animation = GetCompositionAnimation(animator.Animation);
                // Freeze the animation to indicate that it will not be mutated further. This
                // will ensure that it does not need to be copied when target.StartAnimation is called.
                animation.Freeze();
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



        AnimationController GetAnimationController(AnimationController obj)
        {
            if (GetExisting(obj, out AnimationController result))
            {
                return result;
            }
            var targetObject = GetCompositionObject(obj.TargetObject);

            result = CacheAndInitializeCompositionObject(obj, targetObject.TryGetAnimationController(obj.TargetProperty));
            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        /// <summary>
        /// Returns a copy of the graph of composition objects starting at the given object.
        /// </summary>
        CompositionObject GetCompositionObject(CompositionObject obj)
        {
            switch (obj.Type)
            {
                case CompositionObjectType.AnimationController:
                    return GetAnimationController((AnimationController)obj);
                case CompositionObjectType.ColorKeyFrameAnimation:
                    return GetColorKeyFrameAnimation((ColorKeyFrameAnimation)obj);
                case CompositionObjectType.CompositionColorBrush:
                    return GetCompositionColorBrush((CompositionColorBrush)obj);
                case CompositionObjectType.CompositionContainerShape:
                    return GetCompositionContainerShape((CompositionContainerShape)obj);
                case CompositionObjectType.CompositionEllipseGeometry:
                    return GetCompositionEllipseGeometry((CompositionEllipseGeometry)obj);
                case CompositionObjectType.CompositionGeometricClip:
                    return GetCompositionGeometricClip((CompositionGeometricClip)obj);
                case CompositionObjectType.CompositionPathGeometry:
                    return GetCompositionPathGeometry((CompositionPathGeometry)obj);
                case CompositionObjectType.CompositionPropertySet:
                    return GetCompositionPropertySet((CompositionPropertySet)obj);
                case CompositionObjectType.CompositionRectangleGeometry:
                    return GetCompositionRectangleGeometry((CompositionRectangleGeometry)obj);
                case CompositionObjectType.CompositionRoundedRectangleGeometry:
                    return GetCompositionRoundedRectangleGeometry((CompositionRoundedRectangleGeometry)obj);
                case CompositionObjectType.CompositionSpriteShape:
                    return GetCompositionSpriteShape((CompositionSpriteShape)obj);
                case CompositionObjectType.CompositionViewBox:
                    return GetCompositionViewBox((CompositionViewBox)obj);
                case CompositionObjectType.ContainerVisual:
                    return GetContainerVisual((ContainerVisual)obj);
                case CompositionObjectType.CubicBezierEasingFunction:
                    return GetCubicBezierEasingFunction((CubicBezierEasingFunction)obj);
                case CompositionObjectType.ExpressionAnimation:
                    return GetExpressionAnimation((ExpressionAnimation)obj);
                case CompositionObjectType.InsetClip:
                    return GetInsetClip((InsetClip)obj);
                case CompositionObjectType.LinearEasingFunction:
                    return GetLinearEasingFunction((LinearEasingFunction)obj);
                case CompositionObjectType.PathKeyFrameAnimation:
                    return GetPathKeyFrameAnimation((PathKeyFrameAnimation)obj);
                case CompositionObjectType.ScalarKeyFrameAnimation:
                    return GetScalarKeyFrameAnimation((ScalarKeyFrameAnimation)obj);
                case CompositionObjectType.ShapeVisual:
                    return GetShapeVisual((ShapeVisual)obj);
                case CompositionObjectType.StepEasingFunction:
                    return GetStepEasingFunction((StepEasingFunction)obj);
                case CompositionObjectType.Vector2KeyFrameAnimation:
                    return GetVector2KeyFrameAnimation((Vector2KeyFrameAnimation)obj);
                case CompositionObjectType.Vector3KeyFrameAnimation:
                    return GetVector3KeyFrameAnimation((Vector3KeyFrameAnimation)obj);
                default:
                    throw new InvalidOperationException();
            }
        }

        CompositionPropertySet GetCompositionPropertySet(CompositionPropertySet obj)
        {
            if (GetExisting(obj, out CompositionPropertySet result))
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

            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        Visual GetVisual(Visual obj)
        {
            switch (obj.Type)
            {
                case CompositionObjectType.ContainerVisual:
                    return GetContainerVisual((ContainerVisual)obj);
                case CompositionObjectType.ShapeVisual:
                    return GetShapeVisual((ShapeVisual)obj);
                default:
                    throw new InvalidOperationException();
            }
        }

        CompositionAnimation GetCompositionAnimation(CompositionAnimation obj)
        {
            switch (obj.Type)
            {
                case CompositionObjectType.ExpressionAnimation:
                    return GetExpressionAnimation((ExpressionAnimation)obj);
                case CompositionObjectType.ColorKeyFrameAnimation:
                    return GetColorKeyFrameAnimation((ColorKeyFrameAnimation)obj);
                case CompositionObjectType.PathKeyFrameAnimation:
                    return GetPathKeyFrameAnimation((PathKeyFrameAnimation)obj);
                case CompositionObjectType.ScalarKeyFrameAnimation:
                    return GetScalarKeyFrameAnimation((ScalarKeyFrameAnimation)obj);
                case CompositionObjectType.Vector2KeyFrameAnimation:
                    return GetVector2KeyFrameAnimation((Vector2KeyFrameAnimation)obj);
                case CompositionObjectType.Vector3KeyFrameAnimation:
                    return GetVector3KeyFrameAnimation((Vector3KeyFrameAnimation)obj);
                default:
                    throw new InvalidOperationException();
            }
        }

        ExpressionAnimation GetExpressionAnimation(ExpressionAnimation obj)
        {
            if (GetExisting(obj, out var result))
            {
                return result;
            }
            result = CacheAndInitializeAnimation(obj, _c.CreateExpressionAnimation(obj.Expression));
            StartAnimationsAndFreeze(obj, result);
            return result;

        }

        ColorKeyFrameAnimation GetColorKeyFrameAnimation(ColorKeyFrameAnimation obj)
        {
            if (GetExisting(obj, out var result))
            {
                return result;
            }

            result = CacheAndInitializeKeyframeAnimation(obj, _c.CreateColorKeyFrameAnimation());
            foreach (var kf in obj.KeyFrames)
            {
                switch (kf.Type)
                {
                    case KeyFrameAnimation<Wui.Color>.KeyFrameType.Expression:
                        var expressionKeyFrame = (KeyFrameAnimation<Wui.Color>.ExpressionKeyFrame)kf;
                        result.InsertExpressionKeyFrame(kf.Progress, expressionKeyFrame.Expression, GetCompositionEasingFunction(kf.Easing));
                        break;
                    case KeyFrameAnimation<Wui.Color>.KeyFrameType.Value:
                        var valueKeyFrame = (KeyFrameAnimation<Wui.Color>.ValueKeyFrame)kf;
                        result.InsertKeyFrame(kf.Progress, valueKeyFrame.Value, GetCompositionEasingFunction(kf.Easing));
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        ScalarKeyFrameAnimation GetScalarKeyFrameAnimation(ScalarKeyFrameAnimation obj)
        {
            if (GetExisting(obj, out ScalarKeyFrameAnimation result))
            {
                return result;
            }

            result = CacheAndInitializeKeyframeAnimation(obj, _c.CreateScalarKeyFrameAnimation());
            foreach (var kf in obj.KeyFrames)
            {
                switch (kf.Type)
                {
                    case KeyFrameAnimation<float>.KeyFrameType.Expression:
                        var expressionKeyFrame = (KeyFrameAnimation<float>.ExpressionKeyFrame)kf;
                        result.InsertExpressionKeyFrame(kf.Progress, expressionKeyFrame.Expression, GetCompositionEasingFunction(kf.Easing));
                        break;
                    case KeyFrameAnimation<float>.KeyFrameType.Value:
                        var valueKeyFrame = (KeyFrameAnimation<float>.ValueKeyFrame)kf;
                        result.InsertKeyFrame(kf.Progress, valueKeyFrame.Value, GetCompositionEasingFunction(kf.Easing));
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        Vector2KeyFrameAnimation GetVector2KeyFrameAnimation(Vector2KeyFrameAnimation obj)
        {
            if (GetExisting(obj, out Vector2KeyFrameAnimation result))
            {
                return result;
            }

            result = CacheAndInitializeKeyframeAnimation(obj, _c.CreateVector2KeyFrameAnimation());
            foreach (var kf in obj.KeyFrames)
            {
                switch (kf.Type)
                {
                    case KeyFrameAnimation<Vector2>.KeyFrameType.Expression:
                        var expressionKeyFrame = (KeyFrameAnimation<Vector2>.ExpressionKeyFrame)kf;
                        result.InsertExpressionKeyFrame(kf.Progress, expressionKeyFrame.Expression, GetCompositionEasingFunction(kf.Easing));
                        break;
                    case KeyFrameAnimation<Vector2>.KeyFrameType.Value:
                        var valueKeyFrame = (KeyFrameAnimation<Vector2>.ValueKeyFrame)kf;
                        result.InsertKeyFrame(kf.Progress, valueKeyFrame.Value, GetCompositionEasingFunction(kf.Easing));
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        Vector3KeyFrameAnimation GetVector3KeyFrameAnimation(Vector3KeyFrameAnimation obj)
        {
            if (GetExisting(obj, out Vector3KeyFrameAnimation result))
            {
                return result;
            }

            result = CacheAndInitializeKeyframeAnimation(obj, _c.CreateVector3KeyFrameAnimation());
            foreach (var kf in obj.KeyFrames)
            {
                switch (kf.Type)
                {
                    case KeyFrameAnimation<Vector3>.KeyFrameType.Expression:
                        var expressionKeyFrame = (KeyFrameAnimation<Vector3>.ExpressionKeyFrame)kf;
                        result.InsertExpressionKeyFrame(kf.Progress, expressionKeyFrame.Expression, GetCompositionEasingFunction(kf.Easing));
                        break;
                    case KeyFrameAnimation<Vector3>.KeyFrameType.Value:
                        var valueKeyFrame = (KeyFrameAnimation<Vector3>.ValueKeyFrame)kf;
                        result.InsertKeyFrame(kf.Progress, valueKeyFrame.Value, GetCompositionEasingFunction(kf.Easing));
                        break;
                    default:
                        throw new InvalidCastException();
                }
            }
            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        PathKeyFrameAnimation GetPathKeyFrameAnimation(PathKeyFrameAnimation obj)
        {
            if (GetExisting(obj, out PathKeyFrameAnimation result))
            {
                return result;
            }

            result = CacheAndInitializeKeyframeAnimation(obj, _c.CreatePathKeyFrameAnimation());
            foreach (var kf in obj.KeyFrames)
            {
                result.InsertKeyFrame(kf.Progress, GetCompositionPath(((PathKeyFrameAnimation.ValueKeyFrame)kf).Value), GetCompositionEasingFunction(kf.Easing));
            }
            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        CompositionEasingFunction GetCompositionEasingFunction(CompositionEasingFunction obj)
        {
            switch (obj.Type)
            {
                case CompositionObjectType.LinearEasingFunction:
                    return GetLinearEasingFunction((LinearEasingFunction)obj);
                case CompositionObjectType.StepEasingFunction:
                    return GetStepEasingFunction((StepEasingFunction)obj);
                case CompositionObjectType.CubicBezierEasingFunction:
                    return GetCubicBezierEasingFunction((CubicBezierEasingFunction)obj);
                default:
                    throw new InvalidOperationException();
            }
        }

        CompositionClip GetCompositionClip(CompositionClip obj)
        {
            switch (obj.Type)
            {
                case CompositionObjectType.InsetClip:
                    return GetInsetClip((InsetClip)obj);
                case CompositionObjectType.CompositionGeometricClip:
                    return GetCompositionGeometricClip((CompositionGeometricClip)obj);
                default:
                    throw new InvalidOperationException();
            }
        }

        InsetClip GetInsetClip(InsetClip obj)
        {
            if (GetExisting(obj, out InsetClip result))
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
            StartAnimationsAndFreeze(obj, result);
            return result;

        }

        CompositionGeometricClip GetCompositionGeometricClip(CompositionGeometricClip obj)
        {
            if (GetExisting(obj, out CompositionGeometricClip result))
            {
                return result;
            }

            result = CacheAndInitializeCompositionObject(obj, _c.CreateCompositionGeometricClip());
            result.Geometry = GetCompositionGeometry(obj.Geometry);

            return result;
        }

        LinearEasingFunction GetLinearEasingFunction(LinearEasingFunction obj)
        {
            if (GetExisting(obj, out LinearEasingFunction result))
            {
                return result;
            }

            result = CacheAndInitializeCompositionObject(obj, _c.CreateLinearEasingFunction());
            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        StepEasingFunction GetStepEasingFunction(StepEasingFunction obj)
        {
            if (GetExisting(obj, out StepEasingFunction result))
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
            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        CubicBezierEasingFunction GetCubicBezierEasingFunction(CubicBezierEasingFunction obj)
        {
            if (GetExisting(obj, out CubicBezierEasingFunction result))
            {
                return result;
            }

            result = CacheAndInitializeCompositionObject(obj, _c.CreateCubicBezierEasingFunction(obj.ControlPoint1, obj.ControlPoint2));
            StartAnimationsAndFreeze(obj, result);
            return result;
        }
        CompositionViewBox GetCompositionViewBox(CompositionViewBox obj)
        {
            if (GetExisting(obj, out CompositionViewBox result))
            {
                return result;
            }

            result = CacheAndInitializeCompositionObject(obj, _c.CreateViewBox());
            result.Size = obj.Size;
            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        CompositionShape GetCompositionShape(CompositionShape obj)
        {
            switch (obj.Type)
            {
                case CompositionObjectType.CompositionSpriteShape:
                    return GetCompositionSpriteShape((CompositionSpriteShape)obj);
                case CompositionObjectType.CompositionContainerShape:
                    return GetCompositionContainerShape((CompositionContainerShape)obj);
                default:
                    throw new InvalidOperationException();
            }
        }

        CompositionContainerShape GetCompositionContainerShape(CompositionContainerShape obj)
        {
            if (GetExisting(obj, out CompositionContainerShape result))
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
                else if (!child.Animators.Any() && child.Type == CompositionObjectType.CompositionContainerShape)
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
            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        CompositionSpriteShape GetCompositionSpriteShape(CompositionSpriteShape obj)
        {
            if (GetExisting(obj, out CompositionSpriteShape result))
            {
                return result;
            }

            result = CacheAndInitializeShape(obj, _c.CreateSpriteShape());

            if (obj.StrokeBrush != null)
            {
                result.StrokeBrush = GetCompositionBrush(obj.StrokeBrush);
                if (obj.StrokeDashCap != CompositionStrokeCap.Flat)
                {
                    result.StrokeDashCap = obj.StrokeDashCap;
                }
                if (obj.StrokeStartCap != CompositionStrokeCap.Flat)
                {
                    result.StrokeStartCap = obj.StrokeStartCap;
                }
                if (obj.StrokeEndCap != CompositionStrokeCap.Flat)
                {
                    result.StrokeEndCap = obj.StrokeEndCap;
                }
                if (obj.StrokeThickness != 1)
                {
                    result.StrokeThickness = obj.StrokeThickness;
                }
                if (obj.StrokeMiterLimit != 1)
                {
                    result.StrokeMiterLimit = obj.StrokeMiterLimit;
                }
                if (obj.StrokeLineJoin != CompositionStrokeLineJoin.Miter)
                {
                    result.StrokeLineJoin = obj.StrokeLineJoin;
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
            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        CompositionGeometry GetCompositionGeometry(CompositionGeometry obj)
        {
            switch (obj.Type)
            {
                case CompositionObjectType.CompositionPathGeometry:
                    return GetCompositionPathGeometry((CompositionPathGeometry)obj);

                case CompositionObjectType.CompositionEllipseGeometry:
                    return GetCompositionEllipseGeometry((CompositionEllipseGeometry)obj);

                case CompositionObjectType.CompositionRectangleGeometry:
                    return GetCompositionRectangleGeometry((CompositionRectangleGeometry)obj);

                case CompositionObjectType.CompositionRoundedRectangleGeometry:
                    return GetCompositionRoundedRectangleGeometry((CompositionRoundedRectangleGeometry)obj);

                default:
                    throw new InvalidOperationException();
            }
        }

        CompositionEllipseGeometry GetCompositionEllipseGeometry(CompositionEllipseGeometry obj)
        {
            if (GetExisting(obj, out CompositionEllipseGeometry result))
            {
                return result;
            }
            result = CacheAndInitializeCompositionGeometry(obj, _c.CreateEllipseGeometry());
            if (obj.Center.X != 0 || obj.Center.Y != 0)
            {
                result.Center = obj.Center;
            }
            result.Radius = obj.Radius;
            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        CompositionRectangleGeometry GetCompositionRectangleGeometry(CompositionRectangleGeometry obj)
        {
            if (GetExisting(obj, out CompositionRectangleGeometry result))
            {
                return result;
            }
            result = CacheAndInitializeCompositionGeometry(obj, _c.CreateRectangleGeometry());
            if (obj.Offset != null)
            {
                result.Offset = obj.Offset;
            }
            result.Size = obj.Size;
            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        CompositionRoundedRectangleGeometry GetCompositionRoundedRectangleGeometry(CompositionRoundedRectangleGeometry obj)
        {
            if (GetExisting(obj, out CompositionRoundedRectangleGeometry result))
            {
                return result;
            }
            result = CacheAndInitializeCompositionGeometry(obj, _c.CreateRoundedRectangleGeometry());
            if (obj.Offset != null)
            {
                result.Offset = obj.Offset;
            }
            result.Size = obj.Size;
            result.CornerRadius = obj.CornerRadius;
            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        CompositionPathGeometry GetCompositionPathGeometry(CompositionPathGeometry obj)
        {
            if (GetExisting(obj, out CompositionPathGeometry result))
            {
                return result;
            }
            result = CacheAndInitializeCompositionGeometry(obj, _c.CreatePathGeometry(GetCompositionPath(obj.Path)));
            StartAnimationsAndFreeze(obj, result);
            return result;
        }

        CompositionPath GetCompositionPath(CompositionPath obj)
        {
            if (GetExisting(obj, out CompositionPath result))
            {
                return result;
            }

            result = Cache(obj, new CompositionPath(GetCanvasGeometry(obj.Source)));
            InitializeIDescribable(obj, result);
            return result;
        }

        CanvasGeometry GetCanvasGeometry(Wg.IGeometrySource2D obj)
        {
            if (GetExistingCanvasGeometry((CanvasGeometry)obj, out CanvasGeometry result))
            {
                return result;
            }

            var canvasGeometry = (CanvasGeometry)obj;
            switch (canvasGeometry.Type)
            {
                case CanvasGeometry.GeometryType.Combination:
                    {
                        var combination = (CanvasGeometry.Combination)canvasGeometry;
                        result = CacheCanvasGeometry((CanvasGeometry)obj, GetCanvasGeometry(combination.A).CombineWith(
                            GetCanvasGeometry(combination.B),
                            combination.Matrix,
                            combination.CombineMode));
                        break;
                    }
                case CanvasGeometry.GeometryType.Ellipse:
                    var ellipse = (CanvasGeometry.Ellipse)canvasGeometry;
                    result = CanvasGeometry.CreateEllipse(
                        null,
                        ellipse.X,
                        ellipse.Y,
                        ellipse.RadiusX,
                        ellipse.RadiusY);
                    break;
                case CanvasGeometry.GeometryType.Path:
                    using (var builder = new CanvasPathBuilder(null))
                    {
                        var path = (CanvasGeometry.Path)canvasGeometry;

                        if (path.FilledRegionDetermination != CanvasFilledRegionDetermination.Alternate)
                        {
                            builder.SetFilledRegionDetermination(path.FilledRegionDetermination);
                        }

                        foreach (var command in path.Commands)
                        {
                            switch (command.Type)
                            {
                                case CanvasPathBuilder.CommandType.BeginFigure:
                                    builder.BeginFigure(((CanvasPathBuilder.Command.BeginFigure)command).StartPoint);
                                    break;
                                case CanvasPathBuilder.CommandType.EndFigure:
                                    builder.EndFigure(((CanvasPathBuilder.Command.EndFigure)command).FigureLoop);
                                    break;
                                case CanvasPathBuilder.CommandType.AddLine:
                                    builder.AddLine(((CanvasPathBuilder.Command.AddLine)command).EndPoint);
                                    break;
                                case CanvasPathBuilder.CommandType.AddCubicBezier:
                                    var cb = (CanvasPathBuilder.Command.AddCubicBezier)command;
                                    builder.AddCubicBezier(cb.ControlPoint1, cb.ControlPoint2, cb.EndPoint);
                                    break;
                                default:
                                    throw new InvalidOperationException();
                            }
                        }
                        result = CacheCanvasGeometry((CanvasGeometry)obj, CanvasGeometry.CreatePath(builder));
                    }
                    break;
                case CanvasGeometry.GeometryType.RoundedRectangle:
                    var roundedRectangle = (CanvasGeometry.RoundedRectangle)canvasGeometry;
                    result = CanvasGeometry.CreateRoundedRectangle(
                        null,
                        roundedRectangle.X,
                        roundedRectangle.Y,
                        roundedRectangle.W,
                        roundedRectangle.H,
                        roundedRectangle.RadiusX,
                        roundedRectangle.RadiusY);
                    break;
                case CanvasGeometry.GeometryType.TransformedGeometry:
                    var transformedGeometry = (CanvasGeometry.TransformedGeometry)canvasGeometry;
                    result = GetCanvasGeometry(transformedGeometry.SourceGeometry).Transform(transformedGeometry.TransformMatrix);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            InitializeIDescribable(canvasGeometry, result);
            return result;
        }

        CompositionBrush GetCompositionBrush(CompositionBrush obj)
        {
            return GetCompositionColorBrush((CompositionColorBrush)obj);
        }

        CompositionColorBrush GetCompositionColorBrush(CompositionColorBrush obj)
        {
            if (GetExisting(obj, out CompositionColorBrush result))
            {
                return result;
            }
            result = CacheAndInitializeCompositionObject(obj, _c.CreateColorBrush(obj.Color));
            StartAnimationsAndFreeze(obj, result);
            return result;
        }
    }
}
