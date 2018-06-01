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
    /// <summary>
    /// Class PropertySetReferenceNode.
    /// </summary>
    /// <seealso cref="Microsoft.Toolkit.Uwp.UI.Animations.Expressions.ReferenceNode" />
    public class PropertySetReferenceNode : ReferenceNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySetReferenceNode"/> class.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="ps">The ps.</param>
        internal PropertySetReferenceNode(string paramName, CompositionPropertySet ps = null)
            : base(paramName, ps)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySetReferenceNode"/> class.
        /// </summary>
        // Needed for GetSpecializedReference<>()
        internal PropertySetReferenceNode()
            : base(null, null)
        {
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        internal CompositionPropertySet Source { get; set; }

        /// <summary>
        /// Creates the target reference.
        /// </summary>
        /// <returns>PropertySetReferenceNode.</returns>
        internal static PropertySetReferenceNode CreateTargetReference()
        {
            var node = new PropertySetReferenceNode(null);
            node.NodeType = ExpressionNodeType.TargetReference;

            return node;
        }
    }
}