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
using System.IO;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    /// <summary>
    /// This view will load, deserialize, and display an After Effects animation exported with
    /// bodymovin (https://github.com/bodymovin/bodymovin).
    /// <para>
    /// You may set the animation in one of two ways:
    /// 1) Attrs: <seealso cref="SourceProperty"/>
    /// 2) Programatically: <seealso cref="SetAnimationAsync(string)"/>, <seealso cref="Composition"/>,
    /// or <seealso cref="SetAnimationAsync(TextReader)"/>.
    /// </para>
    /// <para>
    /// You can set a default cache strategy with <seealso cref="CacheStrategy.None"/>.
    /// </para>
    /// <para>
    /// You can manually set the progress of the animation with <seealso cref="Progress"/> or
    /// <seealso cref="Progress"/>
    /// </para>
    /// </summary>
    public partial class LottieAnimationView
    {
        /// <summary>
        /// This event will be invoked whenever the frame of the animation changes.
        /// </summary>
        public event EventHandler<ValueAnimator.ValueAnimatorUpdateEventArgs> AnimatorUpdate
        {
            add => _lottieDrawable.AnimatorUpdate += value;
            remove => _lottieDrawable.AnimatorUpdate -= value;
        }

        /// <summary>
        /// Clears the <seealso cref="AnimatorUpdate"/> event handler.
        /// </summary>
        public void RemoveAllUpdateListeners()
        {
            _lottieDrawable.RemoveAllUpdateListeners();
        }

        /// <summary>
        /// This event will be invoked whenever the internal animator is executed.
        /// </summary>
        public event EventHandler ValueChanged
        {
            add => _lottieDrawable.ValueChanged += value;
            remove => _lottieDrawable.ValueChanged -= value;
        }

        /// <summary>
        /// Clears the <seealso cref="ValueChanged"/> event handler.
        /// </summary>
        public void RemoveAllAnimatorListeners()
        {
            _lottieDrawable.RemoveAllAnimatorListeners();
        }
    }
}
