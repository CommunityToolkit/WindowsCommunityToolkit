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
    public sealed class ManipulationPropertySetReferenceNode : PropertySetReferenceNode
    {
        internal ManipulationPropertySetReferenceNode(string paramName, CompositionPropertySet ps = null)
            : base(paramName, ps)
        {
        }

        // Needed for GetSpecializedReference<>
        internal ManipulationPropertySetReferenceNode()
            : base(null, null)
        {
        }

        // Animatable properties
        public Vector3Node CenterPoint
        {
            get { return ReferenceProperty<Vector3Node>("CenterPoint"); }
        }

        public Vector3Node Pan
        {
            get { return ReferenceProperty<Vector3Node>("Pan"); }
        }

        public Vector3Node Scale
        {
            get { return ReferenceProperty<Vector3Node>("Scale"); }
        }

        public Vector3Node Translation
        {
            get { return ReferenceProperty<Vector3Node>("Translation"); }
        }

        public Matrix4x4Node Matrix
        {
            get { return ReferenceProperty<Matrix4x4Node>("Matrix"); }
        }
    }
}