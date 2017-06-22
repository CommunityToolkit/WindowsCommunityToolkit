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
    public sealed class DropShadowReferenceNode : ReferenceNode
    {
        internal DropShadowReferenceNode(string paramName, DropShadow source = null)
            : base(paramName, source)
        {
        }

        internal static DropShadowReferenceNode CreateTargetReference()
        {
            var node = new DropShadowReferenceNode(null);
            node.NodeType = ExpressionNodeType.TargetReference;

            return node;
        }

        // Animatable properties
        public ScalarNode BlurRadius
        {
            get { return ReferenceProperty<ScalarNode>("BlurRadius"); }
        }

        public ScalarNode Opacity
        {
            get { return ReferenceProperty<ScalarNode>("Opacity"); }
        }

        public Vector3Node Offset
        {
            get { return ReferenceProperty<Vector3Node>("Offset"); }
        }

        public ColorNode Color
        {
            get { return ReferenceProperty<ColorNode>("Color"); }
        }
    }
}