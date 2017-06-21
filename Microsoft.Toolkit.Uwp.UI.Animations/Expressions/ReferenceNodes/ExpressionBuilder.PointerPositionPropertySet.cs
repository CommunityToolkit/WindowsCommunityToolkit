///---------------------------------------------------------------------------------------------------------------------
/// <copyright company="Microsoft">
///     Copyright (c) Microsoft Corporation.  All rights reserved.
/// </copyright>
///---------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    using Windows.UI.Composition;

    public sealed class PointerPositionPropertySetReferenceNode : PropertySetReferenceNode
    {
        internal PointerPositionPropertySetReferenceNode(string paramName, CompositionPropertySet ps = null) : base(paramName, ps) { }

        // Needed for GetSpecializedReference<>
        internal PointerPositionPropertySetReferenceNode() : base(null, null) { }

        // Animatable properties
        public Vector3Node Position { get { return ReferenceProperty<Vector3Node>("Position"); } }
    }
}