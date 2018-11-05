// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class LinearEasing : Easing, IEquatable<LinearEasing>
    {
        LinearEasing() { }

        public static LinearEasing Instance { get; } = new LinearEasing();

        public override EasingType Type => EasingType.Linear;

        public override string ToString() => nameof(LinearEasing);

        // All LinearEasings are equivalent.
        public override int GetHashCode() => (int)Type;

        public override bool Equals(object obj) => Equals(obj as LinearEasing);

        // All LinearEasings are equivalent.
        public bool Equals(LinearEasing other) => other != null;
    }
}
