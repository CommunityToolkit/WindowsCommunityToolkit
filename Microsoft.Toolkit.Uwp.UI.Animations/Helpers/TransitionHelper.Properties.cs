// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A animation helper that morphs between two controls.
    /// </summary>
    public sealed partial class TransitionHelper
    {
        private FrameworkElement _source;
        private int _sourceZIndex = -1;
        private FrameworkElement _target;
        private int _targetZIndex = -1;

        /// <summary>
        /// Gets or sets the source control.
        /// </summary>
        public FrameworkElement Source
        {
            get
            {
                return this._source;
            }

            set
            {
                if (this._source == value)
                {
                    return;
                }

                if (this._source is not null)
                {
                    RestoreElements(this.SourceAnimatedElements);
                }

                this._source = value;
                this._sourceZIndex = value is null ? -1 : Canvas.GetZIndex(value);
                this._needUpdateSourceLayout = true;
                this.UpdateSourceAnimatedElements();
            }
        }

        /// <summary>
        /// Gets or sets the target control.
        /// </summary>
        public FrameworkElement Target
        {
            get
            {
                return this._target;
            }

            set
            {
                if (this._target == value)
                {
                    return;
                }

                if (this._target is not null)
                {
                    RestoreElements(this.TargetAnimatedElements);
                }

                this._target = value;
                this._targetZIndex = value is null ? -1 : Canvas.GetZIndex(value);
                this._needUpdateTargetLayout = true;
                this.IsTargetState = false;
                this.UpdateTargetAnimatedElements();
            }
        }

        /// <summary>
        /// Gets or sets transition configurations of UI elements that need to be connected by animation.
        /// </summary>
        public List<TransitionConfig> Configs { get; set; } = new();

        /// <summary>
        /// Gets or sets the default transition configuration.
        /// </summary>
        public TransitionConfig DefaultConfig { get; set; } = new();

        /// <summary>
        /// Gets a value indicating whether the source control has been morphed to the target control.
        /// The default value is false.
        /// </summary>
        public bool IsTargetState { get; private set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the contained area of the source or target control can return true values for hit testing when animating.
        /// The default value is false.
        /// </summary>
        public bool IsHitTestVisibleWhenAnimating { get; set; } = false;

        /// <summary>
        /// Gets or sets the method of changing the visibility of the source control.
        /// The default value is <see cref="VisualStateToggleMethod.ByVisibility"/>.
        /// </summary>
        public VisualStateToggleMethod SourceToggleMethod { get; set; } = VisualStateToggleMethod.ByVisibility;

        /// <summary>
        /// Gets or sets the method of changing the visibility of the target control.
        /// The default value is <see cref="VisualStateToggleMethod.ByVisibility"/>.
        /// </summary>
        public VisualStateToggleMethod TargetToggleMethod { get; set; } = VisualStateToggleMethod.ByVisibility;

        /// <summary>
        /// Gets or sets the duration of the connected animation between two UI elements.
        /// The default value is 600ms.
        /// </summary>
        public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(600);

        /// <summary>
        /// Gets or sets the reverse duration of the connected animation between two UI elements.
        /// The default value is 600ms.
        /// </summary>
        public TimeSpan ReverseDuration { get; set; } = TimeSpan.FromMilliseconds(600);

        /// <summary>
        /// Gets or sets the duration of the show animation for independent or unpaired UI elements.
        /// The default value is 200ms.
        /// </summary>
        public TimeSpan IndependentElementShowDuration { get; set; } = TimeSpan.FromMilliseconds(200);

        /// <summary>
        /// Gets or sets the delay of the show animation for independent or unpaired UI elements.
        /// The default value is 300ms.
        /// </summary>
        public TimeSpan IndependentElementShowDelay { get; set; } = TimeSpan.FromMilliseconds(300);

        /// <summary>
        /// Gets or sets the interval between the show animations for independent or unpaired UI elements.
        /// The default value is 50ms.
        /// </summary>
        public TimeSpan IndependentElementShowInterval { get; set; } = TimeSpan.FromMilliseconds(50);

        /// <summary>
        /// Gets or sets the duration of the hide animation for independent or unpaired UI elements.
        /// The default value is 100ms.
        /// </summary>
        public TimeSpan IndependentElementHideDuration { get; set; } = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// Gets or sets the default translation used by the show or hide animation for independent or unpaired UI elements.
        /// The default value is (0, 20).
        /// </summary>
        public Point DefaultIndependentTranslation { get; set; } = new(0, 20);

        /// <summary>
        /// Gets or sets the easing function type for animation of independent or unpaired UI elements.
        /// The default value is <see cref="EasingType.Default"/>.
        /// </summary>
        public EasingType IndependentElementEasingType { get; set; } = EasingType.Default;

        /// <summary>
        /// Gets or sets the easing function mode for animation of independent or unpaired UI elements.
        /// The default value is <see cref="EasingMode.EaseInOut"/>.
        /// </summary>
        public EasingMode IndependentElementEasingMode { get; set; } = EasingMode.EaseInOut;
    }
}
