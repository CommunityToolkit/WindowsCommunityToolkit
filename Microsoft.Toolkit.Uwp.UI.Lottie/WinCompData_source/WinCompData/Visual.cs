// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    abstract class Visual : CompositionObject
    {
        protected private Visual() { }
        public Vector3? CenterPoint { get; set; }
        public CompositionClip Clip { get; set; }
        public Vector3? Offset { get; set; }
        public float? Opacity { get; set; }
        public float? RotationAngleInDegrees { get; set; }
        public Vector3? RotationAxis { get; set; }
        public Vector3? Scale { get; set; }
        public Vector2? Size { get; set; }
        public Matrix4x4? TransformMatrix { get; set; }
    }
}
