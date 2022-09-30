// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
    [ContentProperty(Name = nameof(Configs))]
    public sealed partial class TransitionHelper
    {
        private sealed record AnimatedElements<T>(
            Dictionary<string, T> ConnectedElements,
            Dictionary<string, List<T>> CoordinatedElements,
            List<T> IndependentElements)
        {
            public IEnumerable<T> All()
            {
                return this.ConnectedElements.Values.Concat(this.IndependentElements).Concat(this.CoordinatedElements.SelectMany(item => item.Value));
            }
        }

        private const double AlmostZero = 0.01;
        private AnimatedElements<UIElement> _sourceAnimatedElements;
        private AnimatedElements<UIElement> _targetAnimatedElements;
        private CancellationTokenSource _animateCancellationTokenSource;
        private CancellationTokenSource _reverseCancellationTokenSource;
        private bool _needUpdateSourceLayout;
        private bool _needUpdateTargetLayout;

        private AnimatedElements<UIElement> SourceAnimatedElements => _sourceAnimatedElements ??= GetAnimatedElements(this.Source);

        private AnimatedElements<UIElement> TargetAnimatedElements => _targetAnimatedElements ??= GetAnimatedElements(this.Target);

        private IKeyFrameAnimationGroupController _currentAnimationGroupController;

        /// <summary>
        /// Gets a value indicating whether the source and target controls are animating.
        /// </summary>
        public bool IsAnimating => _animateCancellationTokenSource is not null || _reverseCancellationTokenSource is not null;

        /// <summary>
        /// Morphs from source control to target control.
        /// </summary>
        /// <returns>A <see cref="Task"/> that completes when all animations have completed.</returns>
        public Task StartAsync()
        {
            return StartAsync(CancellationToken.None, false);
        }

        /// <summary>
        /// Morphs from source control to target control.
        /// </summary>
        /// <param name="forceUpdateAnimatedElements">Indicates whether to force the update of the child element list before the animation starts.</param>
        /// <returns>A <see cref="Task"/> that completes when all animations have completed.</returns>
        public Task StartAsync(bool forceUpdateAnimatedElements)
        {
            return StartAsync(CancellationToken.None, forceUpdateAnimatedElements);
        }

        /// <summary>
        /// Morphs from source control to target control.
        /// </summary>
        /// <param name="token">The cancellation token to stop animations while they're running.</param>
        /// <param name="forceUpdateAnimatedElements">Indicates whether to force the update of the child element list before the animation starts.</param>
        /// <returns>A <see cref="Task"/> that completes when all animations have completed.</returns>
        public async Task StartAsync(CancellationToken token, bool forceUpdateAnimatedElements)
        {
            IsNotNullAndIsInVisualTree(this.Source, nameof(this.Source));
            IsNotNullAndIsInVisualTree(this.Target, nameof(this.Target));
            if (this._animateCancellationTokenSource is not null)
            {
                return;
            }

            if (this._reverseCancellationTokenSource is not null)
            {
                this._reverseCancellationTokenSource.Cancel();
            }
            else if (this.IsTargetState)
            {
                return;
            }
            else
            {
                this._currentAnimationGroupController = null;
                await this.InitControlsStateAsync(forceUpdateAnimatedElements);
            }

            this._animateCancellationTokenSource = new CancellationTokenSource();
            var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, this._animateCancellationTokenSource.Token);
            await this.AnimateControls(this.Duration, false, linkedTokenSource.Token);
            this._animateCancellationTokenSource = null;
            if (linkedTokenSource.Token.IsCancellationRequested)
            {
                return;
            }

            this.RestoreState(true);
        }

        /// <summary>
        /// Reverse animation, morphs from target control to source control.
        /// </summary>
        /// <returns>A <see cref="Task"/> that completes when all animations have completed.</returns>
        public Task ReverseAsync()
        {
            return ReverseAsync(CancellationToken.None, false);
        }

        /// <summary>
        /// Reverse animation, morphs from target control to source control.
        /// </summary>
        /// <param name="forceUpdateAnimatedElements">Indicates whether to force the update of child elements before the animation starts.</param>
        /// <returns>A <see cref="Task"/> that completes when all animations have completed.</returns>
        public Task ReverseAsync(bool forceUpdateAnimatedElements)
        {
            return ReverseAsync(CancellationToken.None, forceUpdateAnimatedElements);
        }

        /// <summary>
        /// Reverse animation, morphs from target control to source control.
        /// </summary>
        /// <param name="token">The cancellation token to stop animations while they're running.</param>
        /// <param name="forceUpdateAnimatedElements">Indicates whether to force the update of child elements before the animation starts.</param>
        /// <returns>A <see cref="Task"/> that completes when all animations have completed.</returns>
        public async Task ReverseAsync(CancellationToken token, bool forceUpdateAnimatedElements)
        {
            IsNotNullAndIsInVisualTree(this.Source, nameof(this.Source));
            IsNotNullAndIsInVisualTree(this.Target, nameof(this.Target));
            if (this._reverseCancellationTokenSource is not null)
            {
                return;
            }

            if (this._animateCancellationTokenSource is not null)
            {
                this._animateCancellationTokenSource.Cancel();
            }
            else if (this.IsTargetState is false)
            {
                return;
            }
            else
            {
                this._currentAnimationGroupController = null;
                await this.InitControlsStateAsync(forceUpdateAnimatedElements);
            }

            this._reverseCancellationTokenSource = new CancellationTokenSource();
            var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, this._reverseCancellationTokenSource.Token);
            await this.AnimateControls(this.ReverseDuration, true, linkedTokenSource.Token);
            this._reverseCancellationTokenSource = null;
            if (linkedTokenSource.Token.IsCancellationRequested)
            {
                return;
            }

            this.RestoreState(false);
        }

        /// <summary>
        /// Reset to initial or target state.
        /// </summary>
        /// <param name="toInitialState">Indicates whether to reset to initial state. default value is True, if it is False, it will be reset to target state.</param>
        public void Reset(bool toInitialState = true)
        {
            if (IsAnimating)
            {
                this._animateCancellationTokenSource?.Cancel();
                this._animateCancellationTokenSource = null;
                this._reverseCancellationTokenSource?.Cancel();
                this._reverseCancellationTokenSource = null;
            }

            this._currentAnimationGroupController = null;
            this.RestoreState(!toInitialState);
        }
    }
}
