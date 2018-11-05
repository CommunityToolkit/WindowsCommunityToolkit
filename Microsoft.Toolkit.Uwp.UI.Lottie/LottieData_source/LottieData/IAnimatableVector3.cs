// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    interface IAnimatableVector3 : IAnimatableValue<Vector3>
    {
        AnimatableVector3Type Type { get; }
    }

#if !WINDOWS_UWP
    public
#endif
    enum AnimatableVector3Type
    {
        Vector3,
        XYZ,
    }
}
