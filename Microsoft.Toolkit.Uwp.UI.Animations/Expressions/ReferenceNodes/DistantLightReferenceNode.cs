using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    public sealed class DistantLightReferenceNode : ReferenceNode
    {
        internal DistantLightReferenceNode(string paramName, DistantLight light = null)
            : base(paramName, light)
        {
        }

        internal static DistantLightReferenceNode CreateTargetReference()
        {
            var node = new DistantLightReferenceNode(null);
            node.NodeType = ExpressionNodeType.TargetReference;

            return node;
        }

        // Animatable properties
        public ColorNode Color { get { return ReferenceProperty<ColorNode>("Color"); } }
        public Vector3Node Direction { get { return ReferenceProperty<Vector3Node>("Direction"); } }
    }
}