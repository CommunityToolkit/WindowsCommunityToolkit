using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    public sealed class ColorBrushReferenceNode : ReferenceNode
    {
        internal ColorBrushReferenceNode(string paramName, CompositionColorBrush brush = null) : base(paramName, brush) { }
        
        internal static ColorBrushReferenceNode CreateTargetReference()
        {
            var node = new ColorBrushReferenceNode(null);
            node._nodeType = ExpressionNodeType.TargetReference;

            return node;
        }

        // Animatable properties
        public ColorNode Color { get { return ReferenceProperty<ColorNode>("Color"); } }
    }
}