// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly TimeSpan almostZeroDuration = TimeSpan.FromMilliseconds(1);

        private CancellationTokenSource _animateCancellationTokenSource;
        private CancellationTokenSource _reverseCancellationTokenSource;
        private TaskCompletionSource<object> _animateTaskSource;
        private TaskCompletionSource<object> _reverseTaskSource;
        private bool _needUpdateSourceLayout = false;
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
            await StartInterruptibleAnimationsAsync(false, _animateCancellationTokenSource.Token, forceUpdateAnimatedElements);
            this._animateCancellationTokenSource = null;
        }

        /// <summary>
        /// Reverse animation, morphs from target control to source control.
        /// </summary>
        /// <param name="forceUpdateAnimatedElements">Indicates whether to force the update of child elements before the animation starts.</param>
        /// <returns>A <see cref="Task"/> that completes when all animations have completed.</returns>
        public async Task ReverseAsync(bool forceUpdateAnimatedElements = false)
        {
            if (this._reverseCancellationTokenSource is not null)
            {
                return;
            }

            this._reverseCancellationTokenSource = new CancellationTokenSource();
            await StartInterruptibleAnimationsAsync(true, _reverseCancellationTokenSource.Token, forceUpdateAnimatedElements);
            this._reverseCancellationTokenSource = null;
        }

        /// <summary>
        /// Reset to initial state.
        /// </summary>
        /// <param name="forceRestoreChildElements">Indicates whether to force the reset of child elements.</param>
        public void Reset(bool forceRestoreChildElements = false)
        {
            var needRestoreChildElements = forceRestoreChildElements || this.IsTargetState;
            if (this._animateCancellationTokenSource is not null)
            {
                needRestoreChildElements = true;
                this._animateCancellationTokenSource.Cancel();
                this._animateCancellationTokenSource = null;
            }

            if (_reverseCancellationTokenSource is not null)
            {
                needRestoreChildElements = true;
                this._reverseCancellationTokenSource.Cancel();
                this._reverseCancellationTokenSource = null;
            }

            this._isInterruptedAnimation = false;
            this.RestoreState(false);

            if (needRestoreChildElements)
            {
                this.RestoreUIElements(this.sourceAnimatedElements.Values.Concat(GetIgnoredElements(Source)));
                this.RestoreUIElements(this.targetAnimatedElements.Values.Concat(GetIgnoredElements(Target)));
            }
        }
    }
}
