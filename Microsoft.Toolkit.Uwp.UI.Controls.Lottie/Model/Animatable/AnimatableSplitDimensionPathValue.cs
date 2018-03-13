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

using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Keyframe;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Animatable
{
    internal class AnimatableSplitDimensionPathValue : IAnimatableValue<Vector2?, Vector2?>
    {
        private readonly AnimatableFloatValue _animatableXDimension;
        private readonly AnimatableFloatValue _animatableYDimension;

        public AnimatableSplitDimensionPathValue(AnimatableFloatValue animatableXDimension, AnimatableFloatValue animatableYDimension)
        {
            _animatableXDimension = animatableXDimension;
            _animatableYDimension = animatableYDimension;
        }

        public IBaseKeyframeAnimation<Vector2?, Vector2?> CreateAnimation()
        {
            return new SplitDimensionPathKeyframeAnimation(_animatableXDimension.CreateAnimation(), _animatableYDimension.CreateAnimation());
        }
    }
}