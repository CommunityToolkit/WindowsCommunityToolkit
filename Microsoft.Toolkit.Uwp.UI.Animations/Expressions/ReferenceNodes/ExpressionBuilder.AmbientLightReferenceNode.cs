///---------------------------------------------------------------------------------------------------------------------
/// <copyright company="Microsoft">
///     Copyright (c) Microsoft Corporation.  All rights reserved.
/// </copyright>
///---------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    using Windows.UI.Composition;

    public sealed class AmbientLightReferenceNode : ReferenceNode
    {
        internal AmbientLightReferenceNode(string paramName, AmbientLight light = null) : base(paramName, light) { }
        
        internal static AmbientLightReferenceNode CreateTargetReference()
        {
            var node = new AmbientLightReferenceNode(null);
            node._nodeType = ExpressionNodeType.TargetReference;

            return node;
        }
        
        // Animatable properties
        public ColorNode Color { get { return ReferenceProperty<ColorNode>("Color"); } }
    }
}