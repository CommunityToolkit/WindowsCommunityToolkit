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