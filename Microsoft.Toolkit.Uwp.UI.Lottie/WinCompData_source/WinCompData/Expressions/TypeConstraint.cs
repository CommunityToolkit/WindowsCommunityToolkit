// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.


using System;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Expressions
{
    /// <summary>
    /// Types to which an expression or variable is constrained.
    /// </summary>
    [Flags]
#if !WINDOWS_UWP
    public
#endif
    enum TypeConstraint
    {
        NoType = 0,
        Boolean = 1 << 0,
        Scalar = 1 << 1,
        Vector2 = 1 << 2,
        Vector3 = 1 << 3,
        Vector4 = 1 << 4,
        Matrix3x2 = 1 << 5,
        Matrix4x4 = 1 << 6,
        Quaternion = 1 << 7,
        Color = 1 << 8,
        AllValidTypes = Boolean | Scalar | Vector2 | Vector3 | Vector4 | Matrix3x2 | Matrix4x4 | Quaternion | Color,
    }
}
