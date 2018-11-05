// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class CompositionSpriteShape : CompositionShape
    {
        internal CompositionSpriteShape() { }

        public CompositionBrush FillBrush { get; set; }
        public CompositionGeometry Geometry { get; set; }
        public bool IsStrokeNonScaling { get; set; }
        public CompositionBrush StrokeBrush { get; set; }
        public CompositionStrokeCap StrokeDashCap { get; set; } = CompositionStrokeCap.Flat;
        public float StrokeDashOffset { get; set; }
        public List<float> StrokeDashArray { get; } = new List<float>();
        public CompositionStrokeCap StrokeEndCap { get; set; } = CompositionStrokeCap.Flat;
        public CompositionStrokeLineJoin StrokeLineJoin { get; set; } = CompositionStrokeLineJoin.Miter;
        public CompositionStrokeCap StrokeStartCap { get; set; } = CompositionStrokeCap.Flat;
        public float StrokeMiterLimit { get; set; } = 1;
        public float StrokeThickness { get; set; } = 1;
        public override CompositionObjectType Type => CompositionObjectType.CompositionSpriteShape;

    }
}
