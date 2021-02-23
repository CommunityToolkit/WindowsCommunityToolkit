// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// A model containing options for configuring the Surface Dial support through <see cref="TextBoxExtensions"/>.
    /// </summary>
    public sealed class SurfaceDialOptions : DependencyObject
    {
        /// <summary>
        /// Gets an internal cached instance to avoid allocations from <see cref="TextBoxExtensions"/>.
        /// </summary>
        internal static SurfaceDialOptions Default { get; } = new();

        /// <summary>
        /// Gets or sets a value indicating whether new menu items shouldn't be added automatically.
        /// This should be set to <see langword="true"/> if you provide the <see cref="RadialController"/> yourself.
        /// </summary>
        public bool ForceMenuItem { get; set; }

        /// <summary>
        /// Gets or sets the default icon of the menu item that gets added.
        /// A user will most likely not see this.
        /// Defaults to <see cref="RadialControllerMenuKnownIcon.Ruler"/>.
        /// </summary>
        public RadialControllerMenuKnownIcon Icon { get; set; } = RadialControllerMenuKnownIcon.Ruler;

        /// <summary>
        /// Gets or sets the amount the <see cref="TextBox"/> will be modified for each rotation step on the Surface Dial.
        /// This can be any double value.
        /// </summary>
        public double StepValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable the haptic feedback when rotating the dial for the give TextBox.
        /// This is enabled by default.
        /// </summary>
        public bool EnableHapticFeedback { get; set; } = true;

        /// <summary>
        /// Gets or sets the minimum value the <see cref="TextBox"/> can have when modifying it using a Surface Dial.
        /// Default is -100.0.
        /// </summary>
        public double MinValue { get; set; } = -100;

        /// <summary>
        /// Gets or sets the maximum value the <see cref="TextBox"/> can have when modifying it using a Surface Dial.
        /// Default is 100.0.
        /// </summary>
        public double MaxValue { get; set; } = 100;

        /// <summary>
        /// Gets or sets a value indicating whether to automatically try to focus the next focusable element from the Surface Dial enabled <see cref="TextBox"/>.
        /// This is on by default.
        /// </summary>
        public bool EnableTapToNextControl { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to limit the value in the <see cref="TextBox"/> to <see cref="MinValue"/> and <see cref="MaxValue"/>.
        /// </summary>
        public bool EnableMinMaxValue { get; set; }
    }
}
