// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class StepEasingFunction : CompositionEasingFunction
    {
        internal StepEasingFunction(int steps)
        {
            StepCount = steps;
            // TODO - setting the FinalStep here is necessary if it's not set
            //        explicitly, but the real Comp object doesn't seem to do this... why?

            FinalStep = steps;
        }

        public int StepCount { get; set; }
        public bool IsInitialStepSingleFrame { get; set; }
        public int InitialStep { get; set; }
        public int FinalStep { get; set; }
        public bool IsFinalStepSingleFrame { get; set; }

        public override CompositionObjectType Type => CompositionObjectType.StepEasingFunction;
    }
}
