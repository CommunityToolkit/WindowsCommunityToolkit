using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    public class PropertySetReferenceNode : ReferenceNode
    {
        internal PropertySetReferenceNode(string paramName, CompositionPropertySet ps = null)
            : base(paramName, ps)
        {
        }

        // Needed for GetSpecializedReference<>()
        internal PropertySetReferenceNode()
            : base(null, null)
        {
        }

        internal CompositionPropertySet Source { get; set; }

        internal static PropertySetReferenceNode CreateTargetReference()
        {
            var node = new PropertySetReferenceNode(null);
            node.NodeType = ExpressionNodeType.TargetReference;

            return node;
        }
    }
}