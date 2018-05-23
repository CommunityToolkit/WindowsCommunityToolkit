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
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Defines <see cref="AnimationTask"/> which is used by
    /// <see cref="AnimationSet"/> to run animations that require
    /// asynchronous initialization
    /// </summary>
    internal class AnimationTask
    {
        /// <summary>
        /// Gets or sets <see cref="Task"/> that will run before any animation
        /// and it will add the animation to the AnimationSet once complete
        /// </summary>
        public Task Task { get; set; }

        /// <summary>
        /// Gets or sets <see cref="AnimationSet"/> that will run the animation
        /// </summary>
        public AnimationSet AnimationSet { get; set; }

        /// <summary>
        /// Gets or sets Duration to be applied to the animation once the task is completed
        /// Used when Duration is changed before Task completes
        /// </summary>
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// Gets or sets Delay to be applied to the animation once the task is completed
        /// Used when Duration is changed before Task completes
        /// </summary>
        public TimeSpan? Delay { get; set; }
    }
}
