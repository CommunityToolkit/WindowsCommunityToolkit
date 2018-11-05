// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Mgcg;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools
{

#if !WINDOWS_UWP
    public
#endif
    abstract class Graph
    {
        int _vertexCounter;

        protected private Graph() { }

        /// <summary>
        /// Returns the graph of nodes reachable from the given <see cref="CompositionObject"/> root.
        /// </summary>
        public static ObjectGraph<Node> FromCompositionObject(CompositionObject root, bool includeVertices)
            => ObjectGraph<Node>.FromCompositionObject(root, includeVertices);

        /// <summary>
        /// A simple non-extensible node type.
        /// </summary>
        public sealed class Node : Node<Node> { }

        public class Node<T> : INodePrivate<T> where T : Node<T>, new()
        {
            static readonly Vertex[] s_emptyArray = new Vertex[0];

            List<Vertex> _inReferences;

            public object Object { get; set; }

            public Vertex[] InReferences => _inReferences == null ? s_emptyArray : _inReferences.ToArray();

            public int ReferenceCount => InReferences.Length;

            public NodeType Type { get; private set; }

            /// <summary>
            /// The position of this node in a traversal of the graph.
            /// </summary>
            public int Position { get; private set; }

            public struct Vertex
            {
                /// <summary>
                /// The position of this vertex in a traversal of the graph.
                /// </summary>
                public int Position { get; internal set; }

                /// <summary>
                /// The node at the other end of the <see cref="Vertex"/>.
                /// </summary>
                public T Node { get; internal set; }
            }


            List<Vertex> INodePrivate<T>.InReferences
            {
                get
                {
                    if (_inReferences == null)
                    {
                        _inReferences = new List<Vertex>();
                    }
                    return _inReferences;
                }
            }

            void INodePrivate<T>.Initialize(NodeType type, int position)
            {
                Type = type;
                Position = position;
            }

        }

        public enum NodeType
        {
            CompositionObject,
            CompositionPath,
            CanvasGeometry,
        }


        protected void InitializeNode<T>(T node, NodeType type, int position) where T : Node<T>, new()
            => NodePrivate(node).Initialize(type, position);

        protected void AddVertex<T>(T from, T to) where T : Node<T>, new()
        {
            var toNode = NodePrivate(to);
            toNode.InReferences.Add(new Node<T>.Vertex { Position = _vertexCounter++, Node = from });
        }

        static INodePrivate<T> NodePrivate<T>(T node) where T : Node<T>, new()
        {
            return node;
        }

        // Private inteface that allows ObjectGraph to modify Nodes.
        interface INodePrivate<T> where T : Node<T>, new()
        {
            void Initialize(NodeType type, int position);
            List<Node<T>.Vertex> InReferences { get; }
        }
    }

    /// <summary>
    /// The graph of creatable objects reachable from a <see cref="CompositionObject"/>.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class ObjectGraph<T> : Graph where T : Graph.Node<T>, new()
    {
        readonly bool _includeVertices;
        readonly Dictionary<Wg.IGeometrySource2D, T> _canvasGeometryReferences = new Dictionary<Wg.IGeometrySource2D, T>();
        readonly Dictionary<CompositionObject, T> _compositionObjectReferences = new Dictionary<CompositionObject, T>();
        readonly Dictionary<CompositionPath, T> _compositionPathReferences = new Dictionary<CompositionPath, T>();
        readonly Dictionary<CompositionObjectType, int> _compositionObjectCounter = new Dictionary<CompositionObjectType, int>();
        int _positionCounter;

        ObjectGraph(bool includeVertices)
        {
            _includeVertices = includeVertices;
        }

        /// <summary>
        /// Returns the graph of nodes reachable from the given <see cref="CompositionObject"/> root.
        /// </summary>
        public static new ObjectGraph<T> FromCompositionObject(CompositionObject root, bool includeVertices)
        {
            var result = new ObjectGraph<T>(includeVertices);
            result.Reference(null, root);
            return result;
        }

        public IEnumerable<T> Nodes => _compositionObjectReferences.Values.Concat(_compositionPathReferences.Values).Concat(_canvasGeometryReferences.Values);

        public IEnumerable<(T Node, CanvasGeometry Object)> CanvasGeometryNodes =>
            _canvasGeometryReferences.Values.Select(n => (n, (CanvasGeometry)n.Object));

        public IEnumerable<(T Node, CompositionObject Object)> CompositionObjectNodes =>
            _compositionObjectReferences.Values.Select(n => (n, (CompositionObject)n.Object));

        public IEnumerable<(T Node, CompositionPath Object)> CompositionPathNodes =>
            _compositionPathReferences.Values.Select(n => (n, (CompositionPath)n.Object));

        internal T this[Wg.IGeometrySource2D obj] => _canvasGeometryReferences[obj];

        internal T this[CompositionObject obj] => _compositionObjectReferences[obj];

        internal T this[CompositionPath obj] => _compositionPathReferences[obj];


        void Reference(T from, CompositionObject obj)
        {
            if (obj == null)
            {
                return;
            }

            if (_compositionObjectReferences.TryGetValue(obj, out var node))
            {
                // Object has been seen before. Just add the reference.
                if (_includeVertices && from != null)
                {
                    AddVertex(from, node);
                }
                return;
            }

            // Object has not been seen before. Register it, and visit it.
            // Create a node for the object.
            node = new T { Object = obj };

            InitializeNode(node, NodeType.CompositionObject, _positionCounter++);

            // Link the nodes in the graph.
            if (_includeVertices && from != null)
            {
                AddVertex(from, node);
            }
            _compositionObjectReferences.Add(obj, node);

            switch (obj.Type)
            {
                case CompositionObjectType.AnimationController:
                    VisitAnimationController((AnimationController)obj, node);
                    break;
                case CompositionObjectType.ColorKeyFrameAnimation:
                    VisitColorKeyFrameAnimation((ColorKeyFrameAnimation)obj, node);
                    break;
                case CompositionObjectType.CompositionColorBrush:
                    VisitCompositionColorBrush((CompositionColorBrush)obj, node);
                    break;
                case CompositionObjectType.CompositionContainerShape:
                    VisitCompositionContainerShape((CompositionContainerShape)obj, node);
                    break;
                case CompositionObjectType.CompositionEllipseGeometry:
                    VisitCompositionEllipseGeometry((CompositionEllipseGeometry)obj, node);
                    break;
                case CompositionObjectType.CompositionPathGeometry:
                    VisitCompositionPathGeometry((CompositionPathGeometry)obj, node);
                    break;
                case CompositionObjectType.CompositionPropertySet:
                    VisitCompositionPropertySet((CompositionPropertySet)obj, node);
                    break;
                case CompositionObjectType.CompositionRectangleGeometry:
                    VisitCompositionRectangleGeometry((CompositionRectangleGeometry)obj, node);
                    break;
                case CompositionObjectType.CompositionRoundedRectangleGeometry:
                    VisitCompositionRoundedRectangleGeometry((CompositionRoundedRectangleGeometry)obj, node);
                    break;
                case CompositionObjectType.CompositionSpriteShape:
                    VisitCompositionSpriteShape((CompositionSpriteShape)obj, node);
                    break;
                case CompositionObjectType.CompositionViewBox:
                    VisitCompositionViewBox((CompositionViewBox)obj, node);
                    break;
                case CompositionObjectType.ContainerVisual:
                    VisitContainerVisual((ContainerVisual)obj, node);
                    break;
                case CompositionObjectType.CubicBezierEasingFunction:
                    VisitCubicBezierEasingFunction(node, (CubicBezierEasingFunction)obj);
                    break;
                case CompositionObjectType.ExpressionAnimation:
                    VisitExpressionAnimation((ExpressionAnimation)obj, node);
                    break;
                case CompositionObjectType.InsetClip:
                    VisitInsetClip((InsetClip)obj, node);
                    break;
                case CompositionObjectType.CompositionGeometricClip:
                    VisitCompositionGeometricClip((CompositionGeometricClip)obj, node);
                    break;
                case CompositionObjectType.LinearEasingFunction:
                    VisitLinearEasingFunction((LinearEasingFunction)obj, node);
                    break;
                case CompositionObjectType.PathKeyFrameAnimation:
                    VisitPathKeyFrameAnimation((PathKeyFrameAnimation)obj, node);
                    break;
                case CompositionObjectType.ScalarKeyFrameAnimation:
                    VisitScalarKeyFrameAnimation((ScalarKeyFrameAnimation)obj, node);
                    break;
                case CompositionObjectType.ShapeVisual:
                    VisitShapeVisual((ShapeVisual)obj, node);
                    break;
                case CompositionObjectType.StepEasingFunction:
                    VisitStepEasingFunction((StepEasingFunction)obj, node);
                    break;
                case CompositionObjectType.Vector2KeyFrameAnimation:
                    VisitVector2KeyFrameAnimation((Vector2KeyFrameAnimation)obj, node);
                    break;
                case CompositionObjectType.Vector3KeyFrameAnimation:
                    VisitVector3KeyFrameAnimation((Vector3KeyFrameAnimation)obj, node);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            // Reference the animators after referencing all the other contents of the object. This is
            // done to ensure the position of animators is higher than the position of other
            // references. This ordering is consistent with how CompositionObjects are initialized:
            // 1. Instantiate the object
            // 2. Assign the values for the object's properties
            // 3. Start the animations
            foreach (var animator in obj.Animators)
            {
                Reference(node, animator.Animation);
                Reference(node, animator.Controller);
            }
        }

        bool Reference(T from, CompositionPath obj)
        {
            if (_compositionPathReferences.TryGetValue(obj, out var node))
            {
                AddVertex(from, node);
                return true;
            }
            else
            {
                node = new T { Object = obj };
                InitializeNode(node, NodeType.CompositionPath, _positionCounter++);
                AddVertex(from, node);
                _compositionPathReferences.Add(obj, node);
            }
            Reference(node, (CanvasGeometry)obj.Source);
            return true;
        }

        bool Reference(T from, CanvasGeometry obj)
        {
            if (_canvasGeometryReferences.TryGetValue(obj, out var node))
            {
                AddVertex(from, node);
                return true;
            }
            else
            {
                node = new T { Object = obj };
                InitializeNode(node, NodeType.CanvasGeometry, _positionCounter++);
                AddVertex(from, node);
                _canvasGeometryReferences.Add(obj, node);
            }

            switch (obj.Type)
            {
                case CanvasGeometry.GeometryType.Combination:
                    return VisitCombination((CanvasGeometry.Combination)obj, node);
                case CanvasGeometry.GeometryType.Ellipse:
                    return VisitEllipse((CanvasGeometry.Ellipse)obj, node);
                case CanvasGeometry.GeometryType.Path:
                    return VisitPath((CanvasGeometry.Path)obj, node);
                case CanvasGeometry.GeometryType.RoundedRectangle:
                    return VisitRoundedRectangle((CanvasGeometry.RoundedRectangle)obj, node);
                case CanvasGeometry.GeometryType.TransformedGeometry:
                    return VisitTransformedGeometry((CanvasGeometry.TransformedGeometry)obj, node);
                default:
                    throw new InvalidOperationException();
            }
        }

        bool VisitAnimationController(AnimationController obj, T node)
        {
            VisitCompositionObject(obj, node);
            return true;
        }

        bool VisitCanvasGeometry(CanvasGeometry obj, T node)
        {
            return true;
        }

        bool VisitCombination(CanvasGeometry.Combination obj, T node)
        {
            Reference(node, obj.A);
            Reference(node, obj.B);
            return VisitCanvasGeometry(obj, node);
        }

        bool VisitEllipse(CanvasGeometry.Ellipse obj, T node)
        {
            return VisitCanvasGeometry(obj, node);
        }

        bool VisitPath(CanvasGeometry.Path obj, T node)
        {
            return VisitCanvasGeometry(obj, node);
        }

        bool VisitRoundedRectangle(CanvasGeometry.RoundedRectangle obj, T node)
        {
            return VisitCanvasGeometry(obj, node);
        }
        bool VisitTransformedGeometry(CanvasGeometry.TransformedGeometry obj, T node)
        {
            Reference(node, obj.SourceGeometry);
            return VisitCanvasGeometry(obj, node);
        }

        bool VisitCompositionObject(CompositionObject obj, T node)
        {
            // Prevent infinite recursion on CompositionPropertySet (its Properties
            // refer back to itself).
            if (obj.Type != CompositionObjectType.CompositionPropertySet)
            {
                Reference(node, obj.Properties);
            }
            // Do not visit the animators here. That is done after visiting the
            // references from the derived class' properties. This is to be consistent
            // the with the order of initialization of objects.
            return true;
        }

        bool VisitCompositionPropertySet(CompositionPropertySet obj, T node)
        {
            return VisitCompositionObject(obj, node);
        }

        bool VisitCompositionRectangleGeometry(CompositionRectangleGeometry obj, T node)
        {
            return VisitCompositionGeometry(obj, node);
        }

        bool VisitCompositionRoundedRectangleGeometry(CompositionRoundedRectangleGeometry obj, T node)
        {
            return VisitCompositionGeometry(obj, node);
        }

        bool VisitExpressionAnimation(ExpressionAnimation obj, T node)
        {
            return VisitCompositionAnimation(obj, node);
        }

        bool VisitCompositionClip(CompositionClip obj, T node)
        {
            return VisitCompositionObject(obj, node);
        }

        bool VisitInsetClip(InsetClip obj, T node)
        {
            return VisitCompositionClip(obj, node);
        }

        bool VisitCompositionGeometricClip(CompositionGeometricClip obj, T node)
        {
            VisitCompositionClip(obj, node);
            Reference(node, obj.Geometry);
            return true;
        }

        bool VisitCompositionEasingFunction(CompositionEasingFunction obj, T node)
        {
            return VisitCompositionObject(obj, node);
        }

        bool VisitCubicBezierEasingFunction(T node, CubicBezierEasingFunction obj)
        {
            return VisitCompositionEasingFunction(obj, node);
        }

        bool VisitLinearEasingFunction(LinearEasingFunction obj, T node)
        {
            return VisitCompositionEasingFunction(obj, node);
        }

        bool VisitPathKeyFrameAnimation(PathKeyFrameAnimation obj, T node)
        {
            VisitKeyFrameAnimation(obj, node);
            foreach (var keyFrame in obj.KeyFrames)
            {
                Reference(node, ((KeyFrameAnimation<CompositionPath>.ValueKeyFrame)keyFrame).Value);
            }
            return true;
        }

        bool VisitCompositionAnimation(CompositionAnimation obj, T node)
        {
            VisitCompositionObject(obj, node);
            foreach (var parameter in obj.ReferenceParameters)
            {
                Reference(node, parameter.Value);
            }

            return true;
        }

        bool VisitKeyFrameAnimation<V>(KeyFrameAnimation<V> obj, T node)
        {
            VisitCompositionAnimation(obj, node);
            foreach (var keyFrame in obj.KeyFrames)
            {
                Reference(node, keyFrame.Easing);
            }

            return true;
        }

        bool VisitScalarKeyFrameAnimation(ScalarKeyFrameAnimation obj, T node)
        {
            return VisitKeyFrameAnimation(obj, node);
        }

        bool VisitCompositionShape(CompositionShape obj, T node)
        {
            return VisitCompositionObject(obj, node);
        }

        bool VisitCompositionSpriteShape(CompositionSpriteShape obj, T node)
        {
            VisitCompositionShape(obj, node);
            Reference(node, obj.FillBrush);
            Reference(node, obj.Geometry);
            Reference(node, obj.StrokeBrush);
            return true;
        }

        bool VisitCompositionViewBox(CompositionViewBox obj, T node)
        {
            return VisitCompositionObject(obj, node);
        }

        bool VisitCompositionGeometry(CompositionGeometry obj, T node)
        {
            return VisitCompositionObject(obj, node);
        }

        bool VisitCompositionEllipseGeometry(CompositionEllipseGeometry obj, T node)
        {
            return VisitCompositionGeometry(obj, node);
        }


        bool VisitCompositionPathGeometry(CompositionPathGeometry obj, T node)
        {
            VisitCompositionGeometry(obj, node);
            Reference(node, obj.Path);
            return true;
        }

        bool VisitCompositionBrush(CompositionBrush obj, T node)
        {
            return VisitCompositionObject(obj, node);
        }

        bool VisitColorKeyFrameAnimation(ColorKeyFrameAnimation obj, T node)
        {
            return VisitKeyFrameAnimation(obj, node);
        }

        bool VisitCompositionColorBrush(CompositionColorBrush obj, T node)
        {
            return VisitCompositionBrush(obj, node);
        }

        bool VisitCompositionContainerShape(CompositionContainerShape obj, T node)
        {
            VisitCompositionShape(obj, node);
            foreach (var shape in obj.Shapes)
            {
                Reference(node, shape);
            }
            return true;
        }

        bool VisitVisual(Visual obj, T node)
        {
            VisitCompositionObject(obj, node);
            Reference(node, obj.Clip);
            return true;
        }

        bool VisitShapeVisual(ShapeVisual obj, T node)
        {
            VisitVisual(obj, node);
            Reference(node, obj.ViewBox);
            foreach (var shape in obj.Shapes)
            {
                Reference(node, shape);
            }
            return true;
        }

        bool VisitStepEasingFunction(StepEasingFunction obj, T node)
        {
            return VisitCompositionEasingFunction(obj, node);
        }

        bool VisitVector2KeyFrameAnimation(Vector2KeyFrameAnimation obj, T node)
        {
            return VisitKeyFrameAnimation(obj, node);
        }

        bool VisitVector3KeyFrameAnimation(Vector3KeyFrameAnimation obj, T node)
        {
            return VisitKeyFrameAnimation(obj, node);
        }

        bool VisitContainerVisual(ContainerVisual obj, T node)
        {
            VisitVisual(obj, node);
            foreach (var child in obj.Children)
            {
                Reference(node, child);
            }
            return true;
        }

    }
}
