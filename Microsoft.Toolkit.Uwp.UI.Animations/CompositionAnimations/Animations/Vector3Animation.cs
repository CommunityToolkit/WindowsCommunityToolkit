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
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Animation that animates a value of type <see cref="Vector3"/>
    /// </summary>
    public class Vector3Animation : TypedAnimationBase<Vector3KeyFrame, string>
    {
        /// <inheritdoc/>
        protected override KeyFrameAnimation GetTypedAnimationFromCompositor(Compositor compositor)
        {
            return compositor.CreateVector3KeyFrameAnimation();
        }

        /// <inheritdoc/>
        protected override void InsertKeyFrameToTypedAnimation(KeyFrameAnimation animation, Vector3KeyFrame keyFrame)
        {
            (animation as Vector3KeyFrameAnimation).InsertKeyFrame((float)keyFrame.Key, keyFrame.Value.ToVector3());
        }
    }
}
