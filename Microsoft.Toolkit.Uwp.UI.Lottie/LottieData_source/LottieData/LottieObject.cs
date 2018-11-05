// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.


namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    abstract class LottieObject
    {
        protected private LottieObject(string name) { Name = name; }

        public string Name { get; }

        public abstract LottieObjectType ObjectType { get; }
    }
}
