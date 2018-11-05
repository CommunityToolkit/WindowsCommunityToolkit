// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class SolidLayer : Layer
    {
        public SolidLayer(
            string name,
            int index,
            int? parent,
            bool isHidden,
            Transform transform,
            int width,
            int height,
            Color color,
            double timeStretch,
            double startFrame,
            double inFrame,
            double outFrame,
            BlendMode blendMode,
            bool is3d,
            bool autoOrient,
            IEnumerable<Mask> masks)
            : base(
             name,
             index,
             parent,
             isHidden,
             transform,
             timeStretch,
             startFrame,
             inFrame,
             outFrame,
             blendMode,
             is3d,
             autoOrient,
             masks)
        {
            Color = color;
            Height = height;
            Width = width;
        }

        public Color Color { get; }

        public int Height { get; }

        public int Width { get; }

        public override LayerType Type => LayerType.Solid;

        public override LottieObjectType ObjectType => LottieObjectType.SolidLayer;

    }
}
