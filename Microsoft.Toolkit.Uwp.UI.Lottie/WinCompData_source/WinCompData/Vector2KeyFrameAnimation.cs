// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class Vector2KeyFrameAnimation : KeyFrameAnimation<Vector2>
    {
        internal Vector2KeyFrameAnimation() : base(null) { }
        Vector2KeyFrameAnimation(Vector2KeyFrameAnimation other) : base(other) { }

        public override CompositionObjectType Type => CompositionObjectType.Vector2KeyFrameAnimation;

        internal override CompositionAnimation Clone() => new Vector2KeyFrameAnimation(this);
    }
}
