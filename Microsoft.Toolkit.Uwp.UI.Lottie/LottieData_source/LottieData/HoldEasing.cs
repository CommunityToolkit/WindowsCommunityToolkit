// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
    /// <summary>
    /// An easing that holds the current value until the key frame time, then
    /// jumps to the key frame value.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class HoldEasing : Easing, IEquatable<HoldEasing>
    {
        HoldEasing() { }

        public static HoldEasing Instance { get; } = new HoldEasing();

        public override EasingType Type => EasingType.Hold;

        // All SetpEeasings are equivalent.
        public override int GetHashCode() => (int)Type;

        public override bool Equals(object obj) => Equals(obj as HoldEasing);

        // All LinearEasings are equivalent.
        public bool Equals(HoldEasing other) => other != null;

        public override string ToString() => nameof(HoldEasing);
    }
}
