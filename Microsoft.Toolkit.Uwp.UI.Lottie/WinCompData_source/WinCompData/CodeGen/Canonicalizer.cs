// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Mgcg;
using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools;
using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Wui;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.CodeGen
{
    static class Canonicalizer
    {
        internal static void Canonicalize<T>(ObjectGraph<T> graph, bool ignoreCommentProperties) where T : CanonicalizedNode<T>, new()
        {
            CanonicalizerWorker<T>.Canonicalize(graph, ignoreCommentProperties);
        }

        sealed class CanonicalizerWorker<T> where T : CanonicalizedNode<T>, new()
        {
            readonly ObjectGraph<T> _graph;
            readonly bool _ignoreCommentProperties;

            CanonicalizerWorker(ObjectGraph<T> graph, bool ignoreCommentProperties)
            {
                _graph = graph;
                _ignoreCommentProperties = ignoreCommentProperties;
            }

            internal static void Canonicalize(ObjectGraph<T> graph, bool ignoreCommentProperties)
            {
                var canonicalizer = new CanonicalizerWorker<T>(graph, ignoreCommentProperties);
                canonicalizer.Canonicalize();
            }

            // Find the nodes that are equivalent and point them all to a single canonical representation.
            void Canonicalize()
            {
                CanonicalizeInsetClips();
                CanonicalizeEllipseGeometries();
                CanonicalizeRectangleGeometries();
                CanonicalizeRoundedRectangleGeometries();

                CanonicalizeCanvasGeometryPaths();

                // CompositionPath must be canonicalized after CanvasGeometry paths.
                CanonicalizeCompositionPaths();

                // CompositionPathGeometry must be canonicalized after CompositionPath.
                CanonicalizeCompositionPathGeometries();

                // Easing functions must be canonicalized before keyframes are canonicalized.
                CanonicalizeLinearEasingFunctions();
                CanonicalizeCubicBezierEasingFunctions();
                CanonicalizeStepEasingFunctions();

                CanonicalizeExpressionAnimations();

                CanonicalizeKeyFrameAnimations<KeyFrameAnimation<Color>, Color>(CompositionObjectType.ColorKeyFrameAnimation);
                CanonicalizeKeyFrameAnimations<KeyFrameAnimation<float>, float>(CompositionObjectType.ScalarKeyFrameAnimation);
                CanonicalizeKeyFrameAnimations<KeyFrameAnimation<Vector2>, Vector2>(CompositionObjectType.Vector2KeyFrameAnimation);
                CanonicalizeKeyFrameAnimations<KeyFrameAnimation<Vector3>, Vector3>(CompositionObjectType.Vector3KeyFrameAnimation);

                // ColorKeyFrameAnimations must be canonicalized before color brushes are canonicalized.
                CanonicalizeColorBrushes();
            }

            T NodeFor(Wg.IGeometrySource2D obj) => _graph[obj].Canonical;
            T NodeFor(CompositionObject obj) => _graph[obj].Canonical;
            T NodeFor(CompositionPath obj) => _graph[obj].Canonical;

            C CanonicalObject<C>(Wg.IGeometrySource2D obj) => (C)NodeFor(obj).Object;
            C CanonicalObject<C>(CompositionObject obj) => (C)NodeFor(obj).Object;
            C CanonicalObject<C>(CompositionPath obj) => (C)NodeFor(obj).Object;

            IEnumerable<(T Node, C Object)> GetCompositionObjects<C>(CompositionObjectType type) where C : CompositionObject
                => _graph.CompositionObjectNodes.Where(n => n.Object.Type == type).Select(n => (n.Node, (C)n.Object));

            IEnumerable<(T Node, C Object)> GetCanonicalizableCompositionObjects<C>(CompositionObjectType type)
                where C : CompositionObject
            {
                var items = GetCompositionObjects<C>(type);
                return
                    from item in items
                    let obj = item.Object
                    where (_ignoreCommentProperties || obj.Comment == null)
                       && !obj.Properties.PropertyNames.Any()
                       && !obj.Animators.Any()
                    select (item.Node, obj);
            }

            IEnumerable<(T Node, C Object)> GetCanonicalizableCanvasGeometries<C>(CanvasGeometry.GeometryType type)
                where C : CanvasGeometry
            {
                return
                    from item in _graph.CanvasGeometryNodes
                    let obj = item.Object
                    where obj.Type == type
                    select (item.Node, (C)obj);
            }

            void CanonicalizeExpressionAnimations()
            {
                var items = GetCanonicalizableCompositionObjects<ExpressionAnimation>(CompositionObjectType.ExpressionAnimation);

                // TODO - handle more than one reference parameter.
                var grouping =
                    from item in items
                    where item.Object.ReferenceParameters.Count() == 1
                    group item.Node by GetExpressionAnimationKey1(item.Object)
                    into grouped
                    select grouped;

                CanonicalizeGrouping(grouping);
            }


            (WinCompData.Expressions.Expression, string, string, CompositionObject) GetExpressionAnimationKey1(ExpressionAnimation animation)
            {
                var rp0 = animation.ReferenceParameters.First();

                return (animation.Expression, animation.Target, rp0.Key, CanonicalObject<CompositionObject>(rp0.Value));
            }

            void CanonicalizeKeyFrameAnimations<A, V>(CompositionObjectType animationType) where A : KeyFrameAnimation<V>
            {
                var items = GetCanonicalizableCompositionObjects<A>(animationType);

                var grouping =
                    from item in items
                    group item.Node by new KeyFrameAnimationKey<V>(this, item.Object)
                    into grouped
                    select grouped;

                CanonicalizeGrouping(grouping);
            }



            sealed class KeyFrameAnimationKey<V>
            {
                readonly CanonicalizerWorker<T> _owner;
                readonly KeyFrameAnimation<V> _obj;

                internal KeyFrameAnimationKey(CanonicalizerWorker<T> owner, KeyFrameAnimation<V> obj)
                {
                    _owner = owner;
                    _obj = obj;
                }

                public override int GetHashCode()
                {
                    // Not the perfect hash, but not terrible
                    return _obj.KeyFrameCount ^ (int)_obj.Duration.Ticks;
                }

                public override bool Equals(object obj)
                {
                    if (ReferenceEquals(this, obj))
                    {
                        return true;
                    }
                    if (obj == null)
                    {
                        return false;
                    }
                    var other = obj as KeyFrameAnimationKey<V>;
                    if (other == null)
                    {
                        return false;
                    }

                    var thisObj = _obj;
                    var otherObj = other._obj;

                    if (thisObj.Duration != otherObj.Duration)
                    {
                        return false;
                    }

                    if (thisObj.KeyFrameCount != otherObj.KeyFrameCount)
                    {
                        return false;
                    }

                    if (thisObj.Target != otherObj.Target)
                    {
                        return false;
                    }

                    var thisKfs = thisObj.KeyFrames.ToArray();
                    var otherKfs = otherObj.KeyFrames.ToArray();

                    for (var i = 0; i < thisKfs.Length; i++)
                    {
                        var thisKf = thisKfs[i];
                        var otherKf = otherKfs[i];
                        if (thisKf.Progress != otherKf.Progress)
                        {
                            return false;
                        }
                        if (thisKf.Type != otherKf.Type)
                        {
                            return false;
                        }
                        if (_owner.NodeFor(thisKf.Easing) != _owner.NodeFor(otherKf.Easing))
                        {
                            return false;
                        }
                        switch (thisKf.Type)
                        {
                            case KeyFrameAnimation<V>.KeyFrameType.Expression:
                                var thisExpressionKeyFrame = (KeyFrameAnimation<V>.ExpressionKeyFrame)thisKf;
                                var otherExpressionKeyFrame = (KeyFrameAnimation<V>.ExpressionKeyFrame)otherKf;
                                if (thisExpressionKeyFrame.Expression != otherExpressionKeyFrame.Expression)
                                {
                                    return false;
                                }
                                break;
                            case KeyFrameAnimation<V>.KeyFrameType.Value:
                                var thisValueKeyFrame = (KeyFrameAnimation<V>.ValueKeyFrame)thisKf;
                                var otherValueKeyFrame = (KeyFrameAnimation<V>.ValueKeyFrame)otherKf;
                                if (!thisValueKeyFrame.Value.Equals(otherValueKeyFrame.Value))
                                {
                                    return false;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    return true;
                }
            }

            void CanonicalizeColorBrushes()
            {
                // Canonicalize color brushes that have no animations, or have just a Color animation.
                var nodes = GetCompositionObjects<CompositionColorBrush>(CompositionObjectType.CompositionColorBrush);

                var items =
                    from item in nodes
                    let obj = item.Object
                    where (_ignoreCommentProperties || obj.Comment == null)
                       && !obj.Properties.PropertyNames.Any()
                    select (item.Node, obj);

                var grouping =
                    from item in items
                    let obj = item.obj
                    let animators = obj.Animators.ToArray()
                    where animators.Length == 0 || (animators.Length == 1 && animators[0].AnimatedProperty == "Color")
                    let animator = animators.FirstOrDefault()
                    let canonicalAnimator = animator == null ? null : CanonicalObject<ColorKeyFrameAnimation>(animator.Animation)
                    group item.Node by new
                    {
                        obj.Color.A,
                        obj.Color.R,
                        obj.Color.G,
                        obj.Color.B,
                        canonicalAnimator
                    } into grouped
                    select grouped;

                CanonicalizeGrouping(grouping);
            }

            void CanonicalizeEllipseGeometries()
            {
                var items = GetCanonicalizableCompositionObjects<CompositionEllipseGeometry>(CompositionObjectType.CompositionEllipseGeometry);

                var grouping =
                    from item in items
                    let obj = item.Object
                    group item.Node by new
                    {
                        obj.Center,
                        obj.Radius,
                        obj.TrimStart,
                        obj.TrimEnd,
                        obj.TrimOffset
                    } into grouped
                    select grouped;

                CanonicalizeGrouping(grouping);
            }

            void CanonicalizeRectangleGeometries()
            {
                var items = GetCanonicalizableCompositionObjects<CompositionRectangleGeometry>(CompositionObjectType.CompositionRectangleGeometry);

                var grouping =
                    from item in items
                    let obj = item.Object
                    group item.Node by new
                    {
                        obj.Offset,
                        obj.Size,
                        obj.TrimStart,
                        obj.TrimEnd,
                        obj.TrimOffset
                    }
                    into grouped
                    select grouped;

                CanonicalizeGrouping(grouping);
            }

            void CanonicalizeRoundedRectangleGeometries()
            {
                var items = GetCanonicalizableCompositionObjects<CompositionRoundedRectangleGeometry>(CompositionObjectType.CompositionRoundedRectangleGeometry);

                var grouping =
                    from item in items
                    let obj = item.Object
                    group item.Node by new
                    {
                        obj.Offset,
                        obj.Size,
                        obj.CornerRadius,
                        obj.TrimStart,
                        obj.TrimEnd,
                        obj.TrimOffset
                    }
                    into grouped
                    select grouped;

                CanonicalizeGrouping(grouping);
            }


            void CanonicalizeCompositionPathGeometries()
            {
                var items = GetCanonicalizableCompositionObjects<CompositionPathGeometry>(CompositionObjectType.CompositionPathGeometry);

                var grouping =
                    from item in items
                    let obj = item.Object
                    let path = CanonicalObject<CompositionPath>(obj.Path)
                    group item.Node by new
                    {
                        path,
                        obj.TrimStart,
                        obj.TrimEnd,
                        obj.TrimOffset
                    } into grouped
                    select grouped;

                CanonicalizeGrouping(grouping);
            }

            void CanonicalizeCanvasGeometryPaths()
            {
                var items = GetCanonicalizableCanvasGeometries<CanvasGeometry.Path>(CanvasGeometry.GeometryType.Path);
                var grouping =
                    from item in items
                    let obj = item.Object
                    group item.Node by obj into grouped
                    select grouped;

                CanonicalizeGrouping(grouping);
            }

            void CanonicalizeCompositionPaths()
            {
                var grouping =
                    from item in _graph.CompositionPathNodes
                    let obj = item.Object
                    let canonicalSource = CanonicalObject<CanvasGeometry>(obj.Source)
                    group item.Node by canonicalSource into grouped
                    select grouped;

                CanonicalizeGrouping(grouping);
            }

            void CanonicalizeInsetClips()
            {
                var items = GetCanonicalizableCompositionObjects<InsetClip>(CompositionObjectType.InsetClip);

                var grouping =
                    from item in items
                    let obj = item.Object
                    group item.Node by
                    new
                    {
                        obj.BottomInset,
                        obj.LeftInset,
                        obj.RightInset,
                        obj.TopInset,
                        obj.CenterPoint,
                        obj.Scale,
                    }
                    into grouped
                    select grouped;

                CanonicalizeGrouping(grouping);
            }

            void CanonicalizeCubicBezierEasingFunctions()
            {
                var items = GetCanonicalizableCompositionObjects<CubicBezierEasingFunction>(CompositionObjectType.CubicBezierEasingFunction);

                var grouping =
                    from item in items
                    let obj = item.Object
                    group item.Node by
                    new
                    {
                        obj.ControlPoint1,
                        obj.ControlPoint2,
                    }
                    into grouped
                    select grouped;

                CanonicalizeGrouping(grouping);
            }

            void CanonicalizeLinearEasingFunctions()
            {
                var items = GetCanonicalizableCompositionObjects<LinearEasingFunction>(CompositionObjectType.LinearEasingFunction);

                // Every LinearEasingFunction is equivalent.
                var grouping =
                    from item in items
                    group item.Node by true into grouped
                    select grouped;

                CanonicalizeGrouping(grouping);
            }

            void CanonicalizeStepEasingFunctions()
            {
                var items = GetCanonicalizableCompositionObjects<StepEasingFunction>(CompositionObjectType.StepEasingFunction);

                var grouping =
                    from item in items
                    let obj = item.Object
                    group item.Node by
                    new
                    {
                        obj.FinalStep,
                        obj.InitialStep,
                        obj.IsFinalStepSingleFrame,
                        obj.IsInitialStepSingleFrame,
                        obj.StepCount,
                    }
                    into grouped
                    select grouped;

                CanonicalizeGrouping(grouping);
            }

            static void CanonicalizeGrouping<K>(IEnumerable<IGrouping<K, T>> grouping)
            {
                foreach (var group in grouping)
                {
                    // The canonical node is the node that appears first in the
                    // traversal of the tree.
                    var orderedGroup = group.OrderBy(n => n.Position);
                    var groupArray = orderedGroup.ToArray();
                    var canonical = groupArray[0];

                    // Point every node to the canonical node.
                    foreach (var node in group)
                    {
                        node.Canonical = canonical;
                        node.NodesInGroup = groupArray;
                    }
                }
            }

        }
    }
}
