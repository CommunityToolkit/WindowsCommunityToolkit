// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
    /// <summary>
    /// Copies shapes and applies a transform to the copies.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class Repeater : ShapeLayerContent
    {
        public Repeater(
            Animatable<double> count,
            Animatable<double> offset,
            RepeaterTransform transform,
            string name,
            string matchName)
            : base(name, matchName)
        {
            Count = count;
            Offset = offset;
            Transform = transform;
        }

        /// <summary>
        /// The number of copies to make.
        /// </summary>
        public Animatable<double> Count { get; }

        /// <summary>
        /// The offset of each copy.
        /// </summary>
        public Animatable<double> Offset { get; }
        
        /// <summary>
        /// The transform to apply. The transform is applied n times to the n-th copy.
        /// </summary>
        public RepeaterTransform Transform { get; }

        public override ShapeContentType ContentType => ShapeContentType.Repeater;

        public override LottieObjectType ObjectType => LottieObjectType.Repeater;
    }
}
