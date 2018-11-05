// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Lottie
{
    /// <summary>
    /// Options for controlling how the <see cref="LottieVisualSource"/> processes a Lottie file.
    /// </summary>
    [Flags]
    public enum LottieVisualOptions
    {
        None = 0,

        /// <summary>
        /// Optimizes the translation of the Lottie so as to reduce resource
        /// usage during rendering. Note that this may slow down loading.
        /// </summary>
        Optimize = 1,

        /// <summary>
        /// Sets the <see cref="AnimatedVisualPlayer.Diagnostics"/> property with information
        /// about the Lottie.
        /// </summary>
        IncludeDiagnostics = 2,

        All = IncludeDiagnostics | Optimize,
    }
}
