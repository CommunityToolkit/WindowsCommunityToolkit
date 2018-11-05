// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class TextLayer : Layer
    {
        public TextLayer(
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
        }

        /// <summary>
        /// The id of an <see cref="Asset"/> referenced by this layer.
        /// </summary>
        public string RefId { get; }

        public override LayerType Type => LayerType.Text;

        public override LottieObjectType ObjectType => LottieObjectType.TextLayer;
    }
}
