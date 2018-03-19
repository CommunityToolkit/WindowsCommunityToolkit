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

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    /// <summary>
    /// Base abstract class that provides basic animation functionality
    /// </summary>
    public abstract class Animator
    {
        /// <summary>
        /// Gets or sets the total duration os the animation
        /// </summary>
        public virtual long Duration { get; set; }

        /// <summary>
        /// Gets a value indicating whether the animation is currently running
        /// </summary>
        public abstract bool IsRunning { get; }

        /// <summary>
        /// Cancels the animation that is being executed
        /// </summary>
        public virtual void Cancel()
        {
            AnimationCanceled();
        }

        /// <summary>
        /// Invoked whenever the animation is canceled
        /// </summary>
        protected virtual void AnimationCanceled()
        {
        }
    }
}