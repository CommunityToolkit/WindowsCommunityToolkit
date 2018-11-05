// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class PreCompLayer : Layer
    {
        public PreCompLayer(
            string name,
            int index,
            int? parent,
            bool isHidden,
            Transform transform,
            double timeStretch,
            double startFrame,
            double inFrame,
            double outFrame,
            BlendMode blendMode,
            bool is3d,
            bool autoOrient,
            string refId,
            double width,
            double height,
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
            RefId = refId;
            Width = width;
            Height = height;
        }


        /// <summary>
        /// The id of an <see cref="Asset"/> that contains the <see cref="Layers"/> under this <see cref="PreCompLayer"/>.
        /// </summary>
        public string RefId { get; }
        public double Width { get; }
        public double Height { get; }

        public override LayerType Type => LayerType.PreComp;

        public override LottieObjectType ObjectType => LottieObjectType.PreCompLayer;
    }
}
