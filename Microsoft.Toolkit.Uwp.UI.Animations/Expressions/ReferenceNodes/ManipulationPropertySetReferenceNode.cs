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