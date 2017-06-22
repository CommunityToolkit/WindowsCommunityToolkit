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
    public sealed class NineGridBrushReferenceNode : ReferenceNode
    {
        internal NineGridBrushReferenceNode(string paramName, CompositionNineGridBrush brush = null)
            : base(paramName, brush)
        {
        }

        internal static NineGridBrushReferenceNode CreateTargetReference()
        {
            var node = new NineGridBrushReferenceNode(null);
            node.NodeType = ExpressionNodeType.TargetReference;

            return node;
        }

        // Animatable properties
        public ScalarNode BottomInset
        {
            get { return ReferenceProperty<ScalarNode>("BottomInset"); }
        }

        public ScalarNode BottomInsetScale
        {
            get { return ReferenceProperty<ScalarNode>("BottomInsetScale"); }
        }

        public ScalarNode LeftInset
        {
            get { return ReferenceProperty<ScalarNode>("LeftInset"); }
        }

        public ScalarNode LeftInsetScale
        {
            get { return ReferenceProperty<ScalarNode>("LeftInsetScale"); }
        }

        public ScalarNode RightInset
        {
            get { return ReferenceProperty<ScalarNode>("RightInset"); }
        }

        public ScalarNode RightInsetScale
        {
            get { return ReferenceProperty<ScalarNode>("RightInsetScale"); }
        }

        public ScalarNode TopInset
        {
            get { return ReferenceProperty<ScalarNode>("TopInset"); }
        }

        public ScalarNode TopInsetScale
        {
            get { return ReferenceProperty<ScalarNode>("TopInsetScale"); }
        }
    }
}