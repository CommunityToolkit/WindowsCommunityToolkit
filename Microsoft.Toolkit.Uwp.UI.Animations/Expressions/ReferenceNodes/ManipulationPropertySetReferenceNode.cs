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
    /// Class ManipulationPropertySetReferenceNode. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Microsoft.Toolkit.Uwp.UI.Animations.Expressions.PropertySetReferenceNode" />
    public sealed class ManipulationPropertySetReferenceNode : PropertySetReferenceNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManipulationPropertySetReferenceNode"/> class.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="ps">The ps.</param>
        internal ManipulationPropertySetReferenceNode(string paramName, CompositionPropertySet ps = null)
            : base(paramName, ps)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManipulationPropertySetReferenceNode"/> class.
        /// Needed for GetSpecializedReference
        /// </summary>
        internal ManipulationPropertySetReferenceNode()
            : base(null, null)
        {
        }

        /// <summary>
        /// Gets the center point.
        /// </summary>
        /// <value>The center point.</value>
        public Vector3Node CenterPoint
        {
            get { return ReferenceProperty<Vector3Node>("CenterPoint"); }
        }

        /// <summary>
        /// Gets the pan.
        /// </summary>
        /// <value>The pan.</value>
        public Vector3Node Pan
        {
            get { return ReferenceProperty<Vector3Node>("Pan"); }
        }

        /// <summary>
        /// Gets the scale.
        /// </summary>
        /// <value>The scale.</value>
        public Vector3Node Scale
        {
            get { return ReferenceProperty<Vector3Node>("Scale"); }
        }

        /// <summary>
        /// Gets the translation.
        /// </summary>
        /// <value>The translation.</value>
        public Vector3Node Translation
        {
            get { return ReferenceProperty<Vector3Node>("Translation"); }
        }

        /// <summary>
        /// Gets the matrix.
        /// </summary>
        /// <value>The matrix.</value>
        public Matrix4x4Node Matrix
        {
            get { return ReferenceProperty<Matrix4x4Node>("Matrix"); }
        }
    }
}