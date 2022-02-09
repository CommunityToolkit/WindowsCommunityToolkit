// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A animation helper that morphs between two controls.
    /// </summary>
    [ContentProperty(Name = nameof(AnimationConfigs))]
    public sealed partial class TransitionHelper
    {
        private readonly Dictionary<string, UIElement> sourceAnimatedElements = new();
        private readonly Dictionary<string, UIElement> targetAnimatedElements = new();
        private readonly double interruptedAnimationReverseDurationRatio = 0.7;

        private CancellationTokenSource _animateCancellationTokenSource;
        private CancellationTokenSource _reverseCancellationTokenSource;
        private TaskCompletionSource<object> _animateTaskSource;
        private TaskCompletionSource<object> _reverseTaskSource;
        private bool _needUpdateTargetLayout = false;
        private bool _isInterruptedAnimation = false;

        /// <summary>
        /// Morphs from source control to target control.
        /// </summary>
        /// <param name="forceUpdateAnimatedElements">Indicates whether to force the update of the child element list before the animation starts.</param>
        /// <returns>A <see cref="Task"/> that completes when all animations have completed.</returns>
        public async Task AnimateAsync(bool forceUpdateAnimatedElements = false)
        {
            if (this._animateCancellationTokenSource is not null)
            {
                return;
            }

            this._animateCancellationTokenSource = new CancellationTokenSource();

            if (this._reverseCancellationTokenSource is not null)
            {
                if (!this._isInterruptedAnimation)
                {
                    this._reverseCancellationTokenSource?.Cancel();
                    this._isInterruptedAnimation = true;
                }
                else
                {
                    this._isInterruptedAnimation = false;
                }

                _ = await this._reverseTaskSource.Task;
                this._reverseTaskSource = null;
            }

            if (!this.IsTargetState && !this._animateCancellationTokenSource.IsCancellationRequested)
            {
                this._animateTaskSource = new TaskCompletionSource<object>();
                await this.InitStateAsync(forceUpdateAnimatedElements);
                await this.AnimateFromSourceToTargetAsync(this._animateCancellationTokenSource.Token);
                this.RestoreState(true);
                _ = this._animateTaskSource?.TrySetResult(null);
            }

            this.IsTargetState = true;
            if (!this._animateCancellationTokenSource.IsCancellationRequested)
            {
                this._isInterruptedAnimation = false;
            }

            this._animateCancellationTokenSource = null;
        }

        /// <summary>
        /// Reverse animation, morphs from target control to source control.
        /// </summary>
        /// <param name="forceUpdateAnimatedElements">Indicates whether to force the update of the child element list before the animation starts.</param>
        /// <returns>A <see cref="Task"/> that completes when all animations have completed.</returns>
        public async Task ReverseAsync(bool forceUpdateAnimatedElements = false)
        {
            if (this._reverseCancellationTokenSource is not null)
            {
                return;
            }

            this._reverseCancellationTokenSource = new CancellationTokenSource();

            if (this._animateCancellationTokenSource is not null)
            {
                if (!this._isInterruptedAnimation)
                {
                    this._animateCancellationTokenSource?.Cancel();
                    this._isInterruptedAnimation = true;
                }
                else
                {
                    this._isInterruptedAnimation = false;
                }

                _ = await this._animateTaskSource.Task;
                this._animateTaskSource = null;
            }

            if (this.IsTargetState && !this._reverseCancellationTokenSource.IsCancellationRequested)
            {
                this._reverseTaskSource = new TaskCompletionSource<object>();
                await this.InitStateAsync(forceUpdateAnimatedElements);
                await this.AnimateFromTargetToSourceAsync(this._reverseCancellationTokenSource.Token);
                this.RestoreState(false);
                _ = this._reverseTaskSource?.TrySetResult(null);
            }

            this.IsTargetState = false;
            if (!this._reverseCancellationTokenSource.IsCancellationRequested)
            {
                this._isInterruptedAnimation = false;
            }

            this._reverseCancellationTokenSource = null;
        }
    }
}
