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
using System.Linq;
using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Keyframe;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Animatable
{
    internal class AnimatablePathValue : IAnimatableValue<Vector2?, Vector2?>
    {
        private readonly List<Keyframe<Vector2?>> _keyframes;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatablePathValue"/> class.
        /// Create a default static animatable path.
        /// </summary>
        public AnimatablePathValue()
        {
            _keyframes = new List<Keyframe<Vector2?>> { new Keyframe<Vector2?>(new Vector2(0, 0)) };
        }

        public AnimatablePathValue(List<Keyframe<Vector2?>> keyframes)
        {
            _keyframes = keyframes;
        }

        public IBaseKeyframeAnimation<Vector2?, Vector2?> CreateAnimation()
        {
            if (_keyframes[0].Static)
            {
                return new PointKeyframeAnimation(_keyframes);
            }

            return new PathKeyframeAnimation(_keyframes.ToList());
        }
    }
}