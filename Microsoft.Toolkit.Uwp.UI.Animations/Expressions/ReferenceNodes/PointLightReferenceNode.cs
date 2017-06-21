///---------------------------------------------------------------------------------------------------------------------
/// <copyright company="Microsoft">
///     Copyright (c) Microsoft Corporation.  All rights reserved.
/// </copyright>
///---------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    using Windows.UI.Composition;

    public sealed class PointLightReferenceNode : ReferenceNode
    {
        internal PointLightReferenceNode(string paramName, PointLight light = null) : base(paramName, light) { }
        
        internal static PointLightReferenceNode CreateTargetReference()
        {
            var node = new PointLightReferenceNode(null);
            node._nodeType = ExpressionNodeType.TargetReference;

            return node;
        }

        // Animatable properties
        public ScalarNode ConstantAttenuation   { get { return ReferenceProperty<ScalarNode>("ConstantAttenuation");   } }
        public ScalarNode LinearAttenuation     { get { return ReferenceProperty<ScalarNode>("LinearAttenuation");     } }
        public ScalarNode QuadraticAttentuation { get { return ReferenceProperty<ScalarNode>("QuadraticAttentuation"); } }

        public ColorNode Color                  { get { return ReferenceProperty<ColorNode>("Color");                  } }
        public Vector3Node Direction            { get { return ReferenceProperty<Vector3Node>("Direction");            } }
        public Vector3Node Offset               { get { return ReferenceProperty<Vector3Node>("Offset");               } }        
    }
}