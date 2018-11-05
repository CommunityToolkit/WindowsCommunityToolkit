// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class MergePaths : ShapeLayerContent
    {
        public MergePaths(
            string name,
            string matchName,
            MergeMode mergeMode)
            : base(name, matchName)
        {
            Mode = mergeMode;
        }

        public MergeMode Mode { get; }

        public override ShapeContentType ContentType => ShapeContentType.MergePaths;

        public override LottieObjectType ObjectType => LottieObjectType.MergePaths;
        public enum MergeMode
        {
            Merge,
            Add,
            Subtract,
            Intersect,
            ExcludeIntersections,
        }
    }
}
