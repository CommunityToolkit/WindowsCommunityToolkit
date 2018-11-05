// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class AnimationController : CompositionObject
    {
        internal AnimationController(CompositionObject targetObject, string targetProperty)
        {
            TargetObject = targetObject;
            TargetProperty = targetProperty;
        }

        public CompositionObject TargetObject { get; }
        public string TargetProperty { get; }
        public bool IsPaused { get; private set; }
        public void Pause()
        {
            IsPaused = true;
        }

        public override CompositionObjectType Type => CompositionObjectType.AnimationController;
    }
}
