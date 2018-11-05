// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    interface IAnimatableValue<T>
    {
        /// <summary>
        /// The initial value.
        /// </summary>
        T InitialValue { get; }

        /// <summary>
        /// True if the value is animated.
        /// </summary>
        bool IsAnimated { get; }
    }
}
