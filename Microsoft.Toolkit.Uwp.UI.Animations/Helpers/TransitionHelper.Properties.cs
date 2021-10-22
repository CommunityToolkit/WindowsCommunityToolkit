// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A animation helper that morphs between two controls.
    /// </summary>
    public sealed partial class TransitionHelper
    {
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
                this._source = value;
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
                this._target = value;
                this._needUpdateTargetLayout = true;
                this.UpdateTargetAnimatedElements();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the source control has been morphed to the target control.
        /// </summary>
        public bool IsTargetState { get; private set; } = false;

        /// <summary>
        /// Gets or sets the method of changing the visibility of the source control.
        /// </summary>
        public VisualStateToggleMethod SourceToggleMethod { get; set; } = VisualStateToggleMethod.ByVisibility;

        /// <summary>
        /// Gets or sets the method of changing the visibility of the target control.
        /// </summary>
        public VisualStateToggleMethod TargetToggleMethod { get; set; } = VisualStateToggleMethod.ByVisibility;

        /// <summary>
        /// Gets or sets the duration of the connected animation between two UI elements.
        /// </summary>
        public TimeSpan AnimationDuration { get; set; } = TimeSpan.FromMilliseconds(600);

        /// <summary>
        /// Gets or sets the duration of the show animation of ignored UI elements.
        /// </summary>
        public TimeSpan IgnoredElementShowDuration { get; set; } = TimeSpan.FromMilliseconds(200);

        /// <summary>
        /// Gets or sets the delay of the show animation of ignored UI elements.
        /// </summary>
        public TimeSpan IgnoredElementShowDelayDuration { get; set; } = TimeSpan.FromMilliseconds(300);

        /// <summary>
        /// Gets or sets the duration of the interval between the show animations of ignored UI elements.
        /// </summary>
        public TimeSpan IgnoredElementShowStepDuration { get; set; } = TimeSpan.FromMilliseconds(50);

        /// <summary>
        /// Gets or sets the duration of the hide animation of ignored UI elements.
        /// </summary>
        public TimeSpan IgnoredElementHideDuration { get; set; } = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// Gets or sets the translation of the hide animation of ignored UI elements.
        /// </summary>
        public Vector3 IgnoredElementHideTranslation { get; set; } = new Vector3(0, 20, 0);
    }
}
