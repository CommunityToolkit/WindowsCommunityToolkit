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
    /// Class NineGridBrushReferenceNode. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Microsoft.Toolkit.Uwp.UI.Animations.Expressions.ReferenceNode" />
    public sealed class NineGridBrushReferenceNode : ReferenceNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NineGridBrushReferenceNode"/> class.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="brush">The brush.</param>
        internal NineGridBrushReferenceNode(string paramName, CompositionNineGridBrush brush = null)
            : base(paramName, brush)
        {
        }

        /// <summary>
        /// Creates the target reference.
        /// </summary>
        /// <returns>NineGridBrushReferenceNode.</returns>
        internal static NineGridBrushReferenceNode CreateTargetReference()
        {
            var node = new NineGridBrushReferenceNode(null);
            node.NodeType = ExpressionNodeType.TargetReference;

            return node;
        }

        /// <summary>
        /// Gets the bottom inset.
        /// </summary>
        /// <value>The bottom inset.</value>
        public ScalarNode BottomInset
        {
            get { return ReferenceProperty<ScalarNode>("BottomInset"); }
        }

        /// <summary>
        /// Gets the bottom inset scale.
        /// </summary>
        /// <value>The bottom inset scale.</value>
        public ScalarNode BottomInsetScale
        {
            get { return ReferenceProperty<ScalarNode>("BottomInsetScale"); }
        }

        /// <summary>
        /// Gets the left inset.
        /// </summary>
        /// <value>The left inset.</value>
        public ScalarNode LeftInset
        {
            get { return ReferenceProperty<ScalarNode>("LeftInset"); }
        }

        /// <summary>
        /// Gets the left inset scale.
        /// </summary>
        /// <value>The left inset scale.</value>
        public ScalarNode LeftInsetScale
        {
            get { return ReferenceProperty<ScalarNode>("LeftInsetScale"); }
        }

        /// <summary>
        /// Gets the right inset.
        /// </summary>
        /// <value>The right inset.</value>
        public ScalarNode RightInset
        {
            get { return ReferenceProperty<ScalarNode>("RightInset"); }
        }

        /// <summary>
        /// Gets the right inset scale.
        /// </summary>
        /// <value>The right inset scale.</value>
        public ScalarNode RightInsetScale
        {
            get { return ReferenceProperty<ScalarNode>("RightInsetScale"); }
        }

        /// <summary>
        /// Gets the top inset.
        /// </summary>
        /// <value>The top inset.</value>
        public ScalarNode TopInset
        {
            get { return ReferenceProperty<ScalarNode>("TopInset"); }
        }

        /// <summary>
        /// Gets the top inset scale.
        /// </summary>
        /// <value>The top inset scale.</value>
        public ScalarNode TopInsetScale
        {
            get { return ReferenceProperty<ScalarNode>("TopInsetScale"); }
        }
    }
}