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
using System.Text;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Keyframe;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Animatable
{
    internal abstract class BaseAnimatableValue<TV, TO> : IAnimatableValue<TV, TO>
    {
        private readonly List<Keyframe<TV>> _keyframes;

        internal List<Keyframe<TV>> Keyframes => _keyframes;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAnimatableValue{TV, TO}"/> class.
        /// Create a default static animatable path.
        /// </summary>
        internal BaseAnimatableValue(TV value)
            : this(new List<Keyframe<TV>> { new Keyframe<TV>(value) })
        {
        }

        internal BaseAnimatableValue(List<Keyframe<TV>> keyframes)
        {
            _keyframes = keyframes;
        }

        public abstract IBaseKeyframeAnimation<TV, TO> CreateAnimation();

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Keyframes.Count > 0)
            {
                sb.Append("values=").Append("[" + string.Join(",", Keyframes) + "]");
            }

            return sb.ToString();
        }
    }
}