// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class CompositionRectangleGeometry : CompositionGeometry
    {
        internal CompositionRectangleGeometry() { }

        public Vector2? Offset { get; set; }

        public Vector2 Size { get; set; }

        public override CompositionObjectType Type => CompositionObjectType.CompositionRectangleGeometry;
    }
}
