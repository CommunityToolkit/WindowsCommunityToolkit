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
    /// Class ColorBrushReferenceNode. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Microsoft.Toolkit.Uwp.UI.Animations.Expressions.ReferenceNode" />
    public sealed class ColorBrushReferenceNode : ReferenceNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorBrushReferenceNode"/> class.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="brush">The brush.</param>
        internal ColorBrushReferenceNode(string paramName, CompositionColorBrush brush = null)
            : base(paramName, brush)
        {
        }

        /// <summary>
        /// Creates the target reference.
        /// </summary>
        /// <returns>ColorBrushReferenceNode.</returns>
        internal static ColorBrushReferenceNode CreateTargetReference()
        {
            var node = new ColorBrushReferenceNode(null);
            node.NodeType = ExpressionNodeType.TargetReference;

            return node;
        }

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <value>The color.</value>
        public ColorNode Color
        {
            get { return ReferenceProperty<ColorNode>("Color"); }
        }
    }
}