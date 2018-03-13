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

using System;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Keyframe
{
    /// <summary>
    /// Internal interface that indicates basic keyframe animations properties
    /// </summary>
    public interface IBaseKeyframeAnimation
    {
        /// <summary>
        /// Gets or sets the current progress of this animation
        /// </summary>
        float Progress { get; set; }

        /// <summary>
        /// This event is invoked whenever a value of this animation changed
        /// </summary>
        event EventHandler ValueChanged;

        /// <summary>
        /// Method that should invoke the ValueChanged event, whenever a value of this animation changes
        /// </summary>
        void OnValueChanged();
    }

    internal interface IBaseKeyframeAnimation<out TK, TA> : IBaseKeyframeAnimation
    {
        TA Value { get; }

        void SetValueCallback(ILottieValueCallback<TA> valueCallback);
    }
}