// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Keyframe
{
    internal class MaskKeyframeAnimation
    {
        private readonly List<IBaseKeyframeAnimation<ShapeData, Path>> _maskAnimations;
        private readonly List<IBaseKeyframeAnimation<int?, int?>> _opacityAnimations;

        internal MaskKeyframeAnimation(List<Mask> masks)
        {
            Masks = masks;
            _maskAnimations = new List<IBaseKeyframeAnimation<ShapeData, Path>>(masks.Count);
            _opacityAnimations = new List<IBaseKeyframeAnimation<int?, int?>>(masks.Count);
            for (var i = 0; i < masks.Count; i++)
            {
                _maskAnimations.Add(masks[i].MaskPath.CreateAnimation());
                var opacity = masks[i].Opacity;
                _opacityAnimations.Add(opacity.CreateAnimation());
            }
        }

        internal virtual List<Mask> Masks { get; }

        internal virtual List<IBaseKeyframeAnimation<ShapeData, Path>> MaskAnimations => _maskAnimations;

        internal virtual List<IBaseKeyframeAnimation<int?, int?>> OpacityAnimations => _opacityAnimations;
    }
}