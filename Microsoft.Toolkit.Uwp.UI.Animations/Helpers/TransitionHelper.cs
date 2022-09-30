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
        private CancellationTokenSource _animationCancellationTokenSource;
        private bool _needUpdateSourceLayout;
        private bool _needUpdateTargetLayout;

        private AnimatedElements<UIElement> SourceAnimatedElements => _sourceAnimatedElements ??= GetAnimatedElements(this.Source);

        private AnimatedElements<UIElement> TargetAnimatedElements => _targetAnimatedElements ??= GetAnimatedElements(this.Target);

        private IKeyFrameAnimationGroupController _currentAnimationGroupController;

        /// <summary>
        /// Gets a value indicating whether the source and target controls are animating.
        /// </summary>
        public bool IsAnimating => _animationCancellationTokenSource is not null && this._currentAnimationGroupController is not null;

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
        public Task StartAsync(CancellationToken token, bool forceUpdateAnimatedElements)
        {
            return this.AnimateControlsAsync(false, token, forceUpdateAnimatedElements);
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
        public Task ReverseAsync(CancellationToken token, bool forceUpdateAnimatedElements)
        {
            return this.AnimateControlsAsync(true, token, forceUpdateAnimatedElements);
        }

        /// <summary>
        /// Stop all animations.
        /// </summary>
        public void Stop()
        {
            if (IsAnimating is false)
            {
                return;
            }

            this._animationCancellationTokenSource?.Cancel();
            this._animationCancellationTokenSource = null;
        }

        /// <summary>
        /// Reset to initial or target state.
        /// </summary>
        /// <param name="toInitialState">Indicates whether to reset to initial state. default value is True, if it is False, it will be reset to target state.</param>
        public void Reset(bool toInitialState = true)
        {
            this.Stop();
            this._currentAnimationGroupController = null;
            this.RestoreState(!toInitialState);
        }
    }
}
