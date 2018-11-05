// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class CubicBezierEasingFunction : CompositionEasingFunction
    {
        internal CubicBezierEasingFunction(Vector2 controlPoint1, Vector2 controlPoint2) {
            ControlPoint1 = controlPoint1;
            ControlPoint2 = controlPoint2;
        }

        public Vector2 ControlPoint1 { get; }
        public Vector2 ControlPoint2 { get; }

        public override CompositionObjectType Type => CompositionObjectType.CubicBezierEasingFunction;
    }
}
