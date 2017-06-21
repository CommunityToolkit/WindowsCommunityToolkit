using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    public sealed class VisualReferenceNode : ReferenceNode
    {
        internal VisualReferenceNode(string paramName, Visual v = null) : base(paramName, v) { }

        internal static VisualReferenceNode CreateTargetReference()
        {
            var node = new VisualReferenceNode(null);
            node._nodeType = ExpressionNodeType.TargetReference;

            return node;
        }

        // Animatable properties
        public ScalarNode     Opacity                { get { return ReferenceProperty<ScalarNode>("Opacity");                } }
        public ScalarNode     RotationAngle          { get { return ReferenceProperty<ScalarNode>("RotationAngle");          } }
        public ScalarNode     RotationAngleInDegrees { get { return ReferenceProperty<ScalarNode>("RotationAngleInDegrees"); } }

        public Vector2Node    AnchorPoint            { get { return ReferenceProperty<Vector2Node>("AnchorPoint");           } }
        public Vector2Node    RelativeSize           { get { return ReferenceProperty<Vector2Node>("RelativeSize");          } }
        public Vector2Node    Size                   { get { return ReferenceProperty<Vector2Node>("Size");                  } }

        public Vector3Node    CenterPoint            { get { return ReferenceProperty<Vector3Node>("CenterPoint");           } }
        public Vector3Node    Offset                 { get { return ReferenceProperty<Vector3Node>("Offset");                } }
        public Vector3Node    RelativeOffset         { get { return ReferenceProperty<Vector3Node>("RelativeOffset");        } }
        public Vector3Node    RotationAxis           { get { return ReferenceProperty<Vector3Node>("RotationAxis");          } }
        public Vector3Node    Scale                  { get { return ReferenceProperty<Vector3Node>("Scale");                 } }

        public QuaternionNode Orientation            { get { return ReferenceProperty<QuaternionNode>("Orientation");        } }

        public Matrix4x4Node  TransformMatrix        { get { return ReferenceProperty<Matrix4x4Node>("TransformMatrix");     } }
    }
}