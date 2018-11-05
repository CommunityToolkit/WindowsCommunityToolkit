// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Toolkit.Uwp.UI.Lottie.LottieData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieToWinComp
{
    sealed class TranslationIssues
    {

        readonly HashSet<(string Code, string Description)> _issues = new HashSet<(string, string)>();
        readonly bool _throwOnIssue;

        internal TranslationIssues(bool throwOnIssue)
        {
            _throwOnIssue = throwOnIssue;
        }

        internal (string Code, string Description)[] GetIssues() => _issues.ToArray();

        internal void AnimatedRectangleWithTrimPath()
        {
            Report("LT0001", "Rectangle with animated size and TrimPath");
        }
        internal void AnimatedTrimOffsetWithStaticTrimOffset()
        {
            Report("LT0002", "Animated trim offset with static trim offset");
        }
        internal void AnimationMultiplication()
        {
            Report("LT0003", "Multiplication of two or more animated values");
        }
        internal void BlendMode(BlendMode blendMode)
        {
            Report("LT0004", $"Blend mode: {blendMode}");
        }
        internal void CombiningAnimatedShapes()
        {
            Report("LT0005", "Combining animated shapes");
        }
        internal void GradientFill()
        {
            Report("LT0006", "Gradient fill");
        }
        internal void GradientStroke()
        {
            Report("LT0007", "Gradient stroke");
        }
        internal void ImageAssets()
        {
            Report("LT0008", "Image assets");
        }
        internal void ImageLayer()
        {
            Report("LT0009", "Image layers");
        }
        internal void MergingALargeNumberOfShapes()
        {
            Report("LT0010", "Merging a large number of shapes");
        }
        internal void MultipleAnimatedRoundedCorners()
        {
            Report("LT0011", "Multiple animated rounded corners");
        }
        internal void MultipleFills()
        {
            Report("LT0012", "Multiple fills");
        }
        internal void MultipleStrokes()
        {
            Report("LT0013", "Multiple strokes");
        }
        internal void MultipleTrimPaths()
        {
            Report("LT0014", "Multiple trim paths");
        }
        internal void OpacityAndColorAnimatedTogether()
        {
            Report("LT0015", "Opacity and color animated at the same time");
        }
        internal void PathWithRoundedCorners()
        {
            Report("LT0016", "Path with rounded corners");
        }
        internal void Polystar()
        {
            Report("LT0017", "Polystar");
        }
        internal void Repeater()
        {
            Report("LT0018", "Repeater");
        }
        internal void TextLayer()
        {
            Report("LT0019", "Text layer");
        }
        internal void ThreeD()
        {
            Report("LT0020", "3d composition");
        }
        internal void ThreeDLayer()
        {
            Report("LT0021", "3d layer");
        }
        internal void TimeStretch()
        {
            Report("LT0022", "Time stretch");
        }
        internal void MaskWithInvert()
        {
            Report("LT0023", "Mask with invert");
        }
        internal void MaskWithUnsupportedMode(Mask.MaskMode mode)
        {
            Report("LT0024", $"Mask mode: {mode}");
        }
        internal void MaskWithAlpha()
        {
            Report("LT0025", "Mask with alpha value other than 1");
        }
        internal void MultipleShapeMasks()
        {
            Report("LT0026", "Mask with multiple shapes");
        }

        void Report(string code, string description)
        {
            _issues.Add((code, description));

            if (_throwOnIssue)
            {
                throw new NotSupportedException($"{code}: {description}");
            }
        }

    }
}
