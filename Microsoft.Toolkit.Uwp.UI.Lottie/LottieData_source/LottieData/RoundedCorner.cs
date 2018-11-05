// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class RoundedCorner : ShapeLayerContent
    {
        public RoundedCorner(
            string name,
            string matchName,
            Animatable<double> radius)
            : base(name, matchName)
        {
            Radius = radius;
        }

        public Animatable<double> Radius { get; }

        public override ShapeContentType ContentType => ShapeContentType.RoundedCorner;

        public override LottieObjectType ObjectType => LottieObjectType.RoundedCorner;
    }
}
