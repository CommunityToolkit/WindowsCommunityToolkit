///---------------------------------------------------------------------------------------------------------------------
/// <copyright company="Microsoft">
///     Copyright (c) Microsoft Corporation.  All rights reserved.
/// </copyright>
///---------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    using Windows.UI.Composition;

    public class PropertySetReferenceNode : ReferenceNode
    {
        internal PropertySetReferenceNode(string paramName, CompositionPropertySet ps = null) : base(paramName, ps) { }


        // Needed for GetSpecializedReference<>()
        internal PropertySetReferenceNode() : base(null, null) { }
        internal CompositionPropertySet Source { get; set; }

        internal static PropertySetReferenceNode CreateTargetReference()
        {
            var node = new PropertySetReferenceNode(null);
            node._nodeType = ExpressionNodeType.TargetReference;

            return node;
        }
    }
}