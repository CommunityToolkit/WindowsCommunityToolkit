// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    public sealed class SpotLightReferenceNode : ReferenceNode
    {
        internal SpotLightReferenceNode(string paramName, SpotLight light = null)
            : base(paramName, light)
        {
        }

        internal static SpotLightReferenceNode CreateTargetReference()
        {
            var node = new SpotLightReferenceNode(null);
            node.NodeType = ExpressionNodeType.TargetReference;

            return node;
        }

        // Animatable properties
        public ScalarNode ConstantAttenuation
        {
            get { return ReferenceProperty<ScalarNode>("ConstantAttenuation"); }
        }

        public ScalarNode LinearAttenuation
        {
            get { return ReferenceProperty<ScalarNode>("LinearAttenuation"); }
        }

        public ScalarNode QuadraticAttentuation
        {
            get { return ReferenceProperty<ScalarNode>("QuadraticAttentuation"); }
        }

        public ScalarNode InnerConeAngle
        {
            get { return ReferenceProperty<ScalarNode>("InnerConeAngle"); }
        }

        public ScalarNode InnerConeAngleInDegrees
        {
            get { return ReferenceProperty<ScalarNode>("InnerConeAngleInDegrees"); }
        }

        public ScalarNode OuterConeAngle
        {
            get { return ReferenceProperty<ScalarNode>("OuterConeAngle"); }
        }

        public ScalarNode OuterConeAngleInDegrees
        {
            get { return ReferenceProperty<ScalarNode>("OuterConeAngleInDegrees"); }
        }

        public ColorNode Color
        {
            get { return ReferenceProperty<ColorNode>("Color"); }
        }

        public ColorNode InnerConeColor
        {
            get { return ReferenceProperty<ColorNode>("InnerConeColor"); }
        }

        public ColorNode OuterConeColor
        {
            get { return ReferenceProperty<ColorNode>("OuterConeColor"); }
        }

        public Vector3Node Direction
        {
            get { return ReferenceProperty<Vector3Node>("Direction"); }
        }

        public Vector3Node Offset
        {
            get { return ReferenceProperty<Vector3Node>("Offset"); }
        }
    }
}