// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class CompositionEllipseGeometry : CompositionGeometry
    {
        internal CompositionEllipseGeometry() { }
        public Vector2 Center { get; set; }
        public Vector2 Radius { get; set; }

        public override CompositionObjectType Type => CompositionObjectType.CompositionEllipseGeometry;
    }
}
