// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
    /// <summary>
    /// An animatable Vector3 value expressed as 3 animatable floating point values.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class AnimatableXYZ : IAnimatableVector3
    {
        public AnimatableXYZ(Animatable<double> x, Animatable<double> y, Animatable<double> z)
        {
            InitialValue = new Vector3(x.InitialValue, y.InitialValue, z.InitialValue);
            X = x;
            Y = y;
            Z = z;
        }

        public AnimatableVector3Type Type => AnimatableVector3Type.XYZ;

        public Vector3 InitialValue { get; }
        public Animatable<double> X { get; }
        public Animatable<double> Y { get; }
        public Animatable<double> Z { get; }

        public bool IsAnimated => X.IsAnimated || Y.IsAnimated || Z.IsAnimated;
    }
}
