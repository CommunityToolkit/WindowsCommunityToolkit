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
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Keyframe
{
    internal class PathKeyframe : Keyframe<Vector2?>
    {
        internal PathKeyframe(LottieComposition composition, Keyframe<Vector2?> keyframe)
            : base(composition, keyframe.StartValue, keyframe.EndValue, keyframe.Interpolator, keyframe.StartFrame, keyframe.EndFrame)
        {
            var equals = EndValue != null && StartValue != null && StartValue.Equals(EndValue.Value);
            if (EndValue != null && !equals)
            {
                Path = Utils.Utils.CreatePath(StartValue.Value, EndValue.Value, keyframe.PathCp1, keyframe.PathCp2);
            }
        }

        /// <summary>
        /// Gets the path of the keyframe. This will be null if the startValue and endValue are the same.
        /// </summary>
        internal Path Path { get; }
    }
}