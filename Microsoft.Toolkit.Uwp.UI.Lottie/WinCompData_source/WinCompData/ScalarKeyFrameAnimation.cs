// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class ScalarKeyFrameAnimation : KeyFrameAnimation<float>
    {     
        internal ScalarKeyFrameAnimation() : base(null) { }
        ScalarKeyFrameAnimation(ScalarKeyFrameAnimation other) : base(other) { }

        public override CompositionObjectType Type => CompositionObjectType.ScalarKeyFrameAnimation;
        internal override CompositionAnimation Clone() => new ScalarKeyFrameAnimation(this);
    }
}
